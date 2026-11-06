<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="Resell_Export.aspx.cs" Inherits="Page_Report_Resell_Export" %>
<asp:Content ContentPlaceHolderID="content" ID="content" runat="server">

    <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="20"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Nama Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
</asp:Content>

