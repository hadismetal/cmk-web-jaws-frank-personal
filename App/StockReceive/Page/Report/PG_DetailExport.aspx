<%@ page language="C#" masterpagefile="~/MasterPagePrint.master" autoeventwireup="true" codefile="PG_DetailExport.aspx.cs" inherits="Page_DJ" %>


<asp:content contentplaceholderid="content" runat="server">
    <div id="form_area" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="100">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Kode Barang</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Consignment</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Stock</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Exchange</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110">Product Level</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Model</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Kadar Logam</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Kadar Tukaran</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Frame Color</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Supplier</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Gold Rate</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Ongkos</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">HPPM</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Supplier SJ</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

