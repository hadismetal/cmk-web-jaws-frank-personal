<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="CZdetail.aspx.cs" Inherits="Page_Approval_CZdetail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" Runat="Server">
    Cubic Zirconia
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" Runat="Server">
    Approval Detail ( <asp:Label ID="Label2" runat="server"></asp:Label>)
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" Runat="Server">
    
     <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="100">No. </asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="300">Image</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Kode Barang</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Ring Size</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Berat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Model</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Frame color</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Kadar Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Kadar Beli</asp:TableHeaderCell>
                  </asp:TableHeaderRow>
            </asp:Table>
        </div>
</asp:Content>

