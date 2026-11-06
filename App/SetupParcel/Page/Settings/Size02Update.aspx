<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="Size02Update.aspx.cs" inherits="Page_Settings_Size02Update" %>

<asp:content contentplaceholderid="h1" runat="server">
	Colour Diamond
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Size Control
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p>
		<span class="text-muted">Kode Parcel :</span><br />
		<span class="text-lg" id="header_nama" runat="server" />
	</p>
	<hr />
	<form runat="server">
		<table class="table-form">
			<tr>
				<td>Size Minimum</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="sizemin" runat="server" cssclass="form-control text-lg text-right" width="120" />
						<span class="input-group-addon">Carat</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="sizemin_RF" runat="server" />
					<span id="sizemin_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
			<tr>
				<td>Size Maximum</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="sizemax" runat="server" cssclass="form-control text-lg text-right" width="120" />
						<span class="input-group-addon">Carat</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="sizemax_RF" runat="server" />
					<span id="sizemax_ERR" runat="server" class="text-validator" visible="false">Format</span>
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
