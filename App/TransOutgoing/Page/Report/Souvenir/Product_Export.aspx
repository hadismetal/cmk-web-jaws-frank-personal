<%@ Page Language="C#" MasterPageFile="~/MasterPageExport.master"
AutoEventWireup="true" CodeFile="Product_Export.aspx.cs" Inherits="DJ"
Debug="true" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <style>
    table {
      border: 1px solid black;
      text-align: left;
    }
    th {
      border: 1px solid black;
      padding: 5px;
      text-align: left;
    }
    td {
      border: 1px solid black;
      padding: 5px;
      text-align: left;
    }
  </style>
  <asp:table id="tb" runat="server">
    <asp:tableheaderrow>
      <asp:tableheadercell width="40">No.</asp:tableheadercell>
      <asp:tableheadercell width="200">No Dokumen</asp:tableheadercell>
      <asp:tableheadercell width="200">Tanggal</asp:tableheadercell>
      <asp:tableheadercell width="200">Keterangan</asp:tableheadercell>
      <asp:tableheadercell width="150">From</asp:tableheadercell>
      <asp:tableheadercell width="150">To</asp:tableheadercell>
      <asp:tableheadercell width="100">Total Item</asp:tableheadercell>
      <asp:tableheadercell width="100">Status</asp:tableheadercell>
      <asp:tableheadercell width="160">Operator Nama</asp:tableheadercell>
      <asp:tableheadercell width="160">Operator Tanggal</asp:tableheadercell>
      <asp:tableheadercell width="160">Approval Nama</asp:tableheadercell>
      <asp:tableheadercell width="160">Approval Tanggal</asp:tableheadercell>
    </asp:tableheaderrow>
  </asp:table>
</asp:Content>
