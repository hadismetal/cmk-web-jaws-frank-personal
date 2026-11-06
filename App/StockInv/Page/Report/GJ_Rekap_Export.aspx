<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="GJ_Rekap_Export.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <div id="form_area" runat="server">
        <div>
            <h2>Gold Jewellery Rekap</h2>
        </div>
        <div>
            <div style="float: left;">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Report DateTime </td>
                        <td>:</td>
                        <td id="report_datetime" runat="server" class="text-bold"></td>
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
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableRow>
                    <asp:TableHeaderCell Width="140">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Stock</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">M</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">R</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>

            
        </div>

    </div>
</asp:Content>

