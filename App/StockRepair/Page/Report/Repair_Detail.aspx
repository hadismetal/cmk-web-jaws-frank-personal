<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Repair_Detail.aspx.cs" inherits="Page_Register_Repair_Submit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Repair Invoice
</asp:Content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Report Detail
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	
	<p>
		<span class="text-muted">Nomor Invoice :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		product characteristics
	</div>
	<div class="row">
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">PLU</td>
					<td>:</td>
					<td id="productplu" runat="server" />
				</tr>
                <tr>
					<td class="text-label">Product Item</td>
					<td>:</td>
					<td id="productitem" runat="server" />
				</tr>
                <tr>
					<td class="text-label">Product Category</td>
					<td>:</td>
					<td id="productcategory" runat="server" />
				</tr>
                <tr>
					<td class="text-label">Product Level</td>
					<td>:</td>
					<td id="productlevel" runat="server" />
				</tr>
			</table>
		</div>
		<br class="visible-sm visible-xs" />
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">Gross Weight</td>
					<td>:</td>
					<td class="text-right" id="grossweight" runat="server" />
					<td class="text-silver"><small>Gram</small></td>
				</tr>
				<tr>
					<td class="text-label">Total Butir</td>
					<td>:</td>
					<td class="text-right" id="totalbutir" runat="server" />
					<td class="text-silver"><small>Butir</small></td>
				</tr>
                <tr>
					<td class="text-label">Selisih Berat</td>
					<td>:</td>
					<td class="text-right" id="selisihberat" runat="server" />
					<td class="text-silver"><small>Gram</small></td>
				</tr>
                <tr>
					<td class="text-label">Total Harga</td>
					<td>:</td>
					<td class="text-right" id="totalharga" runat="server" />
					<td class="text-silver"><small>IDR</small></td>
				</tr>
			</table>
		</div>
	</div>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		stone
	</div>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="200">Stone</asp:tableheadercell>
				<asp:tableheadercell width="50">P</asp:tableheadercell>
				<asp:tableheadercell width="50">L</asp:tableheadercell>
				<asp:tableheadercell width="50">T</asp:tableheadercell>
				<asp:tableheadercell width="110" cssclass="text-right">Carat / Butir</asp:tableheadercell>
				<asp:tableheadercell width="100" cssclass="text-center">Total Butir</asp:tableheadercell>
				<asp:tableheadercell width="140" cssclass="text-center">Total Carat</asp:tableheadercell>
                <asp:tableheadercell width="140" cssclass="text-center">Harga</asp:tableheadercell>
                <asp:tableheadercell width="250">Keterangan</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<hr />
    <div class="file-subheader hidden-sm hidden-xs">
		List Repair
	</div>
	<div class="table-responsive">
		<asp:table id="tb2" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="150">Item</asp:tableheadercell>
                <asp:tableheadercell width="150">Biaya</asp:tableheadercell>
                <asp:tableheadercell width="250">Keterangan</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<hr />
</asp:content>

