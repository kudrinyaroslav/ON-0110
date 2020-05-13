using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestTool.Tests.Definitions.Attributes;
using TestTool.Tests.Definitions.Enums;
using TestTool.Tests.Common.CommonUtils;
using TestTool.Proxies.Onvif;
using System.Xml;
using TestTool.Proxies.Event;
using TestTool.Tests.CommonUtils.SoapValidation;
using TestTool.Tests.TestCases.Base;
using TestTool.Tests.Definitions.Exceptions;
using TestTool.Tests.Engine.Base.Definitions;

namespace TestTool.Tests.TestCases.TestSuites
{
    [TestClass]
    partial class AccessControlEventsTestSuite : PacsEventsTestSuite
    {
        public AccessControlEventsTestSuite(TestLaunchParam param)
            : base(param)
        {

        }

        protected override void Release()
        {
            base.Release();
        }

        private const string PATHPROPERTYEVENTS = "Access Control\\Property Events";

        const string ACCESSPOINTTOKENSIMPLEITEM = "AccessPointToken";

        #region additional services 

        #endregion
        
        [Test(Name = "ACCESS CONTROL – ACCESS POINT ENABLED EVENT",
            Path = PATHPROPERTYEVENTS,
            Order = "05.01.01",
            Id = "5-1-1",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, 
                Functionality.TopicFilter, 
                Functionality.AccessPointEnabledEvent  })]
        public void AccessPointEnabledEventTest()
        {
            EndpointReferenceType subscriptionReference = null;
            System.DateTime subscribeStarted = System.DateTime.MaxValue;

            int timeout = 60;

            RunTest(
                () =>
                {
                    // Topic for current test
                    // tns1:AccessPoint/State/Enabled 
                    TopicInfo topicInfo = ConstructTopic(new string[]{"AccessPoint", "State", "Enabled"});

                    //3.	Get complete list of access points from the DUT (see Annex A.1).
                    List<AccessPointInfo> fullAccessPointsList = GetAccessPointInfoList();

                    List<AccessPointInfo> accessPoints = 
                        fullAccessPointsList.Where(A => A.Capabilities != null && A.Capabilities.DisableAccessPoint).ToList();

                    //4.	ONVIF Client will invoke GetEventPropertiesRequest message to retrieve all events supported by the DUT.
                    //5.	Verify the GetEventPropertiesResponse message from the DUT.
                    //6.	Check if there is an event with Topic tns1:AccessControl/AccessPoint/Enabled. If there is no event with such Topic skip other steps, fail the test and go to the next test.
                    // Get topic description from the DUT.                
                    XmlElement topicElement = GetTopicElement(topicInfo);

                    Assert(topicElement != null,
                        string.Format("Topic {0} not supported", topicInfo.GetDescription()),
                        "Check that the event topic is supported");

                    //7.	Check that this event is a Property event (MessageDescription.IsProperty="true").
                    //8.	Check that this event contains Source.SimpleItemDescription item with Name="AccessPointToken" and Type="pt:ReferenceToken".
                    //9.	Check that this event contains Data.SimpleItemDescription item with Name="Enabled" and Type=" xs:boolean".
                    //10.	Check that this event contains Data.SimpleItemDescription item with Name="Reason" and Type=" xs:string".

                    XmlElement messageDescription = topicElement.GetMessageDescription();
                    ValidateAccessPointEnabledTopic(messageDescription, topicInfo);

                    FilterInfo filter = CreateFilter(topicInfo, messageDescription);

                    //11.	ONVIF Client will invoke SubscribeRequest message with tns1:AccessControl/AccessPoint/Enabled Topic as Filter and an InitialTerminationTime of 60s to ensure that the SubscriptionManager is deleted after one minute.
                    //12.	Verify that the DUT sends a SubscribeResponse message.

                    //13.	Verify that DUT sends Notify message(s) 

                    Dictionary<NotificationMessageHolderType, XmlElement> notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();

                    subscriptionReference =
                        ReceiveMessages(filter.Filter,
                        timeout,
                        new Action(() => { }),
                        fullAccessPointsList.Count,
                        notifications,
                        out subscribeStarted);

                    //14.	Verify received Notify messages  (correct value for UTC time, TopicExpression and wsnt:Message).
                    //15.	Verify that TopicExpression is equal to tns1:AccessControl/AccessPoint/Enabled 
                    // for all received Notify messages.
                    //16.	Verify that each notification contains Source.SimpleItem item with Name="AccessPointToken" 
                    // and Value is equal to one of existing Access Point Tokens (e.g. complete list of access points contains Access Point with the same token). Verify that there are Notification messages for each Access Point.
                    //17.	Verify that each notification contains Data.SimpleItem item with Name="Enabled" and 
                    // Value with type is equal to xs:boolean.
                    //18.	Verify that each notification which contains Data.SimpleItem item with Name="Reason" contains 
                    // Value with type is equal to xs:string.
                    //19.	Verify that Notify PropertyOperation="Initialized".

                    ValidateMessages(notifications, topicInfo, OnvifMessage.INITIALIZED, accessPoints, ValidateAccessPointEnabledMessage);

                    Dictionary<string, NotificationMessageHolderType> accessPointsMessages = new Dictionary<string, NotificationMessageHolderType>();
                    ValidateMessagesSet(notifications.Keys, accessPoints, accessPointsMessages);

                    //20.	ONVIF Client will invoke GetAccessPointStateRequest message for each Access Point 
                    // with corresponding tokens.
                    //21.	Verify the GetAccessPointStateResponse messages from the DUT. Verify that Data.SimpleItem 
                    // item with Name="Enabled" from Notification message has the same value with Enabled elements 
                    // from corresponding GetAccessPointStateResponse messages for each AccessPoint.

                    foreach (string accessPointToken in accessPointsMessages.Keys)
                    {
                        AccessPointState state = GetAccessPointState(accessPointToken);

                        string expectedState = state.Enabled.ToString().ToLower();

                        XmlElement messageElement = accessPointsMessages[accessPointToken].Message;

                        // Simple Items must be OK by that moment
                        Dictionary<string, string> dataSimpleItems = messageElement.GetMessageDataSimpleItems();

                        string notificationState = dataSimpleItems["State"];

                        Assert(expectedState == notificationState,
                            string.Format("State is different ({0} in GetAccessPointStateResponse, {1} in Notification)", expectedState, notificationState),
                            "Check that state is the same in Notification and in GetAccessPointStateResponse");
                    }
                }, 
                () => 
                {
                    ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                });

        }

