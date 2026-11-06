<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="MULD.aspx.cs" inherits="Page_Update_MU" %>

<asp:content contentplaceholderid="h1" runat="server">
	Resell Loose Diamond Mark-Up System
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="100" />
				<asp:tableheadercell width="130" cssclass="text-right">Persen</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

