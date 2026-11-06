<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="5.aspx.cs" inherits="Page_Create_5" %>

<%@ register src="Menu5.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Pearl
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form runat="server">
		<table class="table-form">
			<tr>
				<td>Code</td>
				<td>:</td>
				<td>
					<asp:dropdownlist id="parcel01" runat="server" cssclass="form-control text-bold" width="275" />
				</td>
				<td>
					<span id="parcel01_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				</td>
			</tr>
			<tr>
				<td>Source</td>
				<td>:</td>
				<td>
					<asp:dropdownlist id="parcel02" runat="server" cssclass="form-control text-bold" width="275" />
				</td>
				<td>
					<span id="parcel02_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				</td>
			</tr>
			<tr>
				<td>Colour</td>
				<td>:</td>
				<td>
					<asp:dropdownlist id="parcel03" runat="server" cssclass="form-control text-bold" width="275" />
				</td>
				<td>
					<span id="parcel03_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				</td>
			</tr>
			<tr>
				<td>Shape</td>
				<td>:</td>
				<td>
					<asp:dropdownlist id="parcel04" runat="server" cssclass="form-control text-bold" width="275" />
				</td>
				<td>
					<span id="parcel04_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				</td>
			</tr>
			<tr>
				<td>Overtone</td>
				<td>:</td>
				<td>
					<asp:dropdownlist id="parcel05" runat="server" cssclass="form-control text-bold" width="275" />
				</td>
				<td>
					<span id="parcel05_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				</td>
			</tr>
			<tr>
				<td>Diameter</td>
				<td>:</td>
				<td>
					<asp:dropdownlist id="parcel06" runat="server" cssclass="form-control text-bold" width="275" />
				</td>
				<td>
					<span id="parcel06_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				</td>
			</tr>
			<tr>
				<td>Surface</td>
				<td>:</td>
				<td>
					<asp:dropdownlist id="parcel07" runat="server" cssclass="form-control text-bold" width="275" />
				</td>
				<td>
					<span id="parcel07_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				</td>
			</tr>
			<tr>
				<td>Luster</td>
				<td>:</td>
				<td>
					<asp:dropdownlist id="parcel08" runat="server" cssclass="form-control text-bold" width="275" />
				</td>
				<td>
					<span id="parcel08_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				</td>
			</tr>
			<tr>
				<td>Treatment</td>
				<td>:</td>
				<td>
					<asp:dropdownlist id="parcel09" runat="server" cssclass="form-control text-bold" width="275" />
				</td>
				<td>
					<span id="parcel09_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				</td>
			</tr>
		</table>
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<span id="kodeparcel_ERR" runat="server" class="text-validator" visible="false">Kode Parcel Sudah Digunakan</span>
		</div>
	</form>
</asp:content>
