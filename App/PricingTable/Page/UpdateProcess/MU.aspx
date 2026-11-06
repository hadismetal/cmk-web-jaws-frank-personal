<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="MU.aspx.cs" inherits="Page_UpdateProcess_MU" %>

<asp:content contentplaceholderid="h1" runat="server">
	Mark-Up System
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update Process
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
		<tr>
			<td class="text-label">Stone Distribution</td>
			<td>:</td>
			<td id="header_dist" runat="server" class="text-bold" />
		</tr>
	</table>
	<hr />
	<form runat="server">
		<table class="table-form">
			<tr>
				<td colspan="2">Persen :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="persen" runat="server" cssclass="form-control text-lg text-center" width="110" />
						<span class="input-group-addon">%</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="persen_RF" runat="server" />
					<span id="persen_ERR" runat="server" class="text-validator" visible="false">Format</span>
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
