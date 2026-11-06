<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Titipan_Form.aspx.cs" inherits="Page_Register_Titipan_Form" %>

<%@ register src="MenuTitipan.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Titipan Customer
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<p>
		<span class="text-muted">Location</span><br />
		<span class="text-bold" id="warehouse" runat="server" />
	</p>
	<hr />
	<form runat="server">
		
		<table class="table-form">
            <tr>
				<td class="fixed-150">Customer
				</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="Titipan_DataCustomer.aspx" class="btn btn-info" id="search_customer" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="customer" Enabled="false" runat="server" cssclass="form-control" maxlength="50" width="250" />
					</div>
				</td>
			</tr>
            <tr>
				<td class="fixed-150">JPJR
				</td>
				<td>:</td>
				<td>
					<div class="input-group">
						<div class="input-group-btn">
							<a href="Titipan_DataAgent.aspx" class="btn btn-info" id="search_agent" runat="server">
								<i class="icon-search"></i>
							</a>
						</div>
						<asp:textbox id="agent" Enabled="false" runat="server" cssclass="form-control" maxlength="50" width="250" />
					</div>
				</td>
			</tr>
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
				<td>Tipe Titipan</td>
				<td>:</td>
				<td>
					<select id="tipe_titipan" class="form-control" >						
					<option value="-1">--- Silahkan Pilih Data Dibawah ---</option>
					</select>
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
			<button id="saveBtn" class="btn btn-primary" type="button">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>

			<button id="save" runat="server" class="btn btn-primary" style="display: none;" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<a href="Titipan.aspx" class="btn btn-default">
				<i class="icon-remove"></i>
				Cancel
			</a>
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
			
			
			$.ajax({
				url: "Titipan_Form.aspx/GetTipeTitipan",
				type: "GET",
				contentType: "application/json; charset=utf-8",
				dataType: "json",
				success: function (response) {
					let data = response.d;
					let html = ``;
					data.forEach(function (e,i) {
						html+=`
						<option value='` + e.ID + `'>` + e.Nama + `</option>
						`;
					});
					$("#tipe_titipan").append(html);
				},
				error: function (response) {
					console.log(response);
				}



				
			});
			$('#saveBtn').click(()=> {
				const urlParams = new URLSearchParams(window.location.search);                        	
				let IDDoc = urlParams.get('id');
				let tipe = urlParams.get('tipe');
				let status = urlParams.get('status');
				let ddlTipe = $("#tipe_titipan").val();
				let JR = $("#<%= agent.ClientID %>").val();
				let customerValue = $("#<%= customer.ClientID %>").val();
				let tglValue = $("#<%= tgl.ClientID %>").val();	
				let keteranganValue = $("#<%= keterangan.ClientID %>").val();
				
				if (customerValue == '') {
					toastr.error("Customer harus dipilih!!");
					$("#<%= customer.ClientID %>").focus();
					return;
				}
				if (JR == '') {
					toastr.error("JP/JR harus dipilih!!");
					$("#<%= agent.ClientID %>").focus();
					return;
				}

				if (ddlTipe == -1) {
					toastr.error("Tipe Titipan harus dipilih!!");
					$('#tipe_titipan').focus();
					return;
				}

				let params ={ 
					IDDoc: IDDoc,					
					Keterangan: keteranganValue,
					TipeTitipan: ddlTipe
				};

				 $.ajax({
					type: "POST",
					url: "Titipan_Form.aspx/SaveData",
					data: JSON.stringify(params),
					contentType: "application/json; charset=utf-8",
					dataType: "json",
					success: function (response) {                   
						let responseData = response.d; 						
						if (responseData[0].StatusCode == 200) {							 
							window.location.href = `Titipan_Data.aspx?id=${IDDoc}`;
						}else{
							console.log(responseData[0].MessageText);
							toastr.error("Terjadi kesalahan saat menyimpan data: harap hubungi IT");
						}
					},
					error: function (xhr, status, error) {
						$('#btnSave').removeAttr("disabled");
						$("#errorMsg").text("Error saving : " + error).show();
						toastr.error(`Error saving : ${error}`);
						
					}
				});

			});			
		});
	</script>
</asp:content>
