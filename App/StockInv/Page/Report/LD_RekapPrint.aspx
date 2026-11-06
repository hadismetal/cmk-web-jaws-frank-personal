<%@ Page Language="C#" masterpagefile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="LD_RekapPrint.aspx.cs" Inherits="Page_Report_LD_RekapPrint" %>

<asp:content contentplaceholderid="content" runat="server">

    <div id="form_area" runat="server">
        <div>
            <h2>Loose Diamond Rekap</h2>
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
                        <td class="text-label">Shape</td>
                        <td>:</td>
                        <td id="shape" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Cutting</td>
                        <td>:</td>
                        <td id="cutting" runat="server" class="text-bold" />
                    </tr>
                </table>
            </div>
            <div style="float: left; padding: 0 0 0 50px;">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Size</td>
                        <td>:</td>
                        <td id="size" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Brand</td>
                        <td>:</td>
                        <td id="brand" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Colour</td>
                        <td>:</td>
                        <td id="colour" runat="server" class="text-bold" />
                    </tr>
                </table>
            </div>
            <div style="float: left; padding: 0 0 0 50px;">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Category</td>
                        <td>:</td>
                        <td id="category" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Clarity</td>
                        <td>:</td>
                        <td id="clarity" runat="server" class="text-bold" />
                    </tr>
                </table>
            </div>
            <div style="clear: both"></div>
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Nama</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Shape</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Colour</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Clarity</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Cutting</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Brand</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Carat</asp:TableHeaderCell>                    
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>
