<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="GJ_Submit.aspx.cs" inherits="Page_Register_GJ_Submit" %>

<%@ register src="MenuGJ.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Gold Jewellery
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Nomor :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		transaction form
	</div>
	<div class="row">
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">Warehouse</td>
					<td>:</td>
					<td id="warehouse" runat="server" />
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
					<td class="text-label">Sumber</td>
					<td>:</td>
					<td id="sumber" runat="server" />
				</tr>
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
					<td class="text-label">No. Prototype</td>
					<td>:</td>
					<td id="protonomor" runat="server" />
				</tr>
				<tr>
					<td class="text-label">No. Product Order</td>
					<td>:</td>
					<td id="productordernomor" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Supplier</td>
					<td>:</td>
					<td id="suppliernama" runat="server" />
				</tr>
				<tr>
					<td class="text-label">No. Supplier</td>
					<td>:</td>
					<td id="suppliernomor" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Req. Outlet</td>
					<td>:</td>
					<td id="requestoutletnama" runat="server" />
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
				<tr>
					<td class="text-label">Stock Keeper</td>
					<td>:</td>
					<td id="humanstocknama" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Expired Product</td>
					<td>:</td>
					<td id="expiredproduct" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Expired Request</td>
					<td>:</td>
					<td id="expiredrequest" runat="server" />
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
		costing
	</div>
	<table class="table-file">
		<tr>
			<td class="text-label">Unit Price</td>
			<td>:</td>
			<td class="fixed-100 text-right" id="costing_unitprice" runat="server" />
			<td class="text-silver"><small>IDR</small></td>
		</tr>
        <tr>
			<td class="text-label">Frame</td>
			<td>:</td>
			<td class="fixed-100 text-right" id="costing_frame" runat="server" />
			<td class="text-silver"><small>IDR</small></td>
		</tr>
		<tr>
			<td class="text-label">GJ</td>
			<td>:</td>
			<td class="fixed-100 text-right" id="costing_mounting" runat="server" />
			<td class="text-silver"><small>IDR</small></td>
		</tr>
		<tr>
			<td class="text-label">Setting</td>
			<td>:</td>
			<td class="fixed-100 text-right" id="costing_setting" runat="server" />
			<td class="text-silver"><small>IDR</small></td>
		</tr>
		<tr>
			<td class="text-label">Finishing</td>
			<td>:</td>
			<td class="fixed-100 text-right" id="costing_finishing" runat="server" />
			<td class="text-silver"><small>IDR</small></td>
		</tr>
		<tr>
			<td class="text-label">Ongkos Lain - lain</td>
			<td>:</td>
			<td class="fixed-100 text-right" id="costing_lain2" runat="server" />
			<td class="text-silver"><small>IDR</small></td>
		</tr>
		<tr>
			<td class="text-label">Total</td>
			<td>:</td>
			<td class="fixed-100 text-right" id="costing_totalbiaya" runat="server" />
			<td class="text-silver"><small>IDR</small></td>
		</tr>
	</table>
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
				Finish
			</button>
		</div>
	</form>
</asp:content>

