<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="MasterPage.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="RepairDeliveryOrder_Index" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
  Form Pengrajin
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
  Penerimaan / Proses / Finish
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  </form>

  <div class="container-fluid">
    <form id="form-service-point">
      <div class="row d-flex gx-3">
        <div class="col-2">
          <label for="select-location-from" class="form-label fw-bold"
            >Lokasi Outlet Tujuan</label
          >
          <select class="w-100" id="select-location-from"></select>
          <span class="text-danger text-xs" id="notif-alert"></span>
        </div>
        <div class="col-2">
          <div id="content-select-outlet-tujuan">
            <label for="input-plu-reparasi" class="form-label fw-bold"
              >Cari No PLU / Reparasi</label
            >
            <input value="" class="form-control" id="input-plu-reparasi" />
          </div>
        </div>
        <button
          class="btn btn-primary btn-lg w-auto align-self-end"
          id="btn-search-do"
        >
          Cari <span style="margin-left: 5px"><i class="icon-search"></i></span>
        </button>
      </div>
    </form>
    <div
      class="row mt-3 p-5 rounded border d-none"
      id="content-list-service-point-reparasi"
    >
      <div class="col-12">
        <div class="table-responsive">
          <table class="table table-hover">
            <thead>
              <tr>
                <th class="text-nowrap">No</th>
                <th class="text-nowrap">No. Repair</th>
                <th class="text-nowrap">No. PLU</th>
                <th class="text-nowrap">Customer</th>
                <th class="text-nowrap">Sales</th>
                <th class="text-nowrap">Operator Terima</th>
                <th class="text-nowrap">Operator Proses</th>
                <th class="text-nowrap">Operator Finish</th>
                <th class="text-nowrap"></th>
              </tr>
            </thead>
            <tbody id="tbody-list-service-point" class="text-nowrap"></tbody>
          </table>
        </div>
      </div>
    </div>
  </div>

  <!-- Modal -->
  <div
    class="modal fade"
    id="changeStatusModal"
    data-bs-backdrop="static"
    data-bs-keyboard="false"
    tabindex="-1"
    aria-labelledby="staticBackdropLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="staticBackdropLabel">
            Ubah Status Reparasi
          </h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          Apakah anda ingin mengubah status proses reparasi ini menjadi
          <span id="span-status" class="fw-bold"></span> oleh pengrajin?
        </div>
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-secondary"
            data-bs-dismiss="modal"
          >
            Batal
          </button>
          <button type="button" class="btn btn-primary" id="btn-update-status">
            Yakin
          </button>
        </div>
      </div>
    </div>
  </div>

  <script type="text/javascript">
    const SELECT_LOCATION_FROM = $("#select-location-from");
    const SELECT_LOCATION_TO = $("#select-location-to");
    const TBODY_LIST_SERVICE_POINT_REPARASI = $("#tbody-list-service-point");
    const CONTENT_LIST_SERVICE_POINT_REPARASI = $(
      "#content-list-service-point-reparasi"
    );
    const SUBMIT_DO_SEARCH_FORM = $("#form-service-point");
    const BUTTON_DO_SEARCH = $("#btn-search-do");

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

      const idBrand = select2LocationElement.attr("data-id-brand-lokasi");
      const parameters = {
        IDLokasi: select2LocationElement.attr("data-id-lokasi"),
        KodeBrand: `'${idBrand == 7 ? "F" : idBrand == 5 ? "P" : "M"}'`,
        TipeLokasi: select2LocationElement.attr("data-tipe-lokasi"),
        Keywords: `"${$("#input-plu-reparasi").val().trim()}"`,
      };

      BUTTON_DO_SEARCH.prop("disabled", true).text("Loading...");

      $.ajax({
        url: "./index.aspx/GetListRepairServicePoint",
        data: parameters,
        contentType: "application/json; charset=utf-8",
        type: "GET",
        success: function (data) {
          const DATA_LIST_SERVICE_POINT_REPARASI = data["d"];

          TBODY_LIST_SERVICE_POINT_REPARASI.empty();
          if (DATA_LIST_SERVICE_POINT_REPARASI.length === 0 && !isSubmitting) {
            CONTENT_LIST_SERVICE_POINT_REPARASI.addClass("d-none");
            toastr.error("Data tidak ditemukan!");
            return;
          }
          const stringAdditional = DATA_LIST_SERVICE_POINT_REPARASI.reduce(
            (accumulator, calculator) => {
              const {
                No,
                ID,
                NamaCustomer,
                IDBrandAsal,
                NamaSales,
                Nomor,
                PLU,
                Finish,
                FinishTgl,
                Edit,
                EditTgl,
                Status,
                OperatorTerima,
                NIKTerima,
                TglTerima,
                OperatorProses,
                NIKProses,
                TglProses,
                OperatorFinish,
                NIKFinish,
                TglFinish,
              } = calculator;

              const buttonShow =
                Status === 1
                  ? `<button class="btn btn-secondary btn-lg btn-open-modal" data-bs-toggle="modal" data-bs-target="#changeStatusModal"  data-id-doc-repair="${ID}" data-status="2" data-id-brand-asal="${IDBrandAsal}">Terima</button>`
                  : Status === 2
                  ? `<button class="btn btn-primary btn-lg btn-open-modal" data-bs-toggle="modal" data-bs-target="#changeStatusModal"  data-id-doc-repair="${ID}" data-status="3" data-id-brand-asal="${IDBrandAsal}">Proses</button>`
                  : Status === 3
                  ? `<button class="btn btn-success btn-lg btn-open-modal" data-bs-toggle="modal" data-bs-target="#changeStatusModal"  data-id-doc-repair="${ID}" data-status="4" data-id-brand-asal="${IDBrandAsal}">Selesai</button>`
                  : "<button class='btn btn-lg btn-outline-success fw-bold'>Selesai dikerjakan</button>";

              const terima = !!OperatorTerima
                ? `
                      <div>${OperatorTerima} (${NIKTerima})</div>
                      <div>${TglTerima}</div>
                    `
                : "-";

              const proses = !!OperatorProses
                ? `
                      <div>${OperatorProses} (${NIKProses})</div>
                      <div>${TglProses}</div>
                    `
                : "-";

              const finish = !!OperatorFinish
                ? `
                      <div>${OperatorFinish} (${NIKFinish})</div>
                      <div>${TglFinish}</div>
                    `
                : "-";

              accumulator += `
                <tr>
                  <td style="vertical-align: middle;">${No}</td>
                  <td style="vertical-align: middle;">${Nomor}</td>
                  <td style="vertical-align: middle;">${PLU}</td>
                  <td style="vertical-align: middle;">${NamaCustomer}</td>
                  <td style="vertical-align: middle;">${NamaSales}</td>
                  <td style="vertical-align: middle;">${terima}</td>
                  <td style="vertical-align: middle;">${proses}</td>
                  <td style="vertical-align: middle;">${finish}</td>
                  <td style="vertical-align: middle;">${buttonShow}</td>
                </tr>
              `;
              return accumulator;
            },
            ""
          );
          CONTENT_LIST_SERVICE_POINT_REPARASI.removeClass("d-none");
          TBODY_LIST_SERVICE_POINT_REPARASI.empty().append(stringAdditional);
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          BUTTON_DO_SEARCH.prop("disabled", false).html(
            'Cari <span style="margin-left: 5px"><i class="icon-search"></i></span>'
          );
        },
      });
    }

    // Initializes while document is ready
    $(document).ready(function () {
      SELECT_LOCATION_FROM.select2();
    });

    // event handler
    $(document).ready(function () {
      BUTTON_DO_SEARCH.on("click", () => onSearchFormDORepair(false));

      $(document).on("click", ".btn-open-modal", function (e) {
        const text = $(this).text();
        $("#span-status").text(text);

        $("#btn-update-status").attr(
          "data-id-doc-repair",
          $(this).attr("data-id-doc-repair")
        );
        $("#btn-update-status").attr(
          "data-status",
          $(this).attr("data-status")
        );
        $("#btn-update-status").attr(
          "data-id-brand-asal",
          $(this).attr("data-id-brand-asal")
        );
      });

      $(document).on("click", "#btn-update-status", function (e) {
        const element = $(this);
        const IDDocRepair = element.attr("data-id-doc-repair");
        const Status = element.attr("data-status");
        const IDBrandAsal = element.attr("data-id-brand-asal");

        const buttons = element.parent().children("button");

        buttons.text("Loading...").attr("disabled", true);        
        $.ajax({
          type: "POST",
          url: "./index.aspx/UpdateStatusRepairServicePoint",
          contentType: "application/json; charset=utf-8",
          data: JSON.stringify({ IDDocRepair, Status, IDBrandAsal }),
          success: function (data) {
            const message = data["d"];
            onSearchFormDORepair(true);
            toastr.success(message);
            $("#changeStatusModal").modal("hide");
          },
          error: function (error) {
            console.error(error);
            toastr.error(
              "Error: Terjadi kesalahan pada system, silahkan hubungi tim IT!"
            );
          },
          complete: function (data) {
            element.text("Yakin").attr("disabled", false);
            element.siblings("button").text("Batal").attr("disabled", false);
          },
        });
      
      });
    });

    // Fetch Initial Data
    $(document).ready(function () {
      $.ajax({
        type: "GET",
        url: "./index.aspx/GetLocationPengrajin",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          DATA_LOCATION = data["d"]["location"];
          $("#notif-alert").text(data["d"]["message"]);
          const select2Location = SELECT_LOCATION_FROM.select2();

          const mapLocation = DATA_LOCATION.forEach(
            (
              { IDLokasi, NamaOutlet, KodeOutlet, TipeLokasi, IDBrand },
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
    });
  </script>
</asp:Content>
