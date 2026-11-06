<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="MasterPage.master" AutoEventWireup="true"
CodeFile="detail.aspx.cs" Inherits="DOKRM_detail" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
  Report Reparasi
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server"> Detail </asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  </form>

  <div id="loader">Loading...</div>

  <div class="d-none flex-column" id="main-content">
    <div class="container-fluid border rounded p-5">
      <div class="row">
        <div class="col-7">
          <h1 style="border: none; padding-bottom: 10px">Detail Store</h1>
          <div class="row">
            <div class="col d-flex flex-column gap-1">
              <h6 class="text-md fw-bold">Store</h6>
              <h5 class="text-lg fw-normal" id="Store"></h5>
            </div>
            <div class="col d-flex flex-column gap-1">
              <h6 class="text-md fw-bold">JP / JR</h6>
              <h5 class="text-lg fw-normal" id="NamaSales"></h5>
            </div>
            <div class="col d-flex flex-column gap-1">
              <h6 class="text-md fw-bold">Customer</h6>
              <h5 class="text-lg fw-normal" id="NamaCustomer"></h5>
            </div>
          </div>
        </div>
      </div>
    </div>    

    <div
      class="container-fluid border rounded p-5 mt-5 row"
      id="content-detail-instruksi"
    >
      <h1 style="border: none; padding-bottom: 10px">Detail instruksi</h1>
      <div class="col-6">
        <div class="table-responsive mt-4 content-paket-utama">
          <h3 style="border: none; padding-bottom: 10px">Paket Utama</h3>
          <table class="table table-hover">
            <thead>
              <tr>
                <th>No</th>
                <th>Instruksi</th>              
              </tr>
            </thead>
            <tbody id="tbody-detail-mainInstruction"></tbody>
          </table>
        </div>          
      </div>     
      <div class="col-6">
        <div class="table-responsive mt-4 content-paket-lain">
          <h3 style="border: none; padding-bottom: 10px">Lain - lain</h3>
          <table class="table table-hover">
            <thead>
              <tr>
                <th>No</th>
                <th>Instruksi</th>              
              </tr>
            </thead>
            <tbody id="tbody-detail-lain"></tbody>
          </table>
        </div>          
      </div>
       <div class="col-12">
        <div class="table-responsive mt-4 content-satuan-tambahan">
          <h3 style="border: none; padding-bottom: 10px">Satuan Tambahan</h3>
          <table class="table table-hover">
            <thead>
              <tr>
                <th>No</th>
                <th>Instruksi</th>                                       
              </tr>
            </thead>
            <tbody id="tbody-detail-additionalInstruction"></tbody>
          </table>
        </div>
      </div>
    </div>
    <div
      class="container-fluid border rounded p-5 mt-5"
      id="content-detail-stone"
    >
      <h1 style="border: none; padding-bottom: 10px">Detail Stone</h1>
      <div class="table-responsive mt-0">
        <table class="table table-hover">
          <thead>
            <tr>
              <th>No</th>
              <th>Stone</th>
              <th>Total Butir</th>              
              <th>Total Carat</th>                                          
            </tr>
          </thead>
          <tbody id="tbody-detail-stone"></tbody>
        </table>
      </div>
    </div>
    <div
      class="container-fluid border rounded p-5 mt-5"
      id="content-detail-estimasi"
    >
      <h1 style="border: none; padding-bottom: 10px">Detail Estimasi</h1>
      <div class="table-responsive mt-0">
        <table class="table table-hover">
          <thead>
            <tr>
              <th>No</th>
              <th>Tipe</th>
              <th class="text-right">Harga</th>                            
            </tr>
          </thead>
          <tbody id="tbody-detail-estimasi"></tbody>
        </table>
      </div>
    </div>

    <div class="row mt-3 justify-content-end">
      <div class="col-2">
        <button
          class="btn btn-outline-secondary justify-content-endry btn-lg w-100"
          id="btn-cancel-form"
          data-bs-toggle="modal"
          data-bs-target="#modalConfirmationCancel"
        >
          Batal
        </button>
      </div>
      <div class="col-2">
        <button
          class="btn btn-danger btn-lg w-100"
          data-bs-toggle="modal"
          data-bs-target="#modalConfirmation-decline"
          id="btn-modal-confirmation-decline"
        >
          Decline
        </button>
      </div>
      <div class="col-2">
        <button
          class="btn btn-success btn-lg w-100"
          data-bs-toggle="modal"
          data-bs-target="#modalConfirmation"
          id="btn-modal-confirmation"
        >
          Approve
        </button>
      </div>
    </div>
  </div>

   <!-- Modal Confirmation -->  
  <div
    class="modal"
    id="modalConfirmation"
    tabindex="-1"
    aria-labelledby="modalConfirmationLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Confirmation</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <b>Apakah anda yakin?</b>
          <p>Pastikan data yang tertera sudah benar dan customer sudah setuju dengan perubahan tersebut!</p>
        </div>
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-secondary"
            data-bs-dismiss="modal"
          >
            Batal
          </button>
          <button
            type="button"
            class="btn btn-primary"
            id="btn-confirmation-submit"
            data-bs-dismiss="modal"
          >
            Yakin
          </button>
        </div>
      </div>
    </div>
  </div>
  
  <div
    class="modal"
    id="modalConfirmation-decline"
    tabindex="-1"
    aria-labelledby="modalConfirmationdeclineLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Confirmation</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <b>Apakah anda yakin?</b>          
        </div>
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-secondary"
            data-bs-dismiss="modal"
          >
            Batal
          </button>
          <button
            type="button"
            class="btn btn-primary"
            id="btn-confirmation-decline"
            data-bs-dismiss="modal"
          >
            Yakin
          </button>
        </div>
      </div>
    </div>
  </div>

  <div
    class="modal"
    id="modalConfirmationCancel"
    tabindex="-1"
    aria-labelledby="modalConfirmationCancelLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Confirmation</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <b>Apakah anda yakin untuk meninggalkan halaman?</b>          
        </div>
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-secondary"
            data-bs-dismiss="modal"
          >
            Batal
          </button>
          <button
            type="button"
            class="btn btn-primary"
            id="btn-cancel-submit"
            data-bs-dismiss="modal"
          >
            Yakin
          </button>
        </div>
      </div>
    </div>
  </div>

  <script type="text/javascript">
    const IMG_PRODUCT = $("#img-product");
    const MAIN_CONTENT = $("#main-content");
    const LOADER = $("#loader");
    let BUTTON_SUBMIT_FORM = $("#btn-confirmation-submit");
    let BUTTON_DECLINE_FORM = $("#btn-confirmation-decline");
    let BUTTON_CANCEL_FORM = $("#btn-cancel-submit");
    const paketUtama =[];
    const satuanTambahan =[];
    const ongkosLain = [];
    const dataEstimasi =[];

    BUTTON_SUBMIT_FORM.on("click", () => SaveData(tipe = "Approve"));      
    BUTTON_DECLINE_FORM.on("click", () => SaveData(tipe = "Decline"));
    BUTTON_CANCEL_FORM.on("click", () => Cancel());
    

    toastr.options = {
      closeButton: true,
      progressBar: true,
      positionClass: "toast-bottom-center",
      showDuration: "300",
      hideDuration: "1000",
      timeOut: "5000",
    };

    function formatNumberToIDR(number) {
      const IDR = new Intl.NumberFormat("id-ID", {
        style: "currency",
        currency: "IDR",
      }).format(number);
      return IDR;
    }

    function injectTextToElement({ id, text }) {
      if (id === "BeratTimbangActual" || id === "BeratTimbangPotong") {
        $("." + id).text(text);
        return;
      }

      if (id === "ImgPicture") {
        $("#" + id).attr("src", text);
        return;
      }

      if (id === "Blemish") {
        $("#" + id).text(text === "1" ? "Basic" : "Non-Basic");
        return;
      }

      $("#" + id).text(text);
    }

    function setContentPackage({ text, element }) {
      return `
        <div class="col d-flex flex-column gap-1 mt-3">
          <h6 class="text-md fw-bold">${text}</h6>
          <ul>${element}</ul>
        </div>
      `;
    }

    // Fetch Initial Data
    $(document).ready(function () {
      const urlParams = new URLSearchParams(window.location.search);
      const id = urlParams.get("id");
      const idBrand = urlParams.get("idbrandasal");
      

      $.ajax({
        type: "GET",
        url: "./detail.aspx/GetDetailRepair",
        data: {
          IDDocRepair: id,
          IDBrandAsal: idBrand,
        },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];
          const detail = response["docRepairDetail"];
          const package = response["docRepairDetailPackage"];
          const Stone = response["docRepairDetailStone"];
          const Estimasi = response["docRepairDetailEstimasi"];                                        

          Object.keys(detail).forEach((item) => {
            injectTextToElement({
              id: item,
              text: detail[item],
            });
          });
          
          Object.keys(package).forEach((item) => {              
            if (package[item]["TipeReparasi"] == '1') {
              paketUtama.push(package[item]);
            }else if(package[item]["TipeReparasi"] == '2'){
              satuanTambahan.push(package[item]);
            }else{
              ongkosLain.push(package[item]);
            }
              
          });

          Object.keys(Estimasi).forEach((item) => {                          
              dataEstimasi.push(Estimasi[item]);                          
          });

          MAIN_CONTENT.removeClass("d-none").addClass("d-flex");                   
          
          paketUtama.Length > 0 ? $("#content-paket-utama").show() : $("#content-paket-utama").hide();  
          satuanTambahan.Length > 0 ? $("#content-paket-utama").show() : $("#content-paket-utama").hide();            
          ongkosLain.Length > 0 ? $("#content-paket-lain").show() : $("#content-paket-lain").hide();            

          if (Stone.Length > 0) {
            $("#content-detail-stone").show();  
          }

          const mainInstruction = paketUtama.reduce((accumulator, calculator, index) => {
            const {
              IDDocRepair, TipeReparasi, JenisReparasi
            } = calculator;
            
            accumulator += `
                  <tr>
                    <td>${index + 1}</td>
                    <td>${JenisReparasi}</td>
                  </tr>
                `;
            return accumulator;
          }, "");

          const additionalInstruction = satuanTambahan.reduce((accumulator, calculator, index) => {
            const {
              IDDocRepair, TipeReparasi, JenisReparasi
            } = calculator;
            
            accumulator += `
                  <tr>
                    <td>${index + 1}</td>
                    <td>${JenisReparasi}</td>
                  </tr>
                `;
            return accumulator;
          }, "");

          const otherFee = ongkosLain.reduce((accumulator, calculator, index) => {
            const {
              IDDocRepair, TipeReparasi, JenisReparasi
            } = calculator;
            
            accumulator += `
                  <tr>
                    <td>${index + 1}</td>
                    <td>${JenisReparasi}</td>
                  </tr>
                `;
            return accumulator;
          }, "");

          const estimasi = Estimasi.reduce((accumulator, calculator, index) => {
            let {
              IDDocRepair, TipeEstimasi, HargaMinIDR, HargaMaxIDR,               
              harga = HargaMinIDR==HargaMaxIDR||HargaMinIDR==0 ? formatNumberToIDR(HargaMaxIDR) 
              : formatNumberToIDR(HargaMinIDR) + " - "+ formatNumberToIDR(HargaMaxIDR)
            } = calculator;            
            
            accumulator += `
                  <tr>
                    <td>${index + 1}</td>
                    <td>${TipeEstimasi}</td>
                    <td class="text-right">${harga}</td>
                  </tr>
                `;
            return accumulator;
          }, "");
          
          const stone = Stone.reduce((accumulator, calculator, index) => {
            const {
              IDStone, Tipe, KodeParcel, Parcel, TotalButir, TotalCarat
            } = calculator;
            
            accumulator += `
                  <tr>
                    <td>${index + 1}</td>
                    <td>${Parcel}</td>
                    <td>${TotalButir}</td>                    
                    <td>${TotalCarat}</td>                    
                  </tr>
                `;
            return accumulator;
          }, "");

          $("#tbody-detail-mainInstruction").append(mainInstruction);
          $("#tbody-detail-additionalInstruction").append(additionalInstruction);
          $("#tbody-detail-lain").append(otherFee);
          $("#tbody-detail-stone").append(stone);
          $("#tbody-detail-estimasi").append(estimasi);
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          LOADER.hide();
        },
      });
    });

    function SaveData(tipe){      
      const urlParams = new URLSearchParams(window.location.search);
      const id = urlParams.get("id");
      const idBrand = urlParams.get("idbrandasal");

      BUTTON_SUBMIT_FORM.attr("disabled", true);
      BUTTON_DECLINE_FORM.attr("disabled", true);
      BUTTON_CANCEL_FORM.attr("disabled", true);
      console.log(tipe)

      $.ajax({
        type: "POST",
        url: "./detail.aspx/submitChanges",
        data: JSON.stringify({          
            DataConfirmation : {
              IDDocRepair: id,
              IDBrandAsal: idBrand,
              TipeKonfirmasi: tipe,
              PaketUtama: paketUtama,
              satuanTambahan: satuanTambahan,
              estimasi : dataEstimasi
            }
        }),
        contentType: "application/json; charset=utf-8",
        success: function (data, textStatus, jqXHR) {
            const response = data["d"];                                    
            if (jqXHR.status === 200){    
            if (tipe == 'Approve') {
              toastr.success("Estimasi biaya & detail reparasi berhasil dikonfirmasi");
            }else{
              toastr.success("Estimasi biaya & detail reparasi tidak dikonfirmasi");
            }
                
                setTimeout(() => {
                  location.href = "./index.aspx";
                }, 5000);
                
            } else {
                toastr.error("Terjadi kesalahan saat mengkonfirmasi!");
                return;
            }           
            
        },
        error: function (error, textStatus, jqXHR) {
          console.error(error);
          toastr.error("Terjadi kesalahan saat mengkonfirmasi!");
          $("#btn-modal-confirmation").attr("disabled","disabled");
          BUTTON_SUBMIT_FORM.attr("disabled", false);
          BUTTON_DECLINE_FORM.attr("disabled", false);
          BUTTON_CANCEL_FORM.attr("disabled", false);              
        },
        complete: function (data, textStatus, jqXHR) {              
          
        },
      });
    
    }

    function Cancel(){
      location.href = "./index.aspx";
    }
  </script>
</asp:Content>
