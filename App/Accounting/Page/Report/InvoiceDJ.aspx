<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="InvoiceDJ.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Sales Order Diamond Jewellery
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
                    <asp:ListItem Value="0">All</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="-1">Status</asp:ListItem>
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Void</asp:ListItem>
                </asp:DropDownList>
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
                    <asp:TableHeaderCell Width="50" />
                    <asp:TableHeaderCell Width="130">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Nama Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Voucher</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Parcel</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">T. Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">T. Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>

                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>
</asp:Content>

