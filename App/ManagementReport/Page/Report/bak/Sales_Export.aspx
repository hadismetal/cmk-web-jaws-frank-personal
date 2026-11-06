<%@ page language="C#" masterpagefile="~/MasterPageExport.master" autoeventwireup="true" codefile="Sales_Export.aspx.cs" inherits="Page_Report_Sales_Export" %>

<asp:content contentplaceholderid="content" runat="server">
    
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass="text-right">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass="text-right">Jumlah</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass="text-right">Discount</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass="text-right">Discount Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass="text-right">Discount Voucher</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass="text-right">Discount Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" CssClass="text-right">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-right">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-right">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-right">Net Weight (gr)</asp:TableHeaderCell>

                </asp:TableHeaderRow>
		</asp:table>
	</div>
    <hr />
    <div class="table-responsive">
            <asp:Table ID="tb2" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200" ColumnSpan="2" style="text-align: center;">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200" ColumnSpan="2" style="text-align: center;">Trade In / Exchange</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200" ColumnSpan="2" style="text-align: center;">Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200" ColumnSpan="2" style="text-align: center;">Net Omzet</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    <asp:Literal ID="paging_text" runat="server" />
</asp:content>

