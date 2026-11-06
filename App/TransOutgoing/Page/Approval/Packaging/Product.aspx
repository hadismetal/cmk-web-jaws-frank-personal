<%@ page EnableEventValidation="false" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Product.aspx.cs" inherits="DJ" %>

<asp:content contentplaceholderid="h1" runat="server">
	Packaging
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Approval
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	
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
					<asp:tableheadercell width="150" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
					<asp:tableheadercell width="250">From</asp:tableheadercell>
                    <asp:tableheadercell width="40"></asp:tableheadercell>
                    <asp:tableheadercell width="250">To</asp:tableheadercell>
                    <asp:tableheadercell width="100">Total Item</asp:tableheadercell>
			        <asp:tableheadercell width="100">Status</asp:tableheadercell>
			        <asp:tableheadercell width="160">User & Tanggal</asp:tableheadercell>
                </asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
</asp:content>

