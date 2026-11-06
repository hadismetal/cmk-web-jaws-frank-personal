<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Resell_Detail.aspx.cs" Inherits="Page_Register_Resell_Detail" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Resell
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Detail
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">

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
                <asp:TableHeaderCell Width="100"></asp:TableHeaderCell>
                <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">PLU</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Product Item</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">Harga Jual</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">Harga Acuan</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">Harga Beli</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="90">Selisih</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="90">Usia</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
</asp:Content>

