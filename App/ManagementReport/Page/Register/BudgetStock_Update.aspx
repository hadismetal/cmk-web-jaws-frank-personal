<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="BudgetStock_Update.aspx.cs" inherits="Page_UpdateProcess_MU" %>

<asp:content contentplaceholderid="h1" runat="server">
	Budget Stock
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update Process
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<table class="table-file">
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
	<form runat="server">
		<table class="table-form">
			<tr>
				<td colspan="2">Target Qty :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="qty" runat="server" cssclass="form-control text-lg text-center" width="250" />
						<span class="input-group-addon">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="qty_RF" runat="server" />
					<span id="qty_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td colspan="2">Target Omset :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="omset" runat="server" cssclass="form-control text-lg text-center" width="250" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="omset_RF" runat="server" />
					<span id="omset_ERR" runat="server" class="text-validator" visible="false">Format</span>
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
