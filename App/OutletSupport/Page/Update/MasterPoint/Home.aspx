<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Home.aspx.cs" inherits="Page_Approval_Product" %>

<asp:content contentplaceholderid="h1" runat="server">
	Master Point
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div id="form_area" runat="server">
		<form runat="server" class="form-inline">
			<div class="table-responsive">
				<asp:table id="tb" runat="server" cssclass="table-list text-sm">
					<asp:tableheaderrow>
						<asp:tableheadercell width="100" />
						<asp:tableheadercell width="40">No.</asp:tableheadercell>
						<asp:tableheadercell width="170">Point</asp:tableheadercell>
						<asp:tableheadercell width="150">Rupiah</asp:tableheadercell>
					</asp:tableheaderrow>
				</asp:table>
			</div>
		</form>
	</div>
</asp:content>

