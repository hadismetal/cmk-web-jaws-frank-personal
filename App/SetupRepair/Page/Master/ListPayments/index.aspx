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
      <h1 style="border: none; padding: 0">List Jenis Pembayaran</h1>
      <button
        class="btn btn-secondary btn-lg"
        data-bs-toggle="modal"
        data-bs-target="#modalAddServicePoint"
      >
        Tambah Jenis Pembayaran
      </button>
    </div>
    <div class="container-fluid">
      <table class="table table-hover">
        <thead>
          <tr>
            <th>No</th>
            <th>Jenis Pembayaran</th>
            <th>Minimum Pembayaran</th>
            <th class="text-center">Status</th>
            <th class="text-center">Enable/Disable</th>
            <!-- <th>Maximum Pembayaran</th> -->
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
          <h5 class="modal-title">Tambah Jenis Bayar</h5>
        </div>
        <div class="modal-body">
          <div class="d-flex flex-column">
            <label for="select-province" class="form-label fw-bold"
              >Jenis Pembayaran</label
            >
            <select class="w-100" id="select-payment-list"></select>
          </div>
          <div class="d-flex flex-column">
            <label for="select-province" class="form-label fw-bold"
              >Minimum Pembayaran</label
            >
            <input
              type="text"
              class="form-control"
              id="input-minimum-payment"
              name="input-minimum-payment"
              data-nominal=""
              id="nominal"
              pattern="^\$\d{1,3}(,\d{3})*(\.\d+)?$"
              value=""
              data-type="currency"
              placeholder="Masukkan minimum nominal..."
            />
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

  <div
    class="modal"
    tabindex="-1"
    id="modalEditListPayment"
    aria-labelledby="modalEditListPayment"
    aria-hidden="true"
    data-bs-backdrop="static"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">
            Ubah Konfigurasi Pembayaran <span id="span-nama-pembayaran"></span>
          </h5>
        </div>
        <div class="modal-body">
          <div class="d-flex flex-column">
            <label for="select-province" class="form-label fw-bold"
              >Minimum Pembayaran</label
            >
            <input
              type="text"
              class="form-control"
              id="input-minimum-payment"
              name="input-minimum-payment"
              data-nominal=""
              id="nominal"
              pattern="^\$\d{1,3}(,\d{3})*(\.\d+)?$"
              value=""
              data-type="currency"
              placeholder="Masukkan minimum nominal..."
            />
          </div>
          <!-- <div class="d-flex flex-column mt-4">
            <label for="select-location" class="form-label fw-bold"
              >Maximum Pembayaran</label
            >
            <input
              type="text"
              class="form-control"
              id="input-maximum-payment"
              name="input-maximum-payment"
              data-nominal=""
              id="nominal"
              pattern="^\$\d{1,3}(,\d{3})*(\.\d+)?$"
              value=""
              data-type="currency"
              placeholder="Masukkan maximum nominal..."
            />
          </div> -->
        </div>
        <div class="modal-footer">
          <button
            type="button"
            class="btn btn-secondary btn-close-form"
            data-bs-dismiss="modal"
          >
            Close
          </button>
          <button
            type="button"
            class="btn btn-primary btn-submit-form"
            data-type="edit"
            data-id=""
          >
            Save changes
          </button>
        </div>
      </div>
    </div>
  </div>

  <script type="text/javascript">
    const SELECT_PAYMENT_LIST = $("#select-payment-list");
    const BUTTON_SUBMIT = $(".btn-submit-form");
    const BUTTON_CLOSE = $(".btn-close-form");
    const INPUT_MINIMUM_PAYMENT = $("#input-minimum-payment");
    const INPUT_MINIMUM_PAYMENT_CREATE = $("#input-minimum-payment-create");
    // const INPUT_MAXIMUM_PAYMENT = $("#input-maximum-payment");

    function formatNumberToIDR(number) {
      const IDR = new Intl.NumberFormat("id-ID", {
        style: "currency",
        currency: "IDR",
      }).format(number);
      return IDR;
    }

    function formatNumber(n) {
      // Remove non-digit characters
      let cleaned = n.replace(/\D/g, "");

      // Remove leading zeros
      cleaned = cleaned.replace(/^0+/, "");

      // If the cleaned number is empty (e.g., input was just zeros), return "0"
      if (cleaned === "") return "0";

      // Insert periods every three digits
      let formatted = cleaned.replace(/\B(?=(\d{3})+(?!\d))/g, ".");

      return formatted;
    }

    function formatCurrency(input, blur, max_value) {
      // appends $ to value, validates decimal side
      // and puts cursor back in right position.

      // get input value
      var input_val = input.val();

      // don't validate empty input
      if (input_val === "") {
        return;
      }

      // original length
      var original_len = input_val.length;

      // initial caret position
      var caret_pos = input.prop("selectionStart");

      // no decimal entered
      // add commas to number
      // remove all non-digits
      input_val = formatNumber(input_val);
      input_val = "Rp. " + input_val;

      // final formatting
      if (blur === "blur") {
        const number = input_val.substring(4).replaceAll(".", "");

        if (parseInt(number) > max_value) {
          input.attr("data-nominal", max_value);
          input.val("Rp. " + formatNumber(max_value.toString()));
          return;
        }

        input.attr("data-nominal", number);
      }

      // send updated string to input
      input.val(input_val);

      // put caret back in the right position
      var updated_len = input_val.length;
      caret_pos = updated_len - original_len + caret_pos;
      input[0].setSelectionRange(caret_pos, caret_pos);
    }

    toastr.options = {
      closeButton: true,
      progressBar: true,
      positionClass: "toast-bottom-center",
      showDuration: "300",
      hideDuration: "1000",
      timeOut: "5000",
    };

    function getListPaymentMaster() {
      $.ajax({
        type: "GET",
        url: "./index.aspx/GetPaymentListMaster",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];
          response.forEach(({ ID, Nama }) =>
            SELECT_PAYMENT_LIST.append(
              `<option value="${ID}" data-id="${ID}" data-nama="${Nama}">${Nama}</option>`
            )
          );

          SELECT_PAYMENT_LIST.trigger("change");
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          console.log(data);
        },
      });
    }

    function getListPayment() {
      $.ajax({
        type: "GET",
        url: "./index.aspx/GetMasterPaymentList",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];
          const table = $("#tbody-service-point");

          const html = response?.reduce((accumulator, calculator, index) => {
            const {
              Nama,
              ID,
              IDPaymentTypes,
              MinimumPayment,
              MaximumPayment,
              IsDisabled,
            } = calculator;

            //   <td>${formatNumberToIDR(MaximumPayment)}</td>
            return (accumulator += `
                    <tr>
                        <td>${index + 1}.</td>
                        <td>${Nama}</td>
                        <td>${formatNumberToIDR(MinimumPayment)}</td>
                        <td class="text-center">${
                          IsDisabled
                            ? "<span class='badge bg-danger'>Inactive</span>"
                            : "<span class='badge bg-success'>Active</span>"
                        }</td>
                        <td>
                            <div class="d-flex justify-content-center gap-3">
                                <div class="form-check form-switch">
                                    <input class="form-check-input" type="checkbox" ${
                                      !IsDisabled ? "checked" : ""
                                    }
                                    data-id="${ID}"
                                    data-id-payment="${IDPaymentTypes}"
                                    data-minimum-payment="${MinimumPayment}"
                                    data-maximum-payment="${MaximumPayment}"
                                    />
                                </div>
                            </div>
                        </td>
                        <td>
                            <div class="d-flex justify-content-center gap-3">
                                <button data-id="${ID}" data-nama="${Nama}" data-bs-toggle="modal" data-bs-target="#modalEditListPayment" data-id-payment="${IDPaymentTypes}" data-minimum-payment="${MinimumPayment}" data-maximum-payment="${MaximumPayment}" class="btn-add-detail btn btn-primary">Konfigurasi</button>
                            </div>
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
      const payment = SELECT_PAYMENT_LIST.val();
      const minimumNominalValue =
        INPUT_MINIMUM_PAYMENT_CREATE.attr("data-nominal");
      BUTTON_SUBMIT.attr("disabled", true);
      BUTTON_CLOSE.attr("disabled", true);

      $.ajax({
        type: "POST",
        url: `./index.aspx/CreateMasterPayment`,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
          parameters: {
            IDPaymentTypes: payment,
            MinimumPayment: minimumNominalValue,
            IsDisabled: 0,
            MaximumPayment: 0,
          },
        }),
        success: function (data) {
          const message = data["d"];

          if (message === "") {
            getListPayment();
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

    function updateConfiguration(
      args = { id, isFromCheckbox: false, isDisabled: 0 }
    ) {
      const minimumNominalValue = INPUT_MINIMUM_PAYMENT.attr("data-nominal");

      const minimumPayment = args.isFromCheckbox
        ? 0
        : parseFloat(minimumNominalValue);
      const maximumPayment = args.isFromCheckbox
        ? 0
        : parseFloat(minimumNominalValue);

      $.ajax({
        type: "POST",
        url: `./index.aspx/UpdateConfigurationPayment`,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
          parameters: {
            ID: args.id,
            Minimum: minimumPayment,
            Maximum: maximumPayment,
            Disabled: args.isDisabled,
          },
        }),
        success: function (data) {
          const message = data["d"];

          getListPayment();
          $("#modalEditListPayment").modal("hide");

          if (!message) {
            toastr.success("Berhasil mengubah konfigurasi pembayaran");
            return;
          }

          toastr.success(message);
        },
        error: function (error) {
          toastr.error("Terjadi kesalahan, silahkan hubungi tim IT!");
          console.error(error);
        },
        complete: function () {
          BUTTON_SUBMIT.attr("disabled", false);
          BUTTON_CLOSE.attr("disabled", false);
        },
      });
    }

    // Instantiate
    $(document).ready(function () {
      SELECT_PAYMENT_LIST.select2();
    });

    // Event Handler
    $(document).ready(function () {
      $(document).on("change", ".form-check-input", function (e) {
        const inputElement = $(e.currentTarget);
        const id = inputElement.attr("data-id");
        const isDisabled = !inputElement.is(":checked") ? 1 : 0;
        updateConfiguration({ id, isFromCheckbox: true, isDisabled });
      });

      $(document).on("click", ".btn-add-detail", function (e) {
        const buttonElement = $(e.currentTarget);
        const id = buttonElement.attr("data-id");
        const minimumValue = buttonElement.attr("data-minimum-payment");
        // const maximumValue = buttonElement.attr("data-maximum-payment");

        const formatMinimumValue = formatNumber(minimumValue);
        // const formatMaximumValue = formatNumber(maximumValue);

        const formatMinimumValueIDR = formatNumberToIDR(minimumValue);
        // const formatMaximumValueIDR = formatNumberToIDR(maximumValue);

        const buttonEdit = BUTTON_SUBMIT.filter(function () {
          return (
            typeof $(this).attr("data-id") !== "undefined" &&
            $(this).attr("data-id") !== false
          );
        });

        buttonEdit.attr("data-id", id);

        INPUT_MINIMUM_PAYMENT.val("Rp. " + formatMinimumValue).attr(
          "data-nominal",
          formatMinimumValueIDR
        );

        // INPUT_MAXIMUM_PAYMENT.val("Rp. " + formatMaximumValue).attr(
        //   "data-nominal",
        //   formatMaximumValueIDR
        // );
      });

      BUTTON_SUBMIT.on("click", (e) => {
        const buttonElement = $(e.currentTarget);

        const buttonFormType = buttonElement.attr("data-type");

        if (buttonFormType === "edit") {
          const id = buttonElement.attr("data-id");
          updateConfiguration({ id });
          return;
        }
      });

      $("input[data-type='currency']").on({
        keyup: function () {
          formatCurrency($(this));
        },
        blur: function () {
          formatCurrency($(this), "blur");
        },
      });
    });

    // Fetch Initial Data
    $(document).ready(function () {
      getListPayment();
      getListPaymentMaster();
    });
  </script>
</asp:Content>
