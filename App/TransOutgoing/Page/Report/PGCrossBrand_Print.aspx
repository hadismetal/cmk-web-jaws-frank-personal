<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true"  CodeFile="PGCrossBrand_Print.aspx.cs" Inherits="PG_Print" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
<div runat="server" id="div_Error"><asp:Literal runat="server" id="lit_ErrMsg" /></div>
<div runat="server" id="div_Print">
<div>
    <div>
        <h1>Report Kirim Barang Plain Gold</h1>
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
			        <td class="text-label">Company Brand</td>
			        <td>:</td>
			        <td id="brand" runat="server" class="text-bold" />
		        </tr>
		        <tr>
			        <td class="text-label">From <asp:Literal runat="server" ID="lit_type_from" /></td>
			        <td>:</td>
			        <td id="from" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">To <asp:Literal runat="server" ID="lit_type_to" /></td>
			        <td>:</td>
			        <td id="to" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">Status <asp:Literal runat="server" ID="lit_status" /></td>
			        <td>:</td>
			        <td id="status" runat="server" class="text-bold" />
		        </tr>
	        </table>
        </div>
        <div style="float:left; padding:0 0 0 50px;">
            <table class="table-file">
                <tr>
			        <td class="text-label">Approve Datetime</td>
			        <td>:</td>
			        <td id="tglkirim" runat="server" class="text-bold" />
		        </tr>
		        <tr>
			        <td class="text-label">Document #</td>
			        <td>:</td>
			        <td id="nomor" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">Courier</td>
			        <td>:</td>
			        <td id="kurir" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">Description</td>
			        <td>:</td>
			        <td id="keterangan" runat="server" class="text-bold" />
		        </tr>
	        </table>
        </div>
        <div style="clear:both"></div>
    </div>
    <div style="padding-top:15px;">
        <asp:table ID="tb" runat="server" CssClass="table-report">
			<asp:tableheaderrow>
				<asp:tableheadercell style="text-align:left; vertical-align:top;" width="40">No.</asp:tableheadercell>
                    <asp:tableheadercell style="text-align:left; vertical-align:top;" width="100">PLU</asp:tableheadercell>
                    <asp:tableheadercell style="text-align:left; vertical-align:top;" width="160">Model</asp:tableheadercell>
                    <asp:tableheadercell style="text-align:left; vertical-align:top;" width="100">Item</asp:tableheadercell>
					<asp:tableheadercell style="text-align:left; vertical-align:top;" width="60">Qty</asp:tableheadercell>
                    <asp:tableheadercell style="text-align:left; vertical-align:top;" width="60">Netto</asp:tableheadercell>
                    <asp:tableheadercell style="text-align:left; vertical-align:top;" width="60">Gross</asp:tableheadercell>
			        <asp:tableheadercell style="text-align:left; vertical-align:top;" width="120">Selling Price</asp:tableheadercell>
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

