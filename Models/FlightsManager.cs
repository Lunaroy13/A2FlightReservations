﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2FlightReservations.Models
{
    public static class FlightsManager
    {

        public static string FLIGHTCSVPATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\CSVFiles\flights.csv");

        static List<Flight> flights = new List<Flight>(PopulateFlights());

        public static List<Flight> GetFlights() { return flights; }

        public static List<Flight> SearchFlights(string departing, string arriving, string day) {

            List<Flight> validFlights = new List<Flight>();
            foreach (var flight in flights)
            {
                if(flight.Departing == departing && flight.Arriving == arriving && flight.Day == day)
                {
                    validFlights.Add(flight);
                }
            }
            return validFlights;
        }

        private static List<Flight> PopulateFlights()
        {
            List<Flight> myFlights = new List<Flight>();
            string line;
            try
            {
                StreamReader sr = new StreamReader(FLIGHTCSVPATH);
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
                    int.TryParse(sLine[7], out int pricePerSeat);

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
        
    }
}
