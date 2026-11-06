<%@ Page EnableEventValidation="false" Language="C#"
    MasterPageFile="~/MasterPage.master" AutoEventWireup="true"
    CodeFile="index_print.aspx.cs" Inherits="DOKRM_index_print" %>
<asp:Content ContentPlaceHolderID="content" runat="server">
	<style>
		@media print {
    		body {
    		    visibility: visible; /* Hide everything by default */
				font-size: 12px;
    		}
		
    		.print-table {    		 
    		    width: 100%;    	
    		    table-layout: fixed;
				border-collapse: collapse;
				/* word-wrap: break-word;	     */
    		}		
			
			/* .print-table tbody tr td{
        		width:100%;				
    		}		 */

			.print-table th:nth-child(1), .print-table td:nth-child(1) {
        		width: 50px !important;
        		max-width: 50px;
        		text-align: left;
        		white-space: wrap;
        		overflow: hidden;        		
    		}
			.print-table th:nth-child(2), .print-table td:nth-child(2) {
				/* max-width: 50px !important;
        		text-align: left;
				font-size:9px;
        		white-space: normal;
        		overflow: hidden;        		
				word-wrap:break-word; */
				max-width: 130px !important; /* Ensures width doesn't shrink */				
        		text-align: left;
        		font-size: 9px;
        		white-space: normal !important; /* Forces wrapping */
        		word-break: break-word !important;
        		overflow-wrap: break-word !important;
    		}

			.print-table th:nth-child(3), .print-table td:nth-child(3) {
				/* max-width: 50px !important;
        		text-align: left;
				font-size:9px;
        		white-space: normal;
        		overflow: hidden;        		
				word-wrap:break-word; */
				max-width: 130px !important; /* Ensures width doesn't shrink */				
        		text-align: left;
        		font-size: 9px;
        		white-space: normal !important; /* Forces wrapping */
        		word-break: break-word !important;
        		overflow-wrap: break-word !important;
			}
			.print-table th:nth-child(4), .print-table td:nth-child(4) {        		
        		/* max-width: 50px !important;
        		text-align: left;
				font-size:9px;
        		white-space: normal;
        		overflow-wrap: break-word !important;
				word-wrap:break-word; */
				max-width: 130px !important; /* Ensures width doesn't shrink */				
        		text-align: left;
        		font-size: 9px;
        		white-space: normal !important; 
        		word-break: break-word !important;
        		overflow-wrap: break-word !important;
    		}
			/* .print-table td:nth-child(5) {        		
				max-width: 50px !important;
				text-align: left;
				font-size:9px;
				white-space: normal;
				overflow: hidden;        		
				word-break: break-all;
			} */
			.print-table td:nth-child(5) {
        		max-width: 130px !important; /* Ensures width doesn't shrink */
        		text-align: left;
        		font-size: 9px;
        		white-space: normal !important; /* Forces wrapping */
        		word-break: break-word !important;
        		overflow-wrap: break-word !important;
        		display: block; /* Helps force wrapping */
    		}
						
			

		
    		/* Remove scrollbars */
    		.print-table-container {
    		    overflow: visible !important;
    		}

			.print-table tr, .print-table td, .print-table th {
        		page-break-inside: avoid;
    		}

			@page {
				 margin: 10mm;
			}

			html, body {
        		width: 100%;
        		height: auto;
        		overflow: hidden; /* Prevents extra scrolling */
    		}
		}
	</style>
