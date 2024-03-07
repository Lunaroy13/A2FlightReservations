using System.Collections.ObjectModel;
using A2FlightReservations.Models;
using Microsoft.Maui.Controls.Compatibility.Platform;

namespace A2FlightReservations.Views;

public partial class Flights : ContentPage
{
	public Flights()
	{
		InitializeComponent();
	}

    //TEST CODE
    //UNCOMMENT TO DISPLAY ALL FLIGHTS IN PICKER. FOR DEBUG ONLY
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
        Reservation? newReservation = ReservationManager.makeReservation(selectedFlight, entryName.Text, entryCitizenship.Text);
        if (newReservation != null)
        {
            FlightsManager.bookFlight(selectedFlight);
            reservationCodeDisplay.IsVisible = true;
            confirmationMessage.IsVisible = true;
            errorMessage.IsVisible = false;
            labelCode.Text = newReservation.ReservationCode;
        }
        else 
        {
            if (selectedFlight == null)
            {
                errorMessage.Text = "Please choose a flight in the section above before attempting to reserve a flight.";
            }
            else if (string.IsNullOrWhiteSpace(entryCitizenship.Text) || string.IsNullOrEmpty(entryName.Text))
            {
                errorMessage.Text = "Please fill out all the required fields (Name, Citizenship).";
            }
            else if (selectedFlight.AvailableSeats == 0)
            {
                errorMessage.Text = "There are no seats available in this flight, please select another flight.";
            }
            errorMessage.IsVisible = true;
            reservationCodeDisplay.IsVisible = false;
            confirmationMessage.IsVisible = false;
        }
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