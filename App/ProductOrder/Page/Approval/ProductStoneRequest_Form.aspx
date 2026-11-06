<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="ProductStoneRequest_Form.aspx.cs" inherits="Page_Approval_ProductStoneRequest_Form" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewelry
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Approval
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
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
	<div class="row">
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td>No. FPS</td>
					<td>:</td>
					<td id="noproductstonerequest" runat="server" />
				</tr>
				<tr>
					<td>Tanggal</td>
					<td>:</td>
					<td id="tgl" runat="server" />
				</tr>
                <tr>
					<td>Brand</td>
					<td>:</td>
					<td id="brand" runat="server" />
				</tr>
                <tr>
					<td>Tanggal Jatuh Tempo</td>
					<td>:</td>
					<td id="tgljatuhtempo" runat="server" />
				</tr>
				<tr>
					<td>Keterangan</td>
					<td>:</td>
					<td id="keterangan" runat="server" />
				</tr>
			</table>
		</div>
	</div>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		Parcel
	</div>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="250">Parcel</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">P</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">L</asp:tableheadercell>
				<asp:tableheadercell width="70" cssclass="text-center">T</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-center">Total Butir</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right">Total Carat</asp:tableheadercell>
                <asp:tableheadercell width="130" cssclass="text-right">GIA</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<hr />

	<form runat="server">
		<div class="button-left">
			<button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
				<i class="icon-check"></i>
				Approve
			</button>&nbsp;&nbsp;
            <button id="reject" runat="server" class="btn btn-success" onserverclick="reject_Click">
				<i class="icon-check"></i>
				Reject
			</button>
		</div>
	</form>
</asp:content>

