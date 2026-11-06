<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJDO_DataValidasi.aspx.cs" Inherits="Page_Register_DJDO_DJDO_DataValidasi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" Runat="Server">
    Diamond Jewelry
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" Runat="Server">
    Register
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" Runat="Server">
    	<asp:literal id="idbrand" runat="server" visible="false" />
	<hr />
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Search (No. Validasi / Kode Barang) :
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
        <div>
            <asp:Literal runat="server" ID="lit_data" />
        </div>
		<div class="table-responsive">
			<asp:table id="tb" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="50" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
					<asp:tableheadercell width="150">No. Validasi</asp:tableheadercell>
					<asp:tableheadercell width="150">Kode Barang</asp:tableheadercell>
                    <asp:tableheadercell width="150">Kode Bagian</asp:tableheadercell>
					<asp:tableheadercell width="150">No. DO</asp:tableheadercell>
				</asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
</asp:Content>

