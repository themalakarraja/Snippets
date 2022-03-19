public static int addBrand(BrandDto brandDto)
{
    DBConnectionService conService = new DBConnectionService();
    string query = "INSERT INTO tbl_brand (" +
        "brand_id, brand_name, brand_status) " +
        "VALUES (" +
        "@brand_id, @brand_name, @brand_status)";
    Dictionary<string, object> parameters = new Dictionary<string, object>();
    parameters.Add("@brand_id", brandDto.brand_id);
    parameters.Add("@brand_name", brandDto.brand_name);
    parameters.Add("@brand_status", brandDto.brand_status);
    return conService.executeWithParams(query, parameters);
}

public static int updateBrandDetails(BrandDto brandDto)
{
    DBConnectionService conService = new DBConnectionService();
    string query = "UPDATE tbl_brand SET brand_name = @brand_name, " +
        "brand_status = @brand_status WHERE brand_id = @brand_id";
    Dictionary<string, object> parameters = new Dictionary<string, object>();
    parameters.Add("@brand_name", brandDto.brand_name);
    parameters.Add("@brand_status", brandDto.brand_status);
    parameters.Add("@brand_id", brandDto.brand_id);
    return conService.executeWithParams(query, parameters);
}

public static BrandDto getBrandDetails(string brand_id)
{
    BrandDto brandDetails = null;
    DBConnectionService conService = new DBConnectionService();
    string query = "SELECT brand_id, brand_name, brand_status " +
        "FROM tbl_brand " +
        "WHERE brand_id = '" + brand_id + "'";

    DataSet ds = conService.executeSelect(query);
    if (ds.Tables[0].Rows.Count > 0)
    {
        brandDetails = new BrandDto();
        brandDetails.setBrandDetails(ds.Tables[0].Rows[0]);
    }
    return brandDetails;
}

public static List<InvoiceDto> searchInvoice(string start_date, string end_date)
{
    InvoiceDto invoiceDetails = null;
    List<InvoiceDto> invoiceDetailsList = null;
    DBConnectionService conService = new DBConnectionService();

    string query = "SELECT invoice_no, invoice_date, client_name, contact_no, " +
        "payment_type, total_amount FROM tbl_invoice " +
        "WHERE (invoice_date >= '" + start_date + "' AND invoice_date <= '" + 
        end_date + "') ORDER BY invoice_date DESC";

    DataSet ds = conService.executeSelect(query);
    if (ds.Tables[0].Rows.Count > 0)
    {
        invoiceDetailsList = new List<InvoiceDto>();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            invoiceDetails = new InvoiceDto();
            invoiceDetails.setInvoiceDetails(row);
            invoiceDetailsList.Add(invoiceDetails);
        }
    }
    return invoiceDetailsList;
}

public static List<BrandDto> getBrandList(Constants.ROW_STATUS ROW_STATUS)
{
    BrandDto brandDetails = null;
    List<BrandDto> brandDetailsList = null;
    DBConnectionService conService = new DBConnectionService();
    string query = null;

    switch (ROW_STATUS)
    {
        case Constants.ROW_STATUS.ACTIVE:
            query = "SELECT brand_id, brand_name, brand_status " +
                "FROM tbl_brand WHERE brand_status = '1' ORDER BY brand_name ASC";
            break;
        case Constants.ROW_STATUS.INACTIVE:
            query = "SELECT brand_id, brand_name, brand_status " +
                "FROM tbl_brand WHERE brand_status = '0' ORDER BY brand_name ASC";
            break;
        case Constants.ROW_STATUS.ALL:
            query = "SELECT brand_id, brand_name, brand_status " +
                "FROM tbl_brand ORDER BY brand_name ASC";
            break;
    }

    DataSet ds = conService.executeSelect(query);
    if (ds.Tables[0].Rows.Count > 0)
    {
        brandDetailsList = new List<BrandDto>();
        foreach (DataRow row in ds.Tables[0].Rows)
        {
            brandDetails = new BrandDto();
            brandDetails.setBrandDetails(row);
            brandDetailsList.Add(brandDetails);
        }
    }
    return brandDetailsList;
}

public List<UniversityColorEntity> GetColorByUniversity(int universityId)
{
    List<UniversityColorEntity> universityList = null;
    try
    {
        UniversityColorEntity university = null;

        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add("@UniversityId", universityId);

        DataSet ds = executeSelectWithProcedure("spGetColorByUniversity", parameters);

        if (ds != null && ds.Tables[0].Rows.Count > 0)
        {
            universityList = new List<UniversityColorEntity>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                university = new UniversityColorEntity();
                university.ColorId = row["Id"].ToString();
                university.ColorName = row["Name"].ToString();
                university.PMSName = row["PMSName"].ToString();
                university.PMSHex = row["PMSHex"].ToString();
                university.UniversityId = row["SchoolId"].ToString();
                university.Active = Convert.ToInt32(row["Active"]);
                universityList.Add(university);
            }
        }
    }
    catch (Exception ex)
    {
        ExceptionLogging.SaveErrorToText(ex);
    }
    return universityList;
}
