<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true"  CodeFile="DJ_PrintWO.aspx.cs" Inherits="DJ_Print" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
<div runat="server" id="div_Error"><asp:Literal runat="server" id="lit_ErrMsg" /></div>
<div runat="server" id="div_Print">
<style type="text/css">
    table { page-break-inside:auto }
    tr    { page-break-inside:avoid; page-break-after:auto }
    table.print-friendly tr td, table.print-friendly tr th {
        page-break-inside: avoid;
    }
</style>
<div>
    <div>
        <h1>DO Lebur</h1>
    </div>
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
                        <asp:tableheadercell width="140">No. Dokumen</asp:tableheadercell>
                        <asp:tableheadercell width="85">PLU</asp:tableheadercell>
                        <asp:tableheadercell width="120">Item</asp:tableheadercell>
                        <asp:tableheadercell width="65">Material</asp:tableheadercell>
                        <asp:tableheadercell width="40">Qty</asp:tableheadercell>
                        <asp:tableheadercell width="60">Kadar</asp:tableheadercell>
						<asp:tableheadercell width="60">Gross</asp:tableheadercell>
                        <asp:tableheadercell width="60">Netto</asp:tableheadercell>
                        <asp:tableheadercell width="150">Nama Parcel</asp:tableheadercell>
                        <asp:tableheadercell width="50">Butir</asp:tableheadercell>
                        <asp:tableheadercell width="50">Carat</asp:tableheadercell>
                        <asp:TableHeaderCell Width="100">NO GIA</asp:TableHeaderCell>
            </asp:tableheaderrow>
		</asp:table>
    </div>
    <div>
        <div>
            
        </div>
        <div>
            <div style="float:right;">
                <br />
                <table style="border:solid 1px black; border-collapse:collapse;">
                    <tr>
                        <td style="width:150px;text-align:center;border:solid 1px black; ">Pembuat<br /><br /><br /><br /><br /></td>
                        <td style="width:150px;text-align:center;border:solid 1px black; ">Mengetahui<br /><br /><br /><br /><br /></td>
                        <td style="width:150px;text-align:center;border:solid 1px black; ">Penerima<br /><br /><br /><br /><br /></td>
                    </tr>
                </table>
            </div>
            <div style="clear:both" />
        </div>
    </div>
</div>
</div>
</asp:Content>

