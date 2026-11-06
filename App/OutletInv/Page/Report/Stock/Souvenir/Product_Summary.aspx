<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Product_Summary.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Souvenir Report
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Location Type:
                </div>
                <asp:DropDownList ID="locationType" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="0">Warehouse</asp:ListItem>
                    <asp:ListItem Value="1">Outlet</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Location
                </div>
                <asp:DropDownList ID="location" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">All Location</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Product
                </div>
                <asp:DropDownList ID="product" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">All Product</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="dari" runat="server" CssClass="form-control text-center" Width="110" placeholder="Dari" AutoPostBack="true" />
                <script>
                    $(document).ready(function () {
                        $('#<%=dari.ClientID%>').datepicker({
                            dateFormat: 'dd-mm-yy',
                            onSelect: function (date) {
                                $('#<%=sampai.ClientID%>').datepicker('option', { dateFormat: 'dd-mm-yy', minDate: date });
                            }
                        });
                    });
                </script>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="sampai" runat="server" CssClass="form-control text-center" Width="110" placeholder="Sampai" />
                <script>
                    $(document).ready(function () { $('#<%=sampai.ClientID%>').datepicker({ dateFormat: 'dd-mm-yy' }); });
                </script>
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
                    <button id="excel" runat="server" class="btn btn-primary" onserverclick="excel_ServerClick">
                        Print
                    </button>
                </div>
            </div>
        </form>
        <div class="table-responsive">
            
            <asp:Table ID="tb" runat="server" CssClass="table-list" Visible="false">
                <asp:TableRow>
                    <asp:TableHeaderCell runat="server" Width="250" RowSpan="2">Packaging</asp:TableHeaderCell>
                    <asp:TableHeaderCell runat="server" Width="150" RowSpan="2">Kode</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" RowSpan="2">Stock Awal</asp:TableHeaderCell>
                    <asp:TableHeaderCell ColumnSpan="2" Width="200">Penerimaan</asp:TableHeaderCell>
                    <asp:TableHeaderCell ColumnSpan="2" Width="200">Pengeluaran</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" RowSpan="2">Ongoing</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" RowSpan="2">Real Stock</asp:TableHeaderCell>
                </asp:TableRow>
                <asp:TableRow>
                    <asp:TableHeaderCell Width="100">Receiving</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Incoming</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Outgoing</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Sales</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

