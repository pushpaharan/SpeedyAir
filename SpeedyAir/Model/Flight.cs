using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedyAir.Model
{
    public class Flight
    {
        public int FlightNumber { get; set; }
        public string Origination { get; set; }
        public string OriginationCode { get; set; }
        public string Destination { get; set; }
        public string DestinationCode { get; set; }
        public int FlightDay { get; set; }
        public int OrderCapacity { get; set; }
        public List<String> Orders { get; set; }

        public Flight()
        {
            Orders = new List<String>();
            OrderCapacity = 20;
        }
    }
}
