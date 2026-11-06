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

  <div class="container-fluid border rounded p-5">
    <h1 style="border-bottom: none" class="m-0 p-0 fw-bold">List Reparasi</h1>
    <form class="row g-4 py-3" id="form-data">
      <div class="col">
        <label for="input-tgl-awal" class="form-label fw-bold"
          >Tanggal Awal</label
        >
        <input class="form-control" id="input-tgl-awal" type="date" />
      </div>
      <div class="col">
        <label for="input-tgl-hingga" class="form-label fw-bold"
          >Tanggal Hingga</label
        >
        <input class="form-control" id="input-tgl-hingga" type="date" />
      </div>
      <div class="col">
        <label for="select-location" class="form-label fw-bold">Lokasi</label>
        <select class="w-100" id="select-location"></select>
      </div>
      <div class="col">
        <label for="select-lokasi-reparasi" class="form-label fw-bold"
          >Lokasi Reparasi</label
        >
        <select class="w-100" id="select-lokasi-reparasi"></select>
      </div>
      <div class="col">
        <label for="input-search-product" class="form-label fw-bold">PLU</label>
        <input type="text" class="form-control" id="input-search-product" />
      </div>
      <button
        class="btn btn-lg btn-primary align-self-end"
        style="width: max-content"
        id="btn-search-report"
      >
        <i class="icon-search"></i>
      </button>
    </form>

    <table class="table table-hover mt-5">
      <thead>
        <tr>
          <th>No</th>
          <th>No. Repair</th>
          <th>PLU</th>
          <th>No. Invoice/SPK</th>
          <th>Customer</th>
          <th>Sales</th>
          <th>Status</th>
          <th></th>
        </tr>
      </thead>
      <tbody id="tbody-report-repair"></tbody>
    </table>
  </div>

  <div
    class="modal fade"
    id="modalDetail"
    tabindex="-1"
    aria-labelledby="modalDetailLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="modalDetailLabel">
            <b>Approve Free Reparasi</b>
          </h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body" id="modal-body">
          Apakah anda yakin ingin approve pembayaran reparasi dengan nomor
          <b id="nomor-repair"></b> ini?
        </div>
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-secondary"
            data-bs-dismiss="modal"
            id="btn-cancel-repair"
          >
            Batal
          </button>
          <button type="button" class="btn btn-primary" id="btn-approve-repair">
            Approve
          </button>
        </div>
      </div>
    </div>
  </div>

  <script type="text/javascript">
    let TBODY_REPORT_REPAIR = $("#tbody-report-repair");
    let DATA_LOCATION = [];
    let DATA_LOCATION_REPARASI = [];
    let BUTTON_SEARCH_REPORT = $("#btn-search-report");
    const SELECT_LOCATION = $("#select-location");
    const SELECT_LOKASI_REPARASI = $("#select-lokasi-reparasi");
    const SELECT_STATUS_REPARASI = $("#select-status-reparasi");
    const INPUT_SEARCH_PRODUCT = $("#input-search-product");
    const FORM_DATA = $("#form-data");
    const INPUT_TGL_AWAL = $("#input-tgl-awal");
    const INPUT_TGL_HINGGA = $("#input-tgl-hingga");
    const BUTTON_APPROVE_REPAIR = $("#btn-approve-repair");
    const BUTTON_CANCEL_REPAIR = $("#btn-cancel-repair");

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

    function onSearchFormRepair() {
      const select2LocationElement = $(
        SELECT_LOCATION.select2("data")["element"]
      );

      BUTTON_SEARCH_REPORT.attr("disabled", true).html("Loading...");

      $.ajax({
        type: "GET",
        url: "./index.aspx/GetDocRepair",
        data: {
          PLU: `"${INPUT_SEARCH_PRODUCT.val()}"`,
          TglStart: `"${INPUT_TGL_AWAL.val()}"`,
          TglEnd: `"${INPUT_TGL_HINGGA.val()}"`,
          IDBrand: 0,
          IDLocation: select2LocationElement.attr("data-id-lokasi"),
          TipeLocation: select2LocationElement.attr("data-tipe-lokasi"),
          TipeProduct: "''",
          IDLocReparasi: SELECT_LOKASI_REPARASI.val(),
          Status: 12,
        },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];

          // ${
          //             IDStatus === 7
          //               ? `<li class="p-0 m-0"><hr class="dropdown-divider p-0 m-0"></li>
          //           <li><a class="dropdown-item" target="_blank" href="../../Create/RepairOrderPayment?id=${IDDocRepair}">Bayar</a></li>
          //           `
          //               : ``
          //           }

          const stringAdditional = response.reduce(
            (accumulator, calculator) => {
              const {
                No,
                NamaCustomer,
                NamaSales,
                NomorDocRepair,
                NomorInvoice,
                PLU,
                IDDocRepairPivot,
                IDDocRepair,
                IDStatus,
                Status,
              } = calculator;

              accumulator += `
              <tr>
                <td>${No}</td>
                <td>${NomorDocRepair}</td>
                <td>${PLU}</td>
                <td>${NomorInvoice}</td>
                <td>${NamaCustomer}</td>
                <td>${NamaSales}</td>
                <td>${Status}</td>
                <td>
                <td><button class="btn btn-success btn-lg" data-nomor-repair="${NomorDocRepair}" data-id-pivot="${IDDocRepairPivot}" data-bs-toggle="modal" data-bs-target="#modalDetail">Approve</button></td>
              </tr>
            `;

              return accumulator;
            },
            ""
          );

          TBODY_REPORT_REPAIR.empty();
          TBODY_REPORT_REPAIR.append(stringAdditional);
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          BUTTON_SEARCH_REPORT.attr("disabled", false).html(
            " <i class='icon-search'></i>"
          );
        },
      });
    }

    // Initializes while document is ready
    $(document).ready(function () {
      let currDate = new Date();
      const offset = currDate.getTimezoneOffset();
      currDate = new Date(currDate.getTime() - offset * 60 * 1000);
      const currDateISO = currDate.toISOString().split("T")[0];
      const splittedDate = currDateISO.split("-");
      const startDateISO = `${splittedDate[0]}-${splittedDate[1]}-01`;

      INPUT_TGL_AWAL.val(startDateISO);
      INPUT_TGL_HINGGA.val(currDateISO);

      SELECT_LOCATION.select2();
      SELECT_LOKASI_REPARASI.select2();
      SELECT_STATUS_REPARASI.select2();
    });

    // event handler
    $(document).ready(function () {
      FORM_DATA.on("submit", (e) => {
        e.preventDefault();
        onSearchFormRepair();
      });

      $("#tbody-report-repair").on("click", "button", (e) => {
        const element = $(e.currentTarget);
        $("#nomor-repair").html(element.attr("data-nomor-repair"));
        BUTTON_APPROVE_REPAIR.attr(
          "data-id-pivot",
          element.attr("data-id-pivot")
        );
      });

      BUTTON_APPROVE_REPAIR.on("click", (e) => {
        const element = $(e.currentTarget);

        element.attr("disabled", true).html("Loading...");
        BUTTON_CANCEL_REPAIR.attr("disabled", true).html("Loading...");

        $.ajax({
          type: "POST",
          url: "./index.aspx/ApprovePaymentRepair",
          contentType: "application/json; charset=utf-8",
          data: JSON.stringify({
            IDDocRepairPivot: element.attr("data-id-pivot"),
          }),
          success: function () {
            toastr.success("Berhasil approve pembayaran");
          },
          error: function (error) {
            console.error(error);
          },
          complete: function (data) {
            onSearchFormRepair();
            element.attr("disabled", false).html("Approve");
            BUTTON_CANCEL_REPAIR.attr("disabled", false).html("Batal");
            $("#modalDetail").modal("hide");
          },
        });
      });
    });

    // Fetch Initial Data
    $(document).ready(function () {
      $.ajax({
        type: "GET",
        url: "./index.aspx/GetStatusReparasi",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];

          response.forEach(({ ID, Nama }) => {
            const option = new Option(Nama, ID, false, false);
            SELECT_STATUS_REPARASI.append(option);
          });

          SELECT_STATUS_REPARASI.trigger("change");
        },
        error: function (error) {
          console.error(error);
        },
      });

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

      $.ajax({
        type: "GET",
        url: "./index.aspx/GetLocReparasi",
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
