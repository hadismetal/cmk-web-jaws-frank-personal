<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Stone4Rekap.aspx.cs" inherits="Page_Account_Stone" %>

<asp:content contentplaceholderid="h1" runat="server">
	Semi Precious Stone
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Report
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
                &nbsp;
            </div>
            <asp:DropDownList ID="supplier" runat="server" CssClass="form-control" Width="175">
                <asp:ListItem Value="">Supplier</asp:ListItem>
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
                <button id="Button2" runat="server" class="btn btn-primary" onserverclick="Print">
					<i class="icon-search"></i>
					Print
				</button>
			</div>
		</div>
	</form>
	<div class="table-responsive">
		<asp:table id="tb" runat="server" cssclass="table-list">
			<asp:tableheaderrow>
				<asp:tableheadercell width="40">No.</asp:tableheadercell>
				<asp:tableheadercell width="150">Nama</asp:tableheadercell>
                <asp:tableheadercell width="150">Shape</asp:tableheadercell>
                <asp:tableheadercell width="150">Colour</asp:tableheadercell>
				<asp:tableheadercell Width="100" CssClass ="text-right">Total Butir</asp:tableheadercell>
				<asp:tableheadercell Width="100" CssClass ="text-right">Total Carat</asp:tableheadercell>
                <asp:tableheadercell Width="170" CssClass ="text-right">Harga Per Karat USD</asp:tableheadercell>
                <asp:tableheadercell Width="170" CssClass ="text-right">Total Harga USD</asp:tableheadercell>
				<asp:tableheadercell Width="170" CssClass ="text-right">Harga Satuan M</asp:tableheadercell>
                <asp:tableheadercell Width="170" CssClass ="text-right">Harga Total M</asp:tableheadercell>
			</asp:tableheaderrow>
		</asp:table>
	</div>
</asp:content>

