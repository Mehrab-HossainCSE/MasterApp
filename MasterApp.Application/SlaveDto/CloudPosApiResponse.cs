namespace MasterApp.Application.SlaveDto;

public class CloudPosApiResponse
{
    public string UserName { get; set; }
    public string CompanyCode { get; set; }

    public string CompanyName { get; set; }
    public string Status { get; set; }
    public string CompanyStatus { get; set; }

    public string Date_of_Expire { get; set; }
    public string EmployeeCode { get; set; }
    public string FullName { get; set; }

    public string StoreCode { get; set; }
    public string StoreName { get; set; }
    public string StoreType { get; set; }

    public string SaleOn { get; set; }
    public bool MemberPointEnabled { get; set; }
    public string WarehouseCode { get; set; }

    public bool MemberCardEnabled { get; set; }
    public bool GiftVoucherEnabled { get; set; }
    public bool NoVATSale { get; set; }
    public bool IsVatBeforeDiscount { get; set; }
    public DateTime ServerCurrentTime { get; set; }
    public decimal MinimumProfitMarginDiscount { get; set; }
    public string ApiKey { get; set; }
}
