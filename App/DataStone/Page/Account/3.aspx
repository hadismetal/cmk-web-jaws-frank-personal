<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="3.aspx.cs" inherits="Page_Account_3" %>

<asp:content contentplaceholderid="h1" runat="server">
	Precious Stone
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Account
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
				<asp:listitem Value="0">Item :</asp:listitem>
			</asp:dropdownlist>
		</div>
        <div class="form-group">
			<asp:dropdownlist id="parcel02" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Shape :</asp:listitem>
			</asp:dropdownlist>
		</div>
		
        <div class="form-group">
			<asp:dropdownlist id="parcel03" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Size :</asp:listitem>
			</asp:dropdownlist>
		</div>

        <div class="form-group">
			<asp:dropdownlist id="parcel04" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Colour :</asp:listitem>
			</asp:dropdownlist>
		</div>
        <div class="form-group">
			<asp:dropdownlist id="parcel06" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Clarity :</asp:listitem>
			</asp:dropdownlist>
		</div>
		
<%--        <div class="form-group">
			<asp:dropdownlist id="parcel05" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Colour Grading :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel09" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Colour Comment :</asp:listitem>
			</asp:dropdownlist>
		</div>
		
		<div class="clearfix"></div>
		<div class="form-separator hidden-xs"></div>
        <div class="form-group">
			<asp:dropdownlist id="parcel10" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Brilliance :</asp:listitem>
			</asp:dropdownlist>
		</div>
        
		
		<div class="form-group">
			<asp:dropdownlist id="parcel07" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Treatment :</asp:listitem>
			</asp:dropdownlist>
		</div>
		

        <div class="clearfix"></div>
		<div class="form-separator hidden-xs"></div>
            <div class="form-group">
			<asp:dropdownlist id="parcel11" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Proportions :</asp:listitem>
			</asp:dropdownlist>
		</div>

        <div class="form-group">
			<asp:dropdownlist id="parcel08" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Country of Origin :</asp:listitem>
			</asp:dropdownlist>
		</div>
		
		
		<div class="form-group">
			<asp:dropdownlist id="parcel12" runat="server" cssclass="form-control" width="175">
				<asp:listitem Value="0">Supplier :</asp:listitem>
			</asp:dropdownlist>
		</div>--%>

		<div class="clearfix">
			<br class="hidden-xs" />
		</div>
		<div class="form-group">
			<div class="text-muted">
				Status :
			</div>
			<asp:dropdownlist id="disable" runat="server" cssclass="form-control" width="120">
				<asp:listitem>Semua</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<div class="text-muted">
				Diurutkan :
			</div>
			<asp:dropdownlist id="order" runat="server" cssclass="form-control" width="150">
				<asp:listitem value="Nama">Nama</asp:listitem>
				<asp:listitem value="Tgl">Tanggal</asp:listitem>
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
			</div>
		</div>
	</form>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="180" />
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="250">Nama</asp:tableheadercell>
				<asp:tableheadercell width="90">Tgl.</asp:tableheadercell>
				<asp:tableheadercell width="60">Status</asp:tableheadercell>
				<asp:tableheadercell width="250">Kode Parcel</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

