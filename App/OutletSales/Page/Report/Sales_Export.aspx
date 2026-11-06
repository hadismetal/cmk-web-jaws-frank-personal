<%@ Page Language="C#" Debug="true" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="Sales_Export.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="120">Keterangan</asp:TableHeaderCell>
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
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;">Exchange</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;">Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="1" style="text-align: center;">Omset After Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;">Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;">Resell di Store Lain</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;">Resell asal Store Lain</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;">Net Omzet</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:Content>

