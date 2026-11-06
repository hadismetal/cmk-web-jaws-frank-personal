using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.SAP_ZSDFM_SD_VOID_INTRFC;

public partial class SAPM
{
    public static ZsdfmSdVoidIntrfcResponse PostDataVoidSalesOrderToWs(SalesOrder d, string xBillNoSap)
    {
        string BillNoSap = xBillNoSap;
        string Message = "";
        string Process1 = "";
        string Process2 = "";
        string Process3 = "";
        string Process4 = "";
        string Process5 = "";
        string Process6 = "";
        string ReprocessProcess = "";
        string RunnoProcess = "";
        string SoNo = d.Nomor;
        string Status = "";

        string test = string.Empty;

        System.Net.NetworkCredential cred = new System.Net.NetworkCredential();

        ZSDFM_SD_VOID_INTRFC _proxy = new ZSDFM_SD_VOID_INTRFC();
        ZsdfmSdVoidIntrfc _input = new ZsdfmSdVoidIntrfc();
        ZsdfmSdVoidIntrfcResponse _output = new ZsdfmSdVoidIntrfcResponse();
        
        ZsdstVoidSales _void = new ZsdstVoidSales();
        {
            _void.BillNoSap = BillNoSap;
            _void.Message = Message;
            _void.Process1 = Process1;
            _void.Process2 = Process2;
            _void.Process3 = Process3;
            _void.Process4 = Process4;
            _void.Process5 = Process5;
            _void.Process6 = Process6;
            _void.ReprocessProcess = ReprocessProcess;
            _void.RunnoProcess = RunnoProcess;
            _void.SoNo = SoNo;
            _void.Status = Status;
            
        };

        List<ZsdstVoidSales> _listVoid = new List<ZsdstVoidSales>();

        _listVoid.Add(_void);

        _input.Invoice = _listVoid.ToArray();

        cred.UserName = SAP.xUser;
        cred.Password = SAP.xPwd;

        _proxy.PreAuthenticate = true;
        _proxy.Credentials = cred;
        
        _output = _proxy.ZsdfmSdVoidIntrfc(_input);
        
        return _output;
    }
    
}

