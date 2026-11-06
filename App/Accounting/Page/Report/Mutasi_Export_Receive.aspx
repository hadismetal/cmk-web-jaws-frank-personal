<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master"
AutoEventWireup="true" CodeFile="Mutasi_Export_Receive.aspx.cs"
Inherits="Page_Report_Mutasi_Export_Receive" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <div id="form_area" runat="server">
    <asp:Table ID="Table1" runat="server" CssClass="table-list text-sm">
    </asp:Table>
    <div class="table-responsive">
      <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
        <asp:TableRow>
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Brand</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Store</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Group</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Tanggal</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Group Supplier</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Supplier</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >PLU</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Product Item</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Product Category</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Stone Dist</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Net Weight</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Gross Weight</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Total Butir</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Total Carat</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Harga Jual</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Harga M Emas</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Harga M Stone</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Harga M Ongkos</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Harga M</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Harga M2</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Rate</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Harga M Rupiah</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >PLU Lama</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Kode Karet</asp:TableHeaderCell
          >
          <asp:TableHeaderCell
            style="text-align: center"
            BorderWidth="1"
            BorderColor="Gray"
            >Kadar Logam</asp:TableHeaderCell
          >
        </asp:TableRow>
      </asp:Table>
    </div>
  </div>
</asp:Content>
