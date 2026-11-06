<%@ page EnableEventValidation="false" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="LDCrossBrand_Detail.aspx.cs" inherits="LD_Detail" %>

<asp:content contentplaceholderid="h1" runat="server">
	Loose Diamond
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Report
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
<form id="Form1" runat="server">
	<div>
        <div style="float:left;">
			<table class="table-file">
                <tr>
			        <td class="text-label">Report Datetime</td>
			        <td>:</td>
			        <td id="report_datetime" runat="server" class="text-bold" />
		        </tr>
		        <tr>
			        <td class="text-label">Company Brand From</td>
			        <td>:</td>
			        <td id="brandFrom" runat="server" class="text-bold" />
		        </tr>
		        <tr>
			        <td class="text-label">From <asp:Literal runat="server" ID="lit_type_from" /></td>
			        <td>:</td>
			        <td id="from" runat="server" class="text-bold" />
		        </tr
				<tr>
			        <td class="text-label">Company Brand To</td>
			        <td>:</td>
			        <td id="brandTo" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">To <asp:Literal runat="server" ID="lit_type_to" /></td>
			        <td>:</td>
			        <td id="to" runat="server" class="text-bold" />
		        </tr>
	        </table>
		</div>
		<div style="float:left; padding:0 0 0 50px;">
			<table class="table-file">
                <tr>
			        <td class="text-label">Status <asp:Literal runat="server" ID="lit_status" /></td>
			        <td>:</td>
			        <td id="status" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">Approve Datetime</td>
			        <td>:</td>
			        <td id="tglkirim" runat="server" class="text-bold" />
		        </tr>
		        <tr>
			        <td class="text-label">Document #</td>
			        <td>:</td>
			        <td id="nomor" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">Courier</td>
			        <td>:</td>
			        <td id="kurir" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">Description</td>
			        <td>:</td>
			        <td id="keterangan" runat="server" class="text-bold" />
		        </tr>
	        </table>
		</div>
        <div style="clear:both"></div>
    </div>
    <hr />
	<div class="row">
        <br class="visible-sm visible-xs" />
		<div class="col-md-5">
            <div class="table-responsive">
			    <asp:table id="tb" runat="server" cssclass="table-report">
				    <asp:tableheaderrow>
					    <asp:tableheadercell style="text-align:center;" width="50">No.</asp:tableheadercell>
                        <asp:tableheadercell style="text-align:center;" width="120">Tanggal</asp:tableheadercell>
                        <asp:tableheadercell style="text-align:center;" width="120">Status</asp:tableheadercell>
                        <asp:tableheadercell style="text-align:center;" width="100">Kode Produk</asp:tableheadercell>
                        <asp:tableheadercell style="text-align:center;" width="100">Kurir</asp:tableheadercell>
					    <asp:tableheadercell style="text-align:center;" width="60">Qty</asp:tableheadercell>
                        <asp:tableheadercell style="text-align:center;" width="60">Butir</asp:tableheadercell>
			            <asp:tableheadercell style="text-align:center;" width="60">Carat</asp:tableheadercell>
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