<div class="container">
	<div class="form-group">
		<table>
			<tr>
				<td>
					<p class="text-bold">
						Nomor DO KRM
					</p>
				</td>
				<td style="padding-left: 10px; padding-right: 10px">
					<p class="text-bold">:</p>
				</td>
				<td>
					<p class="text-bold" id="lbl_nokrm"></p>
				</td>
			</tr>
			<tr>
				<td>
					<p class="text-bold">
						Nama Kurir
					</p>
				</td>
				<td style="padding-left: 10px; padding-right: 10px">
					<p class="text-bold">:</p>
				</td>
				<td>
					<p class="text-bold" id="lbl_nama_kurir"></p>
				</td>
			</tr>
			<tr>
				<td>
					<p class="text-bold">
						Status DO
					</p>
				</td>
				<td style="padding-left: 10px; padding-right: 10px">
					<p class="text-bold">:</p>
				</td>
				<td>
					<p class="text-bold" id="lbl_status_do"></p>
				</td>
			</tr>
			<tr>
				<td>
					<p class="text-bold">
						Operator Nama
					</p>
				</td>
				<td style="padding-left: 10px; padding-right: 10px">
					<p class="text-bold">:</p>
				</td>
				<td>
					<p class="text-bold" id="lbl_operator"></p>
				</td>
			</tr>
			<tr>
				<td>
					<p class="text-bold">
						Operator Tgl
					</p>
				</td>
				<td style="padding-left: 10px; padding-right: 10px">
					<p class="text-bold">:</p>
				</td>
				<td>
					<p class="text-bold" id="lbl_operator_tgl"></p>
				</td>
			</tr>
			<tr>
				<td>
					<p class="text-bold">
						Estimation Time Departure
					</p>
				</td>
				<td style="padding-left: 10px; padding-right: 10px">
					<p class="text-bold">:</p>
				</td>
				<td>
					<p class="text-bold" id="lbl_departure"></p>
				</td>
			</tr>
			<tr>
				<td>
					<p class="text-bold">
						Keterangan
					</p>
				</td>
				<td style="padding-left: 10px; padding-right: 10px">
					<p class="text-bold">:</p>
				</td>
				<td>
					<p class="text-bold" id="lbl_keterangan"></p>
				</td>
			</tr>
			<tr>
				<td style="vertical-align:top">
					<p class="text-bold">
						Keterangan Titipan
					</p>
				</td>
				<td style="padding-left: 10px; padding-right: 10px; vertical-align:top">
					<p class="text-bold">:</p>
				</td>
				<td id="lbl_keterangan_titipan" style="vertical-align:top">
				</td>
			</tr>
		</table>
		<div class="row">
			<div class="table-responsive text-sm col-md-8 print-table-container">
				<table id="tb_resultdetail" class="table table-bordered print-table">
					<thead>
						<tr>
							<th>No.</th>
							<th>No Krm</th>
							<th class="content-text" >Lokasi Asal</th>
							<th class="content-text" >Lokasi Tujuan</th>
							<th class="content-text" >Keterangan</th>
							<th>Qty</th>
							<!--<th>Gross Weight</th>-->
							<!--<th>Estimasi Harga Jual</th>-->
						</tr>
					</thead>
					<tbody id="tbody-data-detail">
						
					</tbody>
				</table>
			</div>
		</div>
	</div>
	<div>
		<div style="float:right;">
			<br />
			<table style="border:solid 1px black; border-collapse:collapse;">
				<tr>
					<td style="width:150px;text-align:center;border:solid 1px black; ">Pembawa<br /><br /><label id="lblkurir"></label><br /><br /><br /></td>
					<td style="width:150px;text-align:center;border:solid 1px black; ">Mengetahui<br /><br /><br /><br /><br /></td>
					<td style="width:150px;text-align:center;border:solid 1px black; ">Penerima<br /><br /><br /><br /><br /></td>
				</tr>
			</table>
		</div>
		<div style="clear:both" />
	</div>
	<script>
		$(document).ready(function(){
			onLoad();
		});
		
		function onLoad(){
			const urlParams = new URLSearchParams(window.location.search);
			const id = urlParams.get('id');
			$.ajax({
                type: "GET",
                url: "./index_print.aspx/GetDetailKRM",
                data: {
					IDF: id
				},
                contentType: "application/json; charset=utf-8",
                beforeSend: function (request) {
                    request.withCredentials = false;
                },
                success: function (data) {
                    let response = data.d;
					if(response.status == 1){
						let resheader = response.header;
						let resdetail = response.header.detail;
						$('#lbl_keterangan').text(resheader.keterangan);
						$('#lbl_nama_kurir').text(resheader.namakurir);
						$('#lbl_status_do').text(resheader.statusdo);
						$('#lbl_operator').text(resheader.operatornama);
						$('#lbl_operator_tgl').text(resheader.operatortgl);
						$('#lbl_departure').text(resheader.etd);
						$('#lbl_nokrm').text(resheader.nomordo);
						$('#lblkurir').text(resheader.namakurir);
						
						$('#lbl_keterangan_titipan').children().remove();
						$('#lbl_keterangan_titipan').append(resheader.keterangan_titipan);
						
						$('#tbody-data-detail > tr').remove();
						if (resdetail.length > 0) {
							let totalbrg = 0;
							$(resdetail).each(function (index) {
								$("#tbody-data-detail").append(`
							<tr>
								<td>${index + 1}</td>
								<td>${resdetail[index].nokrm}</td>
								<td>${resdetail[index].lokasi_asal}</td>
								<td>${resdetail[index].lokasi_tujuan}</td>
								<td>${resdetail[index].keterangan}</td>
								<td>${resdetail[index].qty_outgoing}</td>
							</tr>
							`);
								totalbrg += parseInt(resdetail[index].qty_outgoing);
							});
							
							$("#tbody-data-detail").append(`
							<tr>
								<td colspan="5"><b>Total</b></td>
								<td><b>${totalbrg}</b></td>
							</tr>
							`)
						}
						window.print();
					}
					else{
						toastr.error("Terjadi kesalahan!");
					}
				},
                error: function (error) {
                    console.error(error);
                    toastr.error("Terjadi kesalahan!");
                },
			});
		}
	</script>
</div>
</asp:Content>