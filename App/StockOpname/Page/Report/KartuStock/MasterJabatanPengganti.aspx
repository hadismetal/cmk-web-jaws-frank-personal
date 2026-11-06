<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="MasterJabatanPengganti.aspx.cs" inherits="Page_MasterJabatanPengganti" %>
<asp:Content ContentPlaceHolderID="h1" runat="server">
  Stock Opname
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
  Report JR on Duty
</asp:Content>

<asp:content contentplaceholderid="content" runat="server">
    <div >
		<form id="Form1" runat="server" class="form-inline">
			<input type="text" id="namaJR" runat="server" class="form-control" style="display:none;" />
			<div class="form-group">
				<div class="text-muted">
					Location Type :
				</div>
				<asp:dropdownlist id="locationtype" runat="server" cssclass="form-control" width="200">				
				</asp:dropdownlist>
			</div>
			<div class="form-group">
				<label for="input-tgl-awal" class="form-label fw-bold"
				>Tanggal Awal</label
				>
				<input class="form-control" id="input-tgl-awal" type="date" />
			</div>
			<div class="form-group">
				<label for="input-tgl-hingga" class="form-label fw-bold"
				>Tanggal Hingga</label
				>
				<input class="form-control" id="input-tgl-hingga" type="date" />
			</div>
			<div class="form-group">
				<div class="hidden-xs">&nbsp;</div>
				<div>
					<a type="button" href="#" class="btn btn-primary" id="btnFind">
						<i class="icon-search"></i>
						Search
					</a>
				</div>
			</div>
		</form>
	</div>
	<div class="row gx-2 py-3">
      <div class="col-12">
        <div class="border rounded p-5">
          <table class="table table-hover tableOpname">
            <thead>
              <tr class="text-center">                
                <th>No</th>                
                <th>Nama Lokasi</th>
                <th>Nama</th>
                <th>Nama Jabatan</th>
                <th>Periode</th>                
                <th class="text-center">Status</th>                
				<th>Action</th>
              </tr>
            </thead>
            <tbody id="tbody-list-do"></tbody>
          </table>
        </div>
      </div>
    </div>


	<div class="modal fade" id="modalKeterangan" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1" aria-labelledby="modalKeteranganLabel" aria-hidden="true">
		<div class="modal-dialog">
			<div class="modal-content">
			<div class="modal-header">
				<h5 class="modal-title" id="modalKeteranganHeader"></h5>
				<button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
			</div>
			<div class="modal-body">				
				<div class="form-group">
					<label for="namaJR">Nama :</label>					
                    <select class="form-control brand-input" name="ListJR" id="ListJR">
                    </select>
				</div>
				<div class="form-group">
					<label for="Jabatan">Tipe Jabatan :</label>					
                    <input type="text" id="Jabatan" disabled class="form-control" />
				</div>
				<div class="form-group">
					<label for="Shift">Shift :</label>
					<input class="form-control" id="Shift" type="input" disabled/>
				</div>				
				<div class="form-group">
					<label for="StartDate">Start Date :</label>
					<input class="form-control" id="StartDate" type="date" disabled/>
				</div>				
			</div>
			<div class="modal-footer">
								
				
			</div>
			</div>
		</div>
	</div>

	<script type="application/javascript">
		$(document).ready(()=>{
			toastr.options = {
				closeButton: true,
				progressBar: true,
				positionClass: "toast-bottom-center",
				showDuration: "300",
				hideDuration: "1000",
				timeOut: "5000",
			};
			$('#select-satuan-tambahan').select2({				
				placeholder:'Pilih Display...'
			});



			let currDate = new Date();
			const offset = currDate.getTimezoneOffset();
			currDate = new Date(currDate.getTime() - offset * 60 * 1000);
			const currDateISO = currDate.toISOString().split("T")[0];
			const splittedDate = currDateISO.split("-");
		
			const startDateISO = `${splittedDate[0]}-${splittedDate[1]}-01`;
			$('#input-tgl-awal').val(startDateISO);							

			$('#input-tgl-hingga').val(currDateISO);
			let dataDisplay = $('#select-satuan-tambahan');
			
			const IDLokasi = $('#<%=locationtype.ClientID %>').val();						
			

			$('#btnFind').on('click',(e)=>{				
				e.preventDefault();

				const IDLokasi1 = $('#<%=locationtype.ClientID %>').val();										
				let tglStart = $('#input-tgl-awal').val()
				let tglhingga = $('#input-tgl-hingga').val()
				let TBODY_LIST_DO = $('#tbody-list-do');
				const parameters = {
					IDLokasi: IDLokasi1,										
					TglStart: `'${tglStart}'`,
          			TglEnd: `'${tglhingga}'`,
				};

				$.ajax({
					url: "./MasterJabatanPengganti.aspx/GetData",
					contentType: "application/json; charset=utf-8",
					type: "GET",
					data: parameters,
					success: function (data) {
					const response = data["d"];
                    console.log(response)
					TBODY_LIST_DO.empty();

					if (response.length === 0 ) {
						toastr.error("Data tidak ditemukan!");
						return;
					}
					let IDUser1 = 0;
					let no =1;
					const additionalString = response.reduce(
						(accumulator, calculator) => {
						const {
							ID,
                            IDLokasi,
							NamaLokasi, 							
							EfektifDate,
							NamaShift,
							Status,							
							TipeJabatan,
                            Nama,
							IDUser
						} = calculator;

						let badge=``;
						let active = `<span class="badge bg-success">Active</span>`;						
						let deactivate = `<span class="badge bg-secondary">InActive</span>`;						
						let badgeStatus = Status == 1 ? active : deactivate;																		                        
						IDUser1 = IDUser;


						accumulator += `
						<tr>
							<td class='text-center'>${no++}</td>							
							<td class='text-center'>${NamaLokasi}</td>                            
							<td class='text-center'>${Nama}</td>
							<td class='text-center'>${TipeJabatan}</td>
							<td class='text-center'>${EfektifDate} (${NamaShift})</td>
							<td class='text-center'>${badgeStatus}</td>
							<td class="text-center">
                                <div>
                                    <a href="#" class="btn btn-sm btn-primary" data-bs-toggle="modal" data-bs-target="#modalKeterangan"
                                    data-start="${EfektifDate}" data-iduser="${IDUser}" data-nama="${Nama}" data-jabatan="${TipeJabatan}"
                                     data-namalokasi="${NamaLokasi}" data-shiftcode="${NamaShift}" data-id="${ID}" data-status="${Status}">
                                        <i class="icon-pen"></i> Update
                                    </a>
                                </td>                            
						</tr>
						`;						

						return accumulator;
						},
						""
					);

					TBODY_LIST_DO.append(additionalString);
					getListJR(IDLokasi1, IDUser1)
					},
					error: function (error) {
					console.error(error);
					},
					complete: function (data) {
						// e.attr('disabled', false).html(
						// 	`<a type="button" href="#" class="btn btn-primary" id="btnFind">
						// 		<i class="icon-search"></i>
						// 		Search
						// 	</a>`
						// );
					},
				});


				// $('#tbody-list-do').empty();				
			})
			
			$('#modalKeterangan').on('show.bs.modal',(e)=>{				
				const div = $(e.relatedTarget);				
				const ID = div.data('id');				
				const IDUserold = div.data('iduser');
                $('#StartDate').val(toHtmlDate(div.data('start')));
				$('#Shift').val(div.data('shiftcode'));
                $('#namaJR').val(div.data('nama'));
                $('#Jabatan').val(div.data('jabatan'));                
                $('#modalKeteranganHeader').text(`Update JR on Duty - ${div.data('namalokasi')}(${div.data('shiftcode')})`);				

				let button = `
				<button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
				<button type="button" class="btn btn-primary" data-id="${ID}" data-iduserold = "${IDUserold}" id="btn-update-data">Save</button>`;
				$('#modalKeterangan .modal-footer').html(button);
			})

			$(document).on('click', '#btn-update-data', function (e) {
				$(this).attr('disabled', "disabled")
				e.preventDefault();
				const div = $(this);	
				const ID = div.data('id');				
				const IDUser = $('#ListJR').val()
				const IDUser1 = div.data('iduserold')

				if (IDUser == IDUser1) {
					toastr.warning('Tidak ada perubahan pada data tersebut!!')
					$(this).removeAttr('disabled','disabled')
					return;
				}else{				
					$.ajax({					
						type: "POST",
						url: "./MasterJabatanPengganti.aspx/UpdateData",
						data: JSON.stringify({ ID: ID, IDUser: IDUser}),
						contentType: "application/json; charset=utf-8",
						dataType: "json",
						success: function (response) {                    
							let responseMsg = response.d[0];
							if(responseMsg.Status == 200){
								toastr.success(`${responseMsg.Message}`);
								setTimeout(() => {
									window.location.reload();
								}, 500);
							} else {
								toastr.error(`${responseMsg.Message}`);
								$(this).removeAttr('disabled', "disabled")
								return; 
							}
							
						},
						error: function (xhr, status, error) {                    
							toastr.error(`Error saving : ${error}`);
							$(this).removeAttr('disabled', "disabled")							
						}
					});				
				}			
			})
			
		
		})
        function toHtmlDate(str) {
            // str format = DD/MM/YYYY
            const parts = str.split('/');
            return `${parts[2]}-${parts[1]}-${parts[0]}`;
        }

		function getListJR(IDLokasi, IDUser){
            $.ajax({
                type: "POST",
                url: "MasterJabatanPengganti.aspx/GetListJR",
                data: JSON.stringify({ IDLokasi: IDLokasi, IDUser: IDUser }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {                    
                    var listJR = response.d;
                    let html=``;
                    listJR.forEach((item) => {
                        html+=`<option value="${item.IDUser}" ${item.IDUser == IDUser ? "selected" : ""}>${item.Nama}</option>`;
                    })
                    $('#ListJR').html(html)
                    // toastr.success(`${response.d}`);
                    // setTimeout(() => {
                    //     window.location.href = "Mstable.aspx";
                    // }, 500);
                },
                error: function (xhr, status, error) {
                    $("#errorMsg").text("Error get data : " + error).show();
                    toastr.error(`Error get data : ${error}`);
                    
                }
            });
        }
        

		
	</script>
</asp:content>

