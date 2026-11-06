<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PreLEI.aspx.cs" Inherits="PreLEI" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    PreLEI Reborn
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Report
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
	<div id="form_area" runat="server">
		<form runat="server" class="form-inline">
			<div class="form-group">
				<div class="text-muted">
					NO. RTS / PLU:
				</div>
				<asp:TextBox ID="txt_rts" runat="server" CssClass="form-control" AutoPostBack="true" OnTextChanged="refreshPage" Width="200" />
			</div>
			<div class="form-group">
				<div class="text-muted">
					&nbsp;
				</div>
				<asp:TextBox ID="dari" runat="server" CssClass="form-control text-center" Width="110" placeholder="Dari" />
				<script>
					$(document).ready(function () { $('#<%=dari.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
				</script>
			</div>
			<div class="form-group">
				<div class="text-muted">
					&nbsp;
				</div>
				<asp:TextBox ID="sampai" runat="server" CssClass="form-control text-center" Width="110" placeholder="Sampai" />
				<script>
					$(document).ready(function () { $('#<%=sampai.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
				</script>
			</div>
			<div class="form-group">
				<div class="text-muted">
					Status :
				</div>
				<asp:DropDownList runat="server" ID="ddl_status" CssClass="form-control" Width="200">
					<asp:ListItem Value="0">All</asp:ListItem>
					<asp:ListItem Value="1">Draft</asp:ListItem>
					<asp:ListItem Value="2">Approved</asp:ListItem>
					<asp:ListItem Value="3">Has Been Reborn</asp:ListItem>
				</asp:DropDownList>
			</div>
			<div class="form-group">
				<div class="hidden-xs">&nbsp;</div>
				<div>
					<asp:Button ID="btn_inputproduct" OnClick="refreshPage" Text="Display" runat="server" class="btn btn-primary">
					</asp:Button>
				</div>
			</div>
			<div class="table-responsive">
				<asp:Table ID="tb" runat="server" CssClass="table-list">
					<asp:TableHeaderRow>
						<asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="150">No. Return </asp:TableHeaderCell>
						<asp:TableHeaderCell Width="150">Tgl. Return </asp:TableHeaderCell>
						<asp:TableHeaderCell Width="100">Asal</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="200">Tujuan</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="100">Tgl. Reborn</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="150">Tipe Product</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="100">PLU</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="120">Level</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="120">Frame Color</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="100">Kadar Logam</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="100">Weight Original</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="100">Weight After QC</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="120">Operator Nama</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="150">Operator Tgl</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="120">Approval</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="150">Approval Tgl</asp:TableHeaderCell>
						<asp:TableHeaderCell Width="100">Status</asp:TableHeaderCell>
					</asp:TableHeaderRow>
				</asp:Table>
			</div>
		</form>
	</div>
</asp:Content>