        [Test(Name = "ACCESS CONTROL – ACCESS POINT ENABLED EVENT STATE CHANGE",
            Path = PATHPROPERTYEVENTS,
            Order = "05.01.02",
            Id = "5-1-2",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, 
                Functionality.TopicFilter, Functionality.AccessPointEnabledEvent})]
        public void AccessPointEnabledStateChangeEventTest()
        {
            List<EndpointReferenceType> subscriptionReferences = new List<EndpointReferenceType>();
            List<System.DateTime> allSubscribeStarted = new List<System.DateTime>();

            int timeout = 60;

            RunTest(
                () =>
                {
                    // Topic for current test
                    // tns1:AccessPoint/State/Enabled 
                    TopicInfo topicInfo = ConstructTopic(new string[] { "AccessPoint", "State", "Enabled" });

                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(topicInfo);

                    //3.	Get complete list of access points from the DUT (see Annex A.1).
                    //4.	If Access Point with Token1 (Token1 is the first AccessPointInfo.token from the 
                    // complete list of access points at step 3) has AccessPointInfo.Capabilities.DisableAccessPoint 
                    // equal to false, then skip steps 7-18 and go to the step 19.

                    List<AccessPointInfo> fullAccessPointsList = GetAccessPointInfoList();

                    List<AccessPointInfo> accessPoints = fullAccessPointsList.Where(AP => AP.Capabilities.DisableAccessPoint).ToList();

                    foreach (AccessPointInfo info in accessPoints)
                    {
                        string accessPointToken = info.token;

                        //5.	ONVIF Client will invoke GetAccessPointStateRequest message (TokenList.Token = Token1, 
                        // where Token1 is the first token from the complete list of access points at step 3) to 
                        // retrieve Access Point state for specified token from the DUT.
                        //6.	Verify the GetAccessPointStateResponse message from the DUT.
                        AccessPointState state = GetAccessPointState(accessPointToken);
                                            
                        //7.	ONVIF Client will invoke SubscribeRequest message with tns1:AccessControl/AccessPoint/Enabled 
                        // Topic as Filter and an InitialTerminationTime of 60s to ensure that the SubscriptionManager is 
                        // deleted after one minute.
                        //8.	Verify that the DUT sends a SubscribeResponse message.
                       
                        
                        
                        //9.	If Access Point with Token1 (Token1 is the first AccessPointInfo.token from the complete 
                        // list of access points at step 3) has AccessPointState.Enabled equal to true, then skip steps 
                        // 10-11 and go to the step 12.
                        //10.	ONVIF Client will invoke EnableAccessPointRequest message (Token = “Token1”, where Token1 
                        // is the first AccessPointInfo.token from the complete list of access points at step 3) to try 
                        // enabling access point.
                        //11.	Verify the EnableAccessPointResponse message from the DUT. Go to the step 13.
                        //12.	ONVIF Client will invoke DisableAccessPointRequest message (Token = “Token1”, where Token1 
                        // is the first AccessPointInfo.token from the complete list of access points at step 3) to try 
                        // disabling access point.
                        //13.	Verify the DisableAccessPointResponse message from the DUT.

                        Action eventInitiationAction = null;
                        if (state.Enabled)
                        {
                            eventInitiationAction = new Action(() => { DisableAccessPoint(accessPointToken); });
                        }
                        else
                        {
                            eventInitiationAction = new Action(() => { EnableAccessPoint(accessPointToken); });
                        }
                        
                        //14.	Verify that DUT sends Notify message.
                        //15.	Verify received Notify messages (correct value for UTC time, TopicExpression and 
                        // wsnt:Message).
                        //16.	Verify that TopicExpression is equal to tns1:AccessControl/AccessPoint/Enabled for all 
                        // received Notify messages.
                        //17.	Verify that notification contains Source.SimpleItem item with Name="AccessPointToken" and 
                        // Value= “Token1” (e.g. complete list of access points contains Access Point with the same token).
                        Dictionary<NotificationMessageHolderType, XmlElement> notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();

                        System.DateTime subscribeStarted = System.DateTime.MaxValue;
                        int cnt = subscriptionReferences.Count;
                        subscriptionReferences.Add(null);
                        allSubscribeStarted.Add(subscribeStarted);
                        EndpointReferenceType subscriptionReference =
                            ReceiveMessages(filter,
                            timeout,
                            eventInitiationAction,
                            1,
                            (n) => CheckMessagePropertyOperation(n, OnvifMessage.CHANGED),
                            notifications,
                            out subscribeStarted);

                        subscriptionReferences[cnt] = subscriptionReference;
                        allSubscribeStarted[cnt] = subscribeStarted;

                        ValidateMessages(notifications, topicInfo, info.token, ValidateAccessPointEnabledMessage);

                        //18.	Verify that notification contains Data.SimpleItem item with Name="Enabled" and Value with 
                        // type is equal to xs:boolean and with value equal to current state of  Access Point.
                        
                        // We ignore messages with property operation different from "Changed"
                        // And the only message expected in this case should be message for "our" AccessPoint
                        foreach (NotificationMessageHolderType message in notifications.Keys)
                        {
                            XmlElement messageElement = message.Message;

                            string expectedState = (!state.Enabled).ToString().ToLower();

                            XmlElement dataElement = messageElement.GetMessageData();
                            // SimpleItems must be OK by that moment
                            Dictionary<string, string> dataSimpleItems = dataElement.GetMessageDataSimpleItems();

                            string notificationState = dataSimpleItems["State"];

                            Assert(expectedState == notificationState,
                                string.Format("State is different ({0} in GetAccessPointStateResponse, {1} in Notification)", expectedState, notificationState),
                                "Check that state is the same in Notification and in GetAccessPointStateResponse");
                        }
                    }
                                        
                    //19.	Repeat steps 4-18 for all other tokens from complete list of access points at step 3.

                },
                () =>
                {
                    for (int i = 0; i < subscriptionReferences.Count; i++)
                    {
                        System.DateTime subscribeStarted = allSubscribeStarted[i];
                        EndpointReferenceType subscriptionReference = subscriptionReferences[i];
                        ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                    }
                });
        }
        
        [Test(Name = "ACCESS CONTROL – ACCESS POINT TAMPERING EVENT",
            Path = PATHPROPERTYEVENTS,
            Order = "05.01.03",
            Id = "5-1-3",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, 
                Functionality.TopicFilter, Functionality.AccessPointTamperingEvent })]
        public void AccessPointTamperingTest()
        {
            EndpointReferenceType subscriptionReference = null;
            System.DateTime subscribeStarted = System.DateTime.MaxValue;

            int timeout = 60;

            RunTest(
                () =>
                {

                    //3.	Get complete list of access points from the DUT (see Annex A.1).                    
                    List<AccessPointInfo> fullAccessPointsList = GetAccessPointInfoList();
                    
                    //4.	Check that there is at least one Access Point with Capabilities.Tamper = “true”. 
                    // Otherwise skip other steps and go to the next test.

                    List<AccessPointInfo> accessPointsList = 
                        fullAccessPointsList.Where(A => A.Capabilities != null && A.Capabilities.TamperSpecified && A.Capabilities.Tamper).ToList();

                    if (accessPointsList.Count == 0)
                    {
                        LogTestEvent("No AccessPoints with required Capabilities found, exit the test." + Environment.NewLine);
                        return;
                    }
                    
                    // Topic for current test
                    // tns1:AccessPoint/State/Tampering  
                    TopicInfo topicInfo = ConstructTopic(new string[] { "AccessPoint", "State", "Tampering" });
                    
                    //5.	ONVIF Client will invoke GetEventPropertiesRequest message to retrieve all events supported by the DUT.
                    //6.	Verify the GetEventPropertiesResponse message from the DUT.
                    //7.	Check if there is an event with Topic tns1:AccessControl/AccessPoint/Tampering. If there is no event with such Topic skip other steps, fail the test and go to the next test.
                    XmlElement topicElement = GetTopicElement(topicInfo);

                    Assert(topicElement != null,
                        string.Format("Topic {0} not supported", topicInfo.GetDescription()),
                        "Check that the event topic is supported");

                    //8.	Check that this event is a Property event (MessageDescription.IsProperty="true").
                    //9.	Check that this event contains Source.SimpleItemDescription item with 
                    // Name="AccessPointToken" and Type="pt:ReferenceToken".
                    //10.	Check that this event contains Data.SimpleItemDescription item with Name="Active" 
                    // and Type=" xs:boolean".
                    //11.	Check that this event contains Data.SimpleItemDescription item with Name="Reason" 
                    // and Type=" xs:string".

                    XmlElement messageDescription = topicElement.GetMessageDescription();
                    ValidateAccessPointTamperingTopic(messageDescription, topicInfo);

                    //12.	ONVIF Client will invoke SubscribeRequest message with tns1:AccessControl/AccessPoint/Tampering Topic as Filter and an InitialTerminationTime of 60s to ensure that the SubscriptionManager is deleted after one minute.
                    //13.	Verify that the DUT sends a SubscribeResponse message.
                    //14.	Verify that DUT sends Notify message(s)
                    
                    FilterInfo filter = CreateFilter(topicInfo, messageDescription);

                    Dictionary<NotificationMessageHolderType, XmlElement> notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();

                    subscriptionReference =
                        ReceiveMessages(filter.Filter,
                        timeout, 
                        new Action(() => { }),
                        fullAccessPointsList.Count, 
                        notifications, 
                        out subscribeStarted);
                                        
                    //15.	Verify received Notify messages (correct value for UTC time, TopicExpression and 
                    // wsnt:Message).
                    //16.	Verify that TopicExpression is equal to tns1:AccessControl/AccessPoint/Tampering 
                    // for all received Notify messages.
                    //17.	Verify that each notification contains Source.SimpleItem item with Name="AccessPointToken" 
                    // and Value is equal to one of existing Access Point Tokens with Capabilities.Tamper = “true” 
                    // (e.g. complete list of access points contains Access Point with the same token). Verify that 
                    // there are Notification messages for each Access Point with Capabilities.Tamper = “true”. 
                    // Verify that there are no Notification messages for each Access Point with Capabilities.Tamper 
                    // = “false”.
                    //18.	Verify that each notification contains Data.SimpleItem item with Name="Active" and Value 
                    // with type is equal to xs:boolean.
                    //19.	Verify that each notification which contains Data.SimpleItem item with Name="Reason" 
                    // contains Value with type is equal to xs:boolean.
                    //20.	Verify that Notify PropertyOperation="Initialized".

                    ValidateMessages(notifications, 
                        topicInfo, 
                        OnvifMessage.INITIALIZED, 
                        accessPointsList,
                        ValidateAccessPointTamperingMessage);

                    // check that there is a notification for each access point
                    Dictionary<string, NotificationMessageHolderType> accessPointsMessages = new Dictionary<string, NotificationMessageHolderType>();
                    ValidateMessagesSet(notifications.Keys, accessPointsList, accessPointsMessages);
                    
                },
                () =>
                {
                    ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                });
        }

        [Test(Name = "ACCESS CONTROL – ACCESS POINT TAMPERING EVENT STATE CHANGE",
            Path = PATHPROPERTYEVENTS,
            Order = "05.01.04",
            Id = "5-1-4",
            Category = Category.ACCESSCONTROL,
            Version = 2.1,
            ExecutionOrder = TestExecutionOrder.First,
            RequiredFeatures = new Feature[] { Feature.AccessControlService },
            RequirementLevel = RequirementLevel.Must,
            FunctionalityUnderTest = new Functionality[] { Functionality.Notify, 
                Functionality.TopicFilter, Functionality.AccessPointTamperingEvent })]
        public void AccessPointTamperingStateChangeTest()
        {
            List<EndpointReferenceType> subscriptionReferences = new List<EndpointReferenceType>();
            List<System.DateTime> allSubscribeStarted = new List<System.DateTime>();

            int timeout = 60;

            RunTest(
                () =>
                {

                    //3.	Get complete list of access points from the DUT (see Annex A.1).                    
                    List<AccessPointInfo> fullAccessPointsList = GetAccessPointInfoList();

                    //4.	Check that there is at least one Access Point with Capabilities.Tamper = “true”. 
                    // Otherwise skip other steps and go to the next test.

                    List<AccessPointInfo> accessPointsList =
                        fullAccessPointsList.Where(A => A.Capabilities != null && A.Capabilities.TamperSpecified && A.Capabilities.Tamper).ToList();

                    if (accessPointsList.Count == 0)
                    {
                        LogTestEvent("No AccessPoints with required Capabilities found, exit the test." + Environment.NewLine);
                        return;
                    }

                    // Topic for current test
                    // tns1:AccessPoint/State/Tampering  
                    TopicInfo topicInfo = ConstructTopic(new string[] { "AccessPoint", "State", "Tampering" });

                    TestTool.Proxies.Event.FilterType filter = CreateSubscriptionFilter(topicInfo);

                    foreach (AccessPointInfo info in accessPointsList)
                    {
                        string accessPointToken = info.token;
                        
                        //5.	If Access Point with Token1 (Token1 is the first AccessPointInfo.token from the 
                        // complete list of access points at step 3) has AccessPointInfo.Capabilities.Tamper equal 
                        // to false, then skip steps 6-13 and go to the step 14.
                        //6.	ONVIF Client will invoke SubscribeRequest message with 
                        // tns1:AccessControl/AccessPoint/Tampering Topic as Filter and an InitialTerminationTime 
                        // of 60s to ensure that the SubscriptionManager is deleted after one minute.
                        //7.	Verify that the DUT sends a SubscribeResponse message.
                        //8.	Test Operator will invoke change of Tampering property.
                        //9.	Verify that DUT sends Notify message.
                        //10.	Verify received Notify messages (correct value for UTC time, TopicExpression and 
                        // wsnt:Message).
                        
                        Dictionary<NotificationMessageHolderType, XmlElement> notifications = new Dictionary<NotificationMessageHolderType, XmlElement>();
                        System.DateTime subscribeStarted = System.DateTime.MaxValue;
                        int cnt = subscriptionReferences.Count;
                        subscriptionReferences.Add(null);
                        allSubscribeStarted.Add(subscribeStarted);

                        EndpointReferenceType subscriptionReference = null;
                        try
                        {
                            subscriptionReference = ReceiveMessages(filter,
                                timeout,
                                new Action(() =>
                                {
                                    _operator.ShowMessage("tns1:AccessPoint/State/Tampering event is expected!");
                                }),
                                1,
                                (n) => CheckMessagePropertyOperation(n, OnvifMessage.CHANGED),
                                notifications,
                                out subscribeStarted);
                        }
                        finally
                        { 
                            _operator.HideMessage();
                        }
                        subscriptionReferences[cnt] = subscriptionReference;
                        allSubscribeStarted[cnt] = subscribeStarted;
                      
                        
                        //11.	Verify that TopicExpression is equal to tns1:AccessControl/AccessPoint/Tampering 
                        // for all received Notify messages.
                        //12.	Verify that notification contains Source.SimpleItem item with Name="AccessPointToken" 
                        // and Value= “Token1” (e.g. complete list of access points contains Access Point with the 
                        // same token).
                        //13.	Verify that notification contains Data.SimpleItem item with Name="Active" and Value 
                        // with type is equal to xs:boolean.
                        ValidateMessages(notifications,
                            topicInfo,
                            OnvifMessage.CHANGED,
                            accessPointsList,
                            ValidateAccessPointTamperingMessage);

                        Dictionary<string, NotificationMessageHolderType> accessPointsMessages = new Dictionary<string, NotificationMessageHolderType>();
                        ValidateMessagesSet(notifications.Keys, accessPointsList, accessPointsMessages);

                        //14.	Repeat steps 5-13 for all other tokens from complete list of access points at step 3.
                    }                 
                },
                () =>
                {
                    _operator.HideMessage();
                    for (int i = 0; i < subscriptionReferences.Count; i++)
                    {
                        System.DateTime subscribeStarted = allSubscribeStarted[i];
                        EndpointReferenceType subscriptionReference = subscriptionReferences[i];
                        ReleaseSubscription(subscribeStarted, subscriptionReference, timeout);
                    }
                });
        }


        #region Steps
        
        protected List<AccessPointInfo> GetAccessPointInfoList()
        {
            PACSPortClient client = PACSPortClient;

            PACS.GetListMethod<AccessPointInfo> getList =
                new PACS.GetListMethod<AccessPointInfo>(
                    (int? limit, string offset, out AccessPointInfo[] list) =>
                    {
                        string newOffset = null;
                        AccessPointInfo[] infos = null;
                        RunStep(() => { newOffset = client.GetAccessPointInfoList(limit, offset, out infos); }, "Get AccessPointInfo list");
                        list = infos;
                        return newOffset;

                    });

            List<AccessPointInfo> fullList = PACS.Extensions.GetFullList(getList, null, "AccessPointInfo", Assert);

            Assert(fullList.Count > 0,
                "No AccessPointInfos returned",
                "Check that the list of AccessPointInfos is not empty");

            return fullList;
        }
        
        protected AccessPointState GetAccessPointState(string token)
        {
            PACSPortClient client = PACSPortClient;

            AccessPointState state = null;
            RunStep(
                () => { state = client.GetAccessPointState(token); },
                string.Format("Get AccessPoint state [token='{0}']", token));
            DoRequestDelay();
            return state;
        }

        protected void EnableAccessPoint(string token)
        {
            PACSPortClient client = PACSPortClient;

            RunStep(
                () => { client.EnableAccessPoint(token); },
                string.Format("Enable AccessPoint [token='{0}']", token));
            DoRequestDelay();
        }

        protected void DisableAccessPoint(string token)
        {
            PACSPortClient client = PACSPortClient;

            RunStep(
                () => { client.DisableAccessPoint(token); },
                string.Format("Disable AccessPoint [token='{0}']", token));
            DoRequestDelay();
        }


        #endregion

        #region topic validation

        /// <summary>
        /// Validates Door topic source (must have "AccessPointToken" name and be of the pt:ReferenceToken type)
        /// </summary>
        /// <param name="messageDescription">Message description</param>
        /// <param name="topicInfo">Topic information</param>
        /// <param name="logger">Logger to add error description, if any</param>
        /// <returns></returns>
        bool ValidateAccessPointEventTopicSource(XmlElement messageDescription, 
            TopicInfo topicInfo, StringBuilder logger)
        {
            return ValidatePacsEventTopicSource(messageDescription, topicInfo, logger, ACCESSPOINTTOKENSIMPLEITEM);
        }

        void ValidateAccessPointTopic(XmlElement messageDescription, 
            TopicInfo topicInfo, 
            MessageDescription messageInfo)
        {
            // checks MessageDescription and IsProperty
            ValidateTopic(messageDescription, topicInfo, messageInfo.IsProperty);

            {
                bool ok = true;
                StringBuilder logger = new StringBuilder();

                // check MessageDescription
                bool localOk = ValidateAccessPointEventTopicSource(messageDescription, topicInfo, logger);
                ok = ok && localOk;

                if (messageInfo.DataSimpleItems.Count > 0)
                {
                    XmlElement dataElement = messageDescription.GetMessageData();
                    if (dataElement == null)
                    {
                        ok = false;
                        logger.AppendLine("Message Data element is missing");
                    }
                    else
                    {
                        string err;
                        bool success;

                        Dictionary<string, XmlElement> dataSimpleItems = messageDescription.GetMessageDataSimpleItemDescriptions(out success, out err);

                        if (success)
                        {
                            foreach (string itemName in messageInfo.DataSimpleItems.Keys)
                            {
                                SimpleItemDescription itemType = messageInfo.DataSimpleItems[itemName];
                                localOk = ValidateSimpleItemDescription(dataSimpleItems, "Data",
                                   itemName, itemType.Type, itemType.Namespace, logger);
                                ok = ok && localOk;
                            }
                        }
                        else
                        {
                            ok = false;
                            logger.AppendLine("Data element is incorrect: " + err);
                        }
                    }
                }
                Assert(ok, logger.ToStringTrimNewLine(), "Check that Topic is correct");
            }
        }

        /// <summary>
        /// Validates tns1:AccessControl/AccessPoint/Enabled topic description
        /// </summary>
        /// <param name="messageDescription">Message description</param>
        /// <param name="topicInfo">Topic information (for logging purposes)</param>
        void ValidateAccessPointEnabledTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = true;
            messageInfo.AddSimpleItem("State", "boolean", XSNAMESPACE);
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE);

            ValidateAccessPointTopic(messageDescription, topicInfo, messageInfo);
        }

        /// <summary>
        /// Validates tns1:AccessControl/AccessPoint/Tampering topic description
        /// </summary>
        /// <param name="messageDescription">Message description</param>
        /// <param name="topicInfo">Topic information (for logging purposes)</param>
        /// <remarks>tns1:AccessControl/AccessPoint/Enabled and tns1:AccessControl/AccessPoint/Tampering 
        /// have the same structure of message (two simple items). If this is not changed, 
        /// methods can be replaced with one</remarks>
        void ValidateAccessPointTamperingTopic(XmlElement messageDescription, TopicInfo topicInfo)
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = true;
            messageInfo.AddSimpleItem("State", "boolean", XSNAMESPACE);
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE);

            ValidateAccessPointTopic(messageDescription, topicInfo, messageInfo);
        }
        
        #endregion
        
        #region Message validation

        bool ValidateAccessPointMessage(NotificationMessageHolderType notification,
            MessageCheckSettings settings,
            StringBuilder logger,
            MessageDescription messageInfo)
        {
            XmlElement messageElement = notification.Message;
            XmlElement messageRawElement = settings.RawMessageElements[notification];
            TopicInfo topicInfo = settings.ExpectedTopic;
            XmlNamespaceManager manager = settings.NamespaceManager;

            // Init
            StringBuilder dump = new StringBuilder();
            bool ok = true;

            ok = ValidateMessageCommonElements(
                notification, messageRawElement, topicInfo, 
                settings.ExpectedPropertyOperation, manager, dump);

            if (messageElement != null)
            {
                // check message source and data 

                // source
                bool localOk = ValidateAccessPointEventSource(messageElement, manager, settings.Data, dump);
                ok = ok && localOk;

                XmlElement messageInnerElement = messageRawElement.GetMessageContentElement();
                XmlElement dataElement = messageInnerElement.GetMessageData();
                localOk = ValidateMessageDataSimpleItems(dataElement, messageInfo, dump);
                ok = ok && localOk;
            }

            if (!ok)
            {
                logger.Append(dump.ToString());
            }
            return ok;
        }
        
        bool ValidateAccessPointEventSource(XmlElement messageElement,
            XmlNamespaceManager manager,
            object data,
            StringBuilder logger)
        {
            bool ok = true;

            List<AccessPointInfo> infos = data as List<AccessPointInfo>;
            string token = data as string;
            EntityListInfo<AccessPointInfo> entityInfo = data as EntityListInfo<AccessPointInfo>;
            
            XmlElement sourceElement = messageElement.GetMessageSource();
            if (sourceElement == null)
            {
                logger.AppendLine("   Message Source element is missing");
                ok = false;
            }
            else
            {
                bool success = false;
                string err;

                Dictionary<string, string> sourceSimpleItems = messageElement.GetMessageSourceSimpleItems(out success, out err);
                if (!success)
                {
                    ok = false;
                    logger.AppendLine("   " + err);
                }
                else
                {
                    if (sourceSimpleItems.ContainsKey(ACCESSPOINTTOKENSIMPLEITEM))
                    {
                        string value = sourceSimpleItems[ACCESSPOINTTOKENSIMPLEITEM];
                        // check value
                        StringBuilder error = new StringBuilder();

                        if (infos != null)
                        {
                            AccessPointInfo found = infos.Where(I => I.token == value).FirstOrDefault();
                            if (found == null)
                            {
                                ok = false;
                                logger.Append(string.Format("   AccessPoint with token '{0}' not found", value));
                            }
                        }
                        else if (entityInfo != null)
                        {
                            AccessPointInfo found = entityInfo.FullList.Where(I => I.token == value).FirstOrDefault();
                            if (found == null)
                            {
                                ok = false;
                                logger.Append(string.Format("   AccessPoint with token '{0}' not found", value));
                            }
                            else
                            {
                                found = entityInfo.FilteredList.Where(I => I.token == value).FirstOrDefault();
                                if (found == null)
                                {
                                    ok = false;
                                    logger.Append(string.Format("   AccessPoint with token '{0}' does not have required capabilities", value));
                                }
                            }
                        }
                        else
                        {
                            if (value != token)
                            {
                                ok = false;
                                logger.Append(string.Format("   Token is incorrect. Expected '{0}', actual '{1}'", token, value));
                            }
                        }
                    }
                    else
                    {
                        logger.AppendLine("   'AccessPointToken' SimpleItem is missing in Source");
                        ok = false;
                    }
                }
            }

            return ok;
        }               

        


        bool ValidateAccessPointEnabledMessage(NotificationMessageHolderType notification,
            MessageCheckSettings settings,
            StringBuilder logger)
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = true;
            messageInfo.AddSimpleItem("State", "boolean", XSNAMESPACE);
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE);

            return ValidateAccessPointMessage(notification, settings, logger, messageInfo);
        }

        bool ValidateAccessPointTamperingMessage(NotificationMessageHolderType notification,
            MessageCheckSettings settings,
            StringBuilder logger)
        {
            MessageDescription messageInfo = new MessageDescription();
            messageInfo.IsProperty = true;
            messageInfo.AddSimpleItem("State", "boolean", XSNAMESPACE);
            messageInfo.AddSimpleItem("Reason", "string", XSNAMESPACE);

            return ValidateAccessPointMessage(notification, settings, logger, messageInfo);
        }
        
        #endregion


        #region Messages set validation

        void ValidateMessagesSet(IEnumerable<NotificationMessageHolderType> messages, 
            IEnumerable<AccessPointInfo> accessPoints, 
            Dictionary<string, NotificationMessageHolderType> accesssPointsMessages)
        {
            ValidateMessagesSet(messages, 
                accessPoints, 
                A => A.token, 
                "AccessPoint", 
                ACCESSPOINTTOKENSIMPLEITEM, 
                accesssPointsMessages);
        }
        
        #endregion
    }
}
