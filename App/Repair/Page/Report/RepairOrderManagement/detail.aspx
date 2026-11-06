<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="MasterPage.master" AutoEventWireup="true"
CodeFile="detail.aspx.cs" Inherits="DOKRM_detail" %>

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
  Report Reparasi
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
  Detail Management
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
                  <th class="text-right">Biaya Reparasi M</th>
                  <th class="text-right">Biaya Reparasi R</th>
                  <th class="text-right">Biaya Reparasi Jual</th>
                </tr>
              </thead>
              <tbody id="tbody-biaya-reparasi"></tbody>
              <tfoot id="tfoot-biaya-reparasi"></tfoot>
            </table>
          </div>
        </div>
      </div>

      <div class="container mt-2">
        <div class="row p-0">
          <div class="col-12 border rounded p-5">
            <h1 style="border: none; padding-bottom: 10px">Tambah Emas</h1>
            <table class="table table-hover">
              <thead>
                <tr class="text-right">
                  <th>Berat Sebelum</th>
                  <th>Berat Sesudah (Tanpa Batu)</th>
                  <th>Berat Sesudah</th>
                  <th>Selisih</th>
                  <th>Batas Toleransi</th>
                  <th>Total Charge</th>
                  <!-- <th>Harga Emas</th> -->
                  <th>Biaya M</th>
                  <th>Biaya R</th>
                  <th>Biaya Jual</th>
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
                  <th class="text-right">Biaya M</th>
                  <th class="text-right">Biaya R</th>
                  <th class="text-right">Biaya Jual</th>
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

    function formatNumberToUSD(number) {
      const USD = new Intl.NumberFormat("en-US", {
        style: "currency",
        currency: "USD",
      }).format(number);
      return USD;
    }

    // Fetch Initial Data
    $(document).ready(function () {
      const urlParams = new URLSearchParams(window.location.search);
      const id = urlParams.get("id");

      $.ajax({
        type: "GET",
        url: "./detail.aspx/GetDetailRepair",
        data: {
          IDDocRepair: id,
        },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];
          DATA_ITEM = response["docRepairDetail"];

          const package = response["docRepairPackage"];
          const parcel = response["docRepairParcel"];

          IMG_PRODUCT.attr(
            "src",
            "/Images.aspx?path=" + DATA_ITEM["ImgProduct"]
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
          let biayaPerbaikanInstruksi=0;
          let biayaPerbaikanInstruksiM = 0;
          let biayaPerbaikanInstruksiR = 0;
          let biayaPerbaikanInstruksiJual = 0;
          const jenisReparasi = package.reduce((accumulator, calculator) => {
            const { NamaPackage, NamaTipePackage, Biaya, BiayaR, BiayaJual } =
              calculator;

            let BiayaJualNew = BiayaJual;

            
            if (NamaPackage.includes("Bongkar/Pasang Batu"))
              BiayaJualNew = hargaTotalPBSetting;

            biayaPerbaikan += parseFloat(BiayaJualNew);
            biayaPerbaikanInstruksi += parseFloat(BiayaJualNew);
            biayaPerbaikanInstruksiM += parseFloat(Biaya);
            biayaPerbaikanInstruksiR += parseFloat(BiayaR);
            biayaPerbaikanInstruksiJual += parseFloat(BiayaJualNew);

            accumulator += `
                  <tr>
                    <td>${NamaTipePackage}</td>
                    <td>${NamaPackage}</td>
                    <td class="text-right">${formatNumberToIDR(
                      parseFloat(Biaya)
                    )}</td>
                    <td class="text-right">${formatNumberToIDR(
                      parseFloat(BiayaR)
                    )}</td>
                    <td class="text-right">${formatNumberToIDR(
                      parseFloat(BiayaJualNew)
                    )}</td>
                  </tr>
                `;
            return accumulator;
          }, "");

          $("#tbody-biaya-reparasi").append(jenisReparasi);
          $("#tfoot-biaya-reparasi").append(`
            <tr class="fw-bold text-right">
              <td colspan="2" class="text-left">Total</td>
              <td>${formatNumberToIDR(biayaPerbaikanInstruksiM)}</td>
              <td>${formatNumberToIDR(biayaPerbaikanInstruksiR)}</td>
              <td>${formatNumberToIDR(biayaPerbaikanInstruksiJual)}</td>
            </tr>
          `);

          const selisihBerat = (
            DATA_ITEM["BeratAkhirTanpaBatu"] - DATA_ITEM["BeratTimbangActual"]
          ).toFixed(2);

          const isPaidSelisih =
            DATA_ITEM["TotalGramLebih"] > 0
              ? "class='text-danger'"
              : "class='text-success'";

          biayaPerbaikan += DATA_ITEM["HargaBayar"];

          DEFAULT.totalNeedToPaidEmas = DATA_ITEM["HargaBayar"];

          // <td>${formatNumberToIDR(DATA_ITEM["HargaEmas"])}</td>

          $("#tbody-biaya-tambah-emas").append(`
                <tr class="text-right">
                  <td>${DATA_ITEM["BeratTimbangActual"]} gr</td>
                  <td>${DATA_ITEM["BeratAkhirTanpaBatu"]} gr</td>
                  <td>${DATA_ITEM["BeratTimbangAkhir"]} gr</td>
                  <td ${isPaidSelisih}>${selisihBerat} gr</td>
                  <td>${DATA_ITEM["BatasToleransi"].toFixed(2)} gr</td>
                  <td>${DATA_ITEM["TotalGramLebih"]} gr</td>
                  <td>${formatNumberToIDR(DATA_ITEM["HargaBayarM"])}</td>
                  <td>${formatNumberToIDR(DATA_ITEM["HargaBayarR"])}</td>
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
                Color,
                HargaTotalRupiahRound,
                HargaTotalM,
                HargaTotal,
                TotalButir,
                TotalCarat,
                CaratButir,
              } = calculator;

              biayaPerbaikan += HargaTotalRupiahRound;
              biayaPerbaikanTambahBatu += HargaTotalRupiahRound;

              accumulator += `
              <tr>
                <td>${Stone} - ${Color} - ${Clarity}</td>
                <td class="text-right">${TotalButir}</td>  
                <td class="text-right">${CaratButir}</td>  
                <td class="text-right">${TotalCarat}</td>  
                <td class="text-right">${formatNumberToUSD(HargaTotalM)}</td>  
                <td class="text-right">${formatNumberToUSD(HargaTotal)}</td>  
                <td class="text-right">${formatNumberToIDR(
                  HargaTotalRupiahRound
                )}</td>  
              </tr>
            `;

              return accumulator;
            }, ``);

            TBODY_BIAYA_TAMBAH_BATU.append(parcelHtml);
            $("#content-tambah-batu").removeClass("d-none");
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

          DEFAULT = {
            ...DEFAULT,
            totalPayment: biayaPerbaikan,
            totalPaid: 0,
            totalNeedToPaid: biayaPerbaikan,
            totalNeedToPaidStone: biayaPerbaikanTambahBatu,
            totalNeedToPaidInstruksi: biayaPerbaikanInstruksiJual,
          };

          TD_TOTAL_BAYAR.html(formatNumberToIDR(biayaPerbaikan));
          TD_TOTAL_PEMBAYARAN.html(formatNumberToIDR(biayaPerbaikan));
        },

        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          const Status = data["responseJSON"]["d"]["docRepairDetail"]["Status"];
          LOADER.hide();
          if (Status != 9 && Status != 10) window.close();
        },
      });
    });
  </script>
</asp:Content>
