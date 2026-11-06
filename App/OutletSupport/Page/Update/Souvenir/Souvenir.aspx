<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Souvenir.aspx.cs" inherits="Page_Create_Product" %>

<%@ register src="MenuPromo.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Souvenir
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update
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

	<uc1:menu id="menu" runat="server" />
	<form runat="server">        
		<div class="form-group width-500">
			<div class="row">
				<div class="col-xs-7">
					<label>
						Nama :
					</label>
					<asp:requiredfieldvalidator id="nama_RF" runat="server" />
					<asp:textbox id="nama" runat="server" cssclass="form-control" maxlength="50" />
				</div>
			</div>
		</div>
        <div class="form-group width-500">
			<div class="row">
				<div class="col-xs-7">
					<label>
						Keterangan :
					</label>
					
                    <asp:textbox id="keterangan" runat="server" cssclass="form-control" textmode="MultiLine" rows="4" />
				</div>
			</div>
		</div>        
        <div class="form-group width-500">
			<div class="row">
				<div class="col-xs-7">
					<label>
						Tanggal Mulai:
					</label>
                    <asp:requiredfieldvalidator id="datepicker_RF" runat="server" />		
                    <asp:TextBox CssClass="form-control text-center" type="text" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" />			
											
				</div>
			</div>
		</div>
        <div class="form-group width-500">
			<div class="row">
				<div class="col-xs-7">
					<label>
						Tanggal Expire:
					</label>
					<asp:requiredfieldvalidator id="datepicker2_RF" runat="server" />		
                    <asp:TextBox CssClass="form-control text-center" type="text" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" />						
					
				</div>
			</div>
		</div>        
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="next_Click">
				<i class="icon-arrow-right"></i>
				Next
			</button>
		</div>
	</form>
</asp:content>
