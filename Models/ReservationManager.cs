using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualBasic;


namespace A2FlightReservations.Models
{
    public static class ReservationManager
    {

        private static string RESERVATIONCSVPATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\CSVFiles\reservations.csv");

        //Creates a list of flight objects from flights.csv
        private static List<Reservation> _reservations = new List<Reservation>(PopulateReservations());

        public static List<Reservation> GetReservations() { return _reservations; }

        private static List<Reservation> PopulateReservations()
        {
            List<Reservation> myReservations = new List<Reservation>();
            string line;
            try
            {
                StreamReader sr = new StreamReader(RESERVATIONCSVPATH);
                line = sr.ReadLine();
                while (line != null)
                {
                    string[] sLine = line.Split(',');
                    string flightCode = sLine[0];
                    string airline = sLine[1];
                    string day = sLine[2];
                    string time = sLine[3];
                    int.TryParse(sLine[4], out int cost);
                    string name = sLine[5];
                    string citizenship = sLine[6];

                    Reservation newReservation = new Reservation { 
                        FlightCode = flightCode, 
                        Airline = airline, 
                        Day = day, 
                        Time = time, 
                        Cost = cost, 
                        Name = name, 
                        Citizenship = citizenship
                    };
                    myReservations.Add(newReservation);
                    line = sr.ReadLine();
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex.Message);
            }
            return myReservations;

        }

        public static Reservation makeReservation(Flight flight, string name, string citizenship)
        {
            Reservation newReservation;
            // Check if name is null or empty
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new Exception("Error making reservation, the given name is empty");
            }
            // Check if citizenship is null or empty
            if (string.IsNullOrWhiteSpace(citizenship))
            {
                throw new Exception("Error making reservation, the given citizenship is empty");
            }
            // Check that there is available space on the flight
            if (flight.AvailableSeats > 0)
            {
                newReservation = new Reservation
                {
                    ReservationCode = GenerateReservationCode(),
                    FlightCode = flight.FlightCode,
                    Airline = flight.Airline,
                    Day = flight.Day,
                    Time = flight.Time,
                    Cost = flight.PricePerSeat,
                    Name = name,
                    Citizenship = citizenship
                };

                // Add new reservation to reservations list
                _reservations.Add(newReservation);

                try
                {
                    using StreamWriter sw = File.AppendText(RESERVATIONCSVPATH);
                    sw.WriteLine(formatForFile(newReservation));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
            }
            else
            {
                throw new Exception("Error making reservation, the flight is fully booked.");
            }
            return newReservation;
        }

        private static string GenerateReservationCode()
        {
            List<string> reservationCodes = [];

            foreach (Reservation reservation in _reservations)
            {
                reservationCodes.Add(reservation.ReservationCode);
            }

            string newCode = "";
            const string VALIDCHAR = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string VALIDNUM = "1234567890";

            var allowedCharSet = VALIDCHAR.ToArray();
            var allowedNumSet = VALIDNUM.ToArray();

            Random random = new();
            
            while (true)
            {
                for (int i = 0; i < 4; i++)
                {
                    int randomCharIndex = random.Next(0, allowedCharSet.Length);
                    char selectedChar = allowedCharSet[randomCharIndex];

                    newCode += selectedChar;
                }
                int randomNumIndex = random.Next(0, allowedNumSet.Length);
                char selectedNum = allowedNumSet[randomNumIndex];
                newCode += selectedNum;

                if (!reservationCodes.Contains(newCode)) 
                {
                    break;
                }
            } 

            return newCode;
        }

        public static List<Reservation> findReservation(string? reservationCode, string? airline, string? travellerName)
        {
            List<Reservation> foundReservations = new List<Reservation>();
            
            foreach (Reservation reservation in _reservations) {
                // Check if a value was given for reservationCode
                if (!string.IsNullOrWhiteSpace(reservationCode)) {
                    // Check if reservation matches given code. Skip this reservation otherwise.
                    if (reservationCode != reservation.ReservationCode) {
                        continue;
                    }
                }

                // Check if a value was given for airline
                if (!string.IsNullOrWhiteSpace(airline)) {
                    // Check if reservation airline matches given airline. Skip this reservation otherwise.
                    if (airline != reservation.Airline) {
                        continue;
                    }
                }

                // Check if a value was given for travellerName
                if (!string.IsNullOrWhiteSpace(travellerName)) {
                    // Check if traveller name matches given name. Skip this reservation otherwise.
                    if (travellerName != reservation.Name) {
                        continue;
                    }
                }

                // Add value to list if we get to this point.
                foundReservations.Add(reservation);
            }

            return foundReservations;
        }

        /// Overwrites csv file with current data
        public static void persist() {
            foreach (Reservation reservation in _reservations) {
                try
                {
                    // Creates file, or overwrites all data
                    using StreamWriter sw = File.CreateText(RESERVATIONCSVPATH);
                    sw.WriteLine(formatForFile(reservation));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
            }
        }

        private static string formatForFile(Reservation reservation)
        {
            string output = "";

            output += reservation.FlightCode + ",";
            output += reservation.Airline + ",";
            output += reservation.Day + ",";
            output += reservation.Time + ",";
            output += reservation.Cost.ToString() + ",";
            output += reservation.Name + ",";
            output += reservation.Citizenship;

            return output;
        }

    }
}
