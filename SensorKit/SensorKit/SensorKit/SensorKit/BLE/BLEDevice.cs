using Plugin.BLE.Abstractions.Contracts;
using SensorKit.Contracts;

namespace SensorKit.BLE
{
    internal sealed class BLEDevice : ISensorKitDevice
    {
        private IDevice _root;
        private BLEDiscoveryProfile _profile;

        public BLEDevice(Plugin.BLE.Abstractions.Contracts.IDevice root, BLEDiscoveryProfile profile)
        {
            _root = root;
            _profile = profile;
        }

        public IDiscoveryProfile Profile => _profile;
    }
}
