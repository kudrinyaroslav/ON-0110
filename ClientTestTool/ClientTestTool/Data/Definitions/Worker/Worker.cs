///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Diagnostics;
using ClientTestTool.Data.Definitions.Worker.Base;
using ClientTestTool.Data.Enums;
using ClientTestTool.Data.Global;
using ClientTestTool.GUI.Logging;

namespace ClientTestTool.Data.Definitions.Worker
{
  public abstract class Worker : BaseWorker
  {
    protected Worker(ApplicationState state) : base(state)
    {
    }

    protected void Run(Action action)
    {
      var timer = new Stopwatch();
      timer.Start();

      WorkStarted();
      action();
      WorkCompleted();

      timer.Stop();
      Logger.ShowErrorList();

      Logger.WriteLine(String.Format("Ellapsed milliseconds: {0}", timer.ElapsedMilliseconds));
    }

    private void WorkStarted()
    {
      StateManager.SetState(mRunningState);
      Logger.WriteLine(String.Format("Worker:{0} has started working", GetType()));

      if (null != OnWorkStarted)
        OnWorkStarted(this, new EventArgs());
    }

    private void WorkCompleted()
    {
      StateManager.SetState(ApplicationState.Idle);
      Logger.WriteLine(String.Format("Worker:{0} has finished", GetType()));

      if (null != OnWorkCompleted)
        OnWorkCompleted(this, new EventArgs());
    }

    #region Events

    public event EventHandler OnWorkStarted;
    public event EventHandler OnProgressChanged;
    public event EventHandler OnWorkCompleted;

    #endregion

    #region Status

    #region Progress

    private int mProgress;

    public int Progress
    {
      get
      {
        return mProgress;
      }
      set
      {
        if (value < 0 || value > 100)
          throw new ArgumentOutOfRangeException("value");

        if (mProgress == value)
          return;

        mProgress = value;

        if (null != OnProgressChanged)
          OnProgressChanged(this, new EventArgs());
      }
    }

    #endregion

    protected virtual void SetStatusMessage(String status)
    {
      ApplicationStatus.SetStatus(status);
    }

    #endregion
  }
}
