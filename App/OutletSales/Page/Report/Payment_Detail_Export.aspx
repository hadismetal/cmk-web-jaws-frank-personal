<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="Payment_Detail_Export.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="130">Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180">Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">EDC</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Bank</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Jenis Kartu</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">CC Number</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">CC Name</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">CC Type</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">MDR</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

