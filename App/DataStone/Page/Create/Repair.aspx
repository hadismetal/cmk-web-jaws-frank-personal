<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Repair.aspx.cs" inherits="Page_Create_Repair" %>

<%@ register src="MenuRepair.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Repair
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form runat="server">
		<table class="table-form">
			<tr>
				<td>Shape</td>
				<td>:</td>
				<td>
					<asp:dropdownlist id="parcel01" runat="server" cssclass="form-control text-bold" width="275" />
				</td>
				<td>
					<span id="parcel01_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				</td>
			</tr>
			<tr>
				<td>Size</td>
				<td>:</td>
				<td>
					<asp:dropdownlist id="parcel02" runat="server" cssclass="form-control text-bold" width="275" />
				</td>
				<td>
					<span id="parcel02_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
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
