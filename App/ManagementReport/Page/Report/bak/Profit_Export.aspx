<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="Profit_Export.aspx.cs" Inherits="Page_Report_Profit_Export" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="100">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">HPP</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">GP</asp:TableHeaderCell>                    
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
</asp:Content>

