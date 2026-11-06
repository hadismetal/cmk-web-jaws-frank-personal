<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Payment_DPAdd.aspx.cs" inherits="Page_Register_Sales_ItemAdd" %>

<asp:content contentplaceholderid="h1" runat="server">
	Sales Order
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Down Payment
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
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
            0% {
                transform: rotate(0deg);
            }

            100% {
                transform: rotate(360deg);
            }
        }
    </style>
    <div id="overlay" class="overlay">
        <div class="spinner"></div>
        <div style="margin-top: 10px;">Loading...</div>
    </div>
	<p>
		<span class="text-muted">Nomor :</span><br />
		<span class="text-lg" id="header_nomor" runat="server" />
	</p>
	<hr />
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Search :
			</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
            <asp:TextBox runat="server" ID="inputoldcustomercode" CssClass="form-control" Width="350" placeholder="Input Kode Customer Lama" />
		</div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
				<a id="cancel" runat="server" class="btn btn-default" href="Payment_ItemList.aspx">
					<i class="icon-remove"></i>
					Cancel
				</a>
			</div>
		</div>
		<div class="table-responsive">
			<asp:table id="tb" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="50" />
					<asp:tableheadercell width="50">No.</asp:tableheadercell>
					<asp:tableheadercell width="170">Nomor DP</asp:tableheadercell>
                    <asp:tableheadercell width="150">Tgl DP</asp:tableheadercell>
                    <asp:tableheadercell width="100">Nominal</asp:tableheadercell>
                    <asp:tableheadercell width="150">Operator</asp:tableheadercell>
                    <asp:tableheadercell width="200">Lokasi</asp:tableheadercell>
					<asp:tableheadercell width="100">Lama DP</asp:tableheadercell>
                    <asp:tableheadercell width="200">Keterangan</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
    <script type="text/javascript">
        function showOverlay() {
            document.getElementById("overlay").style.display = "flex";
        }
    </script>
</asp:content>

