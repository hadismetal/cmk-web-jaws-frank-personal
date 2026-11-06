<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LD_Detail.aspx.cs" Inherits="Page_LD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Loose Diamond Stock Return
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
                        <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">GIA</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Butir</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Carat</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>

