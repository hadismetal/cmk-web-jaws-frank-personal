<%@ Page Language="C#" masterpagefile="~/MasterPagePrint.master" CodeFile="DesignerPerformance_Print.aspx.cs" Inherits="Page_Report_DP_DesignerPerformance_Print" %>

<asp:content ID="Content1" contentplaceholderid="content" runat="server">
    <div id="form_area" runat="server">
         <div>
            <h2>Design Report</h2>
        </div>
        <div>
            <div style="float: left;">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Report DateTime </td>
                        <td>:</td>
                        <td id="report_datetime" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Cabang</td>
                        <td>:</td>
                        <td id="cabang" runat="server" class="text-bold" />
                    </tr>
                </table>
            </div>
            <div style="float: left; padding: 0 0 0 50px;">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Product Level</td>
                        <td>:</td>
                        <td id="level" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Product Category</td>
                        <td>:</td>
                        <td id="category" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Product Item</td>
                        <td>:</td>
                        <td id="item" runat="server" class="text-bold" />
                    </tr>

                </table>
            </div>
            <div style="clear: both"></div>
        </div>
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
