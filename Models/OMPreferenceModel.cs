namespace PeakLogix.PickProApi.Models;

public class OrderManagerPreferenceModel
{
    public int MaxOrders { get; set; }
    public bool AllowInProc { get; set; }
    public bool AllowPartRel { get; set; }
    public bool DefUserFields { get; set; }
    public bool PrintDirectly { get; set; }

    public string? ViewType { get; set; }
    public string? OrderType { get; set; }
}

public class OrderManagerPreferenceIndexModel
{
    public List<OrderManagerPreferenceModel>? Preferences { get; set; }
    public string? CustomReport { get; set; }
    public string? CustomAdmin { get; set; }
    public string? CustomAdminText { get; set; }
}

public class OrderManagerPreferenceRequest
{
    public int MaxOrders { get; set; }
    public bool AllowInProc { get; set; }
    public bool AllowPartRel { get; set; }
    public bool DefUserFields { get; set; }
    public string? CustomReport { get; set; }
    public string? CustomAdmin { get; set; }
    public string? CustomAdminText { get; set; }
    public bool PrintDirectly { get; set; }
    public string? ViewType { get; set; }

    public string? OrderType { get; set; }


}
