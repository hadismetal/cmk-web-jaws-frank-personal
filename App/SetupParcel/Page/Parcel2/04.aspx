<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="04.aspx.cs" inherits="Page_Parcel2_04" %>

<asp:content contentplaceholderid="h1" runat="server">
	Colour Diamond / Colour Grading
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<a href="04Create.aspx" class="btn btn-success" target="_blank" runat="server" id="create">
		<i class="icon-plus"></i>
		Create New
	</a>
	<hr />
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Status :
			</div>
			<asp:dropdownlist id="disable" runat="server" cssclass="form-control" width="120">
				<asp:listitem>Semua</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<div class="text-muted">
				Diurutkan :
			</div>
			<asp:dropdownlist id="order" runat="server" cssclass="form-control" width="150">
				<asp:listitem value="SN">SN</asp:listitem>
				<asp:listitem value="Nama">Nama</asp:listitem>
				<asp:listitem value="NamaKode">Kode</asp:listitem>
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
				<asp:tableheadercell width="100" />
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="250">Nama</asp:tableheadercell>
				<asp:tableheadercell width="60">Status</asp:tableheadercell>
				<asp:tableheadercell width="60" cssclass="text-center">SN</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

