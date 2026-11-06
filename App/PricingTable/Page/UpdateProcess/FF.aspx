<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="FF.aspx.cs" inherits="Page_UpdateProcess_FF" %>

<asp:content contentplaceholderid="h1" runat="server">
	Finishing Fee
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update Process
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<table class="table-file">
		<tr>
			<td class="text-label">Product Item</td>
			<td>:</td>
			<td id="header_item" runat="server" class="text-bold" />
		</tr>
		<tr>
			<td class="text-label">Finishing Process</td>
			<td>:</td>
			<td id="header_finishing" runat="server" class="text-bold" />
		</tr>
	</table>
	<hr />
	<form runat="server">
		<table class="table-form">
			<tr>
				<td colspan="2">Biaya :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="biaya" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="biaya_RF" runat="server" />
					<span id="biaya_ERR" runat="server" class="text-validator" visible="false">Format</span>
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
