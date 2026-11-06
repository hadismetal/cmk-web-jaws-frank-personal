<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="KartuStock.aspx.cs" inherits="Page_kartustock" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
  Register 
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
  Kartu Stock
</asp:Content>

<asp:content contentplaceholderid="content" runat="server">
    <style>
        input[type=number]::-webkit-inner-spin-button, input[type=number]::-webkit-outer-spin-button { -webkit-appearance: none; margin: 0; } /* Firefox */ input[type=number] { -moz-appearance: textfield;}
        .table-container {
            max-height: 500px;  
            overflow-y: auto;
            overflow-x: auto;
            border: 1px solid #ccc;
        }

        .table-list {
            border-collapse: separate;
            border-spacing: 0;
            width: 100%;
            table-layout: fixed;
        }

        .table-list .freeze-row th {
            position: sticky;
            top: 0;
            background-color: #f1f1f1;
            z-index: 2;
            padding: 10px;
        }

        .table-list .freeze-column {
            position: sticky;
            left: 0;
            background-color: #f1f1f1;
            z-index: 3;
        }
        .table-list .freeze-row .freeze-column {
            z-index: 4;  /* Higher z-index for intersection cell */
            background-color: #e0e0e0;  /* Distinguish the top-left cell */
        }

        /* Sticky Footer */
        .table-list .freeze-footer {
            position: sticky;
            bottom: 0;  /* Stick to the bottom */
            background-color: #f9f9f9;
            z-index: 4;
            border-top: 1px solid #ddd;  /* Add border on top of the footer */
        }

        /* Footer cells with frozen first column */
        .table-list .freeze-footer .freeze-column {
            z-index: 7;  /* Higher z-index for footer's frozen first column */
            background-color: #e0e0e0;
        }
        
        .input-table {
            width: 50px;
            height: 32px;
            text-align: center;
            padding: 0;
            border: 1px solid #ced4da;
            border-radius: 6px;
        }       
    </style>
    
    <div id="loader">Harap Tunggu, Sedang Menyimpan Data</div>    
    
    <div id="stock-opname">
        
	<form runat="server" class="form-inline">									
        <div class="row">
            <div class="col-md-6">
                <div class="form-group">
                    <div>Kode Toko : <span id="kodetoko" runat="server"></span></div>
                </div>
            </div>            
            <div class="col-md-6">
                <div class="form-group">
                    <div>Tanggal : <span id="tanggal" runat="server"></span></div>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <div>Nama : <span id="NamaAE" runat="server"></span></div>
                </div>
            </div>

            <div class="col-md-6">
                <div class="form-group">
                    <div>Lokasi : <span id="lokasi" runat="server"></span></div>						
                    <input type="text" style="display: none;" value="" id="IDLokasi" runat="server" />
                    <input type="text" style="display: none;" value="" id="IDUser" runat="server" />
                    <input type="text" style="display: none;" value="" id="IDDisplay" runat="server" />
                    <input type="text" style="display: none;" value="" id="locType" runat="server" />
                    <input type="text" style="display: none;" value="" id="KodeShift" />

                </div>
            </div>            
            <div class="col-md-6">                
                <div class="d-flex justify-content-evenly">
                    <p>DJ = <span style="background-color: #FCDCE1;border: solid 1px black;">&nbsp;&nbsp;&nbsp;&nbsp;</span></p>
                    <p>PG = <span style="background-color: #E9ECCE;border: solid 1px black;">&nbsp;&nbsp;&nbsp;&nbsp;</span></p>
                    <p>LD = <span style="background-color: #CDE9DC;border: solid 1px black;">&nbsp;&nbsp;&nbsp;&nbsp;</span></p>
                </div>
            </div>
            <div class="col-md-6">
                <div class="form-group">
                    <div>Shift : <span id="Shift" runat="server"></span></div>
                </div>
            </div>                                    
        </div>
	</form>
    <p>
        <a class="btn btn-outline-primary " data-bs-toggle="collapse" href="#multiCollapseExample1" role="button" aria-expanded="false" aria-controls="multiCollapseExample1">Barang Display</a>        
        <!-- <button class="btn btn-primary" type="button" data-bs-toggle="collapse" data-bs-target=".multi-collapse" aria-expanded="false" aria-controls="multiCollapseExample1 multiCollapseExample2 multiCollapseExample3 multiCollapseExample4">Lihat Semua</button> -->
        <h1>Barang Display</h1>
        <!-- <button class="btn btn-outline-primary " type="button" data-bs-toggle="collapse" data-bs-target=".multi-collapse" aria-expanded="false" aria-controls="multiCollapseExample1 multiCollapseExample2 multiCollapseExample3">Tampilkan Semua</button> -->
    </p>

    
    <div class="collapse multi-collapse" id="multiCollapseExample1">         
        <div class="table-responsive table-container">
            <asp:Table id="tb" runat="server" cssclass="table-list text-sm freeze-table">
                <asp:TableRow class="freeze-row">
                    <asp:TableHeaderCell class="freeze-column" RowSpan="2" Width="110">Barang Display</asp:TableHeaderCell>
                    <asp:TableHeaderCell ColumnSpan="12" Width="800">DJ</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="75">Total DJ</asp:TableHeaderCell>
                    <asp:TableHeaderCell ColumnSpan="12" Width="800">PG</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="75">Total PG</asp:TableHeaderCell>
                    <asp:TableHeaderCell  Width="80">LD</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="75">Total LD</asp:TableHeaderCell>
                    <asp:TableHeaderCell RowSpan="2" Width="75">Total ALL</asp:TableHeaderCell>                
                </asp:TableRow>
                <asp:TableRow class="freeze-row">
                    <asp:TableHeaderCell style="text-align: center;width:150px">BGL</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">BRC</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">EAR</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">LR</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">BUCK</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">MJ</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">NECK</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">BRO</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">PDT</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">P + C</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">WR</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">OTH</asp:TableHeaderCell>

                    <asp:TableHeaderCell style="text-align: center;width:150px">BGL</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">BRC</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">EAR</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">LR</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">ANK</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">MJ</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">NECK</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">CH</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">PDT</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">P + C</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">WR</asp:TableHeaderCell>
                    <asp:TableHeaderCell style="text-align: center;width:150px">OTH</asp:TableHeaderCell>
                    
                    <asp:TableHeaderCell style="text-align: center;width:150px">OTH</asp:TableHeaderCell>                
                </asp:TableRow>                       
                
        </asp:Table>
        </div>
    </div>

    <span class="mt-3" id="btnBD">
        <button class="btn btn-outline-primary " type="button" data-bs-toggle="collapse" data-bs-target="#multiCollapseExample2" aria-expanded="false" aria-controls="multiCollapseExample2">Barang Dalam</button>
        <h1>Barang Dalam</h1>
    </span>
    <div class="collapse multi-collapse" id="multiCollapseExample2">
        <div class="table-responsive table-container">
            <asp:Table id="tb1" runat="server" cssclass="table-list text-sm freeze-table BS">                    
                        <asp:TableRow class="freeze-row">
                        <asp:TableHeaderCell class="freeze-column" RowSpan="2" Width="150">Barang Dalam</asp:TableHeaderCell>
                        <asp:TableHeaderCell ColumnSpan="12" Width="1000">DJ</asp:TableHeaderCell>
                        <asp:TableHeaderCell RowSpan="2" Width="75">Total DJ</asp:TableHeaderCell>
                        <asp:TableHeaderCell ColumnSpan="12" Width="1000">PG</asp:TableHeaderCell>
                        <asp:TableHeaderCell RowSpan="2" Width="75">Total PG</asp:TableHeaderCell>
                        <asp:TableHeaderCell RowSpan="2" Width="100">LD</asp:TableHeaderCell>
                        <asp:TableHeaderCell RowSpan="2" Width="75">Total LD</asp:TableHeaderCell>
                        <asp:TableHeaderCell RowSpan="2" Width="75">Total ALL</asp:TableHeaderCell>                
                    </asp:TableRow>
                    <asp:TableRow class="freeze-row">
                        <asp:TableHeaderCell style="text-align: center;width:250px">BGL</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">BRC</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">EAR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">LR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">BUCK</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">MJ</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">NECK</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">BRO</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">PDT</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">P + C</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">WR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">OTH</asp:TableHeaderCell>

                        <asp:TableHeaderCell style="text-align: center;width:250px">BGL</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">BRC</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">EAR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">LR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">ANK</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">MJ</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">NECK</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">CH</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">PDT</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">P + C</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">WR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">OTH</asp:TableHeaderCell>
                                        
                    </asp:TableRow>                       
                    

                    
            </asp:Table>
        </div>
    </div>  
    
    <span class="mt-3" id="btnBT">
        <button class="btn btn-outline-primary " type="button" data-bs-toggle="collapse" data-bs-target="#multiCollapseExample3" aria-expanded="false" aria-controls="multiCollapseExample3">Barang Titipan</button>
        <h1>Barang Titipan</h1>
    </span>
    <div class="collapse multi-collapse" id="multiCollapseExample3">
        <div class="table-responsive table-container">
            <asp:Table id="tb2" runat="server" cssclass="table-list text-sm freeze-table BT">
                    
                        <asp:TableRow class="freeze-row">
                        <asp:TableHeaderCell class="freeze-column" RowSpan="2" Width="150">Barang Titipan</asp:TableHeaderCell>
                        <asp:TableHeaderCell ColumnSpan="12" Width="1000">DJ</asp:TableHeaderCell>
                        <asp:TableHeaderCell RowSpan="2" Width="75">Total DJ</asp:TableHeaderCell>
                        <asp:TableHeaderCell ColumnSpan="12" Width="1000">PG</asp:TableHeaderCell>
                        <asp:TableHeaderCell RowSpan="2" Width="75">Total PG</asp:TableHeaderCell>
                        <asp:TableHeaderCell RowSpan="2" Width="100">LD</asp:TableHeaderCell>
                        <asp:TableHeaderCell RowSpan="2" Width="75">Total LD</asp:TableHeaderCell>
                        <asp:TableHeaderCell RowSpan="2" Width="75">Total ALL</asp:TableHeaderCell>                
                    </asp:TableRow>
                    <asp:TableRow class="freeze-row">
                        <asp:TableHeaderCell style="text-align: center;width:250px">BGL</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">BRC</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">EAR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">LR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">BUCK</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">MJ</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">NECK</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">BRO</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">PDT</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">P + C</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">WR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">OTH</asp:TableHeaderCell>

                        <asp:TableHeaderCell style="text-align: center;width:250px">BGL</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">BRC</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">EAR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">LR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">ANK</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">MJ</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">NECK</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">CH</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">PDT</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">P + C</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">WR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">OTH</asp:TableHeaderCell>
                                        
                    </asp:TableRow>                       
                    

                    
            </asp:Table>
        </div>
    </div> 

    <span class="mt-3" id="btnBDS">
        <button class="btn btn-outline-primary " type="button" data-bs-toggle="collapse" data-bs-target="#multiCollapseExample4" aria-expanded="false" aria-controls="multiCollapseExample4">Barang Diluar Sistem</button>
        <h1>Barang Diluar Sistem</h1>
    </span>
    <div class="collapse multi-collapse" id="multiCollapseExample4">
        <div class="table-responsive table-container">
            <asp:Table id="tb3" runat="server" cssclass="table-list text-sm freeze-table BL">
                    
                        <asp:TableRow class="freeze-row">
                        <asp:TableHeaderCell class="freeze-column" RowSpan="2" Width="150">Barang Diluar Sistem</asp:TableHeaderCell>
                        <asp:TableHeaderCell ColumnSpan="12" Width="1000">DJ</asp:TableHeaderCell>
                        <asp:TableHeaderCell RowSpan="2" Width="75">Total DJ</asp:TableHeaderCell>
                        <asp:TableHeaderCell ColumnSpan="12" Width="1000">PG</asp:TableHeaderCell>
                        <asp:TableHeaderCell RowSpan="2" Width="75">Total PG</asp:TableHeaderCell>
                        <asp:TableHeaderCell RowSpan="2" Width="100">LD</asp:TableHeaderCell>
                        <asp:TableHeaderCell RowSpan="2" Width="75">Total LD</asp:TableHeaderCell>
                        <asp:TableHeaderCell RowSpan="2" Width="75">Total ALL</asp:TableHeaderCell>                
                    </asp:TableRow>
                    <asp:TableRow class="freeze-row">
                        <asp:TableHeaderCell style="text-align: center;width:250px">BGL</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">BRC</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">EAR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">LR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">BUCK</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">MJ</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">NECK</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">BRO</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">PDT</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">P + C</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">WR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">OTH</asp:TableHeaderCell>

                        <asp:TableHeaderCell style="text-align: center;width:250px">BGL</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">BRC</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">EAR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">LR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">ANK</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">MJ</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">NECK</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">CH</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">PDT</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">P + C</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">WR</asp:TableHeaderCell>
                        <asp:TableHeaderCell style="text-align: center;width:250px">OTH</asp:TableHeaderCell>
                                        
                    </asp:TableRow>                       
                    

                    
            </asp:Table>
        </div>
    </div>

    <div class="form-group">
		<div class="hidden-xs">&nbsp;</div>
		<div>
            
			<button class="btn btn-primary" 
                data-bs-toggle="modal"
                data-bs-target="#modalConfirmation"
                id="btn-modal-confirmation">
				<i class="icon-search"></i>
				Submit
			</button>
            &nbsp;                
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
            <p>Pastikan data yang tertera masukkan sudah benar!</p>
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
    <script type="text/javascript">
        $(document).ready(()=>{                   
            toastr.options = {
				closeButton: true,
				progressBar: true,
				positionClass: "toast-bottom-center",
				showDuration: "300",
				hideDuration: "1000",
				timeOut: "5000",
			};            



            function showLoader() {
                document.getElementById("loader").style.display = "grid";
                document.getElementById("stock-opname").style.display = "none";
            }

            function hideLoader() {
                document.getElementById("loader").style.display = "none";
                document.getElementById("stock-opname").style.display = "block";
            }
            
            
            const urlParams = new URLSearchParams(window.location.search);
            const Shift = urlParams.get("shift");            
            const IDUser = "<%= App.UserID%>";
            

            $('#KodeShift').val(Shift)
            let BUTTON_SUBMIT = $('#btn-confirmation-submit');
            getUser(IDUser, Shift);


            BUTTON_SUBMIT.on('click',(e)=>{                
                let DataBDisplay = [];
                let DataBDalam = [];
                let DataBTitipan = [];
                let DataBLuarSistem = []
                showLoader();
                $('#btn-modal-confirmation').attr('disabled',true)

                let IDLokasi = document.getElementById("<%= IDLokasi.ClientID %>").value;
                let TipeLokasi = document.getElementById("<%= locType.ClientID %>").value;
                let IDUser = document.getElementById("<%= IDUser.ClientID %>").value;
                let KodeShift = document.getElementById("KodeShift").value;
                let BarangDisplay = document.querySelectorAll('.BD tfoot tr input');
                let BarangDalam = document.querySelectorAll('.BS tfoot tr input');
                let BarangTitipan = document.querySelectorAll('.BT tfoot tr input');
                let BarangLuarSistem = document.querySelectorAll('.BL tfoot tr input');                

                let grouped = {};                 
                
                BarangDisplay.forEach((e, i) => {
                    let el = $(e);                
                    let row = el.data('row');
                    if(row){
                        let rowNumber = row.substring(2); // get everything after first 2 chars
                        let value = el.val();                   // e.g. 1
                        let id = el.attr('id');                                                   
                        let finalKey = id.replace(/\d+$/, "");

                        if (!row.includes('ALL')) {
                            let existing = DataBDisplay.find(x => x.nomor == rowNumber);
                            if (existing) {                                
                                existing[finalKey] = value;
                            } else {                                
                                DataBDisplay.push({ nomor: rowNumber,tipe:'BD', [finalKey]: value });
                            }
                        }
                    }
                });
            
                
                BarangDalam.forEach((e, i) => {
                    let el = $(e);                
                    let row = el.data('row');                    
                    if(row){                 
                        let rowNumber = row.substring(2);
                        let value = el.val();
                        let id = el.attr('id');
                        let finalKey = id.slice(2);
                        if (!(row.toUpperCase()).includes('TOTAL')) {
                            let existing = DataBDalam.find(x => x.nomor == rowNumber);
                            if (existing) {                                
                                existing[finalKey] = value;
                            } else {                                
                                DataBDalam.push({ nomor: rowNumber,tipe:'BS', [finalKey]: value });
                            }
                        }
                    }
                });

                BarangTitipan.forEach((e, i) => {
                    let el = $(e);                
                    let row = el.data('row');                    
                    if(row){                 
                        let rowNumber = row.substring(2);
                        let value = el.val();
                        let id = el.attr('id');
                        let finalKey = id.slice(2);
                        if (!(row.toUpperCase()).includes('TOTAL')) {
                            let existing = DataBTitipan.find(x => x.nomor == rowNumber);
                            if (existing) {                                
                                existing[finalKey] = value;
                            } else {                                
                                DataBTitipan.push({ nomor: rowNumber,tipe:'BT', [finalKey]: value });
                            }
                        }
                    }
                });

                BarangLuarSistem.forEach((e, i) => {
                    let el = $(e);                
                    let row = el.data('row');                    
                    if(row){                 
                        let rowNumber = row.substring(2);
                        let value = el.val();
                        let id = el.attr('id');
                        let finalKey = id.slice(2);
                        if (!(row.toUpperCase()).includes('TOTAL')) {
                            let existing = DataBLuarSistem.find(x => x.nomor == rowNumber);
                            if (existing) {                                
                                existing[finalKey] = value;
                            } else {                                
                                DataBLuarSistem.push({ nomor: rowNumber,tipe:'BL', [finalKey]: value });
                            }
                        }
                    }                    
                });

                const DataOpname = {
                    TipeLokasi: TipeLokasi,
                    IDLokasi : IDLokasi,
                    KodeShift : KodeShift,
                    IDUser : IDUser,
                    BarangDisplay: DataBDisplay,
                    BarangDalam: DataBDalam,
                    BarangTitipan: DataBTitipan,
                    BarangLuarSistem: DataBLuarSistem
                }                           
                 $.ajax({
                     type: "POST",
                     url: "./KartuStock.aspx/InsertDataOpname",
                     data: JSON.stringify({ IDataOpname: DataOpname}),
                     contentType: "application/json; charset=utf-8",
                     dataType: "json",
                     success: function (response) {                    
                         let data = response['d'];
                         let datas = data[0];
                         console.log(data)
                         if (datas.Status == 200) {
                             toastr.success(`Success Insert Data Opname`);
                             hideLoader();
                             setTimeout(() => {
                             window.close();
                             }, 5000);
                         } else {
                             hideLoader();
                             console.log(datas.Message)
                             toastr.error(`Error saat menyimpan data : harap hubungi IT`);
                             $('#btn-modal-confirmation').attr('disabled',false)
                         }                    
                     },
                     error: function (xhr, status, error) {   
                         hideLoader();                 
                         toastr.error(`Error Get Data : ${error}`);                    
                         $('#btn-modal-confirmation').attr('disabled',false)
                     }
                 });
                showLoader();
                /* setTimeout(()=>{
                    hideLoader()
                }, 5000); */
                

            })
        })
        

     var counters = {
        'front':0,
        'A': 0,
        'B': 0,
        'C': 0,
        'D': 0
    };
    addRowFrontDisplay1();
    function getUser(UserID, Shift){

        var payload = {
            UserID: UserID,
            shift : Shift
        };
        
        $.ajax({
                type: "GET",
                url: "./KartuStock.aspx/GetUserData",
                data: payload,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {                    
                    let data = response['d'];
                    
                    if (data == 2) {
                        $('#btnBDS').attr("style","display:none;")
                        $('#btnBT').attr("style","display:none;")
                        $('#btnBD').attr("style","display:none;")                    
                    }
                },
                error: function (xhr, status, error) {                    
                    toastr.error(`Error Get Data : ${error}`);
                    
                }
            });

    }
    
    function addRowFrontDisplay1(){
        const urlParams = new URLSearchParams(window.location.search);
        let IDLokasi = document.getElementById("<%= IDLokasi.ClientID %>").value;
        let dataDisplay = document.getElementById("<%= IDDisplay.ClientID %>").value;  
        var payload = {
            IDLokasi: IDLokasi,
            Display: dataDisplay,
            UserType : parseInt(urlParams.get("typeUser")) 
        };
        
        $.ajax({
                type: "POST",
                url: "./KartuStock.aspx/GetDisplayFront",
                data: JSON.stringify(payload),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {                    
                    let data = response['d'];
                    data.forEach(item => {
                        addRowFrontDisplay(item.ID, item.NamaDisplay);
                        counters['front']++;
                    });
                    
                },
                error: function (xhr, status, error) {                    
                    toastr.error(`Error Get Data : ${error}`);
                    
                }
            });

    }

    function addRowFrontDisplay(IDTable,NamaDisplay) {                
        var table = document.getElementById('<%= tb.ClientID %>');
        var type="front";
        
        // Insert a new row at index 2 (just behind the second header row)
        var newRow = table.insertRow(2);  // Insert after the second header row (index 2)
        table.classList.add(`BD`);        
        
        // Increment the counter for the given type    
        
        // Create a cell for Barang Display and add data
        var cell1 = newRow.insertCell(0);
        cell1.classList.add("freeze-column");
        cell1.innerHTML = `<div class="text-center" style="font-weight:bold;">${NamaDisplay}</div>`;  // Example: 'Barang A1', 'Barang B1'
                
        // Add cells for the other columns (for simplicity, using dummy data here)
        var cell = newRow.insertCell(1);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#FCDCE1";        
        cell.innerHTML = ` <div class="text-center"><input type="text" inputmode="numeric" min="0" data-tipe="DJBGL" oninput="sanitize(this)" onpaste="return false;" data-row="DJ${IDTable}" onchange="kalkulasi('DJBGL','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="DJBGL${IDTable}" /></div>`;
    
        var cell = newRow.insertCell(2);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#FCDCE1";        
        cell.innerHTML = `<div class="text-center" ><input type="text" inputmode="numeric" min="0" data-tipe="DJBRC" oninput="sanitize(this)" onpaste="return false;" data-row="DJ${IDTable}" onchange="kalkulasi('DJBRC','${IDTable}','${type}')" value="0" class="input-table  BarangDisplay" id="DJBRC${IDTable}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(3);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#FCDCE1";        
        cell.innerHTML = `<div class="text-center" ><input type="text" inputmode="numeric" min="0" data-tipe="DJEAR" oninput="sanitize(this)" onpaste="return false;" data-row="DJ${IDTable}" onchange="kalkulasi('DJEAR','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="DJEAR${IDTable}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(4);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#FCDCE1";        
        cell.innerHTML = `<div class="text-center" ><input type=textr inputmode="numeric"" min="0" data-tipe="DJLR" oninput="sanitize(this)" onpaste="return false;" data-row="DJ${IDTable}" onchange="kalkulasi('DJLR','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="DJLR${IDTable}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(5);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#FCDCE1";        
        cell.innerHTML = `<div class="text-center" ><input type="ntext  inputmode="numeric"min="0" data-tipe="DJBUCK" oninput="sanitize(this)" onpaste="return false;" data-row="DJ${IDTable}" onchange="kalkulasi('DJBUCK','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="DJBUCK${IDTable}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(6);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#FCDCE1";        
        cell.innerHTML = `<div class="text-center" ><input type=textr inputmode="numeric"" min="0" data-tipe="DJMJ" oninput="sanitize(this)" onpaste="return false;" data-row="DJ${IDTable}" onchange="kalkulasi('DJMJ','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="DJMJ${IDTable}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(7);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#FCDCE1";        
        cell.innerHTML = `<div class="text-center" ><input type="ntext  inputmode="numeric"min="0" data-tipe="DJNECK" oninput="sanitize(this)" onpaste="return false;" data-row="DJ${IDTable}" onchange="kalkulasi('DJNECK','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="DJNECK${IDTable}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(8);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#FCDCE1";        
        cell.innerHTML = `<div class="text-center" ><input type="text" inputmode="numeric" min="0" data-tipe="DJBRO" oninput="sanitize(this)" onpaste="return false;" data-row="DJ${IDTable}" onchange="kalkulasi('DJBRO','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="DJBRO${IDTable}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(9);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#FCDCE1";        
        cell.innerHTML = `<div class="text-center" ><input type="text" inputmode="numeric" min="0" data-tipe="DJPDT" oninput="sanitize(this)" onpaste="return false;" data-row="DJ${IDTable}" onchange="kalkulasi('DJPDT','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="DJPDT${IDTable}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(10);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#FCDCE1";        
        cell.innerHTML = `<div class="text-center" ><input type="ntext  inputmode="numeric"min="0" data-tipe="DJPDTC" oninput="sanitize(this)" onpaste="return false;" data-row="DJ${IDTable}" onchange="kalkulasi('DJPDTC','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="DJPDTC${IDTable}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(11);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#FCDCE1";        
        cell.innerHTML = `<div class="text-center" ><input type=textr inputmode="numeric"" min="0" data-tipe="DJWR" oninput="sanitize(this)" onpaste="return false;" data-row="DJ${IDTable}" onchange="kalkulasi('DJWR','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="DJWR${IDTable}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(12);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#FCDCE1";        
        cell.innerHTML = `<div class="text-center" ><input type="text" inputmode="numeric" min="0" data-tipe="DJOTH" oninput="sanitize(this)" onpaste="return false;" data-row="DJ${IDTable}" onchange="kalkulasi('DJOTH','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="DJOTH${IDTable}"/></div>`; // Replace with actual data as needed
    
        // Add a cell for Total DJ (example data)
        var totalDJCell = newRow.insertCell(13);
        totalDJCell.innerHTML = `<div class="text-center"><input type="number" min="0" value="0" class="input-table" readonly data-column="totalDJ"  id="totalDJ_${IDTable}"/></div>`;
    
        // Add cells for the other columns in Total DJ section
        var cell = newRow.insertCell(14);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#E9ECCE";
        cell.innerHTML = `<div class="text-center"><input type="text" inputmode="numeric" min="0" data-tipe="PGBGL" oninput="sanitize(this)" onpaste="return false;" data-row="PG${IDTable}" onchange="kalkulasi('PGBGL','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="PGBGL${IDTable}"/></div>`; // Replace with actual data as needed                    
    
        var cell = newRow.insertCell(15);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#E9ECCE";
        cell.innerHTML = `<div class="text-center"><input type="text" inputmode="numeric" min="0" data-tipe="PGBRC" oninput="sanitize(this)" onpaste="return false;" data-row="PG${IDTable}" onchange="kalkulasi('PGBRC','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="PGBRC${IDTable}"/></div>`; // Replace with actual data as needed                
    
        var cell = newRow.insertCell(16);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#E9ECCE";
        cell.innerHTML = `<div class="text-center"><input type="text" inputmode="numeric" min="0" data-tipe="PGEAR" oninput="sanitize(this)" onpaste="return false;" data-row="PG${IDTable}" onchange="kalkulasi('PGEAR','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="PGEAR${IDTable}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(17);  // Insert in the next columns                            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#E9ECCE";
        cell.innerHTML = `<div class="text-center"><input type=textr inputmode="numeric"" min="0" data-tipe="PGLR" oninput="sanitize(this)" onpaste="return false;" data-row="PG${IDTable}" onchange="kalkulasi('PGLR','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="PGLR${IDTable}"/></div>`; // Replace with actual data as needed
            
        var cell = newRow.insertCell(18);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#E9ECCE";
        cell.innerHTML = `<div class="text-center"><input type="ntext  inputmode="numeric"min="0" data-tipe="PGBUCK" oninput="sanitize(this)" onpaste="return false;" data-row="PG${IDTable}" onchange="kalkulasi('PGBUCK','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="PGBUCK${IDTable}"/></div>`; // Replace with actual data as needed
           
        var cell = newRow.insertCell(19);  // Insert in the next columns                     
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#E9ECCE";
        cell.innerHTML = `<div class="text-center"><input type=textr inputmode="numeric"" min="0" data-tipe="PGMJ" oninput="sanitize(this)" onpaste="return false;" data-row="PG${IDTable}" onchange="kalkulasi('PGMJ','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="PGMJ${IDTable}"/></div>`; // Replace with actual data as needed        
    
        var cell = newRow.insertCell(20);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#E9ECCE";
            
        cell.innerHTML = `<div class="text-center"><input type="ntext  inputmode="numeric"min="0" data-tipe="PGNECK" oninput="sanitize(this)" onpaste="return false;" data-row="PG${IDTable}" onchange="kalkulasi('PGNECK','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="PGNECK${IDTable}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(21);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#E9ECCE";
        cell.innerHTML = `<div class="text-center"><input type="text" inputmode="numeric" min="0" data-tipe="PGBRO" oninput="sanitize(this)" onpaste="return false;" data-row="PG${IDTable}" onchange="kalkulasi('PGBRO','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="PGBRO${IDTable}"/></div>`; // Replace with actual data as needed        
    
        var cell = newRow.insertCell(22);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#E9ECCE";
        cell.innerHTML = `<div class="text-center"><input type="text" inputmode="numeric" min="0" data-tipe="PGPDT" oninput="sanitize(this)" onpaste="return false;" data-row="PG${IDTable}" onchange="kalkulasi('PGPDT','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="PGPDT${IDTable}"/></div>`; // Replace with actual data as needed                
    
        var cell = newRow.insertCell(23);  // Insert in the next columns                    
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#E9ECCE";
        cell.innerHTML = `<div class="text-center"><input type="ntext  inputmode="numeric"min="0" data-tipe="PGPDTC" oninput="sanitize(this)" onpaste="return false;" data-row="PG${IDTable}" onchange="kalkulasi('PGPDTC','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="PGPDTC${IDTable}"/></div>`; // Replace with actual data as needed        
    
        var cell = newRow.insertCell(24);  // Insert in the next columns                    
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#E9ECCE";
        cell.innerHTML = `<div class="text-center"><input type=textr inputmode="numeric"" min="0" data-tipe="PGWR" oninput="sanitize(this)" onpaste="return false;" data-row="PG${IDTable}" onchange="kalkulasi('PGWR','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="PGWR${IDTable}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(25);  // Insert in the next columns            
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#E9ECCE";
        cell.innerHTML = `<div class="text-center"><input type="text" inputmode="numeric" min="0" data-tipe="PGOTH" oninput="sanitize(this)" onpaste="return false;" data-row="PG${IDTable}" onchange="kalkulasi('PGOTH','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="PGOTH${IDTable}"/></div>`; // Replace with actual data as needed        
    
        // Add a cell for Total PG
        var totalPGCell = newRow.insertCell(26);
        totalPGCell.innerHTML = `<div class="text-center"><input type="number" min="0"  value="0" class="input-table" readonly data-column="totalPG" id="totalPG_${IDTable}"/></div>`;
    
        // Add cells for the other columns in LD section
        var cell = newRow.insertCell(27);  // Insert in the next columns
        cell.classList.add("text-center");
        cell.style.backgroundColor = "#CDE9DC";        
        cell.innerHTML = `<div class="text-center"><input type="text" inputmode="numeric" min="0" data-tipe="LDOTH" oninput="sanitize(this)" onpaste="return false;" data-row="LD${IDTable}" onchange="kalkulasi('LDOTH','${IDTable}','${type}')" value="0" class="input-table BarangDisplay" id="LDOTH${IDTable}"/></div>`; // Replace with actual data as needed

        // Add a cell for Total LD
        var totalLDCell = newRow.insertCell(28);
        totalLDCell.innerHTML =innerHTML = `<div class="text-center"><input type="number" min="0"  value="0" class="input-table" readonly data-column="totalLD" id="totalLD_${IDTable}"/></div>`;
    
        // Add a cell for Total ALL
        var totalALLCell = newRow.insertCell(29);
        totalALLCell.innerHTML = innerHTML = `<div class="text-center"><input type="number" min="0"  value="0" class="input-table" data-row="ALL" readonly id="totalALL_${IDTable}"/></div>`;
        var totalALLCell2 = newRow.insertCell(30);
        totalALLCell2.innerHTML = '';
    }

    function addRowToTable(type) {
        // Get the table element
        var table = document.getElementById('<%= tb.ClientID %>');

        // Insert a new row at index 2 (just behind the second header row)
        var newRow = table.insertRow(3);  // Insert after the second header row (index 2)

        // Increment the counter for the given type
        counters[type]++;

        // Create a cell for Barang Display and add data
        var cell1 = newRow.insertCell(0);
        cell1.classList.add("freeze-column");
        cell1.innerHTML = `<div class="text-center" style="font-weight:bold;">${type}${counters[type]}</div>`;  // Example: 'Barang A1', 'Barang B1'

        // Add cells for the other columns (for simplicity, using dummy data here)
        var cell = newRow.insertCell(1);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><intextp inputmode="numeric"e="text" data-tipe="DJBGL" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('DJBGL','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="DJBGL${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(2);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><intextp inputmode="numeric"e="text" data-tipe="DJBRC" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('DJBRC','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="DJBRC${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(3);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><intextp inputmode="numeric"e="text" data-tipe="DJEAR" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('DJEAR','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="DJEAR${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(4);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><itexty inputmode="numeric"pe="text" data-tipe="DJLR" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('DJLR','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="DJLR${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(5);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><inptexte inputmode="numeric"="text" data-tipe="DJBUCK" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('BUCK','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="DJBUCK${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(6);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><itexty inputmode="numeric"pe="text" data-tipe="DJMJ" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('MJ','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="DJMJ${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(7);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><inptexte inputmode="numeric"="text" data-tipe="DJNECK" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('NECK','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="DJNECK${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(8);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><intextp inputmode="numeric"e="text" data-tipe="DJBRO" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('BRO','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="DJBRO${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(9);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><intextp inputmode="numeric"e="text" data-tipe="DJPDT" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('PDT','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="DJPDT${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(10);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><inptexte inputmode="numeric"="text" data-tipe="DJPDTC" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('PDTC','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="DJPDTC${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(11);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><itexty inputmode="numeric"pe="text" data-tipe="DJWR" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('WR','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="DJWR${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(12);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><intextp inputmode="numeric"e="text" data-tipe="DJOTH" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('OTH','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="DJOTH${type + counters[type]}"/></div>`; // Replace with actual data as needed

        // Add a cell for Total DJ (example data)
        var totalDJCell = newRow.insertCell(13);
        totalDJCell.innerHTML  = `<div classtext- inputmode="numeric"center"><input type="text" oninput="sanitize(this)" onpaste="return false;" value="0" class="form-control text-center" readonly id="totalDJ${type + counters[type]}"/></div>`;

        // Add cells for the other columns in Total DJ section
          // Add cells for the other columns in Total PG section
        var cell = newRow.insertCell(14);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><intextp inputmode="numeric"e="text" data-tipe="PGBGL" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('PGBGL','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="PGBGL${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(15);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><intextp inputmode="numeric"e="text" data-tipe="PGBRC" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('PGBRC','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="PGBRC${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(16);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><intextp inputmode="numeric"e="text" data-tipe="PGEAR" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('PGEAR','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="PGEAR${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(17);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><itexty inputmode="numeric"pe="text" data-tipe="PGLR" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('PGLR','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="PGLR${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(18);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><inptexte inputmode="numeric"="text" data-tipe="PGBUCK" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('BUCK','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="PGBUCK${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(19);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><itexty inputmode="numeric"pe="text" data-tipe="PGMJ" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('MJ','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="PGMJ${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(20);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><inptexte inputmode="numeric"="text" data-tipe="PGNECK" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('NECK','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="PGNECK${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(21);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><intextp inputmode="numeric"e="text" data-tipe="PGBRO" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('BRO','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="PGBRO${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(22);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><intextp inputmode="numeric"e="text" data-tipe="PGPDT" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('PDT','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="PGPDT${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(23);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><inptexte inputmode="numeric"="text" data-tipe="PGPDTC" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('PDTC','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="PGPDTC${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(24);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><itexty inputmode="numeric"pe="text" data-tipe="PGWR" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('WR','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="PGWR${type + counters[type]}"/></div>`; // Replace with actual data as needed
    
        var cell = newRow.insertCell(25);  // Insert in the next columns            
        cell.innerHTML = `<div class="text-center"><intextp inputmode="numeric"e="text" data-tipe="PGOTH" oninput="sanitize(this)" onpaste="return false;" data-row="${type + counters[type]}" onchange="calculate('OTH','${type + counters[type]}','${type}')" value="0" class="form-control text-center" id="PGOTH${type + counters[type]}"/></div>`; // Replace with actual data as needed
        // for (var i = 13; i < 25; i++) {
        //     var cell = newRow.insertCell(i + 1);  // Insert in the next columns
        //     cell.innerHTML = `<div classtext- inputmode="numeric"center"><input type="text" oninput="sanitize(this)" onpaste="return false;" value="0" class="form-control text-center" id="${type + counters[type]}"/></div>`; // Replace with actual data as needed
        // }

        // Add a cell for Total PG
        var totalPGCell = newRow.insertCell(26);
        totalPGCell.innerHTML = `<div classtext- inputmode="numeric"center"><input type="text" oninput="sanitize(this)" onpaste="return false;" value="0" class="form-control text-center" readonly id="totalPG${type + counters[type]}"/></div>`;

      
        for (var i = 26; i < 27; i++) {
            var cell = newRow.insertCell(i + 1);  // Insert in the next columns
            cell.innerHTML = `<div classtext- inputmode="numeric"center"><input type="text" oninput="sanitize(this)" onpaste="return false;" value="0" class="form-control text-center" id="${type + counters[type]}"/></div>`; // Replace with actual data as needed
        }

        // Add a cell for Total LD
        var totalLDCell = newRow.insertCell(28);
        totalLDCell.innerHTML = `<div classtext- inputmode="numeric"center"><input type="text" oninput="sanitize(this)" onpaste="return false;" value="0" class="form-control text-center" readonly id="totalLD${type + counters[type]}"/></div>`;

        // Add a cell for Total ALL
        var totalALLCell = newRow.insertCell(29);
        totalALLCell.innerHTML = `<div classtext- inputmode="numeric"center"><input type="text" oninput="sanitize(this)" onpaste="return false;" value="0" class="form-control text-center" readonly id="totalALL${type + counters[type]}"/></div>`;

        var deleteCell = newRow.insertCell(30);  // Insert at the last position of the row
        deleteCell.innerHTML = `<div class="text-center"><button class="btn btn-danger btn-sm" onclick="deleteRow(this)">Delete</button></div>`;

        // Now, reorder rows to keep the table sorted
        reorderRows();
    }

    function reorderRows() {
        var table = document.getElementById('<%= tb.ClientID %>');
        var rows = Array.from(table.rows).slice(3);  // Exclude the first two header rows

        // Sort the rows based on the first column (Barang Display)
        rows.sort(function(rowA, rowB) {
            var textA = rowA.cells[0].textContent;  // Get the Barang text (e.g., 'A1', 'B1')
            var textB = rowB.cells[0].textContent;  // Get the Barang text (e.g., 'A2', 'B1')

            var letterA = textA.charAt(0);  // Extract the letter (e.g., 'A')
            var letterB = textB.charAt(0);  // Extract the letter (e.g., 'B')

            // First, compare letters (alphabetical order)
            if (letterA !== letterB) {
                return letterA.localeCompare(letterB);  // A < B < C < D
            } else {
                // If letters are the same, compare the number after the letter (numerical order)
                var numberA = parseInt(textA.slice(1), 10);  // Extract the number after the letter (e.g., 1 from 'A1')
                var numberB = parseInt(textB.slice(1), 10);  // Extract the number after the letter (e.g., 2 from 'A2')
                return numberA - numberB;  // A1 < A2 < B1 < C1 < ...
            }
        });

        // Append the sorted rows back to the table
        rows.forEach(function(row) {
            table.appendChild(row);
        });
    }
    function addFooterRow() {        
            // Get the table element by ID
            var table = document.getElementById('<%= tb.ClientID %>');

            // Create a new footer row
            var footerRow = document.createElement('tr');
            footerRow.id = "totalFooter";

            // Create a footer cell for "Total"
            var footerCell = document.createElement('td');

            footerCell.colSpan = 1;  
            footerCell.classList.add("freeze-column");
            footerCell.innerHTML = `<div  style="font-weight:bold;text-align:center;">Total</div>`;
            footerRow.appendChild(footerCell);  // Append to footer row


            var cell = document.createElement('td');
            cell.innerHTML = `<div class="text-center"><input type="text" value="0" class="form-control text-center" readonly id="totalDJBGL"/></div>`;                
            footerRow.appendChild(cell);  // Append to footer row DJ BGL

            var cell = document.createElement('td');
            cell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="totalDJBRC"/></div>`;                                
            footerRow.appendChild(cell);  // Append to footer row DJ BRC
            
            var cell = document.createElement('td');
            cell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="totalDJEAR"/></div>`;                                
            footerRow.appendChild(cell);  // Append to footer row DJ EAR
            
            var cell = document.createElement('td');
            cell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="totalDJLR"/></div>`;                                
            footerRow.appendChild(cell);  // Append to footer row DJ LR

            var cell = document.createElement('td');
            cell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="totalDJBUCK"/></div>`;                                
            footerRow.appendChild(cell);  // Append to footer row DJ BUCK

            var cell = document.createElement('td');
            cell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="totalDJMJ"/></div>`;                                
            footerRow.appendChild(cell);  // Append to footer row DJ MJ

            var cell = document.createElement('td');
            cell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="totalDJNECK"/></div>`;                                
            footerRow.appendChild(cell);  // Append to footer row DJ NECK
            var cell = document.createElement('td');

            cell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="totalDJBRO"/></div>`;                                
            footerRow.appendChild(cell);  // Append to footer row DJ BRO

            var cell = document.createElement('td');
            cell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="totalDJPDT"/></div>`;                                
            footerRow.appendChild(cell);  // Append to footer row DJ PDT

            var cell = document.createElement('td');
            cell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="totalDJPDTC"/></div>`;                                
            footerRow.appendChild(cell);  // Append to footer row DJ P+C

            var cell = document.createElement('td'); 
            cell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="totalDJWR"/></div>`;                                
            footerRow.appendChild(cell);  // Append to footer row DJ WR

            var cell = document.createElement('td');
            cell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="totalDJOTH"/></div>`;                                
            footerRow.appendChild(cell);  // Append to footer row DJ OTH

            var totalPGCell = document.createElement('td');
            totalPGCell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center"  readonly id="totalDJ"/></div>`;
            footerRow.appendChild(totalPGCell);
            

            // Add cells for the other columns in the "Total DJ" section
            for (var i = 13; i < 25; i++) {
                var cell = document.createElement('td');
                cell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="input_${i}"/></div>`;
                footerRow.appendChild(cell);
            }

            // Add a cell for "Total PG"
            var totalPGCell = document.createElement('td');
            totalPGCell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="totalPG"/></div>`;
            footerRow.appendChild(totalPGCell);

            // Add cells for the other columns in the "Total PG" section
            for (var i = 26; i < 27; i++) {
                var cell = document.createElement('td');
                cell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="input_${i}"/></div>`;
                footerRow.appendChild(cell);
            }

            // Add a cell for "Total LD"
            var totalLDCell = document.createElement('td');
            totalLDCell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="totalLD"/></div>`;
            footerRow.appendChild(totalLDCell);

            // Add a cell for "Total ALL"
            var totalALLCell = document.createElement('td');
            totalALLCell.innerHTML = `<div class="text-center"><input type="text"  value="0" class="form-control text-center" readonly id="totalALL"/></div>`;
            footerRow.appendChild(totalALLCell);

            // Append the footer row to the table
            table.appendChild(footerRow);
    }
    
    function deleteRow(button) {
        // Get the row that contains the "Delete" button that was clicked
        var row = button.closest("tr");

        // Remove the row from the table
        row.parentNode.removeChild(row);

        // Reorder the remaining rows after deletion
        reorderRows();
    }

    function sanitize(el) {
        // remove everything except digits
        el.value = el.value.replace(/[^0-9]/g, '');

        // remove leading zeros (but keep single 0)
        if (el.value.length > 1) {
            el.value = el.value.replace(/^0+/, '');
        }
    }

    function onlyNumbersAndCommas(event) {
        // Get the ASCII code of the pressed key
        var charCode = event.which ? event.which : event.keyCode;
         const allowedKeys = [
            "Backspace",
            "Delete",
            "ArrowLeft",
            "ArrowRight",
            "Tab"
        ];

        // Allow control keys
        if (allowedKeys.includes(event.key)) {
            return true;
        }


        // Allow only numbers (0–9)
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
            return false;
        }

        // Remove leading zeros
        let input = event.target.value.replace(/^0+/, '');
        event.target.value = input;

        return true;
        
    }

    function calculate(item,ID,type){                
        // Start calculate DJ
        console.log([item,ID,type]);

        let DJROW = document.querySelectorAll(`[data-row="${ID}"]`);

        let DJBGL = document.querySelectorAll('[data-tipe="DJBGL"]');
        let DJBRC = document.querySelectorAll('[data-tipe="DJBRC"]');
        let DJEAR = document.querySelectorAll('[data-tipe="DJEAR"]');
        let DJLR = document.querySelectorAll('[data-tipe="DJLR"]');
        let DJBUCK = document.querySelectorAll('[data-tipe="DJBUCK"]');
        let DJMJ = document.querySelectorAll('[data-tipe="DJMJ"]');
        let DJNECK = document.querySelectorAll('[data-tipe="DJNECK"]');
        let DJBRO = document.querySelectorAll('[data-tipe="DJBRO"]');
        let DJPDT = document.querySelectorAll('[data-tipe="DJPDT"]');
        let DJPDTC = document.querySelectorAll('[data-tipe="DJPDTC"]');
        let DJWR = document.querySelectorAll('[data-tipe="DJWR"]');
        let DJOTH = document.querySelectorAll('[data-tipe="DJOTH"]');

        let totDJSBGL = $('#TotalDJSBGL').html();
        let totDJSLBGL = 0;
            
        var totDJRow = 0;

        // Loop through the elements and get the value of data-tipe
        let djbglval = 0;
        let djbrcval = 0;
        let djearval = 0;
        let djlrval = 0;
        let djbuckval = 0;
        let djmjval = 0;
        let djneckval = 0;
        let djbroval = 0;
        let djpdtval = 0;
        let djpdtcval = 0;
        let djwral = 0;
        let djothval = 0;
        

        DJBGL.forEach(function(element) {
            let value = element.value === '' ? 0 : element.value;
            if (value == 0 ) {element.value = 0;}
            djbglval = parseInt(djbglval) + parseInt(value);                        
        });                
        DJBRC.forEach(function(element) {
            let value = element.value === '' ? 0 : element.value;
            if (value == 0 ) {element.value = 0;}
            djbrcval = parseInt(djbrcval) + parseInt(value)
        });                
        DJEAR.forEach(function(element) {
            let value = element.value === '' ? 0 : element.value;
            if (value == 0 ) {element.value = 0;}
            djearval = parseInt(djearval) + parseInt(value)
        });        
        DJLR.forEach(function(element) {
            let value = element.value === '' ? 0 : element.value;
            if (value == 0 ) {element.value = 0;}
            djlrval = parseInt(djlrval) + parseInt(value)
        });
        DJBUCK.forEach(function(element) {
            let value = element.value === '' ? 0 : element.value;
            if (value == 0 ) {element.value = 0;}
            djbuckval = parseInt(djbuckval) + parseInt(value)
        });  
        
        DJROW.forEach((e)=>{
            let value = e.value === '' ? 0 : e.value;            
            if (value == 0 ) {e.value = 0;}
            totDJRow = parseInt(totDJRow)+parseInt(value);
        })

        totDJSLBGL = parseInt(djbglval)-parseInt(totDJSBGL);

        var style = "style='color:black;'";
        if (totDJSLBGL < 0) {
            style = "style='color:red;'";
        }
        
        $('#totalDJBGL').val(djbglval)
        $('#totalDJBRC').val(djbrcval)
        $('#totalDJEAR').val(djearval)
        $('#totalDJLR').val(djlrval)
        $('#totalDJBUCK').val(djbuckval)        
        $('#TotalDJSLBGL').html("<span "+style+">"+totDJSLBGL+"</span>")

        if(type == 'front'){$(`#totalFDJ_${ID}`).val(totDJRow);}else{$(`#totalDJ${ID}`).val(totDJRow)}
        
        // End calculate DJ

        //Start calculate PG
        //End calculate PG
        
    }
    
    function kalkulasi(Column, IDTable, Type){
         let DataTipe = document.querySelectorAll(`[data-tipe="${Column}"]`);
         let DataTipeRow = document.querySelectorAll(`[data-row="${Column.substring(0,2)+IDTable}"]`);
         let DataTipeColumnAll = document.querySelectorAll(`[data-row="ALL"]`);
         let DataTotalColumn = document.querySelectorAll(`[data-column="total${Column.substring(0,2)}"]`)
         let totalColumn=0;
         let totalRow=0;
         let totalColumnAll = 0;
         let totalanColumn = 0;
        
        DataTipe.forEach((e)=>{
            let value = e.value === '' ? 0 : e.value;            
            if (value == 0 ) {e.value = 0;}
            totalColumn = parseInt(totalColumn)+parseInt(value);
        })

        DataTipeRow.forEach((e)=>{
            let value = e.value === '' ? 0 : e.value;            
            if (value == 0 ) {e.value = 0;}
            totalRow = parseInt(totalRow)+parseInt(value);
        })        

        $(`#total${Column}`).val(totalColumn)
        $(`#total${Column.substring(0,2)}_${IDTable}`).val(totalRow)

        DataTotalColumn.forEach((e)=>{
            let value = e.value === '' ? 0 : e.value;            
            if (value == 0 ) {e.value = 0;}
            totalanColumn = parseInt(totalanColumn)+parseInt(value);
        })
        let totalRowDJ = parseInt($(`#totalDJ_${IDTable}`).val());
        let totalRowPG = parseInt($(`#totalPG_${IDTable}`).val());
        let totalRowLD = parseInt($(`#totalLD_${IDTable}`).val());        

        $(`#totalALL_${IDTable}`).val(totalRowDJ+totalRowPG+totalRowLD);

        DataTipeColumnAll.forEach((e)=>{
            let values = e.value === '' ? 0 : e.value;
            if (values == 0 ) {e.value = 0;}            
            totalColumnAll = parseInt(totalColumnAll)+parseInt(values)
        });
        $(`#total${Column.substring(0,2)}`).val(totalanColumn)

        $('#totalALL').val(totalColumnAll);
    }

    function kalkulasiBarangDalam(DataColumn, DataRow, DataColumnProduct){
        
        let DataTipeColumn = document.querySelectorAll(`[data-product="BD${DataColumn.substring(0,2)}${DataColumn.substring(2)}"]`);        
        // let DataTipeColumn = document.querySelectorAll(`[data-tipe="BD${DataColumn.substring(0,2)}"][data-column="${DataColumn}"]`);
        let DataTipeRow = document.querySelectorAll(`[data-row="${DataRow}"]`);
        let DataTipeColumnAll = document.querySelectorAll(`[data-column="TotalALLBD"]`);
        let DataColumnALL = document.querySelectorAll(`[data-column = "Total${DataColumn.substring(0,2)}BD"]`)        
        let totalColumn = $(`#Total${DataColumn}`).val() === '' ? 0 : $(`#Total${DataColumn}`).val();
        let totalRow = $(`#Total${DataRow}`).val() === '' ? 0 : $(`#Total${DataRow}`).val();
        let totalAllColumn = 0;
        let totalColumnTemp = 0;        
        let totalRowTemp = 0;        
        
        // Condition: if DJ (first 2 chars) starts with 0, set to 0
        if (DataColumn.substring(0,2) === 'DJ') {
            totalColumn = 0;
            totalRow = 0;
        }
        let totalColumnAll = 0;

        
        DataTipeColumn.forEach((e)=>{
            let values = Number(e.value) || 0;
            if(values==0){e.value=0;}
            totalColumnTemp += parseInt(values);
        })             
        totalColumn = totalColumnTemp;

        DataTipeRow.forEach((e)=>{
            let values = e.value === '' ? 0 : e.value;
            if(values==0){e.value=0;}            
            totalRowTemp += parseInt(values);
        })
        totalRow = totalRowTemp;

        $(`#Total${DataColumn}`).val(totalColumn)
        $(`#Total${DataRow}`).val(totalRow)        
                
        DataColumnALL.forEach((e)=>{
            let values = e.values ==='' ?0 :e.value;
            if(values == 0){
                e.value = 0;
            }
            totalAllColumn = parseInt(totalAllColumn) + parseInt(values);
        })
        

        let totalRowDJ = parseInt($(`#TotalDJ${DataRow.substring(2,4)}`).val());
        let totalRowPG = parseInt($(`#TotalPG${DataRow.substring(2,4)}`).val());
        let totalRowLD = parseInt($(`#TotalLD${DataRow.substring(2,4)}`).val());
        

        $(`#totalALL${DataRow.substring(2,4)}`).val(totalRowDJ+totalRowPG+totalRowLD)        
        $(`#totalBD${DataColumn.substring(0,2)}`).val(totalAllColumn)        
         DataTipeColumnAll.forEach((e)=>{
            let values = e.value === '' ? 0 : e.value;
            if (values == 0 ) {e.value = 0;}            
            totalColumnAll = parseInt(totalColumnAll)+parseInt(values)
        });
        
        $('#totalBDALL').val(totalColumnAll);

    }
    function kalkulasiBarangTitipan(DataColumn, DataRow){           
        
        let DataTipeColumn = document.querySelectorAll(`[data-tipe="BT${DataColumn.substring(0,2)}"][data-column="${DataColumn}"]`);
        let DataTipeRow = document.querySelectorAll(`[data-row="${DataRow}"]`);
        let DataTipeColumnAll = document.querySelectorAll(`[data-column="TotalALLBT"]`);
        let DataColumnALL = document.querySelectorAll(`[data-column = "Total${DataColumn.substring(0,2)}BT"]`)        
        let totalColumn = 0;
        let totalRow = 0;
        let totalAllColumn = 0;
        let totalColumnAll = 0;

        DataTipeColumn.forEach((e)=>{
            let values = Number(e.value) || 0;
            if(values==0){e.value=0;}
            totalColumn = parseInt(totalColumn)+parseInt(values)            
        })       
        DataTipeRow.forEach((e)=>{
            let values = e.value === '' ? 0 : e.value;
            if(values==0){e.value=0;}
            totalRow = parseInt(totalRow)+parseInt(values)
        })

        $(`#TotalBT${DataColumn}`).val(totalColumn)
        $(`#Total${DataRow}`).val(totalRow)        

        
        DataColumnALL.forEach((e)=>{
            let values = e.value ==='' ?0 :e.value;
            if(values == 0){
                e.value = 0;
            }
            totalAllColumn += Number(values)||0;
        })

        let totalRowDJ = parseInt($(`#TotalDJ${DataRow.substring(2,4)}`).val()) || 0;
        let totalRowPG = parseInt($(`#TotalPG${DataRow.substring(2,4)}`).val()) || 0;
        let totalRowLD = parseInt($(`#TotalLD${DataRow.substring(2,4)}`).val()) || 0;        
        
        $(`#totalALL${DataRow.substring(2,4)}`).val(totalRowDJ+totalRowPG+totalRowLD)
        $(`#TotalBT${DataColumn.substring(0,2)}`).val(totalAllColumn)
        
         DataTipeColumnAll.forEach((e)=>{
            let values = e.value === '' ? 0 : e.value;
            if (values == 0 ) {e.value = 0;}            
            totalColumnAll = parseInt(totalColumnAll)+parseInt(values)
        });
        $('#TotalBTALL').val(totalColumnAll);

    }
    function kalkulasiBarangLuar(DataColumn, DataRow){                
        let DataTipeColumn = document.querySelectorAll(`[data-tipe="BL${DataColumn.substring(0,2)}"][data-column="${DataColumn}"]`);
        let DataTipeRow = document.querySelectorAll(`[data-row="${DataRow}"]`);
        let DataTipeColumnAll = document.querySelectorAll(`[data-column="TotalALLBL"]`);
        let DataColumnALL = document.querySelectorAll(`[data-column = "Total${DataColumn.substring(0,2)}BL"]`)        
        let totalColumn = 0;
        let totalRow = 0;
        let totalAllColumn = 0;
        let totalColumnAll = 0;
        let totalColumnTemp = 0;
        
        DataTipeColumn.forEach((e)=>{
            let values = Number(e.value) || 0;
            if(values==0){e.value=0;}
            totalColumn = parseInt(totalColumn)+parseInt(values)
        })       
        DataTipeRow.forEach((e)=>{
            let values = e.value === '' ? 0 : e.value;
            if(values==0){e.value=0;}
            totalRow = parseInt(totalRow)+parseInt(values)
        })

        $(`#TotalBL${DataColumn}`).val(totalColumn)
        $(`#Total${DataRow}`).val(totalRow)        

        
        DataColumnALL.forEach((e)=>{
            let values = e.values ==='' ?0 :e.value;
            if(values == 0){
                e.value = 0;
            }
            totalAllColumn = parseInt(totalAllColumn) + parseInt(values);
        })

        let totalRowDJ = parseInt($(`#TotalDJ${DataRow.substring(2,4)}`).val());
        let totalRowPG = parseInt($(`#TotalPG${DataRow.substring(2,4)}`).val());
        let totalRowLD = parseInt($(`#TotalLD${DataRow.substring(2,4)}`).val());           

        $(`#totalALL${DataRow.substring(2,4)}`).val(totalRowDJ+totalRowPG+totalRowLD)
        $(`#TotalBL${DataColumn.substring(0,2)}`).val(totalAllColumn)
        
         DataTipeColumnAll.forEach((e)=>{
            let values = e.value === '' ? 0 : e.value;
            if (values == 0 ) {e.value = 0;}            
            totalColumnTemp += parseInt(values);
        });
        totalColumnAll = totalColumnTemp;
        
        $('#TotalBLALL').val(totalColumnAll);

    }
    
    </script>
</asp:content>

