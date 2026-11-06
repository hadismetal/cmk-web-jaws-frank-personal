<%@ page language="C#" masterpagefile="MasterPage.master" autoeventwireup="true" codefile="index.aspx.cs" inherits="Page_Summary" %>
<asp:Content ContentPlaceHolderID="h1" runat="server">
  Register 
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
  Kartu Stock
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
				<div class="text-muted">
					Shift :
				</div>
				<asp:dropdownlist id="shift" runat="server" cssclass="form-control" width="200">					
				</asp:dropdownlist>
			</div>
			<div class="form-group" id="masterDisplay">
				<div class="d-flex flex-column">
                  <label for="select-satuan-tambahan" class="form-label text-muted"
                    >Meja Display</label
                  >
                  <select                    				  
                    id="select-satuan-tambahan"
                    multiple
                    placeholder="Pilih Display..."
                  ></select>
                </div>			
			</div>
			<div class="form-group">
				<div class="hidden-xs">&nbsp;</div>
				<div>
					<a type="button" href="#" class="btn btn-primary" id="StartOpname">
						<i class="icon-search"></i>
						Start Opname
					</a>
				</div>
			</div>
		</form>
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
			let typeUser = 0;

			
			$('#select-satuan-tambahan').select2({				
				placeholder:'Pilih Display...'
			});
			let dataDisplay = $('#select-satuan-tambahan');
			let dataShift = $('#<%=shift.ClientID %>');
			
			const IDLokasi = $('#<%=locationtype.ClientID %>').val();			

			$('#<%=locationtype.ClientID %>').on('change',(e)=>{
				$('#<%=locationtype.ClientID %>').val();				
				getMasterShift( { IDLokasi:parseInt($('#<%=locationtype.ClientID %>').val()) } )
				getMasterTable( { IDLokasi:parseInt($('#<%=locationtype.ClientID %>').val()) } )
			})

			const getMasterTable = ({ IDLokasi}) => {
				$.ajax({
					type: "GET",
					url: "./index.aspx/GetMasterTable",
					data: {					
					IDLokasi: IDLokasi
					},
					contentType: "application/json; charset=utf-8",
					success: function (data) {
						let DATA_DISPLAY = data['d'];
						const select2PackageRepair = dataDisplay.select2().empty();
					
						const mapLocation = DATA_DISPLAY.forEach(
						(
							{			
								ID,					
								NamaDisplay,								
							},
							index
						) => {
							const optionDisplay = new Option(
								NamaDisplay,
								ID,
								false,
								false
						);

						$(optionDisplay)		
							.attr("data-id", ID)					
							.attr("data-nama", NamaDisplay);

						select2PackageRepair.append(optionDisplay);
						}
					);

					},
					error: function (error) {
					console.error(error);
					},
				});
			};

			getMasterTable({ IDLokasi : parseInt(IDLokasi) });
			
			

			// function filterTable(){
			// 	let loc = $('#<%=locationtype.ClientID %>').val();
				
			// 	$.ajax({
			// 		type: "GET",
			// 		url: "./index.aspx/GetDisplayFront",
			// 		data: { IDLokasi: IDLokasi},
			// 		contentType: "application/json; charset=utf-8",
			// 		dataType: "json",
			// 		success: function (response) {                    
			// 			let data = response['d'];
			// 			data.forEach(item => {
			// 				addRowFrontDisplay(item.ID, item.NamaDisplay);
			// 				counters['front']++;
			// 			});
						
			// 		},
			// 		error: function (xhr, status, error) {                    
			// 			toastr.error(`Error saving : ${error}`);
						
			// 		}
			// 	});
			// }

			// const getMasterShift = ({ IDLokasi}) => {
			// 	$.ajax({
			// 		type: "GET",
			// 		url: "./index.aspx/GetMasterShift",
			// 		data: {					
			// 		IDLokasi: IDLokasi
			// 		},
			// 		contentType: "application/json; charset=utf-8",
			// 		success: function (data) {
			// 			let DATA_SHIFT = data['d'];
			// 			// const select2PackageRepair = dataDisplay.empty();
			// 			let html=``;
			// 			// let idshift = 0;

			// 			DATA_SHIFT.forEach((e,i)=>{
			// 				html +=`<option value="${e.ID}" >${e.Nama}</option>`;
			// 				// idshift = e.ID;
			// 			})
			// 			dataShift.html(html)
						
			// 		},
			// 		error: function (error) {
			// 		console.error(error);
			// 		},
			// 	});
			// };
			const getMasterShift = ({ IDLokasi}) => {
				return $.ajax({  // ← Add 'return'
					type: "GET",
					url: "./index.aspx/GetMasterShift",
					data: {					
					IDLokasi: IDLokasi
					},
					contentType: "application/json; charset=utf-8",
					success: function (data) {
						let DATA_SHIFT = data['d'];
						let html=``;

						DATA_SHIFT.forEach((e,i)=>{
							html +=`<option value="${e.ID}" >${e.Nama}</option>`;
						})
						dataShift.html(html);
					},
					error: function (error) {
					console.error(error);
					},
				});
			};

			getMasterShift({IDLokasi:parseInt(IDLokasi)}).then((e)=>{
				checkUser($('#<%=shift.ClientID %>').val());
			})	
			
			function checkUser(shift){
				
				$.ajax({
					type: "GET",
					url: "./index.aspx/CheckUser",
					contentType: "application/json; charset=utf-8",
					data: {					
						Shift: shift
					},
					dataType: "json",
					success: function (response) {                    
						let data = response['d'];
						typeUser = data;
						if (data == 1) {
							$('#masterDisplay').hide();														
						}
						
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
				let kartuStockWindow = window.open('', '_blank');


				dataSatuan.each((item,el)=>{
					dataMeja.push(String(el.text));
				})
				if (typeUser > 0) {
					kartuStockWindow.location.href = `./KartuStock.aspx?loc=${IDLokasi}&shift=${shift}&display=0&locName=${NamaLokasi}&tipeLokasi=${TipeLokasi}&namaJR=${namaJR}&typeUser=${typeUser}`;
				}else{
					if (dataMeja.length > 0) {
						kartuStockWindow.location.href = `./KartuStock.aspx?loc=${IDLokasi}&shift=${shift}&display=${dataMeja.toString()}&locName=${NamaLokasi}&tipeLokasi=${TipeLokasi}&namaJR=${namaJR}&typeUser=${typeUser}`;
					}else{
						toastr.error("Harap pilih display yang ingin anda mulai opname!")
					}
				}
				
			})
		})

		
	</script>
</asp:content>

