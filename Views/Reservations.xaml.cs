namespace A2FlightReservations.Views;

public partial class Reservations : ContentPage
{
	public Reservations()
	{
		InitializeComponent();
	}

    private void HomePageButton(object sender, EventArgs e)
    {
        // This is the method attached to the "Return to book flights page" button.
        Shell.Current.GoToAsync("..");
    }

    private void FindReservation(object sender, EventArgs e)
    {
		// This method is attached to the find reservations button, it should fill the array of options that the user can pick from.
    }
}