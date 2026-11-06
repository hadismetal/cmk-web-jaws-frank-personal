<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="ProductAdjustmentAdd.aspx.cs" inherits="Page_Register_ProductAdd" %>

<%@ register src="MenuAdjustment.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Packaging Adjusment
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Search :
			</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
		</div>
		<div class="clearfix">
			<br class="hidden-xs" />
		</div>
		<div class="form-group">
			<div class="text-muted">
				Brand :
			</div>
			<asp:dropdownlist id="brand" runat="server" cssclass="form-control" width="170">
				<asp:ListItem Value="0">All</asp:ListItem>
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
		<div class="table-responsive">
			<asp:table id="tb" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
					<asp:tableheadercell width="170">Nama</asp:tableheadercell>
					<asp:tableheadercell width="170">Kode</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
</asp:content>

