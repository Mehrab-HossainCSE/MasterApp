using System.Text.Json.Serialization;

namespace MasterApp.Application.SlaveDto;

public class CompanyInfoDto
{
    [JsonPropertyName("COMPANY_CODE")]
    public string COMPANY_CODE { get; set; }
    [JsonPropertyName("COMPANY_NAME")]
    public string COMPANY_NAME { get; set; }
    [JsonPropertyName("ADDRESS1")]
    public string ADDRESS1 { get; set; }
    [JsonPropertyName("ADDRESS2")]
    public string? ADDRESS2 { get; set; }
    [JsonPropertyName("POSTAL_CODE")]
    public string POSTAL_CODE { get; set; }
    [JsonPropertyName("CITY")]
    public string CITY { get; set; }
    [JsonPropertyName("COUNTRY")]
    public string COUNTRY { get; set; }
    [JsonPropertyName("OWNER_NAME")]
    public string OWNER_NAME { get; set; }
    [JsonPropertyName("VATREGNO")]
    public string? VATREGNO { get; set; }
    [JsonPropertyName("TIN")]
    public string? TIN { get; set; }
    [JsonPropertyName("TRADE_LICENSE_NO")]
    public string? TRADE_LICENSE_NO { get; set; }
    [JsonPropertyName("DRUG_LICENSE_NO")]
    public string? DRUG_LICENSE_NO { get; set; }
    [JsonPropertyName("CONTACT_NO")]
    public string CONTACT_NO { get; set; }
    [JsonPropertyName("EMAIL")]
    public string EMAIL { get; set; }
    [JsonPropertyName("WEBSITE")]
    public string? WEBSITE { get; set; }
    [JsonPropertyName("PREFIX")]
    public string? PREFIX { get; set; }
    [JsonPropertyName("DOE")]
    public DateTime DOE { get; set; }
    [JsonPropertyName("DATE_OF_EXPIRE")]
    public DateTime DATE_OF_EXPIRE { get; set; }
    [JsonPropertyName("NUM_OF_STORE")]
    public decimal NUM_OF_STORE { get; set; }
    [JsonPropertyName("NUM_OF_USER")]
    public decimal NUM_OF_USER { get; set; }
    [JsonPropertyName("CENTRALSTOREENABLED")]
    public decimal CENTRALSTOREENABLED { get; set; }
    [JsonPropertyName("CENTRALSTORESALE")]
    public decimal CENTRALSTORESALE { get; set; }
    [JsonPropertyName("SALE_VAT_PRCNT")]
    public decimal SALE_VAT_PRCNT { get; set; }
    [JsonPropertyName("CREDIT_SALES_ALLOW")]
    public decimal CREDIT_SALES_ALLOW { get; set; }
    [JsonPropertyName("NUM_OF_TERMINAL")]
    public decimal NUM_OF_TERMINAL { get; set; }
    [JsonPropertyName("BARCODE_LEN")]
    public decimal BARCODE_LEN { get; set; }
    [JsonPropertyName("LOGO_URL")]
    public string? LOGO_URL { get; set; }
    [JsonPropertyName("INV_TPL_ID")]
    public decimal INV_TPL_ID { get; set; }
    [JsonPropertyName("PO_TPL_ID")]
    public decimal PO_TPL_ID { get; set; }
    [JsonPropertyName("PR_TPL_ID")]
    public decimal PR_TPL_ID { get; set; }
    [JsonPropertyName("STATUS")]
    public string STATUS { get; set; }
    [JsonPropertyName("ENTRY_BY")]
    public string ENTRY_BY { get; set; }
    [JsonPropertyName("ENTRY_DATE")]
    public DateTime ENTRY_DATE { get; set; }
    [JsonPropertyName("UPDATED_BY")]
    public string UPDATED_BY { get; set; }
    [JsonPropertyName("UPDATED_DATE")]
    public DateTime UPDATED_DATE { get; set; }
    [JsonPropertyName("BARCODE_PRINT_OPT")]
    public string? BARCODE_PRINT_OPT { get; set; }
    [JsonPropertyName("MP_ENABLED")]
    public decimal? MP_ENABLED { get; set; }
    [JsonPropertyName("MEM_CRD_ENABLED")]
    public bool? MEM_CRD_ENABLED { get; set; }
    [JsonPropertyName("GIFT_VOUCHER_ENABLED")]
    public bool? GIFT_VOUCHER_ENABLED { get; set; }
    [JsonPropertyName("PUR_VAT_PRCNT")]
    public decimal PUR_VAT_PRCNT { get; set; }
    [JsonPropertyName("IS_PRICE_INCLD_VAT")]
    public bool IS_PRICE_INCLD_VAT { get; set; }
    [JsonPropertyName("IS_SUB_CAT_WISE_VARIANCE")]
    public bool IS_SUB_CAT_WISE_VARIANCE { get; set; }
    [JsonPropertyName("IS_SHOP_WISE_SAL_PRICE")]
    public bool IS_SHOP_WISE_SAL_PRICE { get; set; }
    [JsonPropertyName("IS_VAT_BEFORE_DISC")]
    public bool IS_VAT_BEFORE_DISC { get; set; }
    [JsonPropertyName("IS_CUSTOMER_WISE_PRICE")]
    public bool IS_CUSTOMER_WISE_PRICE { get; set; }
    [JsonPropertyName("IS_OVERWRITE_CD")]
    public bool IS_OVERWRITE_CD { get; set; }
    [JsonPropertyName("IS_BRAND_WISE_STORE")]
    public bool IS_BRAND_WISE_STORE { get; set; }
    [JsonPropertyName("ALLOW_CARTONWISE_SALE")]
    public bool ALLOW_CARTONWISE_SALE { get; set; }
    [JsonPropertyName("CPU_CHANGE_ON_RECEIVE")]
    public bool CPU_CHANGE_ON_RECEIVE { get; set; }
    [JsonPropertyName("MRP_CHANGE_ON_RECEIVE")]
    public bool MRP_CHANGE_ON_RECEIVE { get; set; }
    [JsonPropertyName("VENDOR_APPROVAL")]
    public bool VENDOR_APPROVAL { get; set; }
    [JsonPropertyName("STORE_REQ_APPROVAL")]
    public bool STORE_REQ_APPROVAL { get; set; }
    [JsonPropertyName("PO_APPROVAL")]
    public bool PO_APPROVAL { get; set; }
    [JsonPropertyName("PUR_RCV_APPROVAL")]
    public bool PUR_RCV_APPROVAL { get; set; }
    [JsonPropertyName("WH_DELIVERY_APPROVAL")]
    public bool WH_DELIVERY_APPROVAL { get; set; }
    [JsonPropertyName("STR_DELIVERY_APPROVAL")]
    public bool STR_DELIVERY_APPROVAL { get; set; }
    [JsonPropertyName("DML_APPROVAL")]
    public bool DML_APPROVAL { get; set; }
    [JsonPropertyName("PUR_RTN_APPROVAL")]
    public bool PUR_RTN_APPROVAL { get; set; }
    [JsonPropertyName("PRICE_CHANGE_APPROVAL")]
    public bool PRICE_CHANGE_APPROVAL { get; set; }
    [JsonPropertyName("PROMOTION_APPROVAL")]
    public bool PROMOTION_APPROVAL { get; set; }
    [JsonPropertyName("IS_SUB_SUBCAT_WISE_VARIANCE")]
    public bool? IS_SUB_SUBCAT_WISE_VARIANCE { get; set; }
    [JsonPropertyName("SHOW_SUB_SUBCATEGORY")]
    public bool SHOW_SUB_SUBCATEGORY { get; set; }
    [JsonPropertyName("COMPANY_CODE")]
    public string DEPARTMENT_LABEL { get; set; }
    [JsonPropertyName("SUB_DEPARTMENT_LABEL")]
    public string SUB_DEPARTMENT_LABEL { get; set; }
    [JsonPropertyName("CATEGORY_LABEL")]
    public string CATEGORY_LABEL { get; set; }
    [JsonPropertyName("COMPANY_CODE")]
    public string SUB_CATEGORY_LABEL { get; set; }
    [JsonPropertyName("SUB_SUBCATEGORY_LABEL")]
    public string SUB_SUBCATEGORY_LABEL { get; set; }
    [JsonPropertyName("SHOW_DEPARTMENT")]
    public bool SHOW_DEPARTMENT { get; set; }
    [JsonPropertyName("MIN_PROFIT_MAR_DISC")]
    public decimal MIN_PROFIT_MAR_DISC { get; set; }
    [JsonPropertyName("SDC_VAT_CODE")]
    public string? SDC_VAT_CODE { get; set; }
    [JsonPropertyName("SDC_SD_CODE")]
    public string? SDC_SD_CODE { get; set; }
    [JsonPropertyName("SDC_PKG_VAT_CODE")]
    public string? SDC_PKG_VAT_CODE { get; set; }
    [JsonPropertyName("SDC_PKG_SD_CODE")]
    public string? SDC_PKG_SD_CODE { get; set; }
    [JsonPropertyName("IS_MANUFACTURER_REQUIRED")]
    public bool? IS_MANUFACTURER_REQUIRED { get; set; }
    [JsonPropertyName("IS_STYLE_CODE_REQUIRED")]
    public bool? IS_STYLE_CODE_REQUIRED { get; set; }
    [JsonPropertyName("IS_MANAGE_ARTICLE")]
    public bool? IS_MANAGE_ARTICLE { get; set; }
    [JsonPropertyName("IS_BATCHWISE_RECEIVE")]
    public bool? IS_BATCHWISE_RECEIVE { get; set; }
    [JsonPropertyName("ALLOW_MANUAL_BATCH_INPUT")]
    public bool? ALLOW_MANUAL_BATCH_INPUT { get; set; }
    [JsonPropertyName("MANAGE_VENDORWISE_STOCK")]
    public bool? MANAGE_VENDORWISE_STOCK { get; set; }
    [JsonPropertyName("IS_IMEI_ENABLED")]
    public bool? IS_IMEI_ENABLED { get; set; }
    [JsonPropertyName("BNK_COMM_RDC_ON_SAL_RTN")]
    public bool? BNK_COMM_RDC_ON_SAL_RTN { get; set; }
    [JsonPropertyName("SHOW_CONSOLE")]
    public bool SHOW_CONSOLE { get; set; }
    [JsonPropertyName("VENDOR_ACCT_HEAD_ENABLED")]
    public bool? VENDOR_ACCT_HEAD_ENABLED { get; set; }
    [JsonPropertyName("VENDOR_ACCT_HEAD")]
    public string? VENDOR_ACCT_HEAD { get; set; }
    [JsonPropertyName("MRP_CHANGE_ON_PO")]
    public bool? MRP_CHANGE_ON_PO { get; set; }
    [JsonPropertyName("MANAGE_DONORWISE_STOCK")]
    public bool? MANAGE_DONORWISE_STOCK { get; set; }
    [JsonPropertyName("AREA_LABEL")]
    public string AREA_LABEL { get; set; }
    [JsonPropertyName("PRINT_DELIVERY_PICK_LIST")]
    public bool PRINT_DELIVERY_PICK_LIST { get; set; }
    [JsonPropertyName("IS_RANDOM_GV_SERIAL")]
    public bool IS_RANDOM_GV_SERIAL { get; set; }
    [JsonPropertyName("GV_SERIAL_LEN")]
    public int GV_SERIAL_LEN { get; set; }
    [JsonPropertyName("POINT_REDEEMP_OTP")]
    public bool? POINT_REDEEMP_OTP { get; set; }
    [JsonPropertyName("GV_REDEEMP_OTP")]
    public bool? GV_REDEEMP_OTP { get; set; }
    [JsonPropertyName("SMS_ON_INVOICE")]
    public bool? SMS_ON_INVOICE { get; set; }
    [JsonPropertyName("SMS_INCLD_INV_NO")]
    public bool? SMS_INCLD_INV_NO { get; set; }
    [JsonPropertyName("SMS_INCLD_INV_AMT")]
    public bool? SMS_INCLD_INV_AMT { get; set; }
    [JsonPropertyName("SMS_INCLD_EARN_POINT")]
    public bool? SMS_INCLD_EARN_POINT { get; set; }
    [JsonPropertyName("SMS_INCLD_REDEEMED_POINT")]
    public bool? SMS_INCLD_REDEEMED_POINT { get; set; }
    [JsonPropertyName("SMS_TEMPLATE")]
    public string? SMS_TEMPLATE { get; set; }
    [JsonPropertyName("VENDOR_WISE_CHALLAN_SL")]
    public bool? VENDOR_WISE_CHALLAN_SL { get; set; }
    [JsonPropertyName("CUSTOMER_REQ_ON_SALE")]
    public bool? CUSTOMER_REQ_ON_SALE { get; set; }
    [JsonPropertyName("CUSTOMER_TYPE_LABEL")]
    public string CUSTOMER_TYPE_LABEL { get; set; }
    [JsonPropertyName("PRODUCT_NAME_LABEL")]
    public string PRODUCT_NAME_LABEL { get; set; }
    [JsonPropertyName("PACK_SIZE_LABEL")]
    public string PACK_SIZE_LABEL { get; set; }
    [JsonPropertyName("STYLE_CODE_LABEL")]
    public string STYLE_CODE_LABEL { get; set; }
    [JsonPropertyName("MANAGE_PV_USER_BARCODE")]
    public bool MANAGE_PV_USER_BARCODE { get; set; }
    [JsonPropertyName("FIFO_METHOD")]
    public bool FIFO_METHOD { get; set; }
    [JsonPropertyName("ThirdPartyCreditCustomerEnable")]
    public bool ThirdPartyCreditCustomerEnable { get; set; }
    [JsonPropertyName("ThridPartyCCFor")]
    public string? ThridPartyCCFor { get; set; }
    [JsonPropertyName("ThirdPartyCC_Url")]
    public string? ThirdPartyCC_Url { get; set; }
    [JsonPropertyName("CUSTOMER_DISC_OTP_REQUIRED")]
    public bool CUSTOMER_DISC_OTP_REQUIRED { get; set; }
    [JsonPropertyName("IS_BATCH_AUTO_SERIAL")]
    public bool IS_BATCH_AUTO_SERIAL { get; set; }
    [JsonPropertyName("IS_REASON_TEXT")]
    public bool IS_REASON_TEXT { get; set; }
    [JsonPropertyName("IS_SHOW_VEHICLE_IN_DELIVERY")]
    public bool IS_SHOW_VEHICLE_IN_DELIVERY { get; set; }
    [JsonPropertyName("ENABLE_EXCESS_CREDIT_LIMIT")]
    public bool? ENABLE_EXCESS_CREDIT_LIMIT { get; set; }
    [JsonPropertyName("IS_REPRINT_DELIVERT_ORDER")]
    public bool IS_REPRINT_DELIVERT_ORDER { get; set; }
    [JsonPropertyName("IS_REPRINT_ALLOCATION_CHALLAN")]
    public bool IS_REPRINT_ALLOCATION_CHALLAN { get; set; }
    [JsonPropertyName("IS_REPRINT_GIFT_VOUCHER_GEN_CHALLAN")]
    public bool IS_REPRINT_GIFT_VOUCHER_GEN_CHALLAN { get; set; }
    [JsonPropertyName("ALLOW_PUR_DISCOUNT_CPU")]
    public bool ALLOW_PUR_DISCOUNT_CPU { get; set; }
    [JsonPropertyName("IS_ECOM_AUTO_TRANSFER")]
    public bool IS_ECOM_AUTO_TRANSFER { get; set; }
    [JsonPropertyName("IS_STORE_WISE_ORDER_LOADING")]
    public bool IS_STORE_WISE_ORDER_LOADING { get; set; }
    [JsonPropertyName("IS_MULTIPLE_MRP")]
    public bool IS_MULTIPLE_MRP { get; set; }
    [JsonPropertyName("IS_AUTO_SCAN_RECEIVE")]
    public bool IS_AUTO_SCAN_RECEIVE { get; set; }
    [JsonPropertyName("MULTI_VEN_NOT_VEN_WISE_STOCK")]
    public bool MULTI_VEN_NOT_VEN_WISE_STOCK { get; set; }
    [JsonPropertyName("IS_RCV_CHALLAN_STORE_DELIVERY")]
    public bool IS_RCV_CHALLAN_STORE_DELIVERY { get; set; }
    [JsonPropertyName("IS_TRNS_RCV_CHALLAN_STORE_DELIVERY")]
    public bool IS_TRNS_RCV_CHALLAN_STORE_DELIVERY { get; set; }
    [JsonPropertyName("VAT_INCLUDING_BARCODE")]
    public bool VAT_INCLUDING_BARCODE { get; set; }
    [JsonPropertyName("IS_SHOW_QRCODE_PRINT")]
    public bool IS_SHOW_QRCODE_PRINT { get; set; }
    [JsonPropertyName("PRODUCT_APPROVAL")]
    public bool PRODUCT_APPROVAL { get; set; }
    [JsonPropertyName("RECEIVE_CHALLAN_AUTO_DELIVERY")]
    public bool RECEIVE_CHALLAN_AUTO_DELIVERY { get; set; }
    [JsonPropertyName("MULTIPLE_VENDOR")]
    public bool MULTIPLE_VENDOR { get; set; }
    [JsonPropertyName("CPU_CHECKING_IN_STORE_DELIVERY")]
    public bool CPU_CHECKING_IN_STORE_DELIVERY { get; set; }
    [JsonPropertyName("SAL_BARCODE_WISE_PRICE_CHANGE")]
    public bool SAL_BARCODE_WISE_PRICE_CHANGE { get; set; }
    [JsonPropertyName("CRM_ENABLE")]
    public bool CRM_ENABLE { get; set; }
    [JsonPropertyName("CRM_FOR")]
    public string? CRM_FOR { get; set; }
    [JsonPropertyName("CRM_URL")]
    public string? CRM_URL { get; set; }
    [JsonPropertyName("CRM_USER")]
    public string? CRM_USER { get; set; }
    [JsonPropertyName("CRM_PASS")]
    public string? CRM_PASS { get; set; }
    [JsonPropertyName("MAIN_CHANNEL_LABEL")]
    public string MAIN_CHANNEL_LABEL { get; set; }
    [JsonPropertyName("ZONE_LABEL")]
    public string ZONE_LABEL { get; set; }
    [JsonPropertyName("CUSTOMER_GROUP_LABEL")]
    public string CUSTOMER_GROUP_LABEL { get; set; }
    [JsonPropertyName("CUSTOMER_CATEGORY_LABEL")]
    public string CUSTOMER_CATEGORY_LABEL { get; set; }
    [JsonPropertyName("CUSTOMER_SUB_CATEGORY_LABEL")]
    public string CUSTOMER_SUB_CATEGORY_LABEL { get; set; }
    [JsonPropertyName("DELIVERY_MAN_IN_STORE_DELIVERY")]
    public bool DELIVERY_MAN_IN_STORE_DELIVERY { get; set; }
    [JsonPropertyName("REF_NO_IN_STORE_DELIVERY")]
    public bool REF_NO_IN_STORE_DELIVERY { get; set; }
    [JsonPropertyName("SOFTWARE_VERSION")]
    public string SOFTWARE_VERSION { get; set; }
    [JsonPropertyName("SMS_ON_GV_SALE")]
    public bool SMS_ON_GV_SALE { get; set; }
    [JsonPropertyName("SMS_GV_INCLD_INV_NO")]
    public bool SMS_GV_INCLD_INV_NO { get; set; }
    [JsonPropertyName("SMS_GV_TEMPLATE")]
    public string? SMS_GV_TEMPLATE { get; set; }
    [JsonPropertyName("SHOP_CONNECTION_CHECK")]
    public bool SHOP_CONNECTION_CHECK { get; set; }
    [JsonPropertyName("DIRECT_RECEIVE")]
    public bool DIRECT_RECEIVE { get; set; }
    [JsonPropertyName("CPU_EDIT_ON_PUR_RTN")]
    public bool CPU_EDIT_ON_PUR_RTN { get; set; }
    [JsonPropertyName("REF_REQ_PUR_RCV")]
    public bool REF_REQ_PUR_RCV { get; set; }
    [JsonPropertyName("STORE_WISE_CUSTOMER_ON_SALE")]
    public bool STORE_WISE_CUSTOMER_ON_SALE { get; set; }
    [JsonPropertyName("PO_EMAIL")]
    public bool PO_EMAIL { get; set; }
    [JsonPropertyName("CPU_EDIT_ON_DMG_LOST")]
    public bool CPU_EDIT_ON_DMG_LOST { get; set; }
    [JsonPropertyName("RabbitMQ")]
    public bool RabbitMQ { get; set; }
    [JsonPropertyName("EC_OREDR_EDIT_BEFORE_DELIVERY")]
    public bool EC_OREDR_EDIT_BEFORE_DELIVERY { get; set; }
    [JsonPropertyName("CPU_CHECK_ON_CIRCULAR_PRICE_CHANGE")]
    public bool CPU_CHECK_ON_CIRCULAR_PRICE_CHANGE { get; set; }
    [JsonPropertyName("VENDOR_CODE_SHOW_VENDOR_DD")]
    public bool VENDOR_CODE_SHOW_VENDOR_DD { get; set; }
    [JsonPropertyName("SAL_VAT_PERCENT_IN_CATEGORY")]
    public bool SAL_VAT_PERCENT_IN_CATEGORY { get; set; }
    [JsonPropertyName("SAL_VAT_PERCENT_IN_SUB_SUBCATEGORY")]
    public bool SAL_VAT_PERCENT_IN_SUB_SUBCATEGORY { get; set; }
    [JsonPropertyName("FLEX_ORDER_PROCESS")]
    public bool FLEX_ORDER_PROCESS { get; set; }
    [JsonPropertyName("VENDOR_WISE_USER_BARCODE")]

