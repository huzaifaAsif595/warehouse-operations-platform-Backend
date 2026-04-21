namespace PeakLogix.PickProApi.Common.Enum;

public enum TransactionType
{
    Pick,
    PutAway,
    Count,
    Adjustment,
    LocationChange,
    Complete
}

public static class TransactionTypeExtensions
{
    public static string ToStringValue(this TransactionType type)
    {
        return type switch
        {
            TransactionType.Pick => "Pick",
            TransactionType.PutAway => "Put Away",
            TransactionType.Count => "Count",
            TransactionType.Adjustment => "Adjustment", 
            TransactionType.LocationChange => "Location Change",
            TransactionType.Complete => "Complete",
            _ => type.ToString()
        };
    }

    public static TransactionType FromString(string value)
    {
        return value switch
        {
            "Pick" => TransactionType.Pick,
            "Put Away" => TransactionType.PutAway,
            "Count" => TransactionType.Count,
            "Adjustment" => TransactionType.Adjustment,
            "Location Change" => TransactionType.LocationChange,
            "Complete" => TransactionType.Complete,
            _ => throw new ArgumentException($"Invalid transaction type: {value}")
        };
    }
}
