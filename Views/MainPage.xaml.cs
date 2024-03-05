namespace A2FlightReservations.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }


    private void BookFlight(object sender, EventArgs e)
    {
        // This is the "Book your flight here!" button
        Shell.Current.GoToAsync(nameof(Flights));
    }

    private void ViewReservations(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync(nameof(Reservations));
    }
}
