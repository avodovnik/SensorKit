using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SensorKit
{
    [Flags]
    public enum SensorCapabilities : int
    {
        Hub, // sensor hub device, e.g. mobile device
        IMU,
        ChangeColor,
        Button,
        Mute
    }
}
