using PeakLogix.PickProApi.Interfaces;
using PeakLogix.PickProApi.Models;
using System.Data;

namespace PeakLogix.PickProApi.Repositories;

public class OMMenuRepository(IDatabaseConfig config) : IOMMenuRepository
{
    public OrderManagerMenuModel SelectOMCountData(ClaimObject request)
    {
        return SelectOMCountData();
    }

    private OrderManagerMenuModel SelectOMCountData()
    {
        var response = new OrderManagerMenuModel();

        DataSet dataSet = SqlHelper.ExecuteReader(config.SDConnectionString, "selOMCountInfo");
        if (UtilityHelper.IsDataSetValid(dataSet))
        {
            response.OpenPicks = UtilityHelper.IsValidInt(dataSet.Tables[0].Rows[0][0]);
            response.CompPick = UtilityHelper.IsValidInt(dataSet.Tables[1].Rows[0][0]);
            response.OpenPuts = UtilityHelper.IsValidInt(dataSet.Tables[2].Rows[0][0]);
            response.CompPuts = UtilityHelper.IsValidInt(dataSet.Tables[3].Rows[0][0]);
            response.OpenCounts = UtilityHelper.IsValidInt(dataSet.Tables[4].Rows[0][0]);
            response.CompCounts = UtilityHelper.IsValidInt(dataSet.Tables[5].Rows[0][0]);
            response.CompAdjust = UtilityHelper.IsValidInt(dataSet.Tables[6].Rows[0][0]);
            response.CompLocChange = UtilityHelper.IsValidInt(dataSet.Tables[7].Rows[0][0]);
            response.ReprocCount = UtilityHelper.IsValidInt(dataSet.Tables[8].Rows[0][0]);
        }

        return response;
    }
}
