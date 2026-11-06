<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Product.aspx.cs" inherits="Page_Register_DJ_Form" %>

<%@ register src="Menu.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Packaging
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form runat="server">
		
		<table class="table-form">
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
				<td>No. PO</td>
				<td>:</td>
				<td>
					<asp:textbox id="nopo" runat="server" cssclass="form-control text-center" width="110" />
					
				</td>
				<td>
					<asp:requiredfieldvalidator id="nopo_RF" runat="server" />
				</td>
			</tr>
            <tr>
				<td>No. DO</td>
				<td>:</td>
				<td>
					<asp:textbox id="nodo" runat="server" cssclass="form-control text-center" width="110" />
					
				</td>
				<td>
					<asp:requiredfieldvalidator id="nodo_RF" runat="server" />
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
		</div>
	</form>
</asp:content>
