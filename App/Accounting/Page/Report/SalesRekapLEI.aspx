<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SalesRekapLEI.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Laporan Penjualan LEI
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Search :
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="300" placeholder="Search SO Number..." />
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
                    Status
                </div>
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0" Selected="True">Status</asp:ListItem>
                    <asp:ListItem Value="1">Active</asp:ListItem>
                    <asp:ListItem Value="2">Void</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Payment Type
                </div>
                <asp:DropDownList ID="payment" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Payment</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Characteristic Product :
                </div>
                <asp:DropDownList ID="productitem" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Product Item</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Gold Level :
                </div>
                <asp:DropDownList ID="goldlevel" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Level</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Gold Model :
                </div>
                <asp:DropDownList ID="goldmodel" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Model</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:DropDownList ID="filter" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Filter</asp:ListItem>
                    <asp:ListItem Value="1">Basic Item</asp:ListItem>
                    <asp:ListItem Value="2">Non Basic</asp:ListItem>
                </asp:DropDownList>
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
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="goblog" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                        <i class="icon-search"></i>
                        Display
                    </button>
                    <button id="export" runat="server" class="btn btn-primary" onserverclick="export_ServerClick">
                        Export
                    </button>
                </div>
            </div>
        </form>
		<br />
		<asp:Table ID="tb" runat="server" CssClass="table table-responsive table-bordered">
			<asp:TableHeaderRow style="background-color:lightblue">
				<asp:TableHeaderCell style="font-family:consolas">No.</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Tanggal</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">No. Sales Order</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">No. Referensi</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Lokasi</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Tipe</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">PLU</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Item</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Item Description</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Kode Karet</asp:TableHeaderCell>
                <asp:TableHeaderCell style="font-family:consolas">Gold Level</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Gold Model</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Nama Customer</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Sales</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Qty</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">M</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Harga</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Discount</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Discount Program</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Discount Voucher</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Discount Nominal</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Discount Promo</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Total Bayar</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Kadar Tukaran Beli</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Nett Weight</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">LM Weight</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Rate LM</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Nominal Laku Emas</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Price + PPN</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Price Exclude VAT</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">PPN</asp:TableHeaderCell>
				<asp:TableHeaderCell Style="font-family: consolas">TGP Jual</asp:TableHeaderCell>
				<asp:TableHeaderCell Style="font-family: consolas">TGP Beli</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Nominal Laku Emas (TGP Jual)</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Price + PPN (TGP Jual)</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Price Exclude VAT (TGP Jual)</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">PPN (TGP Jual)</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">Jenis Transaksi</asp:TableHeaderCell>
				<asp:TableHeaderCell style="font-family:consolas">EDC</asp:TableHeaderCell>
			</asp:TableHeaderRow>
		</asp:Table>
    </div>
</asp:Content>
