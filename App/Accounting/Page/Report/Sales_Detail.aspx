<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sales_Detail.aspx.cs" Inherits="Page_Register_Sales_Detail" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Sales Order Detail
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    
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
                <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="130">No. Invoice</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="130">PLU</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Product Item</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Harga</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Discount</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Total Bayar</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="120"></asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
    <hr />
    <div class="table-responsive">
        <asp:Table ID="Table1" runat="server" CssClass="table-list">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="180">Nilai Sales Order</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="170">Discount</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="180">Total Bayar</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="170">Terbayar</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="170">Kekurangan</asp:TableHeaderCell>
            </asp:TableHeaderRow>
            <asp:TableRow>
                <asp:TableCell ID="nilaiso" runat="server"></asp:TableCell>
                <asp:TableCell ID="discount" runat="server"></asp:TableCell>
                <asp:TableCell ID="totalbayar" runat="server"></asp:TableCell>
                <asp:TableCell ID="terbayar" runat="server"></asp:TableCell>
                <asp:TableCell ID="kekurangan" runat="server"></asp:TableCell>
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

