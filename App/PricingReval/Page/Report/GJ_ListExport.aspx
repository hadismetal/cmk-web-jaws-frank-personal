<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="GJ_ListExport.aspx.cs" Inherits="Page_Report_GJ_ListExport" %>


<asp:Content ID="Content3" ContentPlaceHolderID="content" Runat="Server">
    <asp:Table ID="tb" runat="server" >
                    <asp:TableHeaderRow>
                         <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="90">Tanggal</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Harga Sebelum</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Harga Sesudah</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">Mutasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">M Sebelum</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">M Sesudah</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">R Sebelum</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">R Sesusah</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Net Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Gross Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Segmen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Stone Distribution</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
</asp:Content>

