namespace PeakLogix.PickProApi.Common.Enum.Consolidation
{
    /// <summary>
    /// Consolidation-related state enums and extension methods
    /// </summary>
    public static class ConsolidationStates
    {
        public enum RouteStatus
        {
            NotFound,
            ConsolidationNotStarted,
            InConsolidation,
            ReadyForRelease,
            ReleaseRequested,
            ActiveRelease,
            InShipping
        }

        public enum ConsolidationStatus
        {
            NotFound,
            Initialized,
            InductionStarted,
            ConsolidationComplete
        }

        public enum PickOrderStatus
        {
            NotFound,
            Created,
            Released,
            PickingStarted,
            PickingComplete,
            Cancelled
        }

        public enum PickHeaderStatus
        {
            NotFound,
            InProgress,
            Complete,
            Cancelled
        }

        // Extension methods for status conversions
        public static string ToStatusString(this ConsolidationStatus status)
        {
            return status switch
            {
                ConsolidationStatus.Initialized => "Initialized",
                ConsolidationStatus.InductionStarted => "Induction Started",
                ConsolidationStatus.ConsolidationComplete => "Consolidation Complete",
                _ => "Not Found"
            };
        }

        public static ConsolidationStatus ConsolidationStatusFromString(string consolidationStatus)
        {
            return consolidationStatus switch
            {
                "Initialized" => ConsolidationStatus.Initialized,
                "Induction Started" => ConsolidationStatus.InductionStarted,
                "Consolidation Complete" => ConsolidationStatus.ConsolidationComplete,
                _ => ConsolidationStatus.NotFound
            };
        }

        public static string ToStatusString(this RouteStatus status)
        {
            return status switch
            {
                RouteStatus.ConsolidationNotStarted => "Consolidation Not Started",
                RouteStatus.InConsolidation => "In Consolidation",
                RouteStatus.ReadyForRelease => "Ready For Release",
                RouteStatus.ReleaseRequested => "Release Requested",
                RouteStatus.ActiveRelease => "Active Release",
                RouteStatus.InShipping => "In Shipping",
                _ => "Not Found"
            };
        }

        public static RouteStatus RouteStatusFromString(string routeStatus)
        {
            return routeStatus switch
            {
                "Consolidation Not Started" => RouteStatus.ConsolidationNotStarted,
                "In Consolidation" => RouteStatus.InConsolidation,
                "Ready For Release" => RouteStatus.ReadyForRelease,
                "Release Requested" => RouteStatus.ReleaseRequested,
                "Active Release" => RouteStatus.ActiveRelease,
                "In Shipping" => RouteStatus.InShipping,
                _ => RouteStatus.NotFound
            };
        }

        public static string ToStatusString(this PickHeaderStatus status)
        {
            return status switch
            {
                PickHeaderStatus.InProgress => "In Progress",
                PickHeaderStatus.Complete => "Complete",
                PickHeaderStatus.Cancelled => "Cancelled",
                _ => "Not Found"
            };
        }

        public static PickHeaderStatus PickHeaderStatusFromString(string pickHeaderStatus)
        {
            return pickHeaderStatus switch
            {
                "In Progress" => PickHeaderStatus.InProgress,
                "Complete" => PickHeaderStatus.Complete,
                "Cancelled" => PickHeaderStatus.Cancelled,
                _ => PickHeaderStatus.NotFound
            };
        }

        public static string ToStatusString(this PickOrderStatus status)
        {
            return status switch
            {
                PickOrderStatus.Created => "Created",
                PickOrderStatus.Released => "Released",
                PickOrderStatus.PickingStarted => "Picking Started",
                PickOrderStatus.PickingComplete => "Picking Complete",
                PickOrderStatus.Cancelled => "Cancelled",
                _ => "Not Found"
            };
        }

        public static PickOrderStatus PickOrderStatusFromString(string pickOrdersStatus)
        {
            return pickOrdersStatus switch
            {
                "Created" => PickOrderStatus.Created,
                "Released" => PickOrderStatus.Released,
                "Picking Started" => PickOrderStatus.PickingStarted,
                "Picking Complete" => PickOrderStatus.PickingComplete,
                "Cancelled" => PickOrderStatus.Cancelled,
                _ => PickOrderStatus.NotFound
            };
        }
    }
}

