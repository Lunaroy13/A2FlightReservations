using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.VisualBasic;


namespace A2FlightReservations.Models
{
    public static class ReservationManager
    {
        private static string RESERVATIONBINPATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\Files\reservations.bin");

        //Creates a list of flight objects from flights.csv
        private static List<Reservation> _reservations = new List<Reservation>(DeserializeReservations());

        public static List<Reservation> GetReservations() { return _reservations; }

        private static List<Reservation> DeserializeReservations()
        {
            List<Reservation> myReservations = new List<Reservation>();

            using (var fileStream = File.Open(RESERVATIONBINPATH, FileMode.Open))
            {
                using (var binaryReader = new BinaryReader(fileStream))
                {
                    while (fileStream.Position < fileStream.Length)
                    {
                        Reservation newReservation = new Reservation();

                        newReservation.ReservationCode = binaryReader.ReadString();
                        newReservation.FlightCode = binaryReader.ReadString();
                        newReservation.Airline = binaryReader.ReadString();
                        newReservation.Day = binaryReader.ReadString();
                        newReservation.Time = binaryReader.ReadString();
                        newReservation.Cost = binaryReader.ReadDouble();
                        newReservation.Name = binaryReader.ReadString();
                        newReservation.Citizenship = binaryReader.ReadString();

                        myReservations.Add(newReservation);
                    }
                }
            }
            return myReservations;

        }

        public static Reservation? makeReservation(Flight flight, string name, string citizenship)
        {
            Reservation newReservation;
            // Check if name is null or empty

            try
            {
                if (flight == null)
                {
                    throw new Exception("Error making reservation, no flight was selected");
                }
            }
            catch
            {
                return null;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new Exception("Error making reservation, the given name is empty");
                }
            }
            catch
            {
                return null;
            }

            // Check if citizenship is null or empty
            try
            {
                if (string.IsNullOrWhiteSpace(citizenship))
                {
                    throw new Exception("Error making reservation, the given citizenship is empty");
                }
            }
            catch
            {
                return null;
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

                SerializeReservations(_reservations);
            }
            else
            {
                try
                {
                    throw new Exception("Error making reservation, the flight is fully booked.");
                }
                catch
                {
                    return null;
                }
            }
            return newReservation;
        }

        private static void SerializeReservations(List<Reservation> reservations)
        {
            using (var fileStream = File.Open(RESERVATIONBINPATH, FileMode.OpenOrCreate))
            {
                using (var binaryWriter = new BinaryWriter(fileStream))
                {

                    foreach (var reservation in reservations)
                    {
                        binaryWriter.Write(reservation.ReservationCode);
                        binaryWriter.Write(reservation.FlightCode);
                        binaryWriter.Write(reservation.Airline);
                        binaryWriter.Write(reservation.Day);
                        binaryWriter.Write(reservation.Time);
                        binaryWriter.Write(reservation.Cost);
                        binaryWriter.Write(reservation.Name);
                        binaryWriter.Write(reservation.Citizenship);
                    }

                }
            }

        }

        // GenerateReservationCode generates a Reservation code for a Reservation following the ABCD1 format
        private static string GenerateReservationCode()
        {
            // Create an empty string list to hold existing reservation codes
            List<string> reservationCodes = [];

            // Add each existing reservation code to this list
            foreach (Reservation reservation in _reservations)
            {
                reservationCodes.Add(reservation.ReservationCode);
            }

            // Create an empty string to hold the new code
            string newCode = "";

            // Create constants to hold valid characters and numbers to add to the Reservation Code
            const string VALIDCHAR = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string VALIDNUM = "1234567890";

            // Create arrays out of these character sets
            var allowedCharSet = VALIDCHAR.ToArray();
            var allowedNumSet = VALIDNUM.ToArray();

            Random random = new();

            while (true)
            {
                // Generate 4 letters
                for (int i = 0; i < 4; i++)
                {
                    int randomCharIndex = random.Next(0, allowedNumSet.Length);
                    char selectedChar = allowedNumSet[randomCharIndex];

                    // Add letter to new code string
                    newCode += selectedChar;
                }
                
                // Generate 1 random number
                int randomNumIndex = random.Next(0, allowedCharSet.Length);
                char selectedNum = allowedCharSet[randomNumIndex];

                // Add number to new Reservation code
                newCode += selectedNum;

                if (!reservationCodes.Contains(newCode))
                {
                    // If Reservation code doesn't already exist, break out of this loop.
                    break;
                }
                else
                {
                    // If it does exist, reset the string and try again.
                    newCode = "";
                }
            }

            return newCode;
        }

        // findReservation returns a list of Reservations that match the given reservation code, airline and traveller name.
        // Each of these fields is optional
        public static List<Reservation> findReservation(string? reservationCode, string? airline, string? travellerName)
        {
            List<Reservation> foundReservations = new List<Reservation>();

            foreach (Reservation reservation in _reservations)
            {
                // Check if a value was given for reservationCode
                if (!string.IsNullOrWhiteSpace(reservationCode))
                {
                    // Check if reservation matches given code. Skip this reservation otherwise.
                    if (reservationCode != reservation.ReservationCode)
                    {
                        continue;
                    }
                }

                // Check if a value was given for airline
                if (!string.IsNullOrWhiteSpace(airline))
                {
                    // Check if reservation airline matches given airline. Skip this reservation otherwise.
                    if (airline != reservation.Airline)
                    {
                        continue;
                    }
                }

                // Check if a value was given for travellerName
                if (!string.IsNullOrWhiteSpace(travellerName))
                {
                    // Check if traveller name matches given name. Skip this reservation otherwise.
                    if (travellerName != reservation.Name)
                    {
                        continue;
                    }
                }

                // Add value to list if we get to this point.
                foundReservations.Add(reservation);
            }

            return foundReservations;
        }


        // formatForFile formats a Reservation object for adding to the Reservation CSV file.
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
