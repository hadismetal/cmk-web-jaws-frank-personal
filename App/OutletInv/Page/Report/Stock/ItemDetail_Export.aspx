<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master"
AutoEventWireup="true" CodeFile="ItemDetail_Export.aspx.cs" Inherits="Page_DJ"
%>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <div id="form_area" runat="server">
    <div class="table-responsive">
      <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
        <asp:TableHeaderRow>
          <asp:TableHeaderCell Width="30">No.</asp:TableHeaderCell>
          <asp:TableHeaderCell Width="150">PLU</asp:TableHeaderCell>
          <asp:TableHeaderCell Width="150">Kode Karet</asp:TableHeaderCell>
          <asp:TableHeaderCell Width="150">Product Item</asp:TableHeaderCell>
          <asp:TableHeaderCell Width="150">Tipe</asp:TableHeaderCell>
          <asp:TableHeaderCell Width="150">Substorage</asp:TableHeaderCell>
          <asp:TableHeaderCell Width="80">Qty</asp:TableHeaderCell>
          <asp:TableHeaderCell Width="120">H. Jual</asp:TableHeaderCell>
          <asp:TableHeaderCell Width="100">Butir</asp:TableHeaderCell>
          <asp:TableHeaderCell Width="100">Carat</asp:TableHeaderCell>
        </asp:TableHeaderRow>
      </asp:Table>
    </div>
    <asp:Literal ID="paging_text" runat="server" />
  </div>
</asp:Content>
