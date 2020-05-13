///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ClientTestTool.Data.Definitions.Devices;
using ClientTestTool.Data.Definitions.Devices.Base;
using ClientTestTool.Data.Definitions.Devices.Extensions;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.Data.Extensions;
using ClientTestTool.GUI.Enums;
using ClientTestTool.Tests.Engine.Enums;

namespace ClientTestTool.Data.Global
{
  public sealed class UnitSet
  {
    #region Singleton

    private static UnitSet mInstance;

    public static UnitSet Instance
    {
      get
      {
        return mInstance ?? (mInstance = new UnitSet());
      }
    }

    static UnitSet()
    {}

    #endregion

    private UnitSet()
    {
      mUnits = new BindingList<Unit>();
      mUnits.ListChanged             += OnUnitsListChanged;
      NetworkTraceSet.OnTraceRemoved += (sender, args) => RemoveUnits(args.NetworkTrace);
    }

    #region Events

    public static event EventHandler<ListChangedEventArgs> OnUnitListChanged;

    #endregion

    #region Properties

    public static bool IsEmpty
    {
      get
      {
        return 0 == Instance.mUnits.Count;
      }
    }

    public static int Count
    {
      get
      {
        return Instance.mUnits.Count;
      }
    }


    #endregion

    #region List Manipulation

    public static void Add(Unit item)
    {
      Instance.mUnits.Add(item);
    }

    public static void AddRange(IEnumerable<Unit> items)
    {
      Instance.mUnits.AddRange(items);
    }

    public static void Remove(Unit item)
    {
      Instance.mUnits.Remove(item);
    }

    public static void RemoveAll(Func<Unit, bool> predicate)
    {
      var matches = mInstance.mUnits.Where(predicate).ToList();
      matches.ForEach(item => mInstance.mUnits.Remove(item));
    }

    /// <summary>
    /// Remove all devices connected to trace
    /// </summary>
    /// <param name="trace"></param>
    private void RemoveUnits(NetworkTraceInfo trace)
    {
      foreach (Unit unit in GetUnits(trace))
      {
        if (UnitType.System == unit.Type)
        {
          var system = (ClientSystem) unit;

          system.FoundInTraces.Remove(trace);

          var clients = system.Clients.ToList();

          foreach (var client in clients)
          {
            client.FoundInTraces.Remove(trace);

            if (0 != client.FoundInTraces.Count)
              continue;

            ConversationList.RemoveConnectedConversations(client);
            system.Remove(client);
          }

          if (system.Clients.Count <= 1)
            Split(system);
        }
        else
        {
          unit.FoundInTraces.Remove(trace);

          if (0 != unit.FoundInTraces.Count)
            continue;

          ConversationList.RemoveConnectedConversations(unit);
          Instance.mUnits.Remove(unit);
        }
      }
    }

    #endregion

    #region Static Methods

    public static bool Contains(Unit item)
    {
      return Instance.mUnits.Contains(item);
    }

    public static List<Unit> GetUnits()
    {
      return Instance.mUnits.ToList();
    }

    public static List<Unit> GetUnits(NetworkTraceInfo trace)
    {
      return Instance.mUnits.Where(item => item.FoundInTraces.Contains(trace)).ToList();
    }

    public static Unit GetUnit(String mac)
    {
      if (String.IsNullOrEmpty(mac))
        throw new ArgumentNullException("mac");

      var units = Instance.mUnits.ToList().Where(item => UnitType.System != item.Type).Cast<Unit>();

      return units.ToList().FirstOrDefault(item => item.Mac == mac);
    }

    public static Unit GetUnitAt(int index)
    {
      if (index < 0 || index >= mInstance.mUnits.Count)
        throw new ArgumentOutOfRangeException("index");

      return mInstance.mUnits[index];
    }

    public static Client GetClient()
    {
      var matches = Instance.mUnits.Where(item => !item.IsIgnored && (UnitType.System == item.Type || UnitType.Client == item.Type)).Cast<Client>();

      return matches.SingleOrDefault();
    }

    public static List<Client> GetClients()
    {
      return Instance.mUnits.Where(item => UnitType.Client == item.Type && !item.IsIgnored).Cast<Client>().ToList();
    }

    public static List<ClientSystem> GetSystems()
    {
      return Instance.mUnits.Where(item => UnitType.System == item.Type && !item.IsIgnored).Cast<ClientSystem>().ToList();
    }

    public static List<Device> GetDevices()
    {
      return Instance.mUnits.Where(item => UnitType.Device == item.Type && !item.IsIgnored).Cast<Device>().ToList();
    }

    public static List<Device> GetDevices(Profile supportedProfile)
    {
      return GetDevices().Where(device => device.GetSupportedProfiles().Contains(supportedProfile)).ToList();
    }

    public static List<Profile> GetSupportedProfiles()
    {
      return new HashSet<Profile>(GetDevices().SelectMany(device => device.GetSupportedProfiles())).ToList();
    }

    public static ClientSystem Merge(String systemName, List<Client> clients)
    {
      var traces = new HashSet<NetworkTraceInfo>(clients.SelectMany(item => item.FoundInTraces));
      var system = new ClientSystem(systemName, clients, traces);

      clients.ForEach(item => Instance.mUnits.Remove(item));
      Instance.mUnits.Add(system);

      return system;
    }

    public static void Split(ClientSystem system)
    {
      var clients = system.Clients.ToList();
      Instance.mUnits.Remove(system);
      clients.ForEach(item => Instance.mUnits.Add(item));
    }

  #endregion

    /// <summary>
    /// Removing duplicates and list sorting
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void OnUnitsListChanged(object sender, ListChangedEventArgs args)
    {
      if (ListChangedType.ItemAdded == args.ListChangedType)
      {
        ISet<Unit> unitSet = new HashSet<Unit>(mUnits);

        if (mUnits.Count != unitSet.Count)
        {
          mUnits.RaiseListChangedEvents = false;
          mUnits.Clear();
          unitSet.ToList().ForEach(mUnits.Add);
          mUnits.RaiseListChangedEvents = true;
        }

        mUnits.Sort(new UnitComparer());
      }

      if (null != OnUnitListChanged)
        OnUnitListChanged(sender, args);
    }

    private readonly BindingList<Unit> mUnits;
  }
}
