<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="BudgetStock.aspx.cs" inherits="Page_Update_MU" %>

<asp:content contentplaceholderid="h1" runat="server">
	Budget Stock
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Product Item :
			</div>
			<asp:dropdownlist id="item" runat="server" cssclass="form-control" width="200">
				<asp:listitem>Semua</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<div class="text-muted">
				Product Level :
			</div>
			<asp:dropdownlist id="level" runat="server" cssclass="form-control" width="200">
				<asp:listitem>Semua</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<div class="text-muted">
				Stone Distribution :
			</div>
			<asp:dropdownlist id="dist" runat="server" cssclass="form-control" width="200">
				<asp:listitem>Semua</asp:listitem>
			</asp:dropdownlist>
		</div>
        <div class="form-group">
			<div class="text-muted">
				Segmen :
			</div>
			<asp:dropdownlist id="segmen" runat="server" cssclass="form-control" width="200">
				<asp:listitem>Semua</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="clearfix">
			<br class="hidden-xs" />
		</div>
		<div class="form-group">
			<div class="text-muted">
				Status :
			</div>
			<asp:dropdownlist id="status" runat="server" cssclass="form-control" width="175">
				<asp:listitem>Semua</asp:listitem>
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
				<asp:tableheadercell width="250">Product Item</asp:tableheadercell>
				<asp:tableheadercell width="175">Operator</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right">Target Qty</asp:tableheadercell>
                <asp:tableheadercell width="130" cssclass="text-right">Qty Jual</asp:tableheadercell>
				<asp:tableheadercell width="130" cssclass="text-right">Target Omset</asp:tableheadercell>
                <asp:tableheadercell width="130" cssclass="text-right">Omset</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

