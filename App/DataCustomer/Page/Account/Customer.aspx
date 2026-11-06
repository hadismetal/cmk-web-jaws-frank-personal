<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Customer.aspx.cs" inherits="Page_Account_Customer" %>

<asp:content contentplaceholderid="h1" runat="server">
	Customer
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Account
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Search (Kode / Nama Customer) :</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
		</div>
		<div class="form-group">
			<div class="text-muted">
				Diurutkan :
			</div>
			<asp:dropdownlist id="order" runat="server" cssclass="form-control" width="170">
				<asp:listitem value="Nama">Nama</asp:listitem>
				<asp:listitem value="NoCustomer">Nomor Customer</asp:listitem>
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
    <div>
        <br />
        <asp:Literal runat="server" ID="lit_countcustomer" />
    </div>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="86" />
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="300">Nama</asp:tableheadercell>
				<asp:tableheadercell width="90">Tgl.</asp:tableheadercell>
				<asp:tableheadercell width="100">Status</asp:tableheadercell>
				<asp:tableheadercell width="100">Merge</asp:tableheadercell>
				
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

