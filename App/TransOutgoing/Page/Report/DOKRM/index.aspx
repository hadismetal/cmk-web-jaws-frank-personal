<%@ Page EnableEventValidation="false" Language="C#"
    MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="index.aspx.cs" Inherits="DOKRM_index" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">DO KRM </asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Report
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    </form>
    <div
        style="border: 1px solid rgb(216, 216, 216); border-radius: 10px; margin: 3px; padding: 20px;">
        <h2 style="border-bottom: 1px solid gray; width: max-content">Delivery Order - Report
        </h2>

        <div style="display: flex; flex-direction: row; gap: 10px">
            <div class="col-lg-2" style="padding: 10px 0 0 0">
                <label for="ddl_brand_location_asal">No DO KRM</label>
                <input id="kw" class="form-control" style="width: 200px" placeholder="Input Keyword..." />
            </div>
            <div class="col-lg-2" style="padding: 10px 0 0 5px">
                <label for="ddl_location_tujuan">Start Date</label>
                <input id="dari" class="form-control text-center" style="width: 110" placeholder="Dari" />
                <script>
                    $(document).ready(function () {
                        $('#dari').datepicker({ format: 'dd-mm-yyyy' });
                    });
                </script>
            </div>
            <div class="col-lg-2" style="padding: 10px 0 0 5px">
                <label for="ddl_location_tujuan">End Date</label>
                <input id="sampai" class="form-control text-center" style="width: 110" placeholder="Sampai" />
                <script>
                    $(document).ready(function () {
                        $('#sampai').datepicker({ format: 'dd-mm-yyyy' });
                    });
                </script>
            </div>
            <div class="col-lg-2" style="padding: 10px 0 0 5px">
                <label for="ddl_status">Status</label>
                <select class="form-control"
                    id="ddl_status">
                    <option value="0">All</option>
                    <option value="1">Waiting Approval</option>
                    <option value="2">Outgoing - Approved / On The Way</option>
                    <!--<option value="3">Rejected</option>-->
                    <option value="4">Delivered</option>
                </select>
            </div>
		

            <div style="display: flex; flex-direction: row; gap: 10px">
                <div class="col-lg-2" style="padding: 10px 0 0 5px">
                    <label for="btn_search_krm">&nbsp;</label>
                    <button class="btn btn-primary btn-md" id="btn_search_krm" type="button" onclick="appendTableBody();">
                        Display
                    </button>
                </div>
            </div>
			<div>
				<div class="col-lg-2" style="padding: 10px 0 0 0px">
                    <label for="btn_search_krm">&nbsp;</label>
                    <button class="btn btn-primary btn-md" id="btn_export" type="button" onclick="appendTableBodyExcel();">
                        Excel
                    </button>
                </div>
			</div>
        </div>
        <div style="margin-top: 20px">
            <div class="row" style="padding: 0; margin: 0">
                <div class="table-responsive">
                    <table id="tb_result" class="table table-bordered" style="max-width:100%">
                        <thead>
                            <tr>
                                <th>No.</th>
                                <th>NO DO KRM</th>
								<th>NO KRM</th>
								<th>Tujuan</th>
								<th>Qty</th>
								<th>Estimasi Harga</th>
                                <th>Kurir</th>
                                <th>Status DO</th>
                                <th>Operator</th>
                                <th>Operator Tgl</th>
                                <th>Approval</th>
                                <th>Approval Tgl</th>
                                <th>ETD</th>
                                <th>Keterangan</th>
								<th>Action</th>
                            </tr>
                        </thead>
                        <tbody id="tbody-data"></tbody>
                    </table>
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Detail -->
     <div
        class="modal fade"
        id="myModalDetail"
        role="dialog">
        <div class="modal-dialog" style="width:80%">
            <div class="modal-content">
                <div class="modal-header">
                    <button
                        type="button"
                        class="close"
                        data-dismiss="modal"
                        aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="myModaDetaillLabel">Detail KRM
                    </h4>
                </div>
                <div class="modal-body">
					<div class="form-group">
						<table>
							<tr>
								<td>
									<p class="text-bold">
										Nama Kurir
									</p>
								</td>
								<td style="padding-left: 10px; padding-right: 10px">
									<p class="text-bold">:</p>
								</td>
								<td>
									<p class="text-bold" id="lbl_nama_kurir"></p>
								</td>
							</tr>
							<tr>
								<td>
									<p class="text-bold">
										Status DO
									</p>
								</td>
								<td style="padding-left: 10px; padding-right: 10px">
									<p class="text-bold">:</p>
								</td>
								<td>
									<p class="text-bold" id="lbl_status_do"></p>
								</td>
							</tr>
							<tr>
								<td>
									<p class="text-bold">
										Operator Nama
									</p>
								</td>
								<td style="padding-left: 10px; padding-right: 10px">
									<p class="text-bold">:</p>
								</td>
								<td>
									<p class="text-bold" id="lbl_operator"></p>
								</td>
							</tr>
							<tr>
								<td>
									<p class="text-bold">
										Operator Tgl
									</p>
								</td>
								<td style="padding-left: 10px; padding-right: 10px">
									<p class="text-bold">:</p>
								</td>
								<td>
									<p class="text-bold" id="lbl_operator_tgl"></p>
								</td>
							</tr>
							<tr>
								<td>
									<p class="text-bold">
										Estimation Time Departure
									</p>
								</td>
								<td style="padding-left: 10px; padding-right: 10px">
									<p class="text-bold">:</p>
								</td>
								<td>
									<p class="text-bold" id="lbl_departure"></p>
								</td>
							</tr>
							<tr>
								<td>
									<p class="text-bold">
										Keterangan
									</p>
								</td>
								<td style="padding-left: 10px; padding-right: 10px">
									<p class="text-bold">:</p>
								</td>
								<td>
									<p class="text-bold" id="lbl_keterangan"></p>
								</td>
							</tr>
							<tr>
								<td style="vertical-align:top">
									<p class="text-bold">
										Keterangan Titipan
									</p>
								</td>
								<td style="padding-left: 10px; padding-right: 10px; vertical-align:top">
									<p class="text-bold">:</p>
								</td>
								<td id="lbl_keterangan_titipan" style="vertical-align:top">
								</td>
							</tr>
							
						</table>
						<ul class="nav nav-tabs" role="tablist">
  						  <li role="presentation" class="active"><a href="#profile" aria-controls="profile" role="tab"  data-toggle="tab">Add KRM</a></li>
  						  <li role="presentation" ><a href="#home" aria-controls="home" role="tab" data-toggle="tab" >Delete</a></li>
  						</ul>
						<div class="tab-content">
							<div role="tabpanel" class="tab-pane active" id="profile">
								<form id="form_input_krm">
    							  <div class="row" style="padding: 0; margin: 0">
    							    <div class="col-lg-12" style="display: flex; align-items: end; gap: 5px; padding: 0px">
    							      <div class="col-lg-5" style="padding: 0px">
    							        <label for="nomor_krm_input">Nomor KRM (Same Brand / Cross Brand)</label>
    							        <input
    							          class="form-control"
    							          id="nomor_krm_input"
    							          value=""
    							          placeholder="Masukkan nomor KRM..."
    							        />
    							      </div>
    							      <div id="btnCari" class="col-lg-6">

									  </div>
    							    </div>
    							  </div>
    							</form>

								<div style="margin-top: 20px">
    							  <table class="table table-bordered">
    							    <thead>
    							      <tr>
    							        <th>No.</th>
    							        <th>Nomor Kirim</th>
										<th>Tipe Product</th>
										<th>Cross Brand</th>
    							        <th>Keterangan</th>
    							        <th>Jumlah Barang</th>
										<th>Gross Weight</th>
										<th>Estimasi Harga</th>
										<th>Lokasi Asal</th>
    							        <th>Lokasi Tujuan</th>
    							        <th>Action</th>
    							      </tr>
    							    </thead>
									<input type="hidden" id="totalAdd" value="0">
    							    <tbody id="tbody-dataAdd"></tbody>
    							  </table>								  	
    							</div>
								<div id="addKRM" class="text-right">									
								</div>
							</div>
    						<div role="tabpanel" class="tab-pane" id="home">  					
								<div class="table-responsive text-sm">
									<table id="tb_resultdetail" class="table table-bordered">
										<thead>
											<tr>
												<th>No.</th>
                            	                <th>
                            	                	<input type="checkbox" id="checkbox-all" onclick="onCheckAll(this);">									
									            </th>
												<th>No Krm</th>
												<th>Lokasi Asal</th>
												<th>Lokasi Tujuan</th>
												<th>Keterangan</th>
												<th>Qty</th>
												<th>Gross Weight</th>											
											</tr>
										</thead>
										<tbody id="tbody-data-detail"></tbody>

									</table>
								</div>
								<div id="deleteKRM" class="text-right">									
								</div>
							</div>
						</div>
					</div>
                </div>
                <div class="modal-footer" id="detailFoot">
					
                </div>
            </div>
        </div>
    </div>
	<!-- Modal Add KRM -->    
    <script type="text/javascript">
        function appendTableBody() {
            const btnSearch = $("#btn_search_krm");
            const inputValue = $("#kw");
            const inputDari = $("#dari");
            const inputSampai = $("#sampai");
            const statusDO = $("#ddl_status");            
            btnSearch.prop('disabled', true);

            $.ajax({
                type: "GET",
                url: "./index.aspx/ReportKRM",
                data: {
                    keyword: `'${inputValue.val()}'`,
                    start: `'${inputDari.val()}'`,
                    end: `'${inputSampai.val()}'`,
                    loccode: ``,
                    statusdo: `'${statusDO.val()}'`
                },
                contentType: "application/json; charset=utf-8",
                beforeSend: function (request) {
                    request.withCredentials = false;
                },
                success: function (data) {
                    let response = data.d;
                    $('#tbody-data > tr').remove();
                    let btnUpdate = "";
                    if (response.length > 0) {
						let totalbarang = 0;
						let totalhargabarang = 0;
                        $(response).each(function (index) {                                                        							
						if(`${response[index].statusdo}` == `Approved` || `${response[index].statusdo}` == `Delivered`){
                            $("#tbody-data").append(`
							<tr>
								<td>${index + 1}</td>
								<td style="display:none">${response[index].id}</td>
								<td>${response[index].nodokrm}</td>
								<td>${response[index].nokrm}</td>
								<td>${response[index].tujuan}</td>
								<td>${response[index].qty}</td>
								<td>${response[index].estimasi_harga_int}</td>
								<td>${response[index].kurir}</td>
								<td>${response[index].statusdo}</td>    
								<td>${response[index].operatornama}</td>
								<td>${response[index].operatortgl}</td>
								<td>${response[index].approvalnama}</td>
								<td>${response[index].approvaltgl}</td>
								<td>${response[index].etd}</td>
								<td>${response[index].keterangan}</td>
								<td>
									<div style="display: flex; gap: 5px;">
										<button type="button" class="btn btn-success btn-md" data-id="${response[index].id}" onclick="onPrint(this);">Print</a>
									</div>
								</td>
							</tr>
						`);
						totalbarang += parseInt(response[index].qty);
						totalhargabarang += parseInt(response[index].estimasi_harga_int);
						}
						else if(`${response[index].statusdo}` == `Waiting Approval`){
							$("#tbody-data").append(`
							<tr>
								<td>${index + 1}</td>
								<td style="display:none">${response[index].id}</td>
								<td>${response[index].nodokrm}</td>
								<td>${response[index].nokrm}</td>
								<td>${response[index].tujuan}</td>
								<td>${response[index].qty}</td>
								<td>${response[index].estimasi_harga_int}</td>
								<td>${response[index].kurir}</td>
								<td>${response[index].statusdo}</td>    
								<td>${response[index].operatornama}</td>
								<td>${response[index].operatortgl}</td>
								<td>${response[index].approvalnama}</td>
								<td>${response[index].approvaltgl}</td>
								<td>${response[index].etd}</td>
								<td>${response[index].keterangan}</td>
								<td>                                    		
								</td>
							</tr>
							`);
							totalbarang += parseInt(response[index].qty);
							totalhargabarang += parseInt(response[index].estimasi_harga_int);
						}
						else{
							$("#tbody-data").append(`
						<tr>
							<td>${index + 1}</td>
							<td style="display:none">${response[index].id}</td>
                            <td>${response[index].nodokrm}</td>
                            <td>${response[index].nokrm}</td>
                            <td>${response[index].kurir}</td>
							<td>${response[index].statusdo}</td>    
							<td>${response[index].operatornama}</td>
							<td>${response[index].operatortgl}</td>
                            <td>${response[index].approvalnama}</td>
                            <td>${response[index].approvaltgl}</td>
							<td>${response[index].etd}</td>
							<td>${response[index].keterangan}</td>
							<td></td>
						</tr>
						`);
						totalbarang += parseInt(response[index].qty);
						totalhargabarang += parseInt(response[index].estimasi_harga_int);
						}
                        });
						$("#tbody-data").append(`
						<tr>
							<td colspan="4"><b>Total</b></td>
							<td><b>${totalbarang}</b></td>
							<td><b>${totalhargabarang}</b></td>
							<td colspan="8"></td>
						</tr>
						`)
                        toastr.success("Nomor KRM berhasil didaftarkan.");
                    }
					else{
						toastr.error("Nomor KRM tidak ditemukan.");
					}
					
                    btnSearch.prop("disabled", false);
                    inputValue.val("");
                },
                error: function (error) {
                    console.error(error);
                    btnSearch.prop("disabled", false);
                    toastr.error("Terjadi kesalahan!");
                },
            });
            btnSearch.prop('disabled', false);
        }
       
        function appendTableBodyAdd(tipeTujuan,idLokasiTujuan,idBrandTujuan) {			
        	const btnSearch = $("#btn_search_krm");
			const rowscount = document.getElementById("tbody-dataAdd").getElementsByTagName("tr").length;
			const tebody = $("#tbody-dataAdd");
			const nomorKRM = $('#nomor_krm_input').val();

		 	var dataKRM ={
				nomorKRM:`'${nomorKRM}'`,
				brandtujuan:idBrandTujuan,
				idlokasitujuan:idLokasiTujuan,
				tipelokasitujuan:tipeTujuan
		 	};

		btnSearch.prop('disabled', true);
		console.log(dataKRM)
		$.ajax({
          type: "GET",
          url: "./index.aspx/GetNomorKRM",
          data: dataKRM,
          contentType: "application/json; charset=utf-8",
          beforeSend: function (request) {
            request.withCredentials = false;
          },
          success: function (data) {

            const response_message = data.d.error_message;
			let nokrm = 0;
			console.log(data)
			$('#tbody-dataAdd > tr').each(function(index, tr) {
				if($.trim(nomorKRM) == $(tr).find('td').eq(1).text())
					nokrm = nokrm + 1;
			});
			
			if(nokrm > 0){
			  toastr.error("Nomor KRM " + $.trim($("#nomor_krm_input").val()) + " sudah terdaftar.");
			}
			else if(response_message === ""){
				const response = data.d.krm;
				if (response === null) {
				  toastr.error("Nomor KRM tidak ditemukan.");
				}
				else {
					$("#tbody-dataAdd").append(`
						<tr>
							<td>${rowscount + 1}</td>
							<td>${response.Nomor}</td>
							<td>${response.Tipe_Product}</td>
							<td>${response.Is_Cross_Brand}</td>
							<td>${response.Keterangan}</td>    
							<td>${response.JumlahBarang}</td>
							<td>${response.GrossWeight}</td>
							<td>${response.EstimasiHarga}</td>
							<td>${response.LokasiAsal}</td>
							<td>${response.LokasiTujuan}</td>    
							<td style="display:none;">${response.Brand_Asal}</td>    
							<td style="display:none;">${response.ID_Tipe_Asal}</td>    
							<td style="display:none;">${response.ID_Lokasi_Asal}</td>
							<td style="display:none;">${response.Brand_Tujuan}</td>    
							<td style="display:none;">${response.ID_Tipe_Asal}</td>    
							<td style="display:none;">${response.ID_Lokasi_Tujuan}</td>
							<td>
								<div style="display: flex; gap: 5px;">
									<button type="button" class="btn btn-danger" data-nomor="${response.Nomor}" onclick="onDelete(this);">Hapus</button>
									<a 
									target="_blank"
									href="${
									  window.location.origin + response.PathToDetail
									}" class="btn btn-success btn-md">Detail</a>
								</div>
							</td>  
						</tr>
						`);
					toastr.success("Nomor KRM berhasil didaftarkan.");
				}
			}
			else
			{
				toastr.error(response_message);
			}
			
			$('#nomor_krm_input').val("");
			
			if(document.getElementById("tbody-dataAdd").getElementsByTagName("tr").length == 0){
			  $("#btnSaveKRM").prop("disabled", true);
			  $("#ddl_brand_location_tujuan").prop("disabled", false);
			  $("#ddl_tipe_location_tujuan").prop("disabled", false);
			  $("#ddl_location_tujuan").prop("disabled", false);
			}
			else{
			  $("#btnSaveKRM").prop("disabled", false);
			  $("#ddl_brand_location_tujuan").prop("disabled", true);
			  $("#ddl_tipe_location_tujuan").prop("disabled", true);
			  $("#ddl_location_tujuan").prop("disabled", true);
			}
          
		  },
          error: function (error) {
            console.error(error);
            btnSearch.prop("disabled", false).text("Cari");
            toastr.error("Terjadi kesalahan!");
          },
        });
		btnSearch.prop('disabled', false);
        }
       
		function onDetail(e){
			let id = $(e).attr("data-id");
			$.ajax({
                type: "GET",
                url: "./index.aspx/GetDetailKRM",
                data: {
					IDF: id
				},
                contentType: "application/json; charset=utf-8",
                beforeSend: function (request) {
                    request.withCredentials = false;
                },
                success: function (data) {
                    let response = data.d;
					if(response.status == 1){
						let resheader = response.header;
						let resdetail = response.header.detail;
						$('#lbl_keterangan').text(resheader.keterangan);
						$('#lbl_nama_kurir').text(resheader.namakurir);
						$('#lbl_status_do').text(resheader.statusdo);
						$('#lbl_operator').text(resheader.operatornama);
						$('#lbl_operator_tgl').text(resheader.operatortgl);
						$('#lbl_departure').text(resheader.etd);
						
						$('#lbl_keterangan_titipan').children().remove();
						$('#lbl_keterangan_titipan').append(resheader.keterangan_titipan);
						
						$('#tbody-data-detail > tr').remove();
						if (resdetail.length > 0) {
							$(resdetail).each(function (index) {
								$("#tbody-data-detail").append(`
							<tr>
								<td>${index + 1}</td>
								<td>${resdetail[index].nokrm}</td>
								<td>${resdetail[index].lokasi_asal}</td>
								<td>${resdetail[index].lokasi_tujuan}</td>
								<td>${resdetail[index].keterangan}</td>
								<td>${resdetail[index].qty_outgoing}</td>
								<td>${resdetail[index].weight_outgoing}</td>
							</tr>
							`);
							});
						}
						
						$('#myModalDetail').modal('show');
					}
					else{
						toastr.error("Terjadi kesalahan!");
					}
				},
                error: function (error) {
                    console.error(error);
                    toastr.error("Terjadi kesalahan!");
                },
			});
		}			
		function appendTableBodyExcel() {
            const btnSearch = $("#btn_export");
            const inputValue = $("#kw");
            const inputDari = $("#dari");
            const inputSampai = $("#sampai");
            const statusDO = $("#ddl_status");
            btnSearch.prop('disabled', true);

            $.ajax({
                type: "GET",
                url: "./index.aspx/ReportKRM",
                data: {
                    keyword: `'${inputValue.val()}'`,
                    start: `'${inputDari.val()}'`,
                    end: `'${inputSampai.val()}'`,
                    loccode: `'${statusDO.val()}'`,
                    statusdo: ``
                },
                contentType: "application/json; charset=utf-8",
                beforeSend: function (request) {
                    request.withCredentials = false;
                },
                success: function (data) {
                    let response = data.d;
                    $('#tbody-data > tr').remove();
                    if (response.length > 0) {
						let totalbarang = 0;
						let totalhargabarang = 0;
                        $(response).each(function (index) {
                            $("#tbody-data").append(`
						<tr>
							<td>${index + 1}</td>
                            <td>${response[index].nodokrm}</td>
                            <td>${response[index].nokrm}</td>
							<td>${response[index].tujuan}</td>
							<td>${response[index].qty}</td>
							<td>${response[index].estimasi_harga_int}</td>
							<td>${response[index].kurir}</td>
							<td>${response[index].statusdo}</td>    
							<td>${response[index].operatornama}</td>
							<td>${response[index].operatortgl}</td>
                            <td>${response[index].approvalnama}</td>
                            <td>${response[index].approvaltgl}</td>
							<td>${response[index].etd}</td>
							<td>${response[index].keterangan}</td>
							<td></td>
						</tr>
						`);
						totalbarang += parseInt(response[index].qty);
						totalhargabarang += parseInt(response[index].estimasi_harga_int);
                        });
						$("#tbody-data").append(`
						<tr>
							<td colspan="4"><b>Total</b></td>
							<td><b>${totalbarang}</b></td>
							<td><b>${totalhargabarang}</b></td>
							<td colspan="8"></td>
						</tr>
						`)
                        toastr.success("Nomor KRM berhasil didaftarkan.");
                    }
					else {
						toastr.success("Data kosong.");
					}
					var location = 'data:application/vnd.ms-excel;base64,';
					var excelTemplate = '<html> ' +
						'<head> ' +
						'<meta http-equiv="content-type" content="text/plain; charset=UTF-8"/> ' +
						'</head> ' +
						'<body> ' +
						document.getElementById("tb_result").outerHTML +
						'</body> ' +
						'</html>'					
					
					var downloadLink = document.createElement("a");
					downloadLink.href = location + window.btoa(excelTemplate);

					// Change the filename here (e.g., 'DOKRM_Outgoing.xls')
					downloadLink.download = "DOKRM_Outgoing.xls";  // Specify the filename

					downloadLink.click();  // Trigger download
					
                    btnSearch.prop("disabled", false);
                    inputValue.val("");
                },
                error: function (error) {
                    console.error(error);
                    btnSearch.prop("disabled", false);
                    toastr.error("Terjadi kesalahan!");
                },
            });
            btnSearch.prop('disabled', false);
        }
		
		function onPrint(e){
			let id = $(e).attr("data-id");
			let newUrl = window.location.protocol + "//" + window.location.host + "/TransOutgoing/Page/Report/DOKRM/index_print.aspx?id=" + id;
			window.open(newUrl,'_blank');
		}
				
		function onUpdate(e,tipeTujuan,idLokasiTujuan,idBrandTujuan){						
			
			let id = $(e).attr("data-id");
			let htmlADD = `<button type="button" class="btn btn-success" id="btnSaveKRM" disabled value="add" onclick="onSaveKRM(${id});">
                        				Save
                    			</button>`;
			let htmlDELETE = `<button type="button" class="btn btn-danger" id="btnDeleteKRM" disabled value="add" onclick="onDeleteKRM();">
                        				Delete
                    			</button>`;
			$('#addKRM').html(htmlADD);
			$('#deleteKRM').html(htmlDELETE);
			$.ajax({				
                type: "GET",
                url: "./index.aspx/GetDetailKRM",
                data: {
					IDF: id
				},
                contentType: "application/json; charset=utf-8",
                beforeSend: function (request) {
                    request.withCredentials = false;
                },
                success: function (data) {
                    let response = data.d;					
                    console.log(response);
					if(response.status == 1){
						let resheader = response.header;
						let resdetail = response.header.detail;
						$('#lbl_keterangan').text(resheader.keterangan);
						$('#lbl_nama_kurir').text(resheader.namakurir);
						$('#lbl_status_do').text(resheader.statusdo);
						$('#lbl_operator').text(resheader.operatornama);
						$('#lbl_operator_tgl').text(resheader.operatortgl);
						$('#lbl_departure').text(resheader.etd);
						
						$('#lbl_keterangan_titipan').children().remove();
						$('#lbl_keterangan_titipan').append(resheader.keterangan_titipan);
						
						$('#tbody-data-detail > tr').remove();
						if (resdetail.length > 0) {
							$(resdetail).each(function (index) {								
								$("#tbody-data-detail").append(`
							<tr>
								<td>${index + 1}</td>															
								<td><input type="checkbox" data-id="${resdetail[index].idDetailKRM}" onclick="onCheckDetail(this);" class="check-claas"></td>
								<td>${resdetail[index].nokrm}</td>
								<td>${resdetail[index].lokasi_asal}</td>
								<td>${resdetail[index].lokasi_tujuan}</td>
								<td>${resdetail[index].keterangan}</td>
								<td>${resdetail[index].qty_outgoing}</td>
								<td>${resdetail[index].weight_outgoing}</td>
							</tr>
							`);
							});
						}
						let html12 = `									
									<button type="button" class="btn btn-default" data-dismiss="modal">
                        				Close
                    				</button>`;
						$('#detailFoot').html(html12)
						$('#myModalDetail').modal('show');						
						$('#btnCari').html(`<button class="btn btn-success btn-md col-lg-2" id="btn_search_krm" type="button" onclick="appendTableBodyAdd('${tipeTujuan}','${idLokasiTujuan}','${idBrandTujuan}');">Cari</button>`)
					}
					else{
						toastr.error("Terjadi kesalahan!");
					}
				},
                error: function (error) {
                    console.error(error);
                    toastr.error("Terjadi kesalahan!");
                },
			});
		
		}

		function onDeleteKRM(){
			const id = [];
			
			$('#tbody-data-detail > tr').each(function (index, teer) {
                let cekbok = $(teer).find('td').eq(1).find("input[type='checkbox']");
				if(cekbok.is(':checked')){
					id.push(cekbok.attr('data-id'));
				}
            });
			console.log(JSON.stringify({ids : id}));
            $.ajax({
                type: "POST",
                url: "./index.aspx/DeleteDO",
                data: JSON.stringify({ids : id}),
                contentType: "application/json; charset=utf-8",
                beforeSend: function (request) {
                    request.withCredentials = false;
                },
                success: function (result) {
                    if (result.d.status == 1) {
                        toastr.success("Data DO sukses di hapus. Mohon dicek kembali.");												
                    }
                    else {
                        toastr.error(result.d.message);
                    }
                },
                error: function (error) {
                    console.error(error);
                    toastr.error("Terjadi kesalahan!");
                }
            });
		}

		function onSaveKRM(IDHeaderKRM){
			const detail = [];
			
			$('#tbody-dataAdd > tr').each(function(index, tr) {
				const nokrm = $(tr).find('td').eq(1).text();
				const tipe_prod = $(tr).find('td').eq(2).text();
				const cross_brand = $(tr).find('td').eq(3).text();
				const keterang = $(tr).find('td').eq(4).text();
				const jmlbrg = $(tr).find('td').eq(5).text();
				const gw = $(tr).find('td').eq(6).text();
				const estimasi_harga = $(tr).find('td').eq(7).text();
				const asal = $(tr).find('td').eq(8).text();
				const tuju = $(tr).find('td').eq(9).text();
				const idbrandasal = $(tr).find('td').eq(10).text();
				const idtipeasal = $(tr).find('td').eq(11).text();
				const idlokasiasal = $(tr).find('td').eq(12).text();
				const idbrandtujuan = $(tr).find('td').eq(13).text();
				const idtipetujuan = $(tr).find('td').eq(14).text();
				const idlokasitujuan = $(tr).find('td').eq(15).text();
														
				
				detail.push(new DataKrm_Detail(IDHeaderKRM,tipe_prod, cross_brand, nokrm, keterang, jmlbrg, gw, asal, tuju, estimasi_harga,idbrandasal, idtipeasal, idlokasiasal, idbrandtujuan, idtipetujuan, idlokasitujuan));
			});
			const test = new RequestDataKRM(detail);
			const testStringify = JSON.stringify({data: test});
			console.log([testStringify,test]);
			$.ajax({								
			  type: "POST",
			  url: "./index.aspx/UpdateDO",
			  data: testStringify,
			  contentType: "application/json; charset=utf-8",
			  beforeSend: function (request) {
				request.withCredentials = false;
			  },
			  success: function (result) {
				console.log(result.d);
				if(result.d.status == 1){
					toastr.success("Data DO berhasil dibuat. Mohon dicek di menu approve dokrm di menu outgoing (khusus untuk SM / JC).");										
				}
				else{
					toastr.error(result.d.message);
				}
			  },
			  error: function (error) {
				console.error(error);
				toastr.error("Terjadi kesalahan!");
			  }
			 });
		
		}
		function RequestDataKRM(krm_detail){			
			this.krm_detail = krm_detail;
		}

		function onDelete(e){
			$(e).closest('tr').remove();
			$('#tbody-dataAdd > tr').each(function(index, tr) {
				$(tr).find('td').eq(0).text(index + 1);
			});

			if(document.getElementById("tbody-dataAdd").getElementsByTagName("tr").length == 0){
			  $("#btnSaveKRM").prop('disabled',true);
			}
			else{
			  $("#btnSaveKRM").prop('disabled',false);			  
			}
		}

		function onCheckAll(e) {			
            if (!e.checked) {				
                $('#btnDeleteKRM').prop('disabled', 'disabled');
				$('#tbody-data-detail > tr').each(function (index, tr) {
					let cekbok = $(tr).find('td').eq(1).find("input[type='checkbox']");
					cekbok.prop('checked', false);
				});
            }
            else {				
				$('#tbody-data-detail > tr').each(function (index, tr) {					
					let cekbok = $(tr).find('td').eq(1).find("input[type='checkbox']");
					cekbok.prop('checked', true);
				});

                $('#btnDeleteKRM').removeAttr('disabled');
            }
        }
		
		function onCheckDetail(e){
			var i = 0;
			$('#tbody-data-detail > tr').each(function (index, teer) {
                let cekbok = $(teer).find('td').eq(1).find("input[type='checkbox']");
				if(cekbok.is(':checked')){
					i = i + 1;
				}
            });
			
			if(i > 0){
				$('#btnDeleteKRM').prop('disabled', false);
			}
			else{
				$('#btnDeleteKRM').prop('disabled', true);
			}
			
			if(i == document.getElementById("tbody-data-detail").getElementsByTagName("tr").length){
				$('#checkbox-all').prop('checked', true);
			}
			else{
				$('#checkbox-all').prop('checked', false);
			}
		}
		
		function DataKrm_Detail(IDDOHead,Tipe_Product, Is_Cross_Brand, Nomor, Keterangan, JumlahBarang, GrossWeight, Asal, Tujuan, EstimasiHarga,idbrandasal, idtipeasal, idlokasiasal, idbrandtujuan, idtipetujuan, idlokasitujuan){
			this.IDDOHead = IDDOHead;
			this.NoKRM = Nomor;
			this.TipeProduct = Tipe_Product == "DJ" ? 1 : Tipe_Product == "PG" ? 2 : Tipe_Product == "GJ" ? 3 : 4;
			this.IsCrossBrand = (Is_Cross_Brand == "Cross Brand") ? 1 : 0;
			this.QtyOutgoing = parseInt(JumlahBarang);
			this.GrossWeightOutgoing = parseFloat(GrossWeight);
			this.Keterangan = Keterangan;
			this.Asal = Asal;
			this.Tujuan = Tujuan;
			this.EstimasiHarga = EstimasiHarga;
			this.BrandAsal = idbrandasal;
			this.IDTipeAsal = idtipeasal;
			this.IDLokasiAsal = idlokasiasal;
			this.BrandTujuan = idbrandtujuan;
			this.IDTipeTujuan = idtipetujuan;
			this.IDLokasiTujuan = idlokasitujuan;
		}

        $(document).ready(function () {
            var tglsekarang = new Date();
            const tgl = String("0" + tglsekarang.getDate()).slice(-2);
            const bulan = String("0" + (tglsekarang.getMonth() + 1)).slice(-2);
            const tahun = String(tglsekarang.getFullYear());
            $('#dari').val(tgl + '-' + bulan + '-' + tahun);
            $('#sampai').val(tgl + '-' + bulan + '-' + tahun);
				  		
			
			toastr.options = {
                closeButton: true,
                progressBar: true,
                positionClass: "toast-bottom-center",
                showDuration: "250",
                hideDuration: "250",
                timeOut: "1500",
				"showMethod": "slideDown",
				"hideMethod": "slideUp"
            };

			$('#myModalDetail').on('hidden.bs.modal', function (e) {
				e.preventDefault();
				$("#tbody-dataAdd").empty();
				$('#checkbox-all').prop('checked', false);
			$('#tbody-data-detail > tr').each(function (index, tr) {
				let cekbok = $(tr).find('td').eq(1).find("input[type='checkbox']");
				cekbok.prop('checked', false);
			});
			})
        });
    </script>
</asp:Content>
