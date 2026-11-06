<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="Design_Form.aspx.cs" inherits="Page_Approval_Design_Form" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewelry
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Approval
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p>
		<span class="text-muted">Nomor :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<hr />
	<div id="pricing_ERR" runat="server" visible="false">
		<i class="icon-warning-sign icon-5x pull-left text-red"></i>
		<h4>Pricing Invalid</h4>
		<p class="text-muted">
			Transaksi tidak bisa di-input ke dalam sistem.<br />
			Komponen pricing yang belum diatur : <span id="sebab" runat="server" class="text-bold text-red" />
		</p>
		<div class="clearfix"></div>
		<hr />
	</div>
	<div class="file-subheader hidden-sm hidden-xs">
		transaction form
	</div>
	<div class="row">
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">Designer</td>
					<td>:</td>
					<td id="designer" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Market Brand</td>
					<td>:</td>
					<td id="brand" runat="server" />
				</tr>
			</table>
		</div>
		<br class="visible-sm visible-xs" />
		<div class="col-md-5">
			<table class="table-file">
				
				<tr>
					<td class="text-label">Keterangan</td>
					<td>:</td>
					<td id="keterangan" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Tanggal</td>
					<td>:</td>
					<td id="tgl" runat="server" />
				</tr>
			</table>
		</div>
	</div>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		transaction data
	</div>
	<div class="row">
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">No. Design</td>
					<td>:</td>
					<td id="designnomor" runat="server" />
				</tr>
				
				<tr>
					<td class="text-label">Req. Customer</td>
					<td>:</td>
					<td id="requestcustomernama" runat="server" />
				</tr>
			</table>
		</div>
		<br class="visible-sm visible-xs" />
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">Designer</td>
					<td>:</td>
					<td id="humandesignernama" runat="server" />
				</tr>
				
			</table>
		</div>
	</div>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		product characteristics
	</div>
	<div class="row">
		<div class="col-md-5">
			<table class="table-file">
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
				<tr>
					<td class="text-label">Stone Distribution</td>
					<td>:</td>
					<td id="stonedist" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Frame Material</td>
					<td>:</td>
					<td id="framematerial" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Frame Finishing</td>
					<td>:</td>
					<td id="framefinishing" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Frame Color</td>
					<td>:</td>
					<td id="framecolor" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Cons. Process</td>
					<td>:</td>
					<td id="processcons" runat="server" />
				</tr>
			</table>
		</div>
		<br class="visible-sm visible-xs" />
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">Gross Weight</td>
					<td>:</td>
					<td class="fixed-100 text-right" id="grossweight" runat="server" />
					<td class="text-silver"><small>Gram</small></td>
				</tr>
				<tr>
					<td class="text-label">Stone Carat</td>
					<td>:</td>
					<td class="fixed-100 text-right" id="stonecarat" runat="server" />
					<td class="text-silver"><small>Carat</small></td>
				</tr>
				<tr>
					<td class="text-label">Stone Weight</td>
					<td>:</td>
					<td class="fixed-100 text-right" id="stoneweight" runat="server" />
					<td class="text-silver"><small>Gram</small></td>
				</tr>
				<tr>
					<td class="text-label">Netto Weight</td>
					<td>:</td>
					<td class="fixed-100 text-right" id="nettoweight" runat="server" />
					<td class="text-silver"><small>Gram</small></td>
				</tr>
			</table>
			<table class="table-file table-file-nomargin">
				<tr>
					<td class="text-label">Diameter</td>
					<td>:</td>
					<td class="fixed-75 text-right" id="dimensid" runat="server" />
					<td class="text-silver"><small>MM</small></td>
				</tr>
				<tr>
					<td class="text-label">Panjang</td>
					<td>:</td>
					<td class="fixed-75 text-right" id="dimensip" runat="server" />
					<td class="text-silver"><small>MM</small></td>
				</tr>
				<tr>
					<td class="text-label">Lebar</td>
					<td>:</td>
					<td class="fixed-75 text-right" id="dimensil" runat="server" />
					<td class="text-silver"><small>MM</small></td>
				</tr>
				<tr>
					<td class="text-label">Ring Size</td>
					<td>:</td>
					<td class="fixed-75 text-right" id="dimensir" runat="server" />
					<td class="text-silver"><small>MM</small></td>
				</tr>
			</table>
		</div>
	</div>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		design characteristics
	</div>
	<div class="row">
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">Design Category</td>
					<td>:</td>
					<td id="designcategory" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Design Concept</td>
					<td>:</td>
					<td id="designconcept" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Design Process</td>
					<td>:</td>
					<td id="designprocess" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Target Age</td>
					<td>:</td>
					<td id="targetage" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Target Gender</td>
					<td>:</td>
					<td id="targetgender" runat="server" />
				</tr>
			</table>
		</div>
		<br class="visible-sm visible-xs" />
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">Naming Product</td>
					<td>:</td>
					<td id="naming" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Keterangan</td>
					<td>:</td>
					<td id="designket" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Grafir</td>
					<td>:</td>
					<td id="grafir" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Finishing</td>
					<td>:</td>
					<td id="finishing" runat="server" />
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
				<asp:tableheadercell width="250">Stone</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">P</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">L</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">T</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right">Carat / Butir</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-center">Total Butir</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right">Total Carat</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<hr />
	
	<div class="file-subheader hidden-sm hidden-xs">
		picture
	</div>
	<asp:image id="imgpicture" runat="server" cssclass="img-thumbnail" width="500" />
	<hr />
	<form runat="server">
		<div class="button-left">
			<button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
				<i class="icon-check"></i>
				Pricing
			</button>
		</div>
	</form>
</asp:content>

