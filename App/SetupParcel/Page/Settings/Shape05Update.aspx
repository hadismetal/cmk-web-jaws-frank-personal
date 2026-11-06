<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="Shape05Update.aspx.cs" inherits="Page_Settings_Shape05Update" %>

<asp:content contentplaceholderid="h1" runat="server">
	Pearl
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Shape Control
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p>
		<span class="text-muted">Kode Parcel :</span><br />
		<span class="text-lg" id="header_nama" runat="server" />
	</p>
	<hr />
	<form runat="server">
		<div class="form-group cb-multi">
			<p>
				Shape :
			</p>
			<asp:radiobuttonlist id="roundshape" runat="server" />
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
