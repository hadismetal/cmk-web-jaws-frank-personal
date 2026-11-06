<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="WhitelistTradeIn.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Whitelist TradeIn
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Request via :
                </div>
                <asp:DropDownList ID="ddl_requestvia" runat="server" CssClass="form-control" Width="120">
                    <asp:ListItem Value="email">Email</asp:ListItem>
                    <asp:ListItem Value="whatsapp">Whatsapp</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Tipe Produk :
                </div>
                <asp:DropDownList ID="ddl_tipeproduk" runat="server" CssClass="form-control" Width="200">
                    <asp:ListItem Value="dj">Diamond Jewellery</asp:ListItem>
                    <asp:ListItem Value="pg">Plain Gold</asp:ListItem>
                    <asp:ListItem Value="gj">Gold Jewellery</asp:ListItem>
                    <asp:ListItem Value="ld">Loose Diamond</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    PLU :
                </div>
                <asp:TextBox ID="txt_plu" runat="server" CssClass="form-control text-center" Width="150" placeholder="..." />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Keterangan :
                </div>
                <asp:TextBox ID="txt_keterangan" runat="server" CssClass="form-control" Width="500" placeholder="..." />
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                        <i class="icon-plus"></i>
                        Submit
                    </button>
                </div>
            </div>
        </form>
        <div style="padding-top:30px;">
            Showing only last of 3 months transaction.
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm" style="width:100%;">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="100"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">No</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="190">Operator</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="190">Approval</asp:TableHeaderCell>
                    <asp:TableHeaderCell >Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:Content>

