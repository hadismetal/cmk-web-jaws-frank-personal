<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="SegmenUpdate.aspx.cs" inherits="Page_Settings_SegmenUpdate" %>

<asp:content contentplaceholderid="h1" runat="server">
	Segmen System
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Settings
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<table class="table-file">
		<tr>
			<td class="text-label">Product Category</td>
			<td>:</td>
			<td id="header_category" runat="server" class="text-bold" />
		</tr>
		<tr>
			<td class="text-label">Product Level</td>
			<td>:</td>
			<td id="header_level" runat="server" class="text-bold" />
		</tr>
	</table>
	<hr />
	<form runat="server">
		<div class="form-group cb-multi">
			<p>
				Sistem :
			</p>
			<asp:radiobuttonlist id="sistem" runat="server" />
		</div>
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
