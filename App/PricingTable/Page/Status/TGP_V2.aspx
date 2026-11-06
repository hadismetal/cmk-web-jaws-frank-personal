<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TGP_V2.aspx.cs" Inherits="Page_Status_TG_V2" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Today Gold Price
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Status
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
        <div class="alert alert-info" style="margin-top: 25px;">
    <ul style="padding-left:10px;font-size:larger">
        <li><b>Today Gold Price</b> digunakan untuk menghitung harga jual dan beli <b>Plain Gold / Gold Jewellery</b> harian.</li>
        <li><b>Today Gold Price</b> hanya bisa diupdate dan diapprove di <b>Myapps</b>.</li>
    </ul>
</div>
    <form runat="server" class="form-inline">
        <div class="form-group">
            <div class="text-muted">
                Status :
            </div>
            <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem>Semua</asp:ListItem>
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
    <div class="table-responsive">
        <asp:Table ID="tb" runat="server" CssClass="table-list">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="90" />
                <asp:TableHeaderCell Width="250">Nama</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="175">Approval</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150" CssClass="text-right"><small>IDR</small> TGP Jual Jaws</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150" CssClass="text-right"><small>IDR</small> TGP Beli Jaws</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150" CssClass="text-right"><small>IDR</small> TGP Jual Myapps</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150" CssClass="text-right"><small>IDR</small> TGP Beli Myapps</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="200" CssClass="text-left">Status</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
</asp:Content>
