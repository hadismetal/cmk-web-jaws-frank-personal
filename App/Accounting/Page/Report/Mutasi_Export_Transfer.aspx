<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="Mutasi_Export_Transfer.aspx.cs" Inherits="Page_Report_Mutasi_Export_Sales" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <asp:Table ID="Table1" runat="server" CssClass="table-list text-sm">
            
        </asp:Table>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableRow>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Group</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Nomor Outgoing</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Brand Asal</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Tipe Lokasi Asal</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Lokasi Asal</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Nomor Incoming</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Brand Tujuan</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Tipe Lokasi Tujuan</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Lokasi Tujuan</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Netto</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Gross</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Harga Jual</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center" BorderWidth="1" BorderColor="Gray">Total M</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>