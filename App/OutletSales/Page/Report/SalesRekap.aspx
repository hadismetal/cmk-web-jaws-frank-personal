<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SalesRekap.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Sales Detail
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">

    
    <style type="text/css">
        table.floatThead-table {
            border-top: none;
            border-bottom: none;
            background-color: #FFF;
        }
        table.table-list {
            border-top: none;
            border-bottom: none;
            background-color: #FFF;
        }
        table#content_tb {
            border-top: none;
            border-bottom: none;
            background-color: #FFF;
        }
    </style>

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
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="-1">Status</asp:ListItem>
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="2">Active</asp:ListItem>
                    <asp:ListItem Value="1">Void</asp:ListItem>
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
                    <button id="Button2" runat="server" class="btn btn-primary" onserverclick="Export">
                        <i class="icon-search"></i>
                        Export
                    </button>
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Print">
                        <i class="icon-search"></i>
                        Print
                    </button>
                </div>
            </div>
        </form>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40" />
                    <asp:TableHeaderCell Width="120" />
                    <asp:TableHeaderCell Width="130">Tanggal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. Sales Order</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. Referensi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Lokasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Tipe</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Supplier Nomor</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Nama Customer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="40">Qty</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Program</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Voucher</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Nominal</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Discount Promo</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Harga Net</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Price Exclude VAT</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">PPN</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Total Discount</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Cash / Tunai</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Kartu Kredit</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Transfer</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Debit</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Voucher</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Exchange</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Poin</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">DP PO</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Bank</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Program</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

    </div>


    <script src="../../../UI/JS/jquery.floatThead.js"></script>

    <script>
        var $table = $('table#content_tb');
	    $table.floatThead({
		    scrollContainer: function($table){
			    return $table.closest('.wrapper');
		    }
	    });
	</script>
</asp:Content>

