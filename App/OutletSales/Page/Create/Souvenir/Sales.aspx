<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Sales.aspx.cs" inherits="Page_Register_Sales" %>

<%@ register src="MenuSales.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content ID="Content1" contentplaceholderid="h1" runat="server">
	Souvenir
</asp:content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content ID="Content3" contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form id="Form1" runat="server" class="form-inline">
		<div class="form-group">
         
			<div class="text-muted">
				Search (Location / Address) :
			</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="250" placeholder="..." />
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
		<div class="row">
            <div class="col-xs-12">
                <br />
			    <asp:table id="tb" runat="server" cssclass="table-list" style="table-layout:auto;">
				    <asp:tableheaderrow>
					    <asp:tableheadercell width="90" />
					    <asp:tableheadercell width="40">No.</asp:tableheadercell>
					    <asp:tableheadercell width="220">Location</asp:tableheadercell>
					    <asp:tableheadercell width="200">Address</asp:tableheadercell>
					    <asp:tableheadercell width="150">Group</asp:tableheadercell>
				    </asp:tableheaderrow>
			    </asp:table>
            </div>
        </div>
	</form>
</asp:content>

