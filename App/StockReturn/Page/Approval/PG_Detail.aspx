<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PG_Detail.aspx.cs" Inherits="Page_PG" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Plain Gold Stock Return
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">

            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="120">No. PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">Kode Karet</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">Consignment</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">Product Level</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Kadar</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Ongkos</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga Jual</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga M</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Weight</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>

            <div class="form-group">
                <div>
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Approve">
                        <i class="icon-search"></i>
                        Approve
                    </button>
                </div>
            </div>
        </form>
    </div>
</asp:Content>

