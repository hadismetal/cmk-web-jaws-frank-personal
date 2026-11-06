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

  <div class="container border rounded p-5">
    <h1 style="border-bottom: none" class="m-0 p-0 fw-bold">Cari Produk</h1>
    <div class="row g-4 py-3">
      <div class="col-3">
        <label for="select-location" class="form-label fw-bold">Lokasi</label>
        <select class="w-100" id="select-location"></select>
      </div>
      <div class="col-3">
        <label for="select-jr" class="form-label fw-bold">JP / JR</label>
        <select class="w-100" id="select-jr"></select>
      </div>
    </div>
    <hr />
    <div class="row">
      <div class="row align-items-end">
        <div class="col-3">
          <label for="select-search-with" class="form-label fw-bold"
            >Pilih Berdasarkan</label
          >
          <select id="select-search-with" class="w-100">
            <option value="1">PLU</option>
            <option value="2">Kustomer</option>
          </select>
        </div>
        <div class="col-3">
          <label for="input-search-product" class="form-label fw-bold"
            >Kode Product</label
          >
          <input type="text" class="form-control" id="input-search-product" />
        </div>
        <div class="col-1">
          <button
            id="btn-modal-search-customer"
            type="button"
            class="btn btn-primary btn-lg d-none"
            data-bs-toggle="modal"
            data-bs-target="#exampleModal"
          >
            Cari Kustomer
          </button>
          <button
            id="btn-search-product"
            type="button"
            class="btn btn-primary btn-lg"
          >
            Cari Produk
          </button>
        </div>
      </div>
    </div>
  </div>

  <div class="container mx-auto p-0 d-none" id="content-after-plu-fetched">
    <div class="container mt-5 border rounded p-5">
      <h1 style="border-bottom: none" class="h1 m-0 p-0 fw-bold">
        Info Produk
      </h1>
      <div class="row mt-5">
        <div class="col-12 row">
          <div class="col-lg-3">
            <img src="" id="img-product-element" class="border rounded p-5" />
          </div>
          <div class="col-lg-9">
            <div class="row gy-5">
              <div class="col-lg-6 d-flex flex-column">
                <span class="h4">Nomor Invoice</span>
                <h5 id="invoice-text-element" class="h3 fw-bold"></h5>
              </div>
              <div class="col-lg-6 d-flex flex-column">
                <span class="h4">Item Produk</span>
                <h5 id="item-text-element" class="h3 fw-bold"></h5>
              </div>
              <div class="col-lg-6 d-flex flex-column">
                <span class="h4">Nomor PLU</span>
                <h5 id="nomor-text-element" class="h3 fw-bold"></h5>
              </div>
              <div class="col-lg-6 d-flex flex-column"></div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="container mt-5 border rounded p-5">
      <h1 style="border-bottom: none" class="h1 mx-0 mt-0 p-0 fw-bold">
        Riwayat Reparasi
      </h1>
      <div class="row">
        <div
          class="d-flex flex-column gy-2"
          id="content-history-reparasi"
        ></div>
      </div>
    </div>

    <div class="container mt-5 border rounded p-5">
      <h1 style="border-bottom: none" class="h1 mx-0 mb-5 mt-0 p-0 fw-bold">
        Detail QC
      </h1>
      <div class="col-12">
        <div class="row">
          <div class="col-5 d-flex flex-column gap-4">
            <div class="row">
              <div class="col-12">
                <div class="row">
                  <div class="col-4">
                    <label for="input-weight-qc-old" class="form-label fw-bold"
                      >Berat BRJ</label
                    >
                    <input
                      type="text"
                      name="input-weight-qc-old"
                      id="input-weight-qc-old"
                      class="form-control"
                      disabled
                    />
                  </div>
                  <div class="col-4">
                    <label
                      for="input-weight-qc-actual"
                      class="form-label fw-bold"
                      >Berat Actual</label
                    >
                    <input
                      type="number"
                      name="input-weight-qc-actual"
                      id="input-weight-qc-actual"
                      class="form-control"
                      step="0.01"
                    />
                  </div>
                  <!-- <div class="col-4">
                    <label
                      for="input-weight-qc-repair"
                      class="form-label fw-bold"
                      >Berat Repair</label
                    >
                    <input
                      type="number"
                      name="input-weight-qc-repair"
                      id="input-weight-qc-repair"
                      class="form-control"
                      step="0.01"
                    />
                  </div> -->
                </div>
              </div>
            </div>

            <div class="d-flex flex-column">
              <label class="fw-bold">Grafir Kadar Emas</label>
              <div class="d-flex flex-row gap-5">
                <div class="form-check form-check-inline">
                  <input
                    class="form-check-input"
                    type="radio"
                    name="input-radio-grafir"
                    id="input-radio-grafir-ada"
                    value="Ada"
                    checked
                  />
                  <label class="form-check-label" for="input-radio-grafir-ada"
                    >Ada</label
                  >
                </div>
                <div class="form-check form-check-inline">
                  <input
                    class="form-check-input"
                    type="radio"
                    name="input-radio-grafir"
                    id="input-radio-grafir-tidak-ada"
                    value="Tidak Ada"
                  />
                  <label
                    class="form-check-label"
                    for="input-radio-grafir-tidak-ada"
                    >Tidak Ada</label
                  >
                </div>
                <!-- <div class="form-check form-check-inline">
                  <input class="form-check-input"  type="radio"  name="input-radio-grafir"  id="input-radio-grafir-rusak"  value="Damaged"/>
                  <label class="form-check-label" for="input-radio-grafir-rusak">Damaged</label>
                </div> -->
              </div>
            </div>

            <div class="d-flex flex-column">
              <label class="fw-bold">Tes Keaslian</label>
              <div class="d-flex flex-row gap-5">
                <div class="form-check form-check-inline">
                  <input
                    class="form-check-input"
                    type="radio"
                    name="input-radio-tes-keaslian"
                    id="input-radio-tes-keaslian-ada"
                    value="Ya"
                    checked
                  />
                  <label
                    class="form-check-label"
                    for="input-radio-tes-keaslian-ada"
                    >Ya</label
                  >
                </div>
                <div class="form-check form-check-inline">
                  <input
                    class="form-check-input"
                    type="radio"
                    name="input-radio-tes-keaslian"
                    id="input-radio-tes-keaslian-tidak-ada"
                    value="Tidak"
                  />
                  <label
                    class="form-check-label"
                    for="input-radio-tes-keaslian-tidak-ada"
                    >Tidak</label
                  >
                </div>
              </div>
            </div>

            <div class="d-flex flex-column">
              <label class="fw-bold">Basic / Non-Basic</label>
              <div class="d-flex flex-row gap-5">
                <div class="form-check form-check-inline">
                  <input
                    class="form-check-input"
                    type="radio"
                    name="input-radio-tipe"
                    id="input-radio-tipe-basic"
                    value="1"
                    checked
                  />
                  <label
                    class="form-check-label"
                    for="input-radio-tes-keaslian-ada"
                    >Basic</label
                  >
                </div>
                <div class="form-check form-check-inline">
                  <input
                    class="form-check-input"
                    type="radio"
                    name="input-radio-tipe"
                    id="input-radio-tipe-non-basic"
                    value="0"
                  />
                  <label
                    class="form-check-label"
                    for="input-radio-tes-keaslian-tidak-ada"
                    >Non-Basic</label
                  >
                </div>
              </div>
            </div>
          </div>
          <div class="offset-1 col-5 d-flex flex-column gap-4">
            <div class="d-flex flex-column">
              <label for="select-frame-color" class="form-label fw-bold"
                >Frame Color</label
              >
              <select class="w-100" id="select-frame-color"></select>
            </div>

            <div class="d-flex flex-column">
              <label class="fw-bold">Grafir Logo</label>
              <div class="d-flex flex-row gap-5">
                <div class="form-check form-check-inline">
                  <input
                    class="form-check-input"
                    type="radio"
                    name="input-radio-logo"
                    id="input-radio-logo-ada"
                    value="Ada"
                    checked
                  />
                  <label class="form-check-label" for="input-radio-logo-ada"
                    >Ada</label
                  >
                </div>
                <div class="form-check form-check-inline">
                  <input
                    class="form-check-input"
                    type="radio"
                    name="input-radio-logo"
                    id="input-radio-logo-tidak-ada"
                    value="Tidak Ada"
                  />
                  <label
                    class="form-check-label"
                    for="input-radio-logo-tidak-ada"
                    >Tidak Ada</label
                  >
                </div>
                <!-- <div class="form-check form-check-inline">
                  <input  class="form-check-input"  type="radio"  name="input-radio-logo"  id="input-radio-logo-rusak"  value="Damaged"/>
                  <label class="form-check-label" for="input-radio-logo-rusak">Damaged</label>
                </div> -->
              </div>
            </div>

            <div class="d-flex flex-column">
              <label class="fw-bold">Fisik Keseluruhan</label>
              <div class="d-flex flex-row gap-5">
                <div class="form-check form-check-inline">
                  <input
                    class="form-check-input"
                    type="radio"
                    name="input-radio-rangka"
                    id="input-radio-rangka-ada"
                    value="Baik"
                    checked
                  />
                  <label class="form-check-label" for="input-radio-rangka-ada"
                    >Baik</label
                  >
                </div>
                <div class="form-check form-check-inline">
                  <input
                    class="form-check-input"
                    type="radio"
                    name="input-radio-rangka"
                    id="input-radio-rangka-tidak-ada"
                    value="Tidak Baik"
                  />
                  <label
                    class="form-check-label"
                    for="input-radio-rangka-tidak-ada"
                    >Tidak Baik</label
                  >
                </div>
              </div>
            </div>
          </div>

          <div class="mt-4">
            <label for="form-textarea-keterangan" class="form-label fw-bold"
              >Keterangan</label
            >
            <textarea
              class="form-control"
              name="form-textarea-keterangan"
              id="form-textarea-keterangan"
              rows="7"
              cols="10"
            ></textarea>
          </div>
        </div>
      </div>
    </div>

    <div class="container mt-5 border rounded p-5">
      <h1 style="border-bottom: none" class="h1 mx-0 mb-5 mt-0 p-0 fw-bold">
        Detail Reparasi
      </h1>
      <div class="row gx-5">
        <div class="col-12">
          <div class="row g-4">
            <div class="col-12">
              <div class="row">
                <div class="col-6">
                  <div class="row" id="content-size-reparasi">
                    <div class="col">
                      <div>
                        <label for="input-size-qc" class="form-label fw-bold"
                          >Size BRJ</label
                        >
                        <input
                          type="number"
                          name="input-size-brj"
                          id="input-size-brj"
                          class="form-control"
                          disabled
                        />
                      </div>
                    </div>
                    <div class="col">
                      <div>
                        <label for="input-size-qc" class="form-label fw-bold"
                          >Size Actual</label
                        >
                        <input
                          type="number"
                          name="input-size-qc-old"
                          id="input-size-qc-old"
                          class="form-control"
                          step="0.5"
                        />
                      </div>
                    </div>
                    <div class="col">
                      <div>
                        <label for="input-size-qc" class="form-label fw-bold"
                          >Size Akhir</label
                        >
                        <input
                          type="number"
                          name="input-size-qc-actual"
                          id="input-size-qc-actual"
                          class="form-control"
                          disabled
                        />
                      </div>
                    </div>
                  </div>
                </div>
                <div class="col-2 d-none" id="content-plus-minus-size">
                  <div>
                    <label
                      for="input-size-qc-plus-minus"
                      class="form-label fw-bold"
                    ></label>
                    <input
                      type="number"
                      name="input-size-qc-plus-minus"
                      id="input-size-qc-plus-minus"
                      class="form-control"
                      step="0.5"
                    />
                  </div>
                </div>
              </div>
            </div>
            <div class="col-6">
              <div class="d-flex flex-column">
                <label for="select-lokasi-reparasi" class="form-label fw-bold"
                  >Lokasi Reparasi</label
                >
                <select class="w-100" id="select-lokasi-reparasi"></select>
                <small
                  id="text-helper-location-reparasi"
                  class="fw-bold"
                ></small>
              </div>
            </div>
            <div class="col-6">
              <div class="d-flex flex-column">
                <label for="select-paket-utama" class="form-label fw-bold"
                  >Paket Utama</label
                >
                <select class="w-100" id="select-paket-utama"></select>
              </div>
            </div>
            <div class="col-12">
              <div class="d-flex flex-column">
                <label for="select-satuan-tambahan" class="form-label fw-bold"
                  >Satuan Tambahan</label
                >
                <select
                  class="w-100"
                  id="select-satuan-tambahan"
                  multiple
                  placeholder="Pilih satuan tambahan..."
                ></select>
              </div>
            </div>
            <div class="col-12">
              <div class="d-flex flex-column">
                <label for="input-file-foto-product" class="form-label fw-bold"
                  >Foto Produk (JPG/JPEG/PNG)</label
                >
                <input
                  type="file"
                  id="input-file-foto-product"
                  name="input-file-foto-product"
                />
              </div>
            </div>
            <div class="mt-4">
              <label
                for="form-textarea-keterangan-reparasi"
                class="form-label fw-bold"
                >Keterangan</label
              >
              <textarea
                class="form-control"
                name="form-textarea-keterangan-reparasi"
                id="form-textarea-keterangan-reparasi"
                rows="7"
                cols="10"
              ></textarea>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div
      class="container-fluid mt-5 border rounded p-5"
      id="content-detail-stone"
    >
      <h1 style="border-bottom: none" class="h1 mx-0 mb-5 mt-0 p-0 fw-bold">
        Detail Stone
      </h1>
      <table class="table table-hover" id="table-stone">
        <thead>
          <tr>
            <th>No</th>
            <th>Stone</th>
            <th>Butir</th>
            <th>Butir Actual</th>
            <th>Carat</th>
            <th>Carat Actual</th>
            <th class="text-center" style="display: none">Tes Kualitas</th>
            <th class="text-center">Tes Keaslian</th>
            <th class="text-center">Sesuai Sertifikat</th>
            <th>Keterangan</th>
          </tr>
        </thead>
        <tbody id="tbody-detail-stone-table"></tbody>
      </table>
    </div>

    <div class="row mt-3 justify-content-end">
      <div class="col-2">
        <button
          class="btn btn-outline-secondary justify-content-endry btn-lg w-100"
          id="btn-cancel-form"
        >
          Batal
        </button>
      </div>
      <div class="col-2">
        <button
          class="btn btn-primary btn-lg w-100"
          data-bs-toggle="modal"
          data-bs-target="#modalConfirmation"
          id="btn-modal-confirmation"
        >
          Simpan
        </button>
      </div>
    </div>
  </div>

  <!-- Modal -->
  <div
    class="modal fade"
    id="exampleModal"
    tabindex="-1"
    aria-labelledby="exampleModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-xl">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title fs-3" id="exampleModalLabel">Pilih Produk</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <div class="row align-items-end">
            <div class="col-4">
              <label for="input-keyword" class="form-label fw-bold h5"
                >Keywords:</label
              >
              <input
                type="text"
                class="form-control"
                id="input-keyword-customer"
                placeholder="Masukkan Nama, No Telp atau Email Kustomer..."
              />
            </div>
            <div class="col-4">
              <label for="input-keyword" class="form-label fw-bold h5"
                >Kustomer Lama:</label
              >
              <input
                type="text"
                class="form-control"
                id="input-customer-lama"
                placeholder="Masukkan Kode Kustomer Lama..."
              />
            </div>
            <div class="col-2">
              <button class="btn btn-primary btn-lg" id="btn-search-customer">
                Cari Kustomer
              </button>
            </div>
          </div>

          <div id="modal-content-body" class="mt-5"></div>
        </div>
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-secondary"
            data-bs-dismiss="modal"
          >
            Close
          </button>
        </div>
      </div>
    </div>
  </div>

  <!-- Modal -->
  <div
    class="modal fade"
    id="dataProductCustomer"
    tabindex="-1"
    aria-labelledby="dataProductCustomerLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-xl modal-dialog-scrollable">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title fs-3" id="dataProductCustomerLabel">
            Pilih Produk
          </h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <table class="table table-hover">
            <thead>
              <tr>
                <th>Gambar</th>
                <th>Nomor PLU</th>
                <th>Item</th>
                <th>Harga</th>
                <th></th>
              </tr>
            </thead>
            <tbody id="tbody-customer-item-table"></tbody>
          </table>
        </div>
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-secondary"
            data-bs-dismiss="modal"
          >
            Close
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
          <p>Pastikan data yang anda masukkan sudah benar!</p>
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
    let DATA_LOCATION = [];
    let DATA_SALES = [];
    let FRAME_COLOR = [];
    let DATA_ITEM = {};
    let DATA_STONE = [];
    let DATA_HISTORY = [];
    let DATA_LOCATION_REPARASI = [];
    let DATA_PAKET_REPARASI = [];
    let DATA_SATUAN_TAMBAHAN = [];
    let BUTTON_SUBMIT_FORM = $("#btn-confirmation-submit");
    let BUTTON_CANCEL_FORM = $("#btn-cancel-form");
    const SELECT_SEARCH_WITH = $("#select-search-with");
    const SELECT_LOCATION = $("#select-location");
    const SELECT_FRAME_COLOR = $("#select-frame-color");
    const SELECT_LOKASI_REPARASI = $("#select-lokasi-reparasi");
    const SELECT_PAKET_UTAMA = $("#select-paket-utama");
    const SELECT_SATUAN_TAMBAHAN = $("#select-satuan-tambahan");
    const SELECT_SALES = $("#select-jr");
    const BUTTON_SEARCH_PRODUCT = $("#btn-search-product");
    const BUTTON_SEARCH_CUSTOMER = $("#btn-search-customer");
    const BUTTON_MODAL_CONFIRMATION = $("#btn-modal-confirmation");
    const INPUT_SEARCH_PRODUCT = $("#input-search-product");
    const INPUT_KEYWORD_CUSTOMER = $("#input-keyword-customer");
    const INPUT_CUSTOMER_LAMA = $("#input-customer-lama");
    const ELEMENT_CONTENT = $("#content-after-plu-fetched");
    const ELEMENT_CONTENT_SIZE_REPARASI = $("#content-size-reparasi");
    const ELEMENT_CONTENT_SIZE_REPARASI_PLUS_MINUS = $(
      "#content-plus-minus-size"
    );
    const ELEMENT_CONTENT_LABEL_SIZE_REPARASI_PLUS_MINUS = $(
      "label[for='input-size-qc-plus-minus']"
    );
    const INPUT_SIZE_REPARASI_PLUS_MINUS = $("#input-size-qc-plus-minus");

    toastr.options = {
      closeButton: true,
      progressBar: true,
      positionClass: "toast-bottom-center",
      showDuration: "300",
      hideDuration: "1000",
      timeOut: "5000",
    };

    function selectToDefault(element, isMultiple) {
      element.children().each((idx, elem) => {
        if (idx === 0) {
          element.val(isMultiple ? "" : $(elem).val()).trigger("change");
        }
      });
    }

    function checkPaketUtama(text = "") {
      if (!text.includes("Perbesar") && !text.includes("Perkecil")) {
        return false;
      }

      if (!text.toUpperCase().includes("BANGLE")) {
        const splittedText = text.split(" ").join("@").split("@").pop();
        const getMinMax = splittedText.split("-");

        const min = getMinMax.shift();
        const max = getMinMax.pop();

        return { min: parseFloat(min) - 0.5, max: parseInt(max) };
      }

      return { min: 0.5, max: 999999999 };
    }

    function onChangeMainPackage() {
      if (SELECT_PAKET_UTAMA.val() == "0") {
        $("#input-size-qc-actual").val($("#input-size-qc-old").val());
        ELEMENT_CONTENT_SIZE_REPARASI_PLUS_MINUS.addClass("d-none");
        return;
      }

      ELEMENT_CONTENT_SIZE_REPARASI_PLUS_MINUS.addClass("d-none");

      const jenisReparasi = $(
        SELECT_PAKET_UTAMA.select2("data")["element"]
      ).attr("data-jenis-reparasi");

      const isPlusMinusPackage = checkPaketUtama(jenisReparasi);

      if (!isPlusMinusPackage) {
        $("#input-size-qc-actual").val($("#input-size-qc-old").val());
        return;
      }

      const plusOrMinus = jenisReparasi.includes("Perbesar")
        ? "Perbesar"
        : "Perkecil";

      ELEMENT_CONTENT_SIZE_REPARASI_PLUS_MINUS.removeClass("d-none");
      ELEMENT_CONTENT_LABEL_SIZE_REPARASI_PLUS_MINUS.text(
        `${plusOrMinus} Size`
      );

      const { min, max } = isPlusMinusPackage;

      const sizeData = $("#input-size-qc-old").val();
      if (sizeData == "" || sizeData == "0") {
        toastr.error("Pastikan size Actual terisi!");
      }

      INPUT_SIZE_REPARASI_PLUS_MINUS.prop("min", min)
        .prop("max", max)
        .val(min)
        .attr("data-calculator", plusOrMinus === "Perbesar" ? "+" : "-");
      const valueQcSize = parseFloat($("#input-size-qc-old").val());

      const calculate =
        valueQcSize === 0
          ? 0
          : plusOrMinus === "Perbesar"
          ? valueQcSize + min
          : valueQcSize - min;

      const actual = calculate < 0 ? 0 : calculate;
      $("#input-size-qc-actual").val(Math.abs(actual));
    }

    function defaultForm({ isSearchProduct = false } = {}) {
      const arrOfElement = [
        { element: SELECT_LOCATION, isMultiple: false },
        { element: SELECT_SEARCH_WITH, isMultiple: false },
        { element: SELECT_LOKASI_REPARASI, isMultiple: false },
        { element: SELECT_PAKET_UTAMA, isMultiple: false },
        { element: SELECT_SATUAN_TAMBAHAN, isMultiple: true },
      ];

      if (isSearchProduct) {
        arrOfElement.forEach((e, index) => {
          if (index === 0 || index === 1) return;
          selectToDefault(e.element, e.isMultiple);
        });
      } else {
        arrOfElement.forEach((e) => selectToDefault(e.element, e.isMultiple));
      }

      const formValues = [
        { name: "input-radio-grafir", defaultValue: "Ada" },
        { name: "input-radio-logo", defaultValue: "Ada" },
        { name: "input-radio-rangka", defaultValue: "Baik" },
        { name: "input-radio-tes-keaslian", defaultValue: "Ya" },
      ];

      formValues.forEach((item) => {
        $(`input[name='${item.name}']`).each((_, elem) => {
          $(elem).val() === item.defaultValue
            ? $(elem).prop("checked", true)
            : null;
        });
      });

      $("#form-textarea-keterangan").val("");
      $("#form-textarea-keterangan-reparasi").val("");
      $("#input-file-foto-product").val(null);
      $("#input-search-product").val("");

      ELEMENT_CONTENT.addClass("d-none");
    }

    function formatNumberToIDR(number) {
      const IDR = new Intl.NumberFormat("id-ID", {
        style: "currency",
        currency: "IDR",
      }).format(number);
      return IDR;
    }

    function onInputNumber(e) {
      const element = $(e.currentTarget);
      const value = parseFloat(element.val());

      return function checkInputNumber(
        isWithMinMax,
        isAllowNegativeValue,
        isWithStep = false
      ) {
        if (isNaN(value) && !isWithMinMax) {
          element.val(0).trigger("change");
          return;
        }

        if (isNaN(value) && isWithMinMax) {
          const min = parseFloat(element.attr("min"));
          element.val(min).trigger("change");
        }

        if (value === 0 && !isWithMinMax) return;
        if (value === 0 && isWithMinMax) {
          const min = parseFloat(element.attr("min"));
          element.val(min).trigger("change");
        }

        if (isWithStep) {
          const step = parseFloat(element.attr("step"));
          const isValidStep = value % step === 0;

          if (!isValidStep) {
            const roundedValue = Math.round(value / step) * step;
            element.val(roundedValue).trigger("change");
          }
        }

        if (!isAllowNegativeValue && !isWithMinMax) {
          const makeItAbsolute = value < 0 ? Math.abs(value) : value;
          if (value > 0) return;
          element.val(makeItAbsolute).trigger("change");
        }

        if (isWithMinMax) {
          const min = parseFloat(element.attr("min"));
          const max = parseFloat(element.attr("max"));

          if (min || max) {
            const makeItMaxValue =
              value < min ? min : value > max ? max : value;

            if (makeItMaxValue === value) return;
            element.val(makeItMaxValue).trigger("change");
          }
        }
      };
    }

    function onChangeSearchWith(e) {
      const currentTarget = e.currentTarget;
      const selectValue = $(currentTarget).val();

      if (selectValue === "1") {
        $("#input-search-product").attr("disabled", false);
        $("#btn-modal-search-customer").addClass("d-none");
        $("#btn-search-product").removeClass("d-none");
        $("#input-search-product").val("");
        return;
      }

      $("#input-search-product").val(DATA_ITEM["NOMOR"] ?? "");
      $("#input-search-product").attr("disabled", true);
      $("#btn-modal-search-customer").removeClass("d-none");
      $("#btn-search-product").addClass("d-none");
    }

    function onChangeLocation(e) {
      const currentTarget = e.currentTarget;

      const selectDataAttribute = $(currentTarget).find(":selected").data();
      const select2Sales = SELECT_SALES.select2();
      const kodeStore = selectDataAttribute["kodeLokasi"];
      select2Sales.empty();
      const mapSales = DATA_SALES.filter(
        (item) => item["KodeOutlet"] === kodeStore
      ).forEach(
        ({
          IDLokasi,
          NamaOutlet,
          KodeOutlet,
          TipeLokasi,
          NamaSales,
          NIK,
          IDSales,
        }) => {
          let optionSales = new Option(NamaSales, IDSales, false, false);
          optionSales.setAttribute("data-id-lokasi", IDLokasi);
          optionSales.setAttribute("data-nama-lokasi", NamaOutlet);
          optionSales.setAttribute("data-kode-lokasi", KodeOutlet);
          optionSales.setAttribute("data-tipe-lokasi", TipeLokasi);
          optionSales.setAttribute("data-nama-sales", NamaSales);
          optionSales.setAttribute("data-id-sales", IDSales);
          optionSales.setAttribute("data-nik-sales", NIK);
          select2Sales.append(optionSales);
        }
      );

      $.ajax({
        type: "GET",
        url: "./index.aspx/GetLocationTujuan",
        data: { kodeStore: `'${selectDataAttribute["kodeLokasi"]}'` },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];

          const helperTextElement = $("#text-helper-location-reparasi");

          if (response.length === 0) {
            SELECT_LOKASI_REPARASI.children().each((index, element) => {
              if ($(element).attr("data-id") == 2) {
                $(element).attr("disabled", true);
              }
            });

            helperTextElement
              .html(`* Store ${kodeStore} tidak memiliki service point`)
              .addClass("text-danger")
              .removeClass("text-success");
            SELECT_LOKASI_REPARASI.val(1).trigger("change");
            return;
          }

          SELECT_LOKASI_REPARASI.children().each((index, element) => {
            $(element).attr("disabled", false);
          });

          const servicePointLocation = response
            .map((item) => item["NamaOutlet"])
            .join(",");
          helperTextElement
            .html("Service point terdaftar pada store: " + servicePointLocation)
            .addClass("text-success")
            .removeClass("text-danger");
        },
        error: function (error) {
          console.log(error);
        },
        complete: function (data) {},
      });

      select2Sales.trigger("change");
    }

    function onChangeLocRepair(e) {
      const currentTarget = e.currentTarget;
      const element = $(currentTarget);
      const value = element.val();

      ELEMENT_CONTENT_SIZE_REPARASI_PLUS_MINUS.addClass("d-none");
      $("#input-size-qc-actual").val($("#input-size-qc-old").val());

      const select2PackageRepair = SELECT_PAKET_UTAMA.select2();
      const select2NormalRepair = SELECT_SATUAN_TAMBAHAN.select2();

      select2PackageRepair.empty();
      select2NormalRepair.empty();

      select2PackageRepair.append(
        new Option("-- Pilih Paket Utama --", 0, false, false)
      );

      const mapLocation = DATA_PAKET_REPARASI.filter(
        (item) => item["IDLocReparasi"] == value && item["TipeReparasi"] === 1
      ).forEach(
        (
          {
            IDLocReparasi,
            IDLocPackageReparasi,
            IDPackage,
            JenisReparasi,
            Kode,
            Nama,
            NamaTipeReparasi,
            Ongkos,
            TipeReparasi,
          },
          index
        ) => {
          let optionPackageRepair = new Option(
            JenisReparasi,
            IDPackage,
            false,
            false
          );
          optionPackageRepair.setAttribute(
            "data-id-loc-reparasi",
            IDLocReparasi
          );
          optionPackageRepair.setAttribute(
            "data-id-loc-package-reparasi",
            IDLocPackageReparasi
          );
          optionPackageRepair.setAttribute("data-id-package", IDPackage);
          optionPackageRepair.setAttribute(
            "data-jenis-reparasi",
            JenisReparasi
          );
          optionPackageRepair.setAttribute("data-kode", Kode);
          optionPackageRepair.setAttribute("data-nama", Nama);
          optionPackageRepair.setAttribute(
            "data-nama-tipe-reparasi",
            NamaTipeReparasi
          );
          optionPackageRepair.setAttribute("data-ongkos", Ongkos);
          optionPackageRepair.setAttribute("data-tipe-reparasi", TipeReparasi);
          select2PackageRepair.append(optionPackageRepair);
        }
      );

      const mapLocation2 = DATA_PAKET_REPARASI.filter(
        (item) => item["IDLocReparasi"] == value && item["TipeReparasi"] === 2
      ).forEach(
        (
          {
            IDLocReparasi,
            IDLocPackageReparasi,
            IDPackage,
            JenisReparasi,
            Kode,
            Nama,
            NamaTipeReparasi,
            Ongkos,
            TipeReparasi,
          },
          index
        ) => {
          let optionNormalRepair = new Option(
            JenisReparasi,
            IDPackage,
            false,
            false
          );
          optionNormalRepair.setAttribute(
            "data-id-loc-reparasi",
            IDLocReparasi
          );
          optionNormalRepair.setAttribute(
            "data-id-loc-package-reparasi",
            IDLocPackageReparasi
          );
          optionNormalRepair.setAttribute("data-id-package", IDPackage);
          optionNormalRepair.setAttribute("data-jenis-reparasi", JenisReparasi);
          optionNormalRepair.setAttribute("data-kode", Kode);
          optionNormalRepair.setAttribute("data-nama", Nama);
          optionNormalRepair.setAttribute(
            "data-nama-tipe-reparasi",
            NamaTipeReparasi
          );
          optionNormalRepair.setAttribute("data-ongkos", Ongkos);
          optionNormalRepair.setAttribute("data-tipe-reparasi", TipeReparasi);
          select2NormalRepair.append(optionNormalRepair);
        }
      );

      select2PackageRepair.trigger("change");
      select2NormalRepair.trigger("change");

      if (Object.keys(DATA_ITEM).length !== 0) {
        // Check kalo item nya bukan bangle atau ring, input disable
        if (
          DATA_ITEM["ITEM"].toUpperCase() === "EARRINGS" ||
          (DATA_ITEM["ITEM"].toUpperCase() !== "EARRINGS" &&
            !DATA_ITEM["ITEM"].toUpperCase().includes("BANGLE") &&
            !DATA_ITEM["ITEM"].toUpperCase().includes("RING") &&
            !DATA_ITEM["ITEM"].toUpperCase().includes("MENS JEWELLERY"))
        ) {
          SELECT_PAKET_UTAMA.children().each((_, elem) => {
            const textContent = $(elem).text().toUpperCase();

            if (
              textContent.includes("PERBESAR") ||
              textContent.includes("PERKECIL")
            ) {
              $(elem).attr("disabled", true);
            }
          });
        } else {
          SELECT_PAKET_UTAMA.children().each((_, elem) => {
            const textContent = $(elem).text().toUpperCase();

            if (DATA_ITEM["ITEM"].toUpperCase().includes("BANGLE")) {
              if (
                textContent.includes("PERBESAR RING SIZE") ||
                textContent.includes("PERKECIL RING SIZE")
              ) {
                $(elem).attr("disabled", true);
              }
            } else if (
              DATA_ITEM["ITEM"].toUpperCase() !== "EARRINGS" &&
              (DATA_ITEM["ITEM"].toUpperCase().includes("RING") ||
                DATA_ITEM["ITEM"].toUpperCase().includes("MENS JEWELLERY"))
            ) {
              if (
                textContent.includes("PERBESAR BANGLE") ||
                textContent.includes("PERKECIL BANGLE")
              ) {
                $(elem).attr("disabled", true);
              }
            } else {
              $(elem).attr("disabled", false);
            }
          });
        }
      }
    }

    function fetchProductToRepair({ isPLU, plu }) {
      DATA_HISTORY = [];
      DATA_ITEM = {};
      DATA_STONE = [];

      defaultForm({ isSearchProduct: true });
      $.ajax({
        type: "GET",
        url: "index.aspx/GetCheckPLUQC",
        contentType: "application/json; charset=utf-8",
        data: {
          Keyword: `"${plu}"`,
        },
        success: function (data) {
          const response = data["d"];

          if (typeof response === "string") {
            toastr.error(response);
            return;
          }

          const item = response["Item"];

          if (item["ID"] === 0) {
            toastr.error("PLU tidak ditemukan...");
            INPUT_SEARCH_PRODUCT.val("");
            return;
          }
          const stone = response["Stone"];
          const history = response["History"];

          DATA_ITEM = item;
          DATA_STONE = stone;
          DATA_HISTORY = history["detailHistoryRepair"].reduce(
            (accumulator, calculator) => {
              const {
                IDDocRepair,
                NomorRepair,
                SizeAwal,
                SizeAkhir,
                BeratAwal,
                BeratAkhir,
                Keterangan,
              } = calculator;
              const filterPackage = history["packageRepair"].filter(
                (item) => item["IDDocRepair"] === IDDocRepair
              );
              accumulator.push({ ...calculator, package: filterPackage });
              return accumulator;
            },
            []
          );

          $("#content-history-reparasi").empty();
          if (DATA_HISTORY.length === 0)
            $("#content-history-reparasi").html(`
              <div class="w-100 d-flex justify-content-center align-items-center flex-column">
                <img src="/Images/png/not-found.png" width="200px" height="200px" />
                <div class="mt-5 fw-bold">Belum ada history reparasi.</div>
              </div>
            `);

          if (DATA_HISTORY.length > 0) {
            DATA_HISTORY.forEach(
              ({
                IDDocRepair,
                NomorRepair,
                SizeAwal,
                SizeAkhir,
                BeratAwal,
                BeratAkhir,
                Keterangan,
                package,
              }) => {
                $("#content-history-reparasi").append(`
              <table>
                <tbody id="tbody-history-reparasi">
                  <tr class="border-bottom border-top">
                    <td class="py-3">
                      <h4 class="fw-bold text-primary fs-2">
                      ${NomorRepair}
                      </h4>
                      <ul>
                      ${package.reduce((accumulator, calculator) => {
                        const { JenisReparasi } = calculator;
                        accumulator += `
                          <li>${JenisReparasi}</li>
                        `;
                        return accumulator;
                      }, "")}
                      </ul>
                      <div class="my-2">
                      ${Keterangan}
                      </div>
                      <div class="d-flex gx-5" style="gap: 30px">
                        <div class="d-flex gx-5" style="gap: 20px">
                          <div class="d-flex gx-5" style="gap: 5px">
                            <span>Berat Awal:</span
                            ><span class="fw-bold">${BeratAwal}</span>
                          </div>
                          <div class="d-flex gx-5" style="gap: 5px">
                            <span>Berat Akhir:</span
                            ><span class="fw-bold">${BeratAkhir}</span>
                          </div>
                        </div>
                        <span>|</span>
                        <div class="d-flex gx-5" style="gap: 20px">
                          <div class="d-flex gx-5" style="gap: 5px">
                            <span>Size Awal:</span><span class="fw-bold">${SizeAwal}</span>
                          </div>
                          <div class="d-flex gx-5" style="gap: 5px">
                            <span>Size Akhir:</span
                            ><span class="fw-bold">${SizeAkhir}</span>
                          </div>
                        </div>
                      </div>
                    </td>
                  </tr>
                </tbody>
              </table>
            `);
              }
            );
          }

          const INVOICE_TEXT_ELEMENT = $("#invoice-text-element");
          const NOMOR_TEXT_ELEMENT = $("#nomor-text-element");
          const ITEM_TEXT_ELEMENT = $("#item-text-element");
          const IMAGE_PRODUCT_ELEMENT = $("#img-product-element");
          const INPUT_SIZE_QC_OLD = $("#input-size-qc-old");
          const INPUT_SIZE_QC_ACTUAL = $("#input-size-qc-actual");
          const INPUT_WEIGHT_QC_OLD = $("#input-weight-qc-old");
          const INPUT_WEIGHT_QC_ACTUAL = $("#input-weight-qc-actual");
          const INPUT_WEIGHT_QC_REPAIR = $("#input-weight-qc-repair");
          const TABLE_BODY_STONE_TABLE = $("#tbody-detail-stone-table");
          const grossWeight = parseFloat(
            item["GROSS_WEIGHT"].split(",").join(".")
          ).toFixed(2);

          INVOICE_TEXT_ELEMENT.text(item["NOMORINVOICE"]);
          NOMOR_TEXT_ELEMENT.text(item["NOMOR"]);
          ITEM_TEXT_ELEMENT.text(item["ITEM"]);
          IMAGE_PRODUCT_ELEMENT.attr(
            "src",
            `/ImageCrop.aspx?path=${item["IMAGE"]}`
          );
          SELECT_FRAME_COLOR.val(
            item["FRAME_COLOR_ID"] || FRAME_COLOR[0]["ID"]
          ).trigger("change");
          INPUT_WEIGHT_QC_OLD.val(grossWeight);
          INPUT_WEIGHT_QC_ACTUAL.val(grossWeight);
          INPUT_WEIGHT_QC_REPAIR.val(grossWeight);
          $("#input-size-brj").val(item["SIZE"]);
          INPUT_SIZE_QC_OLD.val(item["SIZE"]);
          INPUT_SIZE_QC_ACTUAL.val(item["SIZE"]);

          // Check kalo Size nya 0 boleh isi sendiri
          // if (INPUT_SIZE_QC_OLD.val() == 0) {
          // INPUT_SIZE_QC_OLD.prop("disabled", false);
          // INPUT_SIZE_QC_ACTUAL.prop("disabled", false);
          // }

          // Check kalo item nya bukan bangle atau ring, input disable
          if (
            item["ITEM"].toUpperCase() === "EARRINGS" ||
            (item["ITEM"].toUpperCase() !== "EARRINGS" &&
              !item["ITEM"].toUpperCase().includes("BANGLE") &&
              !item["ITEM"].toUpperCase().includes("RING") &&
              !item["ITEM"].toUpperCase().includes("MENS JEWELLERY"))
          ) {
            INPUT_SIZE_QC_OLD.hide().parent().hide();
            INPUT_SIZE_QC_ACTUAL.hide().parent().hide();
            $("#input-size-brj").hide().parent().hide();

            SELECT_PAKET_UTAMA.children().each((_, elem) => {
              const textContent = $(elem).text().toUpperCase();

              if (
                textContent.includes("PERBESAR") ||
                textContent.includes("PERKECIL")
              ) {
                $(elem).attr("disabled", true);
              }
            });
          } else {
            INPUT_SIZE_QC_OLD.show().parent().show();
            INPUT_SIZE_QC_ACTUAL.show().parent().show();
            $("#input-size-brj").show().parent().show();

            SELECT_PAKET_UTAMA.children().each((_, elem) => {
              const textContent = $(elem).text().toUpperCase();

              if (item["ITEM"].toUpperCase().includes("BANGLE")) {
                if (
                  textContent.includes("PERBESAR RING SIZE") ||
                  textContent.includes("PERKECIL RING SIZE")
                ) {
                  $(elem).attr("disabled", true);
                }
              } else if (
                item["ITEM"].toUpperCase() !== "EARRINGS" &&
                (item["ITEM"].toUpperCase().includes("RING") ||
                  item["ITEM"].toUpperCase().includes("MENS JEWELLERY"))
              ) {
                if (
                  textContent.includes("PERBESAR BANGLE") ||
                  textContent.includes("PERKECIL BANGLE")
                ) {
                  $(elem).attr("disabled", true);
                }
              } else {
                $(elem).attr("disabled", false);
              }
            });
          }

          ELEMENT_CONTENT.removeClass("d-none");
          if (item["TYPE"] === "PG") {
            $("#content-detail-stone").addClass("d-none");
            return;
          }

          $("#content-detail-stone").removeClass("d-none");
          const additionalString = stone.reduce(
            (accumulator, calculator, index) => {
              const {
                ID,
                STONETYPE,
                STONETYPEMAP,
                STONE,
                TGL,
                TOTAL_BUTIR,
                TOTAL_CARAT,
                GIA,
                BRANDSTONE,
                CLARITY,
                COLOR,
              } = calculator;

              const totalCarat = parseFloat(
                TOTAL_CARAT.split(",").join(".")
              ).toFixed(3);

              accumulator += `
              <tr data-stone-id=${ID}>
                <td>${index + 1}</td>
                <td>${STONE} - ${COLOR} - ${CLARITY}</td>
                <td id="total_butir_data_${ID}">${TOTAL_BUTIR}</td>
                <td><input class="form-control" value="${TOTAL_BUTIR}" id="total_butir_${ID}" data-id="${ID}" data-input="TotalButirActual" type="number" /></td>
                <td id="total_carat_data_${ID}">${totalCarat}</td>
                <td><input disabled class="form-control" value="${totalCarat}" id="total_carat_${ID}" data-id="${ID}" data-input="TotalCaratActual" type="number" /></td>
                <td style="display:none;">
                  <div class="d-flex justify-content-center align-items-center">
                    <div class="form-check">
                      <input class="form-check-input" type="checkbox" id="kualitas_${ID}" data-id="${ID}" data-input="TesKualitas" value="SESUAI" checked />
                    </div>
                  </div>
                </td>
                <td>
                  <div class="d-flex justify-content-center align-items-center">
                    <div class="form-check">
                      <input class="form-check-input" type="checkbox" id="keaslian_${ID}" data-id="${ID}" data-input="TesKeaslian" value="SESUAI" checked />
                    </div>
                  </div>
                </td>
                <td>
                  <div class="d-flex justify-content-center align-items-center">
                    <div class="form-check">
                      <input class="form-check-input" type="checkbox" id="sertifikat_${ID}" data-id="${ID}" data-input="SesuaiSertifikat" value="SESUAI" checked />
                    </div>
                  </div>
                </td>
                <td>
                  <div class="d-flex justify-content-center align-items-center flex-column" id="content_keterangan_${ID}">
                      <input class="form-control" type="text" id="keterangan_${ID}" data-id="${ID}" data-input="Keterangan" value="" />
                      <input class="form-control" type="hidden" id="stone_type_${ID}" data-id="${ID}" data-input="StoneType" value="${STONETYPEMAP}" />
                  </div>
                </td>
              </tr>
            `;

              return accumulator;
            },
            ""
          );

          TABLE_BODY_STONE_TABLE.empty().append(additionalString);

          if (!isPLU) {
            $("#input-search-product").val(plu);
          }
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          if (isPLU) {
            BUTTON_SEARCH_PRODUCT.prop("disabled", false).text("Cari Product");
            return;
          }

          $("#tbody-customer-item-table button")
            .prop("disabled", false)
            .text("Pilih");

          $("#dataProductCustomer").modal("hide");
        },
      });
    }

    function onFetchProductByPlu() {
      const value = INPUT_SEARCH_PRODUCT.val();
      if (value === "") {
        toastr.error("Masukkan Nomor PLU terlebih dahulu...");
        return;
      }

      BUTTON_SEARCH_PRODUCT.prop("disabled", true).text("Loading...");
      fetchProductToRepair({ isPLU: true, plu: value });
    }

    function onFetchProductByCustomer(e) {
      $("#tbody-customer-item-table button")
        .prop("disabled", true)
        .text("Loading...");

      fetchProductToRepair({
        isPLU: false,
        plu: $(e.currentTarget).attr("data-plu"),
      });
    }

    function getTableForm() {
      let result = [];

      $("#table-stone tbody tr").each(function () {
        var allValues = {};
        $(this)
          .find("input")
          .each(function (index) {
            const fieldName = $(this).attr("id");
            const element = $("#" + fieldName);
            allValues["IDStone"] = parseInt(element.attr("data-id"));

            if (
              fieldName.includes("keaslian") ||
              fieldName.includes("kualitas")
            )
              allValues[$(element).attr("data-input")] = $(this).is(":checked")
                ? "SESUAI"
                : "TIDAK SESUAI";
            else if (fieldName.includes("butir"))
              allValues[$(element).attr("data-input")] = parseInt(
                $(this).val()
              );
            else if (fieldName.includes("carat"))
              allValues[$(element).attr("data-input")] = parseFloat(
                $(this).val()
              ).toFixed(3);
            else if (fieldName.includes("sertifikat"))
              allValues[$(element).attr("data-input")] = $(this).is(":checked")
                ? 1
                : 0;
            else allValues[$(element).attr("data-input")] = $(this).val();
          });
        result.push(allValues);
      });

      return result;
    }

    function onSubmitFormRepair(args = { imagePicture: "" }) {
      const select2LocationElement = $(
        SELECT_LOCATION.select2("data")["element"]
      );
      const select2SalesElement = $(SELECT_SALES.select2("data")["element"]);

      const select2PackageElement = $(
        SELECT_PAKET_UTAMA.select2("data")["element"]
      );

      const select2OneElement = $(SELECT_SATUAN_TAMBAHAN.select2("data"));

      let tempPackage = [];
      select2OneElement.each((item, el) => {
        tempPackage.push($(el["element"]).text());
      });

      if (tempPackage.includes(select2PackageElement.text())) {
        toastr.error(
          "Paket " +
            select2PackageElement.text() +
            " tidak boleh duplikat. Pilih salah satu di paket utama atau paket satuan!"
        );
        return;
      }

      const keterangan = $("#form-textarea-keterangan").val();
      const keteranganReparasi = $("#form-textarea-keterangan-reparasi").val();

      const selectedPackage = select2PackageElement.text().toUpperCase();

      const sizeQcOld = $("#input-size-qc-old").val();
      const sizeQcActual = $("#input-size-qc-old").val();

      if (
        (selectedPackage.includes("PERBESAR") ||
          selectedPackage.includes("PERKECIL")) &&
        (parseFloat(sizeQcOld) == 0 ||
          parseFloat($("#input-size-qc-actual").val()) == 0)
      ) {
        toastr.error("Pastikan size actual terisi!");
        return;
      }

      // if (keterangan === "") {
      //   toastr.error("Tolong isi keterangan!");
      //   return;
      // }

      const stoneDetail = DATA_STONE.reduce(
        (accumulator, calculator) => {
          accumulator.totalButir += calculator["TOTAL_BUTIR"];
          accumulator.totalCarat += calculator["TOTAL_CARAT"];
          return accumulator;
        },
        {
          totalButir: 0,
          totalCarat: 0,
        }
      );

      let package = [];

      if (select2PackageElement.val() != 0) {
        package.push({
          IDLocPackageReparasi: select2PackageElement.attr(
            "data-id-loc-package-reparasi"
          ),
        });
      }

      select2OneElement.each((item, el) => {
        package.push({
          IDLocPackageReparasi: $(el["element"]).attr(
            "data-id-loc-package-reparasi"
          ),
        });
      });

      let dummyData = []; // Change this if you have better way.

      if (getTableForm().length === 0) {
        dummyData.push({
          IDStone: 0,
          TotalButirActual: 0,
          TotalCaratActual: "0",
          TesKualitas: "",
          TesKeaslian: "",
          SesuaiSertifikat: 1,
          Keterangan: "",
          StoneType: "",
        });
      }

      const objectToSubmitDocQC = {
        QCNama: "",
        KodeCustLama: "",
        IDProduct: DATA_ITEM["ID"],
        Nomor: "",
        Blemish: $("input[name='input-radio-tipe']:checked").val(),
        JumlahButir: stoneDetail["totalButir"],
        Sertifikat: $("input[name='input-radio-tes-keaslian']:checked").val(), // Merk & Sertifikat = sama
        NonCZ: "",
        CZ: "",
        Logo: $("input[name='input-radio-logo']:checked").val(),
        KadarEmas: $("input[name='input-radio-grafir']:checked").val(),
        Merk: $("input[name='input-radio-tes-keaslian']:checked").val(), // Merk & Sertifikat = sama
        NetWeight: parseFloat($("#input-weight-qc-actual").val()),
        WarnaEmas: FRAME_COLOR.find(
          (item) => item.ID == SELECT_FRAME_COLOR.val()
        )["Nama"],
        FisikKeseluruhan: $("input[name='input-radio-rangka']:checked").val(),
        OperatorNama: "",
        Draft: 0,
        Keterangan: keterangan,
        GrossWeight: $("#input-weight-qc-actual").val(),
        QC_Additional: getTableForm().length > 0 ? getTableForm() : dummyData,
        TipeProduct: DATA_ITEM["TYPE"],
        KodeLokasi: select2LocationElement.attr("data-kode-lokasi"),
        PLU: DATA_ITEM["NOMOR"],
      };

      const objectToSubmitRepair = {
        PLU: DATA_ITEM["NOMOR"],
        Nomor: "",
        TipeLokasi: select2LocationElement.attr("data-tipe-lokasi"),
        IDLokasi: select2LocationElement.attr("data-id-lokasi"),
        IDCustomer: DATA_ITEM["IDCUSTOMER"],
        KodeLokasi: select2LocationElement.attr("data-kode-lokasi"),
        IDSales: select2SalesElement.attr("data-id-sales"),
        SalesNama: select2SalesElement.attr("data-nama-sales"),
        CustomerNama: DATA_ITEM["NAMACUSTOMER"],
        Operator: "",
        Keterangan: keteranganReparasi,
        ImgPicture: args.imagePicture,
        IDProduct: DATA_ITEM["ID"],
        IDProductTitipan: 0,
        EstimasiHarga: 0,
        IDQC: 0,
        IDQCTitipan: 0,
        TipeProduct: DATA_ITEM["TYPE"],
        IDLocReparasi: SELECT_LOKASI_REPARASI.val(),
        DocRepair_Package: package,
        QC_Additional: getTableForm().length > 0 ? getTableForm() : dummyData,
        Weight: $("#input-weight-qc-actual").val(), // $("#input-weight-qc-repair").val(),
        WeightData: $("#input-weight-qc-old").val(),
        WeightActual: $("#input-weight-qc-actual").val(),
        SizeAwal: $("#input-size-qc-old").val(),
        SizeAkhir: $("#input-size-qc-actual").val(),
        FrameColor: SELECT_FRAME_COLOR.val(),
      };

      BUTTON_MODAL_CONFIRMATION.prop("disabled", true).text("Loading...");
      BUTTON_CANCEL_FORM.prop("disabled", true).text("Loading...");

      $.ajax({
        type: "POST",
        url: "./index.aspx/CreateDocRepair",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
          args: {
            insertDocQCParams: objectToSubmitDocQC,
            insertDocRepairParams: objectToSubmitRepair,
          },
        }),
        success: function (data) {
          toastr.success("Sukses membuat form reparasi!");

          setTimeout(() => {
            window.location.reload();
          }, 500);
          // defaultForm();
        },
        error: function (error) {
          toastr.error("Terjadi kesalahan saat membuat form reparasi!");
          console.error(error);
        },
        complete: function (data) {
          BUTTON_MODAL_CONFIRMATION.prop("disabled", false).text("Submit");
          BUTTON_CANCEL_FORM.prop("disabled", false).text("Batal");
        },
      });

      BUTTON_MODAL_CONFIRMATION.prop("disabled", false).text("Submit");
      BUTTON_CANCEL_FORM.prop("disabled", false).text("Batal");
    }

    function onFetchCustomer() {
      const value = INPUT_KEYWORD_CUSTOMER.val();

      if (value === "") {
        toastr.error("Masukkan keyword terlebih dahulu..");
        return;
      }

      BUTTON_SEARCH_CUSTOMER.prop("disabled", true).text("Loading...");

      $.ajax({
        type: "GET",
        url: "index.aspx/GetDataCustomer",
        contentType: "application/json; charset=utf-8",
        data: {
          keyword: `"${value}"`,
          searchby: 0,
        },
        success: function (data) {
          const MODAL_CONTENT = $("#modal-content-body");
          const response = data["d"];

          if (response.length === 0) {
            MODAL_CONTENT.html("<h1>Data Tidak Ditemukan...</h1>");
            return;
          }

          function startingHtml(rowContent) {
            let html = `
            <table class="table table-hover table-sm">
              <thead>
                <tr>
                  <th>Nama Customer</th>
                  <th>Kode Customer</th>
                  <th>Email</th>
                  <th>No Telp</th>
                  <th>Date of Birth</th>
                  <th>Membership</th>
                  <th>Action</th>
                </tr>
              </thead>
              <tbody id="tbody-customer-table">
                ${rowContent}
              </tbody>
            </table>
          `;

            return html;
          }

          const additionalString = response.reduce(
            (accumulator, calculator) => {
              const {
                ID,
                CustomerStampsID,
                CustomerMembership,
                CustomerCode,
                CustomerBirthday,
                CustomerName,
                CustomerPhone,
                CustomerEmail,
              } = calculator;
              accumulator += `
              <tr>
                <td>${CustomerName}</td>
                <td>${CustomerCode}</td>
                <td>${CustomerEmail}</td>
                <td>${CustomerPhone}</td>
                <td>${CustomerBirthday}</td>
                <td>${CustomerMembership}</td>
                <td><button class="btn btn-primary btn-lg" data-id-customer="${ID}">Pilih</button></td>
              </tr>
            `;

              return accumulator;
            },
            ""
          );

          const html = startingHtml(additionalString);
          MODAL_CONTENT.html(html);
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          BUTTON_SEARCH_CUSTOMER.prop("disabled", false).text("Cari Customer");
        },
      });
    }

    function onFetchItemCustomer(e) {
      const element = $(e.currentTarget);
      const id = $(element).attr("data-id-customer");

      $("#tbody-customer-table button")
        .text("Loading...")
        .prop("disabled", true);

      $.ajax({
        type: "GET",
        url: "./index.aspx/GetDataCustomerItem",
        data: { IDCustomer: id, KodeCustLama: "" },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];
          $("#dataProductCustomer").modal("show");
          const tableContent = response.reduce((accumulator, calculator) => {
            const { Image, Item, Nomor, HargaBeli, IDProduct } = calculator;
            accumulator += `
              <tr>
                <td><img src="https://api-frank.cmk.co.id/api/image?img=${Image}" style="width: 50px; height: 50px;" /></td>
                <td>${Nomor}</td>
                <td>${Item}</td>
                <td>${formatNumberToIDR(HargaBeli)}</td>
                <td><button class="btn btn-primary btn-lg" data-plu="${Nomor}" data-id="${IDProduct}">Pilih</button></td>
              </tr>
            `;

            return accumulator;
          }, "");

          $("#tbody-customer-item-table").html(tableContent);
          $("#exampleModal").modal("hide");
        },
        error: function (error) {
          console.log(error);
        },
        complete: function (data) {
          $("#tbody-customer-table button")
            .text("Pilih")
            .prop("disabled", false);
        },
      });
    }

    function getFormattedTimestamp() {
      const now = new Date();

      // Helper function to pad numbers with leading zeros
      const padZero = (num) => num.toString().padStart(2, "0");

      const year = now.getFullYear();
      const month = padZero(now.getMonth() + 1); // Months are zero-indexed
      const day = padZero(now.getDate());
      const hours = padZero(now.getHours());
      const minutes = padZero(now.getMinutes());
      const seconds = padZero(now.getSeconds());

      return `${year}${month}${day}${hours}${minutes}${seconds}`;
    }

    function onSubmitFormAndUploadFile() {
      $("#tbody-detail-stone-table tr").each((_, row) => {
        const rowId = $(row).attr("data-stone-id");

        const butirData = $(`#total_butir_data_${rowId}`).text();
        const butirActual = $(`#total_butir_${rowId}`).val();
        const keterangan = $(`#keterangan_${rowId}`).val();

        if (butirData != butirActual && keterangan == "") {
          $(`#keterangan_${rowId}`).addClass("is-invalid");
          $(`#feedback_${rowId}`).remove();
          $(`#content_keterangan_${rowId}`).append(
            `<span class="invalid-feedback" id="feedback_${rowId}" style="font-size: 10px;">Jika butir data & actual berbeda keterangan wajib di isi!</span>`
          );
        }
      });

      const fileInput = $("#input-file-foto-product")[0];
      const select2PackageElement = $(
        SELECT_PAKET_UTAMA.select2("data")["element"]
      );

      const select2OneElement = $(SELECT_SATUAN_TAMBAHAN.select2("data"));

      if (!fileInput.files[0]) {
        toastr.error("File product wajib di isi!");
        return;
      }

      if (select2PackageElement.val() == 0 && select2OneElement.length === 0) {
        toastr.error("Tolong pilih paket satuan atau paket utama!");
        return;
      }

      const file = fileInput.files[0];

      var allowedTypeExtension = ["jpeg", "jpg", "png"];
      const fileName = file["name"];

      const fileExtension = fileName.split(".").pop().toLowerCase();

      if (!allowedTypeExtension.includes(fileExtension)) {
        toastr.error(
          "Only types with these extension are allowed: " +
            allowedTypeExtension.join(", ")
        );
        return;
      }

      const reader = new FileReader();

      reader.onload = function (e) {
        // Get the Base64 string of the image
        const imageData = e.target.result.split(",")[1];

        // Send the data to the WebMethod
        $.ajax({
          type: "POST",
          url: "./index.aspx/UploadImage",
          data: JSON.stringify({
            imageData: imageData,
            fileName: getFormattedTimestamp() + "." + fileExtension,
          }),
          contentType: "application/json; charset=utf-8",
          dataType: "json",
          success: function (data) {
            const response = data["d"];

            if (response === "failed") {
              toastr.error("Terjadi kesalahan saat upload file.");
              return;
            }

            onSubmitFormRepair({ imagePicture: response });
          },
          error: function (data) {
            toastr.error("Terjadi kesalahan saat upload file.");
          },
        });
      };

      reader.readAsDataURL(file);

      // onSubmitFormRepair({ imagePicture: "" });
    }

    // Initializes while document is ready
    $(document).ready(function () {
      SELECT_SEARCH_WITH.select2();
      SELECT_FRAME_COLOR.select2();
      SELECT_LOKASI_REPARASI.select2();
      SELECT_PAKET_UTAMA.select2();
      SELECT_SATUAN_TAMBAHAN.select2();
    });

    // event handler
    $(document).ready(function () {
      SELECT_SEARCH_WITH.on("change", onChangeSearchWith);
      SELECT_PAKET_UTAMA.on("change", () => onChangeMainPackage());
      SELECT_LOCATION.on("change", onChangeLocation);
      SELECT_LOKASI_REPARASI.on("change", onChangeLocRepair);
      BUTTON_SEARCH_PRODUCT.on("click", () => onFetchProductByPlu());
      BUTTON_SEARCH_CUSTOMER.on("click", () => onFetchCustomer());
      BUTTON_SUBMIT_FORM.on("click", () => onSubmitFormAndUploadFile());
      BUTTON_CANCEL_FORM.on("click", () => defaultForm());

      $(document).on("change", "input[id*=keterangan_]", function (e) {
        const id = $(this).data("id");

        if ($(this).val() != "") {
          $(`#keterangan_${id}`).removeClass("is-invalid");
          $(`#feedback_${id}`).remove();
        }
      });

      $(document).on("change", 'input[id*="total_butir_"]', function (e) {
        const id = $(this).data("id");
        $(this).val(parseInt($(this).val()));
        const totalCaratData = parseFloat($(`#total_carat_data_${id}`).text());
        const totalButirData = parseInt($(`#total_butir_data_${id}`).text());
        const caratButir = totalCaratData / totalButirData;
        const elementTotalCarat = $(`#total_carat_${id}`);

        elementTotalCarat.val(
          (parseInt($(this).val()) * caratButir).toFixed(3)
        );
      });

      $(document).on("change", "#select-lokasi-reparasi", (e) =>
        onChangeLocRepair(e)
      );

      $(document).on(
        "change",
        'input[type="number"]:not([id*="input-size-qc"])',
        (e) => {
          const vallidateInput = onInputNumber(e);
          vallidateInput(false, false);
        }
      );

      $(document).on(
        "change",
        'input[type="number"][id*="input-size-qc-actual"]',
        (e) => {
          const vallidateInput = onInputNumber(e);
          vallidateInput(false, false, true);
        }
      );

      $(document).on("change", "#input-size-qc-plus-minus", (e) => {
        const vallidateInput = onInputNumber(e);
        vallidateInput(true, false, true);
      });

      INPUT_SIZE_REPARASI_PLUS_MINUS.on("change", function (e) {
        const sizeQcOld = $("#input-size-qc-old");
        const sizeQcActual = $("#input-size-qc-actual");

        if (sizeQcOld.val() == "" || sizeQcOld.val() == "0") {
          toastr.error("Size Actual tidak boleh kosong!");
          return;
        }

        const element = $(e.currentTarget);

        const value = parseFloat(element.val());
        const valueActual = parseFloat($("#input-size-qc-old").val());
        const calculate =
          element.attr("data-calculator") === "-"
            ? valueActual - value
            : valueActual + value;

        const actual = calculate < 0 ? 0 : calculate;
        $("#input-size-qc-actual").val(actual);
      });

      $(document).on("click", "#tbody-customer-table button", function (e) {
        onFetchItemCustomer(e);
      });

      $(document).on(
        "click",
        "#tbody-customer-item-table button",
        function (e) {
          onFetchProductByCustomer(e);
        }
      );

      $("#input-size-qc-old").on("change", (e) => {
        const vallidateInput = onInputNumber(e);
        vallidateInput(false, false, true);
        const element = $(e.currentTarget);
        const calculator =
          INPUT_SIZE_REPARASI_PLUS_MINUS.attr("data-calculator");
        const INPUT_SIZE_QC_ACTUAL = $("#input-size-qc-actual");

        const isHidden =
          ELEMENT_CONTENT_SIZE_REPARASI_PLUS_MINUS.hasClass("d-none");

        const valueAdded = parseFloat(INPUT_SIZE_REPARASI_PLUS_MINUS.val());
        const valueActual = parseFloat(element.val());

        const plusAdded = valueActual + valueAdded;
        const minusAdded = valueActual - valueAdded;
        const preventZeroMinus = minusAdded < 0 ? 0 : minusAdded;

        const calculate =
          isHidden || !calculator
            ? INPUT_SIZE_QC_ACTUAL.val(valueActual)
            : calculator === "+"
            ? INPUT_SIZE_QC_ACTUAL.val(plusAdded)
            : INPUT_SIZE_QC_ACTUAL.val(preventZeroMinus);
      });
    });

    // Fetch Initial Data
    $(document).ready(function () {
      $.ajax({
        type: "GET",
        url: "./index.aspx/GetDetailReparasi",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          DATA_LOCATION_REPARASI = data["d"]["LocRepair"];
          DATA_PAKET_REPARASI = data["d"]["PackageRepair"];

          const select2LocationRepair = SELECT_LOKASI_REPARASI.select2();
          const select2PackageRepair = SELECT_PAKET_UTAMA.select2();
          const select2NormalRepair = SELECT_SATUAN_TAMBAHAN.select2();

          DATA_LOCATION_REPARASI.forEach(({ ID, Nama, Kode }) => {
            let optionLocation = new Option(Nama, ID, false, false);
            optionLocation.setAttribute("data-id", ID);
            optionLocation.setAttribute("data-nama", Nama);
            optionLocation.setAttribute("data-kode", Kode);
            select2LocationRepair.append(optionLocation);
          });

          const mapLocation = DATA_PAKET_REPARASI.filter(
            (item) => item["IDLocReparasi"] === 1
          ).forEach(
            (
              {
                IDLocReparasi,
                IDLocPackageReparasi,
                IDPackage,
                JenisReparasi,
                Kode,
                Nama,
                NamaTipeReparasi,
                Ongkos,
                TipeReparasi,
              },
              index
            ) => {
              let optionPackageRepair = new Option(
                JenisReparasi,
                IDPackage,
                false,
                false
              );
              optionPackageRepair.setAttribute(
                "data-id-loc-reparasi",
                IDLocReparasi
              );
              optionPackageRepair.setAttribute(
                "data-id-loc-package-reparasi",
                IDLocPackageReparasi
              );
              optionPackageRepair.setAttribute("data-id-package", IDPackage);
              optionPackageRepair.setAttribute(
                "data-jenis-reparasi",
                JenisReparasi
              );
              optionPackageRepair.setAttribute("data-kode", Kode);
              optionPackageRepair.setAttribute("data-nama", Nama);
              optionPackageRepair.setAttribute(
                "data-nama-tipe-reparasi",
                NamaTipeReparasi
              );
              optionPackageRepair.setAttribute("data-ongkos", Ongkos);
              optionPackageRepair.setAttribute(
                "data-tipe-reparasi",
                TipeReparasi
              );
              select2PackageRepair.append(optionPackageRepair);

              let optionNormalRepair = new Option(
                JenisReparasi,
                IDPackage,
                false,
                false
              );
              optionNormalRepair.setAttribute(
                "data-id-loc-reparasi",
                IDLocReparasi
              );
              optionNormalRepair.setAttribute(
                "data-id-loc-package-reparasi",
                IDLocPackageReparasi
              );
              optionNormalRepair.setAttribute("data-id-package", IDPackage);
              optionNormalRepair.setAttribute(
                "data-jenis-reparasi",
                JenisReparasi
              );
              optionNormalRepair.setAttribute("data-kode", Kode);
              optionNormalRepair.setAttribute("data-nama", Nama);
              optionNormalRepair.setAttribute(
                "data-nama-tipe-reparasi",
                NamaTipeReparasi
              );
              optionNormalRepair.setAttribute("data-ongkos", Ongkos);
              optionNormalRepair.setAttribute(
                "data-tipe-reparasi",
                TipeReparasi
              );
              select2NormalRepair.append(optionNormalRepair);
            }
          );

          select2LocationRepair.trigger("change");
          select2PackageRepair.trigger("change");
          select2NormalRepair.trigger("change");
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          $.ajax({
            type: "GET",
            url: "./index.aspx/GetUserOutletAndDefaultSales",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
              DATA_LOCATION = data["d"]["location"];
              DATA_SALES = data["d"]["sales"];
              const select2Location = SELECT_LOCATION.select2();
              const mapLocation = DATA_LOCATION.forEach(
                ({ IDLokasi, NamaOutlet, KodeOutlet, TipeLokasi }, index) => {
                  let optionLocation = new Option(
                    NamaOutlet,
                    IDLokasi,
                    false,
                    false
                  );
                  optionLocation.setAttribute("data-id-lokasi", IDLokasi);
                  optionLocation.setAttribute("data-nama-lokasi", NamaOutlet);
                  optionLocation.setAttribute("data-kode-lokasi", KodeOutlet);
                  optionLocation.setAttribute("data-tipe-lokasi", TipeLokasi);
                  select2Location.append(optionLocation);
                }
              );

              select2Location.trigger("change");
            },
            error: function (error) {
              console.error(error);
            },
          });
        },
      });

      $.ajax({
        type: "GET",
        url: "index.aspx/GetFrameColor",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];
          FRAME_COLOR = response;
          const select2FrameColor = SELECT_FRAME_COLOR.select2();
          const mapColor = FRAME_COLOR.forEach(({ ID, Nama }) => {
            let optionFrameColor = new Option(Nama, ID, false, false);
            optionFrameColor.setAttribute("data-id-frame-color", ID);
            optionFrameColor.setAttribute("data-nama-frame-color", Nama);
            select2FrameColor.append(optionFrameColor);
          });

          select2FrameColor.trigger("change");
        },
        error: function (error) {
          console.error(error);
        },
      });
    });
  </script>
</asp:Content>
