namespace PeakLogix.PickProApi.Common.Enum
{
    public static class SteStates
    {

        public enum ImportCompareLineStatus
        {
            Imported,
            Processed,
            ProcessingError
        }

        public enum CompareHeaderState 
        { 
            Created, //Imported
            Released, //Ready for count
            Started, //Count in progress
            CompareCompleted, //Counted
            CountSubmitted, //Cycle count transaction created
            Unknown

        }

        public enum CompareLineState
        {
            Created, //Imported
            Released, //Ready for count
            Processed, //Counted
            Submitted, //Cycle count transactin created
            Cancelled, //wont count
            ProcessingError, //error during processing
            Unknown
        }

        public enum ImportTypeState
        {
            Active,
            Inactive
        }


        public static string ToImportCompareLineStatusString(this ImportCompareLineStatus status)
        {
            return status switch
            {
                ImportCompareLineStatus.Imported => "Imported",
                ImportCompareLineStatus.Processed => "Processed",
                ImportCompareLineStatus.ProcessingError => "Processing Error",
                _ => "Unknown Status"
            };
        }

        public static string ToStatusString(this CompareLineState state)
        {
            return state switch
            {
                CompareLineState.Created => "Created",
                CompareLineState.Released => "Released",
                CompareLineState.Processed => "Processed",
                CompareLineState.Submitted => "Submitted",
                CompareLineState.Cancelled => "Cancelled",
                CompareLineState.ProcessingError => "Processing Error",
                CompareLineState.Unknown => "Unknown State",
                _ => "Unknown State"
            };
        }

        public static CompareLineState CompareLineStateFromString(string state)
        {
            return state switch
            {
                "Created" => CompareLineState.Created,
                "Released" => CompareLineState.Released,
                "Processed" => CompareLineState.Processed,
                "Submitted" => CompareLineState.Submitted,
                "Cancelled" => CompareLineState.Cancelled,
                "Processing Error" => CompareLineState.ProcessingError,
                "Unknown State" => CompareLineState.Unknown,
                _ => throw new ArgumentException($"Invalid state string: {state}")
            };
        }

        public static string ToHeaderStatusString(this CompareHeaderState state)
        {
            return state switch
            {
                CompareHeaderState.Created => "Created",
                CompareHeaderState.Released => "Released",
                CompareHeaderState.Started => "Started",
                CompareHeaderState.CompareCompleted => "Compare Completed",
                CompareHeaderState.CountSubmitted => "Count Submitted",
                CompareHeaderState.Unknown => "Unknown State",
                _ => "Unknown State"
            };
        }

        public static CompareHeaderState FromHeaderString(string state)
        {
            return state switch
            {
                "Created" => CompareHeaderState.Created,
                "Released" => CompareHeaderState.Released,
                "Started" => CompareHeaderState.Started,
                "Compare Completed" => CompareHeaderState.CompareCompleted,
                "Count Submitted" => CompareHeaderState.CountSubmitted,
                "Unknown State" => CompareHeaderState.Unknown,
                _ => throw new ArgumentException($"Invalid state string: {state}")
            };
        }

        public static string ToImportTypeStatusString(this ImportTypeState state)
        {
            return state switch
            {
                ImportTypeState.Active => "Active",
                ImportTypeState.Inactive => "Inactive",
                _ => "Unknown State"
            };
        }
    }
}
