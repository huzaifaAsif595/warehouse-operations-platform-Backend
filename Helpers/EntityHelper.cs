using PeakLogix.EntityFramework.Entities.PickProSD;
using System;

namespace PeakLogix.PickProApi.Common.Helpers
{
    /// <summary>
    /// Provides utility methods for entity operations such as cloning and deep copying
    /// </summary>
    public static class EntityHelper
    {
        /// <summary>
        /// Creates a deep copy of a LocationZone entity for audit logging purposes
        /// </summary>
        /// <param name="source">The source LocationZone to clone</param>
        /// <returns>A new LocationZone instance with all properties copied from the source</returns>
        /// <exception cref="ArgumentNullException">Thrown when source is null</exception>
        public static LocationZone CloneLocationZone(LocationZone source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return new LocationZone
            {
                Zone = source.Zone,
                Carousel = source.Carousel,
                CartonFlow = source.CartonFlow,
                IncludeCfCarouselPick = source.IncludeCfCarouselPick,
                ReplenishmentZone = source.ReplenishmentZone,
                StagingZone = source.StagingZone,
                IncludeInAutoBatch = source.IncludeInAutoBatch,
                IncludeInTransactions = source.IncludeInTransactions,
                DynamicWarehouse = source.DynamicWarehouse,
                Allocable = source.Allocable,
                LocationName = source.LocationName,
                ParentZone = source.ParentZone,
                Sequence = source.Sequence,
                Label1 = source.Label1,
                Label2 = source.Label2,
                Label3 = source.Label3,
                Label4 = source.Label4,
                CaseLabel = source.CaseLabel,
                KanbanZone = source.KanbanZone,
                KanbanReplenishmentZone = source.KanbanReplenishmentZone,
                AllowWholeClearLocation = source.AllowWholeClearLocation
            };
        }
    }
}
