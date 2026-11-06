<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="DJ_List_Export.aspx.cs" Inherits="DJ" Debug="true" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div>
        <h1>Report Rekap Incoming Transfer</h1>
    </div>
    <div>
        <div style="float: left;">
            <table class="table-file">
                <tr>
                    <td class="text-label">Report DateTime</td>
                    <td>:</td>
                    <td id="report_datetime" runat="server" class="text-bold" />
                </tr>
                <tr>
                    <td class="text-label">Periode</td>
                    <td>:</td>
                    <td id="periode" runat="server" class="text-bold" />
                </tr>
                <tr>
                    <td class="text-label">Company Brand</td>
                    <td>:</td>
                    <td id="brand" runat="server" class="text-bold" />
                </tr>
                <tr>
                    <td class="text-label" style="vertical-align: top;">From
                        <asp:Literal runat="server" ID="lit_type_from" /></td>
                    <td style="vertical-align: top;">:</td>
                    <td id="from" runat="server" class="text-bold" />
                </tr>
                <tr>
                    <td class="text-label" style="vertical-align: top;">To
                        <asp:Literal runat="server" ID="lit_type_to" /></td>
                    <td style="vertical-align: top;">:</td>
                    <td id="to" runat="server" class="text-bold" />
                </tr>
            </table>
        </div>
        <div style="float: left; padding: 0 0 0 50px;">
            <table class="table-file">
                <tr>
                    <td class="text-label">Status
                        <asp:Literal runat="server" ID="lit_status" /></td>
                    <td>:</td>
                    <td id="status" runat="server" class="text-bold" />
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
                <tr>
                    <td class="text-label">Search Product</td>
                    <td>:</td>
                    <td id="search" runat="server" class="text-bold" />
                </tr>
            </table>
        </div>
        <div style="clear: both"></div>
    </div>
    <div class="table-responsive">
        <asp:Table ID="tb" runat="server" CssClass="table-list" Font-Size="11px">
            
        </asp:Table>
    </div>
</asp:Content>

