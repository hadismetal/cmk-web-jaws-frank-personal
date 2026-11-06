<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LD_Detail.aspx.cs" Inherits="Page_Create_QC_LD_Detail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
	<title>Point of Sales - Frank & co.</title>
	<meta name="viewport" content="width=device-width, initial-scale=1.0" />

	<!-- CSS -->
	<link rel="stylesheet" href="/UI/CSS/bootstrap.css" />
	<link rel="stylesheet" href="/UI/CSS/bootstrap.datepicker.css" />
	<link rel="stylesheet" href="/UI/CSS/bootstrap.font.css" />
	<link rel="stylesheet" href="/UI/CSS/custom.css" />

	<!-- JavaScript -->
	<script type="text/javascript" src="/UI/JS/jquery.js"></script>
	<script type="text/javascript" src="/UI/JS/bootstrap.js"></script>
	<script type="text/javascript" src="/UI/JS/datepicker.js"></script>
	<script type="text/javascript" src="/UI/JS/custom.js"></script>
</head>
<body>
	<!-- Content -->
	<div>
		<div class="row">
			<div class="col-xs-12">
				<table border="0" cellpadding="0" cellspacing="0" style="width:100%;">
					<tr>
						<td style="width:20%; text-align:left;">
							<img src="/Images/logo_web.jpg" border="0" runat="server" id="img_logo_web" style="height:50px;" />
						</td>
						<td style="width:60%; text-align:center;">
							<span style="font-size:30px;">&nbsp;Resell & QC LD&nbsp;</span><br />
							<span style="font-size:20px;"><asp:Literal runat="server" ID="lit_nomor_repair" /></span>
						</td>
						<td style="width:20%; text-align:right;">
							<asp:Literal id="cabang" runat="server"></asp:Literal>
						</td>
					</tr>
				</table>
				<hr />
			</div>
		</div>
		<form id="Form1" runat="server">
			<div class="col-md-8">
				<div class="form-group">
					<table>
						<tr>
							<td>
								<p class="text-bold" style="font-size: large">Nomor<p>
							</td>
							<td style="padding-left: 10px; padding-right: 10px">
								<p class="text-bold" style="font-size: large">:</p>
							</td>
							<td>
								<p class="text-bold" style="font-size: large" runat="server" id="nodocqc"></p>
							</td>
						</tr>
						<tr>
							<td>
								<p class="text-bold" style="font-size: large">Tanggal QC<p>
							</td>
							<td style="padding-left: 10px; padding-right: 10px">
								<p class="text-bold" style="font-size: large">:</p>
							</td>
							<td>
								<p class="text-bold" style="font-size: large" runat="server" id="tgldocqc"></p>
							</td>
						</tr>
					</table>
				</div>
			</div>
			<div class="col-md-8">
				<div class="panel panel-default">
					<div class="panel-heading">
						QC Product
					</div>
					<div class="panel-body" style="margin-top:10px">
						<table class="table table-bordered table-responsive">
							<tr>
								<td style="width:30%" class="text-muted">
									PLU
								</td>
								<td style="width:40%">
									<label runat="server" id="plu"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									No. Invoice
								</td>
								<td>
									<label runat="server" id="invoice"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									Nama Customer
								</td>
								<td>
									<label runat="server" id="cust"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									Store Asal
								</td>
								<td>
									<label runat="server" id="storeasal"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									GIA
								</td>
								<td>
									<label runat="server" id="gia"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Petugas QC</td>
								<td>
									<label runat="server" id="petugasqc"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Sertifikat No</td>
								<td>
									<label runat="server" id="sertifikat"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Item</td>
								<td>
									<label runat="server" id="item"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Weight (carat)</td>
								<td>
									<label runat="server" id="weight"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Harga Beli</td>
								<td>
									<label runat="server" id="hg_beli"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Shape</td>
								<td>
									<label runat="server" id="shape"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Clarity</td>
								<td>
									<label runat="server" id="cla"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Color</td>
								<td>
									<label runat="server" id="col"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Cutting</td>
								<td>
									<label runat="server" id="cut"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Brand</td>
								<td>
									<label runat="server" id="brand"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Keterangan</td>
								<td>
									<label runat="server" id="keterangan"></label>
								</td>
							</tr>
						</table>
						<br />
						<table class="table table-bordered table-responsive">
							<tr style="border-bottom:groove">
								<td colspan="2"><b>Hasil Physical Checking Oleh SM/BM dan AM</b></td>
							</tr>
							<tr>
								<td style="width:30%">Sertifikat Asli</td>
								<td style="width:40%"><label runat="server" id="isanycertificate"></label></td>
							</tr>
							<tr>
								<td>Invoice Asli</td>
								<td><label runat="server" id="isanyinvoice"></label></td>
							</tr>
							<tr>
								<td>Laser Incription LD</td>
								<td><label runat="server" id="laserincription"></label></td>
							</tr>
							<tr>
								<td>Measurement</td>
								<td><label runat="server" id="measurement"></label></td>
							</tr>
							<tr>
								<td>Kondisi Fisik Batu</td>
								<td><label runat="server" id="kondisibatu"></label></td>
							</tr>
						</table
						<br />
						<asp:Table runat="server" ID="tbextension" CssClass="table table-bordered">
							<asp:TableHeaderRow>
								<asp:TableHeaderCell>No.</asp:TableHeaderCell>
								<asp:TableHeaderCell>Clarity Characteristic</asp:TableHeaderCell>
								<asp:TableHeaderCell>Size</asp:TableHeaderCell>
								<asp:TableHeaderCell>Number</asp:TableHeaderCell>
								<asp:TableHeaderCell>Position</asp:TableHeaderCell>
							</asp:TableHeaderRow>
						</asp:Table>
					</div>
				</div>
			</div>
			<br />
				<div class="col-md-12">
					<div class="row">
						<div class="col-md-12" style="display:none">
							<p>
								<span class="text-muted">Final Price (IDR) :</span><br />
								<span class="text-lg text-orange" id="val" runat="server" />
								<small class="text-silver">IDR</small><br />
							</p>
						</div>
						<br />
						<br />
						<div class="col-md-12">
							<table class="table table-bordered">
								<tr>
									<td style="text-align:center">
										<b>Checked</b>
									</td>
									<td style="text-align:center">
										<b style="text-align:center">SM / AM</b>
									</td>
									<td style="text-align:center">
										<b style="text-align:center">Procurement</b>
									</td>
									<td style="text-align:center">
										<b style="text-align:center">Head Of Procurement</b>
									</td>
									<td style="text-align:center">
										<b style="text-align:center">GM Merchandise</b>
									</td>
								</tr>
								<tr rowspan="2">
									<td style="text-align:center">
										<p runat="server" id="checkedby"></p>
									</td>
									<td style="text-align:center">
										<p runat="server" id="approveby"></p>
									</td>
									<td style="text-align:center">
										<p runat="server" id="approveprocby"></p>
									</td>
									<td style="text-align:center">
										<p runat="server" id="approveprocfinalby"></p>
									</td>
									<td style="text-align:center">
										<p runat="server" id="approvefinalby"></p>
									</td>
								</tr>
							</table>
						</div>
					</div>
				</div>
		</form>
		<script>
			window.onload = function() {
				//alert("QCLD need approval. Please contact SM / AM, Procurement and GM Merchandise for approval.");
                alert("QCLD need approval, please contact SM/AM, Procurement, Head of Procurement and Merchandise Director.");
			}
        </script>
	</div>
</body>
</html>