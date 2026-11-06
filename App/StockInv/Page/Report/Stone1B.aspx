<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Stone1B.aspx.cs" inherits="Page_Account_Stone" %>

<asp:content contentplaceholderid="h1" runat="server">
	Diamond Rapaport
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Report
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server" class="form-inline">
		<div class="row">							
			<div class="col-md-12">
				<div class="form-group">
					<div class="text-muted">
						Shape :
					</div>
					<asp:dropdownlist id="parcel01" runat="server" cssclass="form-control" width="175">
						<asp:listitem Value="0">Shape :</asp:listitem>
					</asp:dropdownlist>
				</div>
				<div class="form-group">
					<div class="text-muted">
						Size :
					</div>
					<asp:dropdownlist id="parcel02" runat="server" cssclass="form-control" width="175">
						<asp:listitem Value="0">Size :</asp:listitem>
					</asp:dropdownlist>
				</div>
				<div class="form-group">
					<div class="text-muted">
						Colour :
					</div>
					<asp:dropdownlist id="parcel03" runat="server" cssclass="form-control" width="175">
						<asp:listitem Value="0">Colour :</asp:listitem>
					</asp:dropdownlist>
				</div>
				<div class="form-group">
					<div class="text-muted">
						Clarity :
					</div>
					<asp:dropdownlist id="parcel04" runat="server" cssclass="form-control" width="175">
						<asp:listitem Value="0">Clarity :</asp:listitem>
					</asp:dropdownlist>
				</div>			
				<div class="form-group">
					<div class="text-muted">
						Cutting :
					</div>
					<asp:dropdownlist id="parcel05" runat="server" cssclass="form-control" width="175">
						<asp:listitem Value="0">Cutting :</asp:listitem>
					</asp:dropdownlist>
				</div>
				<div class="form-group">
					<div class="text-muted">
						Brand :
					</div>
					<asp:dropdownlist id="parcel06" runat="server" cssclass="form-control" width="175">
						<asp:listitem Value="0">Brand :</asp:listitem>
					</asp:dropdownlist>
				</div>
				<div class="form-group">
					<div class="text-muted">
						Category :
					</div>
					<asp:dropdownlist id="parcel07" runat="server" cssclass="form-control" width="175">
						<asp:listitem Value="0">Category :</asp:listitem>
					</asp:dropdownlist>
				</div>	
			</div>
			        <div class="clearfix">
			<br class="hidden-xs" />
		</div>		
		<div class="col-md-12">
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
					<div class="text-muted">
						Search :
					</div>
					<asp:textbox id="kw" runat="server" cssclass="form-control" width="350" placeholder="Input Keyword..." />
				</div>
				<div class="form-group">
					<div class="text-muted">
						Diurutkan :
					</div>
					<asp:dropdownlist id="order" runat="server" cssclass="form-control" width="175">
						<asp:listitem value="Nama">Nama</asp:listitem>
						<asp:listitem value="Nomor">PLU</asp:listitem>
						<asp:listitem value="KodeParcel">Kode Parcel</asp:listitem>
						<asp:listitem value="ID">ID</asp:listitem>
					</asp:dropdownlist>
					<div class="text-00 visible-xs">&nbsp;</div>
					<asp:dropdownlist id="orderby" runat="server" cssclass="form-control" width="175">
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
			</div>		
		</div>
        <div class="clearfix">
			<br class="hidden-xs" />
		</div>        							
		<div class="row">
			<div class="col-md-6">				
				<div class="form-group">
					<div class="hidden-xs">&nbsp;</div>
					<div>
						<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
							<i class="icon-search"></i>
							Display
						</button>
						<button id="Button" runat="server" class="btn btn-primary" onserverclick="Print">
							<i class="icon-print"></i>
							Print
						</button>
						<button id="Button1" runat="server" class="btn btn-primary" onserverclick="Export">
							<i class="icon-download-alt"></i>
							Export
						</button>
						<div style="display: none;" id="startROW" ClientIDMode="Static" runat="server">1</div> 
					
        		        <div class="btn-group" role="group" id="paginationGRP" runat="server" ClientIDMode="Static">
        		        <asp:Label ID="lpage" runat="server" Visible="false"></asp:Label>
					</div>
				</div>
			</div>
		</div>
	</form>
	<div class="clearfix">
			<br class="hidden-xs" />
	</div>
	<div class="container">
		<div class="table-responsive">
			<asp:table id="tb" runat="server" cssclass="table-list">
				
			</asp:table>
		</div>
	</div>
    <asp:Literal ID="paging_text" runat="server" />
</asp:content>

