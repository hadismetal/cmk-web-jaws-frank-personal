<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PKSV.aspx.cs" Inherits="Page_PKSV" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    PK / SV Approval by SM
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
                Search No PK / SV :
            </div>
            <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="250" placeholder="..." />
        </div>
        <div class="form-group">
            <div class="text-muted">
                Tipe :
            </div>
            <asp:DropDownList ID="tipe" runat="server" CssClass="form-control" Width="150">
                <asp:ListItem Value="">All</asp:ListItem>
                <asp:ListItem Value="PK">Packaging</asp:ListItem>
                <asp:ListItem Value="SV">Souvenir</asp:ListItem>
            </asp:DropDownList>
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
                <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                    <i class="icon-search"></i>
                    Display
                </button>
				<button id="excel" runat="server" class="btn btn-primary" onserverclick="excel_Click">
                    <i class="icon-search"></i>
                    Export
                </button>
            </div>
        </div>
    </form>
    <div class="table-responsive" style="width: 120%;">
        <asp:Table ID="tb" runat="server" CssClass="table table-bordered text-sm">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="200"></asp:TableHeaderCell>
                <asp:TableHeaderCell Width="25">No</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="50">Tipe</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Nomor</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Lokasi</asp:TableHeaderCell>
				<asp:TableHeaderCell Width="100">Sales</asp:TableHeaderCell>
				<asp:TableHeaderCell Width="100">Customer</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Keterangan</asp:TableHeaderCell>
				<asp:TableHeaderCell Width="100">Qty</asp:TableHeaderCell>
				<asp:TableHeaderCell Width="100">Item</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Status</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Operator</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Approval By</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Approval Tgl</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Rejected By</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Rejected Tgl</asp:TableHeaderCell>
				<asp:TableHeaderCell Width="100">Keterangan Reject</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>

    <!-- Modal Approve-->
    <div
        class="modal fade"
        id="modalApprove"
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
                    <h4 class="modal-title" id="myModalLabel">WARNING !!!
                    </h4>
                </div>
                <div class="modal-body">
                    Apakah anda yakin ingin approve
                    <label id="h_pksv"></label>
                    tersebut ? Jika anda sudah yakin, klik tombol 'Yakin', tika tidak klik tombol 'Batal'.
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">
                        Batal
                    </button>
                    <input type="hidden" id="h_id_appr" />
                    <input type="hidden" id="h_operator_nama_appr" />
                    <input type="hidden" id="h_status_appr" />
                    <input type="hidden" id="h_tipe_appr" />
                    <button
                        type="button"
                        class="btn btn-primary"
                        id="btn_sure_approve_new"
                        data-dismiss="modal" onclick="redirectToApprove();">
                        Yakin
                    </button>
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
		$(document).ready(function(){});
        function approve(e) {
            $('#h_id_appr').val($(e).attr('data-id'));
            $('#h_operator_nama_appr').val($('#HfOperatorNama').val());
            $('#h_status_appr').val('2');
            $('#h_tipe_appr').val($(e).attr('data-tipe'));
            $('#h_pksv').text($(e).attr('data-no'));
			
            $('#modalApprove').modal('show');
        }

        function reject(e) {
            $('#h_id_rjct').val($(e).attr('data-id'));
            $('#h_operator_nama_rjct').val($('#HfOperatorNama').val());
            $('#h_status_rjct').val('3');
            $('#h_tipe_rjct').val($(e).attr('data-tipe'));
            $('#lbl_no_pksv').text($(e).attr('data-no'));
			
            $('#modalReject').modal('show');
        }
		
		function checkKeteranganReject(){
			let kr = $('#keterangan_reject').val();
			if(kr === ''){
				alert('Keterangan Tidak Boleh Kosong');
			}
			else{
				redirectToReject();
			}
		}

        function redirectToApprove() {
			let id = $('#h_id_appr').val();
			let tipe = $('#h_tipe_appr').val(); 
			let operator_nama = $('#h_operator_nama_appr').val();
			let status = $('#h_status_appr').val();
			
            let newUrl = window.location.protocol + "//" + window.location.host + "/OutletSales/Page/Approval/PackagingSouvenir/PKSV_Approve.aspx";
            newUrl += "?id=" + id;
            newUrl += "&tipe=" + tipe;
            newUrl += "&operator_nama=" + operator_nama;
            newUrl += "&keterangan_reject=";
            newUrl += "&status=" + status;

            window.location.href = newUrl;
        }

        function redirectToReject() {
			let id = $('#h_id_rjct').val();
			let tipe = $('#h_tipe_rjct').val();
			let operator_nama = $('#h_operator_nama_rjct').val(); 
			let keterangan_reject = $('#keterangan_reject').val();
			let status = $('#h_status_rjct').val();
			
            let newUrl = window.location.protocol + "//" + window.location.host + "/OutletSales/Page/Approval/PackagingSouvenir/PKSV_Approve.aspx";
            newUrl += "?id=" + id;
            newUrl += "&tipe=" + tipe;
            newUrl += "&operator_nama=" + operator_nama;
            newUrl += "&keterangan_reject=" + keterangan_reject;
            newUrl += "&status=" + status;

            window.location.href = newUrl;
        }
    </script>
</asp:Content>
