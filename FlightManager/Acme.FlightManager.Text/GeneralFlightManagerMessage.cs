namespace Acme.FlightManager.Text;

public static class GeneralFlightManagerMessage
{
    public static string Invalid(string name) =>
        string.Format(FlightManagerMessage.Invalid, name);
}