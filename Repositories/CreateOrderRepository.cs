using PeakLogix.PickProApi.Interfaces;
using PeakLogix.PickProApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace PeakLogix.PickProApi.Repositories;

public class CreateOrderRepository(IDatabaseConfig config) : ICreateOrderRepository
{
    public List<string> SelectWarehouses()
    {
        var response = new List<string>();

        DataSet dataSet = SqlHelper.ExecuteReader(config.SDConnectionString, "selWarehouses");
        if (UtilityHelper.IsDataSetValid(dataSet))
        {
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                response.Add(UtilityHelper.IsValidString(dr["Warehouse"]));
            }
        }

        return response;

    }

    public List<string> SelCreateOrdersTA(CreateOrderTARequest request)
    {
        var response = new List<string>();
        var param = new SqlParameter[]{
                        new("@OrderNumber", request.OrderNumber)
            };

        DataSet dataSet = SqlHelper.ExecuteReader(config.SDConnectionString, "selOTPendTA", param);
        if (UtilityHelper.IsDataSetValid(dataSet))
        {
            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {
                response.Add(UtilityHelper.IsValidString(dr["Order Number"]));
            }
        }

        return response;

    }
}
