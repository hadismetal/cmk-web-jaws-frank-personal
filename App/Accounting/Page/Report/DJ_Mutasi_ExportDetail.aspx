<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="DJ_Mutasi_ExportDetail.aspx.cs" Inherits="Page_Report_DJ_Mutasi_Export" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <asp:Table ID="Table1" runat="server" CssClass="table-list text-sm">
            
        </asp:Table>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableRow>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Brand</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Groups</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Tipe Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">PLU</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Frame Material</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Level</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Stone Dist</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Netto Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross Weight(24K)</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Netto Weight(24K)</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Kadar Logam</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Kadar Tukar Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Kadar Tukar Beli</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Parcel</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">EmasM</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">StoneM</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">OngkosM</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga Rupiah</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Supplier</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>