namespace PeakLogix.PickProApi.Common.Constants.BulkTransactions;

public static class UserField
{
    public const string USER_FIELD_1_DISPLAY = "User Field 1";
    public const string USER_FIELD_2_DISPLAY = "User Field 2";
    public const string USER_FIELD_3_DISPLAY = "User Field 3";
    public const string USER_FIELD_4_DISPLAY = "User Field 4";
    public const string USER_FIELD_5_DISPLAY = "User Field 5";
    public const string USER_FIELD_6_DISPLAY = "User Field 6";
    public const string USER_FIELD_7_DISPLAY = "User Field 7";
    public const string USER_FIELD_8_DISPLAY = "User Field 8";
    public const string USER_FIELD_9_DISPLAY = "User Field 9";
    public const string USER_FIELD_10_DISPLAY = "User Field 10";
}

public static class UserFieldProperty
{
    public const string USER_FIELD_1_PROPERTY = "UserField1";
    public const string USER_FIELD_2_PROPERTY = "UserField2";
    public const string USER_FIELD_3_PROPERTY = "UserField3";
    public const string USER_FIELD_4_PROPERTY = "UserField4";
    public const string USER_FIELD_5_PROPERTY = "UserField5";
    public const string USER_FIELD_6_PROPERTY = "UserField6";
    public const string USER_FIELD_7_PROPERTY = "UserField7";
    public const string USER_FIELD_8_PROPERTY = "UserField8";
    public const string USER_FIELD_9_PROPERTY = "UserField9";
    public const string USER_FIELD_10_PROPERTY = "UserField10";
}

public enum OrderSortType
{
    ImportDateAndOrderNumber,
    ImportDateAndPriority,
    ImportFileSequence,
    OrderNumberSequence,
    PriorityAndImportDate,
    PriorityAndRequiredDate,
    PriorityDescendingAndImportDate,
    PriorityDescendingAndRequiredDate,
    RequiredDateAndPriority
}

public static class OrderSortTypeExtensions
{
    public static string ToStringValue(this OrderSortType type)
    {
        return type switch
        {
            OrderSortType.ImportDateAndOrderNumber => "Import Date and Order Number",
            OrderSortType.ImportDateAndPriority => "Import Date and Priority",
            OrderSortType.ImportFileSequence => "Import File Sequence",
            OrderSortType.OrderNumberSequence => "Order Number Sequence",
            OrderSortType.PriorityAndImportDate => "Priority and Import Date",
            OrderSortType.PriorityAndRequiredDate => "Priority and Required Date",
            OrderSortType.PriorityDescendingAndImportDate => "Priority Descending and Import Date",
            OrderSortType.PriorityDescendingAndRequiredDate => "Priority Descending and Required Date",
            OrderSortType.RequiredDateAndPriority => "Required Date and Priority",
            _ => type.ToString()
        };
    }

    public static OrderSortType FromString(string value)
    {
        return value switch
        {
            "Import Date and Order Number" => OrderSortType.ImportDateAndOrderNumber,
            "Import Date and Priority" => OrderSortType.ImportDateAndPriority,
            "Import File Sequence" => OrderSortType.ImportFileSequence,
            "Order Number Sequence" => OrderSortType.OrderNumberSequence,
            "Priority and Import Date" => OrderSortType.PriorityAndImportDate,
            "Priority and Required Date" => OrderSortType.PriorityAndRequiredDate,
            "Priority Descending and Import Date" => OrderSortType.PriorityDescendingAndImportDate,
            "Priority Descending and Required Date" => OrderSortType.PriorityDescendingAndRequiredDate,
            "Required Date and Priority" => OrderSortType.RequiredDateAndPriority,
            _ => throw new ArgumentException($"Invalid order sort type: {value}")
        };
    }
}


public static class BulkType
{
    public const string PICK = "pick";
    public const string PUTAWAY = "put away";
    public const string COUNT = "count";
    public const string CYCLECOUNT = "CycleCount";
    public const string ADJUSTMENT = "Adjustment";
}

public static class Messages
{
    public const string AdjustmentNotes = "Inventory adjusted from Bulk Transactions screen. Operator short picked this order because no quantity remains after this Pick at this location.";
    public const string AdjustmentNotesCount = "Location Quantity adjusted from Bulk Cycle Count";
}

public static class HttpResponseMessage
{
    public const string INTERNALSERVERERROR = "Internal Server Error";
    public const string ORDER_NUMBERS_ARE_REQUIRED = "Order numbers are required.";
    public const string ALL_ORDER_NUMBERS_ALREADY_EXISTS = "All provided order numbers already exist";
    public const string INVALID_OPERATION = "Invalid Operation";
    public const string BUSINESS_RULE_VIOLATION = "Business Rule Violation";
}

public static class BulkPickValidationErrors
{
    public const string TRANSACTION_NOT_FOUND = "Transaction with ID {0} not found.";
    public const string INVENTORY_MAP_NOT_FOUND = "Inventory map not found for transaction {0}.";
    public const string QUANTITY_EXCEEDS_AVAILABLE = "Completed Quantity is greater than the available location quantity";
}