<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_Detail.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery Stock Return
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">

            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="120">No. PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">Kode Karet</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga Jual</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga M</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Net Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Gross Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Butir</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Carat</asp:TableHeaderCell>
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

