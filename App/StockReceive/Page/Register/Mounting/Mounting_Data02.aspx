<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Mounting_Data02.aspx.cs" inherits="Page_Register_Mounting_Data02" %>

<%@ register src="MenuMounting.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Mounting (WIP)
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Nomor :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<hr />
	<form runat="server">
		<table class="table-form">
			<tr>
				<td class="fixed-120">Designer</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="Mounting_DataDesigner.aspx" class="btn btn-info" id="search_designer" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="humandesignernama" runat="server" cssclass="form-control" maxlength="50" width="300" />
					</div>
				</td>
			</tr>
			<tr>
				<td class="fixed-120">Stock Keeper</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="Mounting_DataStock.aspx" class="btn btn-info" id="search_stock" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="humanstocknama" runat="server" cssclass="form-control" maxlength="50" width="300" />
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="humanstocknama_RF" runat="server" />
				</td>
			</tr>
		</table>
		<br />
		<table class="table-form">
			<tr>
				<td class="fixed-120">Expired Product</td>
				<td>:</td>
				<td>
					<asp:textbox id="expiredproduct" runat="server" cssclass="form-control text-center" width="110" />
					<script>
						$(document).ready(function () { $('#content_expiredproduct').datepicker({ format: 'dd-mm-yyyy' }); });
					</script>
				</td>
				<td>
					<asp:requiredfieldvalidator id="expiredproduct_RF" runat="server" />
					<span id="expiredproduct_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
			<tr>
				<td class="fixed-120">Expired Request</td>
				<td>:</td>
				<td>
					<asp:textbox id="expiredrequest" runat="server" cssclass="form-control text-center" width="110" />
					<script>
						$(document).ready(function () { $('#content_expiredrequest').datepicker({ format: 'dd-mm-yyyy' }); });
					</script>
				</td>
				<td>
					<span id="expiredrequest_ERR" runat="server" class="text-validator" visible="false">Format</span>
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

