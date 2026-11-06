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

    <div class="container-fluid p-0 mt-5">
      <div class="row">
        <div class="col-7">
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
                      <div class="row">
                        <div class="col-6">
                          <div class="col d-flex flex-column gap-1">
                            <h6 class="text-sm fw-bold">Berat Actual</h6>
                            <input
                              id="BeratTimbangAwal"
                              class="form-control"
                              disabled
                            />
                            <h5 class="fw-normal" id="BeratTimbangAwal"></h5>
                          </div>
                        </div>
                        <div class="col-5">
                          <div class="col d-flex flex-column gap-1">
                            <h6 class="text-sm fw-bold">Berat Akhir</h6>
                            <input id="BeratTimbang" class="form-control" />
                          </div>
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
                            value="Ada"
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
                            value="Tidak Ada"
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
                  <div class="col-6 row gx-5">
                    <div class="col-6">
                      <div>
                        <label for="input-size-qc" class="form-label fw-bold"
                          >Size Awal</label
                        >
                        <input
                          type="text"
                          name="input-size-qc-old"
                          id="SizeAwal"
                          class="form-control"
                          disabled
                        />
                      </div>
                    </div>
                    <div class="col-6">
                      <div>
                        <label for="input-size-qc" class="form-label fw-bold"
                          >Size Akhir</label
                        >
                        <input
                          type="text"
                          name="input-size-qc-actual"
                          id="SizeAkhir"
                          class="form-control"
                        />
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

    <div class="container-fluid border rounded p-5 mt-5">
      <h1 style="border: none; padding-bottom: 10px">Detail Stone</h1>
      <table class="table table-hover">
        <thead>
          <tr>
            <th>No</th>
            <th>Stone Name</th>
            <th>Total Butir</th>
            <th>Total Butir Actual</th>
            <th>Total Carat</th>
            <th>Total Carat Actual</th>
            <th>Tes Kualitas</th>
            <th>Tes Keaslian</th>
            <th>Sesuai Sertifikat</th>
            <th>Keterangan</th>
          </tr>
        </thead>
        <tbody id="tbody-detail-stone"></tbody>
      </table>
    </div>

    <div class="container-fluid mt-5 border rounded p-5">
      <h1 style="border-bottom: none" class="h1 mx-0 mb-5 mt-0 p-0 fw-bold">
        Detail Reparasi
      </h1>
      <div class="row">
        <div class="col-6">
          <div class="row g-4">
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
          </div>
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
    let BUTTON_SUBMIT_FORM = $("#btn-confirmation-submit");
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

    function injectTextToElement({ id, text, isForm }) {
      const LIST_FORM_ID = [
        "BeratTimbangAwal",
        "BeratTimbang",
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

    function onSubmitFormRepair() {
      const select2LocationElement = $(
        SELECT_FRAME_COLOR.select2("data")["element"]
      );
      const select2SalesElement = $(SELECT_SALES.select2("data")["element"]);
      const select2PackageElement = $(
        SELECT_PAKET_UTAMA.select2("data")["element"]
      );
      const select2OneElement = $(SELECT_SATUAN_TAMBAHAN.select2("data"));
      const keterangan = $("#form-textarea-keterangan").val();

      if (select2PackageElement.val() == 0 && select2OneElement.length === 0) {
        toastr.error("Tolong pilih paket satuan atau paket utama!");
        return;
      }

      if (keterangan === "") {
        toastr.error("Tolong isi keterangan!");
        return;
      }

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
            "data-id-loc-reparasi"
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

      const objectToSubmitDocQC = {
        QCNama: "",
        KodeCustLama: "",
        IDProduct: DATA_ITEM["ID"],
        Nomor: "",
        Blemish: "",
        JumlahButir: stoneDetail["totalButir"],
        Sertifikat: $("input[name='input-radio-tes-keaslian']").val(),
        NonCZ: "",
        CZ: "",
        Logo: $("input[name='input-radio-logo']").val(),
        KadarEmas: $("input[name='input-radio-grafir']").val(),
        Merk: $("input[name='input-radio-tes-keaslian']").val(),
        NetWeight: parseFloat($("#input-weight-qc-actual").val()),
        WarnaEmas: FRAME_COLOR.find(
          (item) => item.ID == SELECT_FRAME_COLOR.val()
        )["Nama"],
        FisikKeseluruhan: $("input[name='input-radio-rangka']").val(),
        OperatorNama: "",
        Draft: 0,
        Keterangan: keterangan,
        GrossWeight: $("#input-weight-qc-actual").val(),
        QC_Additional: getTableForm(),
        TipeProduct: "DJ",
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
        Keterangan: $("#form-textarea-keterangan").val(),
        ImgPicture: "",
        IDProduct: DATA_ITEM["ID"],
        IDProductTitipan: 0,
        EstimasiHarga: 0,
        IDQC: 0,
        IDQCTitipan: 0,
        TipeProduct: "DJ",
        IDLocReparasi: SELECT_LOKASI_REPARASI.val(),
        DocRepair_Package: package,
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
          toastr.success("Sukses membuat form reparasi!");
          defaultForm();
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

    function setDefaultPackage({ package }) {
      $.ajax({
        type: "GET",
        url: "./index.aspx/GetDetailReparasi",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          DATA_LOCATION_REPARASI = data["d"]["LocRepair"];
          DATA_PAKET_REPARASI = data["d"]["PackageRepair"];

          const select2PackageRepair = SELECT_PAKET_UTAMA.select2();
          const select2NormalRepair = SELECT_SATUAN_TAMBAHAN.select2();

          select2PackageRepair.append(
            new Option("-- Pilih Paket Utama --", 0, false, false)
          );

          const mapLocation = DATA_PAKET_REPARASI.filter(
            (item) => item["IDLocReparasi"] === 2
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
                  isCurrentSelected,
                  isCurrentSelected
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
                return;
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

          select2PackageRepair.trigger("change");
          select2NormalRepair.trigger("change");
        },
        error: function (error) {
          console.error(error);
        },
      });
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
          const detail = response["docRepairDetail"];
          const package = response["docRepairPackage"];
          const parcel = response["docRepairParcel"];

          setDefaultPackage({ package });

          IMG_PRODUCT.attr(
            "src",
            "https://" +
              window.location.hostname +
              "/Images.aspx?path=" +
              detail["ImgProduct"]
          );
          IMG_PRODUCT.attr("alt", detail["PLU"]);

          Object.keys(detail).forEach((item) => {
            injectTextToElement({
              id: item,
              text: detail[item],
            });
          });

          const paket = package.reduce(
            (accumulator, calculator) => {
              const { NamaPackage, TipePackage } = calculator;

              if (TipePackage === 1) {
                accumulator["utama"] += `
                  <li>${NamaPackage}</li>
                `;
              } else {
                accumulator["satuan"] += `
                  <li>${NamaPackage}</li>
                `;
              }
              return accumulator;
            },
            { satuan: "", utama: "" }
          );

          if (paket["utama"] !== "")
            $("#content-detail-reparasi").append(
              setContentPackage({
                text: "Paket Utama",
                element: paket["utama"],
              })
            );
          if (paket["satuan"] !== "")
            $("#content-detail-reparasi").append(
              setContentPackage({
                text: "Satuan Tambahan",
                element: paket["satuan"],
              })
            );

          const stone = parcel.reduce((accumulator, calculator, index) => {
            const {
              CaratButir,
              IDDocRepair,
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
            } = calculator;

            accumulator += `
                  <tr>
                    <td>${index + 1}</td>
                    <td>${Parcel}</td>
                    <td>${TotalButir}</td>
                    <td><input value="${TotalButirActual}" class="form-control" /></td>
                    <td>${TotalCarat}</td>
                    <td><input value="${TotalCaratActual}" class="form-control" /></td>
                    <td>
                      <div class="form-check">
                        <input class="form-check-input" type="checkbox" id="sertifikat_${Parcel}" data-id="${Parcel}" data-input="SesuaiSertifikat" value="SESUAI" ${
              SesuaiSertifikat ? "checked" : ""
            } />
                      </div>            
                    </td>
                    <td>
                      <div class="form-check">
                        <input class="form-check-input" type="checkbox" id="kualitas_${Parcel}" data-id="${Parcel}" data-input="TesKualitas" value="SESUAI" ${
              TesKualitas === "SESUAI" ? "checked" : ""
            } />
                      </div>
                    </td>
                    <td>
                      <div class="form-check">
                        <input class="form-check-input" type="checkbox" id="keaslian_${Parcel}" data-id="${Parcel}" data-input="TesKeaslian" value="SESUAI" ${
              TesKeaslian === "SESUAI" ? "checked" : ""
            } />
                      </div>
                    </td>
                    <td><input value="${Keterangan}" id="keterangan_${Parcel}" data-id="${Parcel}" data-input="Keterangan" class="form-control" /></td>
                  </tr>
                `;
            return accumulator;
          }, "");
          $("#tbody-detail-stone").append(stone);
          MAIN_CONTENT.removeClass("d-none").addClass("d-flex");
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          LOADER.hide();
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
          const mapSales = FRAME_COLOR.forEach(({ ID, Nama }) => {
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
