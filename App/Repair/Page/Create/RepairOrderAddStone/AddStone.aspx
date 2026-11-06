<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="MasterPage.master" AutoEventWireup="true"
CodeFile="AddStone.aspx.cs" Inherits="DOKRM_detail" %>

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

  <div id="loader">Loading...</div>

  <div class="d-none flex-column" id="main-content">
    <div class="container-fluid border rounded p-5">
      <div class="row">
        <div class="col-12">
          <h1 style="border: none; padding-bottom: 10px">Detail Repair</h1>
          <div class="row">
            <div class="col d-flex flex-column gap-1">
              <h6 class="text-md fw-bold">NomorRepair</h6>
              <h5 class="text-lg fw-normal" id="NomorRepair"></h5>
            </div>
            <div class="col d-flex flex-column gap-1">
              <h6 class="text-md fw-bold">PLU</h6>
              <h5 class="text-lg fw-normal" id="PLU"></h5>
            </div>
            <div class="col d-flex flex-column gap-1">
              <h6 class="text-md fw-bold">Store</h6>
              <h5 class="text-lg fw-normal" id="Store"></h5>
            </div>
            <div class="col d-flex flex-column gap-1">
              <h6 class="text-md fw-bold">Lokasi Repair</h6>
              <h5 class="text-lg fw-normal" id="locRepair"></h5>
            </div>
          </div>
        </div>
      </div>
    </div>    
    
    <div
      class="container-fluid border rounded p-5 mt-5"
      id="content-detail-stone"
    >
      <h1 style="border: none; padding-bottom: 10px">Detail Stone</h1>
      <div class="table-responsive mt-0">
        <div class="row p-2 mt-3">
            <div class="col-md-4">
                <label for="input-search-product" class="form-label fw-bold">PLU</label>
                <input type="text" class="form-control" id="input-search-product" />
            </div>            
            <button
                class="btn btn-lg btn-primary align-self-end"
                style="width: max-content"
                id="btn-search-report"
                onclick="GetStone()"
            >
            <i class="icon-search me-2"></i>
            Search
            </button>      
        </div>
        <table class="table table-hover">
          <thead>
            <tr> 
              <th>PLU</th>
              <th>Product Item</th>
              <th>Tipe Stone</th>
              <th>Parcel</th>
              <th>Total Butir</th>
              <th>Total Carat</th>              
              <th>Carat per Butir</th>                            
              <th>GIA</th>
              <th>Action</th>                            
            </tr>
          </thead>
          <tbody id="tbody-detail-stone"></tbody>
        </table>
      </div>
    </div>

    <div class="row mt-3 justify-content-end">      
      <div class="col-2">
        <button
          class="btn btn-primary btn-lg w-100"
          data-bs-toggle="modal"
          data-bs-target="#modalConfirmation"
          id="btn-modal-confirmation"
          disabled
        >
          Simpan
        </button>
      </div>
    </div>

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
            <b>Apakah anda yakin?</b>
            <p>Pastikan data yang anda masukkan sudah benar!</p>
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
    const IMG_PRODUCT = $("#img-product");
    const MAIN_CONTENT = $("#main-content");
    const LOADER = $("#loader");
    let DATA_STONE=[];
    let BTN_CONFIRMATION_SUBMIT = $("#btn-confirmation-submit");

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

    function injectTextToElement({ id, text }) {
      if (id === "BeratTimbangActual" || id === "BeratTimbangPotong") {
        $("." + id).text(text);
        return;
      }

      if (id === "ImgPicture") {
        $("#" + id).attr("src", text);
        return;
      }

      if (id === "Blemish") {
        $("#" + id).text(text === "1" ? "Basic" : "Non-Basic");
        return;
      }

      $("#" + id).text(text);
    }

    function setContentPackage({ text, element }) {
      return `
        <div class="col d-flex flex-column gap-1 mt-3">
          <h6 class="text-md fw-bold">${text}</h6>
          <ul>${element}</ul>
        </div>
      `;
    }

    // Fetch Initial Data
    $(document).ready(function () {
      const urlParams = new URLSearchParams(window.location.search);
      const IDDocRepair = atob(urlParams.get("id"));           

      $.ajax({
        type: "GET",
        url: "./AddStone.aspx/GetDetailRepair",
        data: {
          IDDocRepair: IDDocRepair,
        },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
            const response = data["d"];                    
            const detailRepair = response["docRepairDetail"];
            
            $("#Store").text(detailRepair.Store);
            $("#NomorRepair").text(detailRepair.NomorRepair);
            $("#PLU").text(detailRepair.PLU);
            $("#locRepair").text(detailRepair.LocRepair);
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          LOADER.hide();
          MAIN_CONTENT.removeClass("d-none").addClass("d-flex");
        },
      });
    
    });
    
    function GetStone(){
        const urlParams = new URLSearchParams(window.location.search);
        const IDDocRepair = atob(urlParams.get("id"));      
        const keyword = $("#input-search-product").val().trim();

        if (keyword == '') {
          $("#input-search-product").focus()
          toastr.error("PLU tidak boleh kosong!");
          return;
        }

        $.ajax({
            type: "GET",
            url: "./AddStone.aspx/GetStone",
            data: {
                keyword: `'${keyword}'`,
                IDDocRepair: IDDocRepair
            },
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                const response = data["d"];                                    

                if (response.length == 0) {
                  toastr.error("PLU Setting tersebut bukan milik Nomor RPR ini!");
                  return;
                }
                if (DATA_STONE.some(d => d.IDProduct === response[0].IDProduct)) {
                  toastr.error("PLU Setting tersebut sudah ditambahkan!");
                  return;
                }

                DATA_STONE.push(...response)                
                console.log(DATA_STONE);
                refreshTable();      
            },
            error: function (error) {
              console.error(error);
            },
            complete: function (data) {
            //   LOADER.hide();
            //   MAIN_CONTENT.removeClass("d-none").addClass("d-flex");
            $('#input-search-product').val('');
            checkStone();
            },
          });
    }

    function checkStone(){
        if (DATA_STONE.length > 0){
            $("#btn-modal-confirmation").removeAttr("disabled");
        } else {
            $("#btn-modal-confirmation").attr("disabled", "disabled");
            
        }
    }

    function deleteStone(id){
        DATA_STONE = DATA_STONE.filter(item => item.IDProduct !== id);        
        checkStone();
        refreshTable();
    }
    
    function SaveData(){                      
        $("#btn-modal-confirmation").attr("disabled", true).text("Loading...");
        $.ajax({
            type: "POST",
            url: "./AddStone.aspx/insertStoneSetting",
            data: JSON.stringify({
                DataStone: DATA_STONE                
            }),
            contentType: "application/json; charset=utf-8",
            success: function (data, textStatus, jqXHR) {
                const response = data["d"];                                    
                if (jqXHR.status === 200){
                    toastr.success("Berhasil meyimpan PLU Setting!");
                    setTimeout(() => {
                      window.location.href = `./index.aspx`;
                    }, 500);
                    
                } else {
                    toastr.error("Terjadi kesalahan saat menyimpan PLU Setting!");
                    return;
                }           
                
            },
            error: function (error, textStatus, jqXHR) {
              console.error(error);
              toastr.error("Terjadi kesalahan saat menyimpan PLU Setting1!");
              $("#btn-modal-confirmation").attr("disabled","disabled");              
            },
            complete: function (data, textStatus, jqXHR) {              
              
            },
          });
    }

    function refreshTable(){
        const stringAdditional = DATA_STONE.reduce(
                (accumulator, calculator) => {
                    const {
                        CaratButir,
                        Parcel,
                        TotalButir,
                        TotalCarat,
                        GIA,
                        IDBrand,
                        IDLokasi,
                        IDProduct,
                        PLU,
                        ProductItem,
                        Substorage,
                        SupplierNomor,
                        TipeLokasi,
                        TipeStone
                    } = calculator;                
                    accumulator += `
                <tr>
                    <td>${PLU}</td>
                    <td>${ProductItem}</td>
                    <td>${TipeStone}</td>
                    <td>${Parcel}</td>
                    <td>${TotalButir}</td>                                                        
                    <td>${TotalCarat}</td>                                                        
                    <td>${CaratButir}</td>                                                        
                    <td>${GIA}</td>                                                        
                    <td>
                        <a class="btn btn-danger btn-md" onclick="deleteStone(${IDProduct})" >Delete Stone</a>                        
                    </td>
                </tr>
                `;

                    return accumulator;
                },
                ""
                );

                $('#tbody-detail-stone').empty();
                $('#tbody-detail-stone').append(stringAdditional);
    }
  $(document).ready(()=>{
    BTN_CONFIRMATION_SUBMIT.on("click", () => SaveData());      
  });
  </script>
</asp:Content>
