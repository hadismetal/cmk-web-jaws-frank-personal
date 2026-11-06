<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="MasterPage.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="RepairDeliveryOrder_Index" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
  Form Register Reparasi
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
  Service Point
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
            >Lokasi Service Point</label
          >
          <select class="w-100" id="select-location-from"></select>
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
        <table class="table table-hover">
          <thead>
            <tr>
              <th>No</th>
              <th>No. Repair</th>
              <th>No. PLU</th>
              <th>Customer</th>
              <th>Sales</th>
              <th></th>
            </tr>
          </thead>
          <tbody id="tbody-list-service-point"></tbody>
        </table>
      </div>
    </div>
  </div>

  <!-- Modal -->
  <div
    class="modal fade"
    id="staticBackdrop"
    data-bs-backdrop="static"
    data-bs-keyboard="false"
    tabindex="-1"
    aria-labelledby="staticBackdropLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <div class="modal-title fs-5 fw-bold" id="staticBackdropLabel">
            Return / Cancel
          </div>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          Apakah anda yakin ingin return / cancel reparasi ini?
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
            id="btn-return-cancel-service-point"
          >
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
    const BUTTON_RETURN_CANCEL_SERVICE_POINT = $(
      "#btn-return-cancel-service-point"
    );

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
              } = calculator;

              accumulator += `
                <tr>
                  <td>${No}</td>
                  <td>${Nomor}</td>
                  <td>${PLU}</td>
                  <td>${NamaCustomer}</td>
                  <td>${NamaSales}</td>
                  <td>
                    <a class="btn btn-success" target="_blank" href="../../Report/RepairPrintWorkOrder?id=${ID}&idbrandasal=${IDBrandAsal}">Print</a>
                    <a class="btn btn-primary" target="_blank" href="../../Report/RepairOrder/detail.aspx?id=${ID}&idbrandasal=${IDBrandAsal}">Detail</a>
                    ${
                      Edit
                        ? ""
                        : `
                        <a
                          class="btn btn-warning"
                          target="_blank"
                          href="../RepairServicePointFinish?id=${ID}&idbrandasal=${IDBrandAsal}"
                        >
                          Finish
                        </a>
                        <button type="button" class="btn btn-danger" data-bs-toggle="modal" data-bs-target="#staticBackdrop" data-id="${ID}" data-id-brand-asal="${IDBrandAsal}">
                          Return / Cancel
                        </button>
                        `
                    }
                  </td>
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

      BUTTON_RETURN_CANCEL_SERVICE_POINT.on("click", function (e) {
        const id = $(e.currentTarget).attr("data-id");
        const brandAsal = $(e.currentTarget).attr("data-id-brand-asal");

        $.ajax({
          type: "POST",
          url: "./index.aspx/ReturnCancelRepair",
          contentType: "application/json; charset=utf-8",
          data: JSON.stringify({ IDDocRepair: id, IDBrandAsal: brandAsal }),
          success: function (data) {
            const response = data["d"];

            if (response === "success") {
              toastr.success("Berhasil return/cancel reparasi.");
            } else {
              toastr.error("Cancel gagal, status sudah berubah!");
            }
            onSearchFormDORepair(false);
            $("#staticBackdrop").modal("hide");
          },
          error: function (error) {
            console.error(error);
            toastr.error("Terjadi kesalahan saat return/cancel reparasi.");
          },
        });
      });

      $(document).on(
        "click",
        "#tbody-list-service-point tr button",
        function (e) {
          BUTTON_RETURN_CANCEL_SERVICE_POINT.attr(
            "data-id",
            $(this).attr("data-id")
          ).attr("data-id-brand-asal", $(this).attr("data-id-brand-asal"));
        }
      );
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
    });
  </script>
</asp:Content>
