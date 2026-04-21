namespace PeakLogix.PickProApi.Models;

public class CreateOrderResponse
{
    public List<string>? ColumnSequence { get; set; }
    public List<string>? Warehouses { get; set; }
    public string? ProcessingBy { get; set; }
    public OrderManagerPreferenceIndexModel? Preferences { get; set; }
}

public class OrderManagerIndexResponse
{
    public int MaxOrder { get; set; }
    public List<string>? ColumnSequence { get; set; }
    public OrderManagerPreferenceIndexModel? Preferences { get; set; }
}

public class OrderManagerTempTableResponse
{
    public int TotalRecords { get; set; }
    public int RecordsFiltered { get; set; }
    public List<OrderManagerTempModel>? Transactions { get; set; }
}

public class OrderManagerTempRequest
{
    public string? startRow { get; set; }
    public string? endRow { get; set; }
    public int sortCol { get; set; }
    public string? sortOrder { get; set; }
    public string? searchColumn { get; set; }
    public string? searchString { get; set; }
}

public class InsertOrderManagerTempRequest
{
    public string? Col { get; set; }
    public string? ColVal1 { get; set; }
    public string? ColVal2 { get; set; }
    public string? WhereClause { get; set; }
    public string? TransType { get; set; }
    public string? ViewType { get; set; }
    public string? OrderType { get; set; }
    public string? MaxOrders { get; set; }
    public string? filter { get; set; }
}

public class UpdateOrderManagerRecordsRequest
{
    public string? ViewType { get; set; }
    public string? OrderType { get; set; }
    public string? ID { get; set; }
    public string? RequiredDate { get; set; }
    public string? Notes { get; set; }
    public string? Priority { get; set; }
    public string? User1 { get; set; }
    public string? User2 { get; set; }
    public string? User3 { get; set; }
    public string? User4 { get; set; }
    public string? User5 { get; set; }
    public string? User6 { get; set; }
    public string? User7 { get; set; }
    public string? User8 { get; set; }
    public string? User9 { get; set; }
    public string? User10 { get; set; }
    public string? Emergency { get; set; }
    public string? Label { get; set; }
    public bool CheckRequiredDate { get; set; }
    public bool CheckNotes { get; set; }
    public bool CheckPriority { get; set; }
    public bool CheckUser1 { get; set; }
    public bool CheckUser2 { get; set; }
    public bool CheckUser3 { get; set; }
    public bool CheckUser4 { get; set; }
    public bool CheckUser5 { get; set; }
    public bool CheckUser6 { get; set; }
    public bool CheckUser7 { get; set; }
    public bool CheckUser8 { get; set; }
    public bool CheckUser9 { get; set; }
    public bool CheckUser10 { get; set; }
    public bool CheckEmergency { get; set; }
    public bool CheckLabel { get; set; }
}

public class CreateOrderRequest
{
    public string? OrderNumber { get; set; }
    public string? Filter { get; set; }
}

public class OpenTransactionPendingModel
{
    public int Total { get; set; }
    public string? TransactionType { get; set; }
    public string? OrderNumber { get; set; }
    public int Priority { get; set; }
    public string? RequiredDate { get; set; }
    public string? UserField1 { get; set; }
    public string? UserField2 { get; set; }
    public string? UserField3 { get; set; }
    public string? UserField4 { get; set; }
    public string? UserField5 { get; set; }
    public string? UserField6 { get; set; }
    public string? UserField7 { get; set; }
    public string? UserField8 { get; set; }
    public string? UserField9 { get; set; }
    public string? UserField10 { get; set; }
    public string? ItemNumber { get; set; }
    public int LineNumber { get; set; }
    public int TransactionQuantity { get; set; }
    public bool Label { get; set; }
    public string? InProcess { get; set; }
    public string? ProcessingBy { get; set; }
    public string? Description { get; set; }
    public string? Warehouse { get; set; }
    public string? UnitOfMeasure { get; set; }
    public string? ImportBy { get; set; }
    public string? ImportDate { get; set; }
    public string? ImportFilename { get; set; }
    public string? ExpirationDate { get; set; }
    public string? LotNumber { get; set; }
    public string? SerialNumber { get; set; }
    public string? Notes { get; set; }
    public string? Revision { get; set; }
    public int ID { get; set; }
    public string? HostTransactionID { get; set; }
    public bool Emergency { get; set; }
    public int LineSequence { get; set; }
    public string? ToteID { get; set; }
    public string? Cell { get; set; }
    public string? BatchPickID { get; set; }
}

