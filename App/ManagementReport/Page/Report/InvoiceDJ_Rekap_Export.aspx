<%@ Page Language="C#" masterpagefile="~/MasterPageExport.master" CodeFile="InvoiceDJ_Rekap_Export.aspx.cs" Inherits="Page_Report_InvoiceDJ_Rekap_Export" %>

<asp:content ID="Content1" contentplaceholderid="content" runat="server">
    
        <div class="table-responsive">
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
                    <asp:TableHeaderCell Width="100">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>

                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>