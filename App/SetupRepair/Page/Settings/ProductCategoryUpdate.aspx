<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="ProductCategoryUpdate.aspx.cs" inherits="Page_Settings_ProductCategoryUpdate" %>

<asp:content contentplaceholderid="h1" runat="server">
	Product Category
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Settings
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p>
		<span class="text-muted">Product Category :</span><br />
		<span class="text-bold" id="header_nama" runat="server" />
	</p>
	<hr />
	<form runat="server">
		<table class="table-form">
			<tr>
				<td>Prototype Code</td>
				<td>:</td>
				<td>
					<asp:textbox id="kodeproto" runat="server" cssclass="form-control text-lg text-center" maxlength="1" width="100" />
				</td>
				<td>
					<asp:requiredfieldvalidator id="kodeproto_RF" runat="server" />
				</td>
			</tr>
		</table>
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-ok"></i>
				Save
			</button>
			<a href="javascript:closeWindow();" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:content>
