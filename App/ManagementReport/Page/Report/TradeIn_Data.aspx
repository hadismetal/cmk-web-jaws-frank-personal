<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TradeIn_Data.aspx.cs" Inherits="Page_Register_Sales_Item" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Trade In
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <p>
        <span class="text-muted">Nomor :</span><br />
        <span class="text-lg" id="header_nomor" runat="server" />
    </p>
    <hr />
    <form id="Form1" runat="server">
        
        <div class="table-responsive">
            <h3>Sales Item</h3>
            <asp:Table ID="tb2" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="120"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Discount <span class="text-muted">%</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Discount Program <span class="text-muted">%</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Gift Voucher <span class="text-muted">IDR</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Discount Nominal <span class="text-muted">IDR</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Total Bayar</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <div class="table-responsive">
            <h3>Item Traded In</h3>
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="100"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Harga Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Harga Acuan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Harga Beli</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Selisih</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Usia</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </form>
</asp:Content>

