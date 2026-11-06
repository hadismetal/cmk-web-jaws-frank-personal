<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_Detail.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery Stock Return
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <p>
            <span class="text-muted">Nomor :</span><br />
            <span class="text-lg" id="header_nomor" runat="server" /> &nbsp;
            <span class="text-lg" id="tgl" runat="server" />
            <br />
            <span class="text-muted">Keterangan :</span><br />
            <asp:Literal ID="ket" runat="server" />
        </p>
        <hr />
        <form id="Form1" runat="server" class="form-inline">

            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tanggal Input</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tanggal Approve</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Frame Price</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Parcel</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">T. Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">T. Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">NO</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">RO</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="100">Finishing</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Stone Setting</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Harga M <span class="text-muted">USD</span></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Harga Jual <span class="text-muted">Rp</span></asp:TableHeaderCell>
                </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>

