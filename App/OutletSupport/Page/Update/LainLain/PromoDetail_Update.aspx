<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PromoDetail_Update.aspx.cs" Inherits="Page_Update_LainLain_PromoDetail_Update" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Promo Lain Lain (Promo Bank, Event)
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Detail
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
	<script>
        $(document).ready(function () {
            $('#<%=start.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
            $('#<%=end.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
        });
    </script>
    <div class="container-fluid">
        <form runat="server">
            <div class="panel panel-default width-800">
                <div class="panel-heading">
                    Promo
                </div>
                <div class="panel-body">
                    <div runat="server" id="divheader">
                        <table class="table table-bordered">
                            <tr>
                                <td>Nama Promo</td>
                                <td>
									<input runat="server" id="promoname" class="form-control" maxlength="100" />
								</td>
                            </tr>
                            <tr>
                                <td>Start Date</td>
                                <td>
									<input runat="server" id="start" class="form-control" />
								</td>
                            </tr>
                            <tr>
                                <td>End Date</td>
                                <td>
									<input runat="server" id="end" class="form-control" />
								</td>
                            </tr>
                            <tr>
                                <td>Status</td>
                                <td>
									<label runat="server" id="status"></label>
								</td>
                            </tr>
                            <tr>
                                <td>Keterangan</td>
                                <td>
									<input runat="server" id="keterangan" class="form-control" maxlength="200" />
								</td>
                            </tr>
                        </table>
                    </div>
                    <div runat="server" id="divdetail">
                        <asp:Table runat="server" id="tb" CssClass="table table-bordered">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell>No.</asp:TableHeaderCell>
								<asp:TableHeaderCell style="display:none">ID</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Minimum Value</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Maximum Value</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Discount</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </div>
					<div runat="server" id="divfooter">
						<button id="save" runat="server" class="btn btn-warning" onserverclick="save_Click">Save</button>
						<button id="cancel" runat="server" class="btn btn-default" onserverclick="cancel_Click">Cancel</button>
						<label id="warning" runat="server"></label>
					</div>
                </div>
            </div>
        </form>
    </div>
</asp:Content>
