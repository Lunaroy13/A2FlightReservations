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
        public string DisplayText => $"{FlightCode}, {Airline}, {Departing}, {Arriving}, {Day}, {Time}, {AvailableSeats}, {PricePerSeat}";

    }
}
