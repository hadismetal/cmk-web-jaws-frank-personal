<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Design_CharProduct02.aspx.cs" inherits="Page_Register_Design_CharProduct02" %>

<%@ register src="MenuDesign.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Jewelry
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Nomor :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<hr />
	<form runat="server">
		<table class="table-form">
			<tr>
				<td colspan="2">Gross Weight :</td>
			</tr>
			<tr>
				<td>
					<div class="input-group">
						<asp:textbox id="grossweight" runat="server" cssclass="form-control text-lg text-center" width="120" />
						<span class="input-group-addon">Gram</span>
					</div>
				</td>
				<td>
					<span id="grossweight_ERR" runat="server" class="text-validator" visible="false">Format</span>
					<span id="grossweight_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
					<asp:requiredfieldvalidator id="grossweight_RF" runat="server" />
				</td>
			</tr>
		</table>
		<table class="table-form">
			<tr>
				<td>Kadar Logam</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="kadarlogam" runat="server" cssclass="form-control text-lg text-center" width="110" />
						<span class="input-group-addon">%</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="kadarlogam_RF" runat="server" />
					<span id="kadarlogam_ERR" runat="server" class="text-validator" visible="false">Format</span>
					<span id="kadarlogam_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
				</td>
			</tr>
			<tr>
				<td>Kadar Tukaran</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="kadartukaran" runat="server" cssclass="form-control text-lg text-center" width="110" />
						<span class="input-group-addon">%</span>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="kadartukaran_RF" runat="server" />
					<span id="kadartukaran_ERR" runat="server" class="text-validator" visible="false">Format</span>
					<span id="kadartukaran_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
				</td>
			</tr>
		</table>
		<hr />
		<table class="table-form">
			<tr>
				<td>Diameter</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="dimensid" runat="server" Text="0" cssclass="form-control text-lg text-center" width="125" />
						<%--span class="input-group-addon">MM</--%>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="dimensid_RF" runat="server" />
					<span id="dimensid_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
			<tr>
				<td>Panjang</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="dimensip" runat="server" Text="0" cssclass="form-control text-lg text-center" width="125" />
						<%--span class="input-group-addon">MM</--%>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="dimensip_RF" runat="server" />
					<span id="dimensip_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
			<tr>
				<td>Lebar</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="dimensil" runat="server" Text="0" cssclass="form-control text-lg text-center" width="125" />
						<%--span class="input-group-addon">MM</--%>
					</div>
				</td>
				<td>
					<asp:requiredfieldvalidator id="dimensil_RF" runat="server" />
					<span id="dimensil_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
			</tr>
			<tr>
				<td>Ring Size</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<asp:textbox id="dimensir" runat="server" Text="0" cssclass="form-control text-lg text-center" width="125" />
						<%--span class="input-group-addon">MM</--%>
					</div>
				</td>
				<td>
					<span id="dimensir_ERR" runat="server" class="text-validator" visible="false">Format</span>
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

