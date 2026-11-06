<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="UserUpdatePass.aspx.cs" inherits="Page_UserUpdatePass" %>

<%@ register src="MenuFile.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	User Account
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	<asp:literal id="h1_st" runat="server" />
</asp:content>
<asp:content contentplaceholderid="h1_id" runat="server">
	ID:<asp:literal id="h1_id" runat="server" />
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form runat="server">
		<div class="form-group width-300">
			<label>
				Password :
			</label>
			<asp:requiredfieldvalidator id="pass1_RF" controltovalidate="pass" runat="server" />
			<span id="pass1_CAPS" class="text-warning" style="display: none;">
				<i class="icon-warning-sign"></i>
				Caps-Lock
			</span>
			<asp:textbox id="pass1" runat="server" cssclass="form-control" textmode="Password" />
		</div>
		<div class="form-group width-300">
			<label>
				Confirm Password :
			</label>
			<asp:requiredfieldvalidator id="pass2_RF" controltovalidate="passconfirm" runat="server" />
			<span id="pass2_CAPS" class="text-warning" style="display: none;">
				<i class="icon-warning-sign"></i>
				Caps-Lock
			</span>
			<asp:textbox id="pass2" runat="server" cssclass="form-control" textmode="Password" />
		</div>
		<p class="text-muted">
			<i>Setelah login, user wajib mengganti password.</i>
		</p>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-ok"></i>
				Save
			</button>
			<asp:comparevalidator id="compare" runat="server" cssclass="text-validator"
				controltocompare="pass1" controltovalidate="pass2"
				text="Confirm Password : Tidak Sesuai" />
		</div>
	</form>
</asp:content>

