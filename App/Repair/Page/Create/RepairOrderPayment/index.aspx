<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="MasterPage.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="DOKRM_index" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
  Form Reparasi
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
  Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  </form>

  <div id="loader">Loading...</div>

  <div class="container d-none flex-column" id="main-content">
    <div class="container border rounded p-5">
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

    <div class="container p-0 mt-5">
      <div class="row">
        <div class="col-12">
          <div class="border rounded p-5">
            <h1 style="border: none; padding-bottom: 10px">Info Produk</h1>
            <div class="row gx-5 justify-content-center align-items-center">
              <div class="col-3">
                <img
                  id="img-product"
                  class="border rounded p-5"
                  style="width: 100%"
                />
              </div>
              <div class="col">
                <div class="row gy-5">
                  <div class="col-6">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">No Invoice</h6>
                      <h5 class="text-lg fw-normal" id="Invoice"></h5>
                    </div>
                  </div>
                  <div class="col-6">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">PLU</h6>
                      <h5 class="text-lg fw-normal" id="PLU"></h5>
                    </div>
                  </div>
                  <div class="col-6">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Item</h6>
                      <h5 class="text-lg fw-normal" id="Item"></h5>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="container mt-5">
        <div class="row p-0">
          <div class="col-12 p-5 border rounded">
            <h1 style="border: none; padding-bottom: 10px">Biaya Reparasi</h1>
            <table class="table table-hover">
              <thead>
                <tr>
                  <th>Tipe Reparasi</th>
                  <th>Jasa Reparasi</th>
                  <th class="text-right">Biaya Reparasi</th>
                </tr>
              </thead>
              <tbody id="tbody-biaya-reparasi"></tbody>
            </table>
          </div>
        </div>
      </div>

      <div class="container mt-2" >
        <div class="row p-0">
          <div class="col-12 border rounded p-5">
            <h1 style="border: none; padding-bottom: 10px">Tambah Emas</h1>
            <table class="table table-hover">
              <thead>
                <tr class="text-right">
                  <th>Berat Sebelum</th>
                  <th>Berat Sesudah (Tanpa Batu)</th>
                  <th>Selisih</th>
                  <th>Batas Toleransi</th>
                  <th>Total Charge</th>
                  <!-- <th>Harga Emas</th> -->
                  <th>Biaya</th>
                </tr>
              </thead>
              <tbody id="tbody-biaya-tambah-emas"></tbody>
            </table>
          </div>
        </div>
      </div>

      <div class="container mt-2 d-none" id="content-tambah-batu" >
        <div class="row p-0">
          <div class="col-12 border rounded p-5">
            <h1 style="border: none; padding-bottom: 10px">Tambah Batu</h1>
            <table class="table table-hover">
              <thead>
                <tr>
                  <th>Stone</th>
                  <th class="text-right">Total Butir</th>
                  <th class="text-right">Carat Butir</th>
                  <th class="text-right">Total Carat</th>
                  <th class="text-right">Biaya</th>
                </tr>
              </thead>
              <tbody id="tbody-biaya-tambah-batu"></tbody>
            </table>
          </div>
        </div>
      </div>
      <div class="container mt-2">
        <div class="row">
          <div
            class="col-12 px-5 py-2 border rounded fw-bold"
            id="content-total-bayar"
          ></div>
        </div>
      </div>

      <div class="container mt-2">
        <div class="row">
          <div
            class="col-12 px-5 py-2 border rounded fw-bold"
            id="content-total-bayar2"
          ></div>
        </div>
      </div>

      <div class="container mt-2" id="content-tambah-discount" style="display: none;">
        <div class="row p-0">
          <div class="col-12 border rounded p-5">            
            <div class="col-md-4">
              <h1 style="border: none; padding-bottom: 10px" class="text-left">Discount</h1>                                       
            </div>
            <div class="col-md-8 d-flex justify-content-end" style="font-style: 12px;">
              <button type="button" class="btn btn-success" data-bs-toggle="modal" data-bs-target="#modalVoucher" id="btn-modal-voucher"><i class="icon-plus me-1"></i> Add Voucher</button>                          
            </div>
            <table class="table table-hover">
              <thead>
                <tr>
                  <th> </th>
                  <th> No.</th>
                  <th >Nama Voucher</th>                  
                  <th >Nominal</th>
                  <th class="text-right">Biaya</th>
                </tr>
              </thead>
              <tbody id="tbody-add-voucher"></tbody>
            </table>
          </div>
        </div>
      </div>

      <div class="container mt-2">
        <div class="row">
          <div
            class="col-12 px-5 py-2 border rounded fw-bold"
            id="content-total-bayar1"
          ></div>
        </div>
      </div>
    </div>
    <div class="row mt-5 justify-content-end" id="btn-content-modal">
      <div class="col-2">
        <button
          class="btn btn-primary btn-lg w-100"
          data-bs-toggle="modal"
          data-bs-target="#modalConfirmation"
          id="btn-modal-confirmation"
        >
          Bayar
        </button>
      </div>
    </div>
    <div class="row mt-5 justify-content-center">      
      <div id="span-error-stone" class="alert alert-danger text-center" style="display: none;" >
        Mohon lakukan Incoming Transfer untuk data stok batu baru (PLU Setting) sebelum melakukan pembayaran.
      </div>      
    </div>
  </div>

  <!-- Modal Add Voucher Discount -->
  <div
    class="modal fade"
    id="modalVoucher"
    tabindex="-1"
    aria-labelledby="modalVoucher"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-lg">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Add Voucher Discount</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <div class="row align-items-end">
            <div class="col-6">
              <label for="input-keyword" class="form-label fw-bold h5"
                >Kode Voucher:</label
              >
              <input
                type="text"
                class="form-control"
                id="input-keyword-voucher"
                placeholder="Masukkan Kode Voucher"
              />
            </div>            
            <div class="col-6">
              <button class="btn btn-primary btn-lg" id="btn-search-voucher">
                Enter
              </button>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-secondary"
            data-bs-dismiss="modal"
            id="btn-modal-close"
          >
            Batal
          </button>          
        </div>
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
          <p>
            Pastikan data yang anda masukkan sudah benar sebelum melakukan
            pembayaran!
          </p>
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

  <script type="text/javascript">
    const IMG_PRODUCT = $("#img-product");
    const MAIN_CONTENT = $("#main-content");
    const LOADER = $("#loader");
    let DATA_LOCATION = [];
    let DATA_SALES = [];
    let FRAME_COLOR = [];
    let DATA_ITEM = {};
    let DATA_STONE = [];
    let DATA_PAKET_REPARASI = [];
    let DATA_SATUAN_TAMBAHAN = [];
    let DATA_CUSTOMER = [];
    let VOUCHER_LIST =[];
    let BUTTON_SUBMIT_FORM = $("#btn-confirmation-submit");
    let BUTTON_FIND_VOUCHER = $("#btn-search-voucher");
    let BUTTON_REMOVE_VOUCHER = $("#btn-remove-voucher");
    const SELECT_FRAME_COLOR = $("#FrameColor");
    const SELECT_PAKET_UTAMA = $("#select-paket-utama");
    const SELECT_SATUAN_TAMBAHAN = $("#select-satuan-tambahan");
    const BUTTON_MODAL_CONFIRMATION = $("#btn-modal-confirmation");

    toastr.options = {
      closeButton: true,
      progressBar: true,
      positionClass: "toast-bottom-center",
      showDuration: "300",
      hideDuration: "1000",
      timeOut: "5000",
    };

    // Duplicate toastr options (if you want a separate config, use a different object)
    const customToastrOptions = {
      closeButton: true,
      progressBar: true,
      positionClass: "toast-top-center",
      showDuration: "300",
      hideDuration: "1000",
      timeOut: "5000",
    };

    function injectTextToElement({ id, text, isForm }) {
      const LIST_FORM_ID = [
        "BeratTimbangActual",
        "BeratTimbangAkhir",
        "SizeAwal",
        "SizeAkhir",
      ];
      const LIST_FORM_ID_WITH_NAME = [
        "GrafirLogo",
        "GrafirKadarEmas",
        "TesKeaslian",
        "FisikKeseluruhan",
      ];        

      if (id === "FrameColor") {
        SELECT_FRAME_COLOR.children().each((idx, elem) => {
          if (
            $(elem).attr("data-nama-frame-color").toLowerCase() ===
            text.toLowerCase()
          ) {
            SELECT_FRAME_COLOR.val($(elem).attr("data-id-frame-color"));
            SELECT_FRAME_COLOR.trigger("change");
          }
        });
        return;
      }

      if (!LIST_FORM_ID.includes(id) && !LIST_FORM_ID_WITH_NAME.includes(id)) {
        $("#" + id).text(text);
        return;
      }

      if (LIST_FORM_ID_WITH_NAME.includes(id)) {
        $(`[name="${id}"]`).each((index, elem) => {
          if ($(elem).val().toLowerCase() === text.toLowerCase())
            $(elem).prop("checked", true);
        });
        return;
      }

      $("#" + id).val(text);
    }

    function formatNumberToIDR(number) {
      const IDR = new Intl.NumberFormat("id-ID", {
        style: "currency",
        currency: "IDR",
      }).format(number);
      return IDR;
    }
    function removeVoucher(){
        VOUCHER_LIST = [];
        $('#tbody-add-voucher').html("");
        $("#content-total-bayar1").html(`
                    <div class="d-flex justify-content-between align-items-center">
                      <div>Total Bayar</div>
                      <div>${formatNumberToIDR(DEFAULT.totalNeedToPaid)}</div>
                    </div>
                `);
      }

      

    // Initializes while document is ready
    $(document).ready(function () {
      SELECT_FRAME_COLOR.select2();
      SELECT_PAKET_UTAMA.select2();
      SELECT_SATUAN_TAMBAHAN.select2();
    });

    // event handler
    $(document).ready(function () {
      BUTTON_SUBMIT_FORM.on("click", function () {
        const urlParams = new URLSearchParams(window.location.search);
        const id = urlParams.get("id");
        const status = 200;

        BUTTON_MODAL_CONFIRMATION.prop("disabled", true).text("Loading...");
        
          try {

            if (VOUCHER_LIST.length > 0) {                
                $.ajax({
                type: "POST",
                url: "./index.aspx/RedeemVoucher",
                data: JSON.stringify({
                  identifier: VOUCHER_LIST[0].IDCustomerStamps,
                  voucher_code: VOUCHER_LIST[0].KodeVoucher,            
                  store: VOUCHER_LIST[0].KodeLokasi
                }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                  const response = data["d"];            
                  
                  if ("error_code" in response && "detail" in response) {
                    toastr.warning(`${response.error_code}: ${response.detail}`);
                    BUTTON_MODAL_CONFIRMATION.prop("disabled", false);
                    BUTTON_MODAL_CONFIRMATION.prop("disabled", false).text("Bayar");
                    status = 500;
                    return ;
                  }

                  if ("error_message" in response) {
                    toastr.error(response.error_message);
                    BUTTON_MODAL_CONFIRMATION.prop("disabled", false);              
                    BUTTON_MODAL_CONFIRMATION.prop("disabled", false).text("Bayar");
                    status = 500;
                    return ;
                  }                                          
                  
                },
                error: function (error) {
                  showErrorAndReturn("Terjadi kesalahan saat melakukan redeem voucher!");
                  BUTTON_MODAL_CONFIRMATION.prop("disabled", false).text("Bayar");
                  status = 500;
                  return;
                },                          
             });
            }
          
          if (status == 200) {
            $.ajax({
                type: "POST",
                url: "./index.aspx/UpdateStatusPayment",
                data: JSON.stringify({
                  IDDocRepair: id,
                  DataVoucher: VOUCHER_LIST,
                }),
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                  if (status == 200) {                    
                    toastr.success("Data berhasil di submit, silahkan lanjutkan pembayaran!")
                    // toastr.success("Voucher discount berhasil di redeem")
                  }

                  setTimeout(() => {
                    window.location.href = "../RepairOrderPayment_Finish?id=" + id;  
                  }, 350);
                  
                },
                error: function (error) {
                  showErrorAndReturn(error);
                  console.error(error);
                  BUTTON_MODAL_CONFIRMATION.prop("disabled", false).text("Bayar");
                  return;
                },
            });
          }
          
          } catch (error) {
            showErrorAndReturn("Error redeeming voucher: " + error);
            console.error("Error redeeming voucher:", error);
            BUTTON_MODAL_CONFIRMATION.prop("disabled", false).text("Bayar");
            return;
          }        
      });          
      
      BUTTON_FIND_VOUCHER.on("click",()=>{
        const voucherNumber = $("#input-keyword-voucher").val().trim();
        const KodeStore=0;
        
        BUTTON_FIND_VOUCHER.prop("disabled", true).text("Loading...");
        if (VOUCHER_LIST.find((item)=>item["KodeVoucher"] == voucherNumber)) {
          BUTTON_FIND_VOUCHER.prop("disabled", false).text("Enter");
          $("#input-keyword-voucher").focus();
          return showErrorAndReturn("Voucher telah digunakan pada invoice ini.");          
        }else{
          $.ajax({
            type: "GET",
            url: "./index.aspx/GetVoucher",
            data: {
              store: `'${DATA_ITEM["KodeStore"]}'`,
              // store: 1,
              voucher_code: `"${voucherNumber}"`,
            },
            contentType: "application/json; charset=utf-8",
            success: function (data) {
              const response = data["d"];                                                               
              const dataCustomer = DATA_CUSTOMER[0];
              if (!response.user || (dataCustomer.Email == response.user.email || dataCustomer.NamaCustomer == response.user.name || dataCustomer.NomorTelpon == response.user.phone)) {                
              
                if (response.error_message) return showErrorAndReturn(response.error_message);
                if (typeof response.detail === "string") return showErrorAndReturn(response.detail);
                
                if (validateKey(response, "is_redeemable", "Returned data expected [is_redeemable] keys")) return;
                if (validateKey(response, "voucher", "Returned data expected [voucher] keys!")) return;                
                
                if (validateKeys(response.voucher, ["start_date", "end_date"], "Returned data expected [start_date] and [end_date] keys inside response.voucher!")) return;
                if (validateKeys(response.voucher, ["name", "extra_data"], "Returned data expected [name] and [extra_data] keys inside response.voucher!")) return;
                if (validateKeys(response.voucher.extra_data, ["voucher_type", "voucher_nominal"], "Returned data expected [voucher_type] and [voucher_nominal] keys inside response.voucher.extra_data!")) return;
                
                if (!response.is_redeemable) return showErrorAndReturn("Voucher is not redeemable!");
                
                const { start_date, end_date, name, extra_data } = response.voucher;
                const { voucher_type, voucher_nominal } = extra_data;
                const checkDate = checkDateRange(start_date, end_date);            
                
                if (typeof checkDate === "string") {
                  return showErrorAndReturn(checkDate);
                }

                
                
                let voucherName = name;
                let nominalPaidVoucher = 0;
                
                
                if (voucher_type === "repair_discount" && voucher_nominal === "full" && 
                (DEFAULT.totalNeedToPaid != (DEFAULT.totalNeedToPaidEmas+DEFAULT.totalNeedToPaidStone)) && !voucher_nominal.includes("%"))
                  nominalPaidVoucher = DEFAULT.totalNeedToPaidInstruksi;              

                  if (voucher_type === "repair_discount" && voucher_nominal === "all")
                  nominalPaidVoucher = DEFAULT.totalNeedToPaidInstruksi + DEFAULT.totalNeedToPaidEmas + DEFAULT.totalNeedToPaidStone;              
                if (voucher_type === "repair_discount" && voucher_nominal === "all")
                  nominalPaidVoucher =
                DEFAULT.totalNeedToPaidEmas + DEFAULT.totalNeedToPaidInstruksi;

                
                if ((
                  voucher_type === "repair_discount" && voucher_nominal.includes('%') &&
                      VOUCHER_LIST.find((item) => item["voucher"] != "full" && item["voucher"] != "voucher repair half" && item['voucher'] != "all" && item['voucher'].includes('%'))
                  )            
                ) {
                  toastr.error(
                    "Voucher Half Reparasi tidak boleh dicampur dengan jenis voucher pembayaran selain voucher amount atau poin customer!"
                  );
                  return;
                }
                
                if ((
                      voucher_type === "repair_discount" && voucher_nominal === "full" &&
                      VOUCHER_LIST.find((item) => item["voucher"] != "full" && item['voucher'] != "all" && item['voucher'].includes('%'))
                    )
                  ) {
                    toastr.error(
                      "Voucher Full Reparasi tidak boleh dicampur dengan jenis voucher pembayaran selain voucher amount atau poin customer!"
                    );
                    return;
                  }
                if ((
                    voucher_type === "repair_discount" &&
                    voucher_nominal === "all" &&
                    VOUCHER_LIST.find((item) => item["voucher"] != "full" && item['voucher'] != "all" && item['voucher'].includes('%'))
                  )
                ) {
                  toastr.error(
                    "Voucher Free Reparasi tidak boleh dicampur dengan jenis pembayaran lain!"
                  );
                  return;
                }
                if (voucher_type === "repair_payment") {
                  toastr.error(
                    "Tipe Voucher tersebut hanya bisa digunakan untuk cara bayar!"
                  );
                  return;
                }
                
                if (VOUCHER_LIST.length > 0) {
                  VOUCHER_LIST.forEach((e, i) => {                
                    VOUCHER_LIST[i]["KodeVoucher"] = voucherNumber;
                    VOUCHER_LIST[i]["voucher"] = response.voucher.name;
                    VOUCHER_LIST[i]["nominal"] = response.voucher.extra_data.voucher_nominal;
                    VOUCHER_LIST[i]["no"] = i+1;
                  });
                }else{
                  VOUCHER_LIST.push({
                    KodeVoucher : voucherNumber,
                    voucher: response.voucher.name,
                    nominal: response.voucher.extra_data.voucher_nominal,
                    no: 1
                  });
                }
                
                let html =``;
                let nominalDiscount=0;

                if (VOUCHER_LIST[0].voucher.includes("all")) {
                  nominalDiscount = DEFAULT.totalNeedToPaid;
                }else if (VOUCHER_LIST[0].voucher.includes("full")) {
                  nominalDiscount = DEFAULT.totalNeedToPaidInstruksi;
                }else{
                  const percentage = parseFloat(VOUCHER_LIST[0].nominal.replace('%', '')) / 100;
                  // nominalPaidVoucher = DEFAULT.totalNeedToPaidInstruksi * percentage;                
                  nominalDiscount = DEFAULT.totalNeedToPaidInstruksi * percentage;                
                  // nominalDiscount = DEFAULT.totalNeedToPaid/2;
                }
                html +=`
                <tr>
                      <td><button type="button" class="btn btn-danger" onclick="removeVoucher()">Remove</button></td>
                      <td>${VOUCHER_LIST[0].no}</td>
                      <td>${VOUCHER_LIST[0].KodeVoucher}</td>
                      <td>${VOUCHER_LIST[0].nominal}</td>
                      <td class="text-right">-${formatNumberToIDR(parseFloat(nominalDiscount))}</td>
                    </tr>              
                `;

                $("#content-total-bayar1").html(`
                    <div class="d-flex justify-content-between align-items-center">
                      <div>Total Bayar</div>
                      <div>${formatNumberToIDR(DEFAULT.totalNeedToPaid - nominalDiscount)}</div>
                    </div>
                `);

                VOUCHER_LIST[0].NominalDiscount = parseFloat(nominalDiscount);
                VOUCHER_LIST[0].IDCustomerStamps = dataCustomer.IDCustomerStamps;
                VOUCHER_LIST[0].KodeLokasi = dataCustomer.KodeLokasi;                                
                
                $('#tbody-add-voucher').html(html);              
                $('#btn-modal-close').click();
                $('#input-keyword-voucher').val('');             
              }else{
                return showErrorAndReturn("Customer is invalid for this voucher code!");
              }
            },
            error: function (error) {
              console.error(error);
            },
            complete: function (data) {
              BUTTON_FIND_VOUCHER.prop("disabled", false).text("Enter");
            },
          });
     
        }
      });      
      
      function redeemVoucher(dataV = { VOUCHER_LIST }){        
        console.log([dataV,dataV[0].IDCustomerStamps]);
        $.ajax({
          type: "POST",
          url: "./index.aspx/RedeemVoucher",
          data: JSON.stringify({
            identifier: dataV[0].IDCustomerStamps,
            voucher_code: dataV[0].KodeVoucher,            
            store: dataV[0].KodeLokasi
          }),
          contentType: "application/json; charset=utf-8",
          success: function (data) {
            const response = data["d"];            
            
            if ("error_code" in response && "detail" in response) {
              toastr.warning(`${response.error_code}: ${response.detail}`);
              BUTTON_MODAL_CONFIRMATION.prop("disabled", false);
              BUTTON_MODAL_CONFIRMATION.prop("disabled", false).text("Bayar");
              return false;
            }

            if ("error_message" in response) {
              toastr.error(response.error_message);
              BUTTON_MODAL_CONFIRMATION.prop("disabled", false);              
              BUTTON_MODAL_CONFIRMATION.prop("disabled", false).text("Bayar");
              return false;
            }              
            return true;          
            
          },
          error: function (error) {
            showErrorAndReturn("Terjadi kesalahan saat melakukan redeem voucher!");
            BUTTON_MODAL_CONFIRMATION.prop("disabled", false).text("Bayar");
            return;
          },
        });

      }
    });

      function onEditDetailPaymentType(arr) {      
        const jenispocer = arr.find(
          (item) => (item["voucher"])        
        );      
        const ifNotFreeRepairExists = arr.find(
          (item) => (item["voucher"] !== "all" || item["voucher"] !== "full")        
        );      
        const totalPaid = arr.reduce(
          (accumulator, calculator) => (accumulator += calculator.nominal),
          0
        );
        
        // DEFAULT = {
        //   ...DEFAULT,
        //   totalPaid: totalPaid,
        //   totalNeedToPaid: DEFAULT.totalPayment - totalPaid,
        // };
      }

      function showErrorAndReturn(message) {
        toastr.options = customToastrOptions;
        toastr.error(message);
        return true;
      }

      function validateKey(obj, key, message) {
        if (!(key in obj)) return showErrorAndReturn(message);
        return false;
      }

      function validateKeys(obj, keys, message) {
        const missing = keys.filter(k => !(k in obj));
        if (missing.length > 0) return showErrorAndReturn(message);
        return false;
      }
    
    function checkDateRange(startDateString, endDateString) {
      const startDate = new Date(startDateString);
      const endDate = new Date(endDateString);
      const currentDate = new Date();

      if (currentDate < startDate) {
        return "Not active yet";
      } else if (currentDate > endDate) {
        return "Expired";
      }

      return true;
    }

    // Fetch Initial Data
    $(document).ready(function () {
      
      const urlParams = new URLSearchParams(window.location.search);
      const id = urlParams.get("id");

      $.ajax({
        type: "GET",
        url: "./index.aspx/GetDetailRepair",
        data: {
          IDDocRepair: id,
        },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];          

          DATA_ITEM = response["docRepairDetail"];
          const package = response["docRepairPackage"];
          const parcel = response["docRepairParcel"];
          const otherCost = response["docRepairOther"];
          DATA_CUSTOMER = response["docRepairCustomer"];          
          const ISLokasiStone = response["docRepairLokasiStoneSetting"];

            let totalStoneNotReady = 0;

            ISLokasiStone.forEach((e, itemIndex) => {
              if (e['IDProduct'] == 0) {
                totalStoneNotReady += 1;
              }
            });


            if (totalStoneNotReady > 0) {
              $('#span-error-stone').show();
              BUTTON_MODAL_CONFIRMATION.hide();
            }

          const urlParams = new URLSearchParams(window.location.search);
          const id = urlParams.get("id");

          if (DATA_ITEM["Status"] > 7) {
            window.location.href = "../RepairOrderPayment_Finish?id=" + id;
            return;
          }

          if (![4,7].includes(DATA_ITEM["Status"])) {
          // if (DATA_ITEM["Status"] !== 7 || DATA_ITEM["Status"] !== 4) {
            window.location.href = "../../Summary.aspx";
          }

          if (DATA_ITEM["Status"] == 8) BUTTON_MODAL_CONFIRMATION.remove();

          IMG_PRODUCT.attr(
            "src",
            "https://" +
              window.location.hostname +
              "/Images.aspx?path=" +
              DATA_ITEM["ImgProduct"]
          );
          IMG_PRODUCT.attr("alt", DATA_ITEM["PLU"]);

          Object.keys(DATA_ITEM).forEach((item) => {
            injectTextToElement({
              id: item,
              text: DATA_ITEM[item],
            });
          });

          const hargaTotalPBSetting = parcel.reduce(
            (accumulator, calculator) => {
              accumulator +=
                calculator["HargaSettingRound"] + calculator["HargaPBRound"];
              return accumulator;
            },
            0
          );

          let biayaPerbaikan = 0;
          let biayaPerbaikanInstruksi = 0;
          let DiskonBiayaJasa = 0;
          
          var jenisReparasi = package.reduce((accumulator, calculator) => {
            const { NamaPackage, NamaTipePackage, Biaya, BiayaR, BiayaJual } =
              calculator;

            let BiayaJualNew = BiayaJual;

            if (NamaPackage.includes("Bongkar/Pasang Batu"))
              BiayaJualNew = hargaTotalPBSetting;
            
            biayaPerbaikan += parseFloat(BiayaJualNew);            
            biayaPerbaikanInstruksi += parseFloat(BiayaJualNew);

            accumulator += `
                  <tr>
                    <td>${NamaTipePackage}</td>
                    <td>${NamaPackage}</td>
                    <td class="text-right">${formatNumberToIDR(
                      parseFloat(BiayaJualNew)
                    )}</td>
                  </tr>
                `;
            return accumulator;
          }, "");
          jenisReparasi += otherCost.reduce((accumulator, calculator) => {
            const { NomorTrans, NamaPackage, CostOther} =
              calculator;
            if (NamaPackage != '-'){              
              biayaPerbaikan += parseFloat(CostOther);            
              biayaPerbaikanInstruksi += parseFloat(CostOther);
              accumulator += `
                    <tr>
                      <td>Lain-Lain</td>
                      <td>${NamaPackage}</td>
                      <td class="text-right">${formatNumberToIDR(
                        parseFloat(CostOther)//before r and mu
                      )}</td>
                    </tr>
                  `;
              return accumulator;
            }
          }, "");
          DiskonBiayaJasa = biayaPerbaikan;
          $("#tbody-biaya-reparasi").append(jenisReparasi);

          const selisihBerat = (
            DATA_ITEM["BeratAkhirTanpaBatu"] - DATA_ITEM["BeratTimbangActual"]
          ).toFixed(2);

          const isPaidSelisih =
            DATA_ITEM["TotalGramLebih"] > 0
              ? "class='text-danger'"
              : "class='text-success'";
              
          biayaPerbaikan += DATA_ITEM["HargaBayar"];

          // <td>${formatNumberToIDR(DATA_ITEM["HargaEmas"])}</td>
          // <td>${DATA_ITEM["BeratTimbangAkhir"]} gr</td>

          $("#tbody-biaya-tambah-emas").append(`
                <tr class="text-right">
                  <td>${DATA_ITEM["BeratTimbangActual"]} gr</td>
                  <td>${DATA_ITEM["BeratAkhirTanpaBatu"]} gr</td>
                  <td ${isPaidSelisih}>${selisihBerat} gr</td>
                  <td>${DATA_ITEM["BatasToleransi"].toFixed(2)} gr</td>
                  <td>${DATA_ITEM["TotalGramLebih"]} gr</td>
                  <td>${formatNumberToIDR(DATA_ITEM["HargaBayar"])}</td>
              </tr>
              `);

          const TBODY_BIAYA_TAMBAH_BATU = $("#tbody-biaya-tambah-batu");
          

          $("#content-tambah-batu").addClass("d-none");
          if (parcel.length > 0) {
            $("#content-tambah-batu").removeClass("d-none");
            const parcelHtml = parcel.reduce((accumulator, calculator) => {
              const {
                Stone,
                Clarity,
                HargaTotalRupiahRound,
                TotalButir,
                TotalCarat,
                CaratButir,
              } = calculator;

              biayaPerbaikan += HargaTotalRupiahRound;

              accumulator += `
              <tr>
                <td>${Stone} (${Clarity})</td>
                <td class="text-right">${TotalButir}</td>  
                <td class="text-right">${CaratButir}</td>  
                <td class="text-right">${TotalCarat}</td>  
                <td class="text-right">${formatNumberToIDR(
                  HargaTotalRupiahRound
                )}</td>  
              </tr>
            `;

              return accumulator;
            }, ``);

            TBODY_BIAYA_TAMBAH_BATU.append(parcelHtml);
          }

          MAIN_CONTENT.removeClass("d-none").addClass("d-flex");

          $("#content-total-bayar").html(`
                  <div class="d-flex justify-content-between align-items-center">
                    <div>Total </div>
                    <div>${formatNumberToIDR(biayaPerbaikan)}</div>
                  </div>
              `);
          $("#content-total-bayar2").html(`
                  <div class="d-flex justify-content-between align-items-center">
                    <div>Total Diskon Biaya Jasa</div>
                    <div class="text-red">- ${formatNumberToIDR(DiskonBiayaJasa)}</div>
                  </div>
              `);
          $("#content-total-bayar1").html(`
                  <div class="d-flex justify-content-between align-items-center">
                    <div>Total Bayar</div>
                    <div>${formatNumberToIDR(biayaPerbaikan - DiskonBiayaJasa)}</div>
                  </div>
              `);

          DEFAULT = {
            totalPayment: biayaPerbaikan-DiskonBiayaJasa,
            totalBeforeDiscount: biayaPerbaikan,
            totalPaid: 0,
            totalNeedToPaid: biayaPerbaikan-DiskonBiayaJasa,
            totalNeedToPaidInstruksi : biayaPerbaikanInstruksi
          };
          console.log(DEFAULT)
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          LOADER.hide();
        },
      });
    });

  </script>
</asp:Content>
