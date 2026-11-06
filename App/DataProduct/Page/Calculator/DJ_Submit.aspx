<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DJ_Submit.aspx.cs" inherits="Page_Register_DJ_Submit" %>

<%@ register src="MenuDJ.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewelry
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div class="text-danger text-sm">
        Tanggal Cetak : <asp:Literal ID="tglcetak" runat="server" />
    </div>
    <br />
	
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
				<asp:tableheadercell width="400">Stone</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">P</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">L</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">T</asp:tableheadercell>
				<asp:tableheadercell width="120" cssclass="text-right">Carat / Butir</asp:tableheadercell>
				<asp:tableheadercell width="120" cssclass="text-center">T. Butir</asp:tableheadercell>
				<asp:tableheadercell width="120" cssclass="text-right">T. Carat</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		Price
	</div>
	<table class="table-file">
		<tr>
			<td class="text-label text-lg">Price Estimation</td>
			<td>:</td>
			<td class="fixed-100 text-right text-lg text-warning" id="price" runat="server" />
			<td class="text-silver text-lg"><small>IDR</small></td>
		</tr>
        
	</table>
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

