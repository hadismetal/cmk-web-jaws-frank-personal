<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GJ_Detail.aspx.cs" Inherits="Page_GJ" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Gold Jewellery Stock Return
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <p>
            <span class="text-muted">Nomor :</span><br />
            <span class="text-lg" id="header_nomor" runat="server" /> &nbsp;
            <span class="text-lg" id="tgl" runat="server" />
            <br />
            <span class="text-muted">Keterangan :</span><br />
            <asp:Literal ID="ket" runat="server" />
        </p>
        <hr />
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
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>

