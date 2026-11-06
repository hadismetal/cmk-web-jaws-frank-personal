<%@ page language="C#" masterpagefile="~/MasterBasic.master" autoeventwireup="true" codefile="DefaultSetup.aspx.cs" inherits="DefaultSetup" %>

<asp:content contentplaceholderid="icon" runat="server">
	<i class="icon-gear icon-4x"></i>
</asp:content>

<asp:content contentplaceholderid="h1" runat="server">
	<asp:literal id="licensedto" runat="server" />
	<small>Application Setup</small>
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server">
		<div id="div_pass" runat="server" class="width-400">
			<div class="form-group">
				<label>
					Password :
				</label>
				<asp:requiredfieldvalidator id="pass_RF" runat="server" />
				<div class="input-group input-group-lg">
					<span class="input-group-addon"><i class="icon-lock"></i></span>
					<asp:textbox id="pass" runat="server" cssclass="form-control" textmode="Password" />
				</div>
			</div>
			<button id="go" runat="server" class="btn btn-primary btn-lg" onserverclick="go_Click">
				<i class="icon-ok"></i>
				Submit
			</button>
			<div id="gagal" runat="server">
				<p class="text-warning">
					<strong>Otorisasi Gagal</strong>
				</p>
				<p>
					<i class="icon-arrow-right icon-large"></i>
					Password Salah
				</p>
			</div>
		</div>
		<div id="div_action" runat="server" class="button-left">
			<p>
				<button id="btn_mt" runat="server" class="btn btn-primary" onserverclick="MT">
					<i class="icon-fixed-width icon-envelope"></i>
					Mail Test
				</button>
				Result : <b id="result_mt" runat="server" />
			</p>
			<p>
				<button id="btn_sm" runat="server" class="btn btn-primary" onserverclick="SM">
					<i class="icon-fixed-width icon-wrench"></i>
					Setup Module
				</button>
				Result : <b id="result_sm" runat="server" />
			</p>
			<p>
				<button id="btn_sa" runat="server" class="btn btn-primary" onserverclick="SA">
					<i class="icon-fixed-width icon-user"></i>
					Setup Administrator
				</button>
				Result : <b id="result_sa" runat="server" />
			</p>
		</div>
	</form>
	<div id="footer">
		<a href="/" class="btn btn-success">
			<i class="icon-arrow-left icon-large"></i>
			Halaman Sign-In
		</a>
	</div>
</asp:content>
