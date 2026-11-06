<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="RepairInvoice_Form.aspx.cs" inherits="Page_Register_Repair_Form" %>

<%@ register src="MenuRepairInvoice.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Repair Invoice
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted"><asp:Literal runat="server" ID="lit_tipelocation" /> :</span><br />
		<span class="text-bold" id="locationname" runat="server" />
	</p>
	<hr />
	<form runat="server">
		<table class="table-form">
            <tr>
				<td class="fixed-150">No. Dokumen Repair</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="RepairInvoice_DataRepair.aspx" class="btn btn-info" id="search_doc" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="doc" Enabled="false" runat="server" cssclass="form-control" maxlength="50" width="200" />
					</div>
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
			<a href="RepairInvoice.aspx" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:content>
