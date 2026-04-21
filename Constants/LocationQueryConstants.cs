namespace PeakLogix.PickProApi.Common.Constants;
public static class LocationQueryConstants
{
    // Empty string constant for null coalescing in location concatenation
    public const string EmptyString = "";

    // Default serial number 
    public const string DefaultSerialNumber = "0";

    // Default lot number
    public const string DefaultLotNumber = "0";

    // Standard datetime format for expiration dates
    public const string DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

    // Maximum number of location results to return in typeahead queries
    public const int TopLocationResults = 20;
}

