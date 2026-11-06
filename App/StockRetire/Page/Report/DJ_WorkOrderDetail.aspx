<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true"  CodeFile="DJ_WorkOrderDetail.aspx.cs" Inherits="DJ_Print" %>

<asp:content ContentPlaceHolderID="h1" runat="server">
	DO Lebur
</asp:content>
<asp:content ContentPlaceHolderID="h1_st" runat="server">
	Report
</asp:content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    

<div runat="server" id="div_Error"><asp:Literal runat="server" id="lit_ErrMsg" /></div>
<div runat="server" id="div_Print"></div>
<style type="text/css">
    table { page-break-inside:auto }
    tr    { page-break-inside:avoid; page-break-after:auto }
    table.print-friendly tr td, table.print-friendly tr th {
        page-break-inside: avoid;
    }
</style>
    
<form id="Form1" runat="server">
<div>
    <div>
        <div style="float:left;">
            <table class="table-file">
                <tr>
			        <td class="text-label">Report Datetime</td>
			        <td>:</td>
			        <td id="report_datetime" runat="server" class="text-bold" />
		        </tr>
		        <tr>
			        <td class="text-label">No. Work Order</td>
			        <td>:</td>
			        <td id="nomor" runat="server" class="text-bold" />
	        </table>
        </div>
        <div style="float:left; padding:0 0 0 50px;">
            <table class="table-file">
                <tr>
			        <td class="text-label">Operator Datetime</td>
			        <td>:</td>
			        <td id="operatortgl" runat="server" class="text-bold" />
		        </tr>
		        <tr>
			        <td class="text-label">Operator</td>
			        <td>:</td>
			        <td id="operator" runat="server" class="text-bold" />
		        </tr>
	        </table>
        </div>
        <div style="clear:both"></div>
    </div>
    <div style="padding-top:15px;">
        <asp:table id="tb" runat="server" cssclass="table-report">
			<asp:tableheaderrow>
                        <asp:tableheadercell width="100"></asp:tableheadercell>
                        <asp:tableheadercell width="140">No. Dokumen</asp:tableheadercell>
                        <asp:tableheadercell width="120">PLU</asp:tableheadercell>
                        <asp:tableheadercell width="120">Item</asp:tableheadercell>
                        <asp:tableheadercell width="80">Material</asp:tableheadercell>
                        <asp:tableheadercell width="80">Quantity</asp:tableheadercell>
                        <asp:tableheadercell width="80">Kadar</asp:tableheadercell>
						<asp:tableheadercell width="80">Berat Bruto</asp:tableheadercell>
                        <asp:tableheadercell width="80">Berat Netto</asp:tableheadercell>

                        <asp:tableheadercell width="150">Nama Parcel</asp:tableheadercell>
                        <asp:tableheadercell width="80">Butir</asp:tableheadercell>
                        <asp:tableheadercell width="80">Carat</asp:tableheadercell>
                        <asp:TableHeaderCell Width="150">NO GIA</asp:TableHeaderCell>
            </asp:tableheaderrow>
		</asp:table>
    </div>
    <div>
        <div class="row" style="padding-left: 20px ">
            <br class="visible-sm visible-xs" />
            <button id="print" runat="server" class="btn btn-primary" onserverclick="print_ServerClick">
                <i class="icon-print"></i>
                Print
            </button>
            <button id="btnVoid" runat="server" class="btn btn-danger" onserverclick="btnVoid_ServerClick">
                <i class="icon-trash"></i>
                Void
            </button>
        </div>
    </div>
</div>
</form>
</asp:Content>

