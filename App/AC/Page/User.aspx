<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="User.aspx.cs" inherits="Page_User" %>

<asp:content contentplaceholderid="h1" runat="server">
	User Account
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Search :
			</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
		</div>
		<div class="form-group">
			<div class="text-muted">
				Login Status :
			</div>
			<asp:dropdownlist id="login" runat="server" cssclass="form-control" width="180">
				<asp:listitem>Semua</asp:listitem>
				<asp:listitem>Blokir Account</asp:listitem>
				<asp:listitem>Tidak Pernah Login</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<div class="text-muted">
				Password :
			</div>
			<asp:dropdownlist id="pass" runat="server" cssclass="form-control" width="150">
				<asp:listitem>Semua</asp:listitem>
				<asp:listitem>Wajib Ganti</asp:listitem>
				<asp:listitem>Baik</asp:listitem>
				<asp:listitem>Kurang Baik</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="clearfix">
			<br class="hidden-xs" />
		</div>
		<div class="form-group">
			<div class="text-muted">
				Status :
			</div>
			<asp:dropdownlist id="status" runat="server" cssclass="form-control" width="120">
				<asp:listitem>Semua</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<div class="text-muted">
				Diurutkan :
			</div>
			<asp:dropdownlist id="order" runat="server" cssclass="form-control" width="150">
				<asp:listitem value="Nama">Nama</asp:listitem>
				<asp:listitem value="Email">Alamat Email</asp:listitem>
				<asp:listitem value="SMS">SMS</asp:listitem>
				<asp:listitem value="LastLogin">Last Login</asp:listitem>
				<asp:listitem value="ID">ID</asp:listitem>
			</asp:dropdownlist>
			<div class="text-00 visible-xs">&nbsp;</div>
			<asp:dropdownlist id="orderby" runat="server" cssclass="form-control" width="100">
				<asp:listitem>ASC</asp:listitem>
				<asp:listitem>DESC</asp:listitem>
			</asp:dropdownlist>
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
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="180" />
				<asp:tableheadercell width="50">No.</asp:tableheadercell>
				<asp:tableheadercell width="250">Nama / NIK</asp:tableheadercell>
				<asp:tableheadercell width="350">Email / Keterangan</asp:tableheadercell>
				<asp:tableheadercell width="120">Status</asp:tableheadercell>
				<asp:tableheadercell width="120">Password</asp:tableheadercell>
				<asp:tableheadercell width="150">Last Login</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

