<%@ Page Language="C#" masterpagefile="~/MasterPageExport.master" CodeFile="DesignerPerformance_Export.aspx.cs" Inherits="Page_Report_DP_DesignerPerformance_Export" %>

<asp:content contentplaceholderid="content" runat="server">
    <div id="form_area" runat="server">
        
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list table-striped text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Product Level</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Stone Distribution</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Design Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Design Concept</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Naming</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Designer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">QTY Product</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="100">QTY Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">FOTO</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>
