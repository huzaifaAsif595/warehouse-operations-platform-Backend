namespace PeakLogix.PickProApi.Common.Enum
{
    public static class CartStates
    {
        public enum CartState
        {
            Available,
            Inducting,
            Inducted,
            InProgress,
            Cancelled,
            Reset,
            Complete,
            Inactive,
            NotFound
        }

        public enum ToteState
        {
            InBuffer,
            UnknownLocation
        }

        public static string ToStatusString(this CartState state)
        {
            return state switch
            {
                CartState.Available => "Available",
                CartState.Inducting => "Inducting",
                CartState.Inducted => "Inducted",
                CartState.InProgress => "In Progress",
                CartState.Cancelled => "Cancelled",
                CartState.Reset => "Reset",
                CartState.Complete => "Complete",
                CartState.Inactive => "Inactive",
                CartState.NotFound => "NotFound",
                _ => "Unknown State"
            };
        }

        public static CartState FromString(string state)
        {
            return state switch
            {
                "Available" => CartState.Available,
                "Inducting" => CartState.Inducting,
                "Inducted" => CartState.Inducted,
                "In Progress" => CartState.InProgress,
                "Cancelled" => CartState.Cancelled,
                "Reset" => CartState.Reset,
                "Complete" => CartState.Complete,
                "In Active" => CartState.Inactive,
                "NotFound" => CartState.NotFound,
                _ => throw new ArgumentException($"Invalid cart state string: {state}")
            };
        }

        public static string ToStatusString(this ToteState state)
        {
            return state switch
            {
                ToteState.InBuffer => "In Buffer",
                ToteState.UnknownLocation => "Unknown Location",
                _ => "Unknown State"
            };
        }

        public static ToteState ToteStateFromString(string state)
        {
            return state switch
            {
                "In Buffer" => ToteState.InBuffer,
                "Unknown Location" => ToteState.UnknownLocation,
                _ => throw new ArgumentException($"Invalid tote state string: {state}")
            };
        }
    }
}
