// 2024-03-06
// Group 1 -> Adam Cunard, Simon Luna, Tyler Meekel, Sunhee Ku 
// FlightsManger.cs
// Takes in no inputs.
// Contains logic for reading in airport and flight files
// Also allows searching flights based on departing and arriving airports along with the day of the week

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace A2FlightReservations.Models
{
    public static class FlightsManager
    {
        //relative path for flights.csv
        public static string FLIGHT_CSV_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\CSVFiles\flights.csv");
        public static string AIRPORT_CSV_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\CSVFiles\airports.csv");

        //Creates a list of flight objects from flights.csv
        static List<Flight> flights = new List<Flight>(PopulateFlights());
        static List<Airport> airports = new List<Airport>(PopulateAirports());

        //returns whole list of flights
        public static List<Flight> GetFlights() { return flights; }

        //given departing airport, arriving airport, and day. returns a list of flight objects that match those critiria
        public static List<Flight> SearchFlights(string departingInput, string arrivingInput, string day) 
        {
            string[] daysOfTheWeek = ["Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday"];

            string departingCode = "";
            if (!string.IsNullOrEmpty(departingInput)) 
            {
                //if the length of the departing input is 3, we assume that the user has inputted an airport code
                if (departingInput.Trim().Length == 3)
                {
                    departingCode = departingInput.Trim().ToUpper();

                }
                //else we assume the user has inputted the name of the airport
                else
                {
                    foreach (var airport in airports)
                    {
                        if (airport.AirportName.ToUpper().Contains(departingInput.ToUpper()))
                        {
                            departingCode = airport.Code;
                            break;
                        }
                    }

                    //if we dont find and airport name that matches the input, search flights based on the user input
                    if (departingCode == "")
                    {
                        departingCode = departingInput.ToUpper();
                    }
                }
            }
          
            string arrivingCode = "";
            if (!string.IsNullOrEmpty(arrivingInput)) 
            {
                //if the length of the arriving input is 3, we assume that the user has inputted an airport code
                if (arrivingInput.Trim().Length == 3)
                {
                    arrivingCode = arrivingInput.Trim().ToUpper();

                }
                //else we assume the user has inputted the name of the airport
                else
                {
                    foreach (var airport in airports)
                    {
                        if (airport.AirportName.ToUpper().Contains(arrivingInput.ToUpper()))
                        {
                            arrivingCode = airport.Code;
                            break;
                        }
                    }

                    //if we dont find and airport name that matches the input, search flights based on the user input
                    if (arrivingCode == "")
                    {
                        arrivingCode = arrivingInput.ToUpper();
                    }
                }
            }

            string dayCode = "";
            if (!string.IsNullOrEmpty(day))
            {
                foreach (string weekday in daysOfTheWeek) 
                {
                    if (weekday.ToUpper().Contains(day.ToUpper())) 
                    { 
                        dayCode = weekday.ToUpper();
                        break;
                    }
                }
            }
       
            //Read every flight in flights, return a list of flights that have the same departing and/or arriving airports and/or same day
            // For example, if the user where to enter 'Monday' all Monday flights will be displayed.
            List<Flight> validFlights = new List<Flight>();
            foreach (var flight in flights)
            {
                // Check if a value was given for reservationCode
                if (!string.IsNullOrWhiteSpace(departingInput))
                {
                    // Check if reservation matches given code. Skip this reservation otherwise.
                    if (departingCode != flight.Departing.ToUpper())
                    {
                        continue;
                    }
                }

                // Check if a value was given for airline
                if (!string.IsNullOrWhiteSpace(arrivingInput))
                {
                    // Check if reservation airline matches given airline. Skip this reservation otherwise.
                    if (arrivingCode != flight.Arriving.ToUpper())
                    {
                        continue;
                    }
                }

                // Check if a value was given for travellerName
                if (!string.IsNullOrWhiteSpace(day))
                {
                    // Check if traveller name matches given name. Skip this reservation otherwise.
                    if (dayCode != flight.Day.ToUpper())
                    {
                        continue;
                    }
                }

                // Add value to list if we get to this point.
                validFlights.Add(flight);
            }

        return validFlights;
        }

        public static void bookFlight(Flight bookedFlight)
        {
            try
            {
                var lines = File.ReadAllLines(FLIGHT_CSV_PATH).ToList();

                for (int i = 0; i < flights.Count; i++)
                {
                    if (flights[i] == bookedFlight)
                    {
                        flights[i].AvailableSeats--;
                        string[] sLine = lines[i].Split(',');
                        sLine[6] = flights[i].AvailableSeats.ToString();
                        lines[i] = string.Join(',', sLine);
                    }
                }

                File.WriteAllLines(FLIGHT_CSV_PATH, lines);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            
        }

        //creates flight objects from csv and returns a list of all flights
        private static List<Flight> PopulateFlights()
        {
            List<Flight> myFlights = new List<Flight>();
            string line;
            try
            {
                StreamReader sr = new StreamReader(FLIGHT_CSV_PATH);
                line = sr.ReadLine();
                while (line != null)
                {
                    string[] sLine = line.Split(',');
                    string flightCode = sLine[0];
                    string airline = sLine[1];
                    string departing = sLine[2];
                    string arriving = sLine[3];
                    string day = sLine[4];
                    string time = sLine[5];
                    int.TryParse(sLine[6], out int availableSeats);
                    double.TryParse(sLine[7], out double pricePerSeat);

                    Flight newFlight = new Flight {FlightCode = flightCode, Airline = airline, Departing = departing, Arriving = arriving, Day = day, Time = time, AvailableSeats = availableSeats, PricePerSeat = pricePerSeat  };
                    myFlights.Add(newFlight);
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            return myFlights;

        }

        //creates airport objects from csv and returns a list of all airports
        private static List<Airport> PopulateAirports()
        {
            List<Airport> myAirports = new List<Airport>();
            string line;
            try
            {
                StreamReader sr = new StreamReader(AIRPORT_CSV_PATH);
                line = sr.ReadLine();
                while (line != null)
                {
                    string[] sLine = line.Split(',');
                    string airportCode = sLine[0];
                    string airportName = sLine[1];

                    Airport newAirport = new Airport { Code = airportCode, AirportName = airportName }; 
                    myAirports.Add(newAirport);
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            return myAirports;

        }

    }
}
