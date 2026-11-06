<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="index.aspx.cs" inherits="Page_Summary" %>
<asp:Content ContentPlaceHolderID="h1" runat="server">
  Stock Opname
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
  Approval Kartu Stock 
</asp:Content>

<asp:content contentplaceholderid="content" runat="server">
	<div id="loader">Harap Tunggu, Sedang Menyiapkan Data</div>
    <div id = "stock-opname">
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
				<div class="text-muted">
					Shift :
				</div>
				<asp:dropdownlist id="shift" runat="server" cssclass="form-control" width="200">					
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
				<tr>                
					<th>No</th>
					<th>Tipe Lokasi</th>
					<th>Nama Lokasi</th>
					<th>Kode Shift</th>
					<th>Tanggal</th>
					<th class="text-center">Status Opname</th>
					<th class="text-center">Status Approval</th>                
					<th>Action</th>
				</tr>
				</thead>
				<tbody id="tbody-list-do"></tbody>
			</table></br>
			<div id="startROW" style="display:none;">1</div> 
			<div class="btn-group" role="group" id="paginationGRP" ></div>				
        </div>
      </div>
    </div>

	
	<script type="application/javascript">
		$(document).ready(()=>{
			$('#select-satuan-tambahan').select2({				
				placeholder:'Pilih Display...'
			});
			function showLoader() {
                document.getElementById("loader").style.display = "grid";
                document.getElementById("stock-opname").style.display = "none";
            }

            function hideLoader() {
                document.getElementById("loader").style.display = "none";
                document.getElementById("stock-opname").style.display = "block";
            }
			
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
				
				let startRowValue = $('#startROW').text();
				let pageStart = startRowValue
        		let pageSize=10;
				let maxDisplay = 10;
				let start = $('#input-tgl-awal').val()
				let end = $('#input-tgl-hingga').val()

				e.preventDefault();				
				// e.attr('disabled', true).html(
				// 	'<i class="icon-spinner2 spinner"></i> Loading...'
				// );
				const IDLokasi1 = $('#<%=locationtype.ClientID %>').val();						
				let TipeLokasi = $('#<%= locationtype.ClientID %>').find(':selected').attr('tipelokasi')
				let KodeShift = $('#<%= shift.ClientID %>').val();
				let TBODY_LIST_DO = $('#tbody-list-do');
				const parameters = {
					IDLokasi: IDLokasi1,
					TipeLokasi: !TipeLokasi?9:TipeLokasi,
					KodeShift: `'${KodeShift}'`,
					start : `'${start}'`,
					end:`'${end}'`,
					PageSize : pageSize,
					PageStart: pageStart
				};
				TBODY_LIST_DO.empty();				

				$.ajax({
					url: "./index.aspx/GetDataOpname",
					contentType: "application/json; charset=utf-8",
					type: "GET",
					data: parameters,
					success: function (data) {
					const datas = data["d"];
					const response = datas["DataTampung"];
					const totalan = datas["Total"];
					TBODY_LIST_DO.empty();

					if (response.length === 0 ) {
						toastr.error("Data tidak ditemukan!");
						return;
					}
					let no =((pageStart-1) * pageSize) + 1;
					const additionalString = response.reduce(
						(accumulator, calculator) => {
						const {
							ID,
							TipeLokasi,
							NamaLokasi, 
							KodeShift, 
							tgl,
							flagStats,
							flagDetail,
							Status,
							tglApprove,
							Approver, 
							IsLate, 
							IsSelisihDJ,
							IsSelisihPG,
							IsSelisihLD
						} = calculator;

						let match = `<span class="badge bg-success">Sesuai</span>`;
						let notmatch = `<span class="badge bg-danger">Tidak Sesuai</span>`;
						let waitingapprove = `<span class="badge bg-warning">Menunggu Persetujuan</span>`;
						let badgeColor = IsSelisihDJ == 0 && IsSelisihPG == 0 && IsSelisihLD == 0 ? 'success' : 'danger';
						let badgean = flagStats == 2 ? (IsSelisihDJ+IsSelisihPG+IsSelisihLD) == 0 ? match : notmatch : waitingapprove;
						let badgeDJ = flagStats == 2 ? IsSelisihDJ == 0 ? match : notmatch : waitingapprove;
						let badgePG = flagStats == 2 ? IsSelisihPG == 0 ? match : notmatch : waitingapprove;
						let badgeLD = flagStats == 2 ? IsSelisihLD == 0 ? match : notmatch : waitingapprove;

						if(flagStats == 2){
							if(IsLate == 0){
								badge = `<span class="badge bg-${badgeColor} text-wrap ">Disetujui oleh ${Approver} (${tglApprove})</span>`;
							}else{
								badge = `<span class="badge bg-danger text-wrap ">Disetujui oleh ${Approver} (${tglApprove})</span>`;
							}
						}else{
							if (tgl) {
								
							}
							badge = `<a href='./KartuStock.aspx?id=${ID}&KodeShift=${KodeShift}&NamaLoc=${NamaLokasi}' class="btn btn-lg btn-outline-success me-2"><i class="icon-eye-open me-2"></i> Approve</a>`;
						}

						let flagging = flagDetail == 0 ? `<span class="badge bg-danger me-2">Harap selesaikan Stock Opname terlebih dahulu sebelum di setujui</span>`:flagStats == 1 ? badge : '';
									// <i class="icon-eye"></i> Detail`: `<span class="badge bg-warning">Opname Sedang Berlangsung</span>`
						// let flagging2 = flagDetail != 0 ? `<a href='./Detail.aspx?id=${ID}&KodeShift=${KodeShift}&NamaLoc=${NamaLokasi}' target="_blank" class="btn btn-lg btn-primary"><i class="icon-info me-2"></i> Detail</a>`:'';
						let flagging2 = '';
						accumulator += `
						<tr>
							<td>${no++}</td>
							<td>${TipeLokasi}</td>
							<td>${NamaLokasi}</td>
							<td>${KodeShift}</td>
							<td>${tgl}</td>
							<td class="text-center">${badgean}</td>
							<td class="text-center">${
								flagStats == 1
									? '<span class="badge bg-warning">'+ Status +'</span>'
									: badge
								}</td>		
							<td>
								<div class="d-flex">
									${flagging}
									${flagging2}
								</div>
							</td>					
						</tr>
						`;

						return accumulator;
						},
						""
					);

					TBODY_LIST_DO.append(additionalString);
					generatePaging(pageStart, totalan, pageSize, maxDisplay)
					},
					error: function (error) {
					console.error(error);
					},
					complete: function (data) {						
					},
				});


				// $('#tbody-list-do').empty();				
				function generatePaging(currentPages, total, pageSize, maxDisplay){
					currentPages = currentPages <= 0 ? 1 : currentPages; 
                						
					const totalPages = Math.ceil(total / pageSize);
					let startPage = Math.max(1, currentPages - Math.floor(maxDisplay / 2));
					let endPage   = Math.min(totalPages, startPage + (maxDisplay - 1));
					
					let wrap = ``;
					let html = ``;
					html +=`<a class='btn btn-secondary btn-lg btn-page' data-page="${parseInt(currentPages)-1}" ${currentPages == 1 ? 'Disabled' : ''}>Prev</a>`
					if (startPage > 1)
					{
						html +=`<a class='btn btn-secondary btn-lg btn-page'  data-page="1">1</a>`
						if (startPage > 1)
						{
							html += "<span readonly class='btn btn-default'>...</span>";
						}
					}
					for (let i = startPage; i <= endPage; i++) {
						html +=`<a class='btn btn-secondary btn-lg btn-page'  data-page="${i}" ${i == currentPages ?'disabled':''}>${i}</a>`
					}					

					if (endPage < totalPages)
					{
						if (endPage < totalPages - 1)
						{
							html += "<span readonly class='btn btn-default'>...</span>";
						}
						html +=`<a class='btn btn-secondary btn-lg btn-page'  data-page="${totalPages}" >${totalPages}</a>`
					}
					html +=`<a class='btn btn-secondary btn-lg btn-page' data-page="${parseInt(currentPages) + 1}" ${currentPages == totalPages ? 'Disabled' : ''}>Next</a>`
					$('#paginationGRP').html(html)
				}

				$(document).on('click', '.btn-page', function (e) {
					e.preventDefault();

					let page = $(this).data('page').toString().trim();

					$('#startROW').html(page);
					$('#btnFind').click();
				});
				
			})			
			
			

			function filterTable(){
				let loc = $('#<%=locationtype.ClientID %>').val();
				
				$.ajax({
					type: "GET",
					url: "./index.aspx/GetDisplayFront",
					data: { IDLokasi: IDLokasi},
					contentType: "application/json; charset=utf-8",
					dataType: "json",
					success: function (response) {                    
						let data = response['d'];
						data.forEach(item => {
							addRowFrontDisplay(item.ID, item.NamaDisplay);
							counters['front']++;
						});
						
					},
					error: function (xhr, status, error) {                    
						toastr.error(`Error saving : ${error}`);
						
					}
				});
			}

			$('#StartOpname').on('click',(e)=>{
				e.preventDefault();
				let dataMeja = [];
				let Lokasi = document.getElementById("<%=locationtype.ClientID %>");
				var optionLokasi = Lokasi.options[Lokasi.selectedIndex];

				let IDLokasi = optionLokasi.value;
				let NamaLokasi = optionLokasi.text
				let TipeLokasi = optionLokasi.getAttribute("tipelokasi");
				
				let shift = $('#<%=shift.ClientID %>').val();
				let namaJR = $('#<%=namaJR.ClientID %>').val();
				const dataSatuan = $(dataDisplay.select2('data'))
// console.log([IDLokasi, NamaLokasi, TipeLokasi]);
				dataSatuan.each((item,el)=>{
					dataMeja.push(parseInt(el.id));				
				})				
				window.location.href = `./KartuStock.aspx?loc=${IDLokasi}&shift=${shift}&display=${dataMeja.toString()}&locName=${NamaLokasi}&tipeLokasi=${TipeLokasi}&namaJR=${namaJR}`;

			})
		})

		
	</script>
</asp:content>

