<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="2_Update.aspx.cs" inherits="Page_Account_2_Update" %>

<%@ register src="Menu2.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Colour Diamond
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	<asp:literal id="h1_st" runat="server" />
</asp:content>
<asp:content contentplaceholderid="h1_id" runat="server">
	ID:<asp:literal id="h1_id" runat="server" />
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form runat="server">
		<div class="form-group width-250">
			<label>
				Nama :
			</label>
			<asp:requiredfieldvalidator id="nama_RF" runat="server" />
			<span id="nama_ERR" runat="server" class="text-validator" visible="false">Sudah Digunakan</span>
			<asp:textbox id="nama" runat="server" cssclass="form-control text-bold" maxlength="50" />
		</div>
		<div class="form-group width-500">
			<label>
				Keterangan :
			</label>
			<asp:textbox id="keterangan" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
		</div>
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
		</table>
		<div class="form-group cb-single">
			<div id="div_draft" runat="server">
				<label for="content_disable">
					<strong class="text-draft">Status : Draft</strong>
					<small>Data tidak dimunculkan di dalam sistem</small>
				</label>
			</div>
			<div id="div_disable" runat="server">
				<asp:checkbox id="disable" runat="server" />
				<label for="content_disable">
					<strong class="text-warning">Status : Disable</strong>
					<small>Data tidak dimunculkan di dalam sistem</small>
				</label>
			</div>
		</div>
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-ok"></i>
				Save
			</button>
			<%--<asp:button id="del" runat="server" cssclass="btn btn-danger" text="Delete" width="80" onclick="del_Click" />--%>
			<span id="nodel" runat="server" class="text-validator" visible="false">Disable Delete : File Sudah Digunakan</span>
		</div>
	</form>
</asp:content>
