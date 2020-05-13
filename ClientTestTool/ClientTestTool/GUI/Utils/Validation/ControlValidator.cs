///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace ClientTestTool.GUI.Utils.Validation
{
  public static class ControlValidator
  {
    public static bool HasValidationErrors(this Control.ControlCollection controls)
    {
      bool hasError = false;

      foreach (Control control in controls)
      {
        bool validControl = IsValid(control);
        // If it's not valid then set the flag and keep going.  We want to get through all
        // the validators so they will display on the screen if errorProviders were used.
        if (!validControl)
          hasError = true;

        // If its a container control then it may have children that need to be checked
        if (control.HasChildren)
          if (HasValidationErrors(control.Controls))
            hasError = true;
      }

      return hasError;
    }

    private static bool IsValid(this Control eventSource)
    {
      const String name = "EventValidating";

      Type targetType = eventSource.GetType();

      do
      {
        FieldInfo[] fields = targetType.GetFields(BindingFlags.Static   |
                                                  BindingFlags.Instance |
                                                  BindingFlags.NonPublic);

        foreach (FieldInfo field in fields)
        {
          if (field.Name == name)
          {
            var eventHandlers = ((EventHandlerList)(eventSource.GetType().GetProperty("Events",
                (BindingFlags.FlattenHierarchy |
                (BindingFlags.NonPublic | 
                 BindingFlags.Instance))).GetValue(eventSource, null)));

            Delegate d = eventHandlers[field.GetValue(eventSource)];

            if (d == null)
              continue;

            Delegate[] subscribers = d.GetInvocationList();

            // ok we found the validation event,  let's get the event method and call it
            foreach (var validationEvent in subscribers)
            {
              var sender    = (object)eventSource;
              var eventArgs = new CancelEventArgs
              {
                Cancel = false
              };

              var parameters = new []
              {
                sender,
                eventArgs
              };

              validationEvent.DynamicInvoke(parameters);

              // if the validation failed we need to return that failure
              if (eventArgs.Cancel)
                return false;

              return true;
            }
          }
        }

        targetType = targetType.BaseType;

      } while (targetType != null);

      return true;
    }

  }
}
