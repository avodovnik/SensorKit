using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SensorKit.Contracts
{
    /// <summary>
    /// Provides the interface for the discovery of supported devices. The implementations
    /// of this interface can decide which protocol to use to connect to them. An example 
    /// implementation is the Bluetooth Low Energy Discovery Profile.
    /// </summary>
    public interface IDiscoveryProfile
    {
        Task StartScanningAsync(
            OnDeviceFoundDelegate onDeviceFound,
            OnDeviceDisconnectedDelegate onDeviceDisconnected,
            OnDeviceConnectionErrorDelegate onDeviceConnectionError);

        void StopScanning();

    }

    public delegate void OnDeviceFoundDelegate(ISensorKitDevice device);
    public delegate void OnDeviceDisconnectedDelegate(ISensorKitDevice device);
    public delegate void OnDeviceConnectionErrorDelegate(ISensorKitDevice device, string errorMessage);

}
