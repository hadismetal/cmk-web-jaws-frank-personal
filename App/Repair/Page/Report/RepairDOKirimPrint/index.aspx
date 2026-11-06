<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="./MasterPagePrint.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="DOKRM_index" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <form runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
  </form>

  <style>
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

    @media print {
      .print-left {
        width: 60%;
      }

      .print-right {
        width: 40%;
      }

      .table-print,
      .information-print,
      .list-repair-print {
        width: 100%;
      }

      .row-print {
        font-size: 12px;
      }

      .no-print {
        width: 25px !important;
      }

      .ttd-print {
        padding-top: 100px !important;
      }

      .spacing-print {
        padding: 0 !important;
        margin: 0 !important;
      }
    }

    @media print and (orientation: portrait) {
      .ttd-print {
        padding-top: 100px !important;
      }
    }
  </style>

  <div class="container-fluid d-flex justify-content-between px-4 m-0">
    <div class="item-flexbox text-center">
      <span style="font-size: 22px; font-weight: bold"
        >&nbsp;Repair KRM Delivery Order &nbsp;</span
      ><br />
      <span style="font-size: 20px" class="NomorKRM"></span>
    </div>
  </div>
  <div class="border-line-dotted mt-3"></div>

  <div class="row p-4">
    <div class="col-7 information-print">
      <div class="mt-5">
        <h1>Repair Information</h1>
        <div class="row table-print">
          <div class="col-6 print-left">
            <table>
              <tbody>
                <tr>
                  <td style="width: 190px; font-size: 14px" class="color-gray">
                    Nomor Kirim
                  </td>
                  <td style="width: 10px" class="color-gray">:</td>
                  <td style="width: 300px" class="NomorKRM"></td>
                </tr>
                <tr>
                  <td style="width: 190px; font-size: 14px" class="color-gray">
                    Pengirim
                  </td>
                  <td style="width: 10px" class="color-gray">:</td>
                  <td style="width: 300px" class="NamaAsal"></td>
                </tr>
                <tr>
                  <td style="width: 190px; font-size: 14px" class="color-gray">
                    Tanggal Cetak
                  </td>
                  <td style="width: 10px" class="color-gray">:</td>
                  <td style="width: 300px" class="TglCetak"></td>
                </tr>
                <tr>
                  <td style="width: 190px; font-size: 14px" class="color-gray">
                    Tanggal Approve
                  </td>
                  <td style="width: 10px" class="color-gray">:</td>
                  <td style="width: 300px" class="TglKirim"></td>
                </tr>
                <tr>
                  <td style="width: 190px; font-size: 14px" class="color-gray">
                    Keterangan
                  </td>
                  <td style="width: 10px" class="color-gray">:</td>
                  <td style="width: 300px" class="Keterangan"></td>
                </tr>
              </tbody>
            </table>
          </div>
          <div class="col-6 print-right">
            <table>
              <tbody>
                <tr>
                  <td style="width: 190px; font-size: 14px" class="color-gray">
                    Kurir
                  </td>
                  <td style="width: 10px" class="color-gray">:</td>
                  <td style="width: 300px" class="NamaKurir"></td>
                </tr>
                <tr>
                  <td style="width: 190px; font-size: 14px" class="color-gray">
                    Penerima
                  </td>
                  <td style="width: 10px" class="color-gray">:</td>
                  <td style="width: 300px" class="NamaTujuan"></td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>

  <div class="row p-4 mt-5 spacing-print">
    <div class="col-8 list-repair-print">
      <h1>List Repair</h1>
      <table class="table table-bordered text-center">
        <thead>
          <tr class="row-print">
            <th class="text-left no-print" style="width: 75px">No.</th>
            <th class="text-left">No Repair</th>
            <th class="text-left">PLU</th>
            <th class="text-left">Item</th>
            <th class="text-right">Qty</th>
            <th class="text-right">Nett Weight</th>
            <th class="text-right">Gross Weight</th>
            <!-- <th>Image</th> -->
          </tr>
        </thead>
        <tbody id="tbody-paket" class="row-print"></tbody>
      </table>
    </div>
  </div>

  <div
    style="display: flex; justify-content: end; padding-top: 150px"
    class="ttd-print"
  >
    <div style="display: flex; gap: 50px; align-items: end; text-align: center">
      <div class="flex flex-column" style="min-width: 150px">
        <div class="NamaKurir"></div>
        <div class="border-bottom"></div>
        <div>Pembawa</div>
      </div>
      <div class="flex flex-column" style="min-width: 150px">
        <div class="Approver"></div>
        <div class="border-bottom"></div>
        <div>Mengetahui</div>
      </div>
      <div class="flex flex-column" style="min-width: 150px">
        <div class=""></div>
        <div class="border-bottom"></div>
        <div>Penerima</div>
      </div>
    </div>
  </div>

  <script>
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get("id");
    const isTerima = urlParams.get("isTerima");
    const isWarehouse = urlParams.get("isWarehouse");
    const Brand = urlParams.get("Brand");

    $.ajax({
      type: "GET",
      url: "./index.aspx/GetDetailRepair",
      data: {
        IDKRM: id,
        IsWarehouse: isWarehouse,
        Brand: `'${Brand}'`
      },
      contentType: "application/json; charset=utf-8",
      success: function (data) {
        const response = data["d"];
        const header = response["KRM"];
        const detail = response["KRMDetail"];
        const printDate = luxon.DateTime.now()
          .setLocale("id")
          .toFormat("dd/LL/yyyy HH.mm.ss");

        $(".TglCetak").text(printDate);

        Object.entries(header).forEach(([key, value]) => {
          if (key !== "AddrAlamat") $(`.${key}`).html(value);
        });

        // $(".NomorKRM").text(
        //   isTerima == 1 ? header["NomorTRM"] : header["NomorKRM"]
        // );

        // const alamatToBreakSpace = header["AddrAlamat"]
        //   .split("\n")
        //   .join("<br />");

        // $("#AddrAlamat").html(
        //   `(<span>${header["KodeStore"]}</span>)</br><span>${alamatToBreakSpace}</span>`
        // );

        const detailKirimHtml = detail.reduce((accumulator, calculator) => {
          const {
            No,
            IDDocRepair,
            NomorRepair,
            CustomerNama,
            SalesNama,
            PLU,
            ImgPicture,
            Item,
            GrossWeight,
            NettWeight,
            Qty,
          } = calculator;

          accumulator += `
            <tr>
              <td class="text-left">${No}</td>  
              <td class="text-left">${NomorRepair}</td>  
              <td class="text-left">${PLU}</td>  
              <td class="text-left">${Item}</td>  
              <td class="text-right">${Qty}</td>  
              <td class="text-right">${NettWeight}</td>  
              <td class="text-right">${GrossWeight}</td>  
              
            </tr>
          `;
          return accumulator;
        }, ``);

        $("#tbody-paket").append(detailKirimHtml);

        // $("#ImgBefore").attr(
        //   "src",
        //   "/Images.aspx?path=" + header["ImgProduct"]
        // );

        window.print();
      },
      error: function (error) {
        console.error(error);
      },
      complete: function (data) {
        // LOADER.hide();
      },
    });
  </script>
</asp:Content>
