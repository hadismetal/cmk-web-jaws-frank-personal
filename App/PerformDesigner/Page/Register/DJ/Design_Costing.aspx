<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Design_Costing.aspx.cs" inherits="Page_Register_Design_Costing" %>

<%@ register src="MenuDesign.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewellery
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Nomor :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<hr />
	<form runat="server">
		<table class="table-form">
			<tr>
				<td>Frame</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="frame" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="frame_RF" runat="server" />
					<span id="frame_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
			<tr>
				<td>Mounting</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="mounting" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="mounting_RF" runat="server" />
					<span id="mounting_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
			<tr>
				<td>Setting</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="setting" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="setting_RF" runat="server" />
					<span id="setting_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
			<tr>
				<td>Finishing</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="finishing" runat="server" cssclass="form-control text-lg text-right" width="175" />
						<span class="input-group-addon">IDR</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="finishing_RF" runat="server" />
					<span id="finishing_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
		</table>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
		</div>
	</form>
</asp:content>

