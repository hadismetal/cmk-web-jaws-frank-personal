<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MasterJabatanPengganti_Form.aspx.cs" Inherits="Page_MasterJabatanPengganti" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Master Table JR on Duty
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
<div class="container" style="width: 100%;height: auto;">
    <form id="Form" runat="server" class="form-horizontal">
        <div class="row">
            <div class="col-xs-12">
                <div id="errorMsg" class="alert alert-danger" style="display:none; margin-top:10px;"></div>
                <div id="successMsg" class="alert alert-success" style="display:none; margin-top:10px;"></div>
            </div>
        </div>
        <input type="hidden" class="form-control" id="IDLokasi" runat="server" />
        <div class="container">
            <!-- <h4>List JR</h4> -->
            <div id="formRows" class="row">
                
                <div class="col-12 form-group row-item">
                    <div class="col-xs-4">                        
                        <label for="ListJR" class="form-label fw-bold">List JR</label>
                        <select class="form-control brand-input" name="ListJR" id="ListJR">
                        </select>                        
                    </div>    
                    <div class="col-xs-4">
                        <label for="TipeJabatan" class="form-label fw-bold">Jabatan Sementara</label>
                        <select class="form-control brand-input" name="TipeJabatan" id="TipeJabatan">                            
                            <option value="SM">Store Manager</option>                            
                        </select>
                    </div>
                </div>
                <div class="col-12 form-group row-item">
                    <div class="col-xs-4">
                        <label for="Shift" class="form-label fw-bold">Shift</label>
                        <select class="form-control shift-input" name="Shift" id="Shift">                            
                        </select>
                    </div>
                    <div class="col-xs-4">
                        <label for="input-tgl-awal" class="form-label fw-bold">Tanggal Duty</label>
                        <input class="form-control" id="input-tgl-awal" type="date" onkeydown="return false;" />
                    </div>
                </div>
            </div>

            <!-- Add Row Button -->

            <div id="formRows1" class="row">
                <!-- Default row -->
                <div class="col-xs-12 form-group row-item">
                    <div class="col-xs-6">                        
                        <button type="button" id="btnSave" class="btn btn-primary" disabled>
                            Save
                        </button>                        
                    </div>                    
                </div>
            </div>
            
        </div>        
    </form>

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
         
            const urlParams = new URLSearchParams(window.location.search);
            const IDLokasi = urlParams.get("idlokasi");            

            getListJR(IDLokasi);
            getMasterShift();
            let currDate = new Date();
			const offset = currDate.getTimezoneOffset();
			currDate = new Date(currDate.getTime() - offset * 60 * 1000);
			const currDateISO = currDate.toISOString().split("T")[0];			
					
			$('#input-tgl-awal').val(currDateISO);
            $('#input-tgl-awal').attr("min", currDateISO);            

        function getListJR(IDLokasi){
            $.ajax({
                type: "POST",
                url: "MasterJabatanPengganti_Form.aspx/GetListJR",
                data: JSON.stringify({ IDLokasi: IDLokasi }),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {                    
                    var listJR = response.d;
                    let html=`<option value="0" class="text-center">--- Silahkan Pilih Data Dibawah ---</option>`;
                    listJR.forEach((item) => {
                        html+=`<option value="${item.IDUser}">${item.Nama}</option>`;
                    })
                    $('#ListJR').html(html)
                    // toastr.success(`${response.d}`);
                    // setTimeout(() => {
                    //     window.location.href = "Mstable.aspx";
                    // }, 500);
                },
                error: function (xhr, status, error) {
                    $("#errorMsg").text("Error get data : " + error).show();
                    toastr.error(`Error get data : ${error}`);
                    
                }
            });
        }
        
        function getMasterShift(){
            $.ajax({
                type: "POST",
                url: "MasterJabatanPengganti_Form.aspx/GetMasterShift",                
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {                                        
                    var Shift = response.d;
                    let html=``;
                    Shift.forEach((item) => {
                        html+=`<option value="${item.ID}" data-maxshift="${(item.MaxShift.trim())}">${item.Nama}</option>`;
                    })
                    $('#Shift').html(html)                    
                },
                error: function (xhr, status, error) {
                    $("#errorMsg").text("Error get data : " + error).show();
                    toastr.error(`Error get data : ${error}`);
                    
                }
            });
        }
        
        
        $('#ListJR').change(()=>{
            $('#ListJR option:selected').each(function(){
                var value = $(this).val();
                if(value != "0"){
                    $("#btnSave").removeAttr("disabled");
                }else{
                    $("#btnSave").attr("disabled", "disabled");
                }
            });            
        })

        $("#btnSave").click(function () {            
            const urlParams = new URLSearchParams(window.location.search);            
            const IDLokasi = urlParams.get("idlokasi");
            let TipeJabatan = $('#TipeJabatan').val();
            let tanggalAwal = $('#input-tgl-awal').val();
            let IDUser = 0;
            let Nama = '';
            let start = '';
            let end = '';

            $("#errorMsg").hide();
            $("#successMsg").hide();                                  
            $("#btnSave").attr('disabled', 'disabled');

             $('#ListJR option:selected').each(function(){
                var value = $(this).val();
                
                IDUser = value;
                Nama = $(this).text();
            });
            let params ={ IDUser: IDUser, tanggalAwal: tanggalAwal, TipeJabatan: TipeJabatan, IDLokasi: IDLokasi, TipeShift: $('#Shift').val() };

            var statusData = checkData(Nama, params);            
        });    
        function checkData(Nama, params){
            $.ajax({
                type: "POST",
                url: "MasterJabatanPengganti_Form.aspx/CheckData",
                data: JSON.stringify({ IDUser: params.IDUser, Nama: Nama, tanggalAwal: params.tanggalAwal, IDLokasi: params.IDLokasi, TipeShift: params.TipeShift}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {   
                    let data = response.d;
                    console.log(response.d);
                    if(data > 0){                                                
                        $("#btnSave").removeAttr("disabled");                                    
                        toastr.warning(`Data dengan nama ${Nama} pada tanggal tersebut sudah terdaftar pada shift dan tanggal tersebut. Silahkan cek kembali.`);
                        return;
                    }else{                        
                        saveData(params);
                    }
                },
                error: function (xhr, status, error) {
                    $("#errorMsg").text("Error saving : " + error).show();
                    toastr.error(`Error saving : ${error}`);
                }
            });
        }
        function saveData(params){

            $.ajax({
                type: "POST",
                url: "MasterJabatanPengganti_Form.aspx/SaveData",
                data: JSON.stringify(params),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {                    
                    toastr.success(`${response.d}`);
                    setTimeout(() => {
                        window.location.href = "MasterJabatanPengganti.aspx";
                    }, 500);
                },
                error: function (xhr, status, error) {
                    $('#btnSave').removeAttr("disabled");
                    $("#errorMsg").text("Error saving : " + error).show();
                    toastr.error(`Error saving : ${error}`);
                    
                }
            });
        }
    });
    
</script>

</asp:Content>

