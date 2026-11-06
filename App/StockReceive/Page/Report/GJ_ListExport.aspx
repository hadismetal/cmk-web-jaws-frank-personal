<%@ page language="C#" masterpagefile="~/MasterPageExport.master" autoeventwireup="true" codefile="GJ_ListExport.aspx.cs" inherits="Page_GJ" %>

<asp:content contentplaceholderid="content" runat="server">
    <div id="form_area" runat="server">
        
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Segmen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Stone Distribution</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Nett Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Frame Price M</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Frame Price R</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">NO M</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">NO M Lot</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">NO R</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">RO M</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">RO R</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Finishing M</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Finishing R</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Ongkos Lain M</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Ongkos Lain R</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Stone Setting M</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Stone Setting R</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Total M (USD)</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Total R (USD)</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="120">Mark Up</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Harga Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">No validasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Supplier</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:content>

