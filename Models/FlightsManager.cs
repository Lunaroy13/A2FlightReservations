using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static List<Flight> SearchFlights(string departingInput, string arrivingInput, string day) {

            string departingCode = "";

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
                if(departingCode == "")
                {
                    departingCode = departingInput.ToUpper();
                }
            }

            string arrivingCode = "";
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

            //Read every flight in flights, return a list of flights that have the same departing and arriving airports and same day
            List<Flight> validFlights = new List<Flight>();
            foreach (var flight in flights)
            {
                if(flight.Departing == departingCode && flight.Arriving == arrivingCode && flight.Day.ToUpper() == day.ToUpper())
                {
                    validFlights.Add(flight);
                }
            }
            return validFlights;
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
