<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Resell_Form.aspx.cs" inherits="Page_Register_Resell_Form" %>

<%@ register src="MenuResell.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Resell
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Outlet :</span><br />
		<span class="text-bold" id="warehouse" runat="server" />
	</p>
	<hr />
	<form runat="server">
		
		<table class="table-form">
            <tr>
				<td class="fixed-150">Customer
				</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="Resell_DataCustomer.aspx" class="btn btn-info" id="search_customer" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="customer" Enabled="false" runat="server" cssclass="form-control" maxlength="50" width="200" />
					</div>
				</td>
				<asp:Panel runat="server" id="divtypetransaction">
					<td class="fixed-150">Tipe Transaksi
					</td>
					<td>:</td>
					<td>
						<asp:DropDownList ID="typetransaction" runat="server" CssClass="form-control" OnSelectedIndexChanged="changetypetransaction_Change" AutoPostBack="true">
							<asp:ListItem Value="1">Resell</asp:ListItem>
							<asp:ListItem Value="2">Gold Tunai</asp:ListItem>
						</asp:DropDownList>
					</td>
				</asp:Panel>
			</tr>
            <tr>
				<td class="fixed-150">JPJR
				</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="Resell_DataAgent.aspx" class="btn btn-info" id="search_agent" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="agent" Enabled="false" runat="server" cssclass="form-control" maxlength="50" width="200" />
					</div>
				</td>
			</tr>
			<tr>
				<td class="fixed-150">Kode Customer Lama</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<button class="btn btn-info" id="search_oldcustumercode" runat="server" onserverclick="search_oldcustumercode_ServerClick">
								<i class="icon-search"></i>
							</button>
						</div>
						<asp:TextBox ID="txtoldcustomercode" runat="server" CssClass="form-control" MaxLength="50" Width="200"/>
					</div>
				</td>
				<td>
					<asp:TextBox ID="txtnameoldcustomercode" runat="server" CssClass="form-control" MaxLength="50" Width="200" ReadOnly="true"/>
					<asp:Label ID="lblcheckoldcustumer" runat="server" Visible="false" style="color:red"></asp:Label>
				</td>
			</tr>
			<tr>
				<td>Tanggal</td>
				<td>:</td>
				<td>
					<asp:textbox id="tgl" runat="server" Enabled="false" cssclass="form-control text-center" width="110" />
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
        <div class="form-group width-500">
			<label>
				Keterangan :
			</label>
			<asp:textbox id="keterangan" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
		</div>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<a href="Resell.aspx" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:content>
