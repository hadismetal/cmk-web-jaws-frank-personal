<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Titipan.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Titipan
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="zero" runat="server" visible="false">
        <i class="icon-ok icon-5x pull-left text-success"></i>
        <h4>Proses Selesai</h4>
        <p class="text-muted">
            Tidak ada transaksi penjualan
        </p>
        <div class="clearfix"></div>
        <hr />
    </div>
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <div class="text-muted">
                    Search :
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Dari :
                </div>
                <asp:TextBox ID="dari" runat="server" CssClass="form-control text-center" Width="110" placeholder="Dari" />
                <script>
                    $(document).ready(function () { $('#<%=dari.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>

            </div>
            <div class="form-group">
                <div class="text-muted">
                    Sampai :
                </div>
                <asp:TextBox ID="sampai" runat="server" CssClass="form-control text-center" Width="110" placeholder="Sampai" />
                <script>
                    $(document).ready(function () { $('#<%=sampai.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                </script>

            </div>
			<div class="form-group">
                <div class="text-muted">
                    Tipe Lokasi
                </div>
                <asp:DropDownList ID="tipelokasi" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasi" AutoPostBack="true">
                    <asp:ListItem Value="-1">Tipe Lokasi</asp:ListItem>
                    <asp:ListItem Value="1">Outlet</asp:ListItem>
                    <asp:ListItem Value="2">Exhibition</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Lokasi
                </div>
                <asp:DropDownList ID="lokasi" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Lokasi</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Status Titipan
                </div>
                <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="-1">All</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Kategori Titipan
                </div>
                <input type="text" style="display: none;" id="selectedTipe" name="selectedTipe" runat="server" value="-1" />
                <asp:DropDownList ID="tipeTitipan"  runat="server" CssClass="form-control" >
                    <asp:ListItem Value="-1">All</asp:ListItem>
                </asp:DropDownList>
            </div>
            
            <div class="form-group">						
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="btnFind" class="btn btn-primary" type="button">
                        <i class="icon-search"></i>
                        Display 
                    </button>
                </div>
            </div>
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="exportData" runat="server" type="button" class="btn btn-primary" onclientclick="return setHiddenValue()" onserverclick="exportTableNew" >
                        <i class="icon-download-alt"></i>
                        Export
                    </button>
                </div>
            </div>
            <div class="table-responsive">          
                <div class="row" style="margin: 30px;">
                    <div class="col-12">
                        <div class="border rounded p-5">
                            <table class="table table-hover tableTitipan">
                                <thead>
                                    <tr>   
                                        <th></th>             
                                        <th>No</th>
                                        <th>Cabang</th>
                                        <th>Tanggal</th>
                                        <th>No. Referensi</th>
                                        <th>NamaCustomer</th>
                                        <th>Nama Sales</th>
                                        <th>No. PLU</th>                                        
                                        <th>Qty</th>
                                        <th>Status</th>
                                        <th>Kategori Titipan</th>
                                        <th>Tanggal Pengembalian</th>
                                        <th>Operator Pengembalian</th>
                                        <th>Bukti Pengembalian</th>                                    
                                    </tr>
                                </thead>
                                <tbody id="tbody-list-titipan"></tbody>
                            </table>
                        </div>
                    </div>
                </div>        
            </div>
        
        <div class="modal fade" id="uploadModal" tabindex="-1" role="dialog" aria-labelledby="modalLabelSmall" aria-hidden="true">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title modal-title-upload" id="modalLabelSmall"></h4>
                    </div>
                    <div class="modal-body modal-body-upload">

                            <table class="table-form">
                                <tr>
                                    <td>Upload Bukti Pengembalian</td>
                                    <td>:</td>
                                    <td>
                                        <asp:fileupload id="imgpicture" runat="server" cssclass="form-control" />
                                        <span id="imgpicture_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                    </td>                                    
                                </tr>                                
                            </table>
                            <span class="text-muted text-sm">
                                Format : <%= Str.ComaExt(Upload.ImageFormat) %> / PDF                                  
                            </span>
                            <asp:HiddenField ID="hiddenNomor" runat="server" />

                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <button type="button" class="btn btn-success" id="uploadBukti">Upload Bukti</button>
                        <button type="button" class="btn btn-success" style="display: none;" runat="server" onserverclick="upload" id="uploadGO"></button>                        
                    </div>
                </div>
            </div>
        </div>
        <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">
            <div class="modal-dialog modal-lg" role="document">
                <div class="modal-content">
                ...
                </div>
            </div>
            </div>
        <div class="modal fade bs-example-modal-lg" id="buktiModal" tabindex="-1" role="dialog" aria-labelledby="myLargeModalLabel">            
            <div class="modal-dialog modal-lg" style="width: 50%;" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                        </button>
                        <h4 class="modal-title modal-title-bukti" id="modalLabelSmall"></h4>
                    </div>
                    <div class="modal-body modal-body-bukti">

                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>                                            
                    </div>
                </div>
            </div>
        </div>
    </form>

    </div>
      
        

    <script>
        $(document).ready(function () {
            toastr.options = {
				closeButton: true,
				progressBar: true,
				positionClass: "toast-top-center",
				showDuration: "300",
				hideDuration: "1000",
				timeOut: "5000",
			};
            // getMasterTipeTitipan();

            $(`#btnFind`).click(function() {
                let kw1 = $("#<%= kw.ClientID %>").val();
                let dari1 = formatDateToISO($("#<%= dari.ClientID %>").val());
                let sampai1 = formatDateToISO($("#<%= sampai.ClientID %>").val());
                let tipelokasi1 = $("#<%= tipelokasi.ClientID %>").val();
                let lokasi1 = $("#<%= lokasi.ClientID %>").val();
                let status1 = $("#<%= status.ClientID %>").val();
                let tipeTitipan1 = $("#<%= tipeTitipan.ClientID %>").val();
                let param = {
                    kw: kw1,
                    dari: dari1,
                    sampai: sampai1,
                    tipelokasi: tipelokasi1,
                    idlokasi: lokasi1,
                    status: status1,
                    tipeTitipan: tipeTitipan1
                };        
                console.log(param)        
                
                $.ajax({
                    url: "Titipan.aspx/getData",
                    type: "POST",
                    data: JSON.stringify(param),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        let data = response.d;
                        let html = ``;
                        data.forEach(function (e,i) {
                            let gambarnya = ``;
                            console.log(e.TglPengembalian)
                            if (e.TglPengembalian == '-') {
                                gambarnya = `<span class="text-muted">Titipan ini belum di kembalikan</span>`;
                            }else{
                                if (e.BuktiPengembalian == '-') {                                    
                                    gambarnya = `<button type="button" class="btn btn-success" data-toggle="modal" data-target="#uploadModal" data-id='${e.ID}' data-nama='${e.Nomor}' data-bukti="${e.BuktiPengembalian}" >Upload Bukti</button>`;
                                }else{
                                    gambarnya = `<button type="button" class="btn btn-primary" data-toggle="modal" data-target="#buktiModal" data-id='${e.ID}' data-nama='${e.Nomor}' data-bukti="${e.BuktiPengembalian}" >Lihat Bukti</button>`;
                                }
                            }

                            let buttonFront = e.StatusTitipanID != 5?`
                                        <a class="btn btn-warning btn-sm" target="_blank" href="Titipan_Update.aspx?id=${e.ID}" title="Update Status">
                                            <i class="icon-edit"></i> Update
                                        </a>`:``;
                            html+=`
                            <tr>
                                <td style="white-space:nowrap;">
                                    <div class="btn-group btn-group-sm" role="group">
                                        <a class="btn btn-info btn-sm" target="_blank" href="Titipan_Detail.aspx?id=${e.ID}" title="View Details">
                                            <i class="icon-search"></i> Open
                                        </a>
                                        ${buttonFront}
                                    </div>                                
                                </td>
                                <td>` + (i+1) + `</td>
                                <td>` + e.Cabang + `</td>
                                <td>` + e.Tgl + `</td>
                                <td>` + e.Nomor + `</td>
                                <td>` + e.CustomerNama + `</td>
                                <td>` + e.SalesNama + `</td>
                                <td>` + e.PLUTitipan + `</td>
                                <td>` + e.Qty + `</td>
                                <td>` + e.StatusTitipan + `</td>
                                <td>` + e.TipeTitipan + `</td>                                
                                <td>` + e.TglPengembalian + `</td>
                                <td>` + e.OperatorPengembalian + `</td>
                                <td>${gambarnya}</td>
                            </tr>
                            `;
                        });
                        $("#tbody-list-titipan").html(html);
                    },
                    error: function (response) {
                        console.log(response);
                    }                    
                })

            });                



            $('#uploadModal').on('show.bs.modal', function (event) {
                var button = $(event.relatedTarget); // Button that triggered the modal
                var nomor = button.data('nama'); // Extract info from data-* attributes
                var id = button.data('id');                
                var modal = $(this);   
                
                modal.find('.modal-title').text('Upload Bukti Pengembalian - ' + nomor);
                $('#<%= hiddenNomor.ClientID %>').val(id);


            });

            $('#buktiModal').on('show.bs.modal', function (event) {
                var button = $(event.relatedTarget); // Button that triggered the modal
                var nomor = button.data('nama'); // Extract info from data-* attributes
                var bukti = button.data('bukti'); // Extract info from data-* attributes
                var id = button.data('id');                
                var modal = $(this);   
                let dot = bukti.lastIndexOf('.');
                let len = bukti.length - dot;
                let ext = /(\.jpg|\.jpeg|\.png)$/i;
                let formatnya = ``;
      
                if (!ext.exec(bukti.substring(dot,dot+len))) {
                    formatnya = `<iframe src="/Images.aspx?path=${bukti}" frameborder="0" style="width: 100%; height: 70vh;"></iframe>`
                }else{
                    formatnya = `<div style="display: flex; justify-content: center;">
                                    <img src="/Images.aspx?path=${bukti}" class="img-responsive img-rounded" />
                                </div>`;
                }

                modal.find('.modal-title-bukti').text('Bukti Pengembalian - ' + nomor);
                modal.find('.modal-body-bukti').html(formatnya);
            });

            $('#uploadBukti').click(function() {
                let checkPict = $("#<%= imgpicture.ClientID %>").val()
				let Gambar = $("#<%= imgpicture.ClientID %>")[0].files[0];				 
				if (checkPict == '') {
					toastr.error("Gambar harus di Upload!!");
					return;
				}

                let allowedExtensions = /(\.jpg|\.jpeg|\.png|\.pdf)$/i;
                if (!allowedExtensions.exec(checkPict)) {
                    toastr.error("Format file tidak valid!!");
                    $("#<%= imgpicture.ClientID %>").val('');
                    return;
                }

                $("#<%= uploadGO.ClientID %>").click();


            });

            function getMasterTipeTitipan() {
                $.ajax({
                    url: "Titipan.aspx/GetTipeTitipan",
                    type: "GET",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        let data = response.d;
                        let html = ``;
                        data.forEach(function (e,i) {
                            html+=`
                            <option value='` + e.ID + `'>` + e.Nama + `</option>
                            `;
                        });
                        $("#<%= tipeTitipan.ClientID %>").append(html);
                    },
                    error: function (response) {
                        console.log(response);
                    }
                });
            }

            function formatDateToISO(dateStr) {
                let parts = dateStr.split('-'); // "09-04-2026"
                return `${parts[2]}-${parts[1]}-${parts[0]}`; // "2026-04-09"
            }                
        }); 
    </script>
</asp:Content>

