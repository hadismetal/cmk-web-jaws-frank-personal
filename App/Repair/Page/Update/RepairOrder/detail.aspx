<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="MasterPage.master" AutoEventWireup="true"
CodeFile="detail.aspx.cs" Inherits="DOKRM_detail" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
  Report Reparasi
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server"> Detail </asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  </form>

  <div id="loader">Loading...</div>

  <div class="d-none flex-column" id="main-content">
    <div class="container-fluid border rounded p-5">
      <div class="row">
        <div class="col-7">
          <h1 style="border: none; padding-bottom: 10px">Detail Store</h1>
          <div class="row">
            <div class="col d-flex flex-column gap-1">
              <h6 class="text-md fw-bold">Store</h6>
              <h5 class="text-lg fw-normal" id="Store"></h5>
            </div>
            <div class="col d-flex flex-column gap-1">
              <h6 class="text-md fw-bold">JP / JR</h6>
              <h5 class="text-lg fw-normal" id="NamaSales"></h5>
            </div>
            <div class="col d-flex flex-column gap-1">
              <h6 class="text-md fw-bold">Customer</h6>
              <h5 class="text-lg fw-normal" id="NamaCustomer"></h5>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="container-fluid p-0 mt-5">
      <div class="row">
        <div class="col-7">
          <div class="border rounded p-5">
            <h1 style="border: none; padding-bottom: 10px">Info Produk</h1>
            <div class="row gx-5 justify-content-center align-items-center">
              <div class="col-3">
                <img
                  id="img-product"
                  class="border rounded p-5"
                  style="width: 100%"
                />
              </div>
              <div class="col">
                <div class="row gy-5">
                  <div class="col-6">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">No Invoice</h6>
                      <h5 class="text-lg fw-normal" id="Invoice"></h5>
                    </div>
                  </div>
                  <div class="col-6">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">PLU</h6>
                      <h5 class="text-lg fw-normal" id="PLU"></h5>
                    </div>
                  </div>
                  <div class="col-6">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Item</h6>
                      <h5 class="text-lg fw-normal" id="Item"></h5>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>

          <div class="border rounded p-5 mt-5">
            <h1 style="border: none; padding-bottom: 10px">Detail QC</h1>
            <div class="row">
              <div class="col-10">
                <div class="row g-3">
                  <div class="col-6">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Weight</h6>
                      <div class="row">
                        <div class="col-6">
                          <div class="col d-flex flex-column gap-1">
                            <h6 class="text-sm fw-bold">Berat Actual</h6>
                            <h5 class="fw-normal" id="BeratTimbangAwal"></h5>
                          </div>
                        </div>
                        <div class="col-6">
                          <div class="col d-flex flex-column gap-1">
                            <h6 class="text-sm fw-bold">Berat Akhir</h6>
                            <h5 class="fw-normal" id="BeratTimbang"></h5>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div class="col-6">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Frame Color</h6>
                      <h5 class="fw-normal" id="FrameColor"></h5>
                    </div>
                  </div>
                  <div class="col-6">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Grafir Kadar Emas</h6>
                      <h5 class="fw-normal" id="GrafirKadarEmas"></h5>
                    </div>
                  </div>
                  <div class="col-6">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Grafir Logo</h6>
                      <h5 class="fw-normal" id="GrafirLogo"></h5>
                    </div>
                  </div>
                  <div class="col-6">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Tes Keaslian</h6>
                      <h5 class="fw-normal" id="TesKeaslian"></h5>
                    </div>
                  </div>
                  <div class="col-6">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Fisik Keseluruhan</h6>
                      <h5 class="fw-normal" id="FisikKeseluruhan"></h5>
                    </div>
                  </div>
                  <div class="col-6">
                    <div class="col d-flex flex-column gap-1">
                      <h6 class="text-md fw-bold">Keterangan</h6>
                      <h5 class="fw-normal" id="Keterangan"></h5>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>

        <div class="col-5">
          <div class="border rounded p-5" id="content-detail-reparasi">
            <h1 style="border: none; padding-bottom: 10px">Detail Reparasi</h1>
            <div class="col d-flex flex-column gap-1">
              <h6 class="text-md fw-bold">Lokasi Reparasi</h6>
              <h5 class="fw-normal" id="LocRepair"></h5>
            </div>
          </div>
        </div>
      </div>
    </div>

    <div class="container-fluid border rounded p-5 mt-5">
      <h1 style="border: none; padding-bottom: 10px">Detail Stone</h1>
      <table class="table table-hover">
        <thead>
          <tr>
            <th>No</th>
            <th>Stone Name</th>
            <th>Total Butir</th>
            <th>Total Butir Actual</th>
            <th>Total Carat</th>
            <th>Total Carat Actual</th>
            <th>Tes Kualitas</th>
            <th>Tes Keaslian</th>
            <th>Sesuai Sertifikat</th>
            <th>Keterangan</th>
          </tr>
        </thead>
        <tbody id="tbody-detail-stone"></tbody>
      </table>
    </div>
  </div>

  <script type="text/javascript">
    const IMG_PRODUCT = $("#img-product");
    const MAIN_CONTENT = $("#main-content");
    const LOADER = $("#loader");

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
      const id = urlParams.get("id");

      $.ajax({
        type: "GET",
        url: "./detail.aspx/GetDetailRepair",
        data: {
          IDDocRepair: id,
        },
        contentType: "application/json; charset=utf-8",
        success: function (data) {
          const response = data["d"];
          const detail = response["docRepairDetail"];
          const package = response["docRepairPackage"];
          const parcel = response["docRepairParcel"];

          IMG_PRODUCT.attr(
            "src",
            "https://" +
              window.location.hostname +
              "/Images.aspx?path=" +
              detail["ImgProduct"]
          );
          IMG_PRODUCT.attr("alt", detail["PLU"]);

          Object.keys(detail).forEach((item) => {
            injectTextToElement({
              id: item,
              text: detail[item],
            });
          });

          const paket = package.reduce(
            (accumulator, calculator) => {
              const { NamaPackage, TipePackage } = calculator;

              if (TipePackage === 1) {
                accumulator["utama"] += `
                  <li>${NamaPackage}</li>
                `;
              } else {
                accumulator["satuan"] += `
                  <li>${NamaPackage}</li>
                `;
              }
              return accumulator;
            },
            { satuan: "", utama: "" }
          );

          if (paket["utama"] !== "")
            $("#content-detail-reparasi").append(
              setContentPackage({
                text: "Paket Utama",
                element: paket["utama"],
              })
            );
          if (paket["satuan"] !== "")
            $("#content-detail-reparasi").append(
              setContentPackage({
                text: "Satuan Tambahan",
                element: paket["satuan"],
              })
            );

          const stone = parcel.reduce((accumulator, calculator, index) => {
            const {
              CaratButir,
              IDDocRepair,
              NomorRepair,
              Parcel,
              StoneType,
              StoneTypeMap,
              TotalButir,
              TotalButirActual,
              TotalCarat,
              TotalCaratActual,
              SesuaiSertifikat,
              TesKualitas,
              TesKeaslian,
              Keterangan,
            } = calculator;

            accumulator += `
                  <tr>
                    <td>${index + 1}</td>
                    <td>${Parcel}</td>
                    <td>${TotalButir}</td>
                    <td>${TotalButirActual}</td>
                    <td>${TotalCarat}</td>
                    <td>${TotalCaratActual}</td>
                    <td>${SesuaiSertifikat ? "SESUAI" : "TIDAK SESUAI"}</td>
                    <td>${TesKualitas}</td>
                    <td>${TesKeaslian}</td>
                    <td>${Keterangan}</td>
                  </tr>
                `;
            return accumulator;
          }, "");

          $("#tbody-detail-stone").append(stone);
          MAIN_CONTENT.removeClass("d-none").addClass("d-flex");
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {
          LOADER.hide();
        },
      });
    });
  </script>
</asp:Content>
