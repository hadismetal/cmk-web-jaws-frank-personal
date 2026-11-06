<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PromoDetail.aspx.cs" Inherits="Page_Approval_LainLain_PromoDetail" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Promo Lain Lain (Promo Bank, Event)
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Approval
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <!-- Modal -->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Message</h4>
                </div>
                <div class="modal-body">
                    <h2 class="text-center">
                        <asp:Label runat="server" ID="lblMessage">Proses Berhasil</asp:Label>
                    </h2>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <form runat="server">
            <div class="panel panel-default width-800">
                <div class="panel-heading">
                    Promo
                </div>
                <div class="panel-body">
                    <div runat="server" id="divheader">
                        <table class="table">
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
                        <asp:Table runat="server" id="tb" CssClass="table table-bordered">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell>No.</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Minimum Value</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Maximum Value</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Discount</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </div>
					<div runat="server" id="divfooter">
						<asp:Button runat="server" ID="btnApprove" Text="Approve" CssClass="btn btn-success" OnClick="Approve_Click"></asp:Button>
						<asp:Button runat="server" ID="btnReject" Text="Reject" CssClass="btn btn-danger" OnClick="Reject_Click"></asp:Button>
						<asp:Label runat="server" ID="lblWarning" Visible="false" CssClass="text-danger"></asp:Label>
					</div>
                </div>
            </div>
        </form>
    </div>
</asp:Content>
