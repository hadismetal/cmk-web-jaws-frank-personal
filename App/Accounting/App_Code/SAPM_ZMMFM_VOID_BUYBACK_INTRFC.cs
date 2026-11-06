using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.SAP_ZMMFM_VOID_BUYBACK_INTRFC;

public partial class SAPM
{
    public static ZmmfmVoidBuybackIntrfcResponse PostDataVoidBuybackToWs(Resell d, string xPoNo)
    {
        string PoNo = xPoNo;

        string test = string.Empty;

        System.Net.NetworkCredential cred = new System.Net.NetworkCredential();

        ZMMFM_VOID_BUYBACK_INTRFC _proxy = new ZMMFM_VOID_BUYBACK_INTRFC();
        ZmmfmVoidBuybackIntrfc _input = new ZmmfmVoidBuybackIntrfc();
        ZmmfmVoidBuybackIntrfcResponse _output = new ZmmfmVoidBuybackIntrfcResponse();

        //ZmmstVoidBuybackIntrfc _void = new ZmmstVoidBuybackIntrfc();
        //{
        //    _void. = xPoNo;

        //};

        //List<ZmmstVoidBuybackIntrfc> _listVoid = new List<ZmmstVoidBuybackIntrfc>();

        //_listVoid.Add(_void);

        _input.PoNo = xPoNo;

        cred.UserName = SAP.xUser;
        cred.Password = SAP.xPwd;

        _proxy.PreAuthenticate = true;
        _proxy.Credentials = cred;
        
        _output = _proxy.ZmmfmVoidBuybackIntrfc(_input);
        
        return _output;
    }
    
}

