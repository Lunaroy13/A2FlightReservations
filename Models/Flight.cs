﻿// 2024-03-06
// Group 1 -> Adam Cunard, Simon Luna, Tyler Meekel, Sunhee Ku 
// Flight.cs
// Takes in no inputs.
// Class for Flight objects
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A2FlightReservations.Models
{
    public class Flight
    {
        public string FlightCode { get; set; }
        public string Airline { get; set; }
        public string Departing { get; set; }
        public string Arriving { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public int AvailableSeats { get; set; }
        public double PricePerSeat { get; set; }
        public string AvailableSeatsAsString => AvailableSeats.ToString();
        public string PricePerSeatsAsString => PricePerSeat.ToString();
        public string DisplayText => ToString();

        public override string ToString()
        {
            return $"{FlightCode}, {Airline}, {Departing}, {Arriving}, {Day}, {Time}, {AvailableSeats}, {PricePerSeat}";
        }

    }
}
