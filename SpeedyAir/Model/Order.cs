using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SpeedyAir.Model
{
    public class Order
    {

        public string OrderNumber { get; set; }
        public string destination { get; set; }

        public int FlightNumber { get; set; }

    } 

}
