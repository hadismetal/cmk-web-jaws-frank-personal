<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/MasterPage.master" CodeFile="SalesInvoicePDF.aspx.cs" Inherits="Page_Report_Kalkulator_SalesInvoicePDF" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Kalkulator Buyback
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form runat="server">
		<div class="row">
			<div class="col-md-12">
				<div class="form-inline">
					<div class="form-group">
						<div class="text-muted">
							Search By :
						</div>
						<asp:TextBox ID="kwplu" runat="server" CssClass="form-control" Width="350" placeholder="Input PLU" />
					</div>
					<div class="form-group" runat="server">
						<div class="text-muted">
							&nbsp;
						</div>
						<asp:Button ID="go" runat="server" CssClass="btn btn-info" Text="Display" OnClick="go_Click"></asp:Button>
					</div>
				</div>
			</div>
			<div class="col-md-12">
				<asp:GridView ID="lblresult" runat="server"></asp:GridView>
			</div>
		</div>
    </form>
</asp:Content>
