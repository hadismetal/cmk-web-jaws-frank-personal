<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="PG_Form.aspx.cs" inherits="Page_Register_PG_Form" %>

<%@ register src="MenuPG.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Plain Gold
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Warehouse :</span><br />
		<span class="text-bold" id="warehouse" runat="server" />
	</p>
	<hr />
	<form id="Form1" runat="server">
		<table class="table-form">
			<tr>
				<td class="fixed-150">No. Dokumen
				</td>
				<td>:</td>
				<td>
                    <asp:textbox id="nodokumen" runat="server" cssclass="form-control" maxlength="50" width="200" />
				</td>
			</tr>
		</table>
		<table class="table-form">
			<tr>
				<td class="fixed-150">Supplier</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="PG_DataSupplier.aspx" class="btn btn-info" id="search_supplier" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="suppliernama" runat="server" cssclass="form-control" maxlength="50" width="300" />
					</div>
				</td>
			</tr>
			<tr>
				<td class="fixed-150">No. Supplier
				</td>
				<td>:</td>
				<td>
					<asp:textbox id="suppliernomor" runat="server" cssclass="form-control" maxlength="50" width="200" />
				</td>
			</tr>
		</table>
        <table class="table-form">
			<tr>
				<td class="fixed-150">Pengirim
				</td>
				<td>:</td>
				<td>
                    <asp:textbox id="pengirim" runat="server" cssclass="form-control" maxlength="50" width="200" />
				</td>
			</tr>
		</table>
        <table class="table-form">
			<tr>
				<td class="fixed-150">Keterangan
				</td>
				<td>:</td>
				<td>
                    <asp:textbox id="keterangan" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
				</td>
			</tr>
		</table>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
		</div>
	</form>
</asp:content>
