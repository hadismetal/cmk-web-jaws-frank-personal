<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="PointRewardGJ.aspx.cs" inherits="Page_StatusLog_FF" %>

<asp:content contentplaceholderid="h1" runat="server">
	Point Reward Gold Jewellery
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Detail Log
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Periode :
			</div>
			<asp:dropdownlist id="bulan" runat="server" cssclass="form-control" width="150" />
			<asp:dropdownlist id="tahun" runat="server" cssclass="form-control" width="100" />
		</div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
			</div>
		</div>
	</form>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-report text-sm">
			<asp:tableheaderrow>
				<asp:tableheadercell width="60" />
                <asp:tableheadercell width="130">Tgl</asp:tableheadercell>
                <asp:tableheadercell width="130">Product Category</asp:tableheadercell>
				<asp:tableheadercell width="100" cssclass="text-right">Rupiah Baru</asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Rupiah Lama</asp:tableheadercell>
                <asp:tableheadercell width="100" cssclass="text-right">Operator</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>
