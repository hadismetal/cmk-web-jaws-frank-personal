<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Sales_Form.aspx.cs" inherits="Page_Register_Sales_Form" %>

<%@ register src="MenuSales.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content ID="Content1" contentplaceholderid="h1" runat="server">
	Souvenir
</asp:content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content ID="Content3" contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Location</span><br />
		<span class="text-bold" id="location" runat="server" />
	</p>
	<hr />
	<form id="Form1" runat="server">
		<div class="row">
			<div class="col-xs-12 col-sm-6">
				<table class="table-form">
					<tr>
						<td class="fixed-150">Customer
						</td>
						<td>:</td>
						<td>
							<div class="input-group">
								<div class="input-group-btn">
									<a href="Sales_DataCustomer.aspx" class="btn btn-info" id="search_customer" runat="server">
										<i class="icon-search"></i>
									</a>
								</div>
								<asp:textbox id="customer" Enabled="false" runat="server" cssclass="form-control" maxlength="50" width="200" />
							</div>
						</td>
					</tr>
					
					<tr>
						<td class="fixed-150">JPJR
						</td>
						<td>:</td>
						<td>
							<div class="input-group">
								<div class="input-group-btn">
									<a href="Sales_DataAgent.aspx" class="btn btn-info" id="search_agent" runat="server">
										<i class="icon-search"></i>
									</a>
								</div>
								<asp:textbox id="agent" Enabled="false" runat="server" cssclass="form-control" maxlength="50" width="200" />
							</div>
						</td>
					</tr>
					<tr>
						<td>Tanggal</td>
						<td>:</td>
						<td>
							<asp:textbox id="tgl" runat="server" cssclass="form-control text-center" width="110" />
							<script>
								$(document).ready(function () { $('#content_tgl').datepicker({ format: 'dd-mm-yyyy' }); });
							</script>
						</td>
						<td>
							<asp:requiredfieldvalidator id="tgl_RF" runat="server" />
							<span id="tgl_ERR" runat="server" class="text-validator" visible="false">Format</span>
						</td>
					</tr>
				</table>
			</div>
			<div class="col-xs-12 col-sm-6">
				<div class="form-group width-500">
					<label>
						Keterangan :
					</label>
					<asp:textbox id="keterangan" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
				</div>
				<div class="form-group width-300">
					<label>
						Event :
					</label>
					<asp:DropDownList ID="ddlevent" runat="server" CssClass="form-control"></asp:DropDownList>
				</div>
			</div>
		</div>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<a href="Sales.aspx" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:content>
