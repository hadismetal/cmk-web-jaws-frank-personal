<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Omset.aspx.cs" Inherits="Page_Summary" %>

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
                    Tipe Lokasi
                </div>
                <asp:DropDownList ID="tipelokasi" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasi" AutoPostBack="true">
                    <asp:ListItem Value="0">Tipe Lokasi</asp:ListItem>
                    <asp:ListItem Value="1">Outlet</asp:ListItem>
                    <asp:ListItem Value="2">Exhibition</asp:ListItem>
                </asp:DropDownList>
            </div>
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
                    Group By
                </div>
                <asp:DropDownList ID="groupby" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product</asp:ListItem>
                    <asp:ListItem Value="1">Product Item</asp:ListItem>
                    <asp:ListItem Value="2">Sales</asp:ListItem>
                    <asp:ListItem Value="3">Customer</asp:ListItem>
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
                    <asp:TableHeaderCell Width="90">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Jumlah</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Discount</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Discount Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Discount Voucher</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Discount Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Net Weight (gr)</asp:TableHeaderCell>

                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <hr />
        <div class="table-responsive">
            <asp:Table ID="tb2" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="130">Keterangan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="2" style="text-align: right;">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="2" style="text-align: right;">Exchange</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="2" style="text-align: right;">Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="1" style="text-align: right;">Gross Omset</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="2" style="text-align: right;">Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="2" style="text-align: right;">Resell di Store Lain</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="2" style="text-align: right;">Resell asal Store Lain</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="160" ColumnSpan="2" style="text-align: right;">Net Omzet</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
</asp:Content>

