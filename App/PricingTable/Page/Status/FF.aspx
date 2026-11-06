<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="FF.aspx.cs" inherits="Page_Status_FF" %>

<asp:content contentplaceholderid="h1" runat="server">
	Finishing Fee
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Status
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Product Item :
			</div>
			<asp:dropdownlist id="item" runat="server" cssclass="form-control" width="200">
				<asp:listitem>Semua</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<div class="text-muted">
				Finishing Process :
			</div>
			<asp:dropdownlist id="finishing" runat="server" cssclass="form-control" width="200">
				<asp:listitem>Semua</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="clearfix">
			<br class="hidden-xs" />
		</div>
		<div class="form-group">
			<div class="text-muted">
				Status :
			</div>
			<asp:dropdownlist id="status" runat="server" cssclass="form-control" width="175">
				<asp:listitem>Semua</asp:listitem>
			</asp:dropdownlist>
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
	</form>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="90" />
				<asp:tableheadercell width="275">Product Item</asp:tableheadercell>
				<asp:tableheadercell width="175">Approval</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right"><small>IDR</small> Biaya</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

