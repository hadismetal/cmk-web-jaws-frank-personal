<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Resell.aspx.cs" inherits="Page_Register_Resell" %>

<%@ register src="MenuResell.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content ID="Content1" contentplaceholderid="h1" runat="server">
	Resell
</asp:content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content ID="Content3" contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form id="Form1" runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Search :
			</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
		</div>
		<div class="form-group">
			<div class="text-muted">
				Market Brand :
			</div>
			<asp:dropdownlist id="brand" runat="server" cssclass="form-control" width="200">
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
		<div class="table-responsive">
            <h3>Outlet</h3>
			<asp:table id="tb" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
					<asp:tableheadercell width="250">Outlet</asp:tableheadercell>
					<asp:tableheadercell width="250">Address</asp:tableheadercell>
					<asp:tableheadercell width="75" cssclass="text-center">Group</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
            
            <hr />
            <h3>Exhibition</h3>
			<asp:table id="tb2" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
					<asp:tableheadercell width="250">Exhibition</asp:tableheadercell>
					<asp:tableheadercell width="250">Address</asp:tableheadercell>
					<asp:tableheadercell width="75" cssclass="text-center">Group</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
</asp:content>

