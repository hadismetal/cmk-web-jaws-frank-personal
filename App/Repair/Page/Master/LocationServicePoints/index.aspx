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

  <div class="container border rounded p-5">
    <div class="d-flex justify-content-between align-items-center">
      <h1 style="border: none; padding: 0">List Lokasi Service Point</h1>
      <button
        class="btn btn-secondary btn-lg"
        data-bs-toggle="modal"
        data-bs-target="#modalAddServicePoint"
      >
        Tambah Service Point
      </button>
    </div>
    <div class="container-fluid">
      <table class="table table-hover">
        <thead>
          <tr>
            <th>No</th>
            <th>Store</th>
            <th>Province</th>
            <th>Brand</th>
            <th></th>
          </tr>
        </thead>
        <tbody id="tbody-service-point"></tbody>
      </table>
    </div>
  </div>

  <div
    class="modal"
    tabindex="-1"
    id="modalListDetailServicePoint"
    aria-labelledby="modalListDetailServicePoint"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-xl">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">List Service Point Detail</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <span id="loading-spinner">Loading...</span>
          <div
            class="rounded border py-3 px-5"
            id="content-modal-service-point"
          >
            <div class="row">
              <div class="col-9">
                <table
                  class="table table-borderless"
                  id="table-header-service-point"
                >
                  <thead>
                    <tr>
                      <th>Store</th>
                      <th>Province</th>
                      <th>Brand</th>
                    </tr>
                  </thead>
                  <tbody id="tbody-header-sevice-point"></tbody>
                </table>
              </div>
            </div>

            <h3 class="fw-bold text-md mt-3">Detail Service Point</h3>
            <table class="table table-hover" id="table-detail-service-point">
              <thead>
                <tr>
                  <th>No.</th>
                  <th>Store</th>
                  <th>Province</th>
                  <th>Brand</th>
                </tr>
              </thead>
              <tbody id="tbody-detail-sevice-point"></tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>

  <div
    class="modal"
    tabindex="-1"
    id="modalAddServicePoint"
    aria-labelledby="modalAddServicePoint"
    aria-hidden="true"
    data-bs-backdrop="static"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Tambah Service Point</h5>
        </div>
        <div class="modal-body">
          <div class="d-flex flex-column w-100">
            <label for="select-province" class="form-label fw-bold"
              >Provinsi</label
            >
            <select id="select-province"></select>
          </div>
          <div class="d-flex flex-column mt-4">
            <label for="select-location" class="form-label fw-bold"
              >Lokasi</label
            >
            <select id="select-location"></select>
          </div>
        </div>
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-secondary"
            data-bs-dismiss="modal"
            id="btn-close-service-point"
          >
            Close
          </button>
          <button
            type="button"
            class="btn btn-primary"
            id="btn-submit-service-point"
          >
            Save changes
          </button>
        </div>
      </div>
    </div>
  </div>

  <script type="text/javascript">
    const SELECT_LOCATION = $("#select-location");
    const SELECT_PROVINCE = $("#select-province");
    const BUTTON_SUBMIT = $("#btn-submit-service-point");
    const BUTTON_CLOSE = $("#btn-close-service-point");

    toastr.options = {
      closeButton: true,
      progressBar: true,
      positionClass: "toast-bottom-center",
      showDuration: "300",
      hideDuration: "1000",
      timeOut: "5000",
    };

    function getServicePoint() {
      $.ajax({
        type: "GET",
        url: "./index.aspx/GetMasterServicePoint",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];
          const table = $("#tbody-service-point");

          const html = response?.reduce((accumulator, calculator, index) => {
            const { Nama, Province, Brand, Kode, IDProvince } = calculator;

            return (accumulator += `
                    <tr>
                        <td>${index + 1}.</td>
                        <td>${Nama}</td>
                        <td>${Province}</td>
                        <td>${Brand}</td>
                        <td class="d-flex justify-content-center gap-3">
                            <button data-bs-toggle="modal" data-bs-target="#modalListDetailServicePoint" data-kode="${Kode}" data-id-province="${IDProvince}" data-nama="${Nama}"  data-province="${Province}" class="btn-view-detail btn btn-primary">Detail Mapping Lokasi</button>
                            <button data-kode="${Kode}" data-nama="${Nama}" data-province="${Province}" class="btn-add-detail btn btn-success">Tambah List</button>
                        </td>
                    </tr>
                `);
          }, "");

          table.empty().append(html);
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          console.log(data);
        },
      });
    }

    function createServicePoint() {
      const storeCode = SELECT_LOCATION.val();
      const idProvince = SELECT_PROVINCE.val();

      BUTTON_SUBMIT.attr("disabled", true);
      BUTTON_CLOSE.attr("disabled", true);

      $.ajax({
        type: "POST",
        url: `./index.aspx/CreateServicePoint`,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
          parameters: { Kode: `${storeCode}`, IDProvince: idProvince },
        }),
        success: function (data) {
          const message = data["d"];

          if (message === "") {
            getServicePoint();
            $("#modalAddServicePoint").modal("hide");
            toastr.success("Berhasil menambah store sebagai service point.");
            return;
          }

          toastr.error(message);
        },
        error: function (error) {
          console.error(error);
        },
        complete: function () {
          BUTTON_SUBMIT.attr("disabled", false);
          BUTTON_CLOSE.attr("disabled", false);
        },
      });
    }

    function generateTableRowHtml(
      args = { array: [], keys: [], isWithIndex: false }
    ) {
      const { array, keys, isWithIndex } = args;
      return array.reduce((accumulator, calculator, index) => {
        let string = "";

        keys.forEach((v, innerIndex) => {
          if (innerIndex === 0 && isWithIndex) {
            string += `<td>${index + 1}.</td>`;
          }

          string += `<td>${calculator[v]}</td>`;
        });

        accumulator += `<tr>${string}</tr>`;
        return accumulator;
      }, "");
    }

    function getDetailServicePoint(args = { kode: "", idProvince: 0 }) {
      $("#content-modal-service-point").addClass("d-none");
      $("#loading-spinner").removeClass("d-none");
      $.ajax({
        type: "GET",
        url: `./index.aspx/GetMasterServicePointDetail?Kode='${args.kode}'&IDProvince=${args.idProvince}`,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];
          const header = response["listServicePoint"];
          const detail = response["listServicePointDetail"];

          const keys = ["Nama", "Province", "Brand"];
          $("#tbody-header-sevice-point")
            .empty()
            .append(generateTableRowHtml({ array: header, keys }));

          $("#tbody-detail-sevice-point")
            .empty()
            .append(
              generateTableRowHtml({ array: detail, keys, isWithIndex: true })
            );
        },
        error: function (error) {
          console.error(error);
        },
        complete: function () {
          $("#content-modal-service-point").removeClass("d-none");
          $("#loading-spinner").addClass("d-none");
        },
      });
    }

    // Instantiate
    $(document).ready(function () {
      SELECT_LOCATION.select2({
        dropdownParent: $("#modalAddServicePoint"),
      });
      SELECT_PROVINCE.select2({
        dropdownParent: $("#modalAddServicePoint"),
      });
    });

    // Event Handler
    $(document).ready(function () {
      BUTTON_SUBMIT.on("click", () => createServicePoint());

      $(document).on(
        "click",
        "#tbody-service-point tr td button.btn-view-detail",
        function (e) {
          const clickedButton = $(e.currentTarget);
          const kode = clickedButton.attr("data-kode");
          const idProvince = clickedButton.attr("data-id-province");

          getDetailServicePoint({ kode, idProvince });
        }
      );
    });

    // Fetch Initial Data
    $(document).ready(function () {
      getServicePoint();
      $.ajax({
        type: "GET",
        url: "./index.aspx/GetLocation",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];
          const location = response["locations"];
          const province = response["provinces"];

          location.forEach((item, index) => {
            const { IDLokasi, TipeLokasi, NamaOutlet, KodeOutlet } = item;
            let isSelected = "";
            if (index === 0) isSelected = "selected";
            SELECT_LOCATION.append(
              `<option ${isSelected} data-id="${IDLokasi}" data-tipe-lokasi="${TipeLokasi}" data-nama="${NamaOutlet}" data-kode="${KodeOutlet}" value="${KodeOutlet}">${NamaOutlet}</option>`
            );
          });

          province.forEach((item, index) => {
            const { ID, Province } = item;

            let isSelected = "";
            if (index === 0) isSelected = "selected";
            SELECT_PROVINCE.append(
              `<option ${isSelected} data-id="${ID}" data-province="${Province}" value="${ID}">${Province}</option>`
            );
          });

          SELECT_LOCATION.trigger("change");
          SELECT_PROVINCE.trigger("change");
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          console.log(data);
        },
      });
    });
  </script>
</asp:Content>
