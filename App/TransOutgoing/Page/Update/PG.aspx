<%@ page EnableEventValidation="false" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="PG.aspx.cs" inherits="PG" %>

<asp:content ID="Content1" contentplaceholderid="h1" runat="server">
	Plain Gold
</asp:content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Approval
</asp:content>

<asp:content ID="Content3" contentplaceholderid="content" runat="server">
	
    <form id="Form1" runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Brand
			</div>
			<asp:dropdownlist id="brand" runat="server" cssclass="form-control" width="200">
                <asp:ListItem Value="-1">All</asp:ListItem>
			</asp:dropdownlist>
		</div>
        <div class="form-group">
			<div class="text-muted">
				Search (PLU / From / To / User)
			</div>
            <asp:TextBox ID="keyword" runat="server" CssClass="form-control text-bold" MaxLength="20" />
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
        <div>
            <br />
            <asp:Literal runat="server" ID="lit_msg" />
        </div>
		<div class="table-responsive">
			<asp:table id="tb" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="50" />
					<asp:tableheadercell width="50">No.</asp:tableheadercell>
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

