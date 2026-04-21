namespace PeakLogix.PickProApi.Common.Enum;

public enum InductionType
{
    OrderNumber,
    BatchId,
    ToteId
}

public static class InductionTypeExtensions
{
    public static string ToStringValue(this InductionType type)
    {
        return type switch
        {
            InductionType.OrderNumber => "Order Number",
            InductionType.BatchId => "Batch ID",
            InductionType.ToteId => "Tote ID",
            _ => type.ToString()
        };
    }

    public static InductionType FromString(string value)
    {
        return value switch
        {
            "Order Number" => InductionType.OrderNumber,
            "Batch ID" => InductionType.BatchId,
            "Tote ID" => InductionType.ToteId,
            _ => throw new ArgumentException($"Invalid induction type: {value}")
        };
    }
}
