<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="MasterPage.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="RepairDeliveryOrder_Index" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
  Form Approval
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
  Pengembalian Ke Outlet Asal
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  </form>

  <div class="container-fluid">
    <div class="row g-5">
      <div class="col-3">
        <div class="border rounded p-5">
          <h1 style="border-bottom: none" class="m-0 p-0 fw-bold">
            Cari Pengiriman
          </h1>
          <div class="py-3">
            <label for="select-location-from" class="form-label fw-bold"
              >Lokasi Outlet Asal</label
            >
            <select class="w-100" id="select-location-from"></select>
          </div>
          <div class="py-3">
            <div class="d-flex flex-column">
              <label for="select-lokasi-reparasi" class="form-label fw-bold"
                >Lokasi Reparasi</label
              >
              <select class="w-100" id="select-lokasi-reparasi"></select>
            </div>
          </div>
          <div class="py-3 d-none" id="content-select-outlet-to">
            <label for="select-location-to" class="form-label fw-bold"
              >Lokasi Outlet Tujuan</label
            >
            <select class="w-100" id="select-location-to"></select>
          </div>
          <div class="py-3">
            <div class="d-flex flex-column">
              <label for="input-tgl-start" class="form-label fw-bold"
                >Dari Tanggal</label
              >
              <input type="date" class="form-control" id="input-tgl-start" />
            </div>
          </div>
          <div class="py-3">
            <div class="d-flex flex-column">
              <label for="input-tgl-end" class="form-label fw-bold"
                >Hingga Tanggal</label
              >
              <input type="date" class="form-control" id="input-tgl-end" />
            </div>
          </div>
          <div class="py-3">
            <button
              class="btn btn-lg btn-outline-primary w-100"
              id="btn-search-do-form"
            >
              Tampilkan Daftar Delivery Order
            </button>
          </div>
        </div>
      </div>
      <div class="col-9">
        <div class="border rounded p-5">
          <table class="table table-hover">
            <thead>
              <tr>
                <th>Action</th>
                <th>Outlet Asal</th>
                <th>Outlet Tujuan</th>
                <th>Kurir</th>
                <th>Keterangan</th>
                <th>Operator</th>
                <th>Operator Tanggal</th>
              </tr>
            </thead>
            <tbody id="tbody-list-do"></tbody>
          </table>
        </div>
      </div>
    </div>
  </div>

  <!-- Modal -->
  <div
    class="modal fade"
    id="modalDetail"
    tabindex="-1"
    aria-labelledby="modalDetailLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-xl">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="modalDetailLabel">
            Approve Delivery Order
          </h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body" id="modal-body"></div>
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-secondary"
            data-bs-dismiss="modal"
            id="btn-cancel-do"
          >
            Batal
          </button>
          <button
            type="button"
            class="btn btn-primary"
            id="btn-approve-do"
            data-id-do=""
            data-is-warehouse=""
          >
            Approve
          </button>
        </div>
      </div>
    </div>
  </div>

  <script type="text/javascript">
    let DATA_LOCATION = [];
    let DATA_LOCATION_REPARASI = [];
    let DATA_LIST_REPARASI = [];
    let BUTTON_SEARCH_DO_FORM = $("#btn-search-do-form");
    const SELECT_LOCATION_FROM = $("#select-location-from");
    const SELECT_LOCATION_TO = $("#select-location-to");
    const SELECT_LOKASI_REPARASI = $("#select-lokasi-reparasi");
    const INPUT_TGL_START = $("#input-tgl-start");
    const INPUT_TGL_END = $("#input-tgl-end");
    const TBODY_LIST_DO = $("#tbody-list-do");
    const MODAL_BODY_CONTENT = $("#modal-body");
    const BUTTON_APPROVE_DO = $("#btn-approve-do");
    const BUTTON_CANCEL_DO = $("#btn-cancel-do");
    const CONTENT_SELECT_OUTLET_TUJUAN = $("#content-select-outlet-to");

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

    function onSearchFormDORepair(isSubmitting = false) {
      const select2LocationElement = $(
        SELECT_LOCATION_FROM.select2("data")["element"]
      );

      const select2LocationReparasiElement = $(
        SELECT_LOKASI_REPARASI.select2("data")["element"]
      );

      const select2LocationToElement = $(
        SELECT_LOCATION_TO.select2("data")["element"]
      );

      const parameters = {
        IDBrandAsal: select2LocationElement.attr("data-id-brand-lokasi"),
        IDAsal: select2LocationElement.attr("data-id-lokasi"),
        TipeAsal: select2LocationElement.attr("data-tipe-lokasi"),
        IDBrandTujuan: select2LocationToElement.attr("data-id-brand-lokasi"),
        IDTujuan: select2LocationToElement.attr("data-id-lokasi"),
        TipeTujuan: select2LocationToElement.attr("data-tipe-lokasi"),
        IDLokasiReparasi: select2LocationReparasiElement.attr("data-id"),
        Status: 1,
        TglStart: `"${INPUT_TGL_START.val()}"`,
        TglEnd: `"${INPUT_TGL_END.val()}"`,
      };

      BUTTON_SEARCH_DO_FORM.prop("disabled", true).text("Loading...");
      $.ajax({
        url: "./index.aspx/GetDeliveryOrderRepairKRM",
        contentType: "application/json; charset=utf-8",
        type: "GET",
        data: parameters,
        success: function (data) {
          const response = data["d"];
          TBODY_LIST_DO.empty();

          if (response.length === 0 && !isSubmitting) {
            toastr.error("Data tidak ditemukan!");
            return;
          }

          const additionalString = response.reduce(
            (accumulator, calculator) => {
              const {
                ID,
                IDBrandAsal,
                IDAsal,
                TipeAsal,
                NamaAsal,
                IDTujuan,
                IDBrandTujuan,
                TipeTujuan,
                NamaTujuan,
                Operator,
                OperatorTgl,
                IsWarehouse,
                NamaKurir,
                Keterangan,
              } = calculator;

              accumulator += `
              <tr>
                <td>
                  <button class="btn btn-lg btn-success" type="button" data-id-do="${ID}" data-id-brand-tujuan=${IsWarehouse==0?IDBrandTujuan:IDBrandAsal} data-is-warehouse="${IsWarehouse}" data-bs-toggle="modal" data-bs-target="#modalDetail"><i class="icon-eye-open"></i> Approve</button>
                </td>
                <td>${NamaAsal}</td>
                <td>${NamaTujuan}</td>
                <td>${NamaKurir}</td>
                <td>${Keterangan}</td>
                <td>${Operator}</td>
                <td>${OperatorTgl}</td>
              </tr>
            `;

              return accumulator;
            },
            ""
          );

          TBODY_LIST_DO.append(additionalString);
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          BUTTON_SEARCH_DO_FORM.prop("disabled", false).text(
            "Tampilkan Daftar Delivery Order"
          );
        },
      });
    }

    function onSelectRepairLocation(e) {
      const element = $(e.currentTarget);

      if (element.val() == 2) {
        const dataKodeLokasi = $(
          SELECT_LOCATION_FROM.select2("data")["element"]
        ).attr("data-kode-lokasi");

        onFetchMappingStore({ kodeStore: dataKodeLokasi });
        CONTENT_SELECT_OUTLET_TUJUAN.removeClass("d-none");
        return;
      }

      CONTENT_SELECT_OUTLET_TUJUAN.addClass("d-none");
    }

    function onFetchMappingStore({ kodeStore }) {
      $.ajax({
        type: "GET",
        url: "./index.aspx/GetLocationTujuan",
        contentType: "application/json; charset=utf-8",
        data: { kodeStore: `"${kodeStore}"` },
        success: function (data) {
          DATA_LOCATION_TO = data["d"];

          if (
            DATA_LOCATION_TO.length === 0 &&
            $("#select-lokasi-reparasi").val() == 2
          ) {
            const select2Location = SELECT_LOCATION_TO.select2();
            select2Location.empty();
            let optionLocation = new Option(
              "-- Tidak terdapat Service Point --",
              0,
              false,
              false
            );

            SELECT_LOCATION_TO.attr("disabled", true);
            select2Location.append(optionLocation);

            BUTTON_SEARCH_DO_FORM.attr("disabled", true);

            select2Location.trigger("change");
            return;
          }

          const select2Location = SELECT_LOCATION_TO.select2().empty();
          BUTTON_SEARCH_DO_FORM.attr("disabled", false);
          SELECT_LOCATION_TO.attr("disabled", false);

          const mapLocation = DATA_LOCATION_TO.forEach(
            (
              {
                IDLokasi,
                NamaOutlet,
                KodeOutlet,
                TipeLokasi,
                BrandOutlet,
                IDBrand,
                Province,
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
              optionLocation.setAttribute("data-province", Province);
              select2Location.append(optionLocation);
            }
          );

          select2Location.trigger("change");
        },
        error: function (error) {
          console.error(error);
        },
      });
    }

    function onOpenDetailDO(e) {
      const buttonElement = $(e.currentTarget);
      const idDo = buttonElement.attr("data-id-do");
      const isWarehouse = buttonElement.attr("data-is-warehouse");
      const idBrandTujuan = buttonElement.attr("data-id-brand-tujuan");

      BUTTON_APPROVE_DO.attr("data-id-do", idDo);
      BUTTON_APPROVE_DO.attr("data-is-warehouse", isWarehouse);
      BUTTON_APPROVE_DO.attr("data-id-brand-tujuan", idBrandTujuan);

      $.ajax({
        type: "GET",
        url: "./index.aspx/GetDetailDeliveryOrder",
        contentType: "application/json; charset=utf-8",
        data: {
          ID: idDo,
          IsWarehouse: isWarehouse,
          IDBrandTujuan: idBrandTujuan,
        },
        success: function (data) {
          const response = data["d"];

          function createTable(html) {
            return `
                <table class="table table-hover">
                  <thead>
                    <tr>
                      <th>Nomor Reparasi</th>
                      <th>PLU</th>
                      <th>Nama Customer</th>
                      <th>Nama Sales</th>
                      <th>Lampiran</th>
                    </tr>
                  </thead>
                  <tbody>${html}</tbody>
                </table>
              `;
          }

          const stringAdditional = response.reduce(
            (accumulator, calculator) => {
              const {
                IDDocRepair,
                NomorReparasi,
                NamaCustomer,
                NamaSales,
                PLU,
                Image,
                IsCancel,
              } = calculator;

              accumulator += `
              <tr>
                <td>
                  <div>
                    <a target="_blank" href="../../Report/RepairOrder/detail.aspx?id=${IDDocRepair}&idbrandasal=${idBrandTujuan}">${NomorReparasi}</a>
                    <span>${
                      IsCancel
                        ? `<span class="badge rounded-pill ml-3 bg-danger">Return / Cancel</span>`
                        : ""
                    }</span>
                  </div>
                </td>
                <td>
                  ${PLU}
                </td>
                <td>
                  ${NamaCustomer}
                </td>
                <td>
                  ${NamaSales}
                </td>
                <td>
                  <a href="${Image}" target="_blank" class="btn btn-outline-primary">
                      <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-paperclip" viewBox="0 0 16 16">
                        <path d="M4.5 3a2.5 2.5 0 0 1 5 0v9a1.5 1.5 0 0 1-3 0V5a.5.5 0 0 1 1 0v7a.5.5 0 0 0 1 0V3a1.5 1.5 0 1 0-3 0v9a2.5 2.5 0 0 0 5 0V5a.5.5 0 0 1 1 0v7a3.5 3.5 0 1 1-7 0z"/>
                      </svg>
                    </a>
                </td>
              </tr>
            `;

              return accumulator;
            },
            ""
          );

          MODAL_BODY_CONTENT.html(createTable(stringAdditional));
        },
        error: function (error) {
          console.log(error);
        },
      });
    }

    function onApproveDO(e) {
      const buttonElement = $(e.currentTarget);
      const idDo = buttonElement.attr("data-id-do");
      const isWarehouse = buttonElement.attr("data-is-warehouse");
      const idBrandTujuan = buttonElement.attr("data-id-brand-tujuan");

      BUTTON_APPROVE_DO.prop("disabled", true).text("Loading...");
      BUTTON_CANCEL_DO.prop("disabled", true).text("Loading...");
      $.ajax({
        url: "./index.aspx/ApproveDetailDeliveryOrder",
        type: "POST",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
          ID: idDo,
          IsWarehouse: isWarehouse,
          IDBrandTujuan: idBrandTujuan,
        }),
        success: function (data) {
          toastr.success("Berhasil approve Delivery Order!");
          onSearchFormDORepair(true);
        },
        error: function (error) {
          toastr.error("Terjadi kesalahan saat approve Delivery Order!");
          console.log(error);
        },
        complete: function (data) {
          $("#modalDetail").modal("hide");
          BUTTON_APPROVE_DO.prop("disabled", false).text("Approve");
          BUTTON_CANCEL_DO.prop("disabled", false).text("Batal");
        },
      });
    }

    // Initializes while document is ready
    $(document).ready(function () {
      SELECT_LOCATION_FROM.select2();
      SELECT_LOCATION_TO.select2();
      SELECT_LOKASI_REPARASI.select2();

      let currDate = new Date();
      const offset = currDate.getTimezoneOffset();
      currDate = new Date(currDate.getTime() - offset * 60 * 1000);
      const currDateISO = currDate.toISOString().split("T")[0];
      const splittedDate = currDateISO.split("-");
      const startDateISO = `${splittedDate[0]}-${splittedDate[1]}-01`;

      INPUT_TGL_START.val(startDateISO);
      INPUT_TGL_END.val(currDateISO);
    });

    // event handler
    $(document).ready(function () {
      $("table tbody").on("click", "button", (e) => onOpenDetailDO(e));
      BUTTON_APPROVE_DO.on("click", (e) => onApproveDO(e));
      SELECT_LOKASI_REPARASI.on("change", (e) => onSelectRepairLocation(e));
      BUTTON_SEARCH_DO_FORM.on("click", () => onSearchFormDORepair());

      $(document).on("change", "#select-location-from", (e) => {
        const element = $(e.currentTarget);

        element.children().each((idx, elem) => {
          if ($(elem).is(":selected")) {
            const kodeStore = $(elem).attr("data-kode-lokasi");
            onFetchMappingStore({ kodeStore: kodeStore });
          }
        });
      });
    });

    // Fetch Initial Data
    $(document).ready(function () {
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
