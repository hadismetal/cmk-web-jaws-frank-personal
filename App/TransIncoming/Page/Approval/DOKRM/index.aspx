<%@ Page EnableEventValidation="false" Language="C#"
    MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="index.aspx.cs" Inherits="DOKRM_index" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">DO KRM </asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Approval
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
    </form>
    <div
        style="border: 1px solid rgb(216, 216, 216); border-radius: 10px; margin: 3px; padding: 20px;">
        <h2 style="border-bottom: 1px solid gray; width: max-content">Delivery Order - Approval
        </h2>

        <div style="display: flex; flex-direction: row; gap: 10px">
            <div class="col-lg-2" style="padding: 10px 0 0 0">
                <label for="ddl_brand_location_asal">Keyword</label>
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
            <div style="display: flex; flex-direction: row; gap: 10px">
                <div class="col-lg-2" style="padding: 10px 0 0 5px">
                    <label for="btn_search_krm">&nbsp;</label>
                    <button class="btn btn-primary btn-md" id="btn_search_krm" type="button" onclick="appendTableBody();">
                        Display
                    </button>
                </div>
            </div>
        </div>
        <div style="margin-top: 20px">
            <div class="row" style="padding: 0; margin: 0">
                <div class="table-responsive">
                    <table id="tb_result" class="table table-bordered">
                        <thead>
                            <tr>
                                <th>No.</th>
                                <th style="display: none">ID</th>
                                <th>
                                    <input type="checkbox" id="checkbox-all" onclick="onCheckAll(this);">									
								</th>
								<th>Nomor DO</th>
                                <th>Lokasi Tujuan</th>
                                <th>Kurir</th>
                                <th>Status DO</th>
                                <th>Operator</th>
                                <th>Operator Tgl</th>
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
        <div>
            <button class="btn btn-success btn-md" id="btn_approve_do" type="button" data-toggle="modal" data-target="#myModal" disabled>
                Approve Checked
            </button>
        </div>
    </div>
	 <!-- Modal -->
	  <div
		class="modal fade"
		id="myModal"
		tabindex="-1"
		role="dialog"
		aria-labelledby="myModalLabel"
	  >
		<div class="modal-dialog" role="document">
		  <div class="modal-content">
			<div class="modal-header">
			  <button
				type="button"
				class="close"
				data-dismiss="modal"
				aria-label="Close"
			  >
				<span aria-hidden="true">&times;</span>
			  </button>
			  <h4 class="modal-title" id="myModalLabel">
				WARNING !!!
			  </h4>
			</div>
			<div class="modal-body">
			  Apakah anda yakin ingin approve DO KRM berikut? Mohon periksa kembali krm tersebut agar tidak salah pilih krm. Jika anda sudah yakin, klik tombol 'Yakin', tika tidak klik tombol 'Batal'.
			</div>
			<div class="modal-footer">
			  <button type="button" class="btn btn-default" data-dismiss="modal">
				Batal
			  </button>
			  <button
				type="button"
				class="btn btn-primary"
				id="btn_sure_delete_new"
				data-dismiss="modal" onclick="save();"
			  >
				Yakin
			  </button>
			</div>
		  </div>
		</div>
	  </div>

    <!-- Modal Detail-->
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
										Nomor DO KRM
									</p>
								</td>
								<td style="padding-left: 10px; padding-right: 10px">
									<p class="text-bold">:</p>
								</td>
								<td>
									<p class="text-bold" id="lbl_nokrm"></p>
								</td>
							</tr>
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
										Approval Outgoing Nama
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
										Approval Outgoing Tgl
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
							<div class="table-responsive text-sm">
								<table id="tb_resultdetail" class="table table-bordered">
									<thead>
										<tr>
											<th>No.</th>
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
					</div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Close
                    </button>
                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">  
        function appendTableBody() {
            const btnSearch = $("#btn_search_krm");
            const inputValue = $("#kw");
            const inputDari = $("#dari");
            const inputSampai = $("#sampai");
            const rowscount = document.getElementById("tbody-data").getElementsByTagName("tr").length;
            const tebody = $("#tbody-data");

            btnSearch.prop('disabled', true);

            $.ajax({
                type: "GET",
                url: "./index.aspx/GetNomorKRM",
                data: {
                    keyword: `'${inputValue.val()}'`,
                    start: `'${inputDari.val()}'`,
                    end: `'${inputSampai.val()}'`,
                    loccode: `''`,
                },
                contentType: "application/json; charset=utf-8",
                beforeSend: function (request) {
                    request.withCredentials = false;
                },
                success: function (data) {
                    let response = data.d;
                    $('#tbody-data > tr').remove();
                    if (response.length > 0) {
                        $(response).each(function (index) {
                            $("#tbody-data").append(`
						<tr>
							<td>${index + 1}</td>
							<td style="display:none">${response[index].id}</td>
							<td><input type="checkbox" data-id="${response[index].id}" onclick="onCheckDetail(this);" class="check-claas"></td>
							<td>${response[index].nokrm}</td>
							<td>${response[index].lokasi}</td>
							<td>${response[index].kurir}</td>
							<td>${response[index].statusdo}</td>    
							<td>${response[index].approvaloutgoingnama}</td>
							<td>${response[index].approvaloutgoingtgl}</td>
							<td>${response[index].etd}</td>
							<td>${response[index].keterangan}</td>
							<td>
								<div style="display: flex; gap: 5px;">									
									<button type="button" class="btn btn-success btn-md" data-id="${response[index].id}" onclick="onDetail(this);">Detail</a>
								</div>
							</td>
						</tr>
						`);
                        });
                        toastr.success("Nomor KRM berhasil didaftarkan.");
                    }
					else {
						toastr.success("Data kosong.");
					}
                    btnSearch.prop("disabled", false);
                    inputValue.val("");

                    if (document.getElementById("tbody-data").getElementsByTagName("tr").length == 0) {
                        $("#btn_approve_do").hide();
                    }
                    else {
                        $("#btn_approve_do").show();
                    }
					
					var i = 0;
					$('#tbody-data > tr').each(function (index, teer) {
						let cekbok = $(teer).find('td').eq(2).find("input[type='checkbox']");
						if(cekbok.is(':checked')){
							i = i + 1;
						}
					});
					
					if(i > 0){
						$('#btn_approve_do').prop('disabled', false);
					}
					else{
						$('#btn_approve_do').prop('disabled', true);
					}
					
					if(i == document.getElementById("tbody-data").getElementsByTagName("tr").length){
						$('#checkbox-all').prop('checked', true);
					}
					else{
						$('#checkbox-all').prop('checked', false);
					}
					
					if(document.getElementById("tbody-data").getElementsByTagName("tr").length == 0){
						$('#checkbox-all').prop('checked', false);
					}
                },
                error: function (error) {
                    console.error(error);
                    btnSearch.prop("disabled", false);
                    toastr.error("Terjadi kesalahan!");
                },
            });
            btnSearch.prop('disabled', false);
        }

        function onDelete(e) {
            $(e).closest('tr').remove();
            $('#tbody-data > tr').each(function (index, tr) {
                $(tr).find('td').eq(0).text(index + 1);
            });

            if (document.getElementById("tbody-data").getElementsByTagName("tr").length == 0) {
                $("#btn_approve_do").hide();
            }
            else {
                $("#btn_approve_do").show();
            }
			
			var i = 0;
			$('#tbody-data > tr').each(function (index, teer) {
				let cekbok = $(teer).find('td').eq(2).find("input[type='checkbox']");
				if(cekbok.is(':checked')){
					i = i + 1;
				}
			});
			
			if(i > 0){
				$('#btn_approve_do').prop('disabled', false);
			}
			else{
				$('#btn_approve_do').prop('disabled', true);
			}
			
			if(i == document.getElementById("tbody-data").getElementsByTagName("tr").length){
				$('#checkbox-all').prop('checked', true);
			}
			else{
				$('#checkbox-all').prop('checked', false);
			}
			
			if(document.getElementById("tbody-data").getElementsByTagName("tr").length == 0){
				$('#checkbox-all').prop('checked', false);
			}
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
						$('#lbl_operator').text(resheader.approvaloutgoingnama);
						$('#lbl_operator_tgl').text(resheader.approvaloutgoingtgl);
						$('#lbl_departure').text(resheader.etd);
						$('#lbl_nokrm').text(resheader.nomordo);
						
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

        function save() {
			const id = [];
			
			$('#tbody-data > tr').each(function (index, teer) {
                let cekbok = $(teer).find('td').eq(2).find("input[type='checkbox']");
				if(cekbok.is(':checked')){
					id.push(cekbok.attr('data-id'));
				}
            });
			
            $.ajax({
                type: "POST",
                url: "./index.aspx/ApproveDO",
                data: JSON.stringify({ids : id}),
                contentType: "application/json; charset=utf-8",
                beforeSend: function (request) {
                    request.withCredentials = false;
                },
                success: function (result) {
                    if (result.d.status == 1) {
                        toastr.success("Data DO sukses diapprove incoming. Mohon dicek di menu report dokrm di menu incoming.");
						
						let newUrl = window.location.protocol + "//" + window.location.host + "/TransIncoming/Page/Report/DOKRM/";
						
						setTimeout(function(){
							window.location.href = newUrl;
						}, 1000);
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

        function onCheckAll(e) {
            if (!e.checked) {
                $('#btn_approve_do').prop('disabled', 'disabled');
				$('#tbody-data > tr').each(function (index, tr) {
					let cekbok = $(tr).find('td').eq(2).find("input[type='checkbox']");
					cekbok.prop('checked', false);
				});
            }
            else {
				$('#tbody-data > tr').each(function (index, tr) {
					let cekbok = $(tr).find('td').eq(2).find("input[type='checkbox']");
					cekbok.prop('checked', true);
				});

                $('#btn_approve_do').removeAttr('disabled');
            }
        }
		
		function onCheckDetail(e){
			var i = 0;
			$('#tbody-data > tr').each(function (index, teer) {
                let cekbok = $(teer).find('td').eq(2).find("input[type='checkbox']");
				if(cekbok.is(':checked')){
					i = i + 1;
				}
            });
			
			if(i > 0){
				$('#btn_approve_do').prop('disabled', false);
			}
			else{
				$('#btn_approve_do').prop('disabled', true);
			}
			
			if(i == document.getElementById("tbody-data").getElementsByTagName("tr").length){
				$('#checkbox-all').prop('checked', true);
			}
			else{
				$('#checkbox-all').prop('checked', false);
			}
		}

        $(document).ready(function () {
            var tglsekarang = new Date();
            const tgl = String("0" + tglsekarang.getDate()).slice(-2);
            const bulan = String("0" + (tglsekarang.getMonth() + 1)).slice(-2);
            const tahun = String(tglsekarang.getFullYear());
            $('#dari').val(tgl + '-' + bulan + '-' + tahun);
            $('#sampai').val(tgl + '-' + bulan + '-' + tahun);

            var rows = document.getElementById("tbody-data").getElementsByTagName("tr").length;
            if (rows == 0) {
                $("#btn_approve_do").hide();
            }
            else {
                $("#btn_approve_do").show();
            }

            toastr.options = {
                closeButton: true,
                progressBar: true,
                positionClass: "toast-bottom-center",
                showDuration: "300",
                hideDuration: "1000",
                timeOut: "5000",
            };

            let DATA_KRM_REGISTERED = [];
            const appendTable = (array) => {
                $("#tbody-data").empty();
                array.forEach((item, index) => {
                    $("#tbody-data").append(`
                <tr>
                    <td>${index + 1}</td>
                    <td>${item["Nomor"]}</td>    
                    <td>${item["Keterangan"]}</td>    
                    <td>${item["JumlahBarang"]}</td>
					<td>${item["LokasiAsal"]}</td>
                    <td>${item["LokasiTujuan"]}</td>    
                    <td>
                        <div style="display: flex; gap: 5px;">
                            <button type="button" class="btn btn-danger" data-toggle="modal" data-target="#myModal" data-nomor="${item["Nomor"]
                        }">Hapus</button>
                            <a 
                            target="_blank"
                            href="${window.location.origin + item["PathToDetail"]
                        }" class="btn btn-success btn-md">Detail</a>
                        </div>
                    </td>    
                </tr>
            `);
                });
                var rows = document.getElementById("tbody-data").getElementsByTagName("tr").length;
                if (rows == 0) {
                    $("#btn_approve_do").css("display", "none");
                }
                else {
                    $("#btn_approve_do").css("display", "block");
                }
            };

            $("table").on("click", "button", function (e) {
                const buttonElement = $(e.currentTarget);
                const dataNomor = buttonElement.data("nomor");
                const btnSure = $("#btn_sure_delete");
                btnSure.attr("data-nomor", dataNomor);
            });

            $("#btn_sure_delete").on("click", function (e) {
                const buttonElement = $(e.currentTarget);
                const nomor = buttonElement.data("nomor");

                const dataFilter = DATA_KRM_REGISTERED.filter(
                    (item) => item["Nomor"] !== nomor
                );

                DATA_KRM_REGISTERED = dataFilter;
                appendTable(DATA_KRM_REGISTERED);
                toastr.warning("Nomor KRM berhasil dihapus.");

            });
        });
    </script>
</asp:Content>
