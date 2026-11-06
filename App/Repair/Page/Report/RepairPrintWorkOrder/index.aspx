<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="./MasterPagePrint.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="DOKRM_index" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  </form>

  <style>
    @media print {
      @page {
        size: A4 portrait;
        margin: 1cm;
      }

      body {
        transform: none;
        width: auto;
      }

      .print-wrapper {
        zoom: 50%; /* ✅ zoom LEBIH AMAN untuk print */
      }

      /* Hide buttons and non-print areas */
      .no-print {
        display: none !important;
      }
    }
    .item-flexbox {
      flex: 1 1 0;
      width: 0;
    }

    .border-line-dotted {
      border: 0.5px dotted rgb(197, 197, 197);
      width: 100%;
    }

    .color-gray {
      color: #999;
    }
  </style>


  <div class="print-wrapper">
    <div class="container-fluid d-flex justify-content-between px-4 m-0">
      <div class="item-flexbox">
        <img
          src="/Images/web/logo_web_cmk_2017.png"
          border="0"
          runat="server"
          id="img_logo_web"
          style="height: 50px"
        />
      </div>
      <div class="item-flexbox text-center">
        <span style="font-size: 22px; font-weight: bold"
          >&nbsp;Repair Work Order&nbsp;</span
        ><br />
        <span style="font-size: 20px" class="NomorRepair"></span>
      </div>
      <div class="item-flexbox"></div>
    </div>
    <div class="border-line-dotted mt-3"></div>

    <div class="row p-4">
      <div class="col-6">
        <div class="mt-4">
          <h1>Product Information</h1>
          <table>
            <tbody>
              <tr>
                <td style="width: 125px; font-size: 14px" class="color-gray">
                  No. PLU
                </td>
                <td style="width: 10px" class="color-gray">:</td>
                <td style="width: 100px" class="PLU"></td>
              </tr>
              <tr>
                <td style="width: 125px; font-size: 14px" class="color-gray">
                  Nama Sales
                </td>
                <td style="width: 10px" class="color-gray">:</td>
                <td style="width: 100px" class="NamaSales"></td>
              </tr>
              <tr>
                <td style="width: 125px; font-size: 14px" class="color-gray">
                  Material
                </td>
                <td style="width: 10px" class="color-gray">:</td>
                <td style="width: 100px" class="FrameMaterial"></td>
              </tr>
              <tr>
                <td style="width: 125px; font-size: 14px" class="color-gray">
                  Kode Karet
                </td>
                <td style="width: 10px" class="color-gray">:</td>
                <td style="width: 100px" class="KodeKaret"></td>
              </tr>
              <tr>
                <td style="width: 125px; font-size: 14px" class="color-gray">
                  Item
                </td>
                <td style="width: 10px" class="color-gray">:</td>
                <td style="width: 100px" class="Item"></td>
              </tr>
              <tr>
                <td style="width: 125px; font-size: 14px" class="color-gray">
                  Grafir Kadar Emas
                </td>
                <td style="width: 10px" class="color-gray">:</td>
                <td style="width: 100px">
                  <span class="GrafirKadarEmas"></span>
                </td>
              </tr>
              <tr>
                <td style="width: 125px; font-size: 14px" class="color-gray">
                  Grafir Kadar Logo
                </td>
                <td style="width: 10px" class="color-gray">:</td>
                <td style="width: 100px">
                  <span class="GrafirLogo"></span>
                </td>
              </tr>
              <tr>
                <td style="width: 125px; font-size: 14px" class="color-gray">
                  Size Awal
                </td>
                <td style="width: 10px" class="color-gray">:</td>
                <td style="width: 100px">
                  <span class="SizeAwal"></span>
                </td>
              </tr>
              <tr>
                <td style="width: 125px; font-size: 14px" class="color-gray">
                  Size Akhir
                </td>
                <td style="width: 10px" class="color-gray">:</td>
                <td style="width: 100px">
                  <span class="SizeAkhir"></span>
                </td>
              </tr>
              <tr>
                <td style="width: 125px; font-size: 14px" class="color-gray">
                  Keterangan
                </td>
                <td style="width: 10px" class="color-gray">:</td>
                <td style="width: 250px" class="Keterangan"></td>
              </tr>
            </tbody>
          </table>
        </div>
      </div>

      <div class="col-6 d-flex justify-content-end">
        <div
          class="border mt-3 rounded"
          style="width: 300px; height: 240px; padding: 5px"
        >
          <img style="width: 100%; height: 100%" id="ImgBefore" />
        </div>
      </div>
    </div>

    <div class="row p-4 mt-5">
      <h3 class="fw-bold">Jenis Reparasi</h3>
      <div class="px-3">
        <table>
          <tbody id="tbody-package">
            <tr
              class="RowPackageUtama d-none"
              style="display: flex; align-items: start"
            >
              <td style="width: 125px">Paket Utama</td>
              <td style="width: 10px">:</td>
              <td id="PackageUtama"></td>
            </tr>
            <tr
              class="RowPackageSatuan d-none"
              style="display: flex; align-items: start"
            >
              <td style="width: 125px">Paket Satuan</td>
              <td style="width: 10px">:</td>
              <td id="PackageSatuan"></td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

    <div class="row p-4 mt-5">
      <div class="col-12">
        <h3 class="fw-bold">List Stone</h3>
        <table class="table table-bordered text-center">
          <thead>
            <tr>
              <th style="width: 125px">No.</th>
              <th>Stone</th>
              <th>Total Butir</th>
              <th>Total Butir Actual</th>
              <th>Total Carat</th>
              <th>Total Carat Actual</th>
            </tr>
          </thead>
          <tbody id="tbody-parcel"></tbody>
        </table>
      </div>
    </div>

    <div class="row p-4">
      <div class="col-9">
        <h3 class="fw-bold">Weight Information</h3>
        <table class="table table-bordered text-center">
          <thead>
            <tr>
              <th>Berat Awal</th>
              <th>Berat Actual</th>
              <th>Berat Potongan</th>
              <th>Berat Setelah Reparasi</th>
            </tr>
          </thead>
          <tbody>
            <tr>
              <td class="BeratTimbangAwal"></td>
              <td class="BeratTimbangActual"></td>
              <td class="BeratTimbangPotong"></td>
              <td class="BeratTimbangAkhir"></td>
            </tr>
          </tbody>
          <tbody id="tbody-parcel"></tbody>
        </table>
      </div>
    </div>

    <div style="display: flex; margin-top: 100px" class="p-4">
      <div style="display: flex; gap: 125px">
        <table class="text-center">
          <tr>
            <td
              class="text-decoration-underline fw-bold"
              style="min-width: 160px; border-bottom: 1px solid black"
            ></td>
          </tr>
          <tr>
            <td class="fst-italic">Perajin</td>
          </tr>
        </table>

        <table class="text-center">
          <tr>
            <td
              class="text-decoration-underline fw-bold"
              style="min-width: 160px; border-bottom: 1px solid black"
            ></td>
          </tr>
          <tr>
            <td class="fst-italic">Administrative Executive</td>
          </tr>
        </table>
      </div>
    </div>
  </div>

  <script>
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get("id");
    const idbrandasal = urlParams.get("idbrandasal");

    $.ajax({
      type: "GET",
      url: "./index.aspx/GetDetailRepair",
      data: {
        IDDocRepair: id,IDBrandAsal : idbrandasal
      },
      contentType: "application/json; charset=utf-8",
      success: function (data) {
        const response = data["d"];
        const detail = response["docRepairDetail"];
        const package = response["docRepairPackage"];
        const parcel = response["docRepairParcel"];

        Object.entries(detail).forEach(([key, value]) => {
          if (key !== "AddrAlamat") $(`.${key}`).html(value);
        });

        const packageHtml = package.reduce(
          (accumulator, calculator) => {
            const { TipePackage, NamaPackage } = calculator;

            if (TipePackage === 1) {
              accumulator["utama"] += `<li>${NamaPackage}</li>`;

              return accumulator;
            }
            accumulator["satuan"] += `<li>${NamaPackage}</li>`;
            return accumulator;
          },
          { utama: ``, satuan: `` }
        );

        if (packageHtml["utama"] !== "") {
          $("#PackageUtama").html(
            `<ul class="m-0">${packageHtml["utama"]}</ul>`
          );

          $(".RowPackageUtama").removeClass("d-none");
        }

        if (packageHtml["satuan"] !== "") {
          $("#PackageSatuan").html(
            `<ul class="m-0">${packageHtml["satuan"]}</ul>`
          );

          $(".RowPackageSatuan").removeClass("d-none");
        }

        const alamatToBreakSpace = detail["AddrAlamat"]
          .split("\n")
          .join("<br />");

        $("#AddrAlamat").html(
          `(<span>${detail["KodeStore"]}</span>)</br><span>${alamatToBreakSpace}</span>`
        );

        $("#ImgBefore").attr(
          "src",
          "/Images.aspx?path=" + detail["ImgProduct"]
        );

        const parcelHtml = parcel.reduce((accumulator, calculator, index) => {
          const {
            Parcel,
            StoneType,
            Clarity,
            Stone,
            Color,
            TotalButir,
            TotalButirActual,
            TotalCarat,
            TotalCaratActual,
            Total,
          } = calculator;

          accumulator += `
            <tr>
              <td>${index + 1}</td>
              <td>${Stone} - ${Color} - ${Clarity}</td>
              <td>${TotalButir}</td>
              <td>${TotalButirActual}</td>
              <td>${TotalCarat}</td>
              <td>${TotalCaratActual}</td>
            </tr>
          `;

          return accumulator;
        }, ``);

        $("#tbody-parcel").html(parcelHtml);
      },
      error: function (error) {
        console.error(error);
      },
      complete: function (data) {},
    });
  </script>
</asp:Content>
