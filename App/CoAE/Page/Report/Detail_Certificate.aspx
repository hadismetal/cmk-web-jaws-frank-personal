<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Detail_Certificate.aspx.cs" Inherits="Page_Report_Detail_Certificate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="Server">
    Print Certificate
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="Server">
    Detail Report
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="Server">

    <div class="row">
        <div class="col-xs-12">
            <div class="panel panel-default">
                <div class="panel-body">

                    <div class="row">
                        <div class="col-xs-12 col-sm-4 col-md-4">
                            <table class="table-file">
                                <tr>
                                    <td class="text-label">Doc No.</td>
                                    <td>:</td>
                                    <td id="txdn" runat="server" />
                                </tr>
                                <tr>
                                    <td class="text-label">Nama Event</td>
                                    <td>:</td>
                                    <td id="txnama" runat="server" />
                                </tr>
                                <tr>
                                    <td class="text-label">Keterangan</td>
                                    <td>:</td>
                                    <td id="txketerangan" runat="server" />
                                </tr>
                                <tr>
                                    <td class="text-label">Status</td>
                                    <td>:</td>
                                    <td id="txstat" runat="server" />
                                </tr>

                            </table>
                        </div>
                        <div class="col-xs-12 col-sm-4 col-md-4">
                            <table class="table-file">
                                <tr>
                                    <td class="text-label">Date Create</td>
                                    <td>:</td>
                                    <td id="txdc" runat="server" />
                                </tr>
                                <tr>
                                    <td class="text-label">Date Expired</td>
                                    <td>:</td>
                                    <td id="txde" runat="server" />
                                </tr>
                                <tr>
                                    <td class="text-label">Operator</td>
                                    <td>:</td>
                                    <td id="txoperator" runat="server" />
                                </tr>
                                <tr>
                                    <td class="text-label">Total Qty</td>
                                    <td>:</td>
                                    <td id="txqty" runat="server" />
                                </tr>
                            </table>
                        </div>
                        <div class="col-xs-12 col-sm-4 col-md-4">
                            <table class="table-file">
                                <tr>
                                    <td class="text-label">Product Item</td>
                                    <td>:</td>
                                    <td id="tproditem" runat="server" />
                                </tr>
                                
                            </table>
                        </div>
                    </div>

                </div>
            </div>
        </div>

        <div class="col-xs-12">

            <div class="panel panel-default">
                <div class="panel-body">
                    <asp:Literal runat="server" ID="lit_plu_list" />

<%--                    <div class="file-subheader hidden-sm hidden-xs">
                        Data Item
                    </div>

                    <div class="table-responsive">
                        <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell Width="50"></asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="50">No</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="150">PLU & Item</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </div>--%>
                </div>
            </div>
        </div>

    </div>

</asp:Content>

