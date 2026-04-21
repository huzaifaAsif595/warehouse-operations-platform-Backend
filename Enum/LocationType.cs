using System.ComponentModel;

namespace PeakLogix.PickProApi.Common.Enum;

// Location types for warehouse storage zones
public enum LocationType
{
    [Description("Carousel")]
    Carousel,

    [Description("Bulk")]
    Bulk,

    [Description("Carton Flow")]
    CartonFlow
}