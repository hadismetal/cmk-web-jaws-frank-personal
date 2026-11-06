<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Designer.aspx.cs" inherits="Page_Create_Designer" %>

<%@ register src="MenuDesigner.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Designer
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form runat="server">
		<table class="table-form">
			<tr>
				<td>Group</td>
				<td>:</td>
				<td>
					<asp:dropdownlist id="group" runat="server" cssclass="form-control text-bold" width="275" />
				</td>
				<td>
					<span id="group_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				</td>
			</tr>
			<tr>
				<td>Market Brand</td>
				<td>:</td>
				<td>
					<asp:dropdownlist id="brand" runat="server" cssclass="form-control text-bold" width="275" />
				</td>
				<td>
					<span id="brand_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				</td>
			</tr>
		</table>
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
		</div>
	</form>
</asp:content>