public class OrderManagerTempModel
{
    public string? WSID { get; set; }
    public string? TransactionType { get; set; }
    public string? OrderNumber { get; set; }
    public int Priority { get; set; }
    public string? RequiredDate { get; set; }
    public string? UserField1 { get; set; }
    public string? UserField2 { get; set; }
    public string? UserField3 { get; set; }
    public string? UserField4 { get; set; }
    public string? UserField5 { get; set; }
    public string? UserField6 { get; set; }
    public string? UserField7 { get; set; }
    public string? UserField8 { get; set; }
    public string? UserField9 { get; set; }
    public string? UserField10 { get; set; }
    public string? ItemNumber { get; set; }
    public string? Description { get; set; }
    public int LineNumber { get; set; }
    public int TransactionQuantity { get; set; }
    public int AllocatedPicks { get; set; }
    public int AllocatedPuts { get; set; }
    public int AvailableQuantity { get; set; }
    public int StockQuantity { get; set; }
    public string? Warehouse { get; set; }
    public string? Zone { get; set; }
    public int LineSequence { get; set; }
    public string? ToteID { get; set; }
    public int ToteNumber { get; set; }
    public string? UnitOfMeasure { get; set; }
    public string? BatchPickID { get; set; }
    public string? Category { get; set; }
    public string? SubCategory { get; set; }
    public string? ImportBy { get; set; }
    public string? ImportDate { get; set; }
    public string? ImportFilename { get; set; }
    public string? ExpirationDate { get; set; }
    public string? LotNumber { get; set; }
    public string? SerialNumber { get; set; }
    public string? Notes { get; set; }
    public string? Revision { get; set; }
    public string? SupplierItemID { get; set; }
    public int ID { get; set; }
    public string? HostTransactionID { get; set; }
    public bool Emergency { get; set; }
    public string? Location { get; set; }
    public bool Label { get; set; }
    public string? Cell { get; set; }
    public int TotalOTLines { get; set; }
    public int RN { get; set; }
}

public class OrderManagerUserFeildModel
{
    public string? UserField1 { get; set; }
    public string? UserField2 { get; set; }
    public string? UserField3 { get; set; }
    public string? UserField4 { get; set; }
    public string? UserField5 { get; set; }
    public string? UserField6 { get; set; }
    public string? UserField7 { get; set; }
    public string? UserField8 { get; set; }
    public string? UserField9 { get; set; }
    public string? UserField10 { get; set; }
}

public class DeleteOTRequest
{
    public List<string>? IDS { get; set; }
}

public class ReleaseOrderRequest
{
    public string? val { get; set; }
    public string? page { get; set; }

    public bool allowPartRel { get; set; }
}

public class DeleteOmotPendRequest
{
    public string? ViewType { get; set; }
    public string? RecordIds { get; set; }
}

public class OpenTransactionTempRequest
{
    public int ID { get; set; }
    public string? OrderNumber { get; set; }
    public string? TransType { get; set; }
    public string? Warehouse { get; set; }
    public string? ItemNumber { get; set; }
    public string? Description { get; set; }
    public string? UnitofMeasure { get; set; }
    public int? TransQty { get; set; }
    public int? LineNumber { get; set; }
    public int? Priority { get; set; }
    public string? RequiredDate { get; set; }
    public string? HostTransID { get; set; }
    public bool Emergency { get; set; }
    public bool Label { get; set; }
    public string? LotNumber { get; set; }
    public string? ExpirationDate { get; set; }
    public string? SerialNumber { get; set; }
    public string? Revision { get; set; }
    public string? BatchPickID { get; set; }
    public string? ToteID { get; set; }
    public string? Cell { get; set; }
    public string? Notes { get; set; }
    public string? UserField1 { get; set; }
    public string? UserField2 { get; set; }
    public string? UserField3 { get; set; }
    public string? UserField4 { get; set; }
    public string? UserField5 { get; set; }
    public string? UserField6 { get; set; }
    public string? UserField7 { get; set; }
    public string? UserField8 { get; set; }
    public string? UserField9 { get; set; }
    public string? UserField10 { get; set; }
    public bool InProcess { get; set; }
    public string? ProcessBy { get; set; }
    public string? ImportBy { get; set; }
    public string? ImportDate { get; set; }
    public string? ImportFileName { get; set; }
}
