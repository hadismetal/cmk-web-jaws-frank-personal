<%@ page language="C#" Debug="true" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="MDRRate.aspx.cs" inherits="Page_Create_Product" %>

<asp:content contentplaceholderid="h1" runat="server">
	MDR Rate
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">

	<form runat="server">        
		<div class="form-group width-500">
			<div class="row">
				<div class="col-xs-7">
					<label>
						EDC :
					</label>
					<asp:DropDownList ID="edc" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0">EDC</asp:ListItem>
					</asp:DropDownList>
				</div>
			</div>
		</div>
        <div class="form-group width-500">
			<div class="row">
				<div class="col-xs-7">
					<label>
						Jenis Kartu :
					</label>
					<asp:DropDownList ID="jeniskartu" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0">Jenis Kartu</asp:ListItem>
					</asp:DropDownList>
				</div>
			</div>
		</div>
        <div class="form-group width-500">
			<div class="row">
				<div class="col-xs-7">
					<label>
						Program :
					</label>
					<asp:DropDownList ID="program" runat="server" CssClass="form-control">
                        <asp:ListItem Value="0">Program</asp:ListItem>
					</asp:DropDownList>
				</div>
			</div>
		</div>          
        <div class="form-group width-500">
			<div class="row">
				<div class="col-xs-7">
					<label>
						Rate :
					</label>
					<asp:requiredfieldvalidator id="rate_RF" runat="server" />
					<asp:textbox id="rate" runat="server" cssclass="form-control" maxlength="50" />
				</div>
			</div>
		</div>  
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="next_Click">
				<i class="icon-arrow-right"></i>
				Save
			</button>
		</div>
	</form>
</asp:content>
