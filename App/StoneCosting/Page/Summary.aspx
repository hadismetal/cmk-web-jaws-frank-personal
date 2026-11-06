<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Summary.aspx.cs" inherits="Page_Summary" %>

<asp:content contentplaceholderid="h1" runat="server">
	Summary
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <form runat="server">
	<div id="div_setup" runat="server" class="button-left">
		<p>
			<a class="btn btn-primary" href="javascript:goSetup()">
				<i class="icon-fixed-width icon-wrench"></i>
				Setup Database
			</a>
            &nbsp;
            <button id="go" runat="server" class="btn btn-primary" onserverclick="Sync">
                <i class="icon-fixed-width icon-wrench"></i>
                Sync All
            </button>
            &nbsp;
            <button id="Button1" runat="server" class="btn btn-primary" onserverclick="SyncFrank">
                <i class="icon-fixed-width icon-wrench"></i>
                Sync Frank n Co
            </button>
            &nbsp;
            <button id="Button2" runat="server" class="btn btn-primary" onserverclick="SyncPalace">
                <i class="icon-fixed-width icon-wrench"></i>
                Sync Palace
            </button>
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
        </form>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="200">Stone</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-center">Normal</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-center">Pending Update</asp:tableheadercell>
				<asp:tableheadercell width="150" cssclass="text-center">Pending Approval</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

