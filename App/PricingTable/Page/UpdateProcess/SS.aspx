<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="SS.aspx.cs" inherits="Page_UpdateProcess_SS" %>

<asp:content contentplaceholderid="h1" runat="server">
	Segmen System
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
		<tr>
			<td class="text-label">Product Segmen</td>
			<td>:</td>
			<td id="header_segmen" runat="server" class="text-bold" />
		</tr>
	</table>
	<hr />
	<h4 id="header_sistem" runat="server" />
	<form runat="server">
		<table class="table-form">
			<tr>
				<td colspan="2">Minimum :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="minimum" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon" id="satuan" runat="server" />
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="minimum_RF" runat="server" />
					<span id="minimum_ERR" runat="server" class="text-validator" visible="false">Format</span>
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
