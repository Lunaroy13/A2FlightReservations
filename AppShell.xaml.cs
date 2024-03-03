using A2FlightReservations.Views;

namespace A2FlightReservations
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));

            Routing.RegisterRoute(nameof(Flights), typeof(Flights));

            Routing.RegisterRoute(nameof(Reservations), typeof(Reservations));
        }
    }
}
