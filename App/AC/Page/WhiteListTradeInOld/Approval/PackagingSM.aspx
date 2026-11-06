<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PackagingSM.aspx.cs" Inherits="Page_Approval_PackagingSM" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Packaging Approval by Store Manager
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <script>
        $(document).ready(function () {
            $('#<%=datepicker.ClientID%>').datepicker({
                viewMode: 'months',
                format: 'mm-yyyy'
            });
        });
    </script>
    <form runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Search (PLU / Keterangan) :
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="250" placeholder="..." />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Periode :
                </div>
                <asp:TextBox CssClass="form-control text-center" Width="150" type="text" placeholder="Month" ID="datepicker" runat="server" data-date-format="MM-yyyy" />

            </div>
            <div class="form-group">
                <div class="text-muted">
                    Status :
                </div>
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="150">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Approved</asp:ListItem>
                    <asp:ListItem Value="2">Pending</asp:ListItem>
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
        <asp:Table ID="tb" runat="server" CssClass="table-list text-sm" Style="width: 100%;">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="100"></asp:TableHeaderCell>
                <asp:TableHeaderCell Width="50">No</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="190">Nomor Packaging</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="190">Link Packaging</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="190">Approval Store Manager</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="190">Approval Accounting</asp:TableHeaderCell>
				<asp:TableHeaderCell Width="190">Approval LSP</asp:TableHeaderCell>
				<asp:TableHeaderCell Width="100">Status</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="200">Keterangan</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
</asp:Content>
