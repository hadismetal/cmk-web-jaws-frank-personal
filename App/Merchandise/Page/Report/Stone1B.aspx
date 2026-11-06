<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Stone1B.aspx.cs" inherits="Page_Account_Stone" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Rapaport
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Report
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server" class="form-inline">
		<div class="form-group">
			<div class="text-muted">
				Search :
			</div>
			<asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
		</div>
		<div class="clearfix">
			<br class="hidden-xs" />
		</div>
		<div class="text-muted">
			Parcel :
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel01" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Shape :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel02" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Size :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel03" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Colour :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel04" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Clarity :</asp:listitem>
			</asp:dropdownlist>
		</div>
        <div class="clearfix">
			<br class="hidden-xs" />
		</div>
        <div class="clearfix"></div>
		<div class="form-separator hidden-xs"></div>
		<div class="form-group">
			<asp:dropdownlist id="parcel05" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Cutting :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel06" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Brand :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel07" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Category :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="clearfix">
			<br class="hidden-xs" />
		</div>
		<div class="form-group">
			<div class="text-muted">
				Diurutkan :
			</div>
			<asp:dropdownlist id="order" runat="server" cssclass="form-control" width="150">
				<asp:listitem value="Nama">Nama</asp:listitem>
				<asp:listitem value="Nomor">PLU</asp:listitem>
				<asp:listitem value="KodeParcel">Kode Parcel</asp:listitem>
				<asp:listitem value="ID">ID</asp:listitem>
			</asp:dropdownlist>
			<div class="text-00 visible-xs">&nbsp;</div>
			<asp:dropdownlist id="orderby" runat="server" cssclass="form-control" width="100">
				<asp:listitem>ASC</asp:listitem>
				<asp:listitem>DESC</asp:listitem>
			</asp:dropdownlist>
		</div>
        <div class="form-group">
            <div class="text-muted">
                &nbsp;
            </div>
            <asp:DropDownList ID="supplier" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="0">Supplier</asp:ListItem>
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <div class="text-muted">
                Tipe Lokasi
            </div>
            <asp:DropDownList ID="tipelokasi" runat="server" CssClass="form-control" Width="175" OnSelectedIndexChanged="FillLokasi" AutoPostBack="true">
                <asp:ListItem Value="-1">Tipe Lokasi</asp:ListItem>
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
                <asp:ListItem Value="-1">Lokasi</asp:ListItem>
            </asp:DropDownList>
        </div>
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
				<button id="Button1" runat="server" class="btn btn-primary" onserverclick="Export">
					<i class="icon-search"></i>
					Export
				</button>
			</div>
		</div>
	</form>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			
		</asp:table>
	</div>
    <asp:Literal ID="paging_text" runat="server" />
</asp:content>

