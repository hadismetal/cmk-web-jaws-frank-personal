<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PromoReport_Detail.aspx.cs" Inherits="Page_Report_LainLain_PromoReport_Detail" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Promo Lain Lain (Promo Bank, Event)
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Report
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
	<div class="container-fluid">
		<form runat="server">
            <div class="panel panel-default">
                <div class="panel-heading">
                    Promo
                </div>
                <div class="panel-body">
                    <div runat="server" id="divheader">
                        <table class="table table-responsive">
                            <tr>
                                <td>Nama Promo</td>
                                <td><label runat="server" id="promoname"></label></td>
                            </tr>
                            <tr>
                                <td>Start Date</td>
                                <td><label runat="server" id="start"></label></td>
                            </tr>
                            <tr>
                                <td>End Date</td>
                                <td><label runat="server" id="end"></label></td>
                            </tr>
                            <tr>
                                <td>Status</td>
                                <td><label runat="server" id="status"></label></td>
                            </tr>
                            <tr>
                                <td>Keterangan</td>
                                <td><label runat="server" id="keterangan"></label></td>
                            </tr>
                        </table>
                    </div>
                    <div runat="server" id="divdetail">
                        <asp:Table runat="server" id="tb" CssClass="table table-responsive">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell>No.</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Minimum Value</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Maximum Value</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Discount</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </div>
					<div class="row">
						<div class="col-xs-12">
							<div class="table-responsive">
								<asp:Table runat="server" id="tb1" CssClass="table table-bordered">
									<asp:TableHeaderRow>
										<asp:TableHeaderCell>No.</asp:TableHeaderCell>
										<asp:TableHeaderCell>No Transaction</asp:TableHeaderCell>
										<asp:TableHeaderCell>Tgl Transaction</asp:TableHeaderCell>
										<asp:TableHeaderCell>Invoice</asp:TableHeaderCell>
										<asp:TableHeaderCell>Store</asp:TableHeaderCell>
										<asp:TableHeaderCell>Customer</asp:TableHeaderCell>
										<asp:TableHeaderCell>Sales</asp:TableHeaderCell>
										<asp:TableHeaderCell>Product Type</asp:TableHeaderCell>
										<asp:TableHeaderCell>Product Code</asp:TableHeaderCell>
										<asp:TableHeaderCell>Item</asp:TableHeaderCell>
										<asp:TableHeaderCell>Qty</asp:TableHeaderCell>
										<asp:TableHeaderCell>Nett Price</asp:TableHeaderCell>
									</asp:TableHeaderRow>
								</asp:Table>
							</div>
						</div>
					</div>
				</div>
			</div>
		</form>
	</div>
</asp:Content>
