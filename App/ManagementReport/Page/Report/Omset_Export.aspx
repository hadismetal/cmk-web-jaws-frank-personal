<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="Omset_Export.aspx.cs" Inherits="Page_Report_Omset_Export" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Jumlah</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Discount</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Discount Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Discount Voucher</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Discount Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Net Weight (gr)</asp:TableHeaderCell>

                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <hr />
        <div class="table-responsive">
            <asp:Table ID="tb2" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="130">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="2" style="text-align: right;">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="2" style="text-align: right;">Exchange</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="2" style="text-align: right;">Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="1" style="text-align: right;">Omset After Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="2" style="text-align: right;">Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="2" style="text-align: right;">Resell di Store Lain</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="2" style="text-align: right;">Resell asal Store Lain</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="2" style="text-align: right;">Net Omzet</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:Content>

