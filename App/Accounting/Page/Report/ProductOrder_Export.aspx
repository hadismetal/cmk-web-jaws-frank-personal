<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="ProductOrder_Export.aspx.cs" Inherits="Page_Report_ProductOrder_Export" %>

<asp:content contentplaceholderid="content" runat="server">
    
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="120" />
                    <asp:TableHeaderCell Width="140">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Product</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="75" />
                    <asp:TableHeaderCell Width="75" />
                    <asp:TableHeaderCell Width="75" />
                    <asp:TableHeaderCell Width="75" />
                    <asp:TableHeaderCell Width="150">Emas</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Stone</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

</asp:content>

