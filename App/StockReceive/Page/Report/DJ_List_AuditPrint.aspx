<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="DJ_List_AuditPrint.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div style="max-width: 1170px">
        <div id="form_area" runat="server">
            <div>
                <h2>Stock Receiving List</h2>
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
                        <tr>
                            <td class="text-label">Brand</td>
                            <td>:</td>
                            <td id="brand" runat="server" class="text-bold" />
                        </tr>
                        <tr>
                            <td class="text-label">Periode</td>
                            <td>:</td>
                            <td id="periode" runat="server" class="text-bold" />
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
                        <tr>
                            <td class="text-label">Supplier</td>
                            <td>:</td>
                            <td id="supplier" runat="server" class="text-bold" />
                        </tr>
                    </table>
                </div>
                <div style="clear: both"></div>
            </div>
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm" Font-Size="7px">
                    
                </asp:Table>
            </div>

        </div>
    </div>
</asp:Content>

