<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="Design_PricingSubmit.aspx.cs" inherits="Page_Approval_Design_PricingSubmit" %>

<asp:content ID="Content1" contentplaceholderid="h1" runat="server">
	Diamond Jewelry
</asp:content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Approval
</asp:content>

<asp:content ID="Content3" contentplaceholderid="content" runat="server">
	<p>
		<span class="text-muted">Nomor :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		frame
	</div>
	<table class="table-file">
		
		<tr>
			<td class="text-label">Netto Weight</td>
			<td>:</td>
			<td class="text-bold text-right fixed-150" id="netto" runat="server" />
			<td><small class="text-silver">Gram</small></td>
		</tr>
		<tr>
			<td class="text-label">Kadar Tukaran</td>
			<td>:</td>
			<td class="text-bold text-right fixed-150" id="kadar" runat="server" />
			<td><small class="text-silver">%</small></td>
		</tr>
	</table>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		stone
	</div>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-report">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="250">Stone</asp:tableheadercell>
				<asp:tableheadercell width="90">Tgl.</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-center">Total Butir</asp:tableheadercell>
                <asp:tableheadercell width="130" cssclass="text-center">Total Carat</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		mounting fee
	</div>
	<div class="row">
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">Brand</td>
					<td>:</td>
					<td id="brand1" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Product Item</td>
					<td>:</td>
					<td id="item1" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Cons. Process</td>
					<td>:</td>
					<td id="cons1" runat="server" />
				</tr>
			</table>
		</div>
		<br class="visible-sm visible-xs" />
	</div>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		Estimasi Harga
	</div>
	<div class="row">
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">Category</td>
					<td>:</td>
					<td id="cat1" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Level</td>
					<td>:</td>
					<td id="lvl1" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Stone Dist.</td>
					<td>:</td>
					<td id="dist1" runat="server" />
				</tr>
			</table>
			
		</div>
		<br class="visible-sm visible-xs" />
		<div class="col-md-5">
			<p>
				<span class="text-muted">Final Price (USD) :</span><br />
				<span class="text-lg" id="hargausd" runat="server" />
				<small class="text-silver">USD</small><br />

			</p>
			<br />
			<p>
				<span class="text-muted">Final Price (IDR) :</span><br />
				<span class="text-lg text-orange" id="hargaidr" runat="server" />
				<small class="text-silver">IDR</small><br />
			</p>
		</div>
	</div>
	<hr />
	<div class="file-subheader hidden-sm hidden-xs">
		segmen
	</div>
	<div class="row">
		<div class="col-md-5">
			<table class="table-file">
				<tr>
					<td class="text-label">Category</td>
					<td>:</td>
					<td id="cat2" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Level</td>
					<td>:</td>
					<td id="lvl2" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Stone Dist.</td>
					<td>:</td>
					<td id="dist2" runat="server" />
				</tr>
			</table>
		</div>
		<br class="visible-sm visible-xs" />
		<div class="col-md-5">
			<p>
				<span class="text-xl" id="segmen" runat="server" />
			</p>
			<br />
		</div>
	</div>
    <div class="row">
		<div class="col-xs-12">
            <asp:Literal ID="lit_test_msg" runat="server"></asp:Literal>
            <asp:Literal ID="lit_msg" runat="server"></asp:Literal>
        </div>
    </div>

	<hr />
	<form id="Form1" runat="server">
		<div class="button-left">
			<%--<button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
				<i class="icon-check"></i>
				Approved
			</button>--%>

            <asp:LinkButton ID="go" CssClass="btn btn-default btn-md" runat="server" OnClick="save_Click">
                <span class="glyphicon glyphicon-ok pull-left" aria-hidden="true"></span>
                <span class="pull-left">&nbsp;&nbsp;Approve</span>
                <img runat="server" id="img_1" src="/Images/web/logo-sap.png" class="img-responsive pull-right" border="0" style="height:20px; margin:0 auto;" />
            </asp:LinkButton>
		</div>
	</form>
</asp:content>

