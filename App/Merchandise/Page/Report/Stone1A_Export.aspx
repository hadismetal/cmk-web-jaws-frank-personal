<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="Stone1A_Export.aspx.cs" Inherits="Page_Account_Stone" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div class="table-responsive">
        <asp:Table ID="tb" runat="server" CssClass="table-list">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">PLU</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Tipe Lokasi</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="200">Lokasi</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Nama</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Shape</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Colour</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120">Clarity</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="90">Butir</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="90">Carat</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="90">Harga Per Karat USD</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="90">Total Harga USD</asp:TableHeaderCell>

            </asp:TableHeaderRow>
        </asp:Table>
    </div>
    <asp:Literal ID="paging_text" runat="server" />
</asp:Content>

