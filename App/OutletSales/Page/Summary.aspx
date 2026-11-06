<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Summary.aspx.cs" inherits="Page_Summary" %>

<asp:content contentplaceholderid="h1" runat="server">
	Summary
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<%--<asp:tableheaderrow>
				<asp:tableheadercell width="200">Keterangan</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-center">Qty</asp:tableheadercell>
                <asp:tableheadercell width="150" cssclass="text-center">Value</asp:tableheadercell>
			</asp:tableheaderrow>--%>
		</asp:table>
	</div>
</asp:content>

