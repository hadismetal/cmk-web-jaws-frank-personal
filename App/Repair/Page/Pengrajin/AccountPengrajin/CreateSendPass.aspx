<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="CreateSendPass.aspx.cs" inherits="Page_CreateSendPass" %>

<%@ register src="MenuCreate.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Create New User
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Nama :</span><br />
		<span class="text-lg" id="header_nama" runat="server" />
	</p>
	<hr />
	<form runat="server">
		<p>
			<span class="text-muted">Generated Password :</span><br />
			<span class="text-xl" id="pass" runat="server" />
		</p>
		<div class="button-left">
			<p>
				<button id="btn_send" runat="server" class="btn btn-primary" onserverclick="sendPass">
					<i class="icon-fixed-width icon-envelope"></i>
					Send Password
				</button>
				Result : <b id="result" runat="server" />
			</p>
			<p>
				<a id="a_mailto" runat="server" class="btn btn-primary" style="width:200px;">
					<i class="icon-fixed-width icon-envelope"></i>
					Send Password, Manual
				</a>
				<b id="email" runat="server" class="text-info" />
			</p>
		</div>
		<p class="text-muted">
			<i>Setelah login, user wajib mengganti password.</i>
		</p>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
		</div>
	</form>
</asp:content>

