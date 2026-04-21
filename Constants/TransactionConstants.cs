namespace PeakLogix.PickProApi.Common.Constants
{
    public static class TransactionConstants
    {
        public static readonly string TransactionTypeCycleCount = "Count";

        public const string InductByOrderNumber = "Order Number";
        public const string InductByBatchId = "Batch ID";
        public const string InductByToteId = "Tote ID";

        public const string PickTypePickAndPass = "Pick and Pass";

        public static readonly string CycleCountOrderNumberPrefix = "CC";
        public static readonly string CycleCountImporFileName = "Cycle Count Audit";
        public static readonly string OrderByDesc = "DESC";
        public static readonly string All = "All";
        public static readonly string NonReplen = "NonReplen";
        public static readonly string Replen = "Replen";
        public static readonly string Replenishment = "Replenishment";
        public static readonly string SupplierItemIDNotFound = "Supplier Item ID not found";
    }
}
