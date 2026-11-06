<%@ Page Language="C#" AutoEventWireup="true" CodeFile="QCLD_Detail.aspx.cs" Inherits="Page_Report_QCLD_Detail" %>

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
		<div class="row">
			<div class="col-xs-12">
				<table border="0" cellpadding="0" cellspacing="0" style="width:100%;">
					<tr>
						<td style="width:20%; text-align:left;">
							<img src="/Images/logo_web.jpg" border="0" runat="server" id="img_logo_web" style="height:50px;" />
						</td>
						<td style="width:60%; text-align:center;">
							<span style="font-size:30px;">&nbsp;Form Resell & QC LD&nbsp;</span><br />
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
								<p class="text-bold" style="font-size: large">
									Tanggal QC<p>
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
								<td class="text-muted" style="width:30%">
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
								<td class="text-muted">Harga</td>
								<td>
									<label runat="server" id="valbeli"></label>
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
						</table>
						<br />
						<asp:Table runat="server" ID="tbextension" CssClass="table table-bordered">
						</asp:Table>
					</div>
				</div>
			</div>
			<div class="col-md-8" runat="server" ID="divqcproc" visible="false">
				<div class="panel panel-default">
					<div class="panel-heading">
						Pricing Procurement
					</div>
					<div class="panel-body">
						<table class="table table-bordered table-responsive">
							<asp:Panel runat="server" ID="panelapproved">
								<tr>
									<td class="text-muted" style="width:30%">Total Harga</td>
									<td style="width:40%">
										<asp:Label runat="server" ID="hargajualcust_1"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="text-muted">Keterangan</td>
									<td>
										<asp:Label ID="keteranganprocurement_1" runat="server"></asp:Label>
									</td>
								</tr>
								<tr>
									<td class="text-muted">Pricing Procurement By</td>
									<td>
										<asp:Label runat="server" ID="operatorprocurement"></asp:Label>
									</td>
								</tr>
							</asp:Panel>
							<asp:Panel runat="server" ID="panelnotapproved">
								<tr>
									<td class="text-muted" style="width:30%">Total Harga</td>
									<td style="width:40%">
										<div class="form-group">
											<div class="input-group">
												<div class="input-group-addon" style="width: 25px;">
													$
												</div>
												<asp:TextBox runat="server" ID="hargajualcust" CssClass="form-control" Text="0"></asp:TextBox>
											</div>
										</div>
									</td>
								</tr>
								<tr>
									<td class="text-muted">Keterangan</td>
									<td>
										<asp:TextBox ID="keteranganprocurement" runat="server" TextMode ="MultiLine" CssClass="form-control" style="height:79px"/>
									</td>
								</tr>
							</asp:Panel>
						</table>
					</div>
				</div>
			</div>
			<br/>
			<div class="row">
				<div class="col-md-8">
					<div class="col-md-12" runat="server" ID="divfinalprice">
						<p>
							<span class="text-muted">Final Price (USD) :</span><br />
							<span class="text-lg text-orange" id="val" runat="server" />
							<small class="text-silver">USD</small><br />
						</p>
					</div>
					<div runat="server" ID="divapprove" class="col-md-4">
						<button runat="server" ID="btnApprove" onserverclick="btnApprove_Click" class="btn btn-primary">Approve</button>
						<button runat="server" ID="btnApproveFinal" onserverclick="btnApproveFinal_Click" class="btn btn-primary">Approve Final</button>
						<div runat="server" ID="divprint2">
							<button type="button" onclick="onperint();" id="btn-print" class="btn btn-success">Print</button>
						</div>
					</div>
					<b runat="server" ID="lblwarning" class="col-md-6 text-danger" style="font-size:x-large;font-family:'Lucida Console'"></b>
					<div runat="server" ID="divprint" class="col-md-4">
						<div class="form-inline">
							<button type="button" onclick="onperint();" id="btn-print" class="btn btn-success">Print</button>
							<button runat="server" onserverclick="btnEdit_Click" id="btnEdit" class="btn btn-warning">Edit</button>
						</div>
					</div>
					<br />
					<br />
					<div class="col-md-12">
						<table class="table table-bordered" id="div_ttd" style="display:none">
							<tr>
								<td style="text-align:center">
									<b>Created</b>
								</td>
								<td style="text-align:center">
									<b>Approved SM</b>
								</td>
								<td style="text-align:center">
									<b>Approved AM</b>
								</td>
								<td style="text-align:center">
									<b>Staff Procurement</b>
								</td>
								<td style="text-align:center">
									<b>Head Of Procurement</b>
								</td>
								<td style="text-align:center">
									<b>Merchandise Director</b>
								</td>
							</tr>
							<tr>
								<td style="text-align:center">
									<b runat="server" id="createdby"></b>
								</td>
								<td style="text-align:center">
									<b runat="server" id="approvesmby"></b>
								</td>
								<td style="text-align:center">
									<b runat="server" id="approveamby"></b>
								</td>
								<td style="text-align:center">
									<b runat="server" id="approveprocurementby"></b>
								</td>
								<td style="text-align:center">
									<b runat="server" id="approveprocurementfinalby"></b>
								</td>
								<td style="text-align:center">
									<b runat="server" id="approvemerchfinalby"></b>
								</td>
							</tr>
						</table>
					</div>
				</div>
			</div>
			<script>
				function onperint(){
					document.getElementById("div_ttd").style = "display:block";
					document.getElementById("btn-print").style = "display:none";
					//document.getElementById("btnEdit").style = "display:none";
					window.print();
					document.getElementById("btn-print").style = "display:block";
					//document.getElementById("btnEdit").style = "display:block";
					document.getElementById("div_ttd").style = "display:none";
				}
			</script>
		</form>
</body>
</html>
