<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="FM.aspx.cs" inherits="Page_UpdateProcess_FM" %>

<asp:content contentplaceholderid="h1" runat="server">
	Mounting Fee
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update Process
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<table class="table-file">
		<tr>
			<td class="text-label">Market Brand</td>
			<td>:</td>
			<td id="header_brand" runat="server" class="text-bold" />
		</tr>
		<tr>
			<td class="text-label">Product Item</td>
			<td>:</td>
			<td id="header_item" runat="server" class="text-bold" />
		</tr>
		<tr>
			<td class="text-label">Cons. Process</td>
			<td>:</td>
			<td id="header_cons" runat="server" class="text-bold" />
		</tr>
	</table>
	<hr />
	<form runat="server">
		<table class="table-form">
            <tr>
				<td colspan="2">Pembagi</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="pembagi" runat="server" cssclass="form-control text-lg text-right" width="175" />
						
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="pembagi_RF" runat="server" />
					<span id="pembagi_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
			<tr>
				<td colspan="2">Biaya NO :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="inputno" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="inputno_RF" runat="server" />
					<span id="inputno_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
			<tr>
				<td colspan="2">Biaya RO :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="inputro" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="inputro_RF" runat="server" />
					<span id="inputro_ERR" runat="server" class="text-validator" visible="false">Format</span>
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
