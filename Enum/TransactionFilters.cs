using PeakLogix.EntityFramework.Entities.PickProSD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PeakLogix.PickProApi.Common.Enum;

public enum TransactionFilterProperty
{
    OrderNumber,
    ImportDate,
    Priority,
    RequiredDate
}

public static class TransactionFilterPropertyExtensions
{
    public static string ToPropertyName(this TransactionFilterProperty property) =>
        property switch
        {
            TransactionFilterProperty.OrderNumber => nameof(OpenTransaction.OrderNumber),
            TransactionFilterProperty.ImportDate => nameof(OpenTransaction.ImportDate),
            TransactionFilterProperty.Priority => nameof(OpenTransaction.Priority),
            TransactionFilterProperty.RequiredDate => nameof(OpenTransaction.RequiredDate),
            _ => throw new ArgumentOutOfRangeException(nameof(property), property, null)
        };
}