using PeakLogix.DAL.Interfaces;
using PeakLogix.PickProApi.Interfaces;
using PeakLogix.PickProApi.Models;
using System.Data;
using System.Data.SqlClient;

namespace PeakLogix.PickProApi.Repositories;

public class OrderManagerRepository(
    IDatabaseConfig config,
    IOMPreferenceRepository orderManagerPreferenceRepo, 
    ICreateOrderRepository createOrderRepo,
    IClaimData claims
    ) : IOrderManagerRepository
{
    public OrderManagerIndexResponse OrderManagerIndex()
    {
        return new OrderManagerIndexResponse
        {
            MaxOrder = SelectMaxOrders(claims.WSID),
            ColumnSequence = UtilityHelper.GetColumSequence(config.SDConnectionString, claims.UserName!, "Order Manager"),
            Preferences = orderManagerPreferenceRepo.SelectOMPreferences()
        };
    }

    public CreateOrderResponse CreateOrders()
    {
        return new CreateOrderResponse
        {
            ColumnSequence = UtilityHelper.GetColumSequence(config.SDConnectionString, claims.UserName, "Order Manager Create"),
            Warehouses = createOrderRepo.SelectWarehouses(),
            ProcessingBy = claims.UserName,
            Preferences = orderManagerPreferenceRepo.SelectOMPreferences()
        };
    }

    public int SelectMaxOrders()
    {
        return SelectMaxOrders(claims.WSID!);
    }

    public OrderManagerTempTableResponse SelectOrderManagerTempDT(OrderManagerTempRequest request)
    {
        return SelectOrderManagerTempDT(request.startRow!, request.endRow!, request.sortCol, request.sortOrder!, request.searchColumn!, request.searchString!, claims.WSID);
    }

    public bool DelOrderManTemp()
    {
        return DelOrderManTemp(claims.WSID!);
    }

    public bool FillOrderManTempData(InsertOrderManagerTempRequest request)
    {
        return FillOrderManTempData(request.Col!, request.ColVal1!, request.ColVal2!, request.WhereClause!, request.TransType!, request.ViewType!, request.OrderType!, request.MaxOrders!, request.filter!, claims.WSID);
    }

    public bool UpdateOrderManagerRecords(UpdateOrderManagerRecordsRequest request)
    {
        return UpdateOrderManagerRecords(request.ViewType!, request.OrderType!, request.ID!, request.RequiredDate!, request.Notes!, request.Priority!, request.User1!,
        request.User2!, request.User3!, request.User4!, request.User5!, request.User6!, request.User7!, request.User8!, request.User9!, request.User10!, request.Emergency!, request.Label!,
        request.CheckRequiredDate, request.CheckNotes, request.CheckPriority, request.CheckUser1, request.CheckUser2, request.CheckUser3, request.CheckUser4, request.CheckUser5,
        request.CheckUser6, request.CheckUser7, request.CheckUser8, request.CheckUser9, request.CheckUser10, request.CheckEmergency, request.CheckLabel, claims.WSID);
    }

    public List<OpenTransactionPendingModel> SelectCreateOrdersDT(CreateOrderRequest request)
    {
        return SelectCreateOrdersDT(request.OrderNumber!, request.Filter!);
    }

    public List<OrderManagerUserFeildModel> SelUserFieldData()
    {
        return SelUserFieldData(claims.WSID);
    }

    public bool UpdUserFieldData(OrderManagerUserFeildModel request)
    {
        return UpdUserFieldData(request.UserField1!, request.UserField2!, request.UserField3!, request.UserField4!, request.UserField5!, request.UserField6!, request.UserField7!, request.UserField8!, request.UserField9!, request.UserField10!, claims.WSID);
    }

    public bool DeleteOTPend(DeleteOTRequest request)
    {
        return DeleteOTPend(request.IDS!, claims.UserName, claims.WSID);
    }

    public bool ReleaseOrders(ReleaseOrderRequest request)
    {
        return ReleaseOrders(request.val!, request.page!,request.allowPartRel! ,claims.WSID);
    }

    public bool DeleteOMOTPend(DeleteOmotPendRequest request)
    {
        return DeleteOMOTPend(request.ViewType!, request.RecordIds, claims.UserName, claims.WSID);
    }

    public bool InsertOTTemp(OpenTransactionTempRequest request)
    {
        return InsertOTTemp(request.OrderNumber!, request.TransType!, request.Warehouse!, request.ItemNumber!, request.Description!, request.UnitofMeasure!,
        request.TransQty, request.LineNumber, request.Priority, request.RequiredDate!, request.HostTransID!, request.Emergency, request.Label, request.LotNumber!, request.ExpirationDate!,
        request.SerialNumber!, request.Revision!, request.BatchPickID!, request.ToteID!, request.Cell!, request.Notes!, request.UserField1!, request.UserField2!, request.UserField3!,
        request.UserField4!, request.UserField5!, request.UserField6!, request.UserField7!, request.UserField8!, request.UserField9!, request.UserField10!, request.InProcess,
        request.ProcessBy!, request.ImportBy!, request.ImportDate!, request.ImportFileName!);
    }

    public bool UpdateOTTemp(OpenTransactionTempRequest request)
    {
        return UpdateOTTemp(request.ID, request.TransType!, request.Warehouse!, request.ItemNumber!, request.Description!, request.UnitofMeasure!,
         request.TransQty, request.LineNumber, request.Priority, request.RequiredDate!, request.HostTransID!, request.Emergency, request.Label, request.LotNumber!, request.ExpirationDate!,
         request.SerialNumber!, request.Revision!, request.BatchPickID!, request.ToteID!, request.Cell!, request.Notes!, request.UserField1!, request.UserField2!, request.UserField3!,
         request.UserField4!, request.UserField5!, request.UserField6!, request.UserField7!, request.UserField8!, request.UserField9!, request.UserField10!, request.InProcess,
         request.ProcessBy!, request.ImportBy!, request.ImportDate!, request.ImportFileName!);
    }

    public OrderManagerTempTableResponse SelectOrderManagerTempDTNew(OrderManagerTempRequest request)
    {
        return SelectOrderManagerTempDTNew(request.startRow!, request.endRow!, request.sortCol, request.sortOrder!, request.searchColumn!, request.searchString!, claims.WSID, claims.UserName);
    }

    private int SelectMaxOrders(string WSID)
    {
        var MaxOrders = 0;
        var param = new SqlParameter[]{
                        new("@WSID", WSID)
            };

        DataSet dataSet = SqlHelper.ExecuteReader(config.SDConnectionString, "selOMPrefs", CommandType.StoredProcedure, param);
        if (UtilityHelper.IsDataSetValid(dataSet))
        {
            MaxOrders = UtilityHelper.IsValidInt(dataSet.Tables[0].Rows[0]["Max Orders"]);
        }
        return MaxOrders;

    }

    private OrderManagerTempTableResponse SelectOrderManagerTempDT(string startRow, string endRow, int sortCol, string sortOrder, string searchColumn, string searchString, string WSID)
    {
        var response = new OrderManagerTempTableResponse();

        searchString = UtilityHelper.cleanSearch(searchString);

        var param = new SqlParameter[]{
                        new("@WSID", WSID),
                        new("@sRow", startRow),
                        new("@eRow", endRow),
                        new("@sortColumn", sortCol),
                        new("@sortOrder", sortOrder),
                        new("@searchCol", searchColumn),
                        new("@searchStr", searchString)
                };

        DataSet dataSet = SqlHelper.ExecuteReader(config.SDConnectionString, "selOTDTNew", CommandType.StoredProcedure, param);

        if (UtilityHelper.IsDataSetValid(dataSet))
        {
            DataRow drT = dataSet.Tables[0].Rows[0];
            response.TotalRecords = UtilityHelper.IsValidInt(drT[0]);

            DataRow dr1 = dataSet.Tables[1].Rows[0];
            response.RecordsFiltered = UtilityHelper.IsValidInt(dr1[0]);

            response.Transactions = (from DataRow dr2 in dataSet.Tables[2].Rows
                                     select new OrderManagerTempModel()
                                     {
                                         WSID = UtilityHelper.IsValidString(dr2["WSID"]),
                                         TransactionType = UtilityHelper.IsValidString(dr2["Transaction Type"]),
                                         OrderNumber = UtilityHelper.IsValidString(dr2["Order Number"]),
                                         Priority = UtilityHelper.IsValidInt(dr2["Priority"]),
                                         RequiredDate = UtilityHelper.IsValidString(dr2["Required Date"]),
                                         UserField1 = UtilityHelper.IsValidString(dr2["User Field1"]),
                                         UserField2 = UtilityHelper.IsValidString(dr2["User Field2"]),
                                         UserField3 = UtilityHelper.IsValidString(dr2["User Field3"]),
                                         UserField4 = UtilityHelper.IsValidString(dr2["User Field4"]),
                                         UserField5 = UtilityHelper.IsValidString(dr2["User Field5"]),
                                         UserField6 = UtilityHelper.IsValidString(dr2["User Field6"]),
                                         UserField7 = UtilityHelper.IsValidString(dr2["User Field7"]),
                                         UserField8 = UtilityHelper.IsValidString(dr2["User Field8"]),
                                         UserField9 = UtilityHelper.IsValidString(dr2["User Field9"]),
                                         UserField10 = UtilityHelper.IsValidString(dr2["User Field10"]),
                                         ItemNumber = UtilityHelper.IsValidString(dr2["Item Number"]),
                                         Description = UtilityHelper.IsValidString(dr2["Description"]),
                                         LineNumber = UtilityHelper.IsValidInt(dr2["Line Number"]),
                                         TransactionQuantity = UtilityHelper.IsValidInt(dr2["Transaction Quantity"]),
                                         AllocatedPicks = UtilityHelper.IsValidInt(dr2["Allocated Picks"]),
                                         AllocatedPuts = UtilityHelper.IsValidInt(dr2["Allocated Puts"]),
                                         AvailableQuantity = UtilityHelper.IsValidInt(dr2["Available Quantity"]),
                                         StockQuantity = UtilityHelper.IsValidInt(dr2["Stock Quantity"]),
                                         Warehouse = UtilityHelper.IsValidString(dr2["Warehouse"]),
                                         Zone = UtilityHelper.IsValidString(dr2["Zone"]),
                                         LineSequence = UtilityHelper.IsValidInt(dr2["Line Sequence"]),
                                         ToteID = UtilityHelper.IsValidString(dr2["Tote ID"]),
                                         ToteNumber = UtilityHelper.IsValidInt(dr2["Tote Number"]),
                                         UnitOfMeasure = UtilityHelper.IsValidString(dr2["Unit of Measure"]),
                                         BatchPickID = UtilityHelper.IsValidString(dr2["Batch Pick ID"]),
                                         Category = UtilityHelper.IsValidString(dr2["Category"]),
                                         SubCategory = UtilityHelper.IsValidString(dr2["Sub Category"]),
                                         ImportBy = UtilityHelper.IsValidString(dr2["Import By"]),
                                         ImportDate = UtilityHelper.IsValidString(dr2["Import Date"]),
                                         ImportFilename = UtilityHelper.IsValidString(dr2["Import Filename"]),
                                         ExpirationDate = UtilityHelper.IsValidString(dr2["Expiration Date"]),
                                         LotNumber = UtilityHelper.IsValidString(dr2["Lot Number"]),
                                         SerialNumber = UtilityHelper.IsValidString(dr2["Serial Number"]),
                                         Notes = UtilityHelper.IsValidString(dr2["Notes"]),
                                         Revision = UtilityHelper.IsValidString(dr2["Revision"]),
                                         SupplierItemID = UtilityHelper.IsValidString(dr2["SupplierItemID"]),
                                         ID = UtilityHelper.IsValidInt(dr2["ID"]),
                                         HostTransactionID = UtilityHelper.IsValidString(dr2["Host Transaction ID"]),
                                         Emergency = UtilityHelper.IsValidBool(dr2["Emergency"]),
                                         Location = UtilityHelper.IsValidString(dr2["Location"]),
                                         Label = UtilityHelper.IsValidBool(dr2["Label"]),
                                         Cell = UtilityHelper.IsValidString(dr2["Cell"]),
                                         TotalOTLines = UtilityHelper.IsValidInt(dr2["TotalOTLines"]),
                                         RN = UtilityHelper.IsValidInt(dr2["RN"])
                                     }).ToList();
        }
        return response;

    }

    private bool DelOrderManTemp(string WSID)
    {
        var isSaved = false;
        var param = new SqlParameter[]{
                        new("@WSID", WSID)
            };

        var rowsEffected = SqlHelper.ExecuteNonQuery(config.SDConnectionString, "delOrderManagerTemp", CommandType.StoredProcedure, param);
        if (rowsEffected > 0)
        {
            isSaved = true;
        }

        return isSaved;

    }

    //ORDER MANAGER HUB

    private bool FillOrderManTempData(string Col, string ColVal1, string ColVal2, string WhereClause, string TransType, string ViewType, string OrderType, string MaxOrders, string filter, string WSID)
    {
        var isSaved = false;
        var param = new SqlParameter[]{
                        new("@Col", Col),
                        new("@ColVal1", ColVal1),
                        new("@ColVal2", ColVal2),
                        new("@WhereClause", WhereClause),
                        new("@TransType", TransType),
                        new("@ViewType", ViewType),
                        new("@OrderType", OrderType),
                        new("@MaxOrders", MaxOrders),
                        new("@filter", filter),
                        new("@WSID", WSID),
            };

        var rowsEffected = SqlHelper.ExecuteNonQuery(config.SDConnectionString, "insOrderManagerTemp", CommandType.StoredProcedure, param);
        if (rowsEffected > 0)
        {
            isSaved = true;
        }

        return isSaved;

    }

    private bool UpdateOrderManagerRecords(string ViewType, string OrderType, string ID, string RequiredDate, string Notes, string Priority, string User1,
        string User2, string User3, string User4, string User5, string User6, string User7, string User8, string User9, string User10, string Emergency, string Label,
        bool CheckRequiredDate, bool CheckNotes, bool CheckPriority, bool CheckUser1, bool CheckUser2, bool CheckUser3, bool CheckUser4, bool CheckUser5,
        bool CheckUser6, bool CheckUser7, bool CheckUser8, bool CheckUser9, bool CheckUser10, bool CheckEmergency, bool CheckLabel, string WSID)
    {
        var isSaved = false;
        var param = new SqlParameter[]
        {
            new("@ViewType", ViewType),
            new("@OrderType", OrderType),
            new("@ID1", ID),
            new("@RequiredDate", string.IsNullOrEmpty(RequiredDate) ? DBNull.Value : RequiredDate),
            new("@Notes", string.IsNullOrEmpty(Notes) ? DBNull.Value : Notes),
            new("@Priority", Priority),
            new("@UserField1", string.IsNullOrEmpty(User1) ? DBNull.Value : User1),
            new("@UserField2", string.IsNullOrEmpty(User2) ? DBNull.Value : User2),
            new("@UserField3", string.IsNullOrEmpty(User3) ? DBNull.Value : User3),
            new("@UserField4", string.IsNullOrEmpty(User4) ? DBNull.Value : User4),
            new("@UserField5", string.IsNullOrEmpty(User5) ? DBNull.Value : User5),
            new("@UserField6", string.IsNullOrEmpty(User6) ? DBNull.Value : User6),
            new("@UserField7", string.IsNullOrEmpty(User7) ? DBNull.Value : User7),
            new("@UserField8", string.IsNullOrEmpty(User8) ? DBNull.Value : User8),
            new("@UserField9", string.IsNullOrEmpty(User9) ? DBNull.Value : User9),
            new("@UserField10", string.IsNullOrEmpty(User10) ? DBNull.Value : User10),
            new("@Emergency", Emergency),
            new("@Label", Label),
            new("@CheckRequiredDate", CheckRequiredDate),
            new("@CheckNotes", CheckNotes),
            new("@CheckPriority", CheckPriority),
            new("@CheckUserField1", CheckUser1),
            new("@CheckUserField2", CheckUser2),
            new("@CheckUserField3", CheckUser3),
            new("@CheckUserField4", CheckUser4),
            new("@CheckUserField5", CheckUser5),
            new("@CheckUserField6", CheckUser6),
            new("@CheckUserField7", CheckUser7),
            new("@CheckUserField8", CheckUser8),
            new("@CheckUserField9", CheckUser9),
            new("@CheckUserField10", CheckUser10),
            new("@CheckEmergency", CheckEmergency),
            new("@CheckLabel", CheckLabel),
            new("@WSID", WSID)
        };

        var rowsEffected = SqlHelper.ExecuteNonQuery(config.SDConnectionString, "updOTPendOTOrderManager", CommandType.StoredProcedure, param);
        if (rowsEffected > 0)
        {
            isSaved = true;
        }

        return isSaved;

    }

    private List<OpenTransactionPendingModel> SelectCreateOrdersDT(string orderNum, string filter)
    {
        var response = new List<OpenTransactionPendingModel>();

        if (orderNum.Contains("'"))
        {
            orderNum = orderNum.Replace("'", "''");
        }

        var param = new SqlParameter[]{
                        new("@orderNumber", orderNum),
                        new("@filter", filter)
                };

        DataSet dataSet = SqlHelper.ExecuteReader(config.SDConnectionString, "selOTPendDT", CommandType.StoredProcedure, param);
        if (UtilityHelper.IsDataSetValid(dataSet))
        {
            response = (from DataRow dr2 in dataSet.Tables[0].Rows
                        select new OpenTransactionPendingModel()
                        {
                            TransactionType = UtilityHelper.IsValidString(dr2["Transaction Type"]),
                            OrderNumber = UtilityHelper.IsValidString(dr2["Order Number"]),
                            Priority = UtilityHelper.IsValidInt(dr2["Priority"]),
                            RequiredDate = UtilityHelper.IsValidString(dr2["Required Date"]),
                            UserField1 = UtilityHelper.IsValidString(dr2["User Field1"]),
                            UserField2 = UtilityHelper.IsValidString(dr2["User Field2"]),
                            UserField3 = UtilityHelper.IsValidString(dr2["User Field3"]),
                            UserField4 = UtilityHelper.IsValidString(dr2["User Field4"]),
                            UserField5 = UtilityHelper.IsValidString(dr2["User Field5"]),
                            UserField6 = UtilityHelper.IsValidString(dr2["User Field6"]),
                            UserField7 = UtilityHelper.IsValidString(dr2["User Field7"]),
                            UserField8 = UtilityHelper.IsValidString(dr2["User Field8"]),
                            UserField9 = UtilityHelper.IsValidString(dr2["User Field9"]),
                            UserField10 = UtilityHelper.IsValidString(dr2["User Field10"]),
                            ItemNumber = UtilityHelper.IsValidString(dr2["Item Number"]),
                            Description = UtilityHelper.IsValidString(dr2["Description"]),
                            LineNumber = UtilityHelper.IsValidInt(dr2["Line Number"]),
                            TransactionQuantity = UtilityHelper.IsValidInt(dr2["Transaction Quantity"]),
                            Warehouse = UtilityHelper.IsValidString(dr2["Warehouse"]),
                            LineSequence = UtilityHelper.IsValidInt(dr2["Line Sequence"]),
                            ToteID = UtilityHelper.IsValidString(dr2["Tote ID"]),
                            UnitOfMeasure = UtilityHelper.IsValidString(dr2["Unit of Measure"]),
                            BatchPickID = UtilityHelper.IsValidString(dr2["Batch Pick ID"]),
                            ImportBy = UtilityHelper.IsValidString(dr2["Import By"]),
                            ImportDate = UtilityHelper.IsValidString(dr2["Import Date"]),
                            ImportFilename = UtilityHelper.IsValidString(dr2["Import Filename"]),
                            ExpirationDate = UtilityHelper.IsValidString(dr2["Expiration Date"]),
                            LotNumber = UtilityHelper.IsValidString(dr2["Lot Number"]),
                            SerialNumber = UtilityHelper.IsValidString(dr2["Serial Number"]),
                            Notes = UtilityHelper.IsValidString(dr2["Notes"]),
                            Revision = UtilityHelper.IsValidString(dr2["Revision"]),
                            ID = UtilityHelper.IsValidInt(dr2["ID"]),
                            HostTransactionID = UtilityHelper.IsValidString(dr2["Host Transaction ID"]),
                            Emergency = UtilityHelper.IsValidBool(dr2["Emergency"]),
                            Label = UtilityHelper.IsValidBool(dr2["Label"]),
                            Cell = UtilityHelper.IsValidString(dr2["Cell"]),
                            InProcess = UtilityHelper.IsValidString(dr2["In Process"]),
                            ProcessingBy = UtilityHelper.IsValidString(dr2["Processing By"])
                        }).ToList();
        }

        return response;
    }

    private List<OrderManagerUserFeildModel> SelUserFieldData(string WSID)
    {
        var response = new List<OrderManagerUserFeildModel>();

        var param = new SqlParameter[]{
                        new("@WSID", WSID)
                };

        DataSet dataSet = SqlHelper.ExecuteReader(config.SDConnectionString, "selUserFieldData", CommandType.StoredProcedure, param);
        if (UtilityHelper.IsDataSetValid(dataSet))
        {
            response = (from DataRow dr2 in dataSet.Tables[0].Rows
                        select new OrderManagerUserFeildModel()
                        {
                            UserField1 = UtilityHelper.IsValidString(dr2["User Field1"]),
                            UserField2 = UtilityHelper.IsValidString(dr2["User Field2"]),
                            UserField3 = UtilityHelper.IsValidString(dr2["User Field3"]),
                            UserField4 = UtilityHelper.IsValidString(dr2["User Field4"]),
                            UserField5 = UtilityHelper.IsValidString(dr2["User Field5"]),
                            UserField6 = UtilityHelper.IsValidString(dr2["User Field6"]),
                            UserField7 = UtilityHelper.IsValidString(dr2["User Field7"]),
                            UserField8 = UtilityHelper.IsValidString(dr2["User Field8"]),
                            UserField9 = UtilityHelper.IsValidString(dr2["User Field9"]),
                            UserField10 = UtilityHelper.IsValidString(dr2["User Field10"])
                        }).ToList();
        }

        return response;
    }

    private bool UpdUserFieldData(string UserField1, string UserField2, string UserField3, string UserField4, string UserField5, string UserField6, string UserField7, string UserField8, string UserField9, string UserField10, string WSID)
    {
        var isSaved = false;
        var param = new SqlParameter[]{
                        new("@UserField1", string.IsNullOrEmpty(UserField1) ? DBNull.Value : UserField1),
                        new("@UserField2", string.IsNullOrEmpty(UserField2) ? DBNull.Value : UserField2),
                        new("@UserField3", string.IsNullOrEmpty(UserField3) ? DBNull.Value : UserField3),
                        new("@UserField4", string.IsNullOrEmpty(UserField4) ? DBNull.Value : UserField4),
                        new("@UserField5", string.IsNullOrEmpty(UserField5) ? DBNull.Value : UserField5),
                        new("@UserField6", string.IsNullOrEmpty(UserField6) ? DBNull.Value : UserField6),
                        new("@UserField7", string.IsNullOrEmpty(UserField7) ? DBNull.Value : UserField7),
                        new("@UserField8", string.IsNullOrEmpty(UserField8) ? DBNull.Value : UserField8),
                        new("@UserField9", string.IsNullOrEmpty(UserField9) ? DBNull.Value : UserField9),
                        new("@UserField10", string.IsNullOrEmpty(UserField10) ? DBNull.Value : UserField10),
                        new("@WSID", WSID)
            };

        var rowsEffected = SqlHelper.ExecuteNonQuery(config.SDConnectionString, "updUserFieldData", CommandType.StoredProcedure, param);
        if (rowsEffected > 0)
        {
            isSaved = true;
        }

        return isSaved;

    }

    private bool DeleteOTPend(List<string> IDS, string User, string WSID)
    {
        var isSaved = false;
        var param = new SqlParameter[]{
                        new("@IDS", string.Join(",",IDS)),
                        new("@DeletedBy", User),
                        new("@WSID", WSID),
            };

        var rowsEffected = SqlHelper.ExecuteNonQuery(config.SDConnectionString, "delOTPend", CommandType.StoredProcedure, param);
        if (rowsEffected > 0)
        {
            isSaved = true;
        }

        return isSaved;

    }

    private bool ReleaseOrders(string val, string page, bool allowpartrel, string WSID)
    {
        var isSaved = false;
        SqlParameter[] param;

        if (page == "Create Orders")
        {
            param = new SqlParameter[]{
                new("@OrderNumber", val),
                new("@WSID", WSID),
                new("@allowPartRel", allowpartrel),
            };
        }
        else
        {
            param = new SqlParameter[]{
                new("@View", val),
                new("@WSID", WSID),
                new("@allowPartRel", allowpartrel),
            };
        }

        var rowsEffected = SqlHelper.ExecuteNonQuery(config.SDConnectionString, "insOTOrderMan", CommandType.StoredProcedure, param);
        if (rowsEffected > 0)
        {
            isSaved = true;
        }

        return isSaved;

    }

    private bool DeleteOMOTPend(string ViewType, string? RecordIds, string User, string WSID)
    {
        var isSaved = false;
        var recordIdsStr = !string.IsNullOrEmpty(RecordIds) ? RecordIds : "";
        
        var param = new SqlParameter[]{
                new("@DeletedBy", User),
                new("@ViewType", ViewType),
                new("@WSID", WSID),
                new("@RecordIds", recordIdsStr)
            };

        var rowsEffected = SqlHelper.ExecuteNonQuery(config.SDConnectionString, "delOTPendOM", CommandType.StoredProcedure, param);
        if (rowsEffected > 0)
        {
            isSaved = true;
        }

        return isSaved;

    }

    private bool InsertOTTemp(string OrderNumber, string TransType, string Warehouse, string ItemNumber, string Description, string UnitofMeasure,
        int? TransQty, int? LineNumber, int? Priority, string RequiredDate, string HostTransID, bool Emergency, bool Label, string LotNumber, string ExpirationDate,
        string SerialNumber, string Revision, string BatchPickID, string ToteID, string Cell, string Notes, string UserField1, string UserField2, string UserField3,
        string UserField4, string UserField5, string UserField6, string UserField7, string UserField8, string UserField9, string UserField10, bool InProcess,
        string ProcessBy, string ImportBy, string ImportDate, string ImportFileName)
    {
        var isSaved = false;
        var param = new SqlParameter[]{
                new("@OrderNum", OrderNumber),
                new("@TransType", TransType),
                new("@Warehouse", string.IsNullOrEmpty(Warehouse) ? DBNull.Value : Warehouse),
                new("@ItemNumber", ItemNumber),
                new("@Description", Description),
                new("@UoM", string.IsNullOrEmpty(UnitofMeasure) ? DBNull.Value : UnitofMeasure),
                new("@TransQty", TransQty),
                new("@LineNum", LineNumber),
                new("@Priority", Priority),
                new("@HostTransID", string.IsNullOrEmpty(HostTransID) ? DBNull.Value : HostTransID),
                new("@Emergency", Emergency),
                new("@Label", Label),
                new("@LotNumber", string.IsNullOrEmpty(LotNumber) ? "0" : LotNumber),
                new("@ReqDate", !string.IsNullOrEmpty(RequiredDate) ? DateTime.Parse(RequiredDate) : DBNull.Value),
                new("@ExpirationDate", !string.IsNullOrEmpty(ExpirationDate) ? DateTime.Parse(ExpirationDate) : DBNull.Value),
                new("@ImportDate", !string.IsNullOrEmpty(ImportDate) ? DateTime.Parse(ImportDate) : DateTime.UtcNow),
                new("@SerialNumber", string.IsNullOrEmpty(SerialNumber) ? "0" : SerialNumber),
                new("@Revision", string.IsNullOrEmpty(Revision) ? DBNull.Value : Revision),
                new("@BatchPickID", string.IsNullOrEmpty(BatchPickID) ? DBNull.Value : BatchPickID),
                new("@ToteID", string.IsNullOrEmpty(ToteID) ? DBNull.Value : ToteID),
                new("@Cell", string.IsNullOrEmpty(Cell) ? DBNull.Value : Cell),
                new("@Notes", string.IsNullOrEmpty(Notes) ? DBNull.Value : Notes),
                new("@UserField1", string.IsNullOrEmpty(UserField1) ? DBNull.Value : UserField1),
                new("@UserField2", string.IsNullOrEmpty(UserField2) ? DBNull.Value : UserField2),
                new("@UserField3", string.IsNullOrEmpty(UserField3) ? DBNull.Value : UserField3),
                new("@UserField4", string.IsNullOrEmpty(UserField4) ? DBNull.Value : UserField4),
                new("@UserField5", string.IsNullOrEmpty(UserField5) ? DBNull.Value : UserField5),
                new("@UserField6", string.IsNullOrEmpty(UserField6) ? DBNull.Value : UserField6),
                new("@UserField7", string.IsNullOrEmpty(UserField7) ? DBNull.Value : UserField7),
                new("@UserField8", string.IsNullOrEmpty(UserField8) ? DBNull.Value : UserField8),
                new("@UserField9", string.IsNullOrEmpty(UserField9) ? DBNull.Value : UserField9),
                new("@UserField10", string.IsNullOrEmpty(UserField10) ? DBNull.Value : UserField10),
                new("@InProcess", InProcess),
                new("@ProcBy", ProcessBy),
                new("@ImportBy", ImportBy),
                new("@ImportFileName", ImportFileName)
            };

        var rowsEffected = SqlHelper.ExecuteNonQuery(config.SDConnectionString, "insOTTemp", CommandType.StoredProcedure, param);
        if (rowsEffected > 0)
        {
            isSaved = true;
        }

        return isSaved;

    }

    private bool UpdateOTTemp(int ID, string TransType, string Warehouse, string ItemNumber, string Description, string UnitofMeasure,
int? TransQty, int? LineNumber, int? Priority, string RequiredDate, string HostTransID, bool Emergency, bool Label, string LotNumber, string ExpirationDate,
string SerialNumber, string Revision, string BatchPickID, string ToteID, string Cell, string Notes, string UserField1, string UserField2, string UserField3,
string UserField4, string UserField5, string UserField6, string UserField7, string UserField8, string UserField9, string UserField10, bool InProcess,
string ProcessBy, string ImportBy, string ImportDate, string ImportFileName)
    {
        var isSaved = false;
        var param = new SqlParameter[]{
                new("@ID", ID),
                new("@TransType", TransType),
                new("@Warehouse", string.IsNullOrEmpty(Warehouse) ? DBNull.Value : Warehouse),
                new("@ItemNumber", ItemNumber),
                new("@Description", Description),
                new("@UoM", string.IsNullOrEmpty(UnitofMeasure) ? DBNull.Value : UnitofMeasure),
                new("@TransQty", TransQty),
                new("@LineNum", LineNumber),
                new("@Priority", Priority),
                new("@HostTransID", string.IsNullOrEmpty(HostTransID) ? DBNull.Value : HostTransID),
                new("@Emergency", Emergency),
                new("@Label", Label),
                new("@LotNumber", string.IsNullOrEmpty(LotNumber) ? "0" : LotNumber),
                new("@SerialNumber", string.IsNullOrEmpty(SerialNumber) ? "0" : SerialNumber),
                new("@Revision", string.IsNullOrEmpty(Revision) ? DBNull.Value : Revision),
                new("@BatchPickID", string.IsNullOrEmpty(BatchPickID) ? DBNull.Value : BatchPickID),
                new("@ToteID", string.IsNullOrEmpty(ToteID) ? DBNull.Value : ToteID),
                new("@ReqDate", !string.IsNullOrEmpty(RequiredDate) ? DateTime.Parse(RequiredDate) : DBNull.Value),
                new("@ExpirationDate", !string.IsNullOrEmpty(ExpirationDate) ? DateTime.Parse(ExpirationDate) : DBNull.Value),
                new("@ImportDate", !string.IsNullOrEmpty(ImportDate) ? DateTime.Parse(ImportDate) : DateTime.UtcNow),
                new("@Cell", string.IsNullOrEmpty(Cell) ? DBNull.Value : Cell),
                new("@Notes", string.IsNullOrEmpty(Notes) ? DBNull.Value : Notes),
                new("@UserField1", string.IsNullOrEmpty(UserField1) ? DBNull.Value : UserField1),
                new("@UserField2", string.IsNullOrEmpty(UserField2) ? DBNull.Value : UserField2),
                new("@UserField3", string.IsNullOrEmpty(UserField3) ? DBNull.Value : UserField3),
                new("@UserField4", string.IsNullOrEmpty(UserField4) ? DBNull.Value : UserField4),
                new("@UserField5", string.IsNullOrEmpty(UserField5) ? DBNull.Value : UserField5),
                new("@UserField6", string.IsNullOrEmpty(UserField6) ? DBNull.Value : UserField6),
                new("@UserField7", string.IsNullOrEmpty(UserField7) ? DBNull.Value : UserField7),
                new("@UserField8", string.IsNullOrEmpty(UserField8) ? DBNull.Value : UserField8),
                new("@UserField9", string.IsNullOrEmpty(UserField9) ? DBNull.Value : UserField9),
                new("@UserField10", string.IsNullOrEmpty(UserField10) ? DBNull.Value : UserField10),
                new("@InProcess", InProcess),
                new("@ProcBy", ProcessBy),
                new("@ImportBy", ImportBy),
                new("@ImportFileName", ImportFileName)
            };

        var rowsEffected = SqlHelper.ExecuteNonQuery(config.SDConnectionString, "updOTTemp", CommandType.StoredProcedure, param);
        if (rowsEffected > 0)
        {
            isSaved = true;
        }

        return isSaved;

    }

    private OrderManagerTempTableResponse SelectOrderManagerTempDTNew(string startRow, string endRow, int sortCol, string sortOrder, string searchColumn, string searchString, string WSID, string User)
    {
        var response = new OrderManagerTempTableResponse();

        List<string> lstSortCol = UtilityHelper.GetTableColumns(config.SDConnectionString, "Order Manager");
        var sortColumn = lstSortCol[sortCol];

        var param = new SqlParameter[]{
                        new("@WSID", WSID),
                        new("@sRow", startRow),
                        new("@eRow", endRow),
                        new("@sortColumn", sortColumn),
                        new("@sortOrder", sortOrder),
                        new("@searchCol", searchColumn),
                        new("@searchStr", searchString)
                };

        DataSet dataSet = SqlHelper.ExecuteReader(config.SDConnectionString, "selOrderManTempDataDTNew", CommandType.StoredProcedure, param);

        if (UtilityHelper.IsDataSetValid(dataSet))
        {
            DataRow drT = dataSet.Tables[0].Rows[0];
            response.TotalRecords = UtilityHelper.IsValidInt(drT[0]);

            DataRow dr1 = dataSet.Tables[1].Rows[0];
            response.RecordsFiltered = UtilityHelper.IsValidInt(dr1[0]);

            response.Transactions = (from DataRow dr2 in dataSet.Tables[2].Rows
                                     select new OrderManagerTempModel()
                                     {
                                         WSID = UtilityHelper.IsValidString(dr2["WSID"]),
                                         TransactionType = UtilityHelper.IsValidString(dr2["Transaction Type"]),
                                         OrderNumber = UtilityHelper.IsValidString(dr2["Order Number"]),
                                         Priority = UtilityHelper.IsValidInt(dr2["Priority"]),
                                         RequiredDate = UtilityHelper.IsValidString(dr2["Required Date"]),
                                         UserField1 = UtilityHelper.IsValidString(dr2["User Field1"]),
                                         UserField2 = UtilityHelper.IsValidString(dr2["User Field2"]),
                                         UserField3 = UtilityHelper.IsValidString(dr2["User Field3"]),
                                         UserField4 = UtilityHelper.IsValidString(dr2["User Field4"]),
                                         UserField5 = UtilityHelper.IsValidString(dr2["User Field5"]),
                                         UserField6 = UtilityHelper.IsValidString(dr2["User Field6"]),
                                         UserField7 = UtilityHelper.IsValidString(dr2["User Field7"]),
                                         UserField8 = UtilityHelper.IsValidString(dr2["User Field8"]),
                                         UserField9 = UtilityHelper.IsValidString(dr2["User Field9"]),
                                         UserField10 = UtilityHelper.IsValidString(dr2["User Field10"]),
                                         ItemNumber = UtilityHelper.IsValidString(dr2["Item Number"]),
                                         Description = UtilityHelper.IsValidString(dr2["Description"]),
                                         LineNumber = UtilityHelper.IsValidInt(dr2["Line Number"]),
                                         TransactionQuantity = UtilityHelper.IsValidInt(dr2["Transaction Quantity"]),
                                         AllocatedPicks = UtilityHelper.IsValidInt(dr2["Allocated Picks"]),
                                         AllocatedPuts = UtilityHelper.IsValidInt(dr2["Allocated Puts"]),
                                         AvailableQuantity = UtilityHelper.IsValidInt(dr2["Available Quantity"]),
                                         StockQuantity = UtilityHelper.IsValidInt(dr2["Stock Quantity"]),
                                         Warehouse = UtilityHelper.IsValidString(dr2["Warehouse"]),
                                         Zone = UtilityHelper.IsValidString(dr2["Zone"]),
                                         LineSequence = UtilityHelper.IsValidInt(dr2["Line Sequence"]),
                                         ToteID = UtilityHelper.IsValidString(dr2["Tote ID"]),
                                         ToteNumber = UtilityHelper.IsValidInt(dr2["Tote Number"]),
                                         UnitOfMeasure = UtilityHelper.IsValidString(dr2["Unit of Measure"]),
                                         BatchPickID = UtilityHelper.IsValidString(dr2["Batch Pick ID"]),
                                         Category = UtilityHelper.IsValidString(dr2["Category"]),
                                         SubCategory = UtilityHelper.IsValidString(dr2["Sub Category"]),
                                         ImportBy = UtilityHelper.IsValidString(dr2["Import By"]),
                                         ImportDate = UtilityHelper.IsValidString(dr2["Import Date"]),
                                         ImportFilename = UtilityHelper.IsValidString(dr2["Import Filename"]),
                                         ExpirationDate = UtilityHelper.IsValidString(dr2["Expiration Date"]),
                                         LotNumber = UtilityHelper.IsValidString(dr2["Lot Number"]),
                                         SerialNumber = UtilityHelper.IsValidString(dr2["Serial Number"]),
                                         Notes = UtilityHelper.IsValidString(dr2["Notes"]),
                                         Revision = UtilityHelper.IsValidString(dr2["Revision"]),
                                         SupplierItemID = UtilityHelper.IsValidString(dr2["Supplier Item ID"]),
                                         ID = UtilityHelper.IsValidInt(dr2["ID"]),
                                         HostTransactionID = UtilityHelper.IsValidString(dr2["Host Transaction ID"]),
                                         Emergency = UtilityHelper.IsValidBool(dr2["Emergency"]),
                                         Location = UtilityHelper.IsValidString(dr2["Location"]),
                                         Label = UtilityHelper.IsValidBool(dr2["Label"]),
                                         Cell = UtilityHelper.IsValidString(dr2["Cell"]),
                                         TotalOTLines = UtilityHelper.IsValidInt(dr2["TotalOTLines"]),
                                         RN = UtilityHelper.IsValidInt(dr2["RN"])
                                     }).ToList();
        }
        return response;

    }
}
