<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="DJ_StoneUpdate.aspx.cs" inherits="Page_Register_DJ_StoneUpdate" %>

<%@ register src="MenuDJ.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content ID="Content1" contentplaceholderid="h1" runat="server">
	Diamond Jewelry
</asp:content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content ID="Content3" contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Nomor :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<hr />
	<form id="Form1" runat="server">
		<div class="row">
			<div class="col-md-3">
				<p>
					<span class="text-bold" id="stone_index" runat="server" />
					<br />
					<span class="text-lg text-warning" id="stone_nama" runat="server" />
				</p>
			</div>
			<div class="col-md-3">
				<p id="p_sizecontrol" runat="server" class="text-muted">
					Size Control :<br />
					<span id="sizecontrol" runat="server" class="text-lg text-success" />
					<small class="text-silver">Carat</small>
				</p>
			</div>
		</div>
		<br />
		<table class="table-form">
			<tr>
				<td colspan="2">Total Butir :</td>
				<td colspan="2">Total Carat :</td>
			</tr>
			<tr>
				<td>
					<asp:textbox id="totalbutir" runat="server" cssclass="form-control text-lg text-center" width="100" />
				</td>
				<td>
					<asp:requiredfieldvalidator id="totalbutir_RF" runat="server" />
					<span id="totalbutir_ERR" runat="server" class="text-validator" visible="false">Format</span>
					<span id="totalbutir_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
				</td>
				<td>
					<div class="input-group">
						<asp:textbox id="totalcarat" runat="server" cssclass="form-control text-lg text-right" width="120" />
						<span class="input-group-addon">Carat</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="totalcarat_RF" runat="server" />
					<span id="totalcarat_ERR" runat="server" class="text-validator" visible="false">Format</span>
					<span id="totalcarat_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
				</td>
			</tr>
		</table>
		<br />
		<table class="table-form">
			<tr>
				<td colspan="2">Panjang :</td>
				<td colspan="2">Lebar :</td>
				<td colspan="2">Tinggi :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="dimensip" runat="server" cssclass="form-control text-lg text-center" width="125" />
						<span class="input-group-addon">MM</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="dimensip_RF" runat="server" />
					<span id="dimensip_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
				<td>
					<div class="input-group">
						<asp:textbox id="dimensil" runat="server" cssclass="form-control text-lg text-center" width="125" />
						<span class="input-group-addon">MM</span>
					</div>
				</td>
				<td>
					<span id="dimensil_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
				<td>
					<div class="input-group">
						<asp:textbox id="dimensit" runat="server" cssclass="form-control text-lg text-center" width="125" />
						<span class="input-group-addon">MM</span>
					</div>
				</td>
				<td>
					<span id="dimensit_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
		</table>
		<div class="form-group width-300" id="div_gia" runat="server" visible="false">
			<br />
			<label>
				No. GIA :
			</label>
			<asp:textbox id="gia" runat="server" cssclass="form-control" maxlength="50" />
		</div>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<a id="cancel" runat="server" class="btn btn-default" href="DJ_Stone.aspx">
				<i class="icon-remove"></i>
				Cancel
			</a>
			<span id="caratbutir_ERR" runat="server" class="text-validator" visible="false">Carat / Butir : Tidak Sesuai</span>
		</div>
	</form>
</asp:content>

