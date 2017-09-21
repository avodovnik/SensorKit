using SensorKit.Contracts;
using System;
using System.Collections.Generic;

namespace SensorKit
{
    /// <summary>
    /// The main entry point for all interactions with all the connetable sensors.
    /// </summary>
    public class SensorKit
    {
        private List<IDiscoveryProfile> _discoveryProfiles;
        private List<ISensorProvider> _sensorProfile;
        private SensorRegistry _registry = null;

        public SensorKit()
        {
            _registry = new SensorRegistry();
            _discoveryProfiles = new List<IDiscoveryProfile>();
            _sensorProfile = new List<ISensorProvider>();
        }

        /// <summary>
        /// Adds a new instance of a discovery profile, used to discover devices.
        /// </summary>
        /// <typeparam name="TDiscoveryProfile">An implementation of the <see cref="IDiscoveryProfile"/> interface.</typeparam>
        /// <returns></returns>
        public SensorKit DiscoverWith<TDiscoveryProfile>()
            where TDiscoveryProfile : IDiscoveryProfile, new()
        {
            return DiscoverWith(new TDiscoveryProfile());
        }

        /// <summary>
        /// Adds a specific instance of a discovery profile, used to discover devices.
        /// </summary>
        /// <typeparam name="TDiscoveryProfile">An implementation of the <see cref="IDiscoveryProfile"/> interface.</typeparam>
        /// <param name="discoveryProfile">The instance of the discovery profile.</param>
        /// <returns></returns>
        public SensorKit DiscoverWith<TDiscoveryProfile>(TDiscoveryProfile discoveryProfile)
            where TDiscoveryProfile : IDiscoveryProfile, new()
        {
            if (discoveryProfile == null)
            {
                throw new ArgumentNullException("discoveryProfile.");
            }

            _discoveryProfiles.Add(discoveryProfile);

            return this;
        }

        /// <summary>
        /// Adds a new instance of a sensor provider, that enables usage of connected 
        /// devices.
        /// </summary>
        /// <typeparam name="TSensorProvider"></typeparam>
        /// <returns></returns>
        public SensorKit AddProvider<TSensorProvider>()
            where TSensorProvider : ISensorProvider, new()
        {
            return AddProvider(new TSensorProvider());
        }

        public SensorKit AddProvider<TSensorProvider>(TSensorProvider sensorProvider)
            where TSensorProvider : ISensorProvider, new()
        {
            if(sensorProvider == null)
            {
                throw new ArgumentNullException("sensorProvider");
            }

            _sensorProfile.Add(sensorProvider);

            return this;
        }

        public void Discover()
        {
            // TODO: implement
            foreach(var discoveryProfile in _discoveryProfiles)
            {
                discoveryProfile.StartScanningAsync((device) =>
                {
                    throw new NotImplementedException();
                });
            }
        }

    }
}

/*
 * 
 * sensorKit.Registry.AddProvider<MoveSenseSensorProvider>();
 * sensorKit.Registry.AddProvider<LocalDeviceSensorProvider>();
 * 
 * sensorKit.Discovery.AddProvider<BluetoothDiscoveryProvider>();
 * sensorKit.Discovery.AddProvider<NFCDiscoveryProvider();
 * 
 * -> device disco -> provider
 * -> sensorKit -> events raised -> pipeline
 * 
 * sensorKit.Registry.OnDeviceFound += (device) => { doSomething(); }
 * sensorKit.Registry.OnDeviceLost += (device or deviceId) => { lost(); }
 * 
 * sensorKit.StartDiscovery();
 * 
 * 
 * var pipeline = new SensorKit()
 *                  .Configuration()
 *                  .DiscoverWith<BluetoothDiscovery>()
 *                  .DiscoverWith<NFCDiscovery>()
 *                  .AddProvider<MoveSenseSensorProvider>()
 *                  .AddStorage<StreamToEventHubWithLocalCaching>();
 *                  
 *  pipeline.Discover(onDeviceDiscovered, onDeviceLost);
 */
