<%@ page language="C#" masterpagefile="~/MasterBasic.master" autoeventwireup="true" codefile="AccessDenied.aspx.cs" inherits="AccessDenied" %>

<asp:content contentplaceholderid="icon" runat="server">
	<i class="icon-ban-circle icon-4x"></i>
</asp:content>

<asp:content contentplaceholderid="h1" runat="server">
	Access Denied
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p>
		Halaman ini tidak bisa dibuka karena hak akses Anda tidak memadai.
	</p>
	<p>
		Silakan menghubungi atasan Anda untuk proses otorisasi.
	</p>
	<div id="footer">
		<a href="javascript:closeBack();" class="btn btn-success">
			<i class="icon-arrow-left icon-large"></i>
			Halaman Sebelumnya
		</a>
	</div>
</asp:content>

