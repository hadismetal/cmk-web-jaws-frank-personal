<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="DOKRM_index" %>

<asp:Content ContentPlaceHolderID="h1" runat="server"> DO KRM </asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
  Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <style>
	.inp {
		border:none;
		border-bottom: 1px solid #1890ff;
		outline: none;
		width: 90%;
		margin-right:10px
	}
  </style>
  <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  </form>

  <div
    style="
      border: 1px solid rgb(216, 216, 216);
      border-radius: 10px;
      margin: 3px;
      padding: 20px;
    "
  >
    <h2 style="border-bottom: 1px solid gray; width: max-content">
      Delivery Order - Form
    </h2>

    <div style="display: flex; flex-direction: column; gap: 10px">
      <div class="row" style="padding: 0px; margin: 0px">
        <div class="col-lg-2" style="padding: 0 0 0 0">
          <label for="operator_delivery_input">Operator Delivery Order</label>
          <input
            class="form-control"
            id="operator_delivery_input"
            runat="server"
            disabled
          />
        </div>
        <div class="col-lg-2" style="padding: 0 0 0 5px">
          <label for="tanggal_delivery_input">Estimation Time of Departure</label>
          <input
            class="form-control"
            id="tanggal_delivery_input"
            type="date"
            runat="server"
          />
        </div>
		<div class="col-lg-8" style="padding: 0 0 0 5px">
          <label for="kurir_delivery_input">Nama Kurir</label>
          <input class="form-control" id="kurir_delivery_input"/>
        </div>
		<div class="col-lg-2" style="padding: 10px 0 0 0;display:none">
          <label for="ddl_brand_location_asal">Brand Lokasi Asal</label>
          <select class="form-control"
          id="ddl_brand_location_asal"
		  onchange="getLocationAsal();"
		  disabled
		  >
			<option value="F" selected="true">Frank</option>
			<option value="M">Mondial</option>
			<option value="P">Palace</option>
          </select>
        </div>
		<div class="col-lg-2" style="padding: 10px 0 0 5px;display:none">
          <label for="ddl_tipe_location_asal">Tipe Lokasi Asal</label>
          <select class="form-control"
          id="ddl_tipe_location_asal"
          onchange="getLocationAsal();"
          >
			<option value="0">Warehouse</option>
			<option value="1">Outlet</option>
			<option value="2">Exhibition</option>
          </select>
        </div>
        <div class="col-lg-2" style="padding: 10px 0 0 5px;display:none">
          <label for="ddl_location_asal">Lokasi Asal</label>
          <select class="form-control"
          id="ddl_location_asal"
          >
          </select>
        </div>
		<div class="col-lg-2" style="padding: 10px 0 0 0">
          <label for="ddl_brand_location_tujuan">Brand Lokasi Tujuan</label>
          <select class="form-control"
          id="ddl_brand_location_tujuan"
		  onchange="getLocationTujuan();"
          >
			<option value="F">Frank</option>
			<option value="M">Mondial</option>
			<option value="P">Palace</option>
          </select>
        </div>
		<div class="col-lg-2" style="padding: 10px 0 0 5px">
          <label for="ddl_tipe_location_tujuan">Tipe Lokasi Tujuan</label>
          <select class="form-control"
          id="ddl_tipe_location_tujuan"
          onchange="getLocationTujuan();"
          >
			<option value="0">Warehouse</option>
			<option value="1">Outlet</option>
			<option value="2">Exhibition</option>
          </select>
        </div>
        <div class="col-lg-2" style="padding: 10px 0 0 5px">
          <label for="ddl_location_tujuan">Lokasi Tujuan</label>
          <select class="form-control"
          id="ddl_location_tujuan"
          >
          </select>
        </div>
		<div class="col-lg-11" style="padding: 0 0 0 0">
			<div class="form-group">
				<label for="keterangan_delivery_input">Keterangan</label>
				<textarea class="form-control"
					id="keterangan_delivery_input"
					rows="2"></textarea>
			</div>
        </div>
		<div class="col-lg-11" style="padding: 10px 0 0 0">
			<div class="form-group">
				<button type="button" class="btn btn-success" onclick="onAddKeteranganTitipan(this);">Add Keterangan Titipan</button>
			</div>
			<button type="button" onclick="onTestToJSON();" style="display:none">Check</button>
        </div>
      </div>
    </div>
  </div>

  <div
    style="
      border: 1px solid rgb(216, 216, 216);
      border-radius: 10px;
      margin: 3px;
      padding: 20px;
    "
  >
    <h2 style="border-bottom: 1px solid gray; width: max-content">
      Delivery Order - Detail
    </h2>

    <form id="form_input_krm">
      <div class="row" style="padding: 0; margin: 0">
        <div class="col-lg-7" style="display: flex; align-items: end; gap: 5px; padding: 0px">
          <div class="col-lg-5" style="padding: 0px">
            <label for="nomor_krm_input">Nomor KRM (Same Brand / Cross Brand)</label>
            <input
              class="form-control"
              id="nomor_krm_input"
              value=""
              placeholder="Masukkan nomor KRM..."
            />
          </div>
          <button class="btn btn-success btn-md col-lg-2" id="btn_search_krm" type="button" onclick="appendTableBody();">
            Cari
          </button>
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
        <tbody id="tbody-data"></tbody>
      </table>
    </div>
	<div>
		<button class="btn btn-primary btn-md" id="btn_save_do" type="button" data-toggle="modal" data-target="#myModal">
            Save
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
          Apakah anda yakin ingin membuat nomor DO KRM dengan List KRM berikut? Mohon periksa kembali krm tersebut agar tidak salah pilih krm. Jika anda sudah yakin, klik tombol 'Yakin', jika tidak klik tombol 'Batal'.
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

  <script type="text/javascript">
	function onTestToJSON(){
		const ids = [...document.querySelectorAll(".inp")].map(el => el.value);
		
		const arrai = [];
		
		for(let i = 0; i < ids.length; i++){
			if(ids[i] !== ""){
				arrai.push(ids[i]);
			}
		}
		
		
	}
	
	function onAddKeteranganTitipan(e){
		let outer = $(e).parent();
		outer.append(`<div class="form-inline">
			<input class="inp" placeholder="Input Keterangan Titipan....."></input><button type="button" onclick="onDeleteKeteranganTitipan(this);">Delete</button>
		</div>`);
	}
	
	function onDeleteKeteranganTitipan(e){
		let sibling = $(e).parent().children().remove();
	}
	
	function getLocationAsal(){
		const brand_asal = $("#ddl_brand_location_asal");
		const tipelokasi = $("#ddl_tipe_location_asal");
		$.ajax({
			 type: "GET",
			 url: "./index.aspx/GetLocationAsal",
			 data:{
				brand: `'${brand_asal.find(":selected").val()}'` ,
				tipelokasi: `'${tipelokasi.find(":selected").val()}'`
			 },
			 contentType: "application/json; charset=utf-8",
			 success: function (data,status,xhr) {
				var datastore = data.d;
				$("#ddl_location_asal").empty();
				$.each(datastore,function(i){
					$("#ddl_location_asal").append("<option value=" + datastore[i].id + ">" + datastore[i].nama + "</option>");
				});
			 },
			 error: function (jqXhr, textStatus, errorMessage) {
				toastr.warning(errorMessage);
			 },
		});
	}
	
	function getLocationTujuan(){
		const brand_tujuan = $("#ddl_brand_location_tujuan");
		const tipelokasi = $("#ddl_tipe_location_tujuan");
		$.ajax({
			type: "GET",
			url: "./index.aspx/GetLocationTujuan",
			data: {
				brand: `'${brand_tujuan.find(":selected").val()}'` ,
				tipelokasi: `'${tipelokasi.find(":selected").val()}'`
			},
			contentType: "application/json; charset=utf-8",
			success: function (data,status,xhr) {
				var datastore = data.d;
				$("#ddl_location_tujuan").empty();
				$.each(datastore,function(i){
					$("#ddl_location_tujuan").append("<option value=" + datastore[i].id + ">" + datastore[i].nama + "</option>");
				});
			},
			error: function (jqXhr, textStatus, errorMessage) {
				toastr.warning(errorMessage);
			},
		});
	  }
	  
	  function appendTableBody(){
		const btnSearch = $("#btn_search_krm");
        const inputValue = $("#nomor_krm_input");
		const inputtipeAsal = $("#ddl_tipe_location_asal");
		const inputbrandAsal = $("#ddl_brand_location_asal");
		const inputidAsal = $("#ddl_location_asal");
		const inputtipeTujuan = $("#ddl_tipe_location_tujuan");
		const inputbrandTujuan = $("#ddl_brand_location_tujuan");
		const inputidTujuan = $("#ddl_location_tujuan");
		const rowscount = document.getElementById("tbody-data").getElementsByTagName("tr").length;
		const tebody = $("#tbody-data");
		
		btnSearch.prop('disabled', true);
		
		$.ajax({
          type: "GET",
          url: "./index.aspx/GetNomorKRM",
          data: { 
			nomorKRM: `'${inputValue.val()}'` ,
			brandtujuan: `'${inputbrandTujuan.val()}'`,
			idlokasitujuan: `'${inputidTujuan.val()}'` ,
			tipelokasitujuan: `'${inputtipeTujuan.val()}'` ,
		  },
          contentType: "application/json; charset=utf-8",
          beforeSend: function (request) {
            request.withCredentials = false;
          },
          success: function (data) {						
            const response_message = data.d.error_message;
			let nokrm = 0;
			
			$('#tbody-data > tr').each(function(index, tr) {
				if($.trim(inputValue.val()) == $(tr).find('td').eq(1).text())
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
					$("#tbody-data").append(`
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
							<td style="display:none;">${inputbrandTujuan.val()}</td>    
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
			
			inputValue.val("");
			
			if(document.getElementById("tbody-data").getElementsByTagName("tr").length == 0){
			  $("#btn_save_do").hide();
			  $("#ddl_brand_location_tujuan").prop("disabled", false);
			  $("#ddl_tipe_location_tujuan").prop("disabled", false);
			  $("#ddl_location_tujuan").prop("disabled", false);
			}
			else{
			  $("#btn_save_do").show();
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
	  
	function onDelete(e){
		$(e).closest('tr').remove();
		$('#tbody-data > tr').each(function(index, tr) {
			$(tr).find('td').eq(0).text(index + 1);
		});
		
		if(document.getElementById("tbody-data").getElementsByTagName("tr").length == 0){
		  $("#btn_save_do").hide();
		  $("#ddl_brand_location_tujuan").prop("disabled", false);
		  $("#ddl_tipe_location_tujuan").prop("disabled", false);
		  $("#ddl_location_tujuan").prop("disabled", false);
		}
		else{
		  $("#btn_save_do").show();
		  $("#ddl_brand_location_tujuan").prop("disabled", true);
		  $("#ddl_tipe_location_tujuan").prop("disabled", true);
		  $("#ddl_location_tujuan").prop("disabled", true);
		}
	}
	
	function RequestDataKRM(krm, krm_detail){
		this.krm = krm;
		this.krm_detail = krm_detail;
	}

	function DataKrm_Detail(Tipe_Product, Is_Cross_Brand, Nomor, Keterangan, JumlahBarang, GrossWeight, Asal, Tujuan, EstimasiHarga,idbrandasal, idtipeasal, idlokasiasal, idbrandtujuan, idtipetujuan, idlokasitujuan){
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

	function DataKRM_Header(NamaKurir, Keterangan, ETD, OperatorOutgoingNama, KeteranganTitipan,brandtujuan,tipetujuan, LokasiTujuan){
		this.NamaKurir = NamaKurir;
		this.StatusDO = 1;
		this.Keterangan = Keterangan;
		this.ETD = ETD;
		this.OperatorOutgoingNama = OperatorOutgoingNama;
		this.OperatorOutgoingTgl = new Date();
		this.KeteranganTitipan = KeteranganTitipan;
		this.IDTipeTujuan = tipetujuan;
		this.IDLokasiTujuan = LokasiTujuan;
		this.brandtujuan = brandtujuan;
	}
	
	function save(){
		const operator_txt = $("#content_operator_delivery_input").val();
		const tanggal_delivery_input = $("#content_tanggal_delivery_input").val();
		const namakurir = $("#kurir_delivery_input").val();
		const inputtipeTujuan = $("#ddl_tipe_location_tujuan");
		const inputbrandTujuan = $("#ddl_brand_location_tujuan");
		const inputidTujuan = $("#ddl_location_tujuan");
		if(namakurir != ""){
			const keterangan = $("#keterangan_delivery_input").val();
			
			const ids = [...document.querySelectorAll(".inp")].map(el => el.value);
			const arrai = [];
			for(let i = 0; i < ids.length; i++){
				if(ids[i] !== ""){
					arrai.push(ids[i]);
				}
			}
			const keterangan_titipan = JSON.stringify(arrai);
			
			const header = new DataKRM_Header(namakurir, keterangan, tanggal_delivery_input, operator_txt, keterangan_titipan,inputbrandTujuan.val(),inputtipeTujuan.val(),inputidTujuan.val());
			
			const detail = [];
			
			$('#tbody-data > tr').each(function(index, tr) {
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
														
				
				detail.push(new DataKrm_Detail(tipe_prod, cross_brand, nokrm, keterang, jmlbrg, gw, asal, tuju, estimasi_harga,idbrandasal, idtipeasal, idlokasiasal, idbrandtujuan, idtipetujuan, idlokasitujuan));
			});
			const test = new RequestDataKRM(header, detail);
			const testStringify = JSON.stringify({data: test});			
			
			$.ajax({				
			  type: "POST",
			  url: "./index.aspx/SaveDataDO",
			  data: testStringify,
			  contentType: "application/json; charset=utf-8",
			  beforeSend: function (request) {
				request.withCredentials = false;
			  },
			  success: function (result) {
				
				if(result.d.status == 1){
					toastr.success("Data DO berhasil dibuat. Mohon dicek di menu approve dokrm di menu outgoing (khusus untuk SM / JC).");
					
					let newUrl = window.location.protocol + "//" + window.location.host + "/TransOutgoing/Page/Register/DOKRM/";
					
					setTimeout(function(){
						window.location.href = newUrl;
					}, 1000);
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
		else{
			toastr.error("Kurir Wajib Diisi!");
		}
	}
	
    $(document).ready(function () {
	let currDate = new Date();
      const offset = currDate.getTimezoneOffset();
      currDate = new Date(currDate.getTime() - offset * 60 * 1000);
      const currDateISO = currDate.toISOString().split("T")[0];
      const splittedDate = currDateISO.split("-");
      const startDateISO = `${splittedDate[0]}-${splittedDate[1]}-01`;

      $("#content_tanggal_delivery_input").val(currDateISO);
	  
	  getLocationAsal();
	  getLocationTujuan();
	  
	  var rows = document.getElementById("tbody-data").getElementsByTagName("tr").length;
	  if(rows == 0){
		  $("#btn_save_do").hide();
	  }
	  else{
		  $("#btn_save_do").show();
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
                            <button type="button" class="btn btn-danger" data-toggle="modal" data-target="#myModal" data-nomor="${
                              item["Nomor"]
                            }">Hapus</button>
                            <a 
                            target="_blank"
                            href="${
                              window.location.origin + item["PathToDetail"]
                            }" class="btn btn-success btn-md">Detail</a>
                        </div>
                    </td>    
                </tr>
            `);
        });
		var rows = document.getElementById("tbody-data").getElementsByTagName("tr").length;
		if(rows == 0){
			$("#btn_save_do").css("display","none");
		}
		else{
			$("#btn_save_do").css("display","block");
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
