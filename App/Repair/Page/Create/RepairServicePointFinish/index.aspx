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
                  <div class="col-5">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Weight</h6>
                      <div class="row g-3">
                        <div class="col-6">
                          <div class="col d-flex flex-column gap-1">
                            <h6 class="text-sm fw-bold">Berat BRJ</h6>
                            <input
                              id="BeratTimbangAwal"
                              class="form-control"
                              disabled
                              type="number"
                            />
                          </div>
                        </div>
                        <div class="col d-flex flex-column gap-1">
                          <h6 class="text-sm fw-bold">Berat Actual</h6>
                          <input
                            class="BeratTimbangActual form-control"
                            id="BeratTimbangActual"
                            type="number"
                          />
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="col-6 offset-1">
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
                  <div class="col-12">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Keterangan</h6>
                      <h5 class="fw-normal" id="Keterangan"></h5>
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
        <div class="col-8">
          <div class="row g-4">
            <div class="col-4">
              <div class="col d-flex flex-column gap-1">
                <h6 class="text-sm fw-bold">Berat Repair</h6>
                <input
                  class="BeratTimbangActual form-control"
                  id="BeratTimbangRepair"
                  type="number"
                />
              </div>
            </div>
            <div class="col-4">
              <div class="col d-flex flex-column gap-1">
                <h6 class="text-sm fw-bold">Berat Akhir</h6>
                <input
                  class="BeratTimbangActual form-control"
                  id="BeratTimbangAkhir"
                  type="number"
                />
              </div>
            </div>
            <div class="col-4">
              <div class="col d-flex flex-column gap-1">
                <h6 class="text-sm fw-bold">Berat Potongan</h6>
                <input
                  id="BeratTimbangPotong"
                  class="form-control"
                  type="number"
                />
              </div>
            </div>
            <div class="col-12 d-flex flex-column gap-1">
              <h6 class="text-md fw-bold">Pengrajin</h6>
              <select name="input-pengrajin" id="Pengrajin"></select>
            </div>
            <div class="col-12">
              <div class="row">
                <div class="col-3">
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
                <div class="col-3">
                  <div>
                    <label for="input-size-qc-old" class="form-label fw-bold"
                      >Size Awal</label
                    >
                    <input
                      type="number"
                      name="input-size-qc-old"
                      id="SizeAwal"
                      class="form-control"
                      step="0.5"
                    />
                  </div>
                </div>
                <div class="col-3">
                  <div>
                    <label for="input-size-qc-actual" class="form-label fw-bold"
                      >Size Akhir</label
                    >
                    <input
                      type="text"
                      name="input-size-qc-actual"
                      id="SizeAkhir"
                      class="form-control"
                      disabled
                    />
                  </div>
                </div>
                <div class="col-3 d-none" id="content-plus-minus-size">
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
                <label for="select-paket-utama" class="form-label fw-bold"
                  >Paket Utama</label
                >
                <select class="w-100" id="select-paket-utama"></select>
              </div>
            </div>
            <div class="col-6">
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
            <!-- <div class="col-12">
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
            </div> -->
          </div>
        </div>
        <div class="d-flex flex-column col-7 mt-5">
          <label for="KeteranganRepair" class="form-label fw-bold"
            >Keterangan</label
          >
          <textarea
            id="KeteranganRepair"
            name="KeteranganRepair"
            class="form-control"
            rows="5"
          ></textarea>
        </div>
      </div>
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
    let DATA_STONE = [];
    let DATA_PAKET_REPARASI = [];
    let DATA_SATUAN_TAMBAHAN = [];
    let BUTTON_CANCEL_FORM = $("#btn-cancel-form");
    let BUTTON_SUBMIT_FORM = $("#btn-confirmation-submit");
    const SELECT_FRAME_COLOR = $("#FrameColor");
    const SELECT_PENGRAJIN = $("#Pengrajin");
    const SELECT_PAKET_UTAMA = $("#select-paket-utama");
    const SELECT_SATUAN_TAMBAHAN = $("#select-satuan-tambahan");
    const BUTTON_MODAL_CONFIRMATION = $("#btn-modal-confirmation");
    const ELEMENT_CONTENT = $("#content-after-plu-fetched");
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

    function getAndSetDefaultFrameColor(header) {
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
    }

    function setDefaultPackageAndValue(response) {
      DATA_ITEM = response["docRepairDetail"];

      if (DATA_ITEM["Status"] != 3) {
        window.close();
      }

      const package = response["docRepairPackage"];
      const pengrajin = response["pengrajin"];

      SELECT_PENGRAJIN.select2();
      SELECT_PENGRAJIN.append(
        `<option value="0"> -- Pilih Pengrajin -- </option>`
      );
      pengrajin.forEach((item) => {
        SELECT_PENGRAJIN.append(
          `<option value="${item["NamaTukang"]}">${item["NamaTukang"]} (${item["NIK"]})</option>`
        );
        SELECT_PENGRAJIN.trigger("change");
      });

      SELECT_PENGRAJIN.trigger("change");

      if (DATA_ITEM["Status"] != 3) {
        window.close();
      }

      const INPUT_SIZE_QC_ACTUAL = $("input[name='input-size-qc-actual']");
      const INPUT_SIZE_QC_OLD = $("input[name='input-size-qc-old']");
      if (
        DATA_ITEM["Item"].toUpperCase() === "EARRINGS" ||
        (DATA_ITEM["Item"].toUpperCase() !== "EARRINGS" &&
          !DATA_ITEM["Item"].toUpperCase().includes("BANGLE") &&
          !DATA_ITEM["Item"].toUpperCase().includes("RING") &&
          !DATA_ITEM["Item"].toUpperCase().includes("MENS JEWELLERY"))
      ) {
        INPUT_SIZE_QC_OLD.hide().parent().hide();
        INPUT_SIZE_QC_ACTUAL.hide().parent().hide();
        $("input[name='input-size-brj']").hide().parent().hide();
      }

      setDefaultPackage({ package, IDLocReparasi: 2, kodeItem: `'${DATA_ITEM["KodeItem"]}'`, tipeReparasi: 1 }); // prettier-ignore
      setDefaultPackage({ package, IDLocReparasi: 2, kodeItem: `'${DATA_ITEM["KodeItem"]}'`, tipeReparasi: 2 }); // prettier-ignore

      Object.keys(DATA_ITEM).forEach((item) => {
        injectTextToElement({
          id: item,
          text: DATA_ITEM[item],
        });
      });

      IMG_PRODUCT.attr("src", "/Images.aspx?path=" + DATA_ITEM["ImgProduct"]);
      IMG_PRODUCT.attr("alt", DATA_ITEM["PLU"]);

      const perbesarPerkecilPackage = package.find(
        (item) =>
          item["NamaPackage"].includes("Perbesar") ||
          item["NamaPackage"].includes("Perkecil")
      );

      if (perbesarPerkecilPackage) {
        $("#input-size-qc-plus-minus").val(
          parseFloat(DATA_ITEM["SizeAkhir"]) - parseFloat(DATA_ITEM["SizeAwal"])
        );
      }

      $("input[name='input-size-brj']").val(DATA_ITEM["SizeAwal"]);
      MAIN_CONTENT.removeClass("d-none").addClass("d-flex");
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
      ];
      const LIST_FORM_ID_WITH_NAME = [
        "GrafirLogo",
        "GrafirKadarEmas",
        "TesKeaslian",
        "FisikKeseluruhan",
      ];

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

      if (id === "BeratTimbangActual") {
        $("." + id).val(text);
        return;
      }

      $("#" + id).val(text);
    }

    function setContentPackage({ text, element }) {
      return `
        <div class="col d-flex flex-column gap-1 mt-3">
          <h6 class="text-md fw-bold">${text}</h6>
          <ul>${element}</ul>
        </div>
      `;
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

      return { min: 0.5, max: 999999999 };
    }

    function formatNumberToIDR(number) {
      const IDR = new Intl.NumberFormat("id-ID", {
        style: "currency",
        currency: "IDR",
      }).format(number);
      return IDR;
    }

    function onSubmitFormRepair() {
      const urlParams = new URLSearchParams(window.location.search);
      const idBrandAsal = urlParams.get("idbrandasal");
      const keterangan = $("#KeteranganRepair");

      if (SELECT_PENGRAJIN.val() === "0") {
        toastr.error("Pengrajin belum dipilih...");
        return;
      }

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

      // if (keterangan === "") {
      //   toastr.error("Tolong isi keterangan!");
      //   return;
      // }

      let package = [];

      const object = {
        Item: "data-nama-item",
        Kode: "data-kode-item",
        Klasifikasi: "data-klasifikasi-item",
        IDPackage: "data-id-package",
        JenisReparasi: "data-jenis-reparasi",
        TipeReparasi: "data-tipe-reparasi",
        NamaTipeReparasi: "data-nama-tipe-reparasi",
        Ongkos: "data-ongkos",
        IDLocReparasi: "data-id-loc-reparasi",
        Nama: "data-nama",
      };

      if (select2PackageElement.val() != 0) {
        let mainPackage = {};

        Object.keys(object).forEach((key) => {
          mainPackage[key] = select2PackageElement.attr(object[key]);
        });

        package.push(mainPackage);
      }

      select2OneElement.each((item, el) => {
        let mainPackage = {};
        Object.keys(object).forEach((key) => {
          mainPackage[key] = $(el["element"]).attr(object[key]);
        });
        package.push(mainPackage);
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
      };

      const objectToSubmitRepair = {
        IDDocRepair: DATA_ITEM["IDDocRepair"],
        PLU: DATA_ITEM["PLU"],
        SalesNama: DATA_ITEM["NamaSales"],
        CustomerNama: DATA_ITEM["NamaCustomer"],
        Operator: "",
        Keterangan: keterangan.val(),
        ImgPicture: "",
        IDProduct: DATA_ITEM["IDDocRepair"],
        IDProductTitipan: 0,
        EstimasiHarga: 0,
        TipeProduct: DATA_ITEM["TipeProduct"],
        DocRepair_Package: package,
        Pengrajin: $("#Pengrajin").val(),
        Weight: parseFloat($("#BeratTimbangAwal").val()),
        WeightActual: parseFloat($("#BeratTimbangActual").val()),
        WeightPotong: parseFloat($("#BeratTimbangPotong").val()),
        WeightData: parseFloat($("#BeratTimbangRepair").val()),
        WeightEnd: parseFloat($("#BeratTimbangAkhir").val()),
        SizeAwal: parseFloat($("#SizeAwal").val()),
        SizeAkhir: parseFloat($("#SizeAkhir").val()),
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
          IDBrandAsal: idBrandAsal,
        }),
        success: function (data) {
          toastr.success("Sukses membuat form reparasi!");
          setTimeout(() => {
            window.location.reload();
          }, 100);
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

    function setDefaultPackage({
      package,
      kodeItem,
      IDLocReparasi,
      tipeReparasi,
    }) {
      $.ajax({
        type: "GET",
        url: "./index.aspx/GetPackageItem",
        contentType: "application/json; charset=utf-8",
        data: { kodeItem, IDLocReparasi, tipeReparasi },
        success: function (data) {
          DATA_PAKET_REPARASI = data["d"];

          const select2PackageRepair =
            tipeReparasi == 1
              ? SELECT_PAKET_UTAMA.select2().empty().append(new Option("-- Pilih Paket Utama --", 0, false, false)) // prettier-ignore
              : SELECT_SATUAN_TAMBAHAN.select2().empty();

          const mapLocation = DATA_PAKET_REPARASI.forEach(
            (
              {
                IDLocReparasi,
                IDPackage,
                JenisReparasi,
                Nama,
                NamaTipeReparasi,
                Ongkos,
                TipeReparasi,
                KodeItem,
                NamaItem,
                KlasifikasiItem,
              },
              index
            ) => {
              const isCurrentSelected = package.find(
                (item) =>
                  item["IDPackage"] === IDPackage &&
                  item["TipePackage"] === TipeReparasi
              );
              const optionPackageRepair = new Option(
                JenisReparasi,
                IDPackage,
                isCurrentSelected,
                isCurrentSelected
              );

              $(optionPackageRepair)
                .attr("data-id-loc-reparasi", IDLocReparasi)
                .attr("data-id-package", IDPackage)
                .attr("data-jenis-reparasi", JenisReparasi)
                .attr("data-nama", Nama)
                .attr("data-kode-item", KodeItem)
                .attr("data-nama-item", NamaItem)
                .attr("data-klasifikasi-item", KlasifikasiItem)
                .attr("data-tipe-reparasi", TipeReparasi)
                .attr("data-ongkos", Ongkos)
                .attr("data-nama-tipe-reparasi", NamaTipeReparasi);

              select2PackageRepair.append(optionPackageRepair);
            }
          );

          select2PackageRepair.trigger("change");
        },
        error: function (error) {
          console.error(error);
        },
      });
    }

    function onChangeMainPackage() {
      ELEMENT_CONTENT_SIZE_REPARASI_PLUS_MINUS.addClass("d-none");

      const jenisReparasi = $(
        SELECT_PAKET_UTAMA.select2("data")["element"]
      ).attr("data-jenis-reparasi");

      console.log(jenisReparasi);

      const isPlusMinusPackage = checkPaketUtama(jenisReparasi);

      if (!isPlusMinusPackage) {
        $("input[name='input-size-qc-actual']").val(
          $("input[name='input-size-qc-old']").val()
        );
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
        changed > 1 ? parseFloat($("#input-size-qc-plus-minus").val()) : min;

      $("#input-size-qc-plus-minus")
        .prop("min", min)
        .prop("max", max)
        .val(defaultValue)
        .attr("data-calculator", plusOrMinus === "Perbesar" ? "+" : "-");

      const valueQcSize = parseFloat(
        $("input[name='input-size-qc-old']").val()
      );

      const calculate =
        valueQcSize === 0
          ? 0
          : plusOrMinus === "Perbesar"
          ? valueQcSize + defaultValue
          : valueQcSize - defaultValue;

      $("#SizeAkhir").val(calculate);
    }

    // Initializes while document is ready
    $(document).ready(function () {
      SELECT_FRAME_COLOR.select2();
      SELECT_PAKET_UTAMA.select2();
      SELECT_SATUAN_TAMBAHAN.select2();
    });

    // event handler
    $(document).ready(function () {
      BUTTON_SUBMIT_FORM.on("click", () => onSubmitFormRepair());

      $(document).on("change", "#select-paket-utama", onChangeMainPackage);

      $(document).on("change", 'input[type="number"]', (e) => {
        const vallidateInput = onInputNumber(e);
        vallidateInput(false, false);
      });

      INPUT_SIZE_REPARASI_PLUS_MINUS.on("change", function (e) {
        const vallidateInput = onInputNumber(e);
        vallidateInput(true, false, true);
        const sizeQcOld = $("input[name='input-size-qc-old']");
        const sizeQcActual = $("input[name='input-size-qc-actual']");

        if (sizeQcOld.val() == "" || sizeQcOld.val() == "0") {
          toastr.error("Size Actual tidak boleh kosong!");
          return;
        }

        const element = $(e.currentTarget);

        const value = parseFloat(element.val());
        const valueActual = parseFloat(
          $("input[name='input-size-qc-old']").val()
        );
        const calculate =
          element.attr("data-calculator") === "-"
            ? valueActual - value
            : valueActual + value;

        const actual = calculate < 0 ? 0 : calculate;
        $("input[name='input-size-qc-actual']").val(actual);
      });

      $("#SizeAwal").on("change", (e) => {
        const vallidateInput = onInputNumber(e);
        vallidateInput(false, false, true);

        const element = $(e.currentTarget);
        const calculator =
          INPUT_SIZE_REPARASI_PLUS_MINUS.attr("data-calculator");
        const INPUT_SIZE_QC_ACTUAL = $("input[name='input-size-qc-actual']");

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
      const urlParams = new URLSearchParams(window.location.search);
      const id = urlParams.get("id");
      const idBrandAsal = urlParams.get("idbrandasal");

      $.ajax({
        type: "GET",
        url: "./index.aspx/GetDetailRepair",
        data: {
          IDDocRepair: id,
          IDBrandAsal: idBrandAsal,
        },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];
          setDefaultPackageAndValue(response);
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (completedData) {
          const header = completedData["responseJSON"]["d"]["docRepairDetail"];
          LOADER.hide();
          getAndSetDefaultFrameColor(header);
        },
      });
    });
  </script>
</asp:Content>
