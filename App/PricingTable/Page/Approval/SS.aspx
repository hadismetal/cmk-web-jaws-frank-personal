<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SS.aspx.cs" Inherits="Page_Approval_SS" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Segmen System
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Approval
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="zero" runat="server" visible="false">
        <i class="icon-ok icon-5x pull-left text-success"></i>
        <h4>Proses Selesai</h4>
        <p class="text-muted">
            Semua data / transaksi dalam kondisi sudah approval.
        </p>
        <div class="clearfix"></div>
        <hr />
    </div>
    <div id="form_area" runat="server">
        <form runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Product Category :
                </div>
                <asp:DropDownList ID="category" runat="server" CssClass="form-control" Width="200">
                    <asp:ListItem>Semua</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Product Level :
                </div>
                <asp:DropDownList ID="level" runat="server" CssClass="form-control" Width="200">
                    <asp:ListItem>Semua</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Stone Distribution :
                </div>
                <asp:DropDownList ID="dist" runat="server" CssClass="form-control" Width="200">
                    <asp:ListItem>Semua</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
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
                <asp:Table ID="tb" runat="server" CssClass="table-list">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="100" />
                        <asp:TableHeaderCell Width="250">Product Segmen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="175">Operator</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130" CssClass="text-right">Minimum</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130" CssClass="text-right">Pending</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150" CssClass="text-right">Mutasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100" CssClass="text-right">Sistem</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
            <div class="button-left">
                <asp:DropDownList ID="process_id" runat="server" Visible="false" />
                <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                    <i class="icon-fixed-width icon-check"></i>
                    Approved All
                </button>
            </div>
        </form>
    </div>
</asp:Content>

