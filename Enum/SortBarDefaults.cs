namespace PeakLogix.PickProApi.Common.Enum
{
    public static class SortBarDefaults
    {
        public enum SortBarType
        {
            PodSortBar,
            UpperLeftToLowerRight,
            Available,
            LogixPro,
            DefaultLevelsCount,
            Stationary
        }

        public static string ToStringValue(this SortBarType type)
        {
            return type switch
            {
                SortBarType.PodSortBar => "POD Sort Bar",
                SortBarType.UpperLeftToLowerRight => "Upper Left to Lower Right",
                SortBarType.Available => "Available",
                SortBarType.LogixPro => "LogixPro",
                SortBarType.DefaultLevelsCount => "1",
                SortBarType.Stationary => "Stationary",
                _ => type.ToString()
            };
        }
    }
}
