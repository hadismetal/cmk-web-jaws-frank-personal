<%@ page language="C#" masterpagefile="~/MasterPageExport.master" autoeventwireup="true" codefile="DJ_DetailExport.aspx.cs" inherits="Page_DJ" %>

<asp:content contentplaceholderid="content" runat="server">
    <div id="form_area" runat="server">
        
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list table-striped text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tanggal Approve</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Approve</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Supplier Nomor</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Parcel</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">T. Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">T. Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Harga Jual <span class="text-muted">Rp</span></asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

