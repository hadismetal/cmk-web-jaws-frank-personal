<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="DJ_List_V2_Export.aspx.cs" Inherits="Page_DJ_List_V2_Export" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
        td
        {
            vertical-align: top;
        }
    </style>
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow TableSection="TableHeader">
                        <asp:TableHeaderCell Width="70">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga Sebelum</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga Sesudah</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Mutasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">M Awal</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">M</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">M Reval</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">R</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">R Reval</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Net Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Gross Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Butir</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Carat</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Segmen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Stone Distribution</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Product Level</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>

