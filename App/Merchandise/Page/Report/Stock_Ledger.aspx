<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Stock_Ledger.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Stock Ledger
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Tipe :
                </div>
                <asp:DropDownList ID="tipe" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Diamond Jewellery</asp:ListItem>
                    <asp:ListItem Value="3">Plain Gold</asp:ListItem>
                    <asp:ListItem Value="1">Gold Jewellery</asp:ListItem>
                    <asp:ListItem Value="2">Loose Diamond</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    PLU :
                </div>
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="..." style="text-align:center;" />
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
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="190">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tipe Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="270">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Value</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160">Remarks</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160">Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

