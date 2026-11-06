<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Sales_PackagingAdd.aspx.cs" inherits="Page_Register_Sales_ItemAdd" %>

<%@ register src="MenuSales.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Packaging
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
		<div class="form-group">
			<div class="text-muted">
				Search :
			</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
		</div>
        <%--<div class="form-group">
			<div class="text-muted">
				Item :
			</div>
			<asp:DropDownList ID="ddl_item" cssclass="form-control" runat="server" />
		</div>--%>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
				<a id="cancel" runat="server" class="btn btn-default" href="Sales_Data.aspx">
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
					<asp:tableheadercell width="200">Kode</asp:tableheadercell>
					<asp:tableheadercell width="200">Product Item</asp:tableheadercell>
                    <asp:tableheadercell width="200">Qty</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
</asp:content>

