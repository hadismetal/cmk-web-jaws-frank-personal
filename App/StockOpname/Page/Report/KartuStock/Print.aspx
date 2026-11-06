<%@ Page EnableEventValidation="false" Language="C#"
MasterPageFile="./MasterPagePrint.master" AutoEventWireup="true"
CodeFile="Print.aspx.cs"  inherits="Page_Summary" %>

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

    .color-gray gap-rows {
      color: #999;
    }
    .gap-rows{
        padding:0 0;
    }
  </style>

  <div class="container-fluid d-flex justify-content-between px-4 mx-0">    
    <div class="item-flexbox text-center">
      <span style="font-size: 19px; font-weight: bold">&nbsp;BERITA ACARA SERAH TERIMA UANG DAN BARANG &nbsp;</span></br>
      <span id="nomordoc" style="font-size: 19px; font-weight: bold"></span></br>

    </div>    
  </div>

  <div class="border-line-dotted mt-2"></div>

  <div class="row p-2">    
    <div class="col-md-12">        
        <div class="mt-2">        
            <table>
                <tbody>
                    <tr>
                      <td style="width: 50px; font-size:12px" class="color-gray gap-rows">
                          TOKO
                      </td>
                      <td style="width:30px" class="color-gray gap-rows">=</td>
                      <td style="width: 100px" class="toko"></td>
                    </tr>
                    <tr>
                      <td style="width: 50px; font-size:12px" class="color-gray gap-rows">
                          LOKASI
                      </td>
                      <td style="width:30px" class="color-gray gap-rows">=</td>
                      <td style="width: 200px" class="lokasi"></td>                        
                    </tr>
                    <tr>
                      <td style="width: 50px; font-size:12px" class="color-gray gap-rows">
                          SHIFT
                      </td>
                      <td style="width:30px" class="color-gray gap-rows">=</td>
                      <td style="width: 100px" class="shift"></td>
                    </tr>
                </tbody>
            </table>
      </div>
    </div>
    <div class="col-md-12 mt-4">
        <!-- <h4>Pada hari ini  Tanggal :  Jam:, telah diadakan serah terima uang dan barang toko sebagai berikut:</h4> -->
         <h5 style="font-size:12px;">
            Pada hari ini  
            <span id="hari"></span>            
            Jam :
            <span id="jam" ></span>,
            telah diadakan serah terima uang dan barang toko sebagai berikut:
        </h5>
    </div>      
    <div class="col-6 mt-2">
      <div class="mt-2">
        <h4>I. Uang Tunai</h4>
        <table style="margin-left: 1.5em;">
          <tbody>
            <tr>
              <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                A. Kas Rp Operasional
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 100px;font-size:12px" class="PLU"></td>
            </tr>
            <tr>
              <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                B. Kas Rp Penjualan
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 100px;font-size:12px" class="FrameMaterial"></td>
            </tr>
            <tr>
              <td style="width: 200px; font-size:12px;font-weight: bold;" class="color-gray gap-rows">
                TOTAL UANG FISIK
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 100px;font-size:12px" class="KodeKaret"></td>
            </tr>
            <tr></tr>
            <tr>
              <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                A. Kas Rp Operasional(GL)
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 100px;font-size:12px" class="Item"></td>
            </tr>
            <tr>
              <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                B. Kas Rp Penjualan(GL)
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 100px;font-size:12px"></td>
            </tr>
            <tr>
              <td style="width: 200px; font-size:12px;font-weight: bold;" class="color-gray gap-rows">
                TOTAL UANG DI SISTEM
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 100px;font-size:12px" class="KodeKaret"></td>
            </tr>

            <tr>
              <td style="width: 200px; font-size:12px;font-weight: bold;" class="color-gray gap-rows">
                SELISIH
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 100px;font-size:12px" class="SELISIH1"></td>
            </tr>
                        <tr>
              <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                Kas Bon Belum dibukukan
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 100px;font-size:12px" class="Item"></td>
            </tr>
            <tr>
              <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                Kas Penjualan Belum dibukukan
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 100px;font-size:12px">                
                <span class="BeratTimbangActual"></span>
              </td>
            </tr>
            <tr>
              <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                C. Uang Titipan Penjualan LM LEI
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 100px;font-size:12px">
                <span class="BeratTimbangAwal"></span>                
              </td>
            </tr>
          </tbody>
        </table>        
      </div>
    </div>        
    <div class="col-6 mt-2">                  
      <h4>II. BARANG STOCK :</h4>
      <table style="margin-left: 1.5em;">
        <tbody class="bd">
                                                    
        </tbody>
      </table>              
    </div>   
    <div class="col-6 mt-5">
        <h4 class="">III. BARANG TITIPAN</h4>
        <table style="margin-left: 1.5em;">
          <tbody class="bt">
                                  
          </tbody>
        </table>
    </div>
    <div class="col-6 mt-5" style="width: 50%;">        
        <h4 class="">IV. BARANG LUAR SISTEM :</h4>
        <table style="margin-left: 1.5em;">
          <tbody class="bls">
                                 
          </tbody>
        </table>

        <h4 class="mt-3" >V. BARANG SERVICE POINT (KHUSUS TOKO YG ADA TUKANG SERVICE) :</h4>
        <table style="margin-left: 1.5em;">
          <tbody>            
            <tr>
              <td style="width: 200px; font-size:12px;" class="color-gray gap-rows">
                1. Barang Service all toko Palace
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 50px; font-size: 12px;"></td>
            </tr>            
            <tr>
              <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                2. Barang Service all toko Frank & Co
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 50px; font-size: 12px;"></td>
            </tr>
            <tr>
              <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                3. Barang Service all toko Mondial
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 50px; font-size: 12px;"></td>
            </tr>            
          </tbody>
        </table>
    </div>       
    <div class="col-12 mt-4" style="border: 1px solid black;">                        
        <table>
          <tbody>            
            <tr>
              <td style="width: 200px; font-size:12px;font-weight: bold;text-decoration: underline;" class="color-gray gap-rows">
                KETERANGAN :
              </td>              
            </tr>            
            <tr>
              <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                Kas Bon Belum Dibukukan
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 100px" class="VAULT"></td>
            </tr>
            <tr>
              <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                Kas Penjualan Belum Dibukukan
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 100px" class="SIMPANAN"></td>
            </tr>            
            <tr>
              <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                Uang Titipan Penjualan LM LEI
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 100px" class="SIMPANAN"></td>
            </tr>            
            <tr>
              <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                Barang Pinjaman dr toko lain
              </td>
              <td style="width:10px" class="color-gray gap-rows">=</td>
              <td style="width: 100px" class="SIMPANAN"></td>
            </tr>            
          </tbody>
        </table>       
    </div>          
    <div class="col-4 mt-3">
        <tr>
            <td style="width: 150px; font-size:12px" class="black">
                Yang menyerahkan,
                <br><br><br><br><br><br>
            </td>

            <td style="width: 200px; text-align:center; font-size:12px;">
                <div>Nama :</div>
                <div style="border-bottom: 1px solid #000; height: 5px;width: 250px; margin: 2px 0;"></div>
                <div>Jabatan :</div>
            </td>
        </tr>        
    </div>
    <div class="col-4 mt-3">
        <tr>
            <td style="width: 150px; font-size:12px" class="black">
                Yang menerima,
                <br><br><br><br><br><br>
            </td>

            <td style="width: 200px; text-align:center; font-size:12px;">
                <div>Nama :</div>
                <div style="border-bottom: 1px solid #000; height: 5px;width: 250px; margin: 2px 0;"></div>
                <div>Jabatan :</div>
            </td>
        </tr>        
    </div>
    <div class="col-4 mt-3">
        <tr>
            <td style="width: 150px; font-size:12px" class="black">
                Mengetahui,
                <br><br><br><br><br><br>
            </td>

            <td style="width: 200px; text-align:center; font-size:12px;">
                <div>Nama :</div>
                <div style="border-bottom: 1px solid #000; height: 5px;width: 250px; margin: 2px 0;"></div>
                <div>Jabatan :</div>
            </td>
        </tr>        
    </div>
    
