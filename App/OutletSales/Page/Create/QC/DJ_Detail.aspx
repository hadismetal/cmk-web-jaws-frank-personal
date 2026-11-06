<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DJ_Detail.aspx.cs" Inherits="Page_Create_QC_DJ_Detail" %>

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
							<span style="font-size:30px;">&nbsp;QC DJ&nbsp;</span><br />
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
								<td class="text-muted">
									Image
								</td>
								<td>
									<asp:Image ID="imgpicture" runat="server" CssClass="img-thumbnail" Height="100" />
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									PLU
								</td>
								<td>
									<label runat="server" id="plu"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Petugas QC</td>
								<td>
									<label runat="server" id="petugasqc"></label>
								</td>
							</tr>
							<!--<tr>
								<td class="text-muted">Blemish untuk > 0,3</td>
								<td>
									<label runat="server" id="blemish"></label>
								</td>
							</tr>-->
							<!--<tr>
								<td class="text-muted">Kecocokan Jumlah Butir</td>
								<td>
									<label runat="server" id="jlhbutir"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Kecocokan Sertifikat</td>
								<td>
									<label runat="server" id="sertifikat"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Grafir Total Carat - Non CZ</td>
								<td>
									<label runat="server" id="noncz"></label>
								</td>
							</tr>-->
							<tr>
								<td class="text-muted">Gross Weight Original</td>
								<td>
									<label runat="server" id="grossweightoriginal"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Gross Weight Actual</td>
								<td>
									<label runat="server" id="grossweight"></label>
								</td>
							</tr>
							<!--<tr>
								<td class="text-muted">Grafir Total Carat - CZ</td>
								<td>
									<label runat="server" id="cz"></label>
								</td>
							</tr>-->
							<tr>
								<td class="text-muted">Grafir Logo</td>
								<td>
									<label runat="server" id="logo"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Grafir Kadar Emas</td>
								<td>
									<label runat="server" id="kadaremas"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Frame Color</td>
								<td>
									<label runat="server" id="framecolor"></label>
								</td>
							</tr>
							<!--<tr>
								<td class="text-muted">Grafir Merk</td>
								<td>
									<label runat="server" id="merk"></label>
								</td>
							</tr>-->
							<tr>
								<td class="text-muted">Fisik Barang Keseluruhan</td>
								<td>
									<label runat="server" id="fisik"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Keterangan</td>
								<td>
									<label runat="server" id="keterangan"></label>
								</td>
							</tr>
						</table>
					</div>
				</div>
			</div>
			<br />
			<br />
			<br />
			<div class="col-md-8">
				<div class="panel panel-default">
					<div class="panel-heading">
						QC Stone
					</div>
					<div class="panel-body">
						<asp:Table runat="server" id="tb" CssClass="table table-responsive" style="font-size:small">
							<asp:TableHeaderRow>
								<asp:TableHeaderCell >No.</asp:TableHeaderCell>
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
			<br/>
			<div class="row">
				<div class="col-md-8">
					<br />
					<div class="col-md-12">
						<h3 runat="server" style="font-family:'Lucida Console';color:orangered">Need Approval SM / AM</h3>
						<table class="pull-right">
							<tr>
								<td style="text-align:center">
									<b>Check By</b>
									<br/>
									<br/>
									<br/>
									<b runat="server" id="checkedby"></b>
								</td>
							</tr>
						</table>
					</div>
				</div>
			</div>
		</form>
	</div>
	<script>
		window.onload = function(){
                alert('QCDJ need approval, please contact SM/AM.'); 
			};
    </script>
</body>
</html>
