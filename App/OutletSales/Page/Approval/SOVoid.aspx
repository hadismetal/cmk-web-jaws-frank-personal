<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"
AutoEventWireup="true" CodeFile="SOVoid.aspx.cs"
Inherits="Page_Approval_SOVoid" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Void SalesOrder
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Approval
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <script>
        $(document).ready(function () {
            $('#<%=start.ClientID%>').datepicker({
                //viewMode: 'months',
                format: 'dd-mm-yyyy'
            });

            $('#<%=end.ClientID%>').datepicker({
                //viewMode: 'months',
                format: 'dd-mm-yyyy'
            });
        });
    </script>
    <form runat="server" class="form-inline">
        <asp:HiddenField runat="server" ID="HfOperatorNama" ClientIDMode="Static" />
        <div class="form-group">
            <div class="text-muted">
                Search No Sales Order :
            </div>
            <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="250" placeholder="..." />
        </div>        
        <div class="form-group">
            <div class="text-muted">
                Start :
            </div>
            <asp:TextBox CssClass="form-control text-center" Width="150" type="text" placeholder="" ID="start" runat="server" data-date-format="dd-MM-yyyy" />
        </div>
        <div class="form-group">
            <div class="text-muted">
                End :
            </div>
            <asp:TextBox CssClass="form-control text-center" Width="150" type="text" placeholder="" ID="end" runat="server" data-date-format="dd-MM-yyyy" />
        </div>
		<div class="form-group">
            <div class="text-muted">
                Location :
            </div>
            <asp:DropDownList ID="loccode" runat="server" CssClass="form-control" Width="150">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Status :
            </div>
            <asp:DropDownList ID="status" runat="server" CssClass="form-control" Width="150">
                <asp:ListItem Value="0">All</asp:ListItem>
                <asp:ListItem Value="1">Pending</asp:ListItem>
                <asp:ListItem Value="2">Approved</asp:ListItem>
                <asp:ListItem Value="3">Reject</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="go" ClientIDMode='Static' runat="server" class="btn btn-primary" onserverclick="refreshPage">
                    <i class="icon-search"></i>
                    Display
                </button>				      
            </div>
        </div>
    </form>
    <div class="table-responsive">
        <asp:Table ID="tb" runat="server" CssClass="table table-bordered text-sm" Style="width: 100%;">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="150"></asp:TableHeaderCell>
                <asp:TableHeaderCell Width="25">No</asp:TableHeaderCell>                
                <asp:TableHeaderCell Width="100">Nomor</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Lokasi</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Keterangan Void</asp:TableHeaderCell>				
				<asp:TableHeaderCell Width="100">JR</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Customer</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Operator</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Status</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Approval By</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Approval Tgl</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Rejected By</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Rejected Tgl</asp:TableHeaderCell>
				<asp:TableHeaderCell Width="100">Keterangan Reject</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
    <div style="display: none;" id="startROW" ClientIDMode="Static" runat="server">1</div> 
    
    <div class="btn-group" role="group" id="paginationGRP" runat="server" ClientIDMode="Static">
                
    </div>

    <!-- Modal Approve-->
    <div class="modal fade" tableindex="-1" id="modalRejectM" role="dialog">
        <div class="modal-dialog modal-lg" role="document">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Reject</h4>
                </div>
                <div class="modal-body" id="modalR">
                </div>
                <div class="modal-footer" id="kakiModal">                                                                            
                </div>
            </div>
        </div>
    </div>

    <!-- Modal Reject-->
    <div
        class="modal fade"
        id="modalReject"
        role="dialog">
        <div class="modal-dialog modal-lg">
            <div class="modal-content">
                <div class="modal-header">
                    <button
                        type="button"
                        class="close"
                        data-dismiss="modal"
                        aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    <h4 class="modal-title" id="myModalReject">WARNING !!!
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label id="lbl_no_pksv">csasd</label>
                    </div>
                    <div class="form-group">
                        <label for="keterangan_reject">Keterangan Reject</label>
                        <textarea class="form-control" id="keterangan_reject"></textarea>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Batal
                    </button>
                    <input type="hidden" id="h_id_rjct" />
                    <input type="hidden" id="h_operator_nama_rjct" />
                    <input type="hidden" id="h_status_rjct" />
                    <input type="hidden" id="h_tipe_rjct" />
                    <button
                        type="button"
                        class="btn btn-primary"
                        id="btn_sure_reject_new" onclick="checkKeteranganReject(this);">
                        Yakin
                    </button>
                </div>
            </div>
        </div>
    </div>
    <script>	
      function approveSelected(ID){
        Swal.fire({
            title: 'apakah data yang anda pilih sudah benar?',
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Yes"
        }).then((result)=>{
          if (result.isConfirmed) {
            $('#btnWrap').css('display','none')            
            $('#bungkusanBTN'+ID).html(`<img id="loading" src='/Images/loading/!loading.gif' style="padding-top:12px;"/>`)
            
            $.ajax({            
                type: "POST",
                url: "SOVoid.aspx/Approve", // URL of the WebMethod
                data: JSON.stringify({IDAppr:ID}), // Send the selected values
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {
                  console.log(response)                          
                  
                    let parsed = JSON.parse(response.d);                                                                         
                    if (parsed.code == 200) {
                      Swal.fire({
                        title: parsed.title,
                        text: 'Data berhasil di approve dan SO berhasil di void.',
                        icon: "success",
                      }).then(()=>{
                        redirectWithoutParam();
                        
                      })
                    }else if(parsed.code == 400){
                        Swal.fire({
                        title: parsed.title,
                        text: `Pengajuan telah di reject sehubungan dengan "${parsed.msg}"".`,
                        icon: "success",
                      }).then(()=>{
                        redirectWithoutParam();
                        
                      })
                    }else{
                     $('#loading').remove();
                     $('#btnWrap').removeAttr('style',true);
                      Swal.fire({
                        title: parsed.title,
                        text: parsed.msg,
                        icon: "error",
                      })
                    }
                },
                error: function (error) {
                    $('#loading').remove();
                    $('#btnWrap').removeAttr('style',true);
                    console.log(error.responseText)
                }
            });
          
        }
        })  
      }
      function rejectSelected(IDAppr){
        var ketVoid = $(`#keterangan${IDAppr}`).val();
        var noso = $(`#noso${IDAppr}`).val();
        Swal.fire({
            title: 'apakah anda yakin untuk me-reject?',
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Yes"
        }).then((result)=>{
          if (result.isConfirmed) {
            $('#bungkusanBTN'+IDAppr).html(`<img id="loading" src='/Images/loading/!loading.gif' style="padding-top:12px;"/>`)            
            $.ajax({
                type: "POST",
                url: "SOVoid.aspx/Reject", // URL of the WebMethod
                data: JSON.stringify({IDAppr:IDAppr,ketVoid:ketVoid,noso:noso}), // Send the selected values
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {                  
                  
                    let parsed = JSON.parse(response.d);                                                                         
                    if (parsed.code == 200) {                        
                      Swal.fire({
                        title: parsed.title,
                        text: `Pengajuan void dengan nomor SO ${noso} berhasil di reject.`,
                        icon: "success",
                      }).then(()=>{
                        redirectWithoutParam();
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

      function redirectWithoutParam() {
        let url = new URL(window.location.href);
        // url.searchParams.delete(paramToRemove); // Remove the parameter
        let newUrl = url.pathname ; // Ensure clean URL without trailing '?'          
        
        // if (newUrl.endsWith("?")) {
        //     newUrl = newUrl.slice(0, -1); // Remove the trailing '?'
        // }
    
        window.location.href = newUrl; // Redirect to the clean URL
      }
        
        function openModalR(IDAppr){
            $('#modalRejectM').modal('show')
            $('#modalRejectM').on('shown.bs.modal', function() {            
            let html = ''            
                // Check if the checkbox is checked
                var div = $(`#btnReject${IDAppr}`)
                var dataNoSo = div.data('noso')                
                
                    html += `                             
                            <div class="container " style="margin-top: 10px;">
                                <div class="form-group ">
                                    <label for="exampleInputName2">Alasan Reject permintaan Void ${dataNoSo}</label>
                                    <textarea style="width: 100%;resize:none;" name="alasan${IDAppr}" id="keterangan${IDAppr}" class="form-control"></textarea>
                                    <input type='hidden' value='${dataNoSo}' id='noso${IDAppr}' />
                                </div>                                
                            </div>                             
                        `;        
                    let html1=`<button runat="server" id="buttonRject" ClientIDMode="Static" onclick="rejectSelected(${IDAppr}); return false;" class="btn btn-danger button-loading btn-default">Reject</button>
                                <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>`;
            $('#modalR').html(html)
            $('#kakiModal').html(html1)

        });
    }				
    </script>
</asp:Content>

