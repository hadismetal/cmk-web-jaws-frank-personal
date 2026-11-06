<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Repair_Form.aspx.cs" inherits="Page_Register_Repair_Form" %>

<%@ register src="MenuRepair.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Repair Order
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Location</span><br />
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
							<a href="Repair_DataCustomer.aspx" class="btn btn-info" id="search_customer" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="customer" Enabled="false" runat="server" cssclass="form-control" maxlength="50" width="250" />
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
							<a href="Repair_DataAgent.aspx" class="btn btn-info" id="search_agent" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="agent" Enabled="false" runat="server" cssclass="form-control" maxlength="50" width="250" />
					</div>
				</td>
			</tr>
            <tr>
				<td class="fixed-150">PLU
				</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="Repair_ItemAdd.aspx" class="btn btn-info" id="search_plu" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="plu" Enabled="false" runat="server" cssclass="form-control" maxlength="50" width="250" />
					</div>
				</td>
			</tr>
            <tr>
				<td class="fixed-150">QC
				</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="Repair_QCAdd.aspx" class="btn btn-info" id="search_qc" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="qc" Enabled="false" runat="server" cssclass="form-control" maxlength="50" width="250" />
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
            <tr>
				<td>Tanggal Selesai</td>
				<td>:</td>
				<td>
					<asp:textbox id="tglselesai" runat="server" cssclass="form-control text-center" width="110" />
					<script>
					    $(document).ready(function () { $('#<%=tglselesai.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
					</script>
				</td>
				<td>
					<asp:requiredfieldvalidator id="tglselesai_RF" runat="server" />
					<span id="tglselesai_ERR" runat="server" class="text-validator" visible="false">Format</span>
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
			<a href="Repair.aspx" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:content>
