using PeakLogix.DAL.Interfaces;
using PeakLogix.PickProApi.Interfaces;
using PeakLogix.PickProApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace PeakLogix.PickProApi.Repositories;

public class OMPreferenceRepository(
    IDatabaseConfig config,
    IClaimData claims) : IOMPreferenceRepository
{
    public OrderManagerPreferenceIndexModel SelectOMPreferences()
    {
        return SelectOMPreferences(claims.WSID);
    }

    public bool UpdateOMPreferences(OrderManagerPreferenceRequest request)
    {
        return UpdateOMPreferences(request.MaxOrders, request.AllowInProc, request.AllowPartRel, request.DefUserFields, request.CustomReport!, request.CustomAdmin!,
        request.CustomAdminText!, request.PrintDirectly, claims.UserName, claims.WSID, request.ViewType!, request.OrderType!);
    }

    private OrderManagerPreferenceIndexModel SelectOMPreferences(string WSID)
    {
        var response = new OrderManagerPreferenceIndexModel();

        var param = new SqlParameter[]{
                        new("@WSID", WSID)
                };

        DataSet dataSet = SqlHelper.ExecuteReader(config.SDConnectionString, "selOMPrefs", param);
        if (UtilityHelper.IsDataSetValid(dataSet))
        {
            response.Preferences = (from DataRow dr2 in dataSet.Tables[0].Rows
                                    select new OrderManagerPreferenceModel()
                                    {
                                        MaxOrders = UtilityHelper.IsValidInt(dr2["Max Orders"]),
                                        AllowInProc = UtilityHelper.IsValidBool(dr2["Allow in Process"]),
                                        AllowPartRel = UtilityHelper.IsValidBool(dr2["Allow Partial Release"]),
                                        DefUserFields = UtilityHelper.IsValidBool(dr2["Default User Fields"]),
                                        PrintDirectly = UtilityHelper.IsValidBool(dr2["Print Directly"]),
                                        ViewType = UtilityHelper.IsValidString(dr2["View Type"]),
                                        OrderType = UtilityHelper.IsValidString(dr2["Order Type"])
                                    }).ToList();

            DataRow drCR = dataSet.Tables[1].Rows[0];
            response.CustomReport = UtilityHelper.IsValidString(drCR[0]);

            DataRow drA = dataSet.Tables[2].Rows[0];
            response.CustomAdmin = UtilityHelper.IsValidString(drA[0]);

            DataRow drCAT = dataSet.Tables[3].Rows[0];
            response.CustomAdminText = UtilityHelper.IsValidString(drCAT[0]);

        }

        return response;
    }

    private bool UpdateOMPreferences(int MaxOrder, bool AllowInProc, bool AllowPartRel, bool DefUserFields, string CustomReport, string CustomAdmin,
        string CustomAdminText, bool PrintDirect, string username, string WSID, string ViewType, string OrderType)
    {
        var isSaved = false;
        var param = new SqlParameter[]{
                        new("@MaxOrder", MaxOrder),
                        new("@AllowInProc", AllowInProc),
                        new("@AllowPartRel", AllowPartRel),
                        new("@DefUserFields", DefUserFields),
                        new("@CustomReport", CustomReport),
                        new("@CustomAdmin", CustomAdmin),
                        new("@CustomAdminText", CustomAdminText),
                        new("@PrintDirect", PrintDirect),
                        new("@ViewType", ViewType),
                        new("@OrderType", OrderType),
                        new("@User", username),
                        new("@WSID", WSID)
            };

        var rowsEffected = SqlHelper.ExecuteNonQuery(config.SDConnectionString, "updOMPrefs", param);
        if (rowsEffected > 0)
        {
            isSaved = true;
        }

        return isSaved;

    }
}
