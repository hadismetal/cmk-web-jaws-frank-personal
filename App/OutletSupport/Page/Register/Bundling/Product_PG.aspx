<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Product_PG.aspx.cs" inherits="Page_Create_ProductAddr" %>

<%@ register src="MenuPromo.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Promo Bundling
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />	
	<hr />
	<form runat="server">
		<div class="form-group width-500">
			<div class="row">
				<div class="col-xs-7">
					<label>
						Model :
					</label>
					<asp:DropDownList cssclass="form-control" id="model" runat="server" AutoPostBack="true" OnSelectedIndexChanged="category_SelectedIndexChanged">
                    <asp:ListItem Value="0"> All</asp:ListItem>
                        </asp:DropDownList>
				</div>
			</div>
		</div>
        <div class="form-group width-500">
			<div class="row">
				<div class="col-xs-7">
					<label>
						Product Level :
					</label>
					<asp:DropDownList cssclass="form-control" id="productlevel" runat="server" AutoPostBack="true" OnSelectedIndexChanged="category_SelectedIndexChanged">
                    <asp:ListItem Value="0"> All</asp:ListItem>
                        </asp:DropDownList>
				</div>
			</div>
		</div>
        <div class="form-group width-500">
			<div class="row">
				<div class="col-xs-7">
					<label>
						Product Item :
					</label>
					<asp:DropDownList cssclass="form-control" id="productitem" runat="server" AutoPostBack="true" OnSelectedIndexChanged="category_SelectedIndexChanged">
                    <asp:ListItem Value="0"> All</asp:ListItem>
                        </asp:DropDownList>
				</div>
			</div>
		</div>        
        <div class="form-group width-500">
			<div class="row">
				<div class="col-xs-7">
					<label>
						Product : <asp:CheckBox ID="all" runat="server" Text="All"/>      
					</label>                    
                    <asp:ListBox CssClass="form-control" runat="server" ID="prod" Height="400" SelectionMode="Multiple"></asp:ListBox>
                    					
				</div>
			</div>
		</div>		
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next
			</button>
            <button id="skip" runat="server" class="btn btn-success" onserverclick="skip_Click">
				<i class="icon-remove"></i>
				Skip
			</button>
		</div>
	</form>
</asp:content>

