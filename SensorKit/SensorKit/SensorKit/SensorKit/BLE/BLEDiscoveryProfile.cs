using Plugin.BLE;
using SensorKit.Contracts;
using System;
using System.Threading.Tasks;
using Plugin.BLE.Abstractions.Contracts;
using System.Threading;
using System.Diagnostics;

namespace SensorKit.BLE
{
    public class BLEDiscoveryProfile : IDiscoveryProfile
    {
        private IBluetoothLE _bleEngine;
        private IAdapter _adapter;

        public BLEDiscoveryProfile()
        {
            _bleEngine = CrossBluetoothLE.Current;
            _adapter = CrossBluetoothLE.Current.Adapter;

            //_bleEngine.StateChanged += OnStateChanged;
            _adapter.DeviceDiscovered += _adapter_DeviceDiscovered;
            _adapter.ScanTimeoutElapsed += _adapter_ScanTimeoutElapsed;
            _adapter.DeviceDisconnected += _adapter_DeviceDisconnected;
            _adapter.DeviceConnectionLost += _adapter_DeviceConnectionLost;
        }

        private bool _isScanning = false;
        private object _scanningLock = new object();
        private CancellationTokenSource _cancellationTokenSource;

        private OnDeviceFoundDelegate _onDeviceFound;
        private OnDeviceDisconnectedDelegate _onDeviceDisconnected;
        private OnDeviceConnectionErrorDelegate _onDeviceConnectionError;

        public async Task StartScanningAsync(OnDeviceFoundDelegate onDeviceFound,
            OnDeviceDisconnectedDelegate onDeviceDisconnected,
            OnDeviceConnectionErrorDelegate onDeviceConnectionError)
        {
            // no use in scanning, if already ther
            if (_isScanning) return;

            _onDeviceFound = onDeviceFound;
            _onDeviceDisconnected = onDeviceDisconnected;
            _onDeviceConnectionError = onDeviceConnectionError;

            AddPairedDevices();

            _cancellationTokenSource = new CancellationTokenSource();
            await _adapter.StartScanningForDevicesAsync(null, null, false, _cancellationTokenSource.Token);
        }

        private async void AddPairedDevices()
        {
            // first check known devices
            foreach (var device in _adapter.ConnectedDevices)
            {
                try
                {
                    await device.UpdateRssiAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine($"Exception when updating RSSI {e} in {e.StackTrace}");
                }

                AddDevice(device);
            }
        }

        private void AddDevice(IDevice device)
        {
            var sensorKitDevice = BuildBLEDevice(device);
            _onDeviceFound?.Invoke(sensorKitDevice);
        }

        public async void StopScanning()
        {
            await _adapter.StopScanningForDevicesAsync();
        }

        private void _adapter_DeviceDiscovered(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            // forward event to our internal device handler
            AddDevice(e.Device);
        }

        private void _adapter_ScanTimeoutElapsed(object sender, EventArgs e)
        {
            CleanupCancellationToken();
            _isScanning = false;
        }

        private void _adapter_DeviceConnectionLost(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceErrorEventArgs e)
        {
            var sensorKitDevice = BuildBLEDevice(e.Device, true);
            _onDeviceConnectionError?.Invoke(sensorKitDevice, e.ErrorMessage);
        }

        private void _adapter_DeviceDisconnected(object sender, Plugin.BLE.Abstractions.EventArgs.DeviceEventArgs e)
        {
            // try and find the device
            if (e.Device == null)
            {
                Debug.WriteLine("A device was disconnected, but we don't know which one.");
                return;
            }

            var d = BuildBLEDevice(e.Device);
            _onDeviceDisconnected?.Invoke(d);
        }

        private void CleanupCancellationToken()
        {
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }

        private BLEDevice BuildBLEDevice(IDevice device, bool isNullable = false)
        {
            if(device == null)
            {
                if (isNullable) return null;
                throw new ArgumentNullException("device");
            }

            return new BLEDevice(device, this);
        }
    }
}
