<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PaymentDP_Form_Add.aspx.cs" Inherits="Page_Create_Payment_PaymentDP_Form_Add" %>

<%@ register src="MenuPaymentDP.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" Runat="Server">
    Down Payment
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" Runat="Server">
    Register
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" Runat="Server">
    <form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Search :
			</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
		</div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
				<a id="cancel" runat="server" class="btn btn-default" onserverclick="cancel_ServerClick">
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
					<asp:tableheadercell width="200">Nomor DP</asp:tableheadercell>
                    <asp:tableheadercell width="200">Tgl DP</asp:tableheadercell>
                    <asp:tableheadercell width="130">Total Pembayaran</asp:tableheadercell>
                    <asp:tableheadercell width="100">Type</asp:tableheadercell>
                    <asp:tableheadercell width="130">Estimasi Harga</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
</asp:Content>

