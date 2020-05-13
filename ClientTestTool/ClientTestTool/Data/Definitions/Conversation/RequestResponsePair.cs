///
/// @Author Matthew Tuusberg
///

using ClientTestTool.Data.Definitions.Conversation.Base;
using ClientTestTool.Data.Definitions.Conversation.Enums;
using ClientTestTool.Data.Definitions.Conversation.Messages.Base;
using ClientTestTool.Data.Definitions.Conversation.Messages.Rtsp;
using ClientTestTool.Data.Definitions.Interfaces;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Enums;
using ClientTestTool.Data.Extensions;

namespace ClientTestTool.Data.Definitions.Conversation
{
  public sealed class RequestResponsePair : ValidatableItem, ITraceItem
  {
    public RequestResponsePair(BaseMessage request, BaseMessage response, NetworkTraceInfo trace, Conversation conversation, ContentType type)
    {
      mResponse        = response;
      mRequest         = request;
      FoundInTrace     = trace;
      Conversation     = conversation;
      ContentType      = type;

      ValidationStatus = ValidationStatus.Pending;
    }

    #region Properties

    public NetworkTraceInfo FoundInTrace
    {
      get;
      private set;
    }

    public Conversation Conversation
    {
      get;
      private set;
    }

    private readonly BaseMessage mRequest;

    public BaseMessage Request
    {
      get
      {
        return mRequest;
      }
    }

    private BaseMessage mResponse;

    public BaseMessage Response
    {
      get
      {
        return mResponse;
      }
    }

    public ContentType ContentType
    {
      get;
      private set;
    }

    #endregion

    public T GetRequest<T>() where T : BaseMessage
    {
      return (T) mRequest;
    }

    public T GetResponse<T>() where T : BaseMessage
    {
      return (T) mResponse;
    }

    public void SetResponse(RtspResponse response)
    {
      mResponse = response;
    }

    public override void Validate()
    {
      mRequest.Validate();
      mResponse.Validate();

      Validated = true;

      ValidationStatus = mRequest.ValidationStatus.Passed() && mResponse.ValidationStatus.Passed()
        ? ValidationStatus.Passed
        : ValidationStatus.Failed;
    }
  }
}
