<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="PG.aspx.cs" inherits="Page_Status_FF" %>

<asp:content contentplaceholderid="h1" runat="server">
	Plain Gold
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Status
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server" class="form-inline">
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
		<asp:table id="tb" runat="server" cssclass="table-list text-sm">
			<asp:tableheaderrow>
				<asp:tableheadercell width="100" />
				<asp:tableheadercell width="100" cssclass="text-right">Merchandise Exchange Min <small>week</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Merchandise Exchange Max <small>week</small></asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Merchandise Exchange <small>%</small></asp:tableheadercell>

			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

