<%@ page language="C#" masterpagefile="~/MasterBasic.master" autoeventwireup="true" codefile="DataNull.aspx.cs" inherits="DataNull" %>

<asp:content contentplaceholderid="icon" runat="server">
	<i class="icon-question icon-4x"></i>
</asp:content>

<asp:content contentplaceholderid="h1" runat="server">
	Not Found
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p>
		Sistem tidak dapat menemukan data / transaksi sesuai dengan kriteria yang ditentukan.
	</p>
	<p>
		Kemungkinan sebab :
	</p>
	<ul>
		<li>Data / transaksi tersebut sudah dihapus oleh pengguna sistem yang lain.</li>
		<li>Proses tertentu sudah dilakukan yang menyebabkan perubahan status data / transaksi tersebut.</li>
	</ul>
	<div id="footer">
		<a href="javascript:closeBack();" class="btn btn-success">
			<i class="icon-arrow-left icon-large"></i>
			Halaman Sebelumnya
		</a>
	</div>
</asp:content>

