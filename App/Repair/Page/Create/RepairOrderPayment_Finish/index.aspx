<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="MasterPage.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="DOKRM_index" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
  <style>
    .bg-gray-custom {
      background-color: rgb(234, 234, 234);
    }

    .payment-type:hover {
      background-color: rgb(234, 234, 234);
    }

    .payment-type.bg-gray-custom:hover {
      background-color: rgb(214, 214, 214);
    }

    .cursor-disabled {
      cursor: not-allowed;
    }
  </style>
  Form Register
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
  Pembayaran Reparasi
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  </form>

  <div id="loader">Loading...</div>

  <div class="d-none flex-column" id="main-content">
    <div class="container border rounded p-5">
      <div class="row">
        <div class="col-12">
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

      <div class="container mt-2" id="content-tambah-batu">
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
            id="content-total-bayar3"
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
      <div class="container mt-2">
        <div class="row">
          <div
            class="col-12 px-5 py-2 border rounded fw-bold"
            id="content-total-bayar"
          ></div>
        </div>
      </div>      
      
      <div class="container mt-2" id="content-tambah-discount" style="display: none;">
        <div class="row p-0">
          <div class="col-12 border rounded p-5">            
            <div class="col-md-4">
              <h1 style="border: none; padding-bottom: 10px" class="text-left">Discount</h1>                                       
            </div>            
            <table class="table table-hover">
              <thead>
                <tr>                                    
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

      <div class="container mt-2 d-none" id="totalbayar-lunas">
        <div class="row">
          <div
            class="col-12 px-5 py-2 border rounded fw-bold"
            id="content-total-bayar1"
          ></div>
        </div>
      </div>      

      <div class="container mt-2" id="content-parent-payment">
        <div class="row">
          <div class="col-7" id="content-payment-tipe">
            <div class="border rounded p-5">
              <h1 style="border: none; padding-bottom: 10px">
                Pilih Pembayaran
              </h1>
              <div id="content-choose-payment-types" class="row g-2"></div>
              <div id="selected-payment-type-content"></div>
            </div>
          </div>
          <div class="col-5">
            <div
              class="border rounded p-5 d-flex flex-column"
              id="detail-payment-content"
            >
              <div>
                <h4 class="fw-bold">Detail Pembayaran</h4>
                <table class="table">
                  <tbody>
                    <tr>
                      <td style="width: 60%" class="fw-bold">Status</td>
                      <td>
                        <div class="badge bg-danger" id="badge-status">
                          Belum Sesuai Nominal
                        </div>
                      </td>
                    </tr>
                  </tbody>
                </table>
              </div>

              <div class="mt-3">
                <h4 class="fw-bold">Biaya Reparasi</h4>
                <table class="table">
                  <tbody>
                    <tr>
                      <td style="width: 60%" class="fw-bold">Total Biaya</td>
                      <td id="td-total-biaya" class="fw-bold"></td>
                    </tr>
                  </tbody>
                </table>
              </div>

              <div class="mt-3">
                <h4 class="fw-bold">Cara Bayar</h4>
                <table class="table">
                  <tbody id="tbody-cara-bayar"></tbody>
                </table>
              </div>

              <table class="table">
                <tbody>
                  <tr>
                    <td style="width: 60%" class="fw-bold">Total Terbayar</td>
                    <td id="td-total-terbayar" class="fw-bold"></td>
                  </tr>
                  <tr>
                    <td style="width: 60%" class="fw-bold">Sisa Pembayaran</td>
                    <td id="td-sisa-terbayar" class="fw-bold"></td>
                  </tr>
                </tbody>
              </table>
              <table class="table">
                <tbody>
                  <tr>
                    <td style="width: 60%" class="fw-bold">Total Pembayaran</td>
                    <td id="td-total-pembayaran" class="fw-bold"></td>
                  </tr>
                </tbody>
              </table>

              <button
                class="btn btn-lg btn-primary"
                id="btn-modal-confirmation"
                disabled
                data-bs-toggle="modal"
                data-bs-target="#submitPayment"
              >
                Submit Pembayaran
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>

  <!-- Modal -->
  <div
    class="modal fade"
    tabindex="-1"
    aria-labelledby="submitPaymentLabel"
    id="submitPayment"
    aria-hidden="true"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="submitPaymentLabel">Submit Pembayaran</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <h5 class="fw-bold">
            Apakah anda yakin ingin submit pembayaran ini?
          </h5>
        </div>
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-outline-danger"
            data-bs-dismiss="modal"
          >
            Batal
          </button>
          <button
            type="button"
            data-bs-dismiss="modal"
            class="btn btn-primary"
            id="btn-payment-confirmation"
          >
            Yakin
          </button>
        </div>
      </div>
    </div>
  </div>

  <!-- Modal -->
  <div
    class="modal fade"
    tabindex="-1"
    aria-labelledby="submitPaymentLabel"
    id="deleteModal"
    aria-hidden="true"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="submitPaymentLabel">Hapus Cara Bayar</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <h5 class="fw-bold">
            Apakah anda yakin ingin menghapus cara bayar ini?
          </h5>
        </div>
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-outline-danger"
            data-bs-dismiss="modal"
          >
            Batal
          </button>
          <button
            type="button"
            data-bs-dismiss="modal"
            class="btn btn-primary"
            id="btn-delete-confirmation"
          >
            Yakin
          </button>
        </div>
      </div>
    </div>
  </div>
  <script src="https://cdn.jsdelivr.net/npm/axios/dist/axios.min.js"></script>

  <script type="text/javascript">
    const IMG_PRODUCT = $("#img-product");
    const MAIN_CONTENT = $("#main-content");
    const LOADER = $("#loader");
    let DATA_LOCATION = [];
    let DATA_CUSTOMER =[];
    let DiscountVoucher;
    let DATA_SALES = [];
    let FRAME_COLOR = [];
    let DATA_ITEM = {};
    let DATA_TAMBAH_EMAS = {};
    let DATA_TAMBAH_BATU = {};    
    let DATA_STONE = [];
    let DATA_PAKET_REPARASI = [];
    let DATA_SATUAN_TAMBAHAN = [];
    const BUTTON_DELETE_CONFIRMATION = $("#btn-delete-confirmation");
    const BUTTON_MODAL_CONFIRMATION = $("#btn-modal-confirmation");
    const BUTTON_SUBMIT_PAYMENT = $("#btn-payment-confirmation");
    const SELECT_FRAME_COLOR = $("#FrameColor");
    const SELECT_PAKET_UTAMA = $("#select-paket-utama");
    const SELECT_SATUAN_TAMBAHAN = $("#select-satuan-tambahan");
    const CONTENT_PAYMENT_TYPE = $("#content-choose-payment-types");
    const SELECTED_PAYMENT_TYPE_CONTENT = $("#selected-payment-type-content");
    const DETAIL_PAYMENT_CONTENT = $("#detail-payment-content");
    const TD_TOTAL_BAYAR = $("#td-total-biaya");
    const TD_TOTAL_TERBAYAR = $("#td-total-terbayar");
    const TD_SISA_TERBAYAR = $("#td-sisa-terbayar");
    const TD_TOTAL_PEMBAYARAN = $("#td-total-pembayaran");
    const TBODY_CARA_BAYAR = $("#tbody-cara-bayar");
    const BADGE_STATUS = $("#badge-status");
    const CONTENT_PARENT_PAYMENT = $("#content-parent-payment");
    const DELETE_MODAL = $(".deleteModal");
    let EDCS = [];
    let QRIS_EDCS = [];
    let BANK_ISSUERS = [];
    let PROGRAMS = [];
    let CARD_TYPES = [];
    let CARD_CLASSIFIES = [];
    let QRIS_ISSUERS = [];
    let PAYMENT_LIST = [];
    let statusRepair = 0;

    const REQUIRED_FIELD = ["cc_name", "cc_number", "approval_code"];

    let DEFAULT = {
      totalPayment: 0,
      totalPaid: 0,
      totalNeedToPaid: 0,
      totalNeedToPaidStone: 0,
      totalNeedToPaidEmas: 0,
      totalNeedToPaidInstruksi: 0,
    };

    toastr.options = {
      closeButton: true,
      progressBar: true,
      positionClass: "toast-bottom-center",
      showDuration: "300",
      hideDuration: "1000",
      timeOut: "5000",
    };

    function mapItemToOption(array = []) {
      return array.reduce((accumulator, calculator) => {
        const { ID, Nama } = calculator;

        accumulator += `
                <option value="${ID}" data-id=${ID} data-nama=${Nama}>${Nama}</option>
              `;

        return accumulator;
      }, "");
    }

    function cashPayment() {
      return ``;
    }

    function creditPayment() {
      return `
          <div class="row p-0 g-3">
            <div class="col-6 d-flex flex-column">
              <label for="cc_name" class="fw-bold">Nama Pemilik Kartu</label>
              <input name="cc_name" class="form-control" id="cc_name" />
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="cc_number" class="fw-bold">Nomor Kartu Kredit</label>
              <input name="cc_number" class="form-control" id="cc_number" />
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="approval_code" class="fw-bold">Kode Approval</label>
              <input name="approval_code" class="form-control" id="approval_code" />
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="edc" class="fw-bold">EDC</label>
              <select id="edc" name="edc">
                ${mapItemToOption(EDCS)}
              </select>
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="bank_issuers" class="fw-bold">Bank Issuers</label>
              <select id="bank_issuers" name="bank_issuers">
                ${mapItemToOption(BANK_ISSUERS)}
              </select>
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="programs" class="fw-bold">Program Bank</label>
              <select id="programs" name="programs">
              ${mapItemToOption(PROGRAMS)}
              </select>
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="card_types" class="fw-bold">Tipe Kartu</label>
              <select id="card_types" name="card_types">
                ${mapItemToOption(CARD_TYPES)}
              </select>
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="card_classifies" class="fw-bold">Jenis Kartu</label>
              <select id="card_classifies" name="card_classifies">
                ${mapItemToOption(CARD_CLASSIFIES)}
              </select>
            </div>
          </div>`;
    }

    function transferPayment() {
      return `
          <div class="row p-0 g-3">
            <div class="col-6 d-flex flex-column">
              <label for="cc_name" class="fw-bold">Nama Pemilik Rekening</label>
              <input name="cc_name" class="form-control" id="cc_name" />
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="cc_number" class="fw-bold">Nomor Rekening</label>
              <input name="cc_number" class="form-control" id="cc_number" />
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="bank_issuers" class="fw-bold">Bank Issuers</label>
              <select id="bank_issuers" name="bank_issuers">
                ${mapItemToOption(BANK_ISSUERS)}
              </select>
            </div>
          </div>`;
    }

    function debitPayment() {
      return `
          <div class="row p-0 g-3">
            <div class="col-6 d-flex flex-column">
              <label for="cc_name" class="fw-bold">Nama Pemilik Kartu</label>
              <input name="cc_name" class="form-control" id="cc_name" />
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="cc_number" class="fw-bold">Nomor Kartu Kredit</label>
              <input name="cc_number" class="form-control" id="cc_number" />
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="approval_code" class="fw-bold">Kode Approval</label>
              <input name="approval_code" class="form-control" id="approval_code" />
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="edc" class="fw-bold">EDC</label>
              <select id="edc" name="edc">
                ${mapItemToOption(EDCS)}
              </select>
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="bank_issuers" class="fw-bold">Bank Issuers</label>
              <select id="bank_issuers" name="bank_issuers">
                ${mapItemToOption(BANK_ISSUERS)}
              </select>
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="card_types" class="fw-bold">Tipe Kartu</label>
              <select id="card_types" name="card_types">
                ${mapItemToOption(CARD_TYPES)}
              </select>
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="card_classifies" class="fw-bold">Jenis Kartu</label>
              <select id="card_classifies" name="card_classifies">
                ${mapItemToOption(CARD_CLASSIFIES)}
              </select>
            </div>
          </div>`;
    }

    function bridestoryPayment() {
      return `
          <div class="row p-0 g-3">
            <div class="col-6 d-flex flex-column">
              <label for="cc_name" class="fw-bold">Nama Pemilik Kartu</label>
              <input name="cc_name" class="form-control" id="cc_name" />
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="cc_number" class="fw-bold">Nomor Kartu Kredit</label>
              <input name="cc_number" class="form-control" id="cc_number" />
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="approval_code" class="fw-bold">Kode Approval</label>
              <input name="approval_code" class="form-control" id="approval_code" />
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="bank_issuers" class="fw-bold">Bank Issuers</label>
              <select id="bank_issuers" name="bank_issuers">
                ${mapItemToOption(BANK_ISSUERS)}
              </select>
            </div>
          </div>`;
    }

    function qrisPayment() {
      return `
          <div class="row p-0 g-3">
            <div class="col-6 d-flex flex-column">
              <label for="cc_name" class="fw-bold">Customer Name</label>
              <input name="cc_name" class="form-control" id="cc_name" />
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="cc_number" class="fw-bold">Customer Phone Number</label>
              <input name="cc_number" class="form-control" id="cc_number" />
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="approval_code" class="fw-bold">Reference Number</label>
              <input name="approval_code" class="form-control" id="approval_code" />
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="edc" class="fw-bold">EDC</label>
              <select id="edc" name="edc">
                ${mapItemToOption(QRIS_EDCS)}
              </select>
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="bank_issuers" class="fw-bold">QRIS Issuers</label>
              <select id="bank_issuers" name="bank_issuers">
                ${mapItemToOption(QRIS_ISSUERS)}
              </select>
            </div>
          </div>`;
    }

    function xenditPayment() {
      return `
          <div class="row p-0 g-3">
            <div class="col-6 d-flex flex-column">
              <label for="cc_name" class="fw-bold">Nama Pemilik Kartu</label>
              <select id="cc_name">
                <option value="XENDIT VIRTUAL ACCOUNT">Virtual Account</option>
                <option value="XENDIT CREDIT CARD / DEBIT">Credit Card / Debit</option>
              </select>
            </div>
            <div class="col-6 d-flex flex-column">
              <label for="cc_number" class="fw-bold">Nomor Kartu Kredit</label>
              <input name="cc_number" class="form-control" id="cc_number" />
            </div>
          </div>`;
    }

    function voucherPayment() {
      return `
          <div class="row p-0 g-3">
            <div class="col-6 d-flex flex-column">
              <label for="cc_number" class="fw-bold">Kode Voucher</label>      
              <input name="cc_number" class="form-control" id="cc_number" />
            </div>
          </div>`;
    }

    function freeReparasiPayment() {
      return ``;
    }

    function formatNumber(n) {
      // format number 1000000 to 1,234,567
      return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ".");
    }

    function formatCurrency(input, blur, max_value) {
      // appends $ to value, validates decimal side
      // and puts cursor back in right position.

      // get input value
      var input_val = input.val();

      // don't validate empty input
      if (input_val === "") {
        return;
      }

      // original length
      var original_len = input_val.length;

      // initial caret position
      var caret_pos = input.prop("selectionStart");

      // no decimal entered
      // add commas to number
      // remove all non-digits
      input_val = formatNumber(input_val);
      input_val = "Rp. " + input_val;

      // final formatting
      if (blur === "blur") {
        const number = input_val.substring(4).replaceAll(".", "");

        if (parseInt(number) > max_value) {
          input.attr("data-nominal", max_value);
          input.val("Rp. " + formatNumber(max_value.toString()));
          return;
        }

        input.attr("data-nominal", number);
      }

      // send updated string to input
      input.val(input_val);

      // put caret back in the right position
      var updated_len = input_val.length;
      caret_pos = updated_len - original_len + caret_pos;
      input[0].setSelectionRange(caret_pos, caret_pos);
    }

    function paymentInput({ id, name }) {
      const objectToHtml = {
        1: cashPayment(),
        2: creditPayment(),
        3: transferPayment(),
        4: debitPayment(),
        5: voucherPayment(),
        10: bridestoryPayment(),
        13: qrisPayment(),
        14: xenditPayment(),
        18: freeReparasiPayment(),
      };

      const shouldDisabled = id == 5 || id == 18 ? "disabled" : "";
      const shouldRemoved = id == 5 ? "d-none" : "";

      return `
            <div class="mt-5">
              <h4 class="fw-bold">${name}</h4>
              ${objectToHtml[id]}
              <div>
                <div class="col-6 d-flex flex-column mt-3 ${shouldRemoved}">
                  <label for="nominal" class="fw-bold">Nominal</label>
                  <input type="text" class="form-control" name="nominal" data-nominal="${
                    id == 18 ? "0" : DEFAULT.totalNeedToPaid
                  }" value="Rp. ${formatNumber(
        id == 18 ? "0" : DEFAULT.totalNeedToPaid.toString()
      )}" id="nominal" pattern="^\$\d{1,3}(,\d{3})*(\.\d+)?$" value="" data-type="currency" placeholder="Masukkan nominal..." ${shouldDisabled}>
                </div>
                <button class="btn btn-lg btn-primary mt-3" id="btn-input-pembayaran">Input Pembayaran</button>
              </div>
            </div>
          `;
    }

    function deletePaymentMethod(e) {
      const element = $(e.currentTarget);
      BUTTON_DELETE_CONFIRMATION.attr("data-index", element.attr("data-index"));
      $("#deleteModal").modal("show");
    }

    function onEditDetailPayment({ totalTerbayar = 0, sisaBayar = 0 }) {
      TD_TOTAL_TERBAYAR.html(formatNumberToIDR(totalTerbayar));
      TD_SISA_TERBAYAR.html(formatNumberToIDR(sisaBayar));
    }

    function paymentList() {
      return PAYMENT_LIST;
    }

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
      
      DEFAULT = {
        ...DEFAULT,
        totalPaid: totalPaid,
        totalNeedToPaid: DEFAULT.totalPayment - totalPaid,
      };
      

      // DEFAULT.totalNeedToPaid = DEFAULT.totalNeedToPaidInstruksi < totalPaid ? (DEFAULT.totalNeedToPaid - DEFAULT.totalNeedToPaidInstruksi) : DEFAULT.totalNeedToPaid-totalPaid;

      $("#nominal")
        .val("Rp. " + formatNumber(DEFAULT.totalNeedToPaid.toString()))
        .attr("data-nominal", DEFAULT.totalNeedToPaid.toString());
      // If free repair exists then make it 0 by default.
      if (!ifNotFreeRepairExists && arr.length !== 0) {
        onEditDetailPayment({
          totalTerbayar: DEFAULT.totalPaid,
          sisaBayar: 0,
        });

        TD_TOTAL_PEMBAYARAN.html(formatNumberToIDR(0));
      } else {
        onEditDetailPayment({
          totalTerbayar: DEFAULT.totalPaid,
          sisaBayar: DEFAULT.totalNeedToPaid,
        });

        // TD_TOTAL_PEMBAYARAN.html(formatNumberToIDR(DEFAULT.totalNeedToPaid));
        TD_TOTAL_PEMBAYARAN.html(formatNumberToIDR(DEFAULT.totalPayment));
      }

      // If the payments is completed or free repair is used
      if (
        DEFAULT.totalNeedToPaid === 0 ||
        (!ifNotFreeRepairExists && arr.length !== 0)
      ) {
        const textBadge = !ifNotFreeRepairExists
          ? "Free Reparasi"
          : "Sesuai Nominal";

        BADGE_STATUS.removeClass("bg-danger")
          .addClass("bg-success")
          .text(textBadge);

        SELECTED_PAYMENT_TYPE_CONTENT.html("");

        $(".payment-types")
          .children()
          .removeClass("bg-gray-custom")
          .addClass("cursor-disabled");

        BUTTON_MODAL_CONFIRMATION.attr("disabled", false);
      } else {
        BADGE_STATUS.removeClass("bg-danger")
          .addClass("bg-danger")
          .text("Belum Sesuai Nominal");

        BUTTON_MODAL_CONFIRMATION.attr("disabled", true);

        $(".payment-types").children().removeClass("cursor-disabled");
      }

      // check if other payment other than free repair is exists, it can't be combined
      if (ifNotFreeRepairExists) {
        $(".payment-types[data-id='18']")
          .children()
          .removeClass("bg-gray-custom")
          .addClass("cursor-disabled");
      }      
      TBODY_CARA_BAYAR.html(`
          ${arr.reduce(
            (accumulator, calculator, index) =>
              (accumulator += `
                  <tr>
                    <td style="width: 60%;" class="fw-bold">${
                      calculator.paymentTypeName
                    }</td>
                    <td class="fw-bold">${formatNumberToIDR(
                      calculator.nominal
                    )}</td>
                    <td><button class="btn btn-danger btn-md deleteModal" data-index="${index}"><i class="icon-trash"></i></button></td>
                  </tr>
                  `),
            ""
          )}
          `);
        
        console.log(PAYMENT_LIST)
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

    function onSubmitPayment(e) {
      const nominal = parseInt($("#nominal").attr("data-nominal"));
      const id = $(e.currentTarget).attr("data-input-payment-id");
      const name = $(e.currentTarget).attr("data-input-payment-name");

      const lengthRequiredField = REQUIRED_FIELD.filter(
        (item) => $(`#${item}`).length > 0 && $(`#${item}`).val() === ""
      ).length;

      if (lengthRequiredField > 0) {
        toastr.error("Harap isi semua field dengan benar!");
        return;
      }

      

      if (id == 5) {
        const voucherNumber = $("#cc_number").val().trim();

        const checkVoucherRedemption = PAYMENT_LIST.find(
          (item) => item["cc_number"] == voucherNumber
        );

        if (checkVoucherRedemption) {
          toastr.error("Error: Voucher telah digunakan pada pembayaran ini.");
          return;
        }

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
            if (!response.user || !(dataCustomer.Email == response.user.email || dataCustomer.NamaCustomer == response.user.name || dataCustomer.NomorTelpon == response.user.phone)) {
              toastr.error("Customer is invalid for this voucher code!");
              return;
            }

            if ("error_message" in response) {
              toastr.error(response.error_message);
              return;
            }

            if ("detail" in response && typeof response.detail === "string") {
              toastr.error(response.detail);
              return;
            }

            if (!("is_redeemable" in response)) {
              toastr.error("Returned data expected [is_redeemable] keys");
              return;
            }

            if (!("voucher" in response)) {
              toastr.error("Returned data expected [voucher] keys!");
              return;
            }

            if (
              !("start_date" in response.voucher) &&
              !("end_date" in response.voucher)
            ) {
              toastr.error(
                "Returned data expected [start_date] and [end_date] keys inside response.voucher!"
              );
              return;
            }

            if (
              !("name" in response.voucher) &&
              !("extra_data" in response.voucher)
            ) {
              toastr.error(
                "Returned data expected [name] and [extra_data] keys inside response.voucher!"
              );
              return;
            }

            if (
              !("voucher_type" in response.voucher.extra_data) &&
              !("voucher_nominal" in response.voucher.extra_data)
            ) {
              toastr.error(
                "Returned data expected [voucher_type] and [voucher_nominal] keys inside response.voucher.extra_data!"
              );
              return;
            }

            if (!response.is_redeemable) {
              toastr.error("Voucher is not redeemable!");
              return;
            }            

            const { start_date, end_date, name, extra_data } = response.voucher;
            const { voucher_type, voucher_nominal } = extra_data;
            const checkDate = checkDateRange(start_date, end_date);

            if (typeof checkDate === "string") {
              toastr.error(checkDate);
              return;
            }

            let voucherName = name;
            let nominalPaidVoucher = 0;

            // if (voucher_type === "repair" && voucher_nominal === "full" && 
            // (DEFAULT.totalNeedToPaid != (DEFAULT.totalNeedToPaidEmas+DEFAULT.totalNeedToPaidStone)) && !voucher_nominal.includes("%"))
            //   nominalPaidVoucher = DEFAULT.totalNeedToPaidInstruksi;              

            // if (voucher_type === "repair" && voucher_nominal === "all")
            //   nominalPaidVoucher = DEFAULT.totalNeedToPaidInstruksi + DEFAULT.totalNeedToPaidEmas + DEFAULT.totalNeedToPaidStone;              
            // if (voucher_type === "repair" && voucher_nominal === "all")
            //   nominalPaidVoucher =
            //     DEFAULT.totalNeedToPaidEmas + DEFAULT.totalNeedToPaidInstruksi;

            if (voucher_type === "repair_payment" && !isNaN(voucher_nominal))
              nominalPaidVoucher = DEFAULT.totalNeedToPaidInstruksi < voucher_nominal ? DEFAULT.totalNeedToPaidInstruksi : parseFloat(voucher_nominal);
              DEFAULT.totalNeedToPaidInstruksi = DEFAULT.totalNeedToPaidInstruksi < nominalPaidVoucher ? 0 : DEFAULT.totalNeedToPaidInstruksi - nominalPaidVoucher;
              
            // if(voucher_type == 'repair' && voucher_nominal.includes("%")) {
            //     const percentage = parseFloat(voucher_nominal.replace('%', '')) / 100;
            //     nominalPaidVoucher = DEFAULT.totalNeedToPaidInstruksi * percentage;                
            // }

            if ((
                  voucher_type === "repair_discount" && voucher_nominal.includes('%') &&
                  PAYMENT_LIST.find((item) => item["voucher"] != "full" && item['voucher'] != "all" && item['voucher'].includes('%'))
              )            
            ) {
              toastr.error(
                "Voucher Half Reparasi tidak boleh dicampur dengan jenis voucher pembayaran selain voucher amount atau poin customer!"
              );
              return;
            }

            if ((
                  voucher_type === "repair_discount" && voucher_nominal === "full" &&
                  PAYMENT_LIST.find((item) => item["voucher"] != "full" && item['voucher'] != "all" && item['voucher'].includes('%'))
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
                  PAYMENT_LIST.find((item) => item["voucher"] != "full" && item['voucher'] != "all" && item['voucher'].includes('%'))
              )
            ) {
              toastr.error(
                "Voucher Free Reparasi tidak boleh dicampur dengan jenis pembayaran lain!"
              );
              return;
            }
            if (voucher_type === "repair_discount") {
              toastr.error(
                "Tipe Voucher tersebut hanya bisa digunakan untuk discount!"
              );
              return;
            }
            

            let PAYMENT_LIST_DATA = {
              cc_number: voucherNumber,
              cc_name: voucherName,
              approval_code: $("#approval_code").val() ?? "",
              bank_issuer: $("#bank_issuers").val() ?? "0",
              edc: $("#edc").val() ?? "0",
              program: $("#programs").val() ?? "0",
              card_type: $("#card_types").val() ?? "0",
              card_classifies: $("#card_classifies").val() ?? "0",
              payment_type: id,
              paymentTypeName: name,
              nominal:
                nominalPaidVoucher > DEFAULT.totalNeedToPaid
                  ? DEFAULT.totalNeedToPaid
                  : nominalPaidVoucher,
              voucher: voucher_nominal,
            };            

            PAYMENT_LIST = [...PAYMENT_LIST, PAYMENT_LIST_DATA];
            onEditDetailPaymentType(PAYMENT_LIST);
            $("#cc_number").val("");
            toastr.success("Berhasil menambah voucher reparasi.");

            // if ("voucher" in response) {
            //   if (
            //     "start_date" in response.voucher &&
            //     "end_date" in response.voucher
            //   ) {
            //     const checkDate = checkDateRange(
            //       response.voucher.start_date,
            //       response.voucher.end_date
            //     );

            //     if (typeof checkDate === "string") {
            //       toastr.error(checkDate);
            //       return;
            //     }
            //   }

            //   let voucherName = "";

            //   if ("name" in response.voucher)
            //     voucherName = response.voucher.name;

            //   if ("extra_data" in response.voucher) {
            //     let nominalPaidVoucher = 0;
            //     if (
            //       "voucher_type" in response.voucher.extra_data &&
            //       "voucher_nominal" in response.voucher.extra_data
            //     ) {
            //       if (
            //         response.voucher.extra_data.voucher_type === "repair" &&
            //         response.voucher.extra_data.voucher_nominal === "all"
            //       ) {
            //         nominalPaidVoucher =
            //           DEFAULT.totalNeedToPaidEmas +
            //           DEFAULT.totalNeedToPaidInstruksi;
            //       }

            //       if (
            //         response.voucher.extra_data.voucher_type === "repair" &&
            //         !isNaN(response.voucher.extra_data.voucher_nominal)
            //       ) {
            //         nominalPaidVoucher = parseFloat(
            //           response.voucher.extra_data.voucher_nominal
            //         );
            //       }

            //       let PAYMENT_LIST_DATA = {
            //         cc_number: voucherNumber,
            //         cc_name: voucherName,
            //         approval_code: $("#approval_code").val() ?? "",
            //         bank_issuer: $("#bank_issuers").val() ?? "0",
            //         edc: $("#edc").val() ?? "0",
            //         program: $("#programs").val() ?? "0",
            //         card_type: $("#card_types").val() ?? "0",
            //         card_classifies: $("#card_classifies").val() ?? "0",
            //         payment_type: id,
            //         paymentTypeName: name,
            //         nominal: nominalPaidVoucher,
            //       };

            //       PAYMENT_LIST = [...PAYMENT_LIST, PAYMENT_LIST_DATA];
            //       onEditDetailPaymentType(PAYMENT_LIST);
            //       $("#cc_number").val("");
            //       toastr.success("Berhasil menambah voucher reparasi.");
            //       return;
            //     }
            //   }
            // }

          },
          error: function (error) {
            console.error(error);
          },
        });

        return;
      }

      let PAYMENT_LIST_DATA = {
        cc_number: $("#cc_number").val() ?? "",
        cc_name: $("#cc_name").val() ?? "",
        approval_code: $("#approval_code").val() ?? "",
        bank_issuer: $("#bank_issuers").val() ?? "0",
        edc: $("#edc").val() ?? "0",
        program: $("#programs").val() ?? "0",
        card_type: $("#card_types").val() ?? "0",
        card_classifies: $("#card_classifies").val() ?? "0",
        payment_type: id,
        paymentTypeName: name,
        nominal,
        voucher: "",
      };

      if (id == "1") {
        // Check if cash exists, join into one and not separate it!
        const cashList = PAYMENT_LIST.find((item) => item["payment_type"] == 1);
        if (cashList) {
          const payments = PAYMENT_LIST.map((item) => {
            if (item["payment_type"] == 1) {
              item["nominal"] += nominal;
            }
            return item;
          });
          onEditDetailPaymentType(payments);
        } else {
          PAYMENT_LIST = [...PAYMENT_LIST, PAYMENT_LIST_DATA];
          onEditDetailPaymentType(PAYMENT_LIST);
        }

        return;
      }

      PAYMENT_LIST = [...PAYMENT_LIST, PAYMENT_LIST_DATA];
      onEditDetailPaymentType(PAYMENT_LIST);
      
    }

    function onSelectPayment(e) {
      const element = $(e.currentTarget);
      const id = element.attr("data-id");
      const name = element.attr("data-nama");

      if (element.children().hasClass("cursor-disabled")) return;

      $(".payment-types").children().removeClass("bg-gray-custom");
      element.children().addClass("bg-gray-custom");

      SELECTED_PAYMENT_TYPE_CONTENT.html(paymentInput({ id, name }));

      $("#btn-input-pembayaran")
        .attr("data-input-payment-name", name)
        .attr("data-input-payment-id", id);

      $("#edc").select2();
      $("#programs").select2();
      $("#card_types").select2();
      $("#bank_issuers").select2();
      $("#card_classifies").select2();

      if (id == 14) {
        $("#cc_name").select2("destroy").select2();
      }
    }

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

    function cancelVoucherRedemption(args = { payments: [] }) {
      const { payments } = args;

      payments.forEach(({ RD_ID }) => {
        $.ajax({
          type: "POST",
          url: "./index.aspx/CancelVoucherRedemption",
          data: JSON.stringify({ RD_ID }),
          contentType: "application/json; charset=utf-8",
          success: function (data) {
            const response = data["d"];

            if (response?.redemption?.status === "Canceled") {
              toastr.warning(
                "WARNING: Pembayaran gagal, voucher redemption berhasil di batalkan!"
              );
            }
          },
          error: function (error) {
            toastr.error("Terjadi kesalahan saat cancel redemption voucher!");
            console.error(error);
          },
        });
      });      
    }

    function insertFormToSalesOrder(array) {
      const urlParams = new URLSearchParams(window.location.search);
      const id = urlParams.get("id");

      const payments = array.map(({ voucher, ...item }) => item);
      const isFree = statusRepair;
          console.log({IDDocRepair: id,
            Keterangan: "",
            Payments: payments,
            IsFree: Number(isFree),
            TotalBayarBeforeDisc : DEFAULT.totalNeedToPaidBeforeDisc,
            TotalBayarAfterDisc : DEFAULT.totalPaid,
            DiskonInstruksi : DEFAULT.totalNeedToPaidInstruksi,
          })
      
      $.ajax({
        type: "POST",
        url: "./index.aspx/InsertSalesOrderRepair",
        data: JSON.stringify({
          insertFormSalesOrderRepair: {
            IDDocRepair: id,
            Keterangan: "",
            Payments: payments,
            IsFree: Number(isFree),
            hargaBeforeDisc : DEFAULT.totalNeedToPaidBeforeDisc,
            hargaAfterDisc : DEFAULT.totalPaid,
            DiskonInstruksi : DEFAULT.totalNeedToPaidInstruksi
          },
        }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"][0]["IsFreeReparasi"];
          console.log(response)
          if (response == 0) {                        

            // const linkToOpen = "../../Report/RepairInvoice/RepairInvoice.aspx?id=" + id;
            // if (DEFAULT.totalNeedToPaidInstruksi) {              
            //   window.open(linkToOpen);
            // }
            if (DEFAULT.totalNeedToPaidEmas > 0 || DEFAULT.totalNeedToPaidStone > 0) {
              const openInvoiceMaterial = "../../Report/RepairInvoice/RepairInvoiceMaterial.aspx?id=" + id;
              window.open(openInvoiceMaterial);
            }
            window.open("../../Report/RepairInvoice/RepairInvoiceGabungan.aspx?id=" + id);
          }

          setTimeout(() => {
            window.location.reload();
          }, 500);
       
        },
        error: function (error) {
          toastr.error(
            "Terjadi kesalahan saat melakukan pembayaran. Silahkan hubungi tim IT!"
          );
          cancelVoucherRedemption({
            payments: payments
              .filter((item) => item["payment_type"] == 5)
              .map((item) => {
                return {
                  RD_ID: item["approval_code"],
                };
              }),
          });
        },
        complete: function (data) {
          BUTTON_MODAL_CONFIRMATION.prop("disabled", false);
          BUTTON_SUBMIT_PAYMENT.prop("disabled", false);
        },
      });
    
    }

    function formatNumberToIDR(number) {
      const IDR = new Intl.NumberFormat("id-ID", {
        style: "currency",
        currency: "IDR",
      }).format(number);
      return IDR;
    }

    function redeemVoucher(args = { cc_number }) {
      return new Promise((resolve, reject) => {
        $.ajax({
          type: "POST",
          url: "./index.aspx/RedeemVoucher",
          data: JSON.stringify({
            voucher_code: args.cc_number,
            identifier: DATA_ITEM["IDCustomerStamps"],
            // identifier: 1168694,
            store: DATA_ITEM["KodeStore"],
          }),
          contentType: "application/json; charset=utf-8",
          success: function (data) {
            const response = data["d"];

            if ("error_code" in response && "detail" in response) {
              toastr.warning(`${response.error_code}: ${response.detail}`);
              BUTTON_MODAL_CONFIRMATION.prop("disabled", false);
              BUTTON_SUBMIT_PAYMENT.prop("disabled", false);
              return;
            }

            if ("error_message" in response) {
              toastr.error(response.error_message);
              BUTTON_MODAL_CONFIRMATION.prop("disabled", false);
              BUTTON_SUBMIT_PAYMENT.prop("disabled", false);
              return;
            }

            if ("detail" in response && typeof response.detail === "string") {
              toastr.error(response.detail);
              BUTTON_MODAL_CONFIRMATION.prop("disabled", false);
              BUTTON_SUBMIT_PAYMENT.prop("disabled", false);
              return;
            }

            if ("redemption" in response) {
              if ("id" in response.redemption) {
                const PAYMENT_LIST_WITH_UPDATED_RD_ID = PAYMENT_LIST.find(
                  (item, index) => item["cc_number"] == args.cc_number
                );

                PAYMENT_LIST_WITH_UPDATED_RD_ID["approval_code"] =
                  response.redemption.id;

                const { paymentTypeName, ...newPaymentList } =
                  PAYMENT_LIST_WITH_UPDATED_RD_ID;
                toastr.success("Berhasil redeem voucher!");
                
                resolve(newPaymentList);
              }
            }
          },
          error: function (error) {
            toastr.error("Terjadi kesalahan saat melakukan redeem voucher!");
            reject(error);
          },
        });
      });
    }

    // Function to update the global array in a loop
    async function resolveRedeemVoucher(args) {
      const promises = args.map((id) => redeemVoucher(id)); // Create an array of promises

      try {
        const results = await Promise.all(promises); // Wait for all AJAX calls to complete
        PAYMENT_LIST.concat(results); // Update the global array with all results
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    }

    // Initializes while document is ready
    $(document).ready(function () {
      SELECT_FRAME_COLOR.select2();
      SELECT_PAKET_UTAMA.select2();
      SELECT_SATUAN_TAMBAHAN.select2();
    });

    // event handler
    $(document).ready(function () {
      $(document).on("click", ".payment-types", (e) => onSelectPayment(e));
      $(document).on(
        {
          keyup: function () {
            formatCurrency($(this));
          },
          blur: function () {
            formatCurrency($(this), "blur", DEFAULT.totalNeedToPaid);
          },
        },
        "input[data-type='currency']"
      );

      $(document).on("click", "#btn-input-pembayaran", (e) =>
        onSubmitPayment(e)
      );

      $("input[data-type='currency']").on({
        keyup: function () {
          formatCurrency($(this));
        },
        blur: function () {
          formatCurrency($(this), "blur");
        },
      });

      $(document).on("click", ".deleteModal", (e) => deletePaymentMethod(e));

      BUTTON_SUBMIT_PAYMENT.on("click", (e) => {
        const element = $(e.currentTarget);

        const urlParams = new URLSearchParams(window.location.search);
        const id = urlParams.get("id");
        

        element.prop("disabled", true);
        BUTTON_MODAL_CONFIRMATION.prop("disabled", true);

        const paymentWithVoucher = PAYMENT_LIST.filter(
          ({ payment_type }) => payment_type == 5
        );

        

        if (paymentWithVoucher.length > 0 ) {          
          const args = paymentWithVoucher.map((item) => {
            return {
              cc_number: item["cc_number"],
            };                                        
          });
          
          resolveRedeemVoucher(args).then(() => {
            insertFormToSalesOrder(
              PAYMENT_LIST.map(
                (
                  {
                    cc_number,
                    cc_name,
                    approval_code,
                    bank_issuer,
                    edc,
                    program,
                    card_type,
                    card_classifies,
                    payment_type,
                    paymentTypeName,
                    nominal,
                    voucher,
                  },
                  index
                ) => {
                  return {
                    IDPaymentType: payment_type,
                    IDCard: card_type,
                    IDProgram: program,
                    IDEDC: edc,
                    IDBankIssuer: bank_issuer,
                    CCNumber: cc_number,
                    CCName: cc_name,
                    Nominal: nominal,
                    IDJenisKartuKredit: card_classifies,
                    NoIndex: index + 1,
                    ApprovalKode: approval_code,
                    voucher,
                  };
                }
              )
            );
            element.prop("disabled", false);
          });

          return;
        }

        insertFormToSalesOrder(
          PAYMENT_LIST.map(
            (
              {
                cc_number,
                cc_name,
                approval_code,
                bank_issuer,
                edc,
                program,
                card_type,
                card_classifies,
                payment_type,
                paymentTypeName,
                nominal,
                voucher,
              },
              index
            ) => {
              return {
                IDPaymentType: payment_type,
                IDCard: card_type,
                IDProgram: program,
                IDEDC: edc,
                IDBankIssuer: bank_issuer,
                CCNumber: cc_number,
                CCName: cc_name,
                Nominal: nominal,
                IDJenisKartuKredit: card_classifies,
                NoIndex: index + 1,
                ApprovalKode: approval_code,
                voucher,
              };
            }
          )
        );
        element.prop("disabled", false);
      });

      BUTTON_DELETE_CONFIRMATION.on("click", (e) => {
        const element = $(e.currentTarget).attr("data-index");
        PAYMENT_LIST = PAYMENT_LIST.filter((item, index) => index != element);
        onEditDetailPaymentType(PAYMENT_LIST);
      });
    });

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
          DiscountVoucher = response["docRepairVoucher"];
          DATA_CUSTOMER = response["docRepairCustomer"];
          

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
            const { NomorRepair, NamaPackage, CostOther} =          
              calculator;
              if (NamaPackage == '-'){              
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

          $("#tbody-biaya-reparasi").append(jenisReparasi);

          const selisihBerat = (
            DATA_ITEM["BeratAkhirTanpaBatu"] - DATA_ITEM["BeratTimbangActual"]
          ).toFixed(2);

          let biayaPerbaikanTambahEmas = 0;
          const isPaidSelisih =
            DATA_ITEM["TotalGramLebih"] > 0
              ? "class='text-danger'"
              : "class='text-success'";

          biayaPerbaikan += DATA_ITEM["HargaBayar"];
          biayaPerbaikanTambahEmas += DATA_ITEM["HargaBayar"];
          DEFAULT.totalNeedToPaidEmas = DATA_ITEM["HargaBayar"];

          if (biayaPerbaikanTambahEmas > 0) { DATA_TAMBAH_EMAS = {"hargaTambahEmas":biayaPerbaikanTambahEmas};}

          // <td>${formatNumberToIDR(DATA_ITEM["HargaEmas"])}</td>

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

          let biayaPerbaikanTambahBatu = 0;
          if (parcel.length > 0) {
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
              biayaPerbaikanTambahBatu += HargaTotalRupiahRound;              

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

            DATA_TAMBAH_BATU = {"hargaTambahBatu":biayaPerbaikanTambahBatu};

            TBODY_BIAYA_TAMBAH_BATU.append(parcelHtml);
            $("#content-tambah-batu").removeClass("d-none");
          }
          if (biayaPerbaikanTambahBatu == 0 && biayaPerbaikanTambahEmas == 0){ 
            statusRepair = 1; // Free Reparasi
          }

          MAIN_CONTENT.removeClass("d-none").addClass("d-flex");          
          $("#content-total-bayar3").html(`
              <div class="d-flex justify-content-between align-items-center">
                <div>Total Tagihan</div>
                <div >${formatNumberToIDR(biayaPerbaikan)}</div>
              </div>
          `);
          $("#content-total-bayar2").html(`
                  <div class="d-flex justify-content-between align-items-center">
                    <div>Diskon Biaya Instruksi</div>
                    <div class="text-red">- ${formatNumberToIDR(biayaPerbaikanInstruksi)}</div>
                  </div>
          `);

          $("#content-total-bayar").html(`
                  <div class="d-flex justify-content-between align-items-center">
                    <div>Total</div>
                    <div>${formatNumberToIDR(biayaPerbaikan - biayaPerbaikanInstruksi)}</div>
                  </div>
          `);
          let biayaBeforeDisc = biayaPerbaikan;
          
          if (DiscountVoucher.length > 0) {   
            if (DiscountVoucher[0]["JenisVoucher"] == 'all' || (DiscountVoucher[0]["JenisVoucher"] == 'full' && DiscountVoucher[0]["NominalDiscount"] == biayaPerbaikan)) {
                biayaPerbaikan = DiscountVoucher[0]["NominalDiscount"];         
                let PAYMENT_LIST_DATA = {
                  cc_number: '',
                  cc_name: '',
                  approval_code: $("#approval_code").val() ?? "",
                  bank_issuer: $("#bank_issuers").val() ?? "0",
                  edc: $("#edc").val() ?? "0",
                  program: $("#programs").val() ?? "0",
                  card_type: $("#card_types").val() ?? "0",
                  card_classifies: $("#card_classifies").val() ?? "0",
                  payment_type: 0,
                  paymentTypeName: 'Voucher Discount',
                  nominal: 0,
                  voucher: DiscountVoucher[0]["JenisVoucher"],              
                };

                PAYMENT_LIST = [...PAYMENT_LIST, PAYMENT_LIST_DATA];                                                                                
            }                        
            biayaPerbaikan -= parseFloat(DiscountVoucher[0]["NominalDiscount"]);
            $('#content-tambah-discount').removeClass("d-none");                        
            $('#tbody-add-voucher').append(`
              <tr>
                <td class="text-left">${DiscountVoucher[0]["KodeVoucher"]}</td>
                <td class="text-left">${DiscountVoucher[0]["JenisVoucher"]}</td>                                                                  
                <td class="text-right">${DiscountVoucher[0]["NominalDiscount"]>0?"-":""}${formatNumberToIDR(parseFloat(DiscountVoucher[0]["NominalDiscount"]))}</td>
              </tr>
            `);                                  
          }          
          
          if (DATA_ITEM.Status === 9 ) {
            $('#totalbayar-lunas').removeClass('d-none')
            $("#content-total-bayar1").html(`
                  <div class="d-flex justify-content-between align-items-center">
                    <div>Total Bayar</div>
                    <div>${formatNumberToIDR(biayaPerbaikan - biayaPerbaikanInstruksi)}</div>
                  </div>
          `);
          }

          DEFAULT = {
            ...DEFAULT,
            totalPayment: biayaPerbaikan - biayaPerbaikanInstruksi,
            totalPaid: 0,
            totalNeedToPaid: biayaPerbaikan-biayaPerbaikanInstruksi,
            totalNeedToPaidEmas: biayaPerbaikanTambahEmas,
            totalNeedToPaidStone: biayaPerbaikanTambahBatu,
            totalNeedToPaidInstruksi: biayaPerbaikanInstruksi,
            totalNeedToPaidBeforeDisc : biayaBeforeDisc,
          };
          

          TD_TOTAL_BAYAR.html(formatNumberToIDR(biayaPerbaikan-biayaPerbaikanInstruksi));
          TD_TOTAL_PEMBAYARAN.html(formatNumberToIDR(biayaPerbaikan-biayaPerbaikanInstruksi));
          onEditDetailPayment({ totalTerbayar: 0, sisaBayar: biayaPerbaikan-biayaPerbaikanInstruksi });
        },

        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          console.log(data);
          // const Status=9;
          const Status = data["responseJSON"]["d"]["docRepairDetail"]["Status"];        

          if (Status === 9) {
            LOADER.html(
              `<div class="alert alert-success" role="alert">
                <h4 class="alert-heading fw-bold">Pembayaran Berhasil!</h4>
              </div>`
            )
              .removeClass("d-none")
              .addClass("container p-0");

            CONTENT_PARENT_PAYMENT.html("");

            return;
          }

          if (Status === 12) {
            LOADER.html(
              `<div class="alert alert-warning" role="alert">
                <h4 class="alert-heading fw-bold">Pembayaran Terbayar!</h4>
                <hr class="border border-bottom border-dark">
                <p class="mb-0">Khusus <b>Free Reparasi</b> Pembayaran perlu di approve oleh Store Manager!</p>
              </div>`
            )
              .removeClass("d-none")
              .addClass("container p-0");

            CONTENT_PARENT_PAYMENT.html("");

            return;
          }
          if (DEFAULT.totalNeedToPaid == 0) {            
            statusRepair = 1; // Free Reparasi

            BADGE_STATUS.removeClass("bg-danger")
              .addClass("bg-success")
              .text("Free Reparasi");

            SELECTED_PAYMENT_TYPE_CONTENT.html("");

            $(".payment-types")
              .children()
              .removeClass("bg-gray-custom")
              .addClass("cursor-disabled");

            BUTTON_MODAL_CONFIRMATION.attr("disabled", false);
            $('#content-payment-tipe').html("");            

                let PAYMENT_LIST_DATA = {
                  cc_number: '',
                  cc_name: '',
                  approval_code:  "",
                  bank_issuer:  "0",
                  edc: "0",
                  program: "0",
                  card_type:  "0",
                  card_classifies: "0",
                  payment_type: 18,
                  paymentTypeName: 'FREE REPAIR',
                  nominal: 0,
                  voucher: "",
                };

                PAYMENT_LIST = [...PAYMENT_LIST, PAYMENT_LIST_DATA];                                                                                
            
          }

          LOADER.hide();

          $.ajax({
            type: "GET",
            url:
              "./index.aspx/GetPaymentType?nominal=" + DEFAULT.totalNeedToPaid,
            contentType: "application/json; charset=utf-8",
            success: function (dataInner) {
              const response = dataInner["d"];
              
              const paymentOptionHtml = response.reduce(
                (accumulator, calculator) => {
                  const { Nama, ID, Icon } = calculator;
                  accumulator += `
                  <div class="col-3 payment-types" data-id="${ID}" data-nama="${Nama}" style="cursor: pointer;">
                    <div class="rounded border d-flex flex-column gap-1 py-3 px-5 justify-content-center align-items-center payment-type">
                      <img style="width: ${
                        Nama.includes("Bridestory") ? "100px" : "25px"
                      }; height: 25px;" src='./assets/${Icon}' alt="${Nama}" />
                      <span class="fw-bold" style="font-size: 12px;">${Nama}</span>
                    </div>
                  </div>
                `;

                  return accumulator;
                },
                ""
              );

              CONTENT_PAYMENT_TYPE.html(paymentOptionHtml);
            },
            error: function (error) {
              console.error(error);
            },
          });
        },
      });

      $.ajax({
        type: "GET",
        url: "./index.aspx/GetDefaultData",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const {
            BankIssuers,
            QRISBankIssuer,
            EDCLists,
            Programs,
            CardTypes,
            CardClasifies,
            QRISEDC,
          } = data["d"];

          EDCS = EDCLists;
          BANK_ISSUERS = BankIssuers;
          PROGRAMS = Programs;
          CARD_CLASSIFIES = CardClasifies;
          CARD_TYPES = CardTypes;
          QRIS_EDCS = QRISEDC;
          QRIS_ISSUERS = QRISBankIssuer;
        },
        error: function (error) {
          console.error(error);
        },
      });
    });
  </script>
</asp:Content>
