<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PaymentDP_POPrint.aspx.cs" Inherits="Page_Create_Payment_PaymentDP_POPrint" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">

   <title>Sales Invoice</title>
    <style id="PO_Form">
        
        .xl004
        {
	       font-size : 22px;
	        font-family: Arial, sans-serif;
	        text-align: center;
	        vertical-align:middle;
	        text-align: center;
        }
        .xl005
        {
	        font-size : 13px;
	        font-family: Arial, sans-serif;
	        text-align: left;
	        vertical-align:top;
            
        }
        .xl006
        {
	        text-align : left;
	        font-size:15px;
        }
        .xl007
        {
	        font-size : 14px;
	        font-family: Arial, sans-serif;
	        text-align : center;
	        vertical-align:middle;
        }
        .xl008
        {
	        font-size : 16px;
	        font-family: Arial, sans-serif;
	        text-align : left;
	        vertical-align:middle;
            border-bottom: thin dotted;
        }

        .xl009
        {
	        font-size : 15px;
	        font-family: Arial, sans-serif;
	        text-align  : center;
	        vertical-align:top;
	        border-top: 0.6px solid windowtext;
            padding-top : 15px;
	
        }
        .xl010
        {
	        font-size : 15px;
	        font-family: Arial, sans-serif;
	        text-align : center;
	        vertical-align:middle;
	        border-top: 1.0px solid windowtext;
	        border-bottom: 1.0px solid windowtext;
	        border-right: 1.0px solid windowtext;
	        border-left: 1.0px solid windowtext;
        }

        .xl011
        {
	        border-left: 1.0px solid windowtext;
        }

        .xl012
        {
	        border-left: 1.0px solid windowtext;
	        border-top: 1.0px solid windowtext;
        }

        .xl013
        {
	        border-left: 1.0px solid windowtext;
	        border-bottom: 1.0px solid windowtext;
        }

        .xl014
        {
	        border-bottom: 1.0px solid windowtext;
        }

        .xl015
        {
	        border-right: 1.0px solid windowtext;
	        border-bottom: 1.0px solid windowtext;
        }

        .xl016
        {
	        border-right: 1.0px solid windowtext;
        }
        .xl017
        {
            border-top: 1.0px solid windowtext;
	        border-right: 1.0px solid windowtext;
            text-align:left;
        }
        .xl018
        {
	        border-top: 1.0px solid windowtext;
        }
        
        .xl019
        {
	        border-bottom: thin dotted;
        }
        .xl020 
        {
            border-top: 0.5px solid windowtext;
	        border-right: 0.5px solid windowtext;
            border-left: 0.5px solid windowtext;
	        border-bottom: 0.5px solid windowtext;
        }

        .table-no, .table-no td {
            border: 0px solid #000 !important;
            border-spacing :0;
            
        }

        .noborder, .noborder tr, .noborder td {
            border: none !important;
        }

    </style>
</head>
<body>
    <div>
    <div>
    <form id="form1" runat="server">
        <table width="800" border="0" cellspacing="0" style="margin:0 auto;">
          <tr>
            <td width="43">&nbsp;</td>
            <td width="43" class="xl012">&nbsp;</td>
            <td width="63" class="xl018">&nbsp;</td>
            <td width="60" class="xl018">&nbsp;</td>
            <td width="27" class="xl018">&nbsp;</td>
            <td width="90" class="xl018">&nbsp;</td>
            <td width="55" class="xl018">&nbsp;</td>
            <td width="77" class="xl018">&nbsp;</td> 
            <td width="77" class="xl018">&nbsp;</td>             
            <td colspan="3" class="xl017">
