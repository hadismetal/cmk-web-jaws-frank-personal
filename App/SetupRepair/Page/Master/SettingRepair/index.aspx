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

  <div class="d-flex flex-column gap-5">
    <div class="container border rounded p-5">
      <div class="d-flex justify-content-between align-items-center">
        <h1 style="border: none; padding: 0">Setting</h1>
      </div>
      <div class="container-fluid">
        <table class="table table-hover">
          <thead>
            <tr>
              <th>No</th>
              <th>Jenis Setting</th>
              <th>Nilai</th>
              <th>Type</th>
              <th></th>
            </tr>
          </thead>
          <tbody id="tbody-settings"></tbody>
        </table>
      </div>
    </div>

    <div class="container border rounded p-5">
      <div class="d-flex justify-content-between align-items-center">
        <h1 style="border: none; padding: 0">Mark Up</h1>
      </div>
      <table class="table table-hover">
        <thead>
          <tr>
            <th>No</th>
            <th>Markup R</th>
            <th>Markup Harga Jual</th>
            <th>Efektif</th>
            <th>Type</th>
          </tr>
        </thead>
        <tbody id="tbody-markups"></tbody>
      </table>
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
          <h5 class="modal-title">Ubah Konfigurasi Setting</h5>
        </div>
        <div class="modal-body">
          <div class="d-flex flex-column">
            <label for="select-province" class="form-label fw-bold"
              >Nilai</label
            >
            <input
              type="number"
              class="form-control"
              id="input-nilai"
              name="input-nilai"
              data-nominal=""
              step="0.01"
              id="nominal"
              pattern="^\$\d{1,3}(,\d{3})*(\.\d+)?$"
              value=""
              data-type="currency"
              placeholder="Masukkan nilai..."
            />
          </div>
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

  <div
    class="modal"
    tabindex="-1"
    id="modalEditMarkUp"
    aria-labelledby="modalEditMarkUp"
    aria-hidden="true"
    data-bs-backdrop="static"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Ubah Konfigurasi Mark Up</h5>
        </div>
        <div class="modal-body">
          <div class="d-flex flex-column">
            <label for="select-province" class="form-label fw-bold"
              >Mark Up R</label
            >
            <input
              type="number"
              class="form-control"
              id="input-markup-r"
              name="input-markup-r"
              data-nominal=""
              step="0.01"
              id="nominal"
              pattern="^\$\d{1,3}(,\d{3})*(\.\d+)?$"
              value=""
              data-type="currency"
              placeholder="Masukkan nilai mark up R..."
            />
          </div>
          <div class="d-flex flex-column">
            <label for="select-province" class="form-label fw-bold"
              >Mark Up Jual</label
            >
            <input
              type="number"
              class="form-control"
              id="input-markup-jual"
              name="input-markup-jual"
              data-nominal=""
              step="0.01"
              id="nominal"
              pattern="^\$\d{1,3}(,\d{3})*(\.\d+)?$"
              value=""
              data-type="currency"
              placeholder="Masukkan nilai mark up Jual..."
            />
          </div>
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
    const SELECT_LOCATION = $("#select-location");
    const SELECT_PROVINCE = $("#select-province");
    const BUTTON_SUBMIT = $(".btn-submit-form");
    const BUTTON_CLOSE = $(".btn-close-form");
    const INPUT_NILAI = $("#input-nilai");
    const INPUT_MARKUP_R = $("#input-markup-r");
    const INPUT_MARKUP_JUAL = $("#input-markup-jual");

    function onInputNumber(e) {
      const element = $(e.currentTarget);
      const value = parseFloat(element.val());

      return function checkInputNumber(isWithMinMax, isAllowNegativeValue) {
        if (!isAllowNegativeValue && !isWithMinMax) {
          const makeItAbsolute = value < 0 ? Math.abs(value) : value;
          if (value > 0) return;
          element.val(makeItAbsolute).trigger("change");
        }

        if (isWithMinMax) {
          const min = parseFloat(element.attr("min"));
          const max = parseFloat(element.attr("max"));

          if (min || max) {
            const makeItMaxValue =
              value < min ? min : value > max ? max : value;

            if (makeItMaxValue === value) return;
            element.val(makeItMaxValue).trigger("change");
          }
        }
      };
    }

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

    function getListSettingRepair() {
      $.ajax({
        type: "GET",
        url: "./index.aspx/GetMasterSetting",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];

          const settings = response["settings"];
          const markups = response["markups"];

          const tableSetting = $("#tbody-settings");
          const tableMarkup = $("#tbody-markups");

          const htmlSetting = settings?.reduce(
            (accumulator, calculator, index) => {
              const { ID, Nama, Nilai, Type } = calculator;

              //   <td>${formatNumberToIDR(MaximumPayment)}</td>
              return (accumulator += `
                    <tr>
                        <td>${index + 1}.</td>
                        <td>${Nama}</td>
                        <td>${Nilai}</td>
                        <td>${Type}</td>
                        <td>
                            <div class="d-flex justify-content-center gap-3">
                                <button 
                                    data-id="${ID}" 
                                    data-nama="${Nama}" 
                                    data-nilai="${Nilai}" 
                                    data-type="${Type}" 
                                    data-bs-toggle="modal" 
                                    data-bs-target="#modalEditListPayment" 
                                    class="btn-add-detail btn btn-primary"
                                >
                                    Konfigurasi
                                </button>
                            </div>
                        </td>
                    </tr>
                `);
            },
            ""
          );

          const htmlMarkup = markups?.reduce(
            (accumulator, calculator, index) => {
              const { ID, MarkUpR, MarkUpHargaJual, Efektif, Type } =
                calculator;

              //   <td>${formatNumberToIDR(MaximumPayment)}</td>
              return (accumulator += `
                    <tr>
                        <td>${index + 1}.</td>
                        <td>${MarkUpR}</td>
                        <td>${MarkUpHargaJual}</td>
                        <td>${Efektif}</td>
                        <td>${Type}</td>
                        <td>
                            <div class="d-flex justify-content-center gap-3">
                                <button 
                                    data-id="${ID}" 
                                    data-markup-r="${MarkUpR}" 
                                    data-markup-harga-jual="${MarkUpHargaJual}" 
                                    data-type="${Type}"
                                    data-bs-toggle="modal" 
                                    data-bs-target="#modalEditMarkUp" 
                                    class="btn-add-detail btn btn-primary"
                                >
                                    Konfigurasi
                                </button>
                            </div>
                        </td>
                    </tr>
                `);
            },
            ""
          );

          tableSetting.empty().append(htmlSetting);
          tableMarkup.empty().append(htmlMarkup);
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          console.log(data);
        },
      });
    }

    function updateConfigurationSetting() {
      const value = INPUT_NILAI.val();

      $.ajax({
        type: "POST",
        url: `./index.aspx/UpdateConfigurationSetting`,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
          parameters: {
            Tipe: "Setting",
            Nilai: value,
          },
        }),
        success: function (data) {
          const message = data["d"];

          getListSettingRepair();
          $("#modalEditListPayment").modal("hide");
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

    function updateConfigurationMarkUp() {
      const markupR = INPUT_MARKUP_R.val();
      const markupJual = INPUT_MARKUP_JUAL.val();

      $.ajax({
        type: "POST",
        url: `./index.aspx/updateConfigurationMarkUp`,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({
          parameters: {
            Tipe: "Markup",
            MarkUpR: markupR,
            MarkUpJual: markupJual,
          },
        }),
        success: function (data) {
          const message = data["d"];

          getListSettingRepair();
          $("#modalEditMarkUp").modal("hide");
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
      SELECT_LOCATION.select2();
      SELECT_PROVINCE.select2();
    });

    // Event Handler
    $(document).ready(function () {
      $(document).on("change", ".form-check-input", function (e) {
        const inputElement = $(e.currentTarget);
        const id = inputElement.attr("data-id");
        const isDisabled = !inputElement.is(":checked") ? 1 : 0;
        updateConfigurationSetting({ id, isFromCheckbox: true, isDisabled });
      });

      $(document).on("click", ".btn-add-detail", function (e) {
        const id = $(this).attr("data-id");

        BUTTON_SUBMIT.attr("data-id", id);
        BUTTON_SUBMIT.attr("data-type", $(this).attr("data-type"));

        if ($(this).attr("data-type") === "Setting") {
          const value = $(this).attr("data-nilai");
          INPUT_NILAI.val(value).attr("data-nominal", value);
          return;
        }

        const markupR = $(this).attr("data-markup-r");
        const markupJual = $(this).attr("data-markup-harga-jual");

        INPUT_MARKUP_R.val(markupR).attr("data-nominal", markupR);
        INPUT_MARKUP_JUAL.val(markupJual).attr("data-nominal", markupJual);
      });

      BUTTON_SUBMIT.on("click", (e) => {
        const buttonElement = $(e.currentTarget);
        const buttonFormType = buttonElement.attr("data-type");
        const id = buttonElement.attr("data-id");

        if (buttonFormType === "Setting") {
          updateConfigurationSetting({ id });
          return;
        }

        updateConfigurationMarkUp({ id });
      });

      $(document).on("change", 'input[type="number"]', (e) => {
        const vallidateInput = onInputNumber(e);
        vallidateInput(false, false);
      });
    });

    // Fetch Initial Data
    $(document).ready(function () {
      getListSettingRepair();
    });
  </script>
</asp:Content>
