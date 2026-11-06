<%@ page EnableEventValidation="false" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="PG_Detail.aspx.cs" inherits="PG_Detail" %>

<asp:content contentplaceholderid="h1" runat="server">
	Plain Gold
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Report
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
<form id="Form1" runat="server">
	
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
		filter
	</div>
	<div class="row">
        <div class="col-md-5">
			<table class="table-file">
                <tr>
			        <td class="text-label">Report DateTime</td>
			        <td>:</td>
			        <td id="report_datetime" runat="server" class="text-bold" />
		        </tr>
		        <tr>
			        <td class="text-label">Company Brand</td>
			        <td>:</td>
			        <td id="brand" runat="server" class="text-bold" />
		        </tr>
		        <tr>
			        <td class="text-label">From <asp:Literal runat="server" ID="lit_type_from" /></td>
			        <td>:</td>
			        <td id="from" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">To <asp:Literal runat="server" ID="lit_type_to" /></td>
			        <td>:</td>
			        <td id="to" runat="server" class="text-bold" />
		        </tr>
	        </table>
		</div>
		<br class="visible-sm visible-xs" />
		<div class="col-md-5">
			<table class="table-file">
                <tr>
			        <td class="text-label">Status <asp:Literal runat="server" ID="lit_status" /></td>
			        <td>:</td>
			        <td id="status" runat="server" class="text-bold" />
		        </tr>
				<tr>
			        <td class="text-label">Approve Datetime</td>
			        <td>:</td>
			        <td id="tglapprove" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">No. Dokumen</td>
			        <td>:</td>
			        <td id="nomor" runat="server" class="text-bold" />
		        </tr>
	        </table>
		</div>
    </div>
    <hr />
	<div class="row">
        <br class="visible-sm visible-xs" />
		<div class="col-md-5">
            <div class="table-responsive">
			    <asp:table id="tb" runat="server" cssclass="table-report">
				    <asp:tableheaderrow>
					    <asp:tableheadercell style="text-align:center;" width="40">No.</asp:tableheadercell>
                        <%--<asp:tableheadercell style="text-align:center;" width="140">Dokumen In</asp:tableheadercell>
                        <asp:tableheadercell style="text-align:center;" width="200">From</asp:tableheadercell>
                        <asp:tableheadercell style="text-align:center;" width="200">To</asp:tableheadercell>--%>
                        <asp:tableheadercell style="text-align:center;" width="100">Kode Produk</asp:tableheadercell>
                        <asp:tableheadercell style="text-align:center;" width="100">Kurir</asp:tableheadercell>
                        <asp:tableheadercell style="text-align:center;" width="100">Model</asp:tableheadercell>
                        <asp:tableheadercell style="text-align:center;" width="100">Item</asp:tableheadercell>
						<asp:tableheadercell style="text-align:center;" width="100">Gold Level</asp:tableheadercell>
					    <asp:tableheadercell style="text-align:center;" width="100">Consignment</asp:tableheadercell>
					    <asp:tableheadercell style="text-align:center;" width="60">Qty</asp:tableheadercell>
                        <asp:tableheadercell style="text-align:center;" width="80">Qty Received</asp:tableheadercell>
                        <asp:tableheadercell style="text-align:center;" width="80">Qty Broken</asp:tableheadercell>
                        <asp:tableheadercell style="text-align:center;" width="80">Qty Lost</asp:tableheadercell>
                        <asp:tableheadercell style="text-align:center;" width="60">Netto</asp:tableheadercell>
                        <asp:tableheadercell style="text-align:center;" width="60">Gross</asp:tableheadercell>
                        <%--<asp:tableheadercell style="text-align:center;" width="60">Butir</asp:tableheadercell>
			            <asp:tableheadercell style="text-align:center;" width="60">Carat</asp:tableheadercell>--%>
			            <asp:tableheadercell style="text-align:center;" width="150">Harga Jual</asp:tableheadercell>
                    </asp:tableheaderrow>
			    </asp:table>
		    </div>
        </div>
	</div>
	<hr />
    <div class="row">
        <br class="visible-sm visible-xs" />
		<div class="col-md-5">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="back_Click">
				<i class="icon-arrow-left"></i>
				Back
			</button>
            <button id="print" runat="server" class="btn btn-primary" onserverclick="print_Click">
				<i class="icon-print"></i>
				Print
			</button>
		</div>
    </div>
</form>
</asp:content>

