namespace A2FlightReservations.Views;

public partial class Flights : ContentPage
{
	public Flights()
	{
		InitializeComponent();
	}

    private void HomePageButton(object sender, EventArgs e)
    {
        // This is the method attached to the "Return to homepage" button
        Shell.Current.GoToAsync("..");
    }

    private void ChosenFlight(object sender, EventArgs e)
    {
        // This is the method attached to them clicking on "Find Flights" after they entered the flight details.
    }

    private void ReservationButton(object sender, EventArgs e)
    {
        // This is the method attached to them clicking on "Reserve" after putting in the remaining details.
    }
}