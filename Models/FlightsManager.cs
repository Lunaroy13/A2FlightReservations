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
        public static string FLIGHT_CSV_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\Files\flights.csv");
        public static string AIRPORT_CSV_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\Files\airports.csv");

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

        // bookFlight books a flight, and updates the CSV file data
        public static void bookFlight(Flight bookedFlight)
        {
            try
            {
                // Read all lines from the csv file
                var lines = File.ReadAllLines(FLIGHT_CSV_PATH).ToList();

                // For each flight in the flights object
                for (int i = 0; i < flights.Count; i++)
                {
                    // Find the flight that matches the booked flight
                    if (flights[i] == bookedFlight)
                    {
                        // Book a seat
                        flights[i].AvailableSeats--;

                        // Create a list of string from the matching flight in the CSV file
                        string[] sLine = lines[i].Split(',');

                        // Update the line for the csv file
                        sLine[6] = flights[i].AvailableSeats.ToString();
                        
                        // Join the line back together and add it back to the CSV lines array
                        lines[i] = string.Join(',', sLine);
                    }
                }

                // Write the lines array to the flights csv file
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
            // Create new empty list of flights
            List<Flight> myFlights = new List<Flight>();

            // Create empty string to hold the data for each line
            string line;
            try
            {
                // open the Flights CSV file as a stream reader
                StreamReader sr = new StreamReader(FLIGHT_CSV_PATH);

                // Read the header line
                line = sr.ReadLine();

                // While there is still information to read from the file
                while (line != null)
                {
                    // Split the line by commas
                    string[] sLine = line.Split(',');

                    // Add each split section to the field
                    string flightCode = sLine[0];
                    string airline = sLine[1];
                    string departing = sLine[2];
                    string arriving = sLine[3];
                    string day = sLine[4];
                    string time = sLine[5];
                    int.TryParse(sLine[6], out int availableSeats);
                    double.TryParse(sLine[7], out double pricePerSeat);

                    // create new flight object, and add each field to the object
                    Flight newFlight = new Flight {FlightCode = flightCode, Airline = airline, Departing = departing, Arriving = arriving, Day = day, Time = time, AvailableSeats = availableSeats, PricePerSeat = pricePerSeat  };
                    
                    // add new flight object to flights list
                    myFlights.Add(newFlight);

                    // Read the next line
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
            // Create empty list of airport objects
            List<Airport> myAirports = new List<Airport>();

            // Create empty string
            string line;

            try
            {
                // Open StreamReader for AIRPORT CSV file
                StreamReader sr = new StreamReader(AIRPORT_CSV_PATH);

                // read a line from the streamreader
                line = sr.ReadLine();

                // While there is still data to read from the CSV file
                while (line != null)
                {
                    // split the line into an array of strings
                    string[] sLine = line.Split(',');

                    // Create fields for each split section
                    string airportCode = sLine[0];
                    string airportName = sLine[1];

                    // Create new airport object from split string
                    Airport newAirport = new Airport { Code = airportCode, AirportName = airportName }; 

                    // Save to airports list
                    myAirports.Add(newAirport);

                    // Read the next line
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
