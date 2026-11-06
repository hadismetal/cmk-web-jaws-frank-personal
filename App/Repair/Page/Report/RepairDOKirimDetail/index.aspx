<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="./MasterPage.master" AutoEventWireup="true"
CodeFile="index.aspx.cs" Inherits="DOKRM_index" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
  Report Reparasi
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
  Detail Pengiriman
</asp:Content>

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
  </style>

  <div class="row p-4">
    <div class="col-7">
      <div class="">
        <h1 style="border: none">Repair Information</h1>
        <div class="row">
          <div class="col-6">
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
          <div class="col-6">
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

  <div class="row p-4 mt-5">
    <div class="col-8">
      <h1 style="border: none">List Repair</h1>
      <table class="table table-bordered text-center">
        <thead>
          <tr>
            <th style="width: 75px">No.</th>
            <th>No Repair</th>
            <th>PLU</th>
            <th>Item</th>
            <th>Qty</th>
            <th>Nett Weight</th>
            <th>Gross Weight</th>
            <!-- <th>Image</th> -->
          </tr>
        </thead>
        <tbody id="tbody-paket"></tbody>
      </table>
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
              <td>${No}</td>  
              <td>${NomorRepair}</td>  
              <td>${PLU}</td>  
              <td>${Item}</td>  
              <td>${Qty}</td>  
              <td>${NettWeight}</td>  
              <td>${GrossWeight}</td>  
              
            </tr>
          `;
          return accumulator;
        }, ``);

        $("#tbody-paket").append(detailKirimHtml);

        // $("#ImgBefore").attr(
        //   "src",
        //   "/Images.aspx?path=" + header["ImgProduct"]
        // );
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
