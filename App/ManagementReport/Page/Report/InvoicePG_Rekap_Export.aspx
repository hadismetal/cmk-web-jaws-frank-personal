<%@ Page Language="C#" masterpagefile="~/MasterPageExport.master" CodeFile="InvoicePG_Rekap_Export.aspx.cs" Inherits="Page_Report_InvoicePG_Rekap_Export" %>

<asp:Content ID="Content1"  ContentPlaceHolderID ="content" runat="server">
    
     <div style="clear: both"></div>
     <div class="table-responsive" style="overflow:hidden;">
        <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="140">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Voucher</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Weight</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
</asp:Content>