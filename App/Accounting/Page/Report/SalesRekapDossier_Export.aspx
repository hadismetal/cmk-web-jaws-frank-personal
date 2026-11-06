<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="SalesRekapDossier_Export.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <div id="form_area" runat="server">
        
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="100">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. Sales Order</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Nama Customer</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="130">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Brand</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Colour</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Clarity</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Carat Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Certificate</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Harga Barcode</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Harga Net</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">M Batu</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">R Batu</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">M Batu Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">R Batu Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">M Total</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">R Total</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Rapaport</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Disc M Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Disc R Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">%</asp:TableHeaderCell>
					<asp:TableHeaderCell Width="130">Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

