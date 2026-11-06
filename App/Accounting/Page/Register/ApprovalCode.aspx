<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ApprovalCode.aspx.cs" Inherits="Page_Register_ApprovalCode" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Approval Code
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
	<div id="zero" runat="server" visible="false">
        <i class="icon-ok icon-5x pull-left text-success"></i>
        <h4>Proses Selesai</h4>
        <p class="text-muted">
            Tidak ada transaksi penjualan
        </p>
        <div class="clearfix"></div>
        <hr />
    </div>
    <form runat="server">
        <div class="modal fade" role="alert" id="divalert">
            <div class="modal-dialog modal-sm">
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title text-center">Message</h4>
                    </div>
                    <div class="modal-body">
                        <h2 class="text-center" id="lblmsg"></h2>
                    </div>
                </div>
            </div>
        </div>
        <!-- Modal -->
        <div class="modal fade" id="modalupdateappcode" role="dialog">
            <div class="modal-dialog">
                <!-- Modal content-->
                <div class="modal-content">
                    <div class="modal-header">
                        <button type="button" class="close" data-dismiss="modal">&times;</button>
                        <h4 class="modal-title">Warning</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-horizontal container">
                            <input type="hidden" id="txtidtable" />
                            <input type="hidden" id="textisdp" value="F" />
                            <div class="form-group">
                                <label class="control-label col-sm-4" for="email">Approval Code:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" id="txtappcodemodal" placeholder="Enter approval code" />
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4" for="email">EDC:</label>
                                <div class="col-sm-8">
                                    <!--<input type="text" class="form-control" id="txtedcmodal" placeholder="Enter approval code" disabled/>-->
                                    <asp:DropDownList runat="server" ID="ddledc" class="form-control">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <label class="control-label col-sm-4" for="pwd">Nominal:</label>
                                <div class="col-sm-8">
                                    <input type="text" class="form-control" id="txtnominalmodal" placeholder="Enter nominal" disabled />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <button type="button" class="btn btn-success" onclick="onClickUpdateAppCode();">Save Update</button>
                        <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        <script type='text/javascript'>
                            function onClickUpdateAppCode() {
                                var appcode = document.getElementById("txtappcodemodal").value;
                                var id = document.getElementById("txtidtable").value;
                                var isdp = document.getElementById("textisdp").value;
                                var idedc = document.getElementById("content_ddledc").value;
                                var so = document.getElementById("content_so").value;
                                window.location.href = "AppovalCode_Submit.aspx?id=" + id + "&appcode=" + appcode + "&so=" + so + "&isdp=" + isdp + "&idedc=" + idedc;
                            }
                        </script>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group">
            <div class="row">
                <div class="col-md-12">
                    <table>
                        <tr>
                            <td>
                                <asp:TextBox ID="so" runat="server" CssClass="form-control" MaxLength="100" placeholder="Input SO/DP" />
                            </td>
                            <td>
                                <asp:DropDownList runat="server" CssClass="form-control" ID="ddltipe">
                                    <asp:ListItem Value="SO">Sales Order</asp:ListItem>
                                    <asp:ListItem Value="DP">Down Payment</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td>
                                <asp:Button runat="server" CssClass="btn btn-primary" Text="Search" OnClick="Search_Click" />
                            </td>
                            <td>
                                <asp:Label ID="so_RF" runat="server" ControlToValidate="so" Text="Nomor SO tidak boleh kosong." Visible="false" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
        <hr />
        <asp:Table runat="server" ID="tblPayment" CssClass="table table-bordered table-responsive">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell CssClass="text-center">Nomor SO/DP</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="text-center">Payment Type</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="text-center">Bank</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="text-center">Approval Code</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="text-center">EDC Bank</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="text-center">Nominal</asp:TableHeaderCell>
                <asp:TableHeaderCell CssClass="text-center"></asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>

    </form>
</asp:Content>

