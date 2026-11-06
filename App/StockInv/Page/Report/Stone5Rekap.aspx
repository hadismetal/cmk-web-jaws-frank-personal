<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Stone5Rekap.aspx.cs" inherits="Page_Account_Stone" %>

<asp:content contentplaceholderid="h1" runat="server">
	Pearl
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
				<asp:listitem>Code :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel02" runat="server" cssclass="form-control" width="175">
				<asp:listitem>Source :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel03" runat="server" cssclass="form-control" width="175">
				<asp:listitem>Colour :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel04" runat="server" cssclass="form-control" width="175">
				<asp:listitem>Shape :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="clearfix">
			<br class="hidden-xs" />
		</div>
        <div class="clearfix"></div>
		<div class="form-separator hidden-xs"></div>
		<div class="form-group">
			<asp:dropdownlist id="parcel05" runat="server" cssclass="form-control" width="175">
				<asp:listitem>Overtone :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel06" runat="server" cssclass="form-control" width="175">
				<asp:listitem>Diameter :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel07" runat="server" cssclass="form-control" width="175">
				<asp:listitem>Surface :</asp:listitem>
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
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
					<i class="icon-search"></i>
					Display
				</button>
				<button id="Button1" runat="server" class="btn btn-primary" onserverclick="Export">
					<i class="icon-download-alt"></i>
					Export
				</button>
			</div>
		</div>
	</form>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				
				<asp:tableheadercell width="120">Nama</asp:tableheadercell>
                <asp:tableheadercell width="120">Code</asp:tableheadercell>
                <asp:tableheadercell width="120">Shape</asp:tableheadercell>
                <asp:tableheadercell width="120">Overtone</asp:tableheadercell>
                <asp:tableheadercell width="120">Diameter</asp:tableheadercell>                
				<asp:tableheadercell width="120">Total Butir</asp:tableheadercell>
				<asp:tableheadercell width="120">Total Carat</asp:tableheadercell>
				
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