    public bool VENDOR_WISE_USER_BARCODE { get; set; }
    [JsonPropertyName("Courier_Selection_on_Order_Created")]
    public bool Courier_Selection_on_Order_Created { get; set; }
    [JsonPropertyName("BRAND_LABEL")]
    public string BRAND_LABEL { get; set; }
    [JsonPropertyName("Store_Select_on_Order")]
    public bool Store_Select_on_Order { get; set; }
    [JsonPropertyName("OrderMargetoDelivery")]
    public bool OrderMargetoDelivery { get; set; }
    [JsonPropertyName("OrderCreatetoReq")]
    public bool OrderCreatetoReq { get; set; }
    [JsonPropertyName("CusAccountsHeadCreation")]
    public bool CusAccountsHeadCreation { get; set; }
    [JsonPropertyName("SupAccountsHeadCreation")]
    public bool SupAccountsHeadCreation { get; set; }
    [JsonPropertyName("SimpleQuickSearch")]
    public bool SimpleQuickSearch { get; set; }
    [JsonPropertyName("ReasonTextRequired")]
    public bool ReasonTextRequired { get; set; }
    [JsonPropertyName("SHOW_CUSTOMER_CATEGORY")]
    public bool SHOW_CUSTOMER_CATEGORY { get; set; }
    [JsonPropertyName("SHOW_SHOPTYPE")]
    public bool? SHOW_SHOPTYPE { get; set; }
    [JsonPropertyName("PUR_RCV_SHOP")]
    public bool? PUR_RCV_SHOP { get; set; }
    [JsonPropertyName("ENABLE_PHARMACY_COLLECTION_BOOTH")]
    public bool ENABLE_PHARMACY_COLLECTION_BOOTH { get; set; }
    [JsonPropertyName("PRODUCT_WISE_DISCOUNT")]
    public bool PRODUCT_WISE_DISCOUNT { get; set; }
    [JsonPropertyName("InvoiceUrlSms")]
    public bool InvoiceUrlSms { get; set; }
    [JsonPropertyName("CATEGORY_WISE_COST_PRICE_CHANGE")]
    public bool CATEGORY_WISE_COST_PRICE_CHANGE { get; set; }
    [JsonPropertyName("EFFECT_PRICE_CHANGE_ALL_TRANSECTION")]
    public bool EFFECT_PRICE_CHANGE_ALL_TRANSECTION { get; set; }
    [JsonPropertyName("RELEASE_VERSION")]
    public int RELEASE_VERSION { get; set; }
    [JsonPropertyName("MAIL_SENDING_IN_CUSTOMER_PRICE_SETUP")]
    public bool MAIL_SENDING_IN_CUSTOMER_PRICE_SETUP { get; set; }
    [JsonPropertyName("SMS_ON_PURCHASE_ORDER")]
    public bool SMS_ON_PURCHASE_ORDER { get; set; }
    [JsonPropertyName("CATEGORY_WISE_CIRCULAR_DISCOUNT")]
    public bool CATEGORY_WISE_CIRCULAR_DISCOUNT { get; set; }
    [JsonPropertyName("IsOrderTypeDefault")]
    public bool IsOrderTypeDefault { get; set; }
    [JsonPropertyName("CITY_BANK_INTEGRATION")]
    public bool CITY_BANK_INTEGRATION { get; set; }
    [JsonPropertyName("SAL_QTY_IN_DESIRED_STOCK")]
    public bool SAL_QTY_IN_DESIRED_STOCK { get; set; }
    [JsonPropertyName("SAL_PRICE_FROM_PUR_RCV_IN_RCV")]
    public bool SAL_PRICE_FROM_PUR_RCV_IN_RCV { get; set; }
    [JsonPropertyName("PUR_PRICE_FROM_PUR_RCV_IN_RCV")]
    public bool PUR_PRICE_FROM_PUR_RCV_IN_RCV { get; set; }
    [JsonPropertyName("SMS_INCLD_BALANCE_POINT")]
    public bool SMS_INCLD_BALANCE_POINT { get; set; }
    [JsonPropertyName("POINT_WITH_DISCOUNT")]
    public bool POINT_WITH_DISCOUNT { get; set; }
    [JsonPropertyName("QuickSearchWithStock")]
    public bool QuickSearchWithStock { get; set; }
    [JsonPropertyName("BARCODE_WISE_EXEC_ON_SALE")]
    public bool BARCODE_WISE_EXEC_ON_SALE { get; set; }
    [JsonPropertyName("AUTO_SERIAL_CUSTOMER_ID")]
    public bool AUTO_SERIAL_CUSTOMER_ID { get; set; }
    [JsonPropertyName("VENDOR_WISE_SALE_WITH_STOCK")]
    public bool VENDOR_WISE_SALE_WITH_STOCK { get; set; }
    [JsonPropertyName("SMS_INV_NO_TEXT")]
    public string? SMS_INV_NO_TEXT { get; set; }
    [JsonPropertyName("SMS_INV_AMT_TEXT")]
    public string? SMS_INV_AMT_TEXT { get; set; }
    [JsonPropertyName("SMS_EARN_POINT_TEXT")]
    public string? SMS_EARN_POINT_TEXT { get; set; }
    [JsonPropertyName("SMS_REDEEMED_POINT_TEXT")]
    public string? SMS_REDEEMED_POINT_TEXT { get; set; }
    [JsonPropertyName("SMS_BALANCE_POINT_TEXT")]
    public string? SMS_BALANCE_POINT_TEXT { get; set; }
    [JsonPropertyName("MONTH_WISE_PERIODICAL_STOCK_REPORT")]
    public bool MONTH_WISE_PERIODICAL_STOCK_REPORT { get; set; }
    [JsonPropertyName("SMS_TEMPLATE_END")]
    public string SMS_TEMPLATE_END { get; set; }
    [JsonPropertyName("NO_VAT_SAL")]
    public bool NO_VAT_SAL { get; set; }
    [JsonPropertyName("WEB_SALE")]
    public bool WEB_SALE { get; set; }
    [JsonPropertyName("DOCTOR_NAME_LABEL")]
    public string? DOCTOR_NAME_LABEL { get; set; }
    [JsonPropertyName("HOSPITAL_NAME_LABEL")]
    public string? HOSPITAL_NAME_LABEL { get; set; }
    [JsonPropertyName("FIELD1_LABEL")]
    public string? FIELD1_LABEL { get; set; }
    [JsonPropertyName("FIELD2_LABEL")]
    public string? FIELD2_LABEL { get; set; }
    [JsonPropertyName("SHOW_CUSTOMER_SEARCH")]
    public bool SHOW_CUSTOMER_SEARCH { get; set; }
    [JsonPropertyName("WITHOUT_PAYMENT")]
    public bool WITHOUT_PAYMENT { get; set; }
    [JsonPropertyName("ECOMMERCE_URL")]

