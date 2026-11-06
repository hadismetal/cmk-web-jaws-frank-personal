<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="FR.aspx.cs" inherits="Page_Update_FF" %>

<asp:content contentplaceholderid="h1" runat="server">
	Repair Fee
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server" class="form-inline">
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
				<asp:tableheadercell width="100" />
				<asp:tableheadercell width="250">Product Item</asp:tableheadercell>
				<asp:tableheadercell width="175">Operator</asp:tableheadercell>
                <asp:tableheadercell width="130" cssclass="text-right"><small>IDR</small> M</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right"><small>IDR</small> M Pending</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right"><small>IDR</small> Biaya</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right"><small>IDR</small> Biaya Pending</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-right">Mutasi</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

