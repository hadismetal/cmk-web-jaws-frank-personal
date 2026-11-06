<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="MasterPage.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="DOKRM_index" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
  Form Penambahan PLU Setting
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
      <div class="col-md-4">
        <label for="input-search-product" class="form-label fw-bold">Nomor Reparasi</label>
        <input type="text" class="form-control" id="input-search-product" />
      </div>            
        <button
        class="btn btn-lg btn-primary align-self-end"
        style="width: max-content"
        id="btn-search-report"
      >
        <i class="icon-search me-2"></i>
        Search
      </button>      
    </form>

    <table class="table table-hover mt-5">
      <thead>
        <tr>
          <th>No</th>
          <th>No. Repair</th>
          <th>PLU</th>          
          <th>Customer</th>
          <th>Sales</th>
          <th>Status</th>
          <th></th>
        </tr>
      </thead>
      <tbody id="tbody-report-repair"></tbody>
    </table>

    <div
    class="modal"
    id="modalConfirmation"
    tabindex="-1"
    aria-labelledby="modalConfirmationLabel"
    aria-hidden="true"
  >
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">Confirmation</h5>
          <button
            type="button"
            class="btn-close"
            data-bs-dismiss="modal"
            aria-label="Close"
          ></button>
        </div>
        <div class="modal-body">
          <b>Apakah anda yakin untuk cancel form reparasi tersebut?</b>          
          <div class="tempid">
            <input type="hidden" id="tempid1" value="">
          </div>
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
            id="btn-confirmation-submit"
            data-bs-dismiss="modal"
          >
            Yakin
          </button>
        </div>
      </div>
    </div>
  </div>
  </div>


  <script type="text/javascript">
    // Fetch Initial Data
    $(document).ready(function () {
      let TBODY_REPORT_REPAIR = $("#tbody-report-repair");
      let DATA_LOCATION = [];
      let DATA_LOCATION_REPARASI = [];
      let BUTTON_SEARCH_REPORT = $("#btn-search-report");
      const SELECT_LOCATION = $("#select-location");
      const SELECT_LOKASI_REPARASI = $("#select-lokasi-reparasi");
      const SELECT_STATUS_REPARASI = $("#select-status-reparasi");
      let BUTTON_SUBMIT_FORM = $("#btn-confirmation-submit");
      const INPUT_SEARCH_PRODUCT = $("#input-search-product");
      const FORM_DATA = $("#form-data");
      const INPUT_TGL_AWAL = $("#input-tgl-awal");
      const INPUT_TGL_HINGGA = $("#input-tgl-hingga");

      BUTTON_SUBMIT_FORM.on("click", () => onSubmitFormAndUploadFile());

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

        const parameters = {
          NomorRPR: `"${INPUT_SEARCH_PRODUCT.val()}"`          
        };

        $.ajax({
          type: "GET",
          url: "./index.aspx/GetDocRepair",
          data: parameters,
          contentType: "application/json; charset=utf-8",
          success: function (data) {
            const response = data["d"];
            console.log(response);
            BUTTON_SEARCH_REPORT.attr("disabled", false).html(
              " <i class='icon-search'></i>"
            );let No = 0;
            const stringAdditional = response.reduce(
              (accumulator, calculator) => {
                const {                
                PLU,
                IDDocRepair,
                NamaCustomer,
                IDCustomer,
                NamaSales,
                IDSales,
                NomorDocRepair,
                IDStatus,
                Status,                 
                No,
                } = calculator;                
                accumulator += `
              <tr>
                <td>${No}</td>
                <td>${NomorDocRepair}</td>
                <td>${PLU}</td>                
                <td>${NamaCustomer}</td>
                <td>${NamaSales}</td>
                <td>${Status}</td>
                <td>
                <td class="w-100 h-auto d-flex">
                  <div class="justify-content-evenly">
                    <a class="btn btn-success btn-md" onclick="AddStone(${IDDocRepair})" >Add Stone</a>
                  </div>
                </td>
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
              " <i class='icon-search me-2'></i>Search"
            );
          },
        });
      }

      const urlParams = new URLSearchParams(window.location.search);
      const idLokasi = urlParams.get("IDLokasi");
      const tipeLokasi = urlParams.get("TipeLokasi");
      const status = urlParams.get("status");

      let currDate = new Date();
      const offset = currDate.getTimezoneOffset();
      currDate = new Date(currDate.getTime() - offset * 60 * 1000);
      const currDateISO = currDate.toISOString().split("T")[0];
      const splittedDate = currDateISO.split("-");

      if (!idLokasi && !tipeLokasi && !status) {
        const startDateISO = `${splittedDate[0]}-${splittedDate[1]}-01`;
        INPUT_TGL_AWAL.val(startDateISO);
      } else {
        INPUT_TGL_AWAL.val("2024-01-01");
      }

      INPUT_TGL_HINGGA.val(currDateISO);

      SELECT_LOCATION.select2();
      SELECT_LOKASI_REPARASI.select2();
      SELECT_STATUS_REPARASI.select2();

      FORM_DATA.on("submit", (e) => {
        e.preventDefault();
        onSearchFormRepair();
      });

      $(document).on("click", "#tbody-report-repair button", function (e) {
        const element = $(e.currentTarget);
        const id = element.attr("data-id");

        window.open("../RepairPrintBonOrder?id=" + id);
        window.open("../RepairPrintWorkOrder?id=" + id);
      });

      $.ajax({
        type: "GET",
        url: "./index.aspx/GetStatusReparasi",
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];

          response.forEach(({ ID, Nama }) => {
            const option = new Option(Nama, ID, ID == status, ID == status);
            SELECT_STATUS_REPARASI.append(option);
          });

          SELECT_STATUS_REPARASI.trigger("change");
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (outer) {
          $.ajax({
            type: "GET",
            url: "./index.aspx/GetUserOutletAndDefaultSales",
            contentType: "application/json; charset=utf-8",
            success: function (data) {
              DATA_LOCATION = data["d"]["location"];
              DATA_SALES = data["d"]["sales"];
              const select2Location = SELECT_LOCATION.select2();

              const mapLocation = DATA_LOCATION.forEach(
                (
                  { IDLokasi, NamaOutlet, KodeOutlet, TipeLokasi, IDBrand },
                  index
                ) => {
                  let optionLocation = new Option(
                    NamaOutlet,
                    IDLokasi,
                    IDLokasi == idLokasi && TipeLokasi == tipeLokasi,
                    IDLokasi == idLokasi && TipeLokasi == tipeLokasi
                  );
                  optionLocation.setAttribute("data-id-lokasi", IDLokasi);
                  optionLocation.setAttribute("data-nama-lokasi", NamaOutlet);
                  optionLocation.setAttribute("data-kode-lokasi", KodeOutlet);
                  optionLocation.setAttribute("data-tipe-lokasi", TipeLokasi);
                  optionLocation.setAttribute("data-id-brand", IDBrand);
                  select2Location.append(optionLocation);
                }
              );

              select2Location.trigger("change");
            },
            error: function (error) {
              console.error(error);
            },
            complete: function (secondOuter) {
              $.ajax({
                type: "GET",
                url: "./index.aspx/GetLocReparasi",
                contentType: "application/json; charset=utf-8",
                success: function (data) {
                  DATA_LOCATION_REPARASI = data["d"];

                  const select2LocationRepair =
                    SELECT_LOKASI_REPARASI.select2();

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
                complete: function (thridOuter) {
                  if (status && idLokasi && tipeLokasi) {
                    onSearchFormRepair();
                  }
                },
              });
            },
          });
        },
      });
    
      $('#btn-modal-confirmation').click((e)=>{
        console.log(e)
        const element = $(e.currentTarget);
        const id = element.attr("data-id");        
        BUTTON_SUBMIT_FORM.attr("data-id", id);
      })

      $('#modalConfirmation').on('hide.bs.modal', () => {
        $('#tempid1').val()        
      });
      
      function onSubmitFormAndUploadFile(e) {      
      var IDDocRepair = $('#tempid1').val();
      $.ajax({
        type: "GET",
        url: "./index.aspx/Cancel",
        data: {IDDocRepair:IDDocRepair},
        contentType: "application/json; charset=utf-8",
        success: function (data) {          
          const response = data["d"];
          if (response[0].Code == 200) {
            toastr.success(response[0].Message, "Success", {
              timeOut: 3000,
            });
            onSearchFormRepair();
          }else{
            toastr.error(response[0].Message, "Error", {
              timeOut: 5000,
            });
          }                              
        },
        error: function (error) {
          console.error(error);
        }
      });
      // onSearchFormRepair();
    }
  });
  function cancel(IDDocRepair){      
    $('#modalConfirmation').modal('show');      
    $('#tempid1').val(IDDocRepair)
  }
  function Print(IDDocRepair){
    window.open(`../RepairInvoice/PrintBuktiPengembalian.aspx?id=${IDDocRepair}`);
  }    
  function PrintInvoice(IDDocRepair){    
    window.open(`../RepairInvoice/RepairInvoiceGabungan.aspx?id=${IDDocRepair}`);
    window.open(`../RepairInvoice/RepairInvoice.aspx?id=${IDDocRepair}`);    
    $.ajax({
          type: "GET",
          url: "./index.aspx/GetDetailInvoice",
          data: { IDDocRepair: IDDocRepair },
          contentType: "application/json; charset=utf-8",
          success: function (data) {
            const response = data["d"];


            if (response.Tax > 0) {              
            window.open(`../RepairInvoice/RepairInvoiceMaterial.aspx?id=${IDDocRepair}`);    
            }            
            
          },
            error: function (error) {
            console.error(error);
          },
          complete: function (data) {            
          },
        });

    
  }    

  function AddStone(IDDocRepair){    
    window.location.href = `./AddStone.aspx?id=${btoa(IDDocRepair)}`;
  }
  //../RepairPrintBonOrder
  </script>

</asp:Content>
