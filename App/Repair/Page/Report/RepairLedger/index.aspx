<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="MasterPage.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="DOKRM_index" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  </form>

  <div class="container-fluid border rounded p-5">
    <h1 style="border: none" class="fw-bold mb-5 border-bottom pb-3">
      Ledger Reparasi
    </h1>
    <div class="d-flex flex-column gap-4">
      <form id="form-search-ledger">
        <div class="row">
          <div class="col-3">
            <label for="keywords" class="fw-bold">Nomor Reparasi / PLU</label>
            <div class="input-group">
              <input type="text" class="form-control" id="keywords" />
              <button class="btn btn-primary btn-lg w-25" id="btn-search">
                Cari
              </button>
            </div>
          </div>
        </div>
      </form>

      <div class="col-12">
        <table class="table-responsive table table-hover">
          <thead>
            <tr>
              <th>No.</th>
              <th>Nomor Repair</th>
              <th>PLU</th>
              <th>Lokasi</th>
              <th>Keterangan</th>
              <th>Created At</th>
              <th>Created By</th>
            </tr>
          </thead>
          <tbody id="tbody-ledger"></tbody>
        </table>
      </div>
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

    function getLedger() {
      $("#btn-search").attr("disabled", true).text("Loading...");

      $.ajax({
        type: "GET",
        url: "./index.aspx/GetLedger",
        data: { Keywords: `"${$("#keywords").val()}"` },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];

          if (response.length === 0) {
            toastr.error("Ledger tidak ditemukan!");
            return;
          }

          $("#tbody-ledger").empty();
          response.forEach(
            ({ NomorRepair, PLU, Keterangan, Status, CreatedAt, CreatedBy, Lokasi, IsCancelRepair}, index) => {
              var KeteranganCancel = IsCancelRepair == 1 ? '(Return/Cancel)':'';              
              $("#tbody-ledger").append(`
              <tr>
                <td>${index + 1}</td>
                <td>${NomorRepair}</td>
                <td>${PLU}</td>
                <td>${Lokasi == null ? '' : Lokasi}</td>
                <td>${Keterangan} ${[4, 5, 6, 7, 10].includes(Status) ? KeteranganCancel:''}</td>
                <td>${CreatedAt}</td>
                <td>${CreatedBy}</td>
              </tr>
            `);
            }
          );
        },
        error: function (error) {
          toastr.error("Terjadi kesalahan saat mencari data ledger!");
          $("#btn-search").attr("disabled", false).text("Cari");
          console.error(error);
        },
        complete: function (data) {
          console.log(data);
          $("#btn-search").attr("disabled", false).text("Cari");
        },
      });
    }

    // Instantiate
    $(document).ready(function () {
      $(".select2").select2();
    });

    // Event Handler
    $(document).ready(function () {
      $(document).on("submit", "#form-search-ledger", function (e) {
        e.preventDefault();
        getLedger();
      });
    });
  </script>
</asp:Content>
