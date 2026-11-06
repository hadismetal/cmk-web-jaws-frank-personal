<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Titipan_Update.aspx.cs" Inherits="Page_Register_Sales_Detail" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Titipan Update
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server">
        <p>
            <span class="text-muted">Nomor :</span><br />
            <span class="text-lg" id="header_nomor" runat="server" />
        </p>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            transaction form
        </div>
        <div class="row">
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Status Titipan</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="statustitipan" runat="server" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Market Brand</td>
                        <td>:</td>
                        <td id="brand" runat="server" />
                    </tr>
                </table>
            </div>
            <br class="visible-sm visible-xs" />
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Customer</td>
                        <td>:</td>
                        <td id="customer" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Keterangan</td>
                        <td>:</td>
                        <td id="keterangan" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Tanggal</td>
                        <td>:</td>
                        <td id="tgl" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Tanggal Pengembalian</td>
                        <td>:</td>
                        <td id="tglBalikin" runat="server" />
                    </tr>
                </table>
            </div>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            transaction data
        </div>

        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">No. Titipan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Item</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

        <div class="button-left">
            <button id="titipan" runat="server" class="btn btn-success" onserverclick="Update_Click">
                <i class="icon-print"></i>
                Update Status
            </button>
        </div>
    </form>
</asp:Content>

