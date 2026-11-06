<%@ page language="C#" masterpagefile="~/MasterFile.master" autoeventwireup="true" codefile="3_File.aspx.cs" inherits="Page_Account_3_File" %>

<%@ register src="Menu3.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Precious Stone
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	<asp:literal id="h1_st" runat="server" />
</asp:content>
<asp:content contentplaceholderid="h1_id" runat="server">
	ID:<asp:literal id="h1_id" runat="server" />
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<div class="row">
		<div class="col-md-10">
			<div class="file-subheader hidden-sm hidden-xs">
				account
			</div>
			<table class="table-file">
				<tr>
					<td class="text-label">Keterangan</td>
					<td>:</td>
					<td id="keterangan" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Tanggal</td>
					<td>:</td>
					<td id="tgl" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Status</td>
					<td>:</td>
					<td id="disable" runat="server" />
				</tr>
			</table>
			<hr />
			<div class="file-subheader hidden-sm hidden-xs">
				parcel
			</div>
			<p class="text-lg text-danger" id="kodeparcel" runat="server" />
			<table class="table-file">
				<tr>
					<td class="text-label">Item</td>
					<td>:</td>
					<td id="parcel01" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Shape</td>
					<td>:</td>
					<td id="parcel02" runat="server" />
				</tr>
				<tr>
					<td class="text-label">Size</td>
					<td>:</td>
					<td id="parcel03" runat="server" />
				</tr>
                <tr>
					<td class="text-label">Colour</td>
					<td>:</td>
					<td id="parcel04" runat="server" />
				</tr>
                <tr>
					<td class="text-label">Clarity</td>
					<td>:</td>
					<td id="parcel06" runat="server" />
				</tr>
				<%--<tr>
					<td class="text-label">Colour Grading</td>
					<td>:</td>
					<td id="parcel05" runat="server" />
				</tr>
                <tr>
					<td class="text-label">Colour Comment</td>
					<td>:</td>
					<td id="parcel09" runat="server" />
				</tr>
                <tr>
					<td class="text-label">Brilliance</td>
					<td>:</td>
					<td id="parcel10" runat="server" />
				</tr>
                <tr>
					<td class="text-label">Treatment</td>
					<td>:</td>
					<td id="parcel07" runat="server" />
				</tr>
                <tr>
					<td class="text-label">Proportions</td>
					<td>:</td>
					<td id="parcel11" runat="server" />
				</tr>
				
				
				
				<tr>
					<td class="text-label">Country of Origin</td>
					<td>:</td>
					<td id="parcel08" runat="server" />
				</tr>

                <tr>
					<td class="text-label">Supplier</td>
					<td>:</td>
					<td id="parcel12" runat="server" />
				</tr>--%>
			</table>
		</div>
		<div class="col-md-2">
			<hr class="visible-sm visible-xs" />
			<asp:literal id="ap" runat="server" />
		</div>
	</div>
</asp:content>

