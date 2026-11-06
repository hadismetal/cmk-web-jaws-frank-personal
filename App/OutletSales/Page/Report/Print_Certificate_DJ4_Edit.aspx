<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Print_Certificate_DJ4.aspx.cs" Inherits="Page_Summary" %>

<!DOCTYPE html>

<html lang="en">
<head id="demo">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Print Certificate</title>
		<!-- CSS -->
	<link rel="stylesheet" href="/UI/CSS/bootstrap.css" />
	<link rel="stylesheet" href="/UI/CSS/bootstrap.font.css" />
	<link rel="stylesheet" href="/UI/CSS/custom.css" />
	<link rel="stylesheet" href="/UI/CSS/custom.portal.css" />
    
	<!-- JavaScript -->
	<script type="text/javascript" src="/UI/JS/jquery.js"></script>
	<script type="text/javascript" src="/UI/JS/bootstrap.js"></script>
	<script type="text/javascript" src="/UI/JS/custom.js"></script>
	
	<style>
		.avenir-book {
			font-family:Avenir, "Century Gothic", sans-serif; 
			font-weight:100; 
			font-style:normal;
		}
		
		.baskerville-semibold {
			font-family:Bodoni MT; 
			font-weight:600; 
			font-style:normal;
		}
		
		.baskerville-semibold-italic {
			font-family:Bodoni MT; 
			font-weight:600; 
			font-style:italic;
		}
		
		.avenir-black {
			font-family:Avenir, "Century Gothic", sans-serif; 
			font-weight:600; 
			font-style:normal;
		}
	</style>
</head>

<body>
	<div class="container" style="width:300px;height:auto;float:left; margin-left:25px; margin-top:60px;color: #7c0545;" >
	<div class="row">
	<table>
	<tr>
	<td>
			<div class="col-md-12">
				<asp:Literal ID="img" runat="server"/>
			</div>
			</td>
			<td style="padding-left:30px">
			<div class="col-md-12">
				<div class="row" style="font-size:16px;font-family:Bodoni MT; font-weight:700;font-style:normal;">
					<asp:Literal ID="plu" runat="server" />
				</div>
				<div class="row" style="max-width:100px;font-size:12px;font-family:Avenir, sans-serif; font-weight:700; font-style:normal;margin-top:-3px;">
					<asp:Literal ID="kadar" runat="server" />% -
					<asp:Literal ID="weight" runat="server" />gr
				</div>
				<div class="row" style="font-size:8px;font-family:Avenir, sans-serif; font-weight:700; font-style:normal;margin-top:3px;">
					<asp:Table runat="server" ID="tbl_stonelist" style="border:none;" />
				</div>
				<div class="row" style="font-size:18px;font-family:Bodoni MT; font-weight:700; font-style:italic;margin-top:3px;">
					<asp:Literal ID="item" runat="server" />
				</div>
				<div class="row" style="font-size:8px;font-family:Avenir, sans-serif; font-weight:700; font-style:normal;margin-top:-3px;">
					<asp:Literal ID="kodekaret" runat="server" />
				</div>
			</div>
			</td>	
	</tr>
	</table>
	</div>
	</div>
    <label id="lbwarning" runat="server"></label>
</body>
    <script type="text/javascript">
		$(document).ready(function(){
			var myobj = document.getElementById("demo");
			myobj.remove();
			window.print();
		});
    </script>
</html>
