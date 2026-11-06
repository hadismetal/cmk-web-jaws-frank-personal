<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="MasterPage.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="RepairDeliveryOrder_Index" %>

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

  <div class="container">
    <div class="border rounded p-5">
      <h1 style="border-bottom: none" class="m-0 p-0 fw-bold" id="naming">
        DO BRJ Factory
      </h1>
      <div class="row mt-3 g-4">
        <div class="col">
          <div class="d-flex flex-column g-2 w-100">
            <label for="select-location-from" class="fw-bold"> Nomor DO</label>
            <div id="nomorDOKirim"></div>
          </div>
        </div>
      </div>
    </div>
    <div class="col-12" id="content-list-reparasi"></div>
  </div>

  <script type="text/javascript">
    let DATA_LOCATION = [];
    let DATA_LOCATION_TO = [];
    let DATA_LOCATION_REPARASI = [];
    let DATA_LIST_REPARASI = [];
    let DATA_LIST_PACKAGE_REPARASI = [];
    let DATA_LIST_STONE_REPARASI = [];
    let BUTTON_SEARCH_DO_FORM = $("#btn-search-do-form");
    let BUTTON_SUBMIT_DO_FORM = $("#btn-submit-form");
    const SELECT_LOCATION_FROM = $("#select-location-from");
    const SELECT_LOCATION_TO = $("#select-location-to");
    const SELECT_LOKASI_REPARASI = $("#select-lokasi-reparasi");
    const INPUT_KURIR = $("#input-kurir");
    const TEXTAREA_KETERANGAN = $("#textarea-keterangan-do");
    const TBODY_LIST_REPARASI = $("#tbody-list-reparasi");
    const CONTENT_LIST_REPARASI = $("#content-list-reparasi");
    const CONTENT_SELECT_OUTLET_TUJUAN = $("#content-select-outlet-tujuan");
    const CHECKBOX_ALL_TABLE = $("#checkbox-all-table");
    const INPUT_TGL_AWAL = $("#input-tgl-awal");
    const INPUT_TGL_AKHIR = $("#input-tgl-akhir");

    function injectTextToElement({ id, text }) {
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

    function getStartDateAndCurrentDate() {
      let currDate = new Date();
      const offset = currDate.getTimezoneOffset();
      currDate = new Date(currDate.getTime() - offset * 60 * 1000);
      const currDateISO = currDate.toISOString().split("T")[0];
      const splittedDate = currDateISO.split("-");
      const startDateISO = `${splittedDate[0]}-${splittedDate[1]}-01`;

      return { currDateISO, startDateISO };
    }

    // Initializes while document is ready
    $(document).ready(function () {
      SELECT_LOCATION_FROM.select2();
      SELECT_LOCATION_TO.select2();
      SELECT_LOKASI_REPARASI.select2();

      const dates = getStartDateAndCurrentDate();
      INPUT_TGL_AWAL.val(dates.startDateISO);
      INPUT_TGL_AKHIR.val(dates.currDateISO);
    });

    // event handler
    $(document).ready(function () {
      CHECKBOX_ALL_TABLE.on("click", (e) => checkAll(e));
      BUTTON_SUBMIT_DO_FORM.on("click", () => onSubmitFormDORepair());
      BUTTON_SEARCH_DO_FORM.on("click", () => onSearchFormDORepair());
    });

    function addPackage({ numOfArray, html, nameOfPackage }) {
      if (numOfArray === 0) return "";

      return `
                <div class="col-6">
                  <div class="col d-flex flex-column gap-1">
                    <h6 class="text-md fw-bold">Paket ${nameOfPackage}:</h6>
                    <h5 class="text-lg fw-normal" id="ListPackageUtama"></h5>
                    <ul>${html}</ul
                  </div>
                </div>
              `;
    }

    function generateHtmlPaket({ array = [] }) {
      const generateHtml = array.reduce((accumulator, calculator) => {
        const { NomorRepair, JenisReparasi, TipeReparasi, NamaTipeReparasi } =
          calculator;

        accumulator += `
                  <li>${JenisReparasi}</li>
                `;
        return accumulator;
      }, "");

      return generateHtml;
    }

    function listPackage({ nomorRepair }) {
      const filterPackageByNomorRepair = DATA_LIST_PACKAGE_REPARASI.filter(
        (item) => item["NomorRepair"] === nomorRepair
      );

      const filterPaketUtama = filterPackageByNomorRepair.filter(
        (item) => item["TipeReparasi"] === 1
      );
      const filterPaketSatuan = filterPackageByNomorRepair.filter(
        (item) => item["TipeReparasi"] === 2
      );

      const resultHtmlPaketUtama = addPackage({
        numOfArray: filterPaketUtama.length,
        html: generateHtmlPaket({ array: filterPaketUtama }),
        nameOfPackage: "Utama",
      });

      const resultHtmlPaketSatuan = addPackage({
        numOfArray: filterPaketSatuan.length,
        html: generateHtmlPaket({ array: filterPaketSatuan }),
        nameOfPackage: "Satuan",
      });

      return resultHtmlPaketUtama + resultHtmlPaketSatuan;
    }

    function listStone({ nomorRepair }) {
      const filterAndGenerateHtml = DATA_LIST_STONE_REPARASI.filter(
        (item) => item["NomorRepair"] === nomorRepair
      ).reduce((accumulator, calculator) => {
        const {
          NomorRepair,
          PLU,
          Parcel,
          GIA,
          Qty,
          Carat,
          Setting,
          OngkosPB,
          OngkosPBR,
          OngkosPBJual,
          OngkosSetting,
        } = calculator;
        accumulator += `
          <tr>
            <td>${Parcel}</td>
            <td>${GIA || "-"}</td>
            <td>${Qty}</td>
            <td>${Carat}</td>
            <td>${Setting}</td>
            <td>${formatNumberToIDR(OngkosPB)}</td>
            <td>${formatNumberToIDR(OngkosPBR)}</td>
            <td>${formatNumberToIDR(OngkosPBJual)}</td>
            <td>${formatNumberToIDR(OngkosSetting)}</td>
          </tr>
        `;

        return accumulator;
      }, "");

      return !filterAndGenerateHtml
        ? ""
        : `
      <table class="table">
        <thead>
        <tr>
          <th>Parcel</th>
          <th>GIA</th>
          <th>Qty</th>
          <th>Carat</th>
          <th>Setting</th>
          <th>Ongkos PB M</th>
          <th>Ongkos PB R</th>
          <th>Ongkos PB Final</th>
          <th>Ongkos Setting</th>
        </tr>
        </thead>
        <tbody>
          ${filterAndGenerateHtml}
        </tbody>
        </table>`;
    }

    // Fetch Initial Data
    $(document).ready(function () {
      const urlParams = new URLSearchParams(window.location.search);
      const NomorDO = urlParams.get("NomorDO");
      NomorDO.substring(4,6) == 'KL'?$('#naming').html('DO BRJ Klampis'):$('#naming').html('DO BRJ Factory');

      $("#nomorDOKirim").text(NomorDO);

      $.ajax({
        url: "./index.aspx/GetSendDeliveryOrderRepairFactoryDetail",
        contentType: "application/json; charset=utf-8",
        type: "GET",
        data: { NomorDO: `'${NomorDO}'` },
        success: function (data) {
          DATA_LIST_REPARASI = data["d"]["DO"];
          DATA_LIST_PACKAGE_REPARASI = data["d"]["Package"];
          DATA_LIST_STONE_REPARASI = data["d"]["Stone"];

          const html = DATA_LIST_REPARASI.reduce((accumulator, calculator) => {
            const {
              IDDetailReparasi,
              NomorDO,
              NomorRepair,
              KodeKaret,
              Material,
              PLU,
              BeratAwal,
              BeratAkhir,
              SizeAwal,
              SizeAkhir,
              Status,
              TanggalDO,
              IDDocRepair,
              IDLokasi,
              TipeLokasi,
              NamaLokasi,
              IDBrandAsal,
              BrandAsal,
              ImgPicture,
              ProductItem,
              FrameColor,
            } = calculator;

            accumulator += `
              <div class="container border rounded p-5 mt-5">
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
                            src="${
                              "https://" +
                              window.location.hostname +
                              "/Images.aspx?path=" +
                              ImgPicture
                            }"
                          />
                        </div>
                        <div class="col">
                          <div class="row gy-5">
                            <div class="col-6">
                              <div class="col d-flex flex-column gap-1">
                                <h6 class="text-md fw-bold">Nomor Repair</h6>
                                <h5 class="text-lg fw-normal" id="NomorRepair">${NomorRepair}</h5>
                              </div>
                            </div>
                            <div class="col-6">
                              <div class="col d-flex flex-column gap-1">
                                <h6 class="text-md fw-bold">PLU</h6>
                                <h5 class="text-lg fw-normal" id="PLU">${PLU}</h5>
                              </div>
                            </div>
                            <div class="col-6">
                              <div class="col d-flex flex-column gap-1">
                                <h6 class="text-md fw-bold">Item</h6>
                                <h5 class="text-lg fw-normal" id="ProductItem">${ProductItem}</h5>
                              </div>
                            </div>
                            <div class="col-6">
                              <div class="col d-flex flex-column gap-1">
                                <h6 class="text-md fw-bold">Frame Color</h6>
                                <h5 class="text-lg fw-normal" id="FrameColor">${FrameColor}</h5>
                              </div>
                            </div>
                            <div class="col-6">
                              <div class="col d-flex flex-column gap-1">
                                <h6 class="text-md fw-bold">Berat Awal</h6>
                                <h5 class="text-lg fw-normal" id="BeratAwal">${BeratAwal} gr</h5>
                              </div>
                            </div>
                            <div class="col-6">
                              <div class="col d-flex flex-column gap-1">
                                <h6 class="text-md fw-bold">Berat Akhir</h6>
                                <h5 class="text-lg fw-normal" id="BeratAkhir">${BeratAkhir} gr</h5>
                              </div>
                            </div>
                            ${listPackage({ nomorRepair: NomorRepair })}
                          </div>
                        </div>
                        <div class="col-12">${listStone({
                          nomorRepair: NomorRepair,
                        })}</div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            `;

            return accumulator;
          }, "");

          CONTENT_LIST_REPARASI.html(html);
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {},
      });

      $.ajax({
        type: "GET",
        url: "./index.aspx/GetUserOutletAndDefaultSales",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          DATA_LOCATION = data["d"]["location"];
          DATA_SALES = data["d"]["sales"];
          const select2Location = SELECT_LOCATION_FROM.select2();
          const mapLocation = DATA_LOCATION.forEach(
            (
              {
                IDLokasi,
                NamaOutlet,
                KodeOutlet,
                TipeLokasi,
                BrandOutlet,
                IDBrand,
              },
              index
            ) => {
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
              optionLocation.setAttribute("data-brand-lokasi", BrandOutlet);
              optionLocation.setAttribute("data-id-brand-lokasi", IDBrand);
              select2Location.append(optionLocation);
            }
          );

          select2Location.trigger("change");
        },
        error: function (error) {
          console.error(error);
        },
      });

      $.ajax({
        type: "GET",
        url: "./index.aspx/GetLocationTujuan",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          DATA_LOCATION_TO = data["d"];
          const select2Location = SELECT_LOCATION_TO.select2();
          const mapLocation = DATA_LOCATION_TO.forEach(
            (
              {
                IDLokasi,
                NamaOutlet,
                KodeOutlet,
                TipeLokasi,
                BrandOutlet,
                IDBrand,
              },
              index
            ) => {
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
              optionLocation.setAttribute("data-brand-lokasi", BrandOutlet);
              optionLocation.setAttribute("data-id-brand-lokasi", IDBrand);
              select2Location.append(optionLocation);
            }
          );

          select2Location.trigger("change");
        },
        error: function (error) {
          console.error(error);
        },
      });

      $.ajax({
        type: "GET",
        url: "./index.aspx/GetLocationRepair",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          DATA_LOCATION_REPARASI = data["d"];
          const select2LocationRepair = SELECT_LOKASI_REPARASI.select2();
          DATA_LOCATION_REPARASI.forEach(({ ID, Nama, Kode }) => {
            let optionLocation = new Option(Nama, ID, false, false);
            optionLocation.setAttribute("data-id", ID);
            optionLocation.setAttribute("data-nama", Nama);
            optionLocation.setAttribute("data-kode", Kode);
            select2LocationRepair.append(optionLocation);
          });
          select2LocationRepair.trigger("change");
        },
        error: function (error) {
          console.error(error);
        },
      });
    });
  </script>
</asp:Content>
