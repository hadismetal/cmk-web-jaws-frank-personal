<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="Mutasi_Export_Gift.aspx.cs" Inherits="Page_Report_Mutasi_Export_Sales" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <asp:Table ID="Table1" runat="server" CssClass="table-list text-sm">
            
        </asp:Table>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableRow>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Brand</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Store</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Nomor</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Invoice</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga M 1</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga M 2</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Rupiah 1</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Rupiah 2</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Stone</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Emas</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Ongkos</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>