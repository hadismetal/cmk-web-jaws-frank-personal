<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="HPPDetail_Print.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="130">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. Invoice</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Product Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Nama Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180">H. Jual Net</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="700" HorizontalAlign="Center" ColumnSpan="10">HPP</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180" ColumnSpan="2">T. HPP</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>


        <div class="table-responsive">
            <h2>Rekap Parcel</h2>
            <asp:Table ID="tb2" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="130">Parcel</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">M Stone</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:Content>

