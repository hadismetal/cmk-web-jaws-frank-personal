import {
  onGeneratePLU,
  updateOrGeneratePLU,
  updateGeneratedPLU,
  onDeleteGeneratedPLU,
  postingToStockProduct,
  onSingleDeleteGeneratedPLU,
} from "./form-action.js";
import {
  DEFAULT_INPUT,
  DEFAULT_LABEL,
  INVALID_INPUT,
  INVALID_LABEL,
} from "./flowbite-css-class.js";

const FORM_GENERATE_PLU = $("#FormGeneratePLU");
const REQUIRED_INPUT_ELEMENT = $(
  "input[data-required='1'][type='text'], input[data-required='1'][type='number'], input[data-required='1'][type='date'], textarea[data-required='1']"
);
const REQUIRED_SELECT_RADIO_ELEMENT = $(
  'input[type="radio"][name="GoldRateType"]'
);
const RADIO_LOGO_SETTINGS = $('input[type="radio"][name="IsWithLogo"]');

REQUIRED_INPUT_ELEMENT.on('input', (e) => {
    const element = $(e.currentTarget);
    const value = element.val();
    const isInvalidInput = value.trim() == '' ? INVALID_INPUT : DEFAULT_INPUT;
    const isInvalidLabel = value.trim() == '' ? INVALID_LABEL : DEFAULT_LABEL;

    element.removeClass().addClass(isInvalidInput);
    element.siblings('label').removeClass().addClass(isInvalidLabel);

    if (value.trim() == '') {
        element.siblings('p').removeClass('hidden');
        return;
    }

    element.siblings('p').removeClass('hidden').addClass('hidden');
});

RADIO_LOGO_SETTINGS.on("change", (e) => updateGeneratedPLU());

REQUIRED_SELECT_RADIO_ELEMENT.on("change", (e) => {
  const value = $(e.currentTarget).filter(":checked").val();

  $.ajax({
    type: "GET",
    url: `PG_Outs.aspx/GetProductModel?IsFixedRate=${value}`,
    contentType: "application/json; charset=utf-8",
    success: function (data) {
      const response = data["d"];

      if (response["MESSAGE_TYPE"] === "ERROR") {
        toastr.error(response["MESSAGE"]);
        return;
      }

      const element = $(`#SelectGoldModel`);
      element.empty();

      const goldModel = response["DATA"];
      goldModel.forEach(({ ID, Nama, NamaKode }) => {
        element.append(
          `<option value="${ID}" data-id="${ID}" data-nama-kode="${NamaKode}" data-nama="${Nama}">${Nama} (${NamaKode})</option>`
        );
      });

      element.trigger("change");
    },
    error: function (error) {
      console.error(error);
      toastr.error("Terjadi kesalahan silahkan hubungi team IT!");
    },
  });

    updateGeneratedPLU();
});

REQUIRED_INPUT_ELEMENT.on('change', (e) => {
    const element = $(e.currentTarget);
    const id = element.attr("id");
    if (["InputQuantity", "InputCertificateNumber", "InputWeight"].includes(id)) return;

    updateGeneratedPLU();
});

//$("#InputWeight").on('blur', function (e) {

//    // Only allow numbers with up to two decimal places
//    e.currentTarget.value = e.currentTarget.value === "" ? "" : cutToTwoDecimalPlaces(parseFloat(e.currentTarget.value));
//});

$("#InputWeight").on("input", function () {
  var value = $(this).val().trim();

  if (!/^\d*,?\d*(\.\d*)?$/.test(value)) {
    value = "0"; // Set to 0 if input is not a valid number
  }

  // Handle decimal places: If there are more than two, slice them
  if (value.includes(".")) {
    var parts = value.split(".");
    parts[1] = parts[1].substring(0, 2); // Limit to two decimal places without rounding
    value = parts.join(".");
  }

  if (value.includes(",")) {
    var parts = value.split(",");
    parts[1] = parts[1].substring(0, 2); // Limit to two decimal places without rounding
    value = parts.join(",");
  }

  value = value.split(",").join(".");

  // Update the input field with the valid or modified value
  $(this).val(value);
});

$("#InputQuantity").on('blur', function (e) {
    e.currentTarget.value = Math.abs(Math.round(e.currentTarget.value));
});

FORM_GENERATE_PLU.on('submit', (e) => {
    e.preventDefault();
    onGeneratePLU();
    $("#InputQuantity").val("1");
    $("#InputWeight").val("").focus();
    $("#InputCertificateNumber").val("");
});

$("#ContentGeneratedPLU").on('blur', '[id*="UpdateQuantity"]', (e) => {
    e.target.value = Math.abs(Math.round(e.target.value));
});

$("#ContentGeneratedPLU").on('blur', '[id*="UpdateWeight"]', (e) => {
    e.target.value = parseFloat(e.target.value).toFixed(2);
});

$("#ContentGeneratedPLU").on('click', '[id*="FormGenerateUpdate"]', (e) => {
    updateOrGeneratePLU(e);
});

$("#ContentGeneratedPLU").on('click', '[id*="FormDeleteUpdate"]', (e) => {
    if (confirm("Apakah anda yakin ingin menghapus group berat ini?")) {
        onDeleteGeneratedPLU(e);
    }
});

$("#ContentGeneratedPLU").on('click', '[id*="DeletePlu"]', (e) => {
    if (confirm("Apakah anda yakin ingin menghapus PLU ini?")) {
        onSingleDeleteGeneratedPLU(e);
    }
});

$("#ModalSubmitFormProduct").on('click', (e) => {
    postingToStockProduct(e);
});