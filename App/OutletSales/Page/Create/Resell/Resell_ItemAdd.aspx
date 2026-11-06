<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Resell_ItemAdd.aspx.cs" inherits="Page_Register_Resell" %>

<%@ register src="MenuResell.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content ID="Content1" contentplaceholderid="h1" runat="server">
	Resell
</asp:content>
<asp:content ID="Content2" contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content ID="Content3" contentplaceholderid="content" runat="server">
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
	<uc1:menu id="menu" runat="server" />
	<form id="Form1" runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Search :
			</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
		</div>
		<div class="form-group">
			<div class="text-muted">
				Search By :
			</div>
			<asp:dropdownlist id="searchby" runat="server" cssclass="form-control" width="200">
				<asp:listitem Value="1">Sales Order</asp:listitem>
                <asp:listitem Value="2">PLU</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage" onclick="showOverlaybtn(this);">
					<i class="icon-search"></i>
					Display
				</button>
				<button id="btnoldcustomercode" runat="server" class="btn btn-success" onserverclick="btnoldcustomercode_ServerClick">
                    Insert Old Customer Code
                </button>		 
			</div>
        </div>
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
				<a href="Resell_ItemAddPLU.aspx" class="btn btn-primary" id="btnadd" runat="server">
					<i class="icon-plus"></i>
					Add PLU DJ LAMA
				</a>
                &nbsp;
                <a href="Resell_ItemAddPLUPG.aspx" class="btn btn-primary" id="btnaddpg" runat="server">
					<i class="icon-plus"></i>
					Add PLU PG LAMA
				</a>
                &nbsp;
                <a href="Resell_ItemAddPLULD.aspx" class="btn btn-primary" id="btnaddld" runat="server">
					<i class="icon-plus"></i>
					Add PLU LD LAMA
				</a>
			</div>
		</div>
		<div class="table-responsive">
            <h3>Diamond Jewellery</h3>
			<asp:table id="tb" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
                    <asp:tableheadercell width="150">Image</asp:tableheadercell>
					<asp:tableheadercell width="150">Sales Order</asp:tableheadercell>
					<asp:tableheadercell width="150">PLU</asp:tableheadercell>
                    <asp:tableheadercell width="150">Product Item</asp:tableheadercell>
					<asp:tableheadercell width="150">Harga</asp:tableheadercell>
                    <asp:tableheadercell width="150">Asal</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
            <br />
            <h3>Plain Gold</h3>
			<asp:table id="tb2" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
                    <asp:tableheadercell width="150">Image</asp:tableheadercell>
					<asp:tableheadercell width="150">Sales Order</asp:tableheadercell>
					<asp:tableheadercell width="150">PLU</asp:tableheadercell>
                    <asp:tableheadercell width="150">Product Item</asp:tableheadercell>
					<asp:tableheadercell width="150">Harga</asp:tableheadercell>
                    <asp:tableheadercell width="150">Asal</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
            <br />
            <h3>Gold Jewellery</h3>
			<asp:table id="tb3" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
                    <asp:tableheadercell width="150">Image</asp:tableheadercell>
					<asp:tableheadercell width="150">Sales Order</asp:tableheadercell>
					<asp:tableheadercell width="150">PLU</asp:tableheadercell>
                    <asp:tableheadercell width="150">Product Item</asp:tableheadercell>
					<asp:tableheadercell width="150">Harga</asp:tableheadercell>
                    <asp:tableheadercell width="150">Asal</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
            <br />
            <h3>Loose Diamond</h3>
			<asp:table id="tb4" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
                    <asp:tableheadercell width="150">Image</asp:tableheadercell>
					<asp:tableheadercell width="150">Sales Order</asp:tableheadercell>
					<asp:tableheadercell width="150">PLU</asp:tableheadercell>
                    <asp:tableheadercell width="150">Product Item</asp:tableheadercell>
					<asp:tableheadercell width="150">Harga</asp:tableheadercell>
                    <asp:tableheadercell width="150">Asal</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
	<script type="text/javascript">
		function showOverlay() {
			$('button').prop("disabled", true);
			document.getElementById("overlay").style.display = "flex";
		}

		function showOverlaybtn(btn) {
			btn.disabled = true;
			$('button').prop("disabled", true);
			document.getElementById("overlay").style.display = "flex";
		}
    </script>
</asp:content>