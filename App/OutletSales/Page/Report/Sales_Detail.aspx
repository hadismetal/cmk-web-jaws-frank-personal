<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sales_Detail.aspx.cs" Inherits="Page_Register_Sales_Detail" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Sales Order Detail
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div>
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
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">No. Invoice</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Discount %</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Discount Program %</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Discount Gift</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Discount Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Total Bayar</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

        <div class="file-subheader hidden-sm hidden-xs">
            Total
        </div>

        <div class="table-responsive">

            <asp:Table ID="tb3" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Tipe Pembayaran</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Bank</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">EDC</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180">Nama Pemegang Kartu</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. Kartu</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Program</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
    <hr />
    <div class="table-responsive">
        <asp:Table ID="tb2" runat="server" CssClass="table-list">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="160">Nilai Sales Order</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">Nilai Resell / Trade In</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="140">Discount</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Total Harus DiBayar</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Terbayar</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Kekurangan</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Pembayaran</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>

                <asp:TableCell ID="nilaiso" runat="server"></asp:TableCell>
                <asp:TableCell ID="resell" runat="server"></asp:TableCell>
                <asp:TableCell ID="discount" runat="server"></asp:TableCell>
                <asp:TableCell ID="totalbayar" runat="server"></asp:TableCell>
                <asp:TableCell ID="terbayar" runat="server"></asp:TableCell>
                <asp:TableCell ID="kekurangan" runat="server"></asp:TableCell>
                <asp:TableCell ID="pembayaran" runat="server"></asp:TableCell>
            </asp:TableRow>
        </asp:Table>
    </div>
    <form id="Form1" runat="server">
        <div class="button-left">
            <button id="invoice" runat="server" class="btn btn-success" onserverclick="PrintSO_Click">
                <i class="icon-print"></i>
                Print Sales Order
            </button>
            <button id="booking" runat="server" class="btn btn-success" onserverclick="PrintBooking_Click">
                <i class="icon-print"></i>
                Print DP
            </button>
        </div>
    </form>
</asp:Content>

