<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"
AutoEventWireup="true" CodeFile="StockConsignment.aspx.cs"
Inherits="Page_Approval_StockConsignment" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Stock Consignment SKK
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Approval
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
  <div id="zero" runat="server" visible="false">
    <i class="icon-ok icon-5x pull-left text-success"></i>
    <h4>Proses Selesai</h4>
    <p class="text-muted">
      Semua data / transaksi dalam kondisi sudah approval.
    </p>
    <div class="clearfix"></div>
    <hr />
  </div>
  <div id="form_area" runat="server">
    <form runat="server" class="form-inline">
      <div class="form-inline">
        <div class="form-group">
          <label class="control-label">Periode</label>
          <br />
          <asp:TextBox
            class="form-control"
            type="text"
            placeholder="Dari"
            ID="datepicker"
            runat="server"
            data-date-format="dd-mm-yyyy"
            Width="110"
          />
          <script>
            $(document).ready(function () {
              $("#<%=datepicker.ClientID%>").datepicker({
                format: "dd-mm-yyyy",
              });
            });
          </script>
          <asp:RequiredFieldValidator ID="datepicker_RF" runat="server" />
          <span
            id="datepicker_ERR"
            runat="server"
            class="text-validator"
            visible="false"
            >Format</span
          >
        </div>
        <div class="form-group">
          <label class="control-label">&nbsp;</label>
          <br />
          <asp:TextBox
            class="form-control"
            type="text"
            placeholder="Sampai"
            ID="datepicker2"
            runat="server"
            data-date-format="dd-mm-yyyy"
            Width="110"
          />
          <script>
            $(document).ready(function () {
              $("#<%=datepicker2.ClientID%>").datepicker({
                format: "dd-mm-yyyy",
              });
            });
          </script>
          <asp:RequiredFieldValidator ID="datepicker2_RF" runat="server" />
          <span
            id="datepicker2_ERR"
            runat="server"
            class="text-validator"
            visible="false"
            >Format</span
          >
        </div>
        <div class="form-group">
          <label class="control-label">Product Level</label>
          <br />
          <asp:DropDownList
            CssClass="form-control"
            runat="server"
            ID="ddl_ProductLevel"
          >
            <asp:ListItem Value="0">Product Level</asp:ListItem>
          </asp:DropDownList>
        </div>

        <div class="form-group">
          <label class="control-label">Item</label>
          <br />
          <asp:DropDownList
            CssClass="form-control"
            runat="server"
            ID="ddl_ProductItem"
          >
            <asp:ListItem Value="0">Product Item</asp:ListItem>
          </asp:DropDownList>
        </div>

        <div class="form-group">
          <label class="control-label">Model</label>
          <br />
          <asp:DropDownList
            CssClass="form-control"
            runat="server"
            ID="ddl_Model"
          >
            <asp:ListItem Value="0">Model</asp:ListItem>
          </asp:DropDownList>
        </div>

        <div class="form-group">
          <label class="control-label">Warna</label>
          <br />
          <asp:DropDownList
            CssClass="form-control"
            runat="server"
            ID="ddl_RangkaWarna"
          >
            <asp:ListItem Value="0">Warna</asp:ListItem>
          </asp:DropDownList>
        </div>
        <div class="form-group">
          <label class="control-label">Supplier</label>
          <br />
          <asp:DropDownList
            CssClass="form-control"
            runat="server"
            ID="ddl_Supplier"
          >
            <asp:ListItem Value="0">Supplier</asp:ListItem>
          </asp:DropDownList>
        </div>
      </div>
      <div class="form-group">
        <div class="hidden-xs">&nbsp;</div>
        <div>
          <asp:HiddenField ID="hdnRefreshType" runat="server" Value=""/>
          <button id="go1" ClientIDMode="Static" value="header" runat="server" class="btn btn-primary btn" onserverclick="btn_display_Header" >
            <i class="icon-search"></i>
            Display
          </button>
          <button id="go" ClientIDMode="Static" value="detail" runat="server" class="btn btn-primary btn" onserverclick="btn_display_Click" style="display: none;">
            <i class="icon-search"></i>
            Display Detail
          </button>
        </div>
      </div>   
      
          <div class="table-responsive" id="tableHeader" ClientIDMode="Static" runat="server">
            <div class="control-group" style="margin:auto auto;">
              <asp:Table ID="tbh" runat="server" CssClass="table-list table-striped text-sm">              
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">No.</asp:TableHeaderCell>                    
                    <asp:TableHeaderCell Width="140">No. Surat Jalan</asp:TableHeaderCell>                    
                    <asp:TableHeaderCell Width="70">Total Pcs</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Berat</asp:TableHeaderCell>
                    
                </asp:TableHeaderRow>
            </asp:Table>
            </div>                    
          </div>
    
          <div class="table-responsive" id="tableDetail" style="display: none;" runat="server" ClientIDMode="Static">
            <div class="control-group" style="margin:auto auto;">
              <asp:Table ID="tb" runat="server" CssClass="table-list table-striped text-sm">              
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">
                    <button runat="server" type="button"  ClientIDMode="Static" id="checkall" class="btn btn-sm btn-primary" disabled="disabled"> Check All</button>
                    <button type="button" id="uncheckall" ClientIDMode="Static" class="btn btn-sm btn-danger" style="display: none">Uncheck All</button>
                    </asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140">No. Surat Jalan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Kode Barang</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Model</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Color</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Berat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Size</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Tanggal</asp:TableHeaderCell>                    
                </asp:TableHeaderRow>
            </asp:Table>
            </div>            
            <div class="control-group pull-right">              
                <div class="controls">
                  <button runat="server" style="display: none" id="btnAppr" ClientIDMode="Static" onclick="approveSelected(); return false;" class="btn btn-success button-loading btn-default" disabled>Approve</button>
                  <input runat="server" type="hidden" ClientIDMode="Static" name="SupplierSJ" id="SupplierSJ" value="">
                  <button runat="server" style="display: none" id="btnReject" onclick="openModalR(); return false;" class="btn btn-danger button-loading btn-default" disabled>Reject</button>
                  <input runat="server" ClientIDMode="Static" type="hidden" name="CountCek" id="Jumrow" value="0">
                </div>
            </div>            
          </div>

          <div class="modal fade" tableindex="-1" id="modalRejectM" role="dialog">
                <div class="modal-dialog modal-lg" role="document">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Reject </h4>
                        </div>
                        <div class="modal-body" id="modalR" style="width:100%;height: 100%; overflow-x: hidden;overflow-y: auto;">
                        </div>
                        <div class="modal-footer">                                                        
                            <button runat="server" id="buttonRject" ClientIDMode="Static" onclick="rejectSelected(); return false;" class="btn btn-danger button-loading btn-default">Reject</button>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
    </form>

  </div>
  <script>
      var jumrow = parseInt($('#Jumrow').val())
      var btnAppr = document.getElementById("<%= btnAppr.ClientID%>");
      var btnReject = document.getElementById("<%= btnReject.ClientID%>");
      
            $(document).ready(function () {                                        
              $('#checkall').on("click", function (e) {
                $(":checkbox").prop("checked", true);
                var jumrow = 0
                let checkedCount = parseInt($(":checkbox:checked").length);

                btnAppr.disabled=false;
                btnReject.disabled=false;

                $('#Jumrow').val(jumrow+checkedCount)
                $("#uncheckall").show();
                $(e.currentTarget).hide();
              });

              $("#uncheckall").on("click", function (e) {
                var jumrow = parseInt($('#Jumrow').val())
                let checkedCount = parseInt($(":checkbox:not(:checked)").length);

                $(":checkbox").prop("checked", false);
                $("#checkall").show();                
                $('#Jumrow').val(0)
                btnAppr.disabled=true;
                btnReject.disabled=true;
                $(e.currentTarget).hide();
              });                      
            });
            function ceklist(no){                
                var ceklis = $(`#check${+no}`)
                var jumrow = parseInt($('#Jumrow').val())
                var jumrow1=0;
                if (ceklis.is(':checked')) {
                    jumrow += 1;
                } else {
                    jumrow -= 1;
                }
                console.log(jumrow)
                $('#Jumrow').val(jumrow)
                if (jumrow > 0) {                  
                  btnAppr.disabled=false;
                  btnReject.disabled=false;
                  $("#checkall").hide();                
                  $("#uncheckall").show();                
                }else{
                  btnAppr.disabled=true;
                  btnReject.disabled=true;
                  $("#checkall").show();                
                  $("#uncheckall").hide();                
                }                

            }
            function openModalR(){
              $('#modalRejectM').modal('show')
              $('#modalRejectM').on('shown.bs.modal', function() {
            // This code will be executed after the modal is shown
            let html = ''
            $('.check').each(function() {
                // Check if the checkbox is checked
                var checkBox = $(this)
                var dataNumber = checkBox.data('no')
                if (checkBox.prop('checked')) {
                    var PLU = $('#PLU' + dataNumber).val()
                    html += `                             
                            <div class="container " style="margin-top: 10px;">
                                <div class="form-group ">
                                    <label for="exampleInputName2">Alasan Reject ${PLU}</label>
                                    <textarea style="width: 75%;" name="alasan${dataNumber}" id="keterangan${dataNumber}" class="form-control"></textarea>
                                </div>                                
                            </div>                             
                        `;
                    // Perform actions when checkbox is checked
                }
            });
            $('#modalR').html(html)

        });

            }
            function detail(e){
                var div = $(e.currentTarget);
                var SupplierSJ = div.data("supp");
                $('#SupplierSJ').val(SupplierSJ)
                $('#go').click();
            }

            function rejectSelected(){
              var selectedValues = [];
              var keterangan =[];
              var PLU = [];
              var no=0;
              $("input[type=checkbox]:checked").each(function () {
                var ceklis = $(this).attr('data-no')
                  selectedValues.push($('#PLU'+ceklis).val()); // Get the checkbox ID
                  keterangan.push($('#keterangan'+ceklis).val()); // Get the checkbox ID
              });                

              var mergedArray = [];
              for (var i = 0; i < selectedValues.length; i++) {
                  mergedArray.push({ keterangan: keterangan[i], PLU: selectedValues[i] });
              }

              console.log([mergedArray,selectedValues,keterangan])
               Swal.fire({
                    title: 'apakah data yang anda pilih sudah benar?',
                    icon: "question",
                    showCancelButton: true,
                    confirmButtonText: "Yes"
                }).then((result)=>{
                  if (result.isConfirmed) {
                    $.ajax({
                        type: "POST",
                        url: "StockConsignment.aspx/Reject", // URL of the WebMethod
                        data: JSON.stringify({ items: mergedArray }), // Send the selected values
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                          console.log(response)                          
                            let parsed = JSON.parse(response.d);                                                                         
                            if (parsed.code == 200) {
                              Swal.fire({
                                title: parsed.title,
                                text: 'Data berhasil di reject',
                                icon: "success",
                              }).then(()=>{
                                $('#go1').click();
                              })
                            }else{
                              Swal.fire({
                                title: parsed.title,
                                text: parsed.msg,
                                icon: "error",
                              })
                            }
                        },
                        error: function (error) {
                            console.log(error.responseText)
                        }
                    });
                  }
                })  
              
            }
            function approveSelected() {
            // Collect the IDs of the checked checkboxes
            var selectedValues = [];
            var PLU = [];
            var no=0;
            $("input[type=checkbox]:checked").each(function () {
              var ceklis = $(this).attr('data-no')
                selectedValues.push($('#PLU'+ceklis).val()); // Get the checkbox ID
                
            });
            Swal.fire({
                    title: 'apakah data yang anda pilih sudah benar?',
                    icon: "question",
                    showCancelButton: true,
                    confirmButtonText: "Yes"
                }).then((result)=>{
                  if (result.isConfirmed) {
                    $.ajax({
                        type: "POST",
                        url: "StockConsignment.aspx/Approve", // URL of the WebMethod
                        data: JSON.stringify({ items: selectedValues }), // Send the selected values
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (response) {
                          console.log(response)                          
                            let parsed = JSON.parse(response.d);                                                                         
                            if (parsed.code == 200) {
                              Swal.fire({
                                title: parsed.title,
                                text: 'Data berhasil di approve',
                                icon: "success",
                              }).then(()=>{
                                $('#go1').click();
                              })
                            }else{
                              Swal.fire({
                                title: parsed.title,
                                text: parsed.msg,
                                icon: "error",
                              })
                            }
                        },
                        error: function (error) {
                            console.log(error.responseText)
                        }
                    });
                  }
                })                        
            
        }
          </script>
</asp:Content>

