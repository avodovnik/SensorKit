using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SensorKit
{
    [DataContract]
    public class SensorItem
    {
        public DateTimeOffset timestamp { get; set; }

        public int activityTypeId { get; set; }

        public double aX { get; set; }
        public double aY { get; set; }
        public double aZ { get; set; }
        public double avX { get; set; }
        public double avY { get; set; }
        public double avZ { get; set; }
        public double qW { get; set; }
        public double qX { get; set; }
        public double qY { get; set; }
        public double qZ { get; set; }

        public double lat { get; set; }
        public double lon { get; set; }
        public double speed { get; set; }
        public double alt { get; set; }
        public double incl { get; set; }
    }
}
