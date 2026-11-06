<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="PG.aspx.cs" inherits="Page_UpdateProcess_FF" %>

<asp:content contentplaceholderid="h1" runat="server">
	Plain Gold
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
			<td class="text-label">Keterangan</td>
			<td>:</td>
			<td id="header_keterangan" runat="server" class="text-bold" />
		</tr>
	</table>
	<hr />
	<form runat="server">
		<table class="table-form">
			<tr>
				<td colspan="2">Merchandise Exchange Min :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="me_min" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">week</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="me_min_RF" runat="server" />
					<span id="me_min_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td colspan="2">Merchandise Exchange Max :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="me_max" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">week</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="me_max_RF" runat="server" />
					<span id="me_max_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td colspan="2">Merchandise Exchange :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="me" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">%</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="me_RF" runat="server" />
					<span id="me_ERR" runat="server" class="text-validator" visible="false">Format</span>
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
