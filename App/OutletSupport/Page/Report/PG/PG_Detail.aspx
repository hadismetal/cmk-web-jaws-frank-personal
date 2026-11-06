<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PG_Detail.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Promo Plain Gold Detail 
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <form runat="server">
    <div>
        <div class="row">
            <div class="col-xs-12 col-sm-6">
                <asp:Literal runat="server" ID="lit_header1" />
            </div>
            <div class="col-xs-12 col-sm-6">
                <asp:Literal runat="server" ID="lit_header2" />
            </div>
        </div>
        
    </div>
    <div id="form_area" runat="server">
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableRow>
                    <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">No. PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Category</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Level</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Current Location</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200">Current Status</asp:TableHeaderCell>
                </asp:TableRow>
            </asp:Table>
        </div>
        <div class="button-inline">
            <button id="back" runat="server" class="btn btn-primary" onserverclick="back_Click">
				<span class="glyphicon glyphicon-arrow-left" aria-hidden="true"></span>
				Back
			</button>
		</div>
    </div>
    </form>
</asp:Content>

