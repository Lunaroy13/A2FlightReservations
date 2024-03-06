using System.Collections.ObjectModel;
using A2FlightReservations.Models;

namespace A2FlightReservations.Views;

public partial class Reservations : ContentPage
{
	public Reservations()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
        base.OnAppearing();

        List<Reservation> reservations = new(ReservationManager.GetReservations());
        pickerReservation.ItemsSource = reservations;
    }

    private void FindReservation(object sender, EventArgs e)
    {
        List<Reservation> reservations = ReservationManager.findReservation(entryReservationCode.Text, entryReservationAirline.Text, entryReservationName.Text); 
        pickerReservation.ItemsSource = reservations;
    }
}