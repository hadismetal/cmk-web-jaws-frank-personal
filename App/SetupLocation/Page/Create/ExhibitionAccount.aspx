<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="ExhibitionAccount.aspx.cs" inherits="Page_Create_ExhibitionAccount" %>

<%@ register src="MenuExhibition.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Exhibition
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<table class="table-file">
		<tr>
			<td class="text-label">Group</td>
			<td>:</td>
			<td id="group" runat="server" class="text-bold" />
		</tr>
		<tr>
			<td class="text-label">Market Brand</td>
			<td>:</td>
			<td id="brand" runat="server" class="text-bold" />
		</tr>
	</table>
	<hr />
	<form runat="server">
		<div class="form-group width-300">
			<label>
				Nama :
			</label>
			<asp:requiredfieldvalidator id="nama_RF" runat="server" />
			<asp:textbox id="nama" runat="server" cssclass="form-control text-bold" maxlength="50" />
		</div>
        <div class="form-group width-300">
			<label>
				Kode :
			</label>
			<asp:requiredfieldvalidator id="kode_RF" runat="server" />
			<asp:textbox id="kode" runat="server" cssclass="form-control text-bold" maxlength="3" />
            <span id="kode_ERR" runat="server" class="text-validator" visible="false">Format</span>
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
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<a href="Exhibition.aspx" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
		</div>
	</form>
</asp:content>

