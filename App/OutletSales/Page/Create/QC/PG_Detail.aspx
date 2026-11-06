<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PG_Detail.aspx.cs" Inherits="Page_Create_QC_PG_Detail" %>

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
							<span style="font-size:30px;">&nbsp;QC PG&nbsp;</span><br />
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
								<p class="text-bold" style="font-size:large">Nomor<p>
							</td>
							<td style="padding-left:10px;padding-right:10px"><p class="text-bold" style="font-size:large">:</p></td>
							<td>
								<p class="text-bold" style="font-size:large" runat="server" id="nodocqc"></p>
							</td>
						</tr>
						<tr>
							<td>
								<p class="text-bold" style="font-size:large">Tanggal QC<p>
							</td>
							<td style="padding-left:10px;padding-right:10px"><p class="text-bold" style="font-size:large">:</p></td>
							<td>
								<p class="text-bold" style="font-size:large" runat="server" id="tgldocqc"></p>
							</td>
						</tr>
					</table>
				</div>
			</div>
			<div class="col-md-8">
				<div class="panel panel-default width-500">
					<div class="panel-heading">
						QC
					</div>
					<div class="panel-body">
						<table class="table table-bordered table-responsive">
							<tr>
								<td class="text-muted">
									PLU
								</td>
								<td>
									<label runat="server" id="plu"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									Netto Weight
								</td>
								<td>
									<label runat="server" id="netto"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									Kadar Logam
								</td>
								<td>
									<label runat="server" id="kadar"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									Frame Color
								</td>
								<td>
									<label runat="server" id="frame"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									Petugas QC
								</td>
								<td>
									<label runat="server" id="petugasqc"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									Grafir Logo
								</td>
								<td>
									<label runat="server" id="logo"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									Grafir Kadar Emas
								</td>
								<td>
									<label runat="server" id="kadaremas"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									Fisik Barang Keseluruhan
								</td>
								<td>
									<label runat="server" id="fisik"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									Net Weight Actual
								</td>
								<td>
									<label runat="server" id="netweight"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									Kadar by Karatimeter
								</td>
								<td>
									<label runat="server" id="kadarkaratimeter"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									Frame Color Actual
								</td>
								<td>
									<label runat="server" id="framecoloractual"></label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">
									Keterangan
								</td>
								<td>
									<label runat="server" id="keterangan"></label>
								</td>
							</tr>
						</table>
					</div>
				</div>
			</div>
			<br/>
			<div class="row">
				<div class="col-md-8">
					<table class="pull-right">
						<tr>
							<td style="text-align:center">
								<b>Check By</b>
								<br/>
								<br/>
								<br/>
								<b runat="server" id="checkedby"></b>
							</td>
							<td style="padding-left: 50px;text-align:center">
								<b style="text-align:center">Approved By</b>
								<br/>
								<br/>
								<br/>
								<b>______________________</b>
							</td>
						</tr>
					</table>
				</div>
			</div>
		</form>
		<script>
			window.onload = function() {
				window.print(); 
			}
		</script>
	</div>
</body>
</html>