using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace A2FlightReservations.Models
{
    public static class ReservationManager
    {

        public static string RESERVATIONCSVPATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\..\..\CSVFiles\reservations.csv");

        //Creates a list of flight objects from flights.csv
        static List<Reservation> reservations = new List<Reservation>(PopulateReservations());

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

                    Reservation newReservation = new Reservation { FlightCode = flightCode, Airline = airline, Day = day, Time = time, Cost = cost, Name = name, Citizenship = citizenship};
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

        public static void MakeReservation(Flight flight, string name, string citizenship)
        {

            if (name.Trim() == "")
            {
                //TODO throw exception
            }
            else if (citizenship.Trim() == "")
            {
                //TODO throw exception
            }
            else if (flight.AvailableSeats > 0)
            {
                flight.AvailableSeats--;
                Reservation newReservation = new Reservation { FlightCode = flight.FlightCode, Airline = flight.Airline, Day = flight.Day, Time = flight.Time, Cost = flight.PricePerSeat, Name = name, Citizenship = citizenship };

                try
                {
                    using (StreamWriter sw = File.AppendText(RESERVATIONCSVPATH))
                    {
                        sw.WriteLine(formatForFile(newReservation));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
            }
            else
            {
                //TODO throw exception
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
