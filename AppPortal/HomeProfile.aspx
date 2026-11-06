<%@ page language="C#" masterpagefile="~/MasterBasic.master" autoeventwireup="true" codefile="HomeProfile.aspx.cs" inherits="HomeProfile" %>

<asp:content contentplaceholderid="icon" runat="server">
	<i class="icon-user icon-4x"></i>
</asp:content>

<asp:content contentplaceholderid="h1" runat="server">
	<asp:literal id="licensedto" runat="server" />
	<small>Profile</small>
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p>
		<span class="text-muted">User Account :</span><br />
		<span class="text-lg" id="nama" runat="server" />
	</p>
	<table class="table-file">
		<tr>
            <td>
			    <span class="text-muted">Email :</span><br />
			    <span id="email" runat="server" />
            </td>
		</tr>
		<tr>
            <td>
			    <span class="text-muted">Last Login :</span><br />
			    <span id="lastlogin" runat="server" />
            </td>
		</tr>
		<tr>
            <td>
			    <span class="text-muted">Update Password :</span><br />
			    <span id="recyclelast" runat="server" />
            </td>
		</tr>
	</table>
	<hr />
	<p class="text-red" id="strong" runat="server">
		Password Anda berkualitas kurang baik, dan sebaiknya diganti.
	</p>
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
		<div class="button-inline-noborder">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-ok"></i>
				Save
			</button>
			<asp:comparevalidator id="compare" runat="server" cssclass="text-validator"
				controltocompare="pass1" controltovalidate="pass2"
				text="Confirm Password : Tidak Sesuai" />
		</div>
	</form>
	<div id="footer">
		<a href="/Home.aspx" class="btn btn-success">
			<i class="icon-arrow-left icon-large"></i>
			Halaman Homepage
		</a>
	</div>
</asp:content>
