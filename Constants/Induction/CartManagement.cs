namespace PeakLogix.PickProApi.Common.Constants.Induction
{
    public static class CartManagement
    {
        public static class ResponseMessages
        {
            public const string Available = "Available";
            public const string NotAvailable = "NotAvailable";
            public const string UnableToGetCart = "Unable to get Cart";
            public const string InternalServerError = "Internal server error occurred";
            public const string ToteRemovedSingle = "{0} has been removed from Cart";
            public const string ToteRemovedMultiple = "All totes have been removed.";
            public const string NoTotesRemoved = "No totes were removed";
            public const string CartDetailsRetrieved = "Cart details retrieved successfully";
            public const string RemoveAllSuccessful = "All totes removed and cart cancelled successfully";
            public const string RemoveAllFailed = "Failed to remove totes.";
            public const string CartAvailableButUpdateFailed = "Cart status available but unable to update it due to API failure";
            public const string DeleteCartFailed = "Failed to delete cart.";
            public const string AddCartFailed = "Failed to add cart.";
            public const string CartNotFound = "Cart ID does not exist.";
            public const string CartInUse = "Cart already in use / Cart not available for induction.";
            public const string DeleteCartPermissionDenied = "You do not have permission to delete carts.";
            public const string AddCartPermissionDenied = "You do not have permission to add carts.";
            public const string ActivateDeactivateCartPermissionDenied = "You do not have permission to activate/deactivate carts.";
            public const string ActivateCartFailed = "Failed to activate cart.";
            public const string DeactivateCartFailed = "Failed to deactivate cart.";
            public const string CartActivated = "Cart has been activated successfully.";
            public const string CartDeactivated = "Cart has been deactivated successfully.";
        }

        public static class LogMessages
        {
            public const string ValidationError = "Error validating cart ID: {CartId}";
            public const string RemoveToteError = "Error removing totes from cart: {CartId}";
            public const string ValidateToteError = "Error validating tote ID: {ToteId} for cart: {CartId}";
            public const string ViewDetailsError = "Error retrieving cart details: {CartId}";
            public const string RemoveAllError = "Error removing all totes and cancelling cart: {CartId}";
            public const string GetCartError = "Error retrieving cart: {CartId}";
            public const string UpdateCartStatusFailed = "Failed to update cart status for cart: {CartId}. Error: {Error}";
            public const string UpdateCartStatusError = "Error updating cart status for cart: {CartId}";
            public const string GetCartListError = "Error retrieving cart list";
            public const string CompleteCartError = "Error completing cart: {CartId}";
            public const string DeleteCartError = "Error deleting cart: {CartId}";
            public const string AddCartError = "Error adding cart: {CartId}";
            public const string CheckCartExistsError = "Error checking cart existence: {CartId}";
            public const string CartValidationStatusUpdateFailed = "Cart validation failed - unable to update cart status for: {CartId}";
            public const string RemoveCartStatusUpdateFailed = "Remove cart failed - unable to update cart status for: {CartId}";
            public const string UpdateToteStatusFailed = "Failed to update tote status for tote: {PickTote}. Error: {Error}";
            public const string UpdateToteStatusError = "Error updating tote status for tote: {PickTote}";
            public const string ValidateToteStatusUpdateFailed = "Tote validation failed - unable to update tote status for: {ToteId}";
            public const string RemoveToteStatusUpdateFailed = "Remove tote failed - unable to update tote status for: {ToteId}";
            public const string AddSortBarError = "Error adding sort bar: {CartBuilder}";
            public const string EventLogWriteError = "Error to log in event: {EventName}";
            public const string UpdateCartStatusActiveInactiveError = "Error updating cart active/inactive status: {CartId}";
        }

        public static class BatchIDType
        {
            public const string TransType = "Put Away";
        }
    }
}
