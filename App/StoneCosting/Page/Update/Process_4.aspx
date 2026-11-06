<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="Process_4.aspx.cs" inherits="Page_Update_Process_4" %>

<asp:content contentplaceholderid="h1" runat="server">
	Semi Precious
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update Process
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p>
		<span class="text-muted">Nama :</span><br />
		<span class="text-lg text-warning" id="header_nama" runat="server" />
	</p>
	<hr />
	<form runat="server">
		<table class="table-form">
			<tr>
				<td colspan="2">Harga :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="harga" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">USD</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="harga_RF" runat="server" />
					<span id="harga_ERR" runat="server" class="text-validator" visible="false">Format</span>
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
