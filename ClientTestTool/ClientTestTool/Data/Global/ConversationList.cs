///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Xml;
using ClientTestTool.Data.Conversations.Events;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Definitions.Conversation.Base;
using ClientTestTool.Data.Definitions.Devices.Base;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.GUI.Logging;

namespace ClientTestTool.Data.Global
{
  public sealed class ConversationList : ValidatableItem
  {
    #region Singleton

    private static ConversationList mInstance;

    public static ConversationList Instance
    {
      get
      {
        return mInstance ?? (mInstance = new ConversationList());
      }
    }

    static ConversationList()
    {}

    #endregion

    #region Events

    public event EventHandler<ListChangedEventArgs>         OnConversationListChanged;
    public event EventHandler<ConversationElementEventArgs> OnPairAdded;
    public event EventHandler<ConversationElementEventArgs> OnPairValidated;
    public event EventHandler<ElementEventArgs>             OnConversationValidated;
    public event EventHandler                               OnConversationListValidated;

    #endregion

    #region Properties

    public static int Count
    {
      get
      {
        return GetConversations().Count;
      }
    }

    public static bool IsEmpty
    {
      get
      {
        return 0 == Count;
      }
    }

    #endregion

    #region Static Methods

    public static Conversation GetConversation(Unit client, Unit device)
    {
      return Instance.mConversations.FirstOrDefault(item => item.Client.Mac == client.Mac && item.Device.Mac == device.Mac);
    }

    public static void Add(Conversation conversation)
    {
      Instance.mConversations.Add(conversation);
      Instance.Validated = false;
    }

    public static List<Conversation> GetConversations()
    {
      return Instance.mConversations.Where(item => !item.IsIgnored).ToList();
    }

    public static List<Conversation> GetConversations(Func<Conversation, bool> predicate)
    {
      return GetConversations().Where(predicate).ToList();
    }

    public static List<Conversation> GetConversations(NetworkTraceInfo trace)
    {
      return GetConversations(item => item.FoundInTraces.Contains(trace));
    }

    public static void RemoveConnectedConversations(Unit unit)
    {
      foreach (Conversation conversation in Instance.mConversations.Reverse())
        if (Equals(conversation.Client, unit) || !(conversation.Device != unit))
          Instance.mConversations.Remove(conversation);
    }

    public static Conversation Find(Unit client, Unit device)
    {
      return GetConversations().FirstOrDefault(item => Equals(item.Client, client) &&
                                                       Equals(item.Device, device));
    }

    public static bool Contains(Unit client, Unit device)
    {
      return null != Find(client, device);
    }

    public static bool Any()
    {
      return !IsEmpty;
    }

    public static bool Any(Func<Conversation, bool> predicate)
    {
      return mInstance.mConversations.Any(predicate);
    }

    #endregion

    #region Validation

    public override void Validate()
    {
      Validated = false;

      foreach (var conversation in mConversations.ToList())
      {
        ApplicationStatus.SetProgress(0);
        ApplicationStatus.SetStatus(String.Format("Validating conversation: {0}", conversation.Name));
        conversation.Validate();
        ApplicationStatus.SetProgress(100);

        if (conversation.IsEmpty)
          mConversations.Remove(conversation);

        if (null != OnConversationValidated)
          OnConversationValidated(this, new ElementEventArgs(mConversations.IndexOf(conversation)));
      }

      Validated = true;

      if (null != OnConversationListValidated)
        OnConversationListValidated(this, new EventArgs());
    }

    #endregion
    
    #region Helpers

    public static void SaveConversationReport(String filename)
    {
      using (var writer = XmlWriter.Create(filename))
      {
        writer.WriteStartDocument();

        writer.WriteStartElement("ConversationList");
        writer.WriteAttributeString("Count", Count.ToString(CultureInfo.InvariantCulture));

        Instance.mConversations.ToList().ForEach(item => item.WriteXml(writer));

        writer.WriteEndElement();

        writer.WriteEndDocument();
      }
    }

    #endregion  

    private ConversationList()
    {
      mConversations = new BindingList<Conversation>();
      mConversations.ListChanged += Conversations_ListChanged;

      NetworkTraceSet.OnTraceRemoved += NetworkTraceSet_OnTraceRemoved;
    }

    private void Conversations_ListChanged(object sender, ListChangedEventArgs e)
    {
      if (ListChangedType.ItemAdded == e.ListChangedType) //bind event to every new conversation
      {
        mConversations[e.NewIndex].OnPairValidated += (o, args) =>
        {
          if (null != OnPairValidated)
            OnPairValidated(this, new ConversationElementEventArgs(e.NewIndex, args.ElementIndex));
        };

        mConversations[e.NewIndex].OnPairAdded += (o, args) =>
        {
          if (null != OnPairAdded)
            OnPairAdded(this, new ConversationElementEventArgs(e.NewIndex, args.ElementIndex));
        };
      }

      if (null != OnConversationListChanged)
        OnConversationListChanged(this, e);
    }

    private void NetworkTraceSet_OnTraceRemoved(object sender, Events.NetworkTraceSetChangedEventArgs e)
    {
      var affectedConversations = mConversations.Where(item => item.FoundInTraces.Contains(e.NetworkTrace)).ToList();

      foreach (var conversation in affectedConversations)
      {
        conversation.RemoveConnectedMessages(e.NetworkTrace);
        conversation.FoundInTraces.Remove   (e.NetworkTrace);
      }
    }

    private readonly BindingList<Conversation> mConversations;
  }
}
