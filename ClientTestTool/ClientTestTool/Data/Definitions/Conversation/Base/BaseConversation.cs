///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Conversations.Events;
using ClientTestTool.Data.Definitions.Conversation.Messages.Rtsp;
using ClientTestTool.Data.Definitions.Devices.Base;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Enums;
using ClientTestTool.Data.Global;
using ClientTestTool.GUI.Logging;

namespace ClientTestTool.Data.Definitions.Conversation.Base
{
  public abstract class BaseConversation : ValidatableItem
  {
    public event EventHandler<ElementEventArgs> OnPairAdded;

    /// <summary>
    /// ctor
    /// </summary>
    protected BaseConversation(Unit client, Unit device)
    {
      mClient              = client;
      mDevice              = device;
      mRequestResponseList = new List<RequestResponsePair>();
      ValidationStatus     = ValidationStatus.Pending;
    }

    #region Events

    public event EventHandler<ElementEventArgs> OnPairValidated;

    #endregion

    public RequestResponsePair this[int i]
    {
      get
      {
        if (i < 0 || i > mRequestResponseList.Count - 1)
          throw new ArgumentOutOfRangeException("i");

        return mRequestResponseList[i];
      }
    }

    public bool ContainsFrame(Frame frame)
    {
      MessageType? messageType;
      return ContainsFrame(frame, out messageType);
    }

    public bool ContainsFrame(Frame frame, out MessageType? messageType)
    {
      if (null == frame)
        throw new ArgumentNullException("frame");

      if ((mClient.Mac == frame.SourceMac && mDevice.Mac == frame.DestinationMac) ||
          (mClient.Ip  == frame.SourceIp  && mDevice.Ip  == frame.DestinationIp))
      {
        messageType = MessageType.Request;
        return mRequestResponseList.Any(pair => pair.Request.FrameNumber == frame.Number);
      }

      if ((mClient.Mac == frame.DestinationMac && mDevice.Mac == frame.SourceMac) ||
          (mClient.Ip  == frame.DestinationIp  && mDevice.Ip  == frame.SourceIp))
      {
        messageType = MessageType.Response;
        return mRequestResponseList.Any(pair => null != pair.Response && pair.Response.FrameNumber == frame.Number);
      }

      messageType = null;
      return false;
    }

    public override void Validate()
    {
      ValidationStatus = ValidationStatus.Pending;

      var notValidatedList = mRequestResponseList.Where(item => ValidationStatus.Pending == item.ValidationStatus).ToList();

      for (int i = 0; i < notValidatedList.Count; ++i)
      {
        ApplicationStatus.SetProgress(i * 100 / notValidatedList.Count);

        notValidatedList[i].Validate();

        if (null != OnPairValidated)
        {
          int exactIndex = mRequestResponseList.IndexOf(notValidatedList[i]);
          OnPairValidated(this, new ElementEventArgs(exactIndex));
        }
      }

      RemoveDeviceSpecificCommands();

      ValidationStatus = (mRequestResponseList.All(item => ValidationStatus.Passed == item.ValidationStatus))
                           ? ValidationStatus.Passed
                           : ValidationStatus.Failed;
      Validated = true;
    }

    public void Add(RequestResponsePair pair)
    {
      if (null == pair)
        throw new ArgumentNullException();

      if (null == pair.Response)
        pair.SetResponse(new RtspResponse(pair.Conversation));

      mRequestResponseList.Add(pair);

      if (null != OnPairAdded)
        OnPairAdded(this, new ElementEventArgs(mRequestResponseList.Count - 1));
    }

    private void RemoveDeviceSpecificCommands()
    {
      if (UnitSet.GetDevices().Contains(mClient))
        mRequestResponseList.RemoveAll(item => mDeviceSpecificCommands.Contains(item.Request.GetDetails()));
    }

    protected readonly List<RequestResponsePair> mRequestResponseList;
    protected readonly Unit                      mClient;
    protected readonly Unit                      mDevice;

    private readonly String[] mDeviceSpecificCommands =
    {
      "Notify",
      "Allow",
      "Bye",
      "ProbeMatch"
    };
  }
}
