using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.SAP_ZSDFM_CUSTOMER_INTRFC;

public partial class SAPM
{
    public static ZsdfmCustomerIntrfcResponse PostDataCustomerToWs(SalesOrder d)
    {
        string CustAccAssgnGroup = "";
        string CustAddress = (d.DataCustomer.AddrAlamat != null ? d.DataCustomer.AddrAlamat : "");
        string CustBirthPlace = "";
        string CustCity = "";
        string CustCompCode = "CMK";
        string CustCountry = "ID";
        string CustCurrency = "";
        string CustDeathDate = "";
        string CustDistributionChannel = "RT";
        string CustDistrict = "";
        string CustDivision = "GL";
        string CustDob = (d.DataCustomer.TglLahir != null ? d.DataCustomer.TglLahir.Value.ToString("dd-MM-yyyy") : "");
        string CustEmail = "";
        string CustEmployer = "";
        string CustFname = "";
        string CustGroup = "Z001";
        string CustLanguage = "EN";
        string CustLname = d.CustomerNama;
        string CustMarital = "";
        string CustNo = d.DataCustomer.NoCustomer;
        string CustNoSap = "";
        string CustNpwp = "";
        string CustPaymentTerm = "C001";
        string CustPostal = "";
        string CustPricingProcedure = "1";
        string CustPropertRegime = (d.DataCustomer.Agama != null ? d.DataCustomer.Agama.Value.ToString() : "");
        string CustReconAccount = "";
        string CustSalesOrg = "CMK";
        string CustSearchTerm = "";
        string CustSex = "";
        string CustTaxClass = "1";
        string CustTelephone = "";
        
        string test = string.Empty;

        System.Net.NetworkCredential cred = new System.Net.NetworkCredential();

        ZSDFM_CUSTOMER_INTRFC _proxy = new ZSDFM_CUSTOMER_INTRFC();
        ZsdfmCustomerIntrfc _input = new ZsdfmCustomerIntrfc();
        ZsdfmCustomerIntrfcResponse _output = new ZsdfmCustomerIntrfcResponse();
        
        ZsdstCustomer _cust = new ZsdstCustomer();
        {
            _cust.CustAccAssgnGroup = CustAccAssgnGroup;
            _cust.CustAddress = CustAddress;
            _cust.CustBirthPlace = CustBirthPlace;
            _cust.CustCity = CustCity;
            _cust.CustCompCode = CustCompCode;
            _cust.CustCountry = CustCountry;
            _cust.CustCurrency = CustCurrency;
            _cust.CustDeathDate = CustDeathDate;
            _cust.CustDistributionChannel = CustDistributionChannel;
            _cust.CustDistrict = CustDistrict;
            _cust.CustDivision = CustDivision;
            _cust.CustDob = CustDob;
            _cust.CustEmail = CustEmail;
            _cust.CustEmployer = CustEmployer;
            _cust.CustFname = CustFname;
            _cust.CustGroup = CustGroup;
            _cust.CustLanguage = CustLanguage;
            _cust.CustLname = CustLname;
            _cust.CustMarital = CustMarital;
            _cust.CustNo = CustNo;
            _cust.CustNoSap = CustNoSap;
            _cust.CustNpwp = CustNpwp;
            _cust.CustPaymentTerm = CustPaymentTerm;
            _cust.CustPostal = CustPostal;
            _cust.CustPricingProcedure = CustPricingProcedure;
            _cust.CustPropertRegime = CustPropertRegime;
            _cust.CustReconAccount = CustReconAccount;
            _cust.CustSalesOrg = CustSalesOrg;
            _cust.CustSearchTerm = CustSearchTerm;
            _cust.CustSex = CustSex;
            _cust.CustTaxClass = CustTaxClass;
            _cust.CustTelephone = CustTelephone;
            _cust.Message = "";
            _cust.Status = "";
        };

        List<ZsdstCustomer> _listCust = new List<ZsdstCustomer>();

        _listCust.Add(_cust);

        _input.Customer = _listCust.ToArray();

        cred.UserName = SAP.xUser;
        cred.Password = SAP.xPwd;

        _proxy.PreAuthenticate = true;
        _proxy.Credentials = cred;
        
        _output = _proxy.ZsdfmCustomerIntrfc(_input);
        
        return _output;
    }
    public static ZsdfmCustomerIntrfcResponse PostDataCustomerToWs(Resell d)
    {
        string CustAccAssgnGroup = "";
        string CustAddress = (d.DataCustomer.AddrAlamat != null ? d.DataCustomer.AddrAlamat : "");
        string CustBirthPlace = "";
        string CustCity = "";
        string CustCompCode = "CMK";
        string CustCountry = "ID";
        string CustCurrency = "";
        string CustDeathDate = "";
        string CustDistributionChannel = "RT";
        string CustDistrict = "";
        string CustDivision = "GL";
        string CustDob = (d.DataCustomer.TglLahir != null ? d.DataCustomer.TglLahir.Value.ToString("dd-MM-yyyy") : "");
        string CustEmail = "";
        string CustEmployer = "";
        string CustFname = "";
        string CustGroup = "Z001";
        string CustLanguage = "EN";
        string CustLname = d.CustomerNama;
        string CustMarital = "";
        string CustNo = d.DataCustomer.NoCustomer;
        string CustNoSap = "";
        string CustNpwp = "";
        string CustPaymentTerm = "C001";
        string CustPostal = "";
        string CustPricingProcedure = "1";
        string CustPropertRegime = (d.DataCustomer.Agama != null ? d.DataCustomer.Agama.Value.ToString() : "");
        string CustReconAccount = "";
        string CustSalesOrg = "CMK";
        string CustSearchTerm = "";
        string CustSex = "";
        string CustTaxClass = "1";
        string CustTelephone = "";

        string test = string.Empty;

        System.Net.NetworkCredential cred = new System.Net.NetworkCredential();

        ZSDFM_CUSTOMER_INTRFC _proxy = new ZSDFM_CUSTOMER_INTRFC();
        ZsdfmCustomerIntrfc _input = new ZsdfmCustomerIntrfc();
        ZsdfmCustomerIntrfcResponse _output = new ZsdfmCustomerIntrfcResponse();

        ZsdstCustomer _cust = new ZsdstCustomer();
        {
            _cust.CustAccAssgnGroup = CustAccAssgnGroup;
            _cust.CustAddress = CustAddress;
            _cust.CustBirthPlace = CustBirthPlace;
            _cust.CustCity = CustCity;
            _cust.CustCompCode = CustCompCode;
            _cust.CustCountry = CustCountry;
            _cust.CustCurrency = CustCurrency;
            _cust.CustDeathDate = CustDeathDate;
            _cust.CustDistributionChannel = CustDistributionChannel;
            _cust.CustDistrict = CustDistrict;
            _cust.CustDivision = CustDivision;
            _cust.CustDob = CustDob;
            _cust.CustEmail = CustEmail;
            _cust.CustEmployer = CustEmployer;
            _cust.CustFname = CustFname;
            _cust.CustGroup = CustGroup;
            _cust.CustLanguage = CustLanguage;
            _cust.CustLname = CustLname;
            _cust.CustMarital = CustMarital;
            _cust.CustNo = CustNo;
            _cust.CustNoSap = CustNoSap;
            _cust.CustNpwp = CustNpwp;
            _cust.CustPaymentTerm = CustPaymentTerm;
            _cust.CustPostal = CustPostal;
            _cust.CustPricingProcedure = CustPricingProcedure;
            _cust.CustPropertRegime = CustPropertRegime;
            _cust.CustReconAccount = CustReconAccount;
            _cust.CustSalesOrg = CustSalesOrg;
            _cust.CustSearchTerm = CustSearchTerm;
            _cust.CustSex = CustSex;
            _cust.CustTaxClass = CustTaxClass;
            _cust.CustTelephone = CustTelephone;
            _cust.Message = "";
            _cust.Status = "";
        };

        List<ZsdstCustomer> _listCust = new List<ZsdstCustomer>();

        _listCust.Add(_cust);

        _input.Customer = _listCust.ToArray();

        cred.UserName = SAP.xUser;
        cred.Password = SAP.xPwd;

        _proxy.PreAuthenticate = true;
        _proxy.Credentials = cred;

        _output = _proxy.ZsdfmCustomerIntrfc(_input);

        return _output;
    }

}

