<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="SPK_LDKP.aspx.cs" inherits="Page_Register_DJ_Form" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewelry
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register LDKP
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<p>
		<span class="text-muted">No. Ref :</span><br />
		<span class="text-bold" id="noref" runat="server" />
	</p>
	<hr />
	<form runat="server">
		
		
		<table class="table-form">
            <tr>
				<td>No. LDKP</td>
				<td>:</td>
				<td>
					<asp:textbox id="ldkp" runat="server" cssclass="form-control text-center" width="200" />
					
				</td>
				<td>
				</td>
			</tr>
            <tr>
				<td>Tanggal LDKP</td>
				<td>:</td>
				<td>
					<asp:textbox id="tglldkp" runat="server" cssclass="form-control text-center" width="200" />
					<script>
					    $(document).ready(function () { $('#<%=tglldkp.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
					</script>
				</td>
				<td>
					<asp:requiredfieldvalidator id="tglldkp_RF" runat="server" />
					<span id="tglldkp_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
		</table>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Save
			</button>
			<a href="DJ.aspx" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:content>
