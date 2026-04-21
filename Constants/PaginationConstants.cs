namespace PeakLogix.PickProApi.Common.Constants
{
    /// <summary>
    /// Constants for pagination parameters
    /// </summary>
    public static class PaginationConstants
    {
        /// <summary>
        /// Default starting index for pagination
        /// </summary>
        public const int DefaultStart = 0;

        /// <summary>
        /// Default page size for pagination
        /// </summary>
        public const int DefaultSize = 50;

        /// <summary>
        /// Maximum allowed page size to prevent memory issues with large result sets
        /// </summary>
        public const int MaxPageSize = 1000;
    }    
}
