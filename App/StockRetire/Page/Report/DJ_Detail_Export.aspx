<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPageExport.master" AutoEventWireup="true" CodeFile="DJ_Detail_Export.aspx.cs" Inherits="Page_Report_DJ_Detail_Export" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" Runat="Server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" Runat="Server">
    <div id="form_area" runat="server">
        <!-- -->
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

    </div>
</asp:Content>

