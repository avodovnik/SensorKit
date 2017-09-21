using SensorKit.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorKit
{
    /// <summary>
    /// Provides the central list of detected sensors.
    /// </summary>
    public class SensorRegistry
    {
        private Dictionary<string, ISensorKitDevice> _devices;

        public SensorRegistry()
        {
            _devices = new Dictionary<string, ISensorKitDevice>();
        }

        public delegate void SensorFoundDelegate();
        public delegate void SensorRemovedDelegate();

        internal void DeviceFound(ISensorKitDevice device)
        {
            lock (_devices)
            {
                ISensorKitDevice sensorKitDevice;
                // try and find the device with the same id
                if (_devices.TryGetValue(device.Id, out sensorKitDevice);)
                // in which case we update it
                // and if it's not found,
                // we add it to the registry
                //device.Id
            }
        }
    }
}
