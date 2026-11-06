<%@ page EnableEventValidation="false" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Product.aspx.cs" inherits="DJ" %>

<asp:content contentplaceholderid="h1" runat="server">
	Packaging
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	List
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	
    <form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Market Brand :
			</div>
			<asp:dropdownlist id="brand" runat="server" cssclass="form-control" width="200">
			</asp:dropdownlist>
		</div>
        <div class="form-group">
            <div class="text-muted">
               Location Type (From)
            </div>
            <asp:DropDownList ID="tipelokasifrom" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasi" AutoPostBack="true">
                <asp:ListItem Value="-1">Tipe Lokasi</asp:ListItem>
                <asp:ListItem Value="0">Warehouse</asp:ListItem>
                <asp:ListItem Value="1">Outlet</asp:ListItem>
                <asp:ListItem Value="2">Exhibition</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Search (From)
            </div>
            <asp:DropDownList ID="lokasifrom" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Lokasi</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
               Location Type (To)
            </div>
            <asp:DropDownList ID="tipelokasito" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasiTo" AutoPostBack="true">
                <asp:ListItem Value="-1">Tipe Lokasi</asp:ListItem>
                <asp:ListItem Value="0">Warehouse</asp:ListItem>
                <asp:ListItem Value="1">Outlet</asp:ListItem>
                <asp:ListItem Value="2">Exhibition</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Search (To)
            </div>
            <asp:DropDownList ID="lokasito" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Lokasi</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
			<div class="text-muted">
				Status :
			</div>
			<asp:dropdownlist id="status" runat="server" cssclass="form-control" width="200">
                <asp:listitem>Semua</asp:listitem>
			</asp:dropdownlist>
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
                    <asp:tableheadercell width="90" />
					<asp:tableheadercell width="40">No.</asp:tableheadercell>
					<asp:tableheadercell width="250">From</asp:tableheadercell>
                    <asp:tableheadercell width="40"></asp:tableheadercell>
                    <asp:tableheadercell width="250">To</asp:tableheadercell>
                    <asp:tableheadercell width="100">Total Item</asp:tableheadercell>
			        <asp:tableheadercell width="100">Status</asp:tableheadercell>
			        <asp:tableheadercell width="160">User & Tanggal</asp:tableheadercell>
                </asp:tableheaderrow>
			</asp:table>
		</div>
	</form>
</asp:content>

