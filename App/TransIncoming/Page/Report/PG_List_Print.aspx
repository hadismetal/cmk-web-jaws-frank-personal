<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true"  CodeFile="PG_List_Print.aspx.cs" Inherits="PG_Print" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div>
        <h1>Report Rekap Incoming Transfer</h1>
    </div>
    <div>
        <div style="float: left;">
            <table class="table-file">
                <tr>
			        <td class="text-label">Report DateTime</td>
			        <td>:</td>
			        <td id="report_datetime" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">Periode</td>
			        <td>:</td>
			        <td id="periode" runat="server" class="text-bold" />
		        </tr>
		        <tr>
			        <td class="text-label">Company Brand</td>
			        <td>:</td>
			        <td id="brand" runat="server" class="text-bold" />
		        </tr>
		        <tr>
			        <td class="text-label" style="vertical-align:top;">From <asp:Literal runat="server" ID="lit_type_from" /></td>
			        <td style="vertical-align:top;">:</td>
			        <td id="from" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label" style="vertical-align:top;">To <asp:Literal runat="server" ID="lit_type_to" /></td>
			        <td style="vertical-align:top;">:</td>
			        <td id="to" runat="server" class="text-bold" />
		        </tr>
	        </table>
        </div>
        <div style="float:left; padding:0 0 0 50px;">
            <table class="table-file">
                <tr>
			        <td class="text-label">Status <asp:Literal runat="server" ID="lit_status" /></td>
			        <td>:</td>
			        <td id="status" runat="server" class="text-bold" />
		        </tr>
		        <tr>
			        <td class="text-label">Product Model</td>
			        <td>:</td>
			        <td id="model" runat="server" class="text-bold" />
		        </tr>
		        <tr>
			        <td class="text-label">Product Item</td>
			        <td>:</td>
			        <td id="item" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">Search Product</td>
			        <td>:</td>
			        <td id="search" runat="server" class="text-bold" />
		        </tr>
	        </table>
        </div>
        <div style="clear:both"></div>
    </div>
    <div class="table-responsive">
        <asp:Table ID="tb" runat="server" CssClass="table-list" Font-Size="11px">
			
		</asp:Table>
    </div>
    <div>
        <div>
            <div style="float:right;">
                <br />
                <table style="border:solid 1px black; border-collapse:collapse;">
                    <tr>
                        <td style="width:150px;text-align:center;border:solid 1px black; ">Pembuat<br /><br /><br /><br /><br /></td>
                        <td style="width:150px;text-align:center;border:solid 1px black; ">Menyerahkan<br /><br /><br /><br /><br /></td>
                        <td style="width:150px;text-align:center;border:solid 1px black; ">Penerima<br /><br /><br /><br /><br /></td>
                    </tr>
                </table>
            </div>
            <div style="clear:both" />
        </div>
    </div>
</asp:Content>

