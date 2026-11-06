<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Voucher.aspx.cs" Inherits="Page_Approval_Product" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Voucher
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Report
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Keyword
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control text-center" Width="110" placeholder="Keyword" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="dari" runat="server" CssClass="form-control text-center" Width="110" placeholder="Dari" />
                <script>
                    $(document).ready(function () { $('#<%=dari.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>

            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="sampai" runat="server" CssClass="form-control text-center" Width="110" placeholder="Sampai" />
                <script>
                    $(document).ready(function () { $('#<%=sampai.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>

            </div>
            <div class="form-group">
                <div class="text-muted">
                    Status
                </div>
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="-1">All</asp:ListItem>
                    <asp:ListItem Value="0">Active</asp:ListItem>
                    <asp:ListItem Value="1">Used</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Jenis
                </div>
                <asp:DropDownList ID="ddlJenis" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Gift Certificate</asp:ListItem>
                    <asp:ListItem Value="2">Discount Card</asp:ListItem>
                    <asp:ListItem Value="3">Birthday Discount</asp:ListItem>
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
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="170">Brand</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="170">Nomor Voucher</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="170">Jenis</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Tanggal dikeluarkan</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Tanggal Expired</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Customer</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Nominal</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>

