<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="MasterPage.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="DOKRM_index" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  </form>

  <div class="container border rounded p-5">
    <h1 style="border: none" class="fw-bold mb-5 border-bottom pb-3">
      Register Akun Perajin
    </h1>
    <div class="d-flex flex-column gap-4">
      <form id="form-search-nik">
        <div class="row">
          <div class="col-6">
            <label for="NIK" class="fw-bold">NIK User</label>
            <div class="input-group">
              <input type="text" class="form-control" id="nik" />
              <button class="btn btn-primary btn-lg w-25" id="btn-search">
                Cari Karyawan
              </button>
            </div>
          </div>
        </div>
      </form>
      <form class="d-flex flex-column gap-4" id="form-insert-tukang">
        <div class="row">
          <div class="col-6">
            <label for="NIK" class="fw-bold">Email Karyawan</label>
            <input
              type="text"
              class="form-control"
              id="m_emailkantor"
              disabled
            />
          </div>
        </div>
        <div class="row">
          <div class="col-3">
            <label for="NIK" class="fw-bold">NIK</label>
            <input type="text" class="form-control" id="m_nik" disabled />
          </div>
          <div class="col-3">
            <label for="NIK" class="fw-bold">Nama Karyawan</label>
            <input type="text" class="form-control" id="m_nama" disabled />
          </div>
        </div>
        <div class="row">
          <div class="col-6">
            <label for="NIK" class="fw-bold">Jabatan Karyawan</label>
            <input type="text" class="form-control" id="m_jabatan" disabled />
          </div>
        </div>
        <div class="row">
          <div class="col-6">
            <label for="NIK" class="fw-bold">Lokasi Service Point</label>
            <select id="select-location" class="select2 w-100"></select>
          </div>
        </div>
        <button
          class="col-6 btn btn-primary btn-lg"
          id="btn-register-akun"
          type="submit"
        >
          Register Akun
        </button>
      </form>
    </div>
  </div>

  <script type="text/javascript">
    const SELECT_LOCATION = $("#select-location");
    const SELECT_PROVINCE = $("#select-province");
    const BUTTON_SUBMIT = $(".btn-submit-form");
    const BUTTON_CLOSE = $(".btn-close-form");
    const INPUT_MINIMUM_PAYMENT = $("#input-minimum-payment");

    toastr.options = {
      closeButton: true,
      progressBar: true,
      positionClass: "toast-bottom-center",
      showDuration: "300",
      hideDuration: "1000",
      timeOut: "5000",
    };

    function getLocation() {
      $.ajax({
        type: "GET",
        url: "./index.aspx/GetLocation",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];

          response.forEach((item, index) => {
            const { ID, Nama, Kode, TipeLokasi } = item;
            const isSelected = index === 0 ? "selected" : "";

            SELECT_LOCATION.append(
              `<option ${isSelected} data-id="${ID}" data-nama="${Nama}" data-kode="${Kode}" data-tipe-lokasi="${TipeLokasi}" value="${Kode}">${Nama}</option>`
            );
          });

          SELECT_LOCATION.trigger("change");
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          console.log(data);
        },
      });
    }

    function getKaryawan() {
      $("#btn-search").attr("disabled", true).text("Loading...");

      $.ajax({
        type: "GET",
        url: "./index.aspx/GetKaryawan",
        data: { nik: `"${$("#nik").val()}"` },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];

          if (response.length === 0) {
            toastr.error("Karyawan tidak ditemukan!");
            return;
          }

          const karyawan = response[0];
          Object.keys(karyawan).forEach((item) =>
            $("#" + item).val(karyawan[item])
          );
        },
        error: function (error) {
          toastr.error("Terjadi kesalahan saat mencari data karyawan!");
          $("#btn-search").attr("disabled", false).text("Cari Karyawan");
          console.error(error);
        },
        complete: function (data) {
          console.log(data);
          $("#btn-search").attr("disabled", false).text("Cari Karyawan");
        },
      });
    }

    function insertKaryawan() {
      $("#btn-register-akun").attr("disabled", true).text("Loading...");

      $.ajax({
        type: "POST",
        url: "./index.aspx/InsertDataKaryawan",
        data: JSON.stringify({
          arguments: {
            NIK: $("#m_nik").val(),
            Email: $("#m_emailkantor").val(),
            Jabatan: $("#m_jabatan").val(),
            Nama: $("#m_nama").val(),
            KodeStore: SELECT_LOCATION.val(),
          },
        }),
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];
          if (!response.Message) {
            toastr.success("Berhasil membuat data tukang.");
            window.location.reload();
            return;
          }

          toastr.error(response.Message);
        },
        error: function (error) {
          toastr.error("Terjadi kesalahan saat mencari data karyawan!");
          $("#btn-register-akun").attr("disabled", false).text("Register Akun");
          console.error(error);
        },
        complete: function (data) {
          console.log(data);
          $("#btn-register-akun").attr("disabled", false).text("Register Akun");
        },
      });
    }

    // Instantiate
    $(document).ready(function () {
      $(".select2").select2();
    });

    // Event Handler
    $(document).ready(function () {
      $(document).on("submit", "#form-search-nik", function (e) {
        e.preventDefault();
        getKaryawan();
      });

      $(document).on("submit", "#form-insert-tukang", function (e) {
        e.preventDefault();
        insertKaryawan();
      });
    });

    // Fetch Initial Data
    $(document).ready(function () {
      getLocation();
    });
  </script>
</asp:Content>
