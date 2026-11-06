<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="MasterPage.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="RepairDeliveryOrder_Index" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
  Form Register Penerimaan
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
  Reparasi Dari Pabrik
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
            List DO Finish Repair BRJ
          </h1>
          <div class="py-3">
            <label for="select-location-from" class="form-label fw-bold"
              >Lokasi Outlet Asal</label
            >
            <select class="w-100" id="select-location-from"></select>
          </div>
          <div class="py-3">
            <label for="input-tgl-awal" class="form-label fw-bold"
              >Tanggal Awal</label
            >
            <input
              class="form-control"
              type="date"
              name="input-tgl-awal"
              id="input-tgl-awal"
            />
          </div>
          <div class="py-3">
            <label for="input-tgl-akhir" class="form-label fw-bold"
              >Tanggal Akhir</label
            >
            <input
              type="date"
              class="form-control"
              name="input-tgl-akhir"
              id="input-tgl-akhir"
            />
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
                <th>Nomor DO</th>
                <th class="text-right">Berat Awal</th>
                <th class="text-right">Berat Akhir</th>                
                <th>Tanggal</th>
                <th class="text-center">Action</th>
              </tr>
            </thead>
            <tbody id="tbody-list-reparasi"></tbody>
          </table>
        </div>

        <div class="d-flex justify-content-end align-items-center mt-3">
          <button
            type="button"
            class="col-3 btn btn-primary btn-lg"
            id="btn-modal-confirmation"
          >
            Submit DO KRM
          </button>
        </div>
      </div>
    </div>
  </div>

  <!-- Modal -->
  <div
    class="modal fade"
    id="exampleModal"
    tabindex="-1"
    aria-labelledby="exampleModalLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog modal-lg">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title fw-bold" id="exampleModalLabel">
            Submit Form DO KRM
          </h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <div class="row">
            <div class="py-3">
              <label for="input-kurir" class="fw-bold">Nama Kurir</label>
              <input
                type="text"
                id="input-kurir"
                input="input-kurir"
                class="form-control"
              />
            </div>
            <div class="py-3">
              <label for="textarea-keterangan-do" class="fw-bold"
                >Keterangan</label
              >
              <textarea
                name="textarea-keterangan-do"
                id="textarea-keterangan-do"
                class="form-control"
                rows="5"
              ></textarea>
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-secondary"
            data-bs-dismiss="modal"
          >
            Close
          </button>
          <button type="button" class="btn btn-primary" id="btn-submit-form">
            Save changes
          </button>
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
    let BUTTON_MODAL_CONFIRMATION = $("#btn-modal-confirmation");
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
    const MODAL = $("#exampleModal");

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
      const tableCheckbox = $('[id*="checkbox_table"]:not([disabled])');
      const isCheckAll = $(e.currentTarget).is(":checked");
      tableCheckbox.each((idx, elem) => $(elem).prop("checked", isCheckAll));
    }

    function onConfirmationModalSubmit(e) {
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
        return false;
      }

      MODAL.modal("show");
    }

    function onSubmitModal(e) {
      let result = [];

      $("table tbody tr").each(function (index) {
        $(this)
          .find("input")
          .each(function (index) {
            const isChecked = $(this).is(":checked");

            if (!isChecked) return;
            const element = $(this);
            result.push({
              NoIndex: result.length + 1,
              DataString: element.attr("data-do"),
            });
          });
      });

      if (INPUT_KURIR.val() == "") {
        toastr.error("Kurir tidak boleh kosong!");
        return;
      }

      const objectToPush = {
        Kurir: INPUT_KURIR.val(),
        Keterangan: TEXTAREA_KETERANGAN.val(),
        DO_Detail: result,
      };

      BUTTON_SEARCH_DO_FORM.prop("disabled", true).text("Loading...");
      BUTTON_SUBMIT_DO_FORM.prop("disabled", true).text("Loading...");

      $.ajax({
        url: "./index.aspx/PostSendDeliveryOrderRepair",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ args: objectToPush }),
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

    function getStartDateAndCurrentDate() {
      let currDate = new Date();
      const offset = currDate.getTimezoneOffset();
      currDate = new Date(currDate.getTime() - offset * 60 * 1000);
      const currDateISO = currDate.toISOString().split("T")[0];
      const splittedDate = currDateISO.split("-");
      const startDateISO = `${splittedDate[0]}-${splittedDate[1]}-01`;

      return { currDateISO, startDateISO };
    }

    function onSearchFormDORepair(isSubmitting = false) {
      const parameters = {
        tglStart: `'${INPUT_TGL_AWAL.val()}'`,
        tglEnd: `'${INPUT_TGL_AKHIR.val()}'`,
        IDLokasi: SELECT_LOCATION_FROM.val()
      };

      BUTTON_SEARCH_DO_FORM.prop("disabled", true).text("Loading...");

      $.ajax({
        url: "./index.aspx/GetSendDeliveryOrderRepairFactory",
        contentType: "application/json; charset=utf-8",
        type: "GET",
        data: parameters,
        success: function (data) {
          DATA_LIST_REPARASI = data["d"];          

          const stringAdditional = DATA_LIST_REPARASI.reduce(
            (accumulator, calculator, index) => {
              const {
                BeratAkhir,
                BeratAwal,
                IDHeaderReparasi,
                NomorDO,   
                Status,             
                Tanggal,
                NomorParcelArray,
              } = calculator;
              
              if (Status == 1) return accumulator;

              const arrayOfData = JSON.parse(NomorParcelArray);
              const errorMessage = arrayOfData.reduce(
                (accumulator, calculator) => {
                  return (accumulator += `<div class='text-md fw-bold'> - ${calculator.NomorRepair} : ${calculator.Parcel}</div>`);
                },
                ``
              );
              accumulator += `
                <tr>
                  <td>
                    <div class="form-check">
                      <input ${
                        arrayOfData.length !== 0 ? "disabled" : ""
                      } class="form-check-input" data-do="${NomorDO}" data-id="${IDHeaderReparasi}" type="checkbox" id="checkbox_table_${IDHeaderReparasi}}" />
                    </div>  
                  </td>
                  <td>
                    ${NomorDO} ${
                arrayOfData.length === 0
                  ? ""
                  : `
                  <i class="icon-warning-sign text-warning" style="cursor: pointer;" data-bs-container="body" data-bs-html="true" data-bs-custom-class="popover" data-bs-toggle="popover" data-bs-placement="top" 
                  data-bs-content="
                    <div>
                      <div class='text-md'>Terdapat parcel yang belum di pricing/costing.</div>
                      ${errorMessage}
                    </div>
                  "
                  >
                  </i>`
              }
                  </td>
                  <td class="text-right">
                    ${BeratAwal}
                  </td>
                  <td class="text-right">
                    ${BeratAkhir}
                  </td>
                  <td>
                    ${Tanggal}
                  </td>
                  <td>
                    <div class="d-flex justify-content-center align-items-center">
                      <a href="../RepairDeliveryOrderFactory_Finish?NomorDO=${NomorDO}" target="_blank" class="btn btn-success btn-lg">Detail</a>
                    </div>
                  </td>
                </tr>
              `;

              return accumulator;
            },
            ""
          );

          if (isSubmitting) MODAL.modal("hide");

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

    // Initializes while document is ready
    $(document).ready(function () {
      SELECT_LOCATION_FROM.select2();
      SELECT_LOCATION_TO.select2();
      SELECT_LOKASI_REPARASI.select2();
      
      const dates = getStartDateAndCurrentDate();
      INPUT_TGL_AWAL.val(dates.startDateISO);
      INPUT_TGL_AKHIR.val(dates.currDateISO);

      getLoc(INPUT_TGL_AWAL.val(),INPUT_TGL_AKHIR.val());      
    });

    // event handler
    $(document).ready(function () {
      $(document).on("click", "[data-bs-toggle='popover']", function (e) {
        // Initialize the popover if it hasn't been initialized yet
        if (!$(this).data("bs.popover")) {
          $(this).popover();
        }

        // Toggle the popover visibility
        $(this).popover("toggle");
      });

      // Close the popover when clicking outside of it
      $(document).on("click", function (e) {
        // Check if the target is not the popover trigger or the popover itself
        if (
          !$(e.target).closest('[data-bs-toggle="popover"], .popover').length
        ) {
          $('[data-bs-toggle="popover"]').popover("hide");
        }
      });

      CHECKBOX_ALL_TABLE.on("click", (e) => checkAll(e));
      BUTTON_MODAL_CONFIRMATION.on("click", (e) =>
        onConfirmationModalSubmit(e)
      );
      BUTTON_SUBMIT_DO_FORM.on("click", (e) => onSubmitModal(e));
      BUTTON_SEARCH_DO_FORM.on("click", () => onSearchFormDORepair());
    });

    $(document).on("change", "#input-tgl-awal,#input-tgl-akhir", (e) => {
        const element = $(e.currentTarget);
        const id = element.attr("id");
        console.log(id)
        var tglAwal = INPUT_TGL_AWAL.val();
        var tglAkhir = INPUT_TGL_AKHIR.val();
        element.each((index,e)=>{
          const date = new Date(e.value);
          const month = date.getMonth() + 1; // Months are zero-based in JavaScript
          const year = date.getFullYear();
          
          const startDate = e.value;
          const endDate = e.value;        
          
          id == "input-tgl-awal" ? tglAwal = startDate :tglAkhir = endDate;
        })
        getLoc(tglAwal,tglAkhir);        
    });

    function getLoc(INPUT_TGL_AWAL,INPUT_TGL_AKHIR){
      const parameters = {
        tglStart: `'${INPUT_TGL_AWAL}'`,
        tglEnd: `'${INPUT_TGL_AKHIR}'`,
      };
      $.ajax({
        type: "GET",
        url: "./index.aspx/GetLocPLURepair",
        data : parameters,
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          console.log(data)
          DATA_LOCATION = data["d"];
          SELECT_LOCATION_FROM.empty(); // Clear existing options
          const select2Location = SELECT_LOCATION_FROM.select2();
          const mapLocation = DATA_LOCATION.forEach(
            (
              {
                IDLokasi,
                NamaOutlet,                
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
  </script>
</asp:Content>
