<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DJ_Detail.aspx.cs" inherits="Page_DJ_Detail" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewelry
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Report
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
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
	<div id="zero" runat="server" visible="false">
		<i class="icon-ok icon-5x pull-left text-success"></i>
		<h4>Proses Selesai</h4>
		<p class="text-muted">
			Semua data / transaksi dalam kondisi sudah approval.
		</p>
		<div class="clearfix"></div>
		<hr />
	</div>
	<div id="form_area" runat="server">
		<form runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    &nbsp;
                </div>
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="Input PLU / Kode Barang" />
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
            <div class="form-group">
                <div class="text-muted">
                    Status :
                </div>
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">All</asp:ListItem>
                    <asp:ListItem Value="1">Approved</asp:ListItem>
                    <asp:ListItem Value="2">Pending Approval</asp:ListItem>
                    <asp:ListItem Value="3">Rejected</asp:ListItem>
                    <asp:ListItem Value="4">Outstanding </asp:ListItem>
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
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Export">
                        <i class="glyphicon glyphicon-file"></i>
                        Export
                    </button>
                </div>
            </div>
			<div class="table-responsive">
				<asp:table id="tb" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
						<asp:tableheadercell width="90">No.</asp:tableheadercell>
                        <asp:tableheadercell width="140">No. Dokumen</asp:tableheadercell>
                        <asp:tableheadercell width="120">Status</asp:tableheadercell>
                        <asp:tableheadercell width="120">PLU</asp:tableheadercell>
                        <asp:tableheadercell width="120">Kode Barang</asp:tableheadercell>
                        <asp:tableheadercell width="120">Product Category</asp:tableheadercell>
                        <asp:tableheadercell width="120">Item</asp:tableheadercell>
                        <asp:tableheadercell width="80">Quantity</asp:tableheadercell>
						<asp:tableheadercell width="80">Berat Bruto Awal</asp:tableheadercell>
                        <asp:tableheadercell width="80">Berat Netto Awal</asp:tableheadercell>
                         <asp:tableheadercell width="80">Carat Awal </asp:tableheadercell>

                        <asp:tableheadercell width="80">Berat Setelah Bongkar</asp:tableheadercell>
                        <asp:tableheadercell width="80">Susut Bongkar</asp:tableheadercell>
                        <asp:tableheadercell width="80">Susut Lebur</asp:tableheadercell>
                        <asp:tableheadercell width="80">Berat Akhir</asp:tableheadercell>
                        <asp:tableheadercell width="80">Butir Awal</asp:tableheadercell>
                        <asp:tableheadercell width="80">Butir Setor</asp:tableheadercell>
                        <asp:tableheadercell width="80">Butir Setor Pecah</asp:tableheadercell>
                        <asp:tableheadercell width="80">Selisih Butir</asp:tableheadercell>

                        <asp:tableheadercell width="80">Carat Terima</asp:tableheadercell>
                        <asp:tableheadercell width="80">Carat Rusak</asp:tableheadercell>
                        <asp:tableheadercell width="80">Selisih Carat</asp:tableheadercell>
                        <asp:tableheadercell width="200">Keterangan </asp:tableheadercell>
                        <asp:tableheadercell width="200">Alasan </asp:tableheadercell>
						<asp:tableheadercell width="200">Harga M Stone</asp:tableheadercell>
                        <asp:tableheadercell width="200">Harga M Emas</asp:tableheadercell>
                        <asp:tableheadercell width="200">Harga M Ongkos</asp:tableheadercell>
                        <asp:tableheadercell width="200">Harga M</asp:tableheadercell>

                        <asp:tableheadercell width="80">Nama Parcel</asp:tableheadercell>
                        <asp:tableheadercell width="80">Butir Awal Parcel</asp:tableheadercell>
                        <asp:tableheadercell width="80">Carat Awal Parcel</asp:tableheadercell>
                        <asp:tableheadercell width="80">Harga M Per Stone</asp:tableheadercell>
                        <asp:tableheadercell width="80">Butir Terima Parcel</asp:tableheadercell>
                        <asp:tableheadercell width="80">Carat Terima Parcel</asp:tableheadercell>  
                        <asp:tableheadercell width="80">Butir Rusak Parcel</asp:tableheadercell>
                        <asp:tableheadercell width="80">Carat Rusak Parcel</asp:tableheadercell>
                        <asp:tableheadercell width="80">Butir Selisih Parcel</asp:tableheadercell>
                        <asp:tableheadercell width="80">Carat Selisih Parcel</asp:tableheadercell>

                        <asp:TableHeaderCell Width="150">NO GIA</asp:TableHeaderCell>
					</asp:tableheaderrow>
				</asp:table>
			</div>
		</form>
	</div>
</asp:content>

