<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Design_StoneAdd.aspx.cs" inherits="Page_Register_Design_StoneAdd" %>

<%@ register src="MenuDesign.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewelry
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Nomor :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<hr />
	<form runat="server" class="form-inline">
		<div class="form-group cb-multi">
			<asp:radiobuttonlist id="stoneindex" runat="server" repeatcolumns="2" />
		</div>
		<hr />
		<div class="form-group">
			<div class="text-muted">
				Search :
			</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
		</div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
				<a id="cancel" runat="server" class="btn btn-default" href="Design_Stone.aspx">
					<i class="icon-remove"></i>
					Cancel
				</a>
			</div>
		</div>
		<div class="table-responsive">
			<asp:table id="tb" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
					<asp:tableheadercell width="200">Nama</asp:tableheadercell>
					<asp:tableheadercell width="200">Kode Parcel</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
</asp:content>

