<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Report_Certificate.aspx.cs" Inherits="Page_Report_Report_Certificate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="Server">
    Print Certificate
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="Server">
    Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="Server">
    <script>
        $(document).ready(function () {
            $('#<%=datepicker.ClientID%>').datepicker({
                viewMode: 'months',
                format: 'mm-yyyy'
            });
        });
    </script>
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Search (Nama Event / Keterangan) :
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
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="100">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="2">Expired</asp:ListItem>
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
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="100"></asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">No</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No Dokumen</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Nama Event</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Date Create</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Date Expired</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="80">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="50">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Operator</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:Content>

