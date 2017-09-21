using System;
using System.Collections.Generic;
using System.Text;

namespace SensorKit.Contracts
{
    public interface ISensorKitDevice
    {
        IDiscoveryProfile Profile { get; }
    }
}
