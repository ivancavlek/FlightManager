using Acme.FlightManager.Common;
using System;

namespace Acme.FlightManager.Text;

public static class GeneralFlightManagerMessage
{
    public static string Invalid(string name) =>
        string.Format(FlightManagerMessage.Invalid, name);

    public static string AirplaneTypeAbbreviationText(this AirplaneTypeAbbreviation airplaneTypeAbbreviation) =>
        airplaneTypeAbbreviation switch
        {
            AirplaneTypeAbbreviation.A320 => FlightManagerMessage.A320,
            AirplaneTypeAbbreviation.A320neo => FlightManagerMessage.A320neo,
            AirplaneTypeAbbreviation.A380 => FlightManagerMessage.A380,
            AirplaneTypeAbbreviation.Boeing7478 => FlightManagerMessage.Boeing7478,
            _ => throw new NotImplementedException()
        };
}