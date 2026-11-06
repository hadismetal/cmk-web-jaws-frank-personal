<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="ProductLevelUpdate.aspx.cs" inherits="Page_Settings_ProductLevelUpdate" %>

<asp:content contentplaceholderid="h1" runat="server">
	Product Level
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Settings
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p>
		<span class="text-muted">Product Level :</span><br />
		<span class="text-bold" id="header_nama" runat="server" />
	</p>
	<hr />
	<form runat="server">
		<div class="form-group cb-multi">
			<p>
				Grup :
			</p>
			<asp:radiobuttonlist id="grup" runat="server">
				<asp:listitem>DJ - Diamond Jewelry</asp:listitem>
				<asp:listitem>PG - Plain Gold</asp:listitem>
				<asp:listitem>LD - Loose Diamond</asp:listitem>
			</asp:radiobuttonlist>
		</div>
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
