<%@ Page Language="C#" Debug="true" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sales.aspx.cs" Inherits="Page_Summary" %>

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
                    Search :
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
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
                   Location Type
                </div>
                <asp:DropDownList ID="tipelokasi" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasi" AutoPostBack="true">
                
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Location
                </div>
                <asp:DropDownList ID="lokasi" runat="server" CssClass="form-control" Width="175">
                
                </asp:DropDownList>
            </div>
            
            
            <div class="form-group">
                <div class="text-muted">
                    Item Type
                </div>
                <asp:DropDownList ID="basic" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="-1">All Item Type</asp:ListItem>
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
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Print">
                        <i class="icon-print"></i>
                        Print
                    </button>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="btnExport" runat="server" class="btn btn-primary" onserverclick="Export">
                        <i class="icon-print"></i>
                        Export
                    </button>
                </div>
            </div>
        </form>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="120" style="visibility:hidden">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" style="visibility:hidden">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" style="visibility:hidden">Jumlah</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" style="visibility:hidden">Discount</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" style="visibility:hidden">Discount Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" style="visibility:hidden">Discount Voucher</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" style="visibility:hidden">Discount Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150" style="visibility:hidden">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" style="visibility:hidden">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" style="visibility:hidden">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" style="visibility:hidden">Net Weight (gr)</asp:TableHeaderCell>

                </asp:TableHeaderRow>
            </asp:Table>
        </div>
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
        <hr />
        <div class="table-responsive">
            <asp:Table ID="tb2" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="130" style ="visibility:hidden " >Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;visibility:hidden;">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;visibility:hidden">Exchange</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;visibility:hidden">Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="1" style="text-align: center;visibility:hidden">Omset After Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;visibility:hidden">Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;visibility:hidden">Resell di Store Lain</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;visibility:hidden">Resell asal Store Lain</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="220" ColumnSpan="2" style="text-align: center;visibility:hidden">Net Omzet</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <hr />
    </div>
</asp:Content>

