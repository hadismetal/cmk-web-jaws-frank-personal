<%@ page EnableEventValidation="false" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="ProductBroken.aspx.cs" inherits="Product" %>

<%@ register src="MenuProductBroken.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Packaging Broken
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Market Brand :
			</div>
			<asp:dropdownlist id="brand" runat="server" cssclass="form-control" width="200">
                <asp:ListItem Value="0">All</asp:ListItem>
			</asp:dropdownlist>
		</div>
        <div class="form-group">
			<div class="text-muted">
				Storage Type (From) :
			</div>
			<asp:dropdownlist id="ddl_from" runat="server" cssclass="form-control" width="200">
			</asp:dropdownlist>
		</div>
        <div class="form-group">
			<div class="text-muted">
				Search (From) :
			</div>
			<asp:TextBox runat="server" ID="txt_from" cssclass="form-control" width="200" />
		</div>
        <div class="form-group">
			<div class="text-muted">
				Storage Type (To) :
			</div>
			<asp:dropdownlist id="ddl_to" runat="server" cssclass="form-control" width="200">
			</asp:dropdownlist>
		</div>
        
        <div class="form-group">
			<div class="text-muted">
				Search (To) :
			</div>
			<asp:TextBox runat="server" ID="txt_to" cssclass="form-control" width="200" />
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
					<asp:tableheadercell width="250">From <asp:Literal runat="server" ID="lit_TypeFrom" /></asp:tableheadercell>
                    <asp:tableheadercell width="40"></asp:tableheadercell>
                    <asp:tableheadercell width="250">To <asp:Literal runat="server" ID="lit_TypeTo" /></asp:tableheadercell>
			    </asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
</asp:content>

