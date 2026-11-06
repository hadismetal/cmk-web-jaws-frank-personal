<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Titipan_CustomerPicture.aspx.cs" inherits="Page_Register_Titipan_Picture" %>

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
			<button id="saveBtn" class="btn btn-primary" type="button">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<button id="save" runat="server" style="display: none;" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<span id="dir_ERR" runat="server" class="text-validator" visible="false">Folder Error : </span>
		</div>
	</form>
	<script>
		$(document).ready(function () {		
			toastr.options = {
				closeButton: true,
				progressBar: true,
				positionClass: "toast-bottom-center",
				showDuration: "300",
				hideDuration: "1000",
				timeOut: "5000",
			};
		
			$('#saveBtn').click(()=> {
				let checkPict = $("#<%= imgpicture.ClientID %>").val()
				let Gambar = $("#<%= imgpicture.ClientID %>")[0].files[0];				 
				if (checkPict == '') {
					toastr.error("Gambar harus di Upload!!");
					return;
				}

				 let allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;
				 if (!allowedExtensions.exec(checkPict)) {
					 toastr.error("Format gambar tidak valid!!");
					 $("#<%= imgpicture.ClientID %>").val('');
					 return;
				 }

				 $("#<%= save.ClientID %>").click();

				 
			});	
		});
	</script>
</asp:content>

