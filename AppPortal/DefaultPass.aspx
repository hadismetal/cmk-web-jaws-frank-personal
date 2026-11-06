<%@ page language="C#" masterpagefile="~/MasterBasic.master" autoeventwireup="true" codefile="DefaultPass.aspx.cs" inherits="DefaultPass" %>

<asp:content contentplaceholderid="icon" runat="server">
	<i class="icon-lock icon-4x"></i>
</asp:content>

<asp:content contentplaceholderid="h1" runat="server">
	<asp:literal id="licensedto" runat="server" />
	<small>Reset Password</small>
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server">
		<div id="div_1" runat="server" class="width-400">
			<div class="form-group">
				<label>
					Alamat Email :
				</label>
				<asp:requiredfieldvalidator id="email_RF" runat="server" />
				<div class="input-group input-group-lg">
					<span class="input-group-addon"><i class="icon-user"></i></span>
					<asp:textbox id="email" runat="server" cssclass="form-control" />
				</div>
			</div>
			<button id="go" runat="server" class="btn btn-primary btn-lg" onserverclick="sendKey">
				<i class="icon-ok"></i>
				Send
			</button>
			<div id="gagal" runat="server">
				<p class="text-warning">
					<strong>Otorisasi Gagal</strong>
				</p>
				<p>
					<i class="icon-arrow-right icon-large"></i>
					Alamat Email Tidak Terdaftar
				</p>
			</div>
			<asp:literal id="userid" runat="server" visible="false" />
		</div>
		<div id="div_2" runat="server" visible="false">
			<p>
				Sebuah kode rahasia sudah dikirimkan ke mailbox : <span id="myMail" runat="server" class="text-bold text-success" />
				<br />
				Silakan masukkan kode tersebut di bagian bawah untuk melanjutkan proses.
			</p>
			<hr />
			<div class="form-group width-200">
				<label>
					Kode Rahasia :
				</label>
				<asp:requiredfieldvalidator id="resetkey_RF" controltovalidate="pass" runat="server" />
				<asp:textbox id="resetkey" runat="server" cssclass="form-control text-lg" />
			</div>
			<div class="button-inline-noborder">
				<button id="submit" runat="server" class="btn btn-primary" onserverclick="validKey">
					<i class="icon-ok"></i>
					Submit
				</button>
				<span id="keyerror" runat="server" visible="false" class="text-validator">Kode Rahasia Tidak Sesuai</span>
			</div>
		</div>
		<div id="div_3" runat="server" visible="false">
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
		</div>
	</form>
	<div id="footer">
		<a href="/" class="btn btn-success">
			<i class="icon-arrow-left icon-large"></i>
			Halaman Sign-In
		</a>
	</div>
</asp:content>
