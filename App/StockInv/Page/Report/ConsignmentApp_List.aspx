<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"
AutoEventWireup="true" CodeFile="ConsignmentApp_List.aspx.cs"
Inherits="Page_Approval_ConsignmentApp_List" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Stock Consignment SKK
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Report
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
                    <asp:TableHeaderCell Width="140">No. Surat Jalan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Kode Barang</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Model</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130">Color</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Berat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70">Size</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100">Tanggal</asp:TableHeaderCell>                    
                    <asp:TableHeaderCell Width="300">Status</asp:TableHeaderCell>                    
                </asp:TableHeaderRow>
            </asp:Table>
            </div>            
            <div class="control-group pull-right">              
                <div class="controls">                  
                  <input runat="server" type="hidden" ClientIDMode="Static" name="SupplierSJ" id="SupplierSJ" value="">                  
                </div>
            </div>            
          </div>   
          <div class="modal fade" tableindex="-1" id="cekRejectS" role="dialog">
                <div class="modal-dialog modal-lg" role="document">
                    <!-- Modal content-->
                    <div class="modal-content">
                        <div class="modal-header" id="headerReject">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>                            
                        </div>
                        <div class="modal-body" id="modalR" style="width:100%;height: 100%; overflow-x: hidden;overflow-y: auto;">
                            <textarea id="alasanReject" class="form-control" readonly>

                            </textarea>
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
      
            
          
            function openModalR1(no){
                $('#cekRejectS').modal('show')
                $('#cekRejectS').on('shown.bs.modal', function(e) {

                    var div = $('#btn_btn'+no);
                    var dataKet = div.data('ket');
                    var PLU = div.data('plu');

                    console.log(dataKet)
                    $('#alasanReject').val(dataKet)
                    $('#headerReject').html(`<h4 class="modal-title">Alasan Reject ${PLU}</h4>`)
                })
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
           
          </script>
</asp:Content>