    public string ECOMMERCE_URL { get; set; }
    [JsonPropertyName("WELCOME_SMS")]
    public bool WELCOME_SMS { get; set; }
    [JsonPropertyName("WELCOME_SMS_TEXT")]
    public string WELCOME_SMS_TEXT { get; set; }
    [JsonPropertyName("SDC_VAT_CODE_LABEL")]
    public string SDC_VAT_CODE_LABEL { get; set; }
    [JsonPropertyName("SDC_SD_CODE_LABEL")]
    public string SDC_SD_CODE_LABEL { get; set; }
    [JsonPropertyName("CPU_CHECK_IN_DISCOUNT")]
    public bool CPU_CHECK_IN_DISCOUNT { get; set; }
    [JsonPropertyName("CHK_STOCK_EDIT_ATTRIBUTE")]
    public bool CHK_STOCK_EDIT_ATTRIBUTE { get; set; }
    [JsonPropertyName("STORE_REQ_APPROVAL1_FOR_PO")]
    public bool STORE_REQ_APPROVAL1_FOR_PO { get; set; }
    [JsonPropertyName("SHOW_STOCK_IN_INVENTORY")]
    public bool SHOW_STOCK_IN_INVENTORY { get; set; }
    [JsonPropertyName("DISABLE_INVENTORY_ITEM_OPERATION")]
    public bool DISABLE_INVENTORY_ITEM_OPERATION { get; set; }
    [JsonPropertyName("COUNTRY_CODE_IN_PHONE")]
    public bool COUNTRY_CODE_IN_PHONE { get; set; }
    [JsonPropertyName("BARCODE_PREFIX")]
    public string BARCODE_PREFIX { get; set; }
    [JsonPropertyName("WEB_POS_VERSION")]
    public string? WEB_POS_VERSION { get; set; }
    [JsonPropertyName("VAT_PRO_ENABLE")]
    public bool VAT_PRO_ENABLE { get; set; }
    [JsonPropertyName("VAT_PRO_URL")]
    public string VAT_PRO_URL { get; set; }
    [JsonPropertyName("VAT_PRO_USER")]
    public string VAT_PRO_USER { get; set; }
    [JsonPropertyName("VAT_PRO_PASS")]
    public string VAT_PRO_PASS { get; set; }
    [JsonPropertyName("CHECK_STOCK_REQ_APPROVAL")]
    public bool CHECK_STOCK_REQ_APPROVAL { get; set; }
    [JsonPropertyName("STORE_WISE_CUSTOMER_TYPE")]
    public bool? STORE_WISE_CUSTOMER_TYPE { get; set; }
     [JsonPropertyName("CUSTOMER_PHONE_ELEVEN_DIGIT")]
    public bool? CUSTOMER_PHONE_ELEVEN_DIGIT { get; set; }
    [JsonPropertyName("DELIVERY_METHOD")]
    public string? DELIVERY_METHOD { get; set; }
    [JsonPropertyName("OFFLINE_SALE")]
    public bool OFFLINE_SALE { get; set; }
    [JsonPropertyName("SMS_BIRTHDAY_TEMPLATE")]
    public string SMS_BIRTHDAY_TEMPLATE { get; set; }
    [JsonPropertyName("CUSTOMER_WISE_INVOICE_SERIAL")]
    public bool CUSTOMER_WISE_INVOICE_SERIAL { get; set; }
    [JsonPropertyName("SMS_BIRTHDAY")]
    public bool SMS_BIRTHDAY { get; set; }
    [JsonPropertyName("PUR_VAT_BFOR_DISC_FOR_SHOP")]
    public bool PUR_VAT_BFOR_DISC_FOR_SHOP { get; set; }
    [JsonPropertyName("EXPIRED_STOCK_TRANSFER")]
    public bool EXPIRED_STOCK_TRANSFER { get; set; }
    [JsonPropertyName("STORE_REQ_APPROVAL3")]
    public bool STORE_REQ_APPROVAL3 { get; set; }
    [JsonPropertyName("PRODUCTION_SETUP")]
    public bool? PRODUCTION_SETUP { get; set; }
    [JsonPropertyName("HerlanApiIntegration")]
    public string HerlanApiIntegration { get; set; }
    [JsonPropertyName("PRINT_COUPON_INVOICE")]
    public string PRINT_COUPON_INVOICE { get; set; }
    [JsonPropertyName("Herlan_API_BASE_URL")]
    public string? Herlan_API_BASE_URL { get; set; }
    [JsonPropertyName("Herlan_API_EMAIL")]
    public string? Herlan_API_EMAIL { get; set; }
    [JsonPropertyName("Herlan_API_PASSWORD")]
    public string? Herlan_API_PASSWORD { get; set; }
    [JsonPropertyName("VAT_TYPE_REQUIRED_FOR_MIS")]
    public bool VAT_TYPE_REQUIRED_FOR_MIS { get; set; }
    [JsonPropertyName("IS_ORDER_AUTO_TRANSFER_BACKGROUND")]
    public bool IS_ORDER_AUTO_TRANSFER_BACKGROUND { get; set; }
    [JsonPropertyName("PRINT_DELIVERY_CHALLAN")]
    public bool PRINT_DELIVERY_CHALLAN { get; set; }
    [JsonPropertyName("BUSINESS_TYPE")]
    public string BUSINESS_TYPE { get; set; }
    [JsonPropertyName("GPStarEnable")]
    public string GPStarEnable { get; set; }
    [JsonPropertyName("GPStarUrl")]
    public string? GPStarUrl { get; set; }
    [JsonPropertyName("GPStarCustomerKey")]
    public string? GPStarCustomerKey { get; set; }
    [JsonPropertyName("GPStarCustomerSecret")]
    public string? GPStarCustomerSecret { get; set; }
    [JsonPropertyName("VSChallanBaseUrl")]
    public string? VSChallanBaseUrl { get; set; }
    [JsonPropertyName("VSChallanClientId")]
    public string? VSChallanClientId { get; set; }
    [JsonPropertyName("VSChallanClientSecret")]
    public string? VSChallanClientSecret { get; set; }
    [JsonPropertyName("VSChallanEnable")]
    public bool? VSChallanEnable { get; set; }
    [JsonPropertyName("LicenseKey")]
    public string LicenseKey { get; set; }
}