</div>


<script>
   
    // function formatNumberToIDR(number) {
    //   const IDR = new Intl.NumberFormat("id-ID", {
    //     style: "currency",
    //     currency: "IDR",
    //   }).format(number);
    //   return IDR;
    // } 

    $(document).ready(()=>{
      const urlParams = new URLSearchParams(window.location.search);
      const id = urlParams.get("id"); 

      getNomorDoc();

      $.ajax({
        type: "GET",
        url: "./Print.aspx/GetDataOpname",
        data: {
          ID: id,
        },
        contentType: "application/json; charset=utf-8",
        success: function (response) {
          const data = response['d'];
          const DataSumDisplay = data['DataSumDisplay']
          const DataSumStock = data['DataSumStock']
          const DataDetailDalam = data['DataDetailDalam']
          const DataDetailTitipan = data['DataDetailTitipan']
          const DataDetailLuarSistem = data['DataDetailLuarSistem']
          const DataSistem = data['DataSistem'];
          let BDishtml='';
          let BTishtml='';
          let BLSishtml='';
          let no=0;
          let noTitipan=0;
          let noLuarSistem=0;
          let SumStock = DataSumStock[0];
          let SumSistem = DataSistem[0];

          BDishtml += DataSumDisplay.reduce((accumulator, calculator, index)=>{
            const {
              IDHeader, IDOpname, Nama, Data
            } = calculator;
            accumulator +=`
            <tr>
                <td style="width: 200px; font-size:12px;" class="color-gray gap-rows">
                  A. ${Nama}
                </td>
                <td style="width:15px" class="color-gray gap-rows"> = </td>
                <td style="width: 100px;font-size:12px;"><smal class="text-right">${Data} Pcs</smal></td>
                </tr>
            `;
            return accumulator;
          }, '')

          BDishtml +=`
              <tr>
                <td style="width: 200px; font-size:12px;" class="color-gray gap-rows">
                  B. Barang DALAM
                </td>              
              </tr>
              `;        

          BDishtml += DataDetailDalam.reduce((accumulator, calculator, index)=>{
              const {
                IDHeader, IDOpname, IDTipeOpname, NamaOpname, NamaDetailOpname, Data
              } = calculator;
              no = no+1;
              accumulator +=`
                <tr>
                  <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                  ${no}. ${NamaDetailOpname}
                  </td>
                  <td style="width:15px" class="color-gray gap-rows">=</td>                
                  <td style="width: 100px;font-size:12px;" class="${NamaDetailOpname}" >${Data} Pcs</td>
                </tr>
              `;
              
              return accumulator
            },''
          );
          
          
          BDishtml +=`
            <tr>
                <td style="width: 200px; font-size:12px; font-weight: bold;" class="color-gray gap-rows">
                  TOTAL BARANG STOCK FISIK
                </td>
                <td style="width:10px" class="color-gray gap-rows">=</td>
                <td style="width: 100px;font-size:12px;font-weight:bold;" class="TOTALFISIK">${SumStock.Data} Pcs</td>
              </tr>
              <tr>
                <td style="width: 200px; font-size:12px;font-weight: bold;" class="color-gray gap-rows">
                  TOTAL BARANG STOCK DI SISTEM
                </td>
                <td style="width:10px" class="color-gray gap-rows">=</td>
                <td style="width: 100px;font-size:12px;font-weight: bold;" class="TOTALSISTEM">${SumSistem.Data} Pcs</td>
              </tr>
              <tr>
                <td style="width: 200px; font-size:12px;font-weight: bold;" class="color-gray gap-rows">
                  SELISIH
                </td>
                <td style="width:10px" class="color-gray gap-rows">=</td>
                <td style="width: 100px;font-size:12px;font-weight: bold;" class="SELISIH2">${SumStock.Data - SumSistem.Data} Pcs</td>
              </tr>
          `;      
          BTishtml += DataDetailTitipan.reduce(
            (accumulator, calculator, index)=>{
              const {
                IDHeader, IDOpname, IDTipeOpname, NamaOpname, NamaDetailOpname, Data
              } = calculator;
              noTitipan = noTitipan+1;
              accumulator +=`
                <tr>
                  <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                  ${noTitipan}. ${NamaDetailOpname}
                  </td>
                  <td style="width:15px" class="color-gray gap-rows">=</td>
                  <td style="width: 100px;font-size:12px;" class="${NamaDetailOpname}" >${Data} Pcs</td>                
                </tr>
              `;
              return accumulator;
            },''
          );
          BTishtml +=`
            <tr>
                <td style="width: 200px; font-size:12px; font-weight: bold;" class="color-gray gap-rows">
                  TOTAL BARANG TITIPAN FISIK
                </td>
                <td style="width:10px" class="color-gray gap-rows">=</td>
                <td style="width: 100px;font-size:12px;font-weight:bold;" class="TOTALFISIK"></td>
              </tr>
              <tr>
                <td style="width: 200px; font-size:12px;font-weight: bold;" class="color-gray gap-rows">
                  TOTAL BARANG TITIPAN DI SISTEM
                </td>
                <td style="width:10px" class="color-gray gap-rows">=</td>
                <td style="width: 100px;font-size:12px;font-weight: bold;" class="TOTALSISTEM"></td>
              </tr>
              <tr>
                <td style="width: 200px; font-size:12px;font-weight: bold;" class="color-gray gap-rows">
                  SELISIH
                </td>
                <td style="width:10px" class="color-gray gap-rows">=</td>
                <td style="width: 100px;font-size:12px;font-weight: bold;" class="SELISIH2"></td>
              </tr>
          `;
          BLSishtml += DataDetailLuarSistem.reduce(
            (accumulator, calculator, index)=>{
              const {
                IDHeader, IDOpname, IDTipeOpname, NamaOpname, NamaDetailOpname, Data
              } = calculator;
              noLuarSistem = noLuarSistem+1;
              accumulator +=`
                <tr>
                  <td style="width: 200px; font-size:12px" class="color-gray gap-rows">
                  ${noLuarSistem}. ${NamaDetailOpname}
                  </td>
                  <td style="width:15px" class="color-gray gap-rows">=</td>
                  <td style="width: 100px;font-size:12px;" class="${NamaDetailOpname}" >${Data} Pcs</td>                
                </tr>
              `;
              return accumulator;
            },''
          );        




          $('#hari').html(SumStock.Approval_at)
          $('#jam').html(SumStock.TimeLine)
          $('.toko').html(SumStock.KodeLokasi)
          $('.lokasi').html(SumStock.NamaLokasi)
          $('.shift').html(SumStock.NamaShift)
          $('.bd').append(BDishtml);
          $('.bt').append(BTishtml);
          $('.bls').append(BLSishtml);
    
        
        },
        error: function (error) {
          console.error(error);
        },
        complete: function (data) {},
      });
      function getNomorDoc(){
        $.ajax({
          type: "GET",
          url: "./Print.aspx/GetNomorDoc",
          contentType: "application/json; charset=utf-8",
          success: function (response) {
            const data = response['d'];
            $('#nomordoc').html(`&nbsp;${data}&nbsp;`);
          },
          error: function (error) {
            console.error(error);
          },
          complete: function (data) {},
        });
      }
    });
  </script> 
</asp:Content>