<%--                <asp:Label ID="Label1" runat="server" Text="Code No :"></asp:Label>--%>
                <asp:Label ID="no_po" runat="server"></asp:Label>                
            </td>
            <%--<td width="31" >&nbsp;</td>--%>
            <td width="40">&nbsp;</td>
          </tr>
          <tr>
            <td height="55">&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="3" rowspan="2">
                <asp:image runat="server" id="img_Logo" style="display:block; max-width: 66px; max-height: 66px; padding: 0; margin: 0 auto" />
            </td>
            <td colspan="4" rowspan="2" align="center" class="xl004"><strong><u> PURCHASE ORDER FORM</u> </strong></td>
            <td colspan="2" class="xl010"></td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <br />
          <tr>
            <td height="29">&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td width="85">&nbsp;</td>
            <td width="55">&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">Store Location</td>
            <td class="xl007">:</td>
            <td colspan="6" class="xl019">
                <asp:Label ID="location" runat="server"></asp:Label>
            </td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">Ph. number</td>
            <td class="xl007">:</td>
            <td colspan="6" class="xl019">
                <asp:Label ID="phnum" runat="server"></asp:Label>
             </td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">Customer Full Name</td>
            <td class="xl007">:</td>
            <td colspan="6" class="xl019">
                <asp:Label ID="customer" runat="server"></asp:Label>
            </td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">Ph. number</td>
            <td class="xl007">:</td>
            <td colspan="6" class="xl019">
                <asp:Label ID="phcust" runat="server"></asp:Label>
             </td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
		  <tr runat="server" id="rowgwp">
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005"><small>*Free GWP</small></td>
            <td class="xl007">:</td>
            <td colspan="6" class="xl019">
                <asp:Label ID="freegwp" runat="server"></asp:Label>
             </td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="3" class="xl006"><strong>PRODUCT DETAILS</strong></td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">Item</td>
            <td class="xl007">:</td>
            <td colspan="6" class="xl019">
                <asp:Label ID="lbitem" runat="server"></asp:Label>
             </td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">PLU 1</td>
            <td class="xl007">:</td>
            <td colspan="2" class="xl019">
                <asp:Label ID="lbplu1" runat="server"></asp:Label>
            </td>
            <td class="xl005">Barcode Pr</td>
            <td class="xl007">:</td>
            <td colspan="2" class="xl019">
                <asp:Label ID="brplu1" runat="server"></asp:Label>
            </td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">PLU 2</td>
            <td class="xl007">:</td>
            <td colspan="2" class="xl019">
                <asp:Label ID="lbplu2" runat="server"></asp:Label>
            </td>
            <td class="xl005">Barcode Pr</td>
            <td class="xl007">:</td>
            <td colspan="2" class="xl019">
                 <asp:Label ID="brplu2" runat="server"></asp:Label>
            </td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">Frame color</td>
            <td class="xl007">:</td>
            <td colspan="4" class="xl008" > 
                <asp:Label ID="lbframe" runat="server"></asp:Label>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
         
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">Gold Weight</td>
            <td class="xl007">:</td>
            <td>Female</td>
            <td class="xl019">
                <asp:Label ID="lbgwf" runat="server"></asp:Label>
            </td>
            <td>Gram</td>
            <td>;</td>
            <td>Male</td>
            <td class="xl019">
                <asp:Label ID="lbgwm" runat="server"></asp:Label>
            </td>
            <td class="xl016">Gram</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">Diamond Carat</td>
            <td class="xl007">:</td>
            <td>Female</td>
            <td class="xl019">
                <asp:Label ID="lbdwf" runat="server"></asp:Label>
            </td>
            <td>Carat</td>
            <td>;</td>
            <td>Male</td>
            <td class="xl019">
                <asp:Label ID="lbdwm" runat="server"></asp:Label>
            </td>
            <td class="xl016">Carat</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">Ring Size</td>
            <td class="xl007">:</td>
            <td>Female</td>
            <td class="xl019">
                <asp:Label ID="lbrsf" runat="server"></asp:Label>
            </td>
              <td>&nbsp;</td>
            <td>;</td>
            <td>Male</td>
            <td class="xl019">
                <asp:Label ID="lbrsm" runat="server"></asp:Label>
            </td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">Inscription</td>
            <td class="xl007">:</td>
            <td>Female</td>
            <td colspan="5" class="xl019"><asp:Label ID="inscf" runat="server"></asp:Label></td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>Male</td>
            <td colspan="5" class="xl019"><asp:Label ID="inscm" runat="server"></asp:Label></td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">Font Type</td>
            <td class="xl007">:</td
            >
            <td colspan="6" class="xl008"><asp:Label ID="lbfont" runat="server"></asp:Label></td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">Date of Order</td>
            <td class="xl007">:</td>
            <td colspan="3" class="xl019"><asp:Label ID="lbdorder" runat="server"></asp:Label></td>
            <td colspan="3"></td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">Date of Delivery</td>
            <td class="xl007">:</td>
            <td colspan="3" class="xl019"><asp:Label ID="lbddeliver" runat="server"></asp:Label></td>
            <td colspan="3"></td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
		  <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">Keterangan</td>
            <td class="xl007">:</td>
            <td colspan="6" class="xl019"><asp:Label ID="lbketerangan" runat="server"></asp:Label></td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="2" class="xl005">Sketch</td>
            <td class="xl007">:</td>
            <td colspan="6">
            </td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td colspan="6" rowspan="6" class="xl020"></td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="3">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td colspan="3">&nbsp;</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td colspan="3" class="xl009">
                <asp:Label ID="lbcustomer" runat="server"></asp:Label>
            </td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td colspan="3" class="xl009">
                <asp:Label ID="lbjr" runat="server"></asp:Label>
            </td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl011">&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            <td colspan="3" class="xl005">Phone No. : ........................</td>
            <td class="xl016">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
          <tr>
            <td>&nbsp;</td>
            <td class="xl013">&nbsp;</td>
            <td class="xl014">&nbsp;</td>
            <td class="xl014">&nbsp;</td>
            <td class="xl014">&nbsp;</td>
            <td class="xl014">&nbsp;</td>
            <td class="xl014">&nbsp;</td>
            <td class="xl014">&nbsp;</td>
            <td class="xl014">&nbsp;</td>
            <td class="xl014">&nbsp;</td>
            <td class="xl014">&nbsp;</td>
            <td class="xl015">&nbsp;</td>
            <td>&nbsp;</td>
          </tr>
        </table>
        <asp:Label ID="Label2" runat="server"  ForeColor="Red"></asp:Label>
    </form>
</div>
</div>
</body>
</html>
