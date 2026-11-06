<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="MULD.aspx.cs" inherits="Page_UpdateProcess_MU" %>

<asp:content contentplaceholderid="h1" runat="server">
	Resell Loose Diamond Mark-Up System
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update Process
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
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
