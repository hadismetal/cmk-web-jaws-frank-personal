<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="StockAuditGJ_Detail.aspx.cs" Inherits="Page_GJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Gold Jewellery Stock Audit Detail
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <label class="control-label">Group By</label>
                <br />
                <asp:DropDownList class="form-control" runat="server" ID="group">
                    <asp:ListItem Value="1">Product Item</asp:ListItem>
                    <asp:ListItem Value="2">Product Category</asp:ListItem>
                    <asp:ListItem Value="3">Product Level</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                        <i class="icon-search"></i>
                        Display
                    </button>
                </div>
            </div>
        </form>

        <hr />
        <span class="text-lg" id="title" runat="server"></span>
        <br />
        <span class="text-lg" id="title2" runat="server"></span>
        <br />
        <span id="title3" runat="server"></span>
        <br />
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="150">Data Fisik</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Data Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="170">Fisik Ada, Data Tidak Ada</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="170">Fisik Tidak Ada, Data Ada</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Gambar Tidak Ada</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Gambar Tidak Sesuai</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Bandrol Tidak Sesuai</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Multiple Input</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

        <span class="text-lg"><b>Detail</b></span>
        <div class="table-responsive">
            <asp:Table ID="tb2" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="150">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Data Fisik</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Data Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Fisik Ada, Data Tidak Ada</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Fisik Tidak Ada, Data Ada</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Gambar Tidak Ada</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Gambar Tidak Sesuai</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Bandrol Tidak Sesuai</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:Content>

