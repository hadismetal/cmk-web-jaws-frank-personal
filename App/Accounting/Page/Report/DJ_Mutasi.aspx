<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_Mutasi.aspx.cs" Inherits="Page_Report_DJ_Mutasi" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery Saldo
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
	<style>
		.table-responsive{
			overflow: auto; 
			height: 900px;
		}
		
		.table-responsive #content_TableTop1 { position: sticky; top: 0; z-index: 1; }
		.table-responsive #content_TableTop2 { position: sticky; top: 4%; z-index: 5; }
		.table-responsive #content_TableTop3 { position: sticky; top: 8%; margin-top: 20px; z-index: 10; }
	</style>
    <script>
        $(document).ready(function () {
            $('#<%=datepicker.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
            $('#<%=datepicker2.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
        });
    </script>
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Location Type :
                </div>
                <asp:DropDownList ID="locationType" runat="server" CssClass="form-control" Width="175" AutoPostBack="true" OnSelectedIndexChanged="locationType_SelectedIndexChanged"> 
                    <asp:ListItem Value="9">All</asp:ListItem>
                    <asp:ListItem Value="0">Warehouse</asp:ListItem>
                    <asp:ListItem Value="1">Outlet</asp:ListItem>
                    <asp:ListItem Value="2">Exhibition</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Cabang :
                </div>
                <asp:DropDownList ID="cabang" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Cabang :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    PLU :
                </div>
                <asp:TextBox ID="plu" runat="server" CssClass="form-control" placeholder="PLU"></asp:TextBox>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Product Item :
                </div>
                <asp:DropDownList ID="productItem" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Item :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Product Category :
                </div>
                <asp:DropDownList ID="productCategory" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Category :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Product Level :
                </div>
                <asp:DropDownList ID="productLevel" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Level :</asp:ListItem>
                </asp:DropDownList>
            </div>
             <div class="form-group">
                <div class="text-muted">
                    Stone Dist :
                </div>
                <asp:DropDownList ID="stoneDist" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Stone Dist :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Periode :
                </div>

                <asp:TextBox CssClass="form-control" type="text" placeholder="Dari" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" />

            </div>
            <div class="form-group">
                <div class="text-muted">
                 &nbsp;
                </div>
                <asp:TextBox CssClass="form-control" type="text" placeholder="Sampai" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" />
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Product :
                </div>
                <asp:DropDownList ID="product" runat="server" CssClass="form-control" Width="170">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">DJ</asp:ListItem>
                    <asp:ListItem Value="2">GJ</asp:ListItem>
                    <asp:ListItem Value="3">PG</asp:ListItem>
                    <asp:ListItem Value="4">LD</asp:ListItem>
                </asp:DropDownList>
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
                <div class="text-muted">
                    Group By
                </div>
                <asp:DropDownList ID="groupby" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product</asp:ListItem>
                    <asp:ListItem Value="1">Product Item</asp:ListItem>
                    <asp:ListItem Value="2">Product Category</asp:ListItem>
                    <asp:ListItem Value="3">Product Level</asp:ListItem>
                    <asp:ListItem Value="4">Stone Dist</asp:ListItem>
                    <asp:ListItem Value="5">Location Type</asp:ListItem>
                    <asp:ListItem Value="6">Cabang</asp:ListItem>
                    <asp:ListItem Value="7">Brand</asp:ListItem>
                    <asp:ListItem Value="8">PLU</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="go_ServerClick">
                        <i class="icon-search"></i>
                        Display
                    </button>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="excel" runat="server" class="btn btn-primary" onserverclick="excel_ServerClick">
                        <i class="icon-download-alt"></i>
                        Export Excel
                    </button>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="closing" runat="server" class="btn btn-primary" onserverclick="closing_ServerClick">
                        Closing
                    </button>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="stockAkhir" runat="server" class="btn btn-primary" onserverclick="stockAkhir_ServerClick">
                        <i class="icon-download-alt"></i>
                        Detail Stock Akhir
                    </button>
                </div>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="btnReceive" runat="server" class="btn btn-primary" onserverclick="btnReceive_ServerClick">
                        <i class="icon-download-alt"></i>
                        Detail Receive
                    </button>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="btnSales" runat="server" class="btn btn-primary" onserverclick="btnSales_ServerClick">
                        <i class="icon-download-alt"></i>
                        Detail Sales
                    </button>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="btnGift" runat="server" class="btn btn-primary" onserverclick="btnGift_ServerClick">
                        <i class="icon-download-alt"></i>
                        Detail Gift
                    </button>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="btnResell" runat="server" class="btn btn-primary" onserverclick="btnResell_ServerClick">
                        <i class="icon-download-alt"></i>
                        Detail Resell
                    </button>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="btnTradeIn" runat="server" class="btn btn-primary" onserverclick="btnTradeIn_ServerClick">
                        <i class="icon-download-alt"></i>
                        Detail Trade In
                    </button>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="btnReturn" runat="server" class="btn btn-primary" onserverclick="btnReturn_ServerClick">
                        <i class="icon-download-alt"></i>
                        Detail Return
                    </button>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="btnLebur" runat="server" class="btn btn-primary" onserverclick="btnLebur_ServerClick">
                        <i class="icon-download-alt"></i>
                        Detail Lebur
                    </button>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="btnTransfer" runat="server" class="btn btn-primary" onserverclick="btnTransfer_ServerClick">
                        <i class="icon-download-alt"></i>
                        Detail Transfer
                    </button>
                </div>
            </div>
        </form>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table table-bordered">
                <asp:TableRow style="background-color:lightblue" ID="TableTop1">
                    <asp:TableHeaderCell style="text-align: center; vertical-align: middle; min-width: 250px;" Width="250" RowSpan="3" ><asp:Label runat="server" ID="descHeader">Deskripsi</asp:Label></asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center; vertical-align: middle;" Width="200" ColumnSpan="6" RowSpan="2">Saldo Awal</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center; vertical-align: middle;" Width="800" ColumnSpan="24">Penerimaan</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center; vertical-align: middle;" Width="1000" ColumnSpan="30">Pengeluaran</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center; vertical-align: middle;" Width="200" ColumnSpan="6" RowSpan="2">Akhir Mutasi</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center; vertical-align: middle;" Width="200" ColumnSpan="6" RowSpan="2">Akhir Opname</asp:TableHeaderCell>
                </asp:TableRow>
                <asp:TableRow style="background-color:lightblue" ID="TableTop2">
                    <asp:TableHeaderCell style="text-align: center; vertical-align: middle;" ColumnSpan="6">Receiving</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center; vertical-align: middle;" ColumnSpan="6">Incoming</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center; vertical-align: middle;" ColumnSpan="6">Resell</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center; vertical-align: middle;" ColumnSpan="6">Trade In</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center; vertical-align: middle;" ColumnSpan="6">Outgoing</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center; vertical-align: middle;" ColumnSpan="6">Sales</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center; vertical-align: middle;" ColumnSpan="6">Gift</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center; vertical-align: middle;" ColumnSpan="6">Return</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center; vertical-align: middle;" ColumnSpan="6">Lebur</asp:TableHeaderCell>
                </asp:TableRow>
                <asp:TableRow style="background-color:aliceblue" ID="TableTop3">
                    <asp:TableHeaderCell style="text-align: center">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Harga M</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Qty</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Nett Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Gross Weight</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Butir</asp:TableHeaderCell>
					<asp:TableHeaderCell style="text-align: center">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center">Harga M</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>

            
        </div>

    </div>
</asp:Content>

