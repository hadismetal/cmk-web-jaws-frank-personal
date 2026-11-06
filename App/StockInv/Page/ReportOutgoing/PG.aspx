<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="PG.aspx.cs" inherits="PG" %>

<asp:content contentplaceholderid="h1" runat="server">
	Plain Gold
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Report
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	
    <form runat="server" class="form-inline">
        
        <table class="table-form">
			<tr>
				<td>Periode</td>
				<td>:</td>
				<td>
					<asp:textbox id="tgl" runat="server" cssclass="form-control text-center" width="110" />
					<script>
					    $(document).ready(function () { $('#content_tgl').datepicker({ format: 'dd-mm-yyyy' }); });
					</script>
				</td>
				<td>
					<asp:requiredfieldvalidator id="tgl_RF" runat="server" />
					<span id="tgl_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
                <td>s/d&nbsp;&nbsp;</td>
				<td>
					<asp:textbox id="tgl2" runat="server" cssclass="form-control text-center" width="110" />
					<script>
					    $(document).ready(function () { $('#content_tgl2').datepicker({ format: 'dd-mm-yyyy' }); });
					</script>
				</td>
				<td>
					<asp:requiredfieldvalidator id="tgl2_RF" runat="server" />
					<span id="tgl2_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
                <td>
                    <asp:TextBox ID="kw" runat="server" CssClass="form-control text-center" Width="300" placeholder="No. Dokumen" />
                </td>
			</tr>
		</table>
		<div class="form-group">
			<div class="text-muted">
				Company Brand :
			</div>
			<asp:dropdownlist id="brand" runat="server" cssclass="form-control" width="200">
			</asp:dropdownlist>
		</div>
        <div class="form-group">
			<div class="text-muted">
				Location Type (From) :
			</div>
			<asp:dropdownlist id="locationtypefrom" runat="server" cssclass="form-control" width="200">
                <asp:listitem Value="-1">Semua</asp:listitem>
			</asp:dropdownlist>
		</div>
        <div class="form-group">
			<div class="text-muted">
				Search (From) :
			</div>
			<asp:TextBox runat="server" ID="txt_from" cssclass="form-control" width="200" />
		</div>
        <div class="form-group">
			<div class="text-muted">
				Location Type (To) :
			</div>
			<asp:dropdownlist id="locationtypeto" runat="server" cssclass="form-control" width="200">
                <asp:listitem Value="-1">Semua</asp:listitem>
			</asp:dropdownlist>
		</div>
        
        <div class="form-group">
			<div class="text-muted">
				Search (To) :
			</div>
			<asp:TextBox runat="server" ID="txt_to" cssclass="form-control" width="200" />
		</div>
        <div class="form-group">
			<div class="text-muted">
				Status Outgoing :
			</div>
			<asp:dropdownlist id="status" runat="server" cssclass="form-control" width="200">
                <asp:listitem Value="-1">Semua</asp:listitem>
			</asp:dropdownlist>
		</div>
        <div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
                <button id="Button2" runat="server" class="btn btn-primary" onserverclick="Export">
                    <i class="icon-search"></i>
                    Export
                </button>
                <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Print">
                    <i class="icon-search"></i>
                    Print
                </button>
			</div>
		</div>
		<div class="table-responsive">
			<asp:table id="tb" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
                    <asp:tableheadercell width="90" />
                    <asp:tableheadercell width="90" />
					<asp:tableheadercell style="text-align:center;" width="60">No.</asp:tableheadercell>
                    <asp:tableheadercell style="text-align:center;" width="100">Tanggal</asp:tableheadercell>
                    <asp:tableheadercell style="text-align:center;" width="150">No. Dokumen</asp:tableheadercell>
                    <asp:tableheadercell style="text-align:center;" width="150">Keterangan</asp:tableheadercell>
                    <asp:tableheadercell style="text-align:center;" width="250">From</asp:tableheadercell>
                    <asp:tableheadercell style="text-align:center;" width="250">To</asp:tableheadercell>
					<asp:tableheadercell style="text-align:center;" width="60">Qty</asp:tableheadercell>
                    <asp:tableheadercell style="text-align:center;" width="60">Netto</asp:tableheadercell>
                    <asp:tableheadercell style="text-align:center;" width="60">Gross</asp:tableheadercell>
                    <%--<asp:tableheadercell style="text-align:center;" width="60">Butir</asp:tableheadercell>
			        <asp:tableheadercell style="text-align:center;" width="60">Carat</asp:tableheadercell>--%>
			        <asp:tableheadercell style="text-align:center;" width="150">Harga Jual</asp:tableheadercell>
                    <asp:TableHeaderCell Width="150">Operator</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Approval</asp:TableHeaderCell>
                </asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
</asp:content>

