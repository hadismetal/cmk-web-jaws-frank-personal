<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sales.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Sales Order Summary
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="zero" runat="server" visible="false">
        <i class="icon-ok icon-5x pull-left text-success"></i>
        <h4>Proses Selesai</h4>
        <p class="text-muted">
            Tidak ada transaksi penjualan
        </p>
        <div class="clearfix"></div>
        <hr />
    </div>
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Lokasi
                </div>
                <asp:DropDownList ID="lokasi" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Lokasi</asp:ListItem>
                </asp:DropDownList>
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
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            
            
            <div class="form-group">
                <div class="text-muted">
                    Item Type
                </div>
                <asp:DropDownList ID="basic" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Basic Item</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Group By
                </div>
                <asp:DropDownList ID="groupby" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product</asp:ListItem>
                    <asp:ListItem Value="1">Product Item</asp:ListItem>
                    <asp:ListItem Value="2">Sales</asp:ListItem>
                    <asp:ListItem Value="3">Customer</asp:ListItem>
                    <asp:ListItem Value="4">Product Category</asp:ListItem>
                    <asp:ListItem Value="5">Product Level</asp:ListItem>
                    <asp:ListItem Value="6">Segmen</asp:ListItem>
                    <asp:ListItem Value="7">Outlet</asp:ListItem>
                </asp:DropDownList>
            </div>
			 <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="tipeproduct" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="-1">Tipe Product</asp:ListItem>
                    <asp:ListItem Value="1">Diamond Jewellery</asp:ListItem>
                    <asp:ListItem Value="2">Gold Jewellery</asp:ListItem>
                    <asp:ListItem Value="3">Plain Gold</asp:ListItem>
                    <asp:ListItem Value="4">Loose Diamond</asp:ListItem>
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
            <asp:Table ID="tb3" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="50" RowSpan="2">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" RowSpan="2">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;">Exchange</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;">Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;">Omset After Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;">Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;">Resell di Store Lain</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;">Resell asal Store Lain</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;">Net Omzet</asp:TableHeaderCell>
                </asp:TableHeaderRow>
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell>Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Value</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Value</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Value</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Value</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Value</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Value</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Value</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell>Value</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:Content>

