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

    <style>
      .hover-input-file:hover {
        background: #f1f1f1;
      }
    </style>
  </form>

  <div id="loader">Loading...</div>

  <div class="container d-none flex-column" id="main-content">
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

          <div class="border rounded p-5 mt-5">
            <h1 style="border: none; padding-bottom: 10px">Detail QC</h1>
            <div class="row">
              <div class="col-12">
                <div class="row g-5">
                  <div class="col-6">
                    <h6 class="text-md fw-bold">Weight</h6>
                    <div class="row g-3">
                      <div class="col-6">
                        <div class="col d-flex flex-column gap-1">
                          <h6 class="text-sm fw-bold">Berat Data</h6>
                          <input
                            id="BeratTimbangAwal"
                            class="form-control"
                            disabled
                          />
                        </div>
                      </div>
                      <div class="col-6">
                        <div class="col d-flex flex-column gap-1">
                          <h6 class="text-sm fw-bold">Berat Actual</h6>
                          <input
                            id="BeratTimbangActual"
                            type="number"
                            class="form-control"
                          />
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="col-6 d-flex align-items-end">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Frame Color</h6>
                      <select class="w-100" id="FrameColor"></select>
                    </div>
                  </div>
                  <div class="col-5">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Grafir Kadar Emas</h6>
                      <div class="d-flex flex-row gap-5">
                        <div class="form-check form-check-inline">
                          <input
                            class="form-check-input"
                            type="radio"
                            name="GrafirKadarEmas"
                            id="input-radio-grafir-ada"
                            value="Ada"
                            data-value="1"
                          />
                          <label
                            class="form-check-label"
                            for="input-radio-grafir-ada"
                            >Ada</label
                          >
                        </div>
                        <div class="form-check form-check-inline">
                          <input
                            class="form-check-input"
                            type="radio"
                            name="GrafirKadarEmas"
                            id="input-radio-grafir-tidak-ada"
                            value="Tidak Ada"
                            data-value="2"
                          />
                          <label
                            class="form-check-label"
                            for="input-radio-grafir-tidak-ada"
                            >Tidak Ada</label
                          >
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="col-6 offset-1">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Grafir Logo</h6>
                      <div class="d-flex flex-row gap-5">
                        <div class="form-check form-check-inline">
                          <input
                            class="form-check-input"
                            type="radio"
                            name="GrafirLogo"
                            id="input-radio-logo-ada"
                            value="Ada"
                            data-value="1"
                          />
                          <label
                            class="form-check-label"
                            for="input-radio-logo-ada"
                            >Ada</label
                          >
                        </div>
                        <div class="form-check form-check-inline">
                          <input
                            class="form-check-input"
                            type="radio"
                            name="GrafirLogo"
                            id="input-radio-logo-tidak-ada"
                            value="Tidak Ada"
                            data-value="2"
                          />
                          <label
                            class="form-check-label"
                            for="input-radio-logo-tidak-ada"
                            >Tidak Ada</label
                          >
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="col-5">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Tes Keaslian</h6>
                      <div class="d-flex flex-row gap-5">
                        <div class="form-check form-check-inline">
                          <input
                            class="form-check-input"
                            type="radio"
                            name="TesKeaslian"
                            id="input-radio-tes-keaslian-ada"
                            value="Ya"
                            data-value="1"
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
                            name="TesKeaslian"
                            id="input-radio-tes-keaslian-tidak-ada"
                            value="Tidak"
                            data-value="2"
                          />
                          <label
                            class="form-check-label"
                            for="input-radio-tes-keaslian-tidak-ada"
                            >Tidak</label
                          >
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="col-6 offset-1">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Keadaan Rangka</h6>
                      <div class="d-flex flex-row gap-5">
                        <div class="form-check form-check-inline">
                          <input
                            class="form-check-input"
                            type="radio"
                            name="FisikKeseluruhan"
                            id="input-radio-rangka-ada"
                            value="Baik"
                            data-value="1"
                          />
                          <label
                            class="form-check-label"
                            for="input-radio-rangka-ada"
                            >Baik</label
                          >
                        </div>
                        <div class="form-check form-check-inline">
                          <input
                            class="form-check-input"
                            type="radio"
                            name="FisikKeseluruhan"
                            id="input-radio-rangka-tidak-ada"
                            value="Tidak Baik"
                            data-value="2"
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

                  <div class="col-5">
                    <div class="d-flex flex-column gap-1">
                      <label class="fw-bold">Basic / Non-Basic</label>
                      <div class="d-flex flex-row gap-5">
                        <div class="form-check form-check-inline">
                          <input
                            class="form-check-input"
                            type="radio"
                            name="Blemish"
                            id="input-radio-tipe-basic"
                            value="1"
                            checked
                          />
                          <label
                            class="form-check-label"
                            for="input-radio-tipe-basic"
                            >Basic</label
                          >
                        </div>
                        <div class="form-check form-check-inline">
                          <input
                            class="form-check-input"
                            type="radio"
                            name="Blemish"
                            id="input-radio-tipe-non-basic"
                            value="0"
                          />
                          <label
                            class="form-check-label"
                            for="input-radio-tipe-non-basic"
                            >Non-Basic</label
                          >
                        </div>
                      </div>
                    </div>
                  </div>

                  <div class="col-12">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Keterangan</h6>
                      <textarea
                        id="Keterangan"
                        name="Keterangan"
                        class="form-control"
                        rows="5"
                      ></textarea>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="container mt-5 border rounded p-5">
      <h1 style="border-bottom: none" class="h1 mx-0 mb-5 mt-0 p-0 fw-bold">
        Detail Reparasi
      </h1>
      <div class="row">
        <div class="col-12">
          <div class="row g-4">
            <div class="col-8"></div>
            <div class="col-12 row mt-3 p-0 m-0">
              <div class="row">
                <div class="col-6">
                  <div class="row" id="content-size-reparasi">
                    <div class="col">
                      <div>
                        <label for="SizeBRJ" class="form-label fw-bold"
                          >Size BRJ</label
                        >
                        <input
                          type="number"
                          name="SizeBRJ"
                          id="SizeBRJ"
                          class="form-control"
                          disabled
                        />
                      </div>
                    </div>
                    <div class="col">
                      <div>
                        <label for="SizeActual" class="form-label fw-bold"
                          >Size Actual</label
                        >
                        <input
                          type="number"
                          name="SizeAwal"
                          id="SizeAwal"
                          class="form-control"
                          step="0.5"
                        />
                      </div>
                    </div>
                    <div class="col">
                      <div>
                        <label for="SizeAkhir" class="form-label fw-bold"
                          >Size Akhir</label
                        >
                        <input
                          type="number"
                          name="SizeAkhir"
                          id="SizeAkhir"
                          class="form-control"
                          step="0.5"
                          disabled
                        />
                      </div>
                    </div>
                    <div class="col d-none" id="content-plus-minus-size">
                      <div>
                        <label
                          for="input-size-qc-plus-minus"
                          class="form-label fw-bold"
                        ></label>
                        <input
                          type="number"
                          name="input-size-qc-plus-minus"
                          id="input-size-qc-plus-minus"
                          step="0.5"
                          class="form-control"
                        />
                      </div>
                    </div>
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
              <div class="d-flex gap-5 align-items-center my-3">
                <div class="d-flex flex-column">
                  <div>
                    <img
                      src=""
                      style="width: 200px; height: 150px"
                      id="ImgPicture"
                      alt="Uploaded Image Picture"
                    />
                  </div>
                </div>
                <div class="fw-bold text-md">&rarr;</div>
                <div id="img-new-picture-container"></div>
              </div>
            </div>
            <div class="col-12">
              <div class="col d-flex flex-column gap-1">
                <h6 class="text-md fw-bold">Keterangan</h6>
                <textarea
                  id="KeteranganRepair"
                  name="KeteranganRepair"
                  class="form-control"
                  rows="5"
                ></textarea>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="rounded border p-5 mt-5">
      <h1 class="fw-bold" style="border: none; padding-bottom: 10px">
        Detail Stone QC
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
          class="btn btn-primary btn-lg w-100"
          data-bs-toggle="modal"
          data-bs-target="#modalConfirmation"
          id="btn-modal-confirmation"
        >
          Finish
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
    const IMG_PRODUCT = $("#img-product");
    const MAIN_CONTENT = $("#main-content");
    const LOADER = $("#loader");
    let DATA_LOCATION = [];
    let DATA_SALES = [];
    let FRAME_COLOR = [];
    let DATA_ITEM = {};
    let DATA_REPAIR = [];
    let DATA_STONE = [];
    let DATA_PAKET_REPARASI = [];
    let DATA_SATUAN_TAMBAHAN = [];
    let BUTTON_SUBMIT_FORM = $("#btn-confirmation-submit");
    let BUTTON_CANCEL_FORM = $("#btn-cancel-form");
    let imagePicture = "";
    const SELECT_FRAME_COLOR = $("#FrameColor");
    const SELECT_LOKASI_REPARASI = $("#select-lokasi-reparasi");
    const SELECT_PAKET_UTAMA = $("#select-paket-utama");
    const SELECT_SATUAN_TAMBAHAN = $("#select-satuan-tambahan");
    const BUTTON_MODAL_CONFIRMATION = $("#btn-modal-confirmation");
    const SELECT_SEARCH_WITH = $("#select-search-with");
    const ELEMENT_CONTENT_SIZE_REPARASI = $("#content-size-reparasi");
    let changed = 0;
    const ELEMENT_CONTENT_SIZE_REPARASI_PLUS_MINUS = $(
      "#content-plus-minus-size"
    );
    const ELEMENT_CONTENT_LABEL_SIZE_REPARASI_PLUS_MINUS = $(
      "label[for='input-size-qc-plus-minus']"
    );
    const INPUT_SIZE_REPARASI_PLUS_MINUS = $("#input-size-qc-plus-minus");

    function onInputNumber(e) {
      const element = $(e.currentTarget);
      const value = parseFloat(element.val());

      return function checkInputNumber(
        isWithMinMax,
        isAllowNegativeValue,
        isWithStep
      ) {
        if (isNaN(value) && !isWithMinMax) {
          element.val(0).trigger("change");
          return;
        }

        if (isNaN(value) && isWithMinMax) {
          const min = parseFloat(element.attr("min"));
          element.val(0).trigger("change");
          return;
        }
        if (value === 0 && !isWithMinMax) return;
        if (value === 0 && isWithMinMax) {
          const min = parseFloat(element.attr("min"));
          element.val(min).trigger("change");
          return;
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

    toastr.options = {
      closeButton: true,
      progressBar: true,
      positionClass: "toast-bottom-center",
      showDuration: "300",
      hideDuration: "1000",
      timeOut: "5000",
    };

    function injectTextToElement({ id, text, isForm }) {
      const LIST_FORM_ID = [
        "BeratTimbang",
        "BeratTimbangActual",
        "BeratTimbangAwal",
        "BeratTimbangPotong",
        "SizeAwal",
        "SizeAkhir",
        "SizeBRJ",
        "KeteranganRepair",
      ];

      const LIST_FORM_CHECK_ID_WITH_NAME = [
        "GrafirLogo",
        "GrafirKadarEmas",
        "TesKeaslian",
        "FisikKeseluruhan",
        "Blemish",
      ];

      if (id === "ImgPicture") {
        $("#ImgPicture").attr("src", text);
        return;
      }

      if (
        !LIST_FORM_ID.includes(id) &&
        !LIST_FORM_CHECK_ID_WITH_NAME.includes(id)
      ) {
        $("#" + id).text(text);
        return;
      }

      if (LIST_FORM_CHECK_ID_WITH_NAME.includes(id)) {
        $(`[name="${id}"]`).each((index, elem) => {
          if ($(elem).val().toLowerCase() === text.toLowerCase())
            $(elem).prop("checked", true);
        });
        return;
      }

      $("#" + id).val(text);
    }

    function setDefaultFrameColor({ text }) {
      SELECT_FRAME_COLOR.children().each((idx, elem) => {
        if (
          $(elem).attr("data-nama-frame-color").toLowerCase() ===
          text.toLowerCase()
        ) {
          SELECT_FRAME_COLOR.val($(elem).attr("data-id-frame-color"));
          SELECT_FRAME_COLOR.trigger("change");
        }
      });
    }

    function setContentPackage({ text, element }) {
      return `
        <div class="col d-flex flex-column gap-1 mt-3">
          <h6 class="text-md fw-bold">${text}</h6>
          <ul>${element}</ul>
        </div>
      `;
    }

    function formatNumberToIDR(number) {
      const IDR = new Intl.NumberFormat("id-ID", {
        style: "currency",
        currency: "IDR",
      }).format(number);
      return IDR;
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

    function onPreviewImage() {
      const reader = new FileReader();

      const fileInput = $("#input-file-foto-product")[0];

      const file = fileInput.files[0];

      var allowedTypeExtension = ["jpeg", "jpg", "png"];
      const fileName = file["name"];

      const fileExtension = fileName.split(".").pop().toLowerCase();

      if (!allowedTypeExtension.includes(fileExtension)) {
        toastr.error(
          "Only types with these extension are allowed: " +
            allowedTypeExtension.join(", ")
        );

        $("#input-file-foto-product").val(null);
        return;
      }

      if (file) {
        reader.onload = function (e) {
          $("#img-new-picture-container").html(
            `<img src="${e.target.result}" style="width: 200px; height: 150px;" />`
          );
        };

        reader.readAsDataURL(file);
      }
    }

    function onSubmitFormAndUploadFile() {
      const fileInput = $("#input-file-foto-product")[0];
      const select2PackageElement = $(
        SELECT_PAKET_UTAMA.select2("data")["element"]
      );

      const select2OneElement = $(SELECT_SATUAN_TAMBAHAN.select2("data"));

      if (!fileInput.files[0]) {
        onSubmitFormRepair({ imagePicture });
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
    }

    function onSubmitFormRepair(args = { imagePicture: "" }) {
      const keterangan = $("#Keterangan");
      const select2LocationElement = $(
        SELECT_FRAME_COLOR.select2("data")["element"]
      );

      const select2PackageElement = $(
        SELECT_PAKET_UTAMA.select2("data")["element"]
      );

      const select2OneElement = $(SELECT_SATUAN_TAMBAHAN.select2("data"));

      if (select2PackageElement.val() == 0 && select2OneElement.length === 0) {
        toastr.error("Tolong pilih paket satuan atau paket utama!");
        return;
      }

      //   if (keterangan === "") {
      //     toastr.error("Tolong isi keterangan!");
      //     return;
      //   }

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

      const selectedPackage = select2PackageElement.text().toUpperCase();
      const sizeQcOld = $("#SizeAwal").val();

      if (
        (selectedPackage.includes("PERBESAR") ||
          selectedPackage.includes("PERKECIL")) &&
        (parseFloat(sizeQcOld) == 0 || parseFloat($("#SizeAkhir").val()) == 0)
      ) {
        toastr.error("Pastikan size actual terisi!");
        return;
      }

      const objectToSubmitDocQC = {
        IDDocQC: DATA_ITEM["IDDocQC"],
        Sertifikat: $("input[name='TesKeaslian']:checked").val(),
        Logo: $("input[name='GrafirLogo']:checked").val(),
        Blemish: $("input[name='Blemish']:checked").val(),
        KadarEmas: $("input[name='GrafirKadarEmas']:checked").val(),
        Merk: $("input[name='TesKeaslian']:checked").val(),
        NetWeight: parseFloat($("#BeratTimbangActual").val()),
        WarnaEmas: FRAME_COLOR.find(
          (item) => item.ID == SELECT_FRAME_COLOR.val()
        )["Nama"],
        FisikKeseluruhan: $("input[name='FisikKeseluruhan']:checked").val(),
        OperatorNama: "",
        GrossWeight: $("#BeratTimbangActual").val(),
        TipeProduct: DATA_ITEM["TipeProduct"],
        Keterangan: keterangan.val(),
        QC_Additional: getTableForm().length > 0 ? getTableForm() : dummyData,
      };

      const objectToSubmitRepair = {
        IDDocRepair: DATA_ITEM["IDDocRepair"],
        PLU: DATA_ITEM["PLU"],
        SalesNama: DATA_ITEM["NamaSales"],
        CustomerNama: DATA_ITEM["NamaCustomer"],
        Operator: "",
        Keterangan: $("#KeteranganRepair").val(),
        ImgPicture: args.imagePicture,
        IDProduct: DATA_ITEM["IDDocRepair"],
        IDProductTitipan: 0,
        EstimasiHarga: 0,
        TipeProduct: DATA_ITEM["TipeProduct"],
        DocRepair_Package: package,
        Pengrajin: $("#Pengrajin").val(),
        Weight: parseFloat($("#BeratTimbangActual").val()),
        WeightActual: parseFloat($("#BeratTimbangActual").val()),
        WeightData: parseFloat($("#BeratTimbangAwal").val()),
        SizeAwal: parseFloat($("#SizeAwal").val()),
        SizeAkhir: parseFloat($("#SizeAkhir").val()),
        QC_Additional: getTableForm().length > 0 ? getTableForm() : dummyData,
        FrameColor: parseInt(SELECT_FRAME_COLOR.val()),
      };

      BUTTON_MODAL_CONFIRMATION.prop("disabled", true).text("Loading...");

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
          const urlParams = new URLSearchParams(window.location.search);
          const id = urlParams.get("id");
          const idbrandasal = urlParams.get("idbrandasal");
          toastr.success("Sukses update form reparasi!");
          setTimeout(() => {
            window.location.href = `../../Report/RepairOrder/detail.aspx?id=${id}&idbrandasal=${idbrandasal}`;
          }, 500);
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
    }

    function setDefaultPackage({ package, IDLocReparasi }) {
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
            const isCurrentSelected = ID === IDLocReparasi;
            let optionLocation = new Option(
              Nama,
              ID,
              isCurrentSelected,
              isCurrentSelected
            );
            optionLocation.setAttribute("data-id", ID);
            optionLocation.setAttribute("data-nama", Nama);
            optionLocation.setAttribute("data-kode", Kode);
            select2LocationRepair.append(optionLocation);
          });

          select2PackageRepair.append(
            new Option("-- Pilih Paket Utama --", 0, false, false)
          );

          const mapLocation = DATA_PAKET_REPARASI.filter(
            (item) => item["IDLocReparasi"] === IDLocReparasi
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
              if (TipeReparasi === 1) {
                const isCurrentSelected = package.find(
                  (item) =>
                    item["IDPackage"] === IDPackage && item["TipePackage"] === 1
                );

                let optionPackageRepair = new Option(
                  JenisReparasi,
                  IDPackage,
                  Boolean(isCurrentSelected),
                  Boolean(isCurrentSelected)
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
              }

              const isCurrentSelected = package.find(
                (item) =>
                  item["IDPackage"] === IDPackage && item["TipePackage"] === 2
              );
              let optionNormalRepair = new Option(
                JenisReparasi,
                IDPackage,
                isCurrentSelected,
                isCurrentSelected
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

          if (Object.keys(DATA_ITEM).length !== 0) {
            // Check kalo item nya bukan bangle atau ring, input disable
            if (
              DATA_ITEM["Item"].toUpperCase() === "EARRINGS" ||
              (DATA_ITEM["Item"].toUpperCase() !== "EARRINGS" &&
                !DATA_ITEM["Item"].toUpperCase().includes("BANGLE") &&
                !DATA_ITEM["Item"].toUpperCase().includes("RING") &&
                !DATA_ITEM["Item"].toUpperCase().includes("MENS JEWELLERY"))
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

                if (DATA_ITEM["Item"].toUpperCase().includes("BANGLE")) {
                  if (
                    textContent.includes("PERBESAR RING SIZE") ||
                    textContent.includes("PERKECIL RING SIZE")
                  ) {
                    $(elem).attr("disabled", true);
                  }
                } else if (
                  DATA_ITEM["Item"].toUpperCase() !== "EARRINGS" &&
                  (DATA_ITEM["Item"].toUpperCase().includes("RING") ||
                    DATA_ITEM["Item"].toUpperCase().includes("MENS JEWELLERY"))
                ) {
                  console.log("hey");

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
        },
        error: function (error) {
          console.error(error);
        },
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

        return { min: parseFloat(min) - 0.5, max: parseFloat(max) };
      }

      return { min: 1, max: 999999999 };
    }

    function onChangeMainPackage() {
      const jenisReparasi = $(this)
        .find(":selected")
        .attr("data-jenis-reparasi");
      ELEMENT_CONTENT_SIZE_REPARASI_PLUS_MINUS.addClass("d-none");

      const isPlusMinusPackage = checkPaketUtama(jenisReparasi);

      if (!isPlusMinusPackage) {
        $("#SizeAkhir").val($("#SizeAwal").val());
        return;
      }

      changed += 1;
      const plusOrMinus = jenisReparasi.includes("Perbesar")
        ? "Perbesar"
        : "Perkecil";

      ELEMENT_CONTENT_SIZE_REPARASI_PLUS_MINUS.removeClass("d-none");
      ELEMENT_CONTENT_LABEL_SIZE_REPARASI_PLUS_MINUS.text(
        `${plusOrMinus} Size`
      );

      const { min, max } = isPlusMinusPackage;

      const sizeData = $("#SizeAwal").val();

      if (sizeData == "" || sizeData == "0") {
        toastr.error("Pastikan size awal terisi!");
      }

      const defaultValue =
        changed === 1 ? parseFloat($("#input-size-qc-plus-minus").val()) : min;

      $("#input-size-qc-plus-minus")
        .prop("min", min)
        .prop("max", max)
        .val(defaultValue)
        .attr("data-calculator", plusOrMinus === "Perbesar" ? "+" : "-");

      const valueQcSize = parseFloat($("#SizeAwal").val());
      const calculate =
        valueQcSize === 0
          ? 0
          : plusOrMinus === "Perbesar"
          ? valueQcSize + defaultValue
          : valueQcSize - defaultValue;
      $("#SizeAkhir").val(calculate);
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

    function onChangeLocRepair(e) {
      const currentTarget = e.currentTarget;
      const element = $(currentTarget);
      const value = e.val;

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

      if (Object.keys(DATA_ITEM).length !== 0) {
        // Check kalo item nya bukan bangle atau ring, input disable
        if (
          DATA_ITEM["Item"].toUpperCase() === "EARRINGS" ||
          (DATA_ITEM["Item"].toUpperCase() !== "EARRINGS" &&
            !DATA_ITEM["Item"].toUpperCase().includes("BANGLE") &&
            !DATA_ITEM["Item"].toUpperCase().includes("RING") &&
            !DATA_ITEM["Item"].toUpperCase().includes("MENS JEWELLERY"))
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

            if (DATA_ITEM["Item"].toUpperCase().includes("BANGLE")) {
              if (
                textContent.includes("PERBESAR RING SIZE") ||
                textContent.includes("PERKECIL RING SIZE")
              ) {
                $(elem).attr("disabled", true);
              }
            } else if (
              DATA_ITEM["Item"].toUpperCase() !== "EARRINGS" &&
              (DATA_ITEM["Item"].toUpperCase().includes("RING") ||
                DATA_ITEM["Item"].toUpperCase().includes("MENS JEWELLERY"))
            ) {
              console.log("hey");

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

      select2PackageRepair.trigger("change");
      select2NormalRepair.trigger("change");
    }

    // Initializes while document is ready
    $(document).ready(function () {
      SELECT_FRAME_COLOR.select2();
      SELECT_PAKET_UTAMA.select2();
      SELECT_SATUAN_TAMBAHAN.select2();
      SELECT_LOKASI_REPARASI.select2();
    });

    // event handler
    $(document).ready(function () {
      SELECT_SEARCH_WITH.on("change", onChangeSearchWith);
      $("#input-file-foto-product").on("change", () => onPreviewImage());
      $(document).on("select2-selecting", "#select-lokasi-reparasi", (e) =>
        onChangeLocRepair(e)
      );

      $(document).on(
        "change",
        'input[type="number"]:not(#input-size-qc-plus-minus):not(#SizeAwal)',
        (e) => {
          const vallidateInput = onInputNumber(e);
          vallidateInput(false, false);
        }
      );

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

      $(document).on("change", "#select-paket-utama", onChangeMainPackage);

      // $("#select-paket-utama").on("change", onChangeMainPackage);

      BUTTON_SUBMIT_FORM.on("click", () => onSubmitFormAndUploadFile());

      INPUT_SIZE_REPARASI_PLUS_MINUS.on("change", function (e) {
        const vallidateInput = onInputNumber(e);
        vallidateInput(true, false, true);

        const sizeQcOld = $("#SizeAwal");
        const sizeQcActual = $("#SizeAkhir");

        if (sizeQcOld.val() == "" || sizeQcOld.val() == "0") {
          toastr.error("Size Awal tidak boleh kosong!");
          return;
        }

        const element = $(e.currentTarget);

        const value = parseFloat(element.val());
        const valueActual = parseFloat($("#SizeAwal").val());
        const calculate =
          element.attr("data-calculator") === "-"
            ? valueActual - value
            : valueActual + value;

        $("#SizeAkhir").val(calculate);
      });

      $("#SizeAwal").on("change", (e) => {
        const vallidateInput = onInputNumber(e);
        vallidateInput(false, false, true);
        const element = $(e.currentTarget);
        const calculator =
          INPUT_SIZE_REPARASI_PLUS_MINUS.attr("data-calculator");
        const INPUT_SIZE_QC_ACTUAL = $("#SizeAkhir");
        const isHidden = INPUT_SIZE_REPARASI_PLUS_MINUS.hasClass("d-none");
        const valueAdded = parseFloat(INPUT_SIZE_REPARASI_PLUS_MINUS.val());
        const valueActual = parseFloat(element.val());

        const calculate =
          isHidden || !calculator
            ? INPUT_SIZE_QC_ACTUAL.val(valueActual)
            : calculator === "+"
            ? INPUT_SIZE_QC_ACTUAL.val(valueActual + valueAdded)
            : INPUT_SIZE_QC_ACTUAL.val(valueActual - valueAdded);
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
          const INPUT_SIZE_QC_OLD = $("#SizeAwal");
          const INPUT_SIZE_QC_ACTUAL = $("#SizeAkhir");
          const response = data["d"];
          DATA_ITEM = response["docRepairDetail"];
          const package = response["docRepairPackage"];
          const parcel = response["docRepairParcel"];

          // Check kalo Size nya 0 boleh isi sendiri
          //   if (INPUT_SIZE_QC_OLD.val() == 0) {
          // INPUT_SIZE_QC_OLD.prop("disabled", false);
          // INPUT_SIZE_QC_ACTUAL.prop("disabled", false);
          //   }

          // Check kalo item nya bukan bangle atau ring, input disable
          if (
            DATA_ITEM["Item"].toUpperCase() === "EARRINGS" ||
            (DATA_ITEM["Item"].toUpperCase() !== "EARRINGS" &&
              !DATA_ITEM["Item"].toUpperCase().includes("BANGLE") &&
              !DATA_ITEM["Item"].toUpperCase().includes("RING") &&
              !DATA_ITEM["Item"].toUpperCase().includes("MENS JEWELLERY"))
          ) {
            INPUT_SIZE_QC_OLD.hide().parent().hide();
            INPUT_SIZE_QC_ACTUAL.hide().parent().hide();
            $("#input-size-brj").hide().parent().hide();
          }

          if (DATA_ITEM["TipeProduct"] === "PG") {
            $("#content-detail-stone").addClass("d-none");
            return;
          }

          setDefaultPackage({
            package,
            IDLocReparasi: DATA_ITEM["IDLocRepair"],
          });

          Object.keys(DATA_ITEM).forEach((item) => {
            injectTextToElement({
              id: item,
              text: DATA_ITEM[item],
            });
          });

          IMG_PRODUCT.attr(
            "src",
            "/Images.aspx?path=" + DATA_ITEM["ImgProduct"]
          );

          imagePicture = DATA_ITEM["ImgPictureRaw"];

          IMG_PRODUCT.attr("alt", DATA_ITEM["PLU"]);

          const perbesarPerkecilPackage = package.find(
            (item) =>
              item["NamaPackage"].includes("Perbesar") ||
              item["NamaPackage"].includes("Perkecil")
          );

          if (perbesarPerkecilPackage) {
            const hitunganPlusMinus = perbesarPerkecilPackage[
              "NamaPackage"
            ].includes("Perbesar")
              ? parseFloat(
                  DATA_ITEM["SizeAkhir"] - parseFloat(DATA_ITEM["SizeAwal"])
                )
              : parseFloat(
                  parseFloat(DATA_ITEM["SizeAwal"]) - DATA_ITEM["SizeAkhir"]
                );
            $("#input-size-qc-plus-minus").val(hitunganPlusMinus);
          }

          const stone = parcel.reduce((accumulator, calculator, index) => {
            const {
              CaratButir,
              IDDocRepair,
              IDProductStone,
              NomorRepair,
              Parcel,
              StoneType,
              StoneTypeMap,
              TotalButir,
              TotalButirActual,
              TotalCarat,
              TotalCaratActual,
              SesuaiSertifikat,
              TesKualitas,
              TesKeaslian,
              Keterangan,
              IDDocStone,
              Stone,
              Clarity,
              Color,
            } = calculator;

            accumulator += `
                  <tr data-stone-id=${IDProductStone}>
                <td>${index + 1}</td>
                <td>${Stone} - ${Clarity} - ${Color}</td>
                <td id="total_butir_data_${IDProductStone}">${TotalButir}</td>
                <td><input class="form-control" value="${TotalButirActual}" id="total_butir_${IDProductStone}" data-id="${IDProductStone}" data-input="TotalButirActual" type="number" /></td>
                <td id="total_carat_data_${IDProductStone}">${TotalCarat}</td>
                <td><input disabled class="form-control" value="${TotalCaratActual}" id="total_carat_${IDProductStone}" data-id="${IDProductStone}" data-input="TotalCaratActual" type="number" /></td>
                <td style="display:none;">
                  <div class="d-flex justify-content-center align-items-center">
                    <div class="form-check">
                      <input class="form-check-input" type="checkbox" id="kualitas_${IDProductStone}" data-id="${IDProductStone}" data-input="TesKualitas" value="SESUAI" checked />
                    </div>
                  </div>
                </td>
                <td>
                  <div class="d-flex justify-content-center align-items-center">
                    <div class="form-check">
                      <input class="form-check-input" type="checkbox" id="keaslian_${IDProductStone}" data-id="${IDProductStone}" data-input="TesKeaslian" value="SESUAI" ${
              TesKeaslian === "SESUAI" ? "checked" : ""
            } />
                    </div>
                  </div>
                </td>
                <td>
                  <div class="d-flex justify-content-center align-items-center">
                    <div class="form-check">
                      <input class="form-check-input" type="checkbox" id="sertifikat_${IDProductStone}" data-id="${IDProductStone}" data-input="SesuaiSertifikat" value="SESUAI" ${
              SesuaiSertifikat === "1" ? "checked" : ""
            } />
                    </div>
                  </div>
                </td>
                <td>
                  <div class="d-flex justify-content-center align-items-center">
                      <input class="form-control" type="text" id="keterangan_${IDProductStone}" data-id="${IDProductStone}" data-input="Keterangan" value="${Keterangan}" />
                      <input class="form-control" type="hidden" id="stone_type_${IDProductStone}" data-id="${IDProductStone}" data-input="StoneType" value="${StoneTypeMap}" />
                  </div>
                </td>
              </tr>
                `;
            return accumulator;
          }, "");

          $("#tbody-detail-stone-table").append(stone);
          MAIN_CONTENT.removeClass("d-none").addClass("d-flex");
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (completedData) {
          const header = completedData["responseJSON"]["d"]["docRepairDetail"];

          $.ajax({
            type: "GET",
            url: "index.aspx/GetFrameColor",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
              const response = data["d"];
              FRAME_COLOR = response;
              const select2FrameColor = SELECT_FRAME_COLOR.select2();
              const mapSales = FRAME_COLOR.forEach(({ ID, Nama }) => {
                let optionFrameColor = new Option(Nama, ID, false, false);
                optionFrameColor.setAttribute("data-id-frame-color", ID);
                optionFrameColor.setAttribute("data-nama-frame-color", Nama);
                select2FrameColor.append(optionFrameColor);
              });

              select2FrameColor.trigger("change");
              setDefaultFrameColor({ text: header["FrameColor"] });
            },
            error: function (error) {
              console.error(error);
            },
          });

          LOADER.hide();
        },
      });
    });
  </script>
</asp:Content>
