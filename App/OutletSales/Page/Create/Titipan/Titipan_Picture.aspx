<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Titipan_Picture.aspx.cs" inherits="Page_Register_Titipan_Picture" %>

<%@ register src="MenuTitipan.ascx" tagname="Menu" tagprefix="uc1" %>

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
		<p class="text-muted">
			Max. Size : 
			<span id="kb" runat="server" class="text-bold text-danger" />
		</p>
		<table class="table-form">
			<tr>
				<td>Product Picture</td>
				<td>:</td>
				<td>
					<asp:fileupload id="imgpicture" runat="server" cssclass="form-control" />
				</td>
				<td>
					<span id="imgpicture_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
				<td id="imgpicture_format" runat="server" class="text-muted text-sm" />
			</tr>
		</table>
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<span id="dir_ERR" runat="server" class="text-validator" visible="false">Folder Error : </span>
		</div>
	</form>
</asp:content>

