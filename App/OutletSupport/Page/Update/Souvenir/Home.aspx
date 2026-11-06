<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Home.aspx.cs" inherits="Page_Approval_Product" %>

<asp:content contentplaceholderid="h1" runat="server">
	Souvenir
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
						<asp:tableheadercell width="170">Nama</asp:tableheadercell>
						<asp:tableheadercell width="150">Operator</asp:tableheadercell>
						<asp:tableheadercell width="130">Start</asp:tableheadercell>
						<asp:tableheadercell width="130">Expired </asp:tableheadercell>   
					</asp:tableheaderrow>
				</asp:table>
			</div>
		</form>
	</div>
</asp:content>

