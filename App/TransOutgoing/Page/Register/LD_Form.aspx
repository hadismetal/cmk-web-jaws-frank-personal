<%@ page EnableEventValidation="false" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="LD_Form.aspx.cs" inherits="LD_Form" %>

<%@ register src="MenuLD.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Loose Diamond
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
    <table class="table-file">
		<tr>
			<td class="text-label">Market Brand</td>
			<td>:</td>
			<td id="brand" runat="server" class="text-bold" />
		</tr>
		<tr>
			<td class="text-label">From <asp:Literal runat="server" ID="lit_type_from" /></td>
			<td>:</td>
			<td id="from" runat="server" class="text-bold" />
		</tr>
        <tr>
			<td class="text-label">To <asp:Literal runat="server" ID="lit_type_to" /></td>
			<td>:</td>
			<td id="to" runat="server" class="text-bold" />
		</tr>
	</table>
    <hr />
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
		</table>
		<div class="form-group width-300">
			<label>
				Nama Kurir :
			</label>
			<asp:requiredfieldvalidator id="namakurir_RF" runat="server" />
			<asp:textbox id="namakurir" runat="server" cssclass="form-control text-bold" maxlength="50" />
		</div>
		<div class="form-group width-500">
			<label>
				Keterangan :
			</label>
			<asp:textbox id="keterangan" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
		</div>
		<table class="table-form">
			<tr>
				<td>Tanggal ETA</td>
				<td>:</td>
				<td>
					<asp:textbox id="tglETA" runat="server" cssclass="form-control text-center" width="110" />
					<script>
					    $(document).ready(function () { $('#content_tglETA').datepicker({ format: 'dd-mm-yyyy' }); });
					</script>
				</td>
				<td>
					<asp:requiredfieldvalidator id="tglETA_RF" runat="server" />
					<span id="tglETA_ERR" runat="server" class="text-validator" visible="false">Format</span>
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

