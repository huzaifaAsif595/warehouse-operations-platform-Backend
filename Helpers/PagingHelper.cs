using PeakLogix.PickProApi.Common.DTOs;

namespace PeakLogix.PickProApi.Common.Helpers
{
    public static class PagingHelper
    {
        public static PagingInfo CalculatePagingInfo<T>(List<T> objList, PagingRequest pagingRequest, int totalItems)
        {
            var pagingInfo = new PagingInfo()
            {
                TotalCount = totalItems,
                Page = pagingRequest.SelectedPage,
                TotalPages = (int)Math.Ceiling(totalItems / (double)pagingRequest.PageSize),
                HasNext = pagingRequest.SelectedPage < (int)Math.Ceiling(totalItems / (double)pagingRequest.PageSize),
                HasPrevious = pagingRequest.SelectedPage > 1
            };
            return pagingInfo;
        }
    }
}

