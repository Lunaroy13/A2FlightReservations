using System.Collections.ObjectModel;
using A2FlightReservations.Models;

namespace A2FlightReservations.Views;

public partial class Flights : ContentPage
{
	public Flights()
	{
		InitializeComponent();


	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
       
        var flights = new List<Flight>(FlightsManager.GetFlights());
        pickerFlight.ItemsSource = flights;
    }

    private void HomePageButton(object sender, EventArgs e)
    {
        // This is the method attached to the "Return to homepage" button
        Shell.Current.GoToAsync("..");
    }

    private void ChosenFlight(object sender, EventArgs e)
    { 
        // takes strings from entry feilds and passes them into SearchFlights
        var flights = new List<Flight>(FlightsManager.SearchFlights(entryFrom.Text,entryTo.Text,entryDay.Text));
        pickerFlight.ItemsSource = flights;
    }

    private void ReservationButton(object sender, EventArgs e)
    {
        // This is the method attached to them clicking on "Reserve" after putting in the remaining details.
        Flight selectedFlight = (Flight)pickerFlight.SelectedItem;
        Reservation newReservation = ReservationManager.makeReservation(selectedFlight, entryName.Text, entryCitizenship.Text);
        labelCode.Text = newReservation.ReservationCode;
    }

    private void pickerFlight_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (pickerFlight.SelectedItem != null)
        {
            Flight selectedFlight = (Flight)pickerFlight.SelectedItem;
            //When flight is selected from picker
            labelFlightCode.Text = selectedFlight.FlightCode;
            labelAirline.Text = selectedFlight.Airline;
            labelDay.Text = selectedFlight.Day;
            labelTime.Text = selectedFlight.Time;
            labelCost.Text = selectedFlight.PricePerSeat.ToString();

        }
    }
}