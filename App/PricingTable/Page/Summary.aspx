<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Summary.aspx.cs" inherits="Page_Summary" %>

<asp:content contentplaceholderid="h1" runat="server">
	Summary
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<div id="div_setup" runat="server" class="button-left">
		<p>
			<a class="btn btn-primary" href="javascript:goSetup()">
				<i class="icon-fixed-width icon-wrench"></i>
				Setup Database
			</a>
		</p>
		<hr />
	</div>
	<script type="text/javascript">
		function goSetup() {
			txtConfirm =
				"Lanjutkan Proses SETUP ?\n\n" +
				"Apabila proses memakan waktu cukup lama :\n" +
				"Harap layar browser jangan ditutup sampai proses selesai.";

			if (confirm(txtConfirm))
				location.href = 'Setup.aspx';
		}
	</script>
	<div class="table-responsive">
		<asp:table id="tb1" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="200">Exchange</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-center">Normal</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-center">Pending Update</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-center">Pending Approval</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<div class="table-responsive">
		<asp:table id="tb2" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="200">Service Fee</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-center">Normal</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-center">Pending Update</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-center">Pending Approval</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
	<div class="table-responsive">
		<asp:table id="tb3" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="200">System</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-center">Normal</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-center">Pending Update</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-center">Pending Approval</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

