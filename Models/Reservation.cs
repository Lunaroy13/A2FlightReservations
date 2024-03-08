// 2024-03-06
// Group 1 -> Adam Cunard, Simon Luna, Tyler Meekel, Sunhee Ku 
// Reservation.cs
// Takes in no inputs.
// Class for reservation objects

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2FlightReservations.Models
{
    public class Reservation
    {
        public string ReservationCode { get; set; }
        public string FlightCode { get; set; }
        public string Airline { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public double Cost { get; set; }
        public string Name { get; set; }
        public string Citizenship { get; set; }

        public string DisplayInfo => ToString();

        public override string ToString()
        {
            return $"{ReservationCode}, {FlightCode}, {Airline}, {Day}, {Time}, {Cost}, {Name}, {Citizenship}";
        }

    }
}
