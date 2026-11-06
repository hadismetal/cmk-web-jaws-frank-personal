<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Product_Rekap.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Souvenir Stock Summary
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Cabang :
                </div>
                <asp:DropDownList ID="cabang" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Cabang :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Search :
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Brand :
                </div>
                <asp:DropDownList ID="brand" runat="server" CssClass="form-control" Width="170">
                    <asp:ListItem Value="0">All</asp:ListItem>
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
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="excel" runat="server" class="btn btn-primary" onserverclick="excel_ServerClick">
                        <i class="icon-download-alt"></i>
                        Excel
                    </button>
                </div>
            </div>
        </form>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Nama</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Kode</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Satuan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Stock</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Dalam Pengiriman</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Rusak</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Hilang</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

