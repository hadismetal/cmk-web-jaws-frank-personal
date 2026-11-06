<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="LD.aspx.cs" inherits="LD" %>

<asp:content contentplaceholderid="h1" runat="server">
	Loose Diamond
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Approval
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<script>
	    $(document).ready(function () {
	        $('#<%=datepicker.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
            $('#<%=datepicker2.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
        });
    </script>
    <form id="Form1" runat="server" class="form-inline">
        <div class="form-group">
            <div class="text-muted">
                Periode :
            </div>

            <asp:TextBox CssClass="form-control" type="text" placeholder="Dari" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" />

        </div>
        <div class="form-group">
            <div class="text-muted">
                &nbsp;
            </div>
            <asp:TextBox CssClass="form-control" type="text" placeholder="Sampai" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" />
        </div>
		<div class="form-group">
			<div class="text-muted">
				Market Brand :
			</div>
			<asp:dropdownlist id="brand" runat="server" cssclass="form-control" width="200">
                <asp:ListItem Value="0">All</asp:ListItem>
			</asp:dropdownlist>
		</div>
        <div class="form-group">
            <div class="text-muted">
                Tipe Lokasi
            </div>
            <asp:DropDownList ID="tipelokasi" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasi" AutoPostBack="true">
                <asp:ListItem Value="-1">Tipe</asp:ListItem>
                <asp:ListItem Value="0">Warehouse</asp:ListItem>
                <asp:ListItem Value="1">Outlet</asp:ListItem>
                <asp:ListItem Value="2">Exhibition</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Lokasi
            </div>
            <asp:DropDownList ID="lokasi" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Lokasi</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
			</div>
		</div>
		<div class="table-responsive">
			<asp:table id="tb" runat="server" cssclass="table-list">
				<asp:tableheaderrow>
					<asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
                    <asp:tableheadercell width="150">No. Dokumen</asp:tableheadercell>
					<asp:tableheadercell width="200">From</asp:tableheadercell>
                    <asp:tableheadercell width="40"></asp:tableheadercell>
                    <asp:tableheadercell width="200">To</asp:tableheadercell>
                    <asp:tableheadercell width="200">Keterangan</asp:tableheadercell>
                    <asp:tableheadercell width="80">Total Item</asp:tableheadercell>
			        <asp:tableheadercell width="100">Status</asp:tableheadercell>
			        <asp:tableheadercell width="150">User & Tanggal</asp:tableheadercell>
                </asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
</asp:content>

