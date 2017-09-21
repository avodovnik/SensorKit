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
            _adapter.DeviceDiscovered += _adapter_DeviceDiscovered; ;
            //_adapter.ScanTimeoutElapsed += Adapter_ScanTimeoutElapsed;
            //_adapter.DeviceDisconnected += OnDeviceDisconnected;
            //_adapter.DeviceConnectionLost += OnDeviceConnectionLost;
        }

        private bool _isScanning = false;
        private object _scanningLock = new object();
        private CancellationTokenSource _cancellationTokenSource;

        private OnDeviceFoundDelegate _onDeviceFound;

        public async Task StartScanningAsync(OnDeviceFoundDelegate onDeviceFound)
        {
            // no use in scanning, if already ther
            if (_isScanning) return;

            _onDeviceFound = onDeviceFound;

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
            var sensorKitDevice = new BLEDevice(device, this);
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
    }
}
