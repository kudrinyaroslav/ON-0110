///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using ClientTestTool.Data.Conversations.Enums;
using ClientTestTool.Data.Definitions.Conversation;
using ClientTestTool.Data.Extensions;
using ClientTestTool.GUI.Logging;
using ClientTestTool.Tests.Definitions.Data;
using ClientTestTool.Tests.Definitions.Enums;
using ClientTestTool.Tests.Definitions.Events;
using ClientTestTool.Tests.Definitions.Exceptions;
using ClientTestTool.Tests.Definitions.Extensions;
using ClientTestTool.Tests.Definitions.Log.Steps;
using ClientTestTool.Tests.Definitions.Log.Test;
using ClientTestTool.Tests.Engine.Interfaces;

namespace ClientTestTool.Tests.Definitions.Base
{
  /// <summary>
  /// Base class for all Test classes
  /// </summary>
  public abstract class BaseTest : ITest
  {
    #region Events

    public event EventHandler                           OnTestStarted;
    public event EventHandler<ConversationLogEventArgs> OnConversationTested;
    public event EventHandler<TestCompletedEventArgs>   OnTestCompleted;

    #endregion

    /// <summary>
    /// ctor
    /// </summary>
    protected BaseTest()
    {
      TestInfo      = TestInfo.Builder.Build(this);
      Log           = new TestLog();
      AffectedPairs = new HashSet<RequestResponsePair>();
    }

    #region Properties

    public bool IsCompleted
    {
      get;
      private set;
    }

    public TestInfo TestInfo
    {
      get;
      private set;
    }

    public TestLog Log
    {
      get;
      private set;
    }

    public HashSet<RequestResponsePair> AffectedPairs
    {
      get;
      private set;
    }

    #endregion

    #region Test Logic

    /// <summary>
    /// Method to implement in derived classes
    /// </summary>
    /// <param name="conversation"></param>
    protected abstract void ProcessConversation(Conversation conversation);

    /// <summary>
    /// Launches Test
    /// </summary>
    public void Start(IEnumerable<Conversation> conversations)
    {
      Log.Clear();
      AffectedPairs.Clear();

      if (null != OnTestStarted)
        OnTestStarted(this, new EventArgs());

      IsCompleted = false;
      StartTest(conversations);
      IsCompleted = true;

      var result = new TestResult(TestInfo, Log);

      TestInfo.FeatureUnderTest.GetInfo().Status = result.TestStatus.ToFeatureStatus();

      if (null != OnTestCompleted)
        OnTestCompleted(this, new TestCompletedEventArgs(result));
    }

    private void StartTest(IEnumerable<Conversation> conversations)
    {
      foreach (Conversation conversation in conversations)
      {
        mCurrentLog = new ConversationLog(this, conversation);

        try
        {
          ProcessConversation(conversation);
        }
        catch (TestNotSupportedException e)
        {
          mCurrentLog.ErrorMessage = e.Message;

          StepNotDetected(e.Message);

          //if (null != OnTestNotSupported)
          //  OnTestNotSupported(this, new TestNotSupportedEventArgs(e.Message));
        }
        catch (Exception e)
        {
          StepFailed(e);
        }
        finally
        {
          if (null != OnConversationTested)
            OnConversationTested(this, new ConversationLogEventArgs(mCurrentLog));

          Log.Add(mCurrentLog);

          mCurrentLog     = null;
          mCurrentStepNum = 0;
        }
      }
    }

    public void ClearTestResults()
    {
      IsCompleted     = false;
      mCurrentStep    = null;
      mCurrentStepNum = 0;

      Log.Clear();
    }

    #endregion

    #region Step Logic

    /// <summary>
    /// Initializes step
    /// </summary>
    protected void BeginStep(String message, List<RequestResponsePair> affectedPairs, MessageType type = MessageType.Request)
    {
      mCurrentStepNum++;
      mCurrentStep = new StepResult(mCurrentStepNum, message, affectedPairs, type);
    }

    /// <summary>
    /// Finishes step
    /// </summary>
    protected void StepCompleted()
    {
      if (null != mCurrentStep) // if errors found, then EndStep() called => null == mCurrentStep
        mCurrentStep.Status = StepStatus.Passed;
  
      EndStep();
    }

    /// <summary>
    /// Marks current step as failed.
    /// </summary>
    protected void StepFailed(String errorMessage)
    {
      if (null != mCurrentStep)
      {
        mCurrentStep.ErrorMessage = errorMessage;
        mCurrentStep.Status       = StepStatus.Failed;
      }

      EndStep();
    }

    /// <summary>
    /// Marks current step as failed.
    /// </summary>
    private void StepFailed(Exception e)
    {
      if (null != mCurrentStep)
      {
        mCurrentStep.ErrorMessage = String.Format("Exception:{0}", e.Message);
        mCurrentStep.Exception    = e;
        mCurrentStep.Status       = StepStatus.Failed;

        Logger.LogException(String.Format("{0} failed in {1} with an Exception:", mCurrentStep.Name, GetType()), e);
      }

      EndStep();
    }

    /// <summary>
    /// Marks current step as not detected
    /// </summary>
    private void StepNotDetected(String message)
    {
       if (null == mCurrentStep)
        return;

       mCurrentStep.ErrorMessage = message;
       mCurrentStep.Status = StepStatus.NotDetected;

       EndStep();
    }

    /// <summary>
    /// Marks current step as completed.
    /// </summary>
    private void EndStep()
    {
      if (null == mCurrentStep)
        return;

      mCurrentLog.Steps.Add(mCurrentStep);
      mCurrentStep = null;
    }

    private ConversationLog mCurrentLog;
    private StepResult      mCurrentStep;
    private int             mCurrentStepNum;

    #endregion

  }
}
