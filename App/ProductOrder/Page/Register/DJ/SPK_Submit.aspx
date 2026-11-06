<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="SPK_Submit.aspx.cs" inherits="Page_Register_SPK_Submit" %>

<%@ Register Src="MenuSPK.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="HeaderSPK.ascx" TagName="Header" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <uc2:Header ID="header" runat="server" />
    <uc1:Menu ID="menu" runat="server" />

	<div class="file-subheader hidden-sm hidden-xs">
		transaction form
	</div>
	<div class="row">
		
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
					<td class="text-label">No. Ref</td>
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
                <tr>
					<td class="text-label">Ring Size 2</td>
					<td>:</td>
					<td class="fixed-75 text-right" id="dimensir2" runat="server" />
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
					<td class="text-label">Grafir 2</td>
					<td>:</td>
					<td id="grafir2" runat="server" />
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
				Finish
			</button>
		</div>
	</form>
</asp:content>

