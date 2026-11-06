<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="DJ.aspx.cs" inherits="Page_UpdateProcess_FF" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewellery
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
						<span class="input-group-addon">month</span>
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
						<span class="input-group-addon">month</span>
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
            <tr>
				<td colspan="2">Trade In Periode 1 Min :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="ti1_min" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">month</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="ti1_min_RF" runat="server" />
					<span id="ti1_min_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td colspan="2">Trade In Periode 1 Max :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="ti1_max" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">month</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="ti1_max_RF" runat="server" />
					<span id="ti1_max_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td colspan="2">Trade In Periode 1 :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="ti1" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">%</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="ti1_RF" runat="server" />
					<span id="ti1_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td colspan="2">Trade In Periode 2 Min :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="ti2_min" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">month</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="ti2_min_RF" runat="server" />
					<span id="ti2_min_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td colspan="2">Trade In Periode 2 Max :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="ti2_max" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">month</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="ti2_max_RF" runat="server" />
					<span id="ti2_max_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td colspan="2">Trade In Periode 2 :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="ti2" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">%</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="ti2_RF" runat="server" />
					<span id="ti2_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td colspan="2">Resell Min :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="r_min" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">month</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="r_min_RF" runat="server" />
					<span id="r_min_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td colspan="2">Resell Max :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="r_max" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">month</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="r_max_RF" runat="server" />
					<span id="r_max_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td colspan="2">Resell :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="r" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">%</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="r_RF" runat="server" />
					<span id="r_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td colspan="2">Resell 2 Max :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="r2_max" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">month</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="r2_max_RF" runat="server" />
					<span id="r2_max_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
            <tr>
				<td colspan="2">Resell :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="r2" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">%</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="r2_RF" runat="server" />
					<span id="r2_ERR" runat="server" class="text-validator" visible="false">Format</span>
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
