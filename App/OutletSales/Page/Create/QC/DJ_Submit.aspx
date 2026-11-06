<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="DJ_Submit.aspx.cs" Inherits="Page_Create_QC_DJ_Submit" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewelery
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    QC
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
	<style>
		    .overlay {
				position: fixed;
				top: 0;
				left: 0;
				width: 100%;
				height: 100%;
				background: rgba(0,0,0,0.5);
				z-index: 9999;
				display: none;
				align-items: center;
				justify-content: center;
				color: white;
				font-size: 24px;
			}

			.spinner {
				border: 6px solid #f3f3f3;
				border-top: 6px solid #3498db;
				border-radius: 50%;
				width: 50px;
				height: 50px;
				animation: spin 1s linear infinite;
			}

			@keyframes spin {
				0% { transform: rotate(0deg); }
				100% { transform: rotate(360deg); }
			}
	</style>
	<div id="overlay" class="overlay">
		<div class="spinner"></div>
		<div style="margin-top: 10px;">Loading...</div>
	</div>
    <form id="Form1" runat="server">
		<div class="panel panel-default width-500">
			<div class="panel-heading">
				Information PLU
			</div>
			<div class="panel-body">
				<div class="form-inline">
					<div class="col-xs-6">
						<div class="form-group">
							<table class="table-file">
								<tr rowspan="2">
									<td class="text-lg text-muted">PLU</td>
									<td class="text-lg">:</td>
									<td class="text-lg" id="header_plu" runat="server"></td>
								</tr>
								<tr rowspan="2">
									<td class="text-lg text-muted">Item</td>
									<td class="text-lg">:</td>
									<td class="text-lg" id="header_item" runat="server"></td>
								</tr>
							</table>
						</div>
					</div>
					<div class="col-xs-6">
						<div class="form-group">
							<span class="text-muted">Picture :</span><br />
							<asp:Image ID="imgpicture" runat="server" CssClass="img-thumbnail" Height="100" />
						</div>
					</div>
				</div>
			</div>
		</div>
        <div class="panel panel-default">
			<div class="panel-heading">
				QC Product
			</div>
            <div class="panel-body">
				<div class="col-xs-6">
					<table class="table-file">
						<tr>
							<td><label class="control-label">Petugas QC</label></td>
							<td>
								<asp:TextBox ID="petugasqc" runat="server" CssClass="form-control input-sm" Enabled="false" />
								<small runat="server" id="lblerrorpetugasqc" class="text-danger" visible="false"></small>
							</td>
						</tr>
						<tr>
							<td>
								<label class="control-label" style="width:75px">Tipe Lokasi QC</label>
							</td>
							<td>
								<asp:DropDownList runat="server" ID="tipe_lokasi" CssClass="form-control" OnSelectedIndexChanged="Location_Changed" AutoPostBack="true">
								</asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td><label class="control-label" style="width:75px">Lokasi QC</label></td>
							<td>
								<asp:DropDownList runat="server" ID="lokasi" CssClass="form-control" >
								</asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td><label class="control-label">Gross Weight</label></td>
							<td>
								<div class="input-group">
									<asp:textbox id="grossweightoriginal" runat="server" cssclass="form-control text-lg text-center" Enabled="false" />
									<span class="input-group-addon">Gram</span>
								</div>
								<center><span class="glyphicon glyphicon-arrow-down" aria-hidden="true"></span></center>
								<div class="input-group">
									<asp:textbox id="grossweight" runat="server" cssclass="form-control text-lg text-center" />
									<span class="input-group-addon">Gram</span>
								</div>
							</td>
						</tr>
						<tr>
							<td><label class="control-label">Grafir Logo</label></td>
							<td><asp:DropDownList ID="logo" runat="server" CssClass="form-control"></asp:DropDownList></td>
						</tr>
					</table>
				</div>
				<div class="col-xs-6">
					<table class="table-file">
						<tr>
							<td><label class="control-label">Grafir Kadar Emas</label></td>
							<td><asp:DropDownList ID="kadaremas" runat="server" CssClass="form-control"></asp:DropDownList></td>
						</tr>
						<tr>
							<td><label class="control-label">Frame Color</label></td>
							<td><asp:DropDownList ID="framecolor" runat="server" CssClass="form-control"></asp:DropDownList></td>
						</tr>
						<tr>
							<td><label class="control-label">Fisik Barang Keseluruhan</label></td>
							<td><asp:DropDownList ID="fisik" runat="server" CssClass="form-control"></asp:DropDownList></td>
						</tr>
						<tr>
							<td><label class="control-label">Keterangan</label></td>
							<td><asp:TextBox ID="keterangan" runat="server" TextMode ="MultiLine" CssClass="form-control input-sm" Height=85 /></td>
						</tr>
					</table>
				</div>
            </div>
        </div>
        <div class="panel panel-default">
			<div class="panel-heading">
				QC Stone
			</div>
			<div class="panel-body">
				<div class="table-responsive">
					<asp:Table ID="tb" runat="server" CssClass="table">
						<asp:TableHeaderRow>
							<asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
							<asp:TableHeaderCell Width="250">Stone</asp:TableHeaderCell>
							<asp:TableHeaderCell Width="90">Tgl.</asp:TableHeaderCell>
							<asp:TableHeaderCell Width="130" CssClass="text-center">Total Butir</asp:TableHeaderCell>
							<asp:TableHeaderCell Width="130" CssClass="text-center">Total Carat</asp:TableHeaderCell>
							<asp:TableHeaderCell Width="130" CssClass="text-center">Total Butir Actual</asp:TableHeaderCell>
							<asp:TableHeaderCell Width="130" CssClass="text-center">Total Carat Actual</asp:TableHeaderCell>
							<asp:TableHeaderCell Width="100" CssClass="text-center">Sesuai Sertifikat</asp:TableHeaderCell>
							<asp:TableHeaderCell Width="200" CssClass="text-center">Keterangan</asp:TableHeaderCell>
						</asp:TableHeaderRow>
					</asp:Table>
				</div>
			</div>
		</div>
        <div class="button-left">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click" onclick="showOverlay();">
                <i class="icon-check"></i>
                Submit
            </button>
			<small runat="server" id="lblwarning" class="text-danger" visible="false"></small>
        </div>
    </form>
	<script>
		document.addEventListener('DOMContentLoaded', function () {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-bottom-center",
                "preventDuplicates": false,
                "onclick": null,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "3000",
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
		});

        function showOverlay() {
            document.getElementById("overlay").style.display = "flex";
        }
    </script>
</asp:Content>

