<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="MU.aspx.cs" inherits="Page_Approval_MU" %>

<asp:content contentplaceholderid="h1" runat="server">
	Mark-Up System
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Approval
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
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
				Product Category :
			</div>
			<asp:dropdownlist id="category" runat="server" cssclass="form-control" width="200">
				<asp:listitem>Semua</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<div class="text-muted">
				Product Level :
			</div>
			<asp:dropdownlist id="level" runat="server" cssclass="form-control" width="200">
				<asp:listitem>Semua</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<div class="text-muted">
				Stone Distribution :
			</div>
			<asp:dropdownlist id="dist" runat="server" cssclass="form-control" width="200">
				<asp:listitem>Semua</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="clearfix">
			<br class="hidden-xs" />
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
			<div class="table-responsive">
				<asp:table id="tb" runat="server" cssclass="table-list">
					<asp:tableheaderrow>
						<asp:tableheadercell width="100" />
						<asp:tableheadercell width="250">Stone Distribution</asp:tableheadercell>
						<asp:tableheadercell width="175">Operator</asp:tableheadercell>
						<asp:tableheadercell width="130" cssclass="text-right">Persen</asp:tableheadercell>
						<asp:tableheadercell width="130" cssclass="text-right">Pending</asp:tableheadercell>
						<asp:tableheadercell width="150" cssclass="text-right">Mutasi</asp:tableheadercell>
					</asp:tableheaderrow>
				</asp:table>
			</div>
			<div class="button-left">
				<asp:dropdownlist id="process_id" runat="server" visible="false" />
				<button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
					<i class="icon-fixed-width icon-check"></i>
					Approved All
				</button>
			</div>
		</form>
	</div>
</asp:content>

