<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="MasterPage.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="RepairDeliveryOrder_Index" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
  Form Register Pengembalian
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
  Menuju Outlet Asal
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
            Opsi Pengiriman
          </h1>
          <div class="py-3">
            <label for="select-location-from" class="form-label fw-bold"
              >Lokasi Outlet Asal</label
            >
            <select class="w-100" id="select-location-from"></select>
          </div>
          <div class="py-3">
            <label for="input-kurir" class="form-label fw-bold">Kurir</label>
            <input
              class="form-control"
              placeholder="Masukkan kurir..."
              id="input-kurir"
            />
          </div>
          <div class="py-3">
            <div class="d-flex flex-column">
              <label for="select-lokasi-reparasi" class="form-label fw-bold"
                >Lokasi Reparasi</label
              >
              <select class="w-100" id="select-lokasi-reparasi"></select>
            </div>
          </div>
          <div class="py-3 d-none" id="content-select-outlet-tujuan">
            <label for="select-location-to" class="form-label fw-bold"
              >Lokasi Outlet Tujuan</label
            >
            <select class="w-100" id="select-location-to"></select>
          </div>
          <div class="py-3">
            <div class="d-flex flex-column">
              <label for="textarea-keterangan-do" class="form-label fw-bold"
                >Keterangan</label
              >
              <textarea
                id="textarea-keterangan-do"
                class="form-control"
                cols="10"
                rows="8"
                placeholder="Masukkan keterangan..."
              ></textarea>
            </div>
          </div>
          <div class="py-3">
            <button
              class="btn btn-lg btn-outline-primary w-100"
              id="btn-search-do-form"
            >
              Tampilkan Daftar Reparasi
            </button>
          </div>
        </div>
      </div>
      <div class="col-9 d-none" id="content-list-reparasi">
        <div class="border rounded p-5">
          <table class="table table-hover">
            <thead>
              <tr>
                <th>
                  <div class="form-check">
                    <input
                      class="form-check-input"
                      type="checkbox"
                      id="checkbox-all-table"
                    />
                  </div>
                </th>
                <th>No. Repair</th>
                <th>No. PLU</th>
                <th>Customer</th>
                <th>Sales</th>
                <th>Attachment</th>
              </tr>
            </thead>
            <tbody id="tbody-list-reparasi"></tbody>
          </table>
        </div>
        <div class="row gx-3 mt-3 justify-content-end">
          <div class="col-3">
            <button class="btn-primary w-100 btn btn-lg" id="btn-submit-form">
              Submit Form
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>

  <script type="text/javascript">
    let DATA_LOCATION = [];
    let DATA_LOCATION_TO = [];
    let DATA_LOCATION_REPARASI = [];
    let DATA_LIST_REPARASI = [];
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

    function checkAll(e) {
      const tableCheckbox = $("[id*='checkbox_table']");
      const isCheckAll = $(e.currentTarget).is(":checked");
      tableCheckbox.each((idx, elem) => $(elem).prop("checked", isCheckAll));
    }

    function onSubmitFormDORepair() {
      let result = [];

      $("table tbody tr").each(function () {
        $(this)
          .find("input")
          .each(function (index) {
            const isChecked = $(this).is(":checked");

            if (!isChecked) return;
            const element = $(this);
            result.push({
              IDDocRepair: element.attr("data-id-doc-repair"),
              NomorRepair: element.attr("data-nomor-reparasi"),
            });
          });
      });

      if (result.length === 0) {
        toastr.error("Tolong pilih data yang ingin di Delivery Order!");
        return;
      }

      const select2LocationElement = $(
        SELECT_LOCATION_FROM.select2("data")["element"]
      );

      const select2LocationReparasiElement = $(
        SELECT_LOKASI_REPARASI.select2("data")["element"]
      );

      const select2LocationToReparasiElement = $(
        SELECT_LOCATION_TO.select2("data")["element"]
      );

      const objectToPush = {
        IDBrandAsal: select2LocationElement.attr("data-id-brand-lokasi"),
        IDAsal: select2LocationElement.attr("data-id-lokasi"),
        TipeAsal: select2LocationElement.attr("data-tipe-lokasi"),
        NamaAsal: select2LocationElement.attr("data-nama-lokasi"),
        IDBrandTujuan: select2LocationToReparasiElement.attr(
          "data-id-brand-lokasi"
        ),
        IDTujuan: select2LocationToReparasiElement.attr("data-id-lokasi"),
        TipeTujuan: select2LocationToReparasiElement.attr("data-tipe-lokasi"),
        NamaTujuan: select2LocationToReparasiElement.attr("data-nama-lokasi"),
        IDLokasiReparasi: select2LocationReparasiElement.attr("data-id"),
        Kurir: INPUT_KURIR.val(),
        Keterangan: TEXTAREA_KETERANGAN.val(),
        Operator: "",
        IsCrossBrand:
          select2LocationToReparasiElement.attr("data-id-brand-lokasi") !=
          select2LocationElement.attr("data-id-brand-lokasi")
            ? 1
            : 0,
        DO_Detail: result,
      };

      BUTTON_SEARCH_DO_FORM.prop("disabled", true).text("Loading...");
      BUTTON_SUBMIT_DO_FORM.prop("disabled", true).text("Loading...");
      $.ajax({
        url: "./index.aspx/PostSendDeliveryOrderRepair",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
          args: objectToPush,
          IDBrandAsal: objectToPush.IDBrandTujuan,
        }),
        type: "POST",
        success: function (data) {
          onSearchFormDORepair(true);
          CHECKBOX_ALL_TABLE.prop("checked", false);
          toastr.success("Berhasil membuat Delivery Order!");
        },
        error: function (error) {
          toastr.error("Terjadi kesalahan saat membuat Delivery Order!");
          console.error(error);
        },
        complete: function (data) {
          BUTTON_SEARCH_DO_FORM.prop("disabled", false).text(
            "Tampilkan Daftar Reparasi"
          );
          BUTTON_SUBMIT_DO_FORM.prop("disabled", false).text("Submit Form");
        },
      });
    }

    function onSearchFormDORepair(isSubmitting = false) {
      const select2LocationElement = $(
        SELECT_LOCATION_FROM.select2("data")["element"]
      );
      const select2LocationToElement = $(
        SELECT_LOCATION_TO.select2("data")["element"]
      );

      const parameters = {
        idbrandasal: select2LocationElement.attr("data-id-brand-lokasi"),
        idasal: select2LocationElement.attr("data-id-lokasi"),
        tipeasal: select2LocationElement.attr("data-tipe-lokasi"),
        idbrandtujuan: select2LocationToElement.attr("data-id-brand-lokasi"),
        idtujuan: select2LocationToElement.attr("data-id-lokasi"),
        tipetujuan: select2LocationToElement.attr("data-tipe-lokasi"),
      };

      BUTTON_SEARCH_DO_FORM.prop("disabled", true).text("Loading...");

      $.ajax({
        url: "./index.aspx/GetSendDeliveryOrderRepair",
        contentType: "application/json; charset=utf-8",
        type: "GET",
        data: parameters,
        success: function (data) {
          DATA_LIST_REPARASI = data["d"];
          TBODY_LIST_REPARASI.empty();

          if (DATA_LIST_REPARASI.length === 0 && !isSubmitting) {
            CONTENT_LIST_REPARASI.addClass("d-none");
            toastr.error("Data tidak ditemukan!");
            return;
          }

          const stringAdditional = DATA_LIST_REPARASI.reduce(
            (accumulator, calculator) => {
              const {
                IDDocRepair,
                NomorReparasi,
                NamaCustomer,
                NamaSales,
                PLU,
                Image,
              } = calculator;

              accumulator += `
                <tr>
                  <td>
                    <div class="form-check">
                      <input class="form-check-input" data-plu="${PLU}" data-nomor-reparasi="${NomorReparasi}" data-id-doc-repair="${IDDocRepair}" type="checkbox" id="checkbox_table_${IDDocRepair}}" />
                    </div>
                  </td>
                  <td>
                    <a target="_blank" href="../../Report/RepairOrder/detail.aspx?id=${IDDocRepair}&idbrandasal=${parameters.idbrandtujuan}">${NomorReparasi}</a>
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

          TBODY_LIST_REPARASI.empty();
          CONTENT_LIST_REPARASI.removeClass("d-none");
          TBODY_LIST_REPARASI.append(stringAdditional);
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          BUTTON_SEARCH_DO_FORM.prop("disabled", false).text(
            "Tampilkan Daftar Reparasi"
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

    // Initializes while document is ready
    $(document).ready(function () {
      SELECT_LOCATION_FROM.select2();
      SELECT_LOCATION_TO.select2();
      SELECT_LOKASI_REPARASI.select2();
    });

    // event handler
    $(document).ready(function () {
      CHECKBOX_ALL_TABLE.on("click", (e) => checkAll(e));
      BUTTON_SUBMIT_DO_FORM.on("click", () => onSubmitFormDORepair());
      BUTTON_SEARCH_DO_FORM.on("click", () => onSearchFormDORepair());
      SELECT_LOKASI_REPARASI.on("change", (e) => onSelectRepairLocation(e));

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
