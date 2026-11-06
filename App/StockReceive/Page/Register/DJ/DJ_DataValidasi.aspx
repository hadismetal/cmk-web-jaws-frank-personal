<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DJ_DataValidasi.aspx.cs" inherits="Page_Register_DJ_DataValidasi" %>

<%@ register src="MenuDJ.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewelry
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<asp:literal id="idbrand" runat="server" visible="false" />
	<hr />
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Search (No. Validasi / Kode Barang) :
			</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
		</div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
				<a id="cancel" runat="server" class="btn btn-default" href="DJ_Data01.aspx">
					<i class="icon-remove"></i>
					Cancel
				</a>
			</div>
		</div>
        <div>
            <asp:Literal runat="server" ID="lit_data" />
        </div>
		<div class="table-responsive">
			<asp:table id="tb" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="50" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
					<asp:tableheadercell width="150">No. Validasi</asp:tableheadercell>
					<asp:tableheadercell width="150">Kode Barang</asp:tableheadercell>
                    <asp:tableheadercell width="150">Kode Bagian</asp:tableheadercell>
					<asp:tableheadercell width="150">No. DO</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
</asp:content>

