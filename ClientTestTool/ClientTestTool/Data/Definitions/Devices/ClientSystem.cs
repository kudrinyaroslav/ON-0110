///
/// @Author Matthew Tuusberg
///

﻿using System;
using System.Collections.Generic;
using System.Linq;
using ClientTestTool.Data.Definitions.Interfaces;
using ClientTestTool.Data.Definitions.Trace;
using ClientTestTool.GUI.Enums;

namespace ClientTestTool.Data.Definitions.Devices
{
  public class ClientSystem : Client, ISystem<Client>
  {
    public ClientSystem(String name, IEnumerable<Client> clients, IEnumerable<NetworkTraceInfo> traces) : base(traces)
    {
      mName     = name;
      mClients  = new List<Client>(clients);
      Type      = UnitType.System;
      Mac       = String.Join(", ", mClients.Select(item => item.Mac));
      Ip        = String.Join(", ", mClients.Select(item => item.Ip));
      IsIgnored = false;
    }

    public List<Client> Clients
    {
      get
      {
        return mClients.ToList();
      }
    }

    public new bool IsIgnored
    {
      get
      {
        return mIsIgnored;
      }
      set
      {
        mClients.ForEach(item => item.IsIgnored = value);
        mIsIgnored = value;
      }
    }

    public void Add(Client item)
    {
      if (null == item)
        throw new ArgumentNullException("item");

      mClients.Add(item);
    }

    public void Remove(Client item)
    {
      if (null == item)
        throw new ArgumentNullException("item");

      mClients.Remove(item);
    }

    private readonly List<Client> mClients;
  }
}
