<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StockAuditLD_Detail.aspx.cs" Inherits="Page_LD" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Loose Diamond Stock Audit Detail
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            
            <span class="text-lg" id="title" runat="server"></span><br />
            <span class="text-lg" id="title2" runat="server"></span><br />
            <span id="title3" runat="server"></span><br />
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="150">Data Fisik</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Data Program</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Fisik Ada, Data Tidak Ada</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Fisik Tidak Ada, Data Ada</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Gambar Tidak Sesuai</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="180">Bandrol Tidak Sesuai</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Multiple Input</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>

