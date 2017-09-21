using Plugin.BLE.Abstractions.Contracts;
using SensorKit.Contracts;
using System;

namespace SensorKit.BLE
{
    internal sealed class BLEDevice : ISensorKitDevice
    {
        private IDevice _root;
        private BLEDiscoveryProfile _profile;

        public BLEDevice(Plugin.BLE.Abstractions.Contracts.IDevice root, BLEDiscoveryProfile profile)
        {
            if (_root == null)
            {
                throw new ArgumentNullException("device root");
            }

            _root = root;
            _profile = profile;
        }

        public IDiscoveryProfile Profile => _profile;

        public string Name => _root.Name;

        public string Id => _root.Id.ToString("N");
    }
}
