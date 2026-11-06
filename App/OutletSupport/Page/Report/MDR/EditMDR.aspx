<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="EditMDR.aspx.cs" Inherits="Page_Approval_Product" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    MDR Rate
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Update
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form runat="server">        
			<div class="form-group width-500">
				<div class="row">
					<div class="col-xs-7">
						<label>
							EDC :
						</label>
						<asp:TextBox ID="edc" runat="server" CssClass="form-control" Style="text-transform: uppercase;"/>
					</div>
				</div>
			</div>
			<div class="form-group width-500">
				<div class="row">
					<div class="col-xs-7">
						<label>
							Jenis Kartu :
						</label>
						<asp:TextBox ID="jenisKartu" runat="server" CssClass="form-control" Style="text-transform: uppercase;" />
					</div>
				</div>
			</div>
			<div class="form-group width-500">
				<div class="row">
					<div class="col-xs-7">
						<label>
							Program :
						</label>
						<asp:TextBox ID="program" runat="server" CssClass="form-control" Style="text-transform: uppercase;" />
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
				<button id="save" runat="server" class="btn btn-primary" onserverclick="save_ServerClick">
					<i class="icon-arrow-right"></i>
					Save
				</button>
			</div>
		</form>
    </div>
</asp:Content>
