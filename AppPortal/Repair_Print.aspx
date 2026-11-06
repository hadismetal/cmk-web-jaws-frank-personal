<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Repair_Print.aspx.cs" Inherits="Page_Repair_1" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
	<title> Point of Sales </title><meta name="viewport" content="width=device-width, initial-scale=1.0">

	<!-- CSS -->
	<link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap@3.3.7/dist/css/bootstrap.min.css">
	
	<style>
		html { overflow-y: scroll; }
		h1 { border-bottom: dotted 2px #CCC; padding-bottom: 25px; margin-bottom: 25px; }
		hr { border-top: dotted 1px #CCC; }
		label { font-weight: normal; }

		/* Callout System */
		div#callout { position: fixed; top: 25px; left: 50%; width: 250px; margin-left: -125px; z-index: 10000; text-align: center; display: none; }

		/* MasterPage : PAGE */
		body.page { padding: 35px 0px 20px 0px; }
		body.page h1 #module { font-size: 12pt; font-weight: bold; color: orange; }

		/* MasterPage : FILE */
		body.file { padding: 0px 0px 20px 0px; }
		body.file h1 code { font-size: 16pt; margin-top: 7px; }
		body.file div#callout { top: 78px; }

		/* MasterPage : REPORT */
		body.report { padding: 0px 15px 20px 15px; }
		body.report { margin-top: -15px; }
		body.report h1 #module { font-size: 12pt; font-weight: bold; color: orange; }
		body.report #output { border-top: dotted 2px #CCC; padding-top: 25px; margin-top: 25px; font-style: italic; }
		body.report #ttd li { padding-left: 0px; }

		/* Custom Layout */
		div.button-inline { border-top: dotted 2px #CCC; padding-top: 25px; margin-top: 25px; }
		div.button-inline-noborder {  }
		div.button-ap a.btn { width: 150px; text-align: left; }
		div.button-left button { width: 200px; text-align: left; }
		div.button-left a.btn { width: 158px; text-align: left; }
		div.table-responsive { margin-top: 30px; }
		div.form-separator { margin-top: 3px; }

		/* Custom Width */
		.width-100 { max-width: 100px; }
		.width-150 { max-width: 150px; }
		.width-200 { max-width: 200px; }
		.width-250 { max-width: 250px; }
		.width-300 { max-width: 300px; }
		.width-400 { max-width: 400px; }
		.width-500 { max-width: 500px; }
		.width-600 { max-width: 600px; }
		.width-700 { max-width: 700px; }
		.width-800 { max-width: 800px; }
		.width-900 { max-width: 900px; }
		.fixed-75 { width: 75px; }
		.fixed-100 { width: 100px; }
		.fixed-120 { width: 120px; }
		.fixed-150 { width: 150px; }

		/* Hanging */
		.hang-10 { padding-left: 10px; }
		.hang-20 { padding-left: 20px; }
		.hang-30 { padding-left: 30px; }
		.hang-40 { padding-left: 40px; }
		.hang-50 { padding-left: 50px; }

		/* Custom Attributes */
		.text-red { color: Red; }
		.text-orange { color: Orange; }
		.text-silver { color: #DDD; }
		.text-draft { color: mediumpurple; }
		.text-validator { color: violet; }
		.text-bold { font-weight: bold; }
		.text-nobold { font-weight: normal; }
		.text-nowrap { white-space: nowrap; }
		.text-addheight { line-height: 1.5; }
		.text-subtotal { color: hotpink; }
		.text-grandtotal { color: saddlebrown; }
		.text-price { color: purple; }

		/* Custom Size */
		.text-00 { font-size: 3pt; }
		.text-xs { font-size: 7pt; }
		.text-sm { font-size: 9pt; }
		.text-ms { font-size: 12pt; }
		.text-md { font-size: 14pt; }
		.text-lg { font-size: 16pt; }
		.text-xl { font-size: 30pt; }

		/* Custom List */
		ol.wizard-step { margin: 0px; padding: 0px; list-style: none; }
		ol.wizard-step li { float: left; margin-right: 30px; color: #CCC; padding-bottom: 5px; }
		ol.wizard-step li.active { color: #999; border-bottom: solid 3px yellowgreen; }

		/* Checkbox */
		div.cb-single input { vertical-align: top; margin-right: 5px; }
		div.cb-single label strong { display: block; margin-bottom: -3px; }
		div.cb-single label small { color: #999; }
		div.cb-multi p { margin-bottom: 2px; }
		div.cb-multi label { font-weight: bold; padding-left: 5px; padding-right: 10px; }
		div.cb-multi td { padding: 0px; }
		div.cb-multi-normal p { margin-bottom: 2px; }
		div.cb-multi-normal label { padding-left: 5px; padding-right: 10px; }
		div.cb-multi-normal td { padding: 0px; }

		/* Table : File */
		div.file-subheader { float: right; width: 150px; text-align: right; font-size: 16pt; color: powderblue; margin-top: -10px; }
		div.file-subheader2 { float: left; width: 150px; text-align: left; font-size: 16pt; color: red; margin-top: -10px; }
		table.table-file { margin-top: -5px; }
		table.table-file-nomargin { margin-top: 0px; }
		table.table-file td { padding: 5px 10px 0 0px; vertical-align: top; }
		table.table-file td.text-label { color: #999; width: 125px; }

		/* Table : Form */
		table.table-form { margin-top: -15px; margin-bottom: 15px; }
		table.table-form-nomargin { margin-bottom: 0px; }
		table.table-form td { padding: 15px 10px 0px 0px; vertical-align: top; }

		/* Table : List */
		table.table-list { table-layout: fixed; width: 0; margin-bottom: 25px; border-bottom: solid 1px darkcyan; background-color: #FFF }
		table.table-list th { padding: 9px 5px 9px 5px; vertical-align: middle; border: solid 1px darkcyan; color: darkcyan; text-align: center; }
		table.table-list td { padding: 9px 5px 9px 5px; vertical-align: top; border: dotted 1px #CCC; line-height: 1.2; margin-right: 3px; text-align: right; }
		table.table-list tr.no-result td { text-align: center; font-size: 20pt; color: silver; padding-top: 30px; padding-bottom: 30px; }
		table.table-list tr.no-header th { border-bottom: dotted 1px #CCC; }
		table.table-list tr.sub-header td { border-top: solid 1px #CCC; border-bottom: solid 1px #CCC; }
		table.table-list tr.text-grandtotal td {  }
		table.table-list tr.vertical-top td { vertical-align: top; }
		table.table-list tr:hover td { background-color: aliceblue; }
		table.table-list p { margin-bottom: 0px; }
		table.table-list th small { color: silver; font-size: 8pt; }
		table.table-list td.paragraph p { margin-bottom: 5px; }

		/* Table : Report */
		table.table-report { table-layout: fixed; width: 0; margin-bottom: 25px; border-bottom: solid 1px darkcyan; }
		table.table-report th { vertical-align: middle; border-bottom: solid 1px darkcyan; color: darkcyan; }
		table.table-report td { vertical-align: top; border-bottom: dotted 1px #CCC; }
		table.table-report tr.no-result td { text-align: center; font-size: 20pt; color: silver; padding-top: 30px; padding-bottom: 30px; }
		table.table-report th small { color: silver; font-size: 8pt; }
	</style>
</head>
<body>
	<div>
		<style>
			h2 { margin-top : 0px; }
			td { vertical-align: top; }
			.row { margin-left: 0px; margin-right: 0px; }
		</style>
		<div class="row">
			<div class="col-xs-12">
				<table border="0" cellpadding="0" cellspacing="0" style="width:100%;">
					<tr>
						<td style="width:33%; text-align:left;">
							<asp:Image ID="img_logo_web" runat="server" border="0" style="height:50px;"/>
						</td>
						<td style="width:33%; text-align:center;">
							<span style="font-size:22px; font-weight:bold;">&nbsp;Repair Work Order&nbsp;</span><br />
							<span style="font-size:20px;"><asp:Literal runat="server" ID="lit_nomor_repair" /></span>
						</td>
						<td style="width:33%; text-align:right;">
							<asp:Literal id="cabang" runat="server"></asp:Literal>
						</td>
					</tr>
				</table>
				<hr />
			</div>
		</div>
		<div class="row">
			<div class="col-xs-4">
				<h3>Repair Information</h3>
				<table class="table-file">
					<tr>
						<td class="text-label">Tanggal Masuk</td>
						<td>:</td>
						<td id="tgl" runat="server" />
					</tr>
					<tr>
						<td class="text-label">Customer</td>
						<td>:</td>
						<td id="customer" runat="server" />
					</tr>
					<tr>
						<td class="text-label">JR</td>
						<td>:</td>
						<td id="jpjr" runat="server" />
					</tr>
				</table>
				<h3>Product Information</h3>
				<table class="table-file">
					
					<tr>
						<td class="text-label">No. PLU</td>
						<td>:</td>
						<td colspan="2" id="noplu" runat="server" />
						<td></td>
					</tr>
				   <tr>
						<td class="text-label">Material</td>
						<td>:</td>
						<td colspan="2" id="material" runat="server" />
						<td></td>
					</tr>
					<tr>
						<td class="text-label">Kode Karet</td>
						<td>:</td>
						<td colspan="2" id="kodeKaret" runat="server" />
						<td></td>
					</tr>
					<tr>
						<td class="text-label">Item</td>
						<td>:</td>
						<td colspan="2" id="productitem" runat="server" />
						<td></td>
					</tr>
					<tr>
						<td class="text-label">Gross Weight</td>
						<td>:</td>
						<td class="text-right" style="width:110px;" id="td_grossweight" runat="server" />
						<td class="text-silver"><small>Gram</small></td>
						<td></td>
					</tr>
					<tr>
						<td class="text-label">Total Butir (+)</td>
						<td>:</td>
						<td class="text-right" id="totalbutir" runat="server" />
						<td class="text-silver"><small>Butir</small></td>
						<td></td>
					</tr>
					<tr>
						<td class="text-label">Diameter</td>
						<td>:</td>
						<td class="fixed-75 text-right" id="dimensid" runat="server" />
						<td class="text-silver"><small>MM</small></td>
						<td></td>
					</tr>
					<tr>
						<td class="text-label">Ring Size</td>
						<td>:</td>
						<td class="fixed-75 text-right" id="dimensir" runat="server" />
						<td class="text-silver"><small></small></td>
						<td></td>
					</tr>
					<tr>
						<td class="text-label"><br /><br /></td>
						<td class="text-silver"><small></small></td>
						<td></td>
					</tr>
					<tr>
						<td class="text-label">Keterangan</td>
						<td>:</td>
						<td colspan="3" id="ket" style="border:solid 1px black; padding:3px; font-size:20px" runat="server" />
					</tr>
					
				</table>

			</div>
			<div class="col-xs-4">
				<div style="text-align:center">
					<span class="text-muted">Original :</span><br />
					<asp:Image ID="imgpicturebefore" runat="server" class="img-thumbnail" style="max-height:200px;max-width:200px" />
				</div>
				<div style="text-align:center" padding-left:10px;">
					<span class="text-muted">Repair :</span><br />
					<asp:Image ID="imgpicturerepair" runat="server" class="img-thumbnail" style="max-height:200px;max-width:200px" />
				</div>
				<div style="clear:both;"></div>
			</div>
			<div class="col-xs-4">
				<div style="float:right;">
					<div style="padding-bottom:20px; width:100%;">
						<div class="pull-right">
							<table class="text-right" border="0" cellpadding="5" cellspacing="0" style="font-size:18px;">
								<tr>
									<td>Estimasi Harga</td>
									<td style="border:solid 1px black; width:00px;"><span id="estimasi" runat="server"></span></td>
								</tr>
							
							</table>
						</div>
						<div style="clear:both;"></div>
					</div>
					<div>
						<div style="padding:15px; border:solid 1px black;">
						<table class="text-left" border="0" cellpadding="2" cellspacing="2" style="font-size:12px;">
							<tr>
								<td style="text-align:left; font-weight:bold; font-style:italic;">Sebelum</td>
								<td style="text-align:center;" colspan="3">TOKO</td>
								<td></td>
								<td style="text-align:center;" colspan="3">CS</td>
							</tr>
							<tr>
								<td></td>
								<td style="text-align:center;" >YA</td>
								<td></td>
								<td style="text-align:center;" >TIDAK</td>
								<td></td>
								<td style="text-align:center;" >YA</td>
								<td></td>
								<td style="text-align:center;" >TIDAK</td>
							</tr>
							<tr>
								<td>Berlian telah ditest keasliannya</td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
							</tr>
							<tr>
								<td>Berlian dalam kondisi baik</td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
							</tr>
							<tr>
								<td>Rangka dalam kondisi baik</td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
							</tr>
						</table>
						<table class="text-left" border="0" cellpadding="2" cellspacing="2" style="font-size:12px; margin-top:25px;">
							<tr>
								<td style="text-align:left; font-weight:bold; font-style:italic;">Sesudah</td>
								<td style="text-align:center;" colspan="3">TOKO</td>
								<td></td>
								<td style="text-align:center;" colspan="3">CS</td>
							</tr>
							<tr>
								<td></td>
								<td style="text-align:center;" >YA</td>
								<td></td>
								<td style="text-align:center;" >TIDAK</td>
								<td></td>
								<td style="text-align:center;" >YA</td>
								<td></td>
								<td style="text-align:center;" >TIDAK</td>
							</tr>
							<tr>
								<td>Berlian telah ditest keasliannya</td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
							</tr>
							<tr>
								<td>Berlian dalam kondisi baik</td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
							</tr>
							<tr>
								<td>Rangka dalam kondisi baik</td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
								<td></td>
								<td style="border:solid 1px black; width:50px;"></td>
							</tr>
						</table>
						</div>
					</div>
					<div style="clear:both;"></div>
					<div class="col-xs-6">
						<div style="padding:15px;">
							<h3>Stone Stock</h3>
								<asp:Table ID="tb3" runat="server" CssClass="table-list text-sm table-responsive">
									<asp:TableHeaderRow>
									<asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
									<asp:TableHeaderCell Width="50" CssClass="text-right">Carat / Butir</asp:TableHeaderCell>
									<asp:TableHeaderCell Width="50" CssClass="text-center">Total Butir</asp:TableHeaderCell>
									<asp:TableHeaderCell Width="50" CssClass="text-center">Total Carat</asp:TableHeaderCell>
									<asp:TableHeaderCell Width="75" visible="true">Keterangan</asp:TableHeaderCell>
								</asp:TableHeaderRow>
							</asp:Table>
						</div>
					</div>
				</div>
				<div style="clear:both;"></div>
			</div>
		</div>
		<div class="row">
			<div class="col-xs-6 pull-left">
				<h3>List Repair</h3>
				<asp:Table ID="tb2" runat="server" CssClass="table-list" Style="width:100%;">
					<asp:TableHeaderRow>
						<asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="150">Item</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="80">Biaya</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="75">Keterangan</asp:TableHeaderCell>
					</asp:TableHeaderRow>
				</asp:Table>
			</div>
			<div class="col-xs-6 pull-right">
				<h3>Stone</h3>
				<asp:Table ID="tb" runat="server" CssClass="table-list" Style="width:100%;">
					<asp:TableHeaderRow>
						<asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="50" CssClass="text-right">Carat / Butir</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="50" CssClass="text-center">Total Butir</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="50" CssClass="text-center">Total Carat</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="75">Keterangan</asp:TableHeaderCell>
					</asp:TableHeaderRow>
				</asp:Table>
			</div>
		</div>
		<div class="row">
			<div class="col-xs-12">
				<br /><br /><br />
				<div style="float: right;">
					<%--<div style="width: 300px; text-align: center; padding: 30px; float: right;">
						<u><b>
							<asp:Literal ID="adminsales" runat="server"></asp:Literal></b></u><br />
						Admin Sales<br />
						<i>Staff Penjualan</i>
					</div>--%>

					<div style="width: 300px; text-align: center; padding: 30px; float: right;">
						<u><b>
							<asp:Literal ID="jp" runat="server"></asp:Literal></b></u><br />
						Jewellery Representative
					</div>
					<div style="width: 300px; text-align: center; padding: 30px; float: right;">
						<u><b>
							<asp:Literal ID="customer2" runat="server"></asp:Literal></b></u><br />
						Customer /<i> Pelanggan </i>
					</div>
				</div>
			</div>
		</div>
		<div class="row">
			<div class="col-xs-12" style="font-size:9px;">
				catatan:
				<ol>
					<li>Tanda terima reparasi tidak berlaku jika tidak ditandatangani oleh, JR dan berstempel.</li>
					<li>Pengambilan barang reparasi harus membawa tanda terima reparasi yang asli dan berstempel.</li>
				</ol>
			</div>
		</div>
	</div>
</body>
</html>

