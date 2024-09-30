// See https://aka.ms/new-console-template for more information

using Newtonsoft.Json.Linq;
using SpeedyAir.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;


string? nullableString = "";
int? nullableInt = 0;
//load the flight details
List<Flight> flights = loadFlight();

//display the flight details
//USER STORY #1 

foreach (Flight flight in flights)
{
    Console.WriteLine(string.Format("Flight: {0}, departure: {1}, arrival: {2}, day: {3}", flight.FlightNumber, flight.OriginationCode, flight.DestinationCode, flight.FlightDay));
}

//Load the orders from file
List<Order> orders = LoadOrder("../../../coding-assigment-orders.json"); 

//Group the orders based on destinations
var GroupedOrders = orders.GroupBy(x => x.destination).OrderBy(y => y.Key);

//assign the flights to the order
foreach(var group in GroupedOrders)
{
    foreach(Order ord in group)
    {
        var avialbleFlight = flights.FindAll(x => x.DestinationCode == group.Key && x.Orders.Count < x.OrderCapacity).FirstOrDefault();
        if (avialbleFlight != null)
        {
            ord.FlightNumber = avialbleFlight.FlightNumber;
            avialbleFlight.Orders.Add(ord.OrderNumber);
        }
        else
        {
            //if there is no flights available, the no need to check for the rst of the orders, so exit the loop
            break; 
        }
    }
}

//get orders with flight info (inner join)
var OrderWithFlight = from _order in orders
                    join _flight in flights
                    on _order.FlightNumber equals _flight.FlightNumber into OrderFlight
                    from _orderFlight in OrderFlight.DefaultIfEmpty()
                    select new
                    { 
                        FlightNumber = _order.FlightNumber,
                        OrderNumber = _order.OrderNumber,
                        OriginationCode = _orderFlight?.OriginationCode,
                        DestinationCode = _orderFlight?.DestinationCode,
                        FlightDay = _orderFlight?.FlightDay
                    };
//gets the orders without flight
var OrderWithoutFlight = from  _order in orders
                         where _order.FlightNumber == 0
                     select new
                     {
                         FlightNumber = 0,
                         OrderNumber = _order.OrderNumber,
                         OriginationCode = nullableString, 
                         DestinationCode = nullableString,
                         FlightDay = nullableInt
                     };
 // merge both lists to iterate
var orderFull = OrderWithoutFlight.Union(OrderWithFlight);


//USER STORY #2
foreach (dynamic _of in orderFull)
{
    if (_of.FlightNumber > 0)
        Console.WriteLine(string.Format("order: {0}, flightNumber: {1}, departure: {2}, arrival: {3}, day: {4}", _of.OrderNumber, _of.FlightNumber, _of.OriginationCode, _of.DestinationCode, _of.FlightDay));
    else
        Console.WriteLine(string.Format("order: {0}, flightNumber: not scheduled", _of.OrderNumber));
}
 

List<Order> LoadOrder(string fileName)
{
    List<Order> ordersList = new List<Order>();
    if (!string.IsNullOrEmpty(fileName))
    {
        using (StreamReader reader = new StreamReader(fileName))
        {
            try
            {
                string jsonString = reader.ReadToEnd(); 
                var orders = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(jsonString); 
                foreach (var (key, val) in orders)
                {
                    ordersList.Add(new Order { OrderNumber = key, destination = val.Values.FirstOrDefault() });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
    return ordersList;
}

List<Flight> loadFlight()
{
    List<Flight> flights = new List<Flight>();
    flights.Add(new Flight { FlightNumber = 1, Origination = "Montreal airport", OriginationCode= "YUL", Destination = "Toronto", DestinationCode= "YYZ", FlightDay = 1 });
    flights.Add(new Flight { FlightNumber =2, Origination = "Montreal", OriginationCode = "YUL", Destination = "Calgary", DestinationCode = "YYC", FlightDay = 1 });
    flights.Add(new Flight { FlightNumber = 3, Origination = "Montreal", OriginationCode = "YUL", Destination = "Vancouver", DestinationCode = "YVR", FlightDay = 1 });
    flights.Add(new Flight { FlightNumber = 4, Origination = "Montreal airport", OriginationCode = "YUL", Destination = "Toronto", DestinationCode = "YYZ", FlightDay = 2 });
    flights.Add(new Flight { FlightNumber = 5, Origination = "Montreal", OriginationCode = "YUL", Destination = "Calgary", DestinationCode = "YYC", FlightDay = 2 });
    flights.Add(new Flight { FlightNumber = 6, Origination = "Montreal", OriginationCode = "YUL", Destination = "Vancouver", DestinationCode = "YVR", FlightDay = 2 });
    return flights;
}

