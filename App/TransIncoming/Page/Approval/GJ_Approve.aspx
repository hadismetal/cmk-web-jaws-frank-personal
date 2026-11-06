<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GJ_Approve.aspx.cs" Inherits="GJ_Approve" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Gold Jewellery
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server">

        <hr />

        <div class="panel panel-info">
            <div class="panel-heading hidden-sm hidden-xs">
            <h3 class="panel-title">Form Summary</h3>
            </div>
                    <div class="panel-body">
                        <div class="col-md-5">
                        <table class="table-file">
                            <tr>
                                <td class="text-label">Outgoing Number</td>
                                <td>:</td>
                                <td id="outgoing" runat="server" class="text-bold" />
                            </tr>
                            <tr>
                                <td class="text-label">Market Brand</td>
                                <td>:</td>
                                <td id="brand" runat="server" class="text-bold" />
                            </tr>
                            <tr>
                                <td class="text-label">From
                                    <asp:Literal runat="server" ID="lit_type_from" /></td>
                                <td>:</td>
                                <td id="from" runat="server" class="text-bold" />
                            </tr>
                            <tr>
                                <td class="text-label">To
                                    <asp:Literal runat="server" ID="lit_type_to" /></td>
                                <td>:</td>
                                <td id="to" runat="server" class="text-bold" />
                            </tr>
                        </table>
                    </div>
                    <br class="visible-sm visible-xs" />
                    <div class="col-md-5">
                        <table class="table-file">
                            <tr>
                                <td class="text-label">Tanggal</td>
                                <td>:</td>
                                <td id="tgl" runat="server" class="text-bold" />
                            </tr>
                            <tr>
                                <td class="text-label">Kurir</td>
                                <td>:</td>
                                <td id="kurir" runat="server" class="text-bold" />
                            </tr>
                            <tr>
                                <td class="text-label">Keterangan</td>
                                <td>:</td>
                                <td id="keterangan" runat="server" class="text-bold" />
                            </tr>
                            <tr>
                                <td class="text-label">Tanggal ETA</td>
                                <td>:</td>
                                <td id="eta" runat="server" class="text-bold" />
                            </tr>
                        </table>
                    </div>
            </div>
        </div>

        <div class="panel panel-info">
          <div class="panel-heading hidden-sm hidden-xs">
            <h3 class="panel-title">Product List Summary</h3>
          </div>
          <div class="panel-body">
                <br class="visible-sm visible-xs" />
                <div class="col-md-5">
                    <table class="table-file">
                        <tr>
                            <td class="text-label">Qty</td>
                            <td>:</td>
                            <td id="totalqty" runat="server" class="text-bold" style="text-align: right;" />
                        </tr>
                        <tr>
                            <td class="text-label">Gross Weight</td>
                            <td>:</td>
                            <td id="totalgross" runat="server" class="text-bold" style="text-align: right;" />
                        </tr>
                        <tr>
                            <td class="text-label">Netto Weight</td>
                            <td>:</td>
                            <td id="totalnetto" runat="server" class="text-bold" style="text-align: right;" />
                        </tr>
                        <tr>
                            <td class="text-label">Butir</td>
                            <td>:</td>
                            <td id="totalbutir" runat="server" class="text-bold" style="text-align: right;" />
                        </tr>
                        <tr>
                            <td class="text-label">Carat</td>
                            <td>:</td>
                            <td id="totalcarat" runat="server" class="text-bold" style="text-align: right;" />
                        </tr>
                        <tr>
                            <td class="text-label">Product Item</td>
                            <td>:</td>
                            <td id="product_item_summary" runat="server" class="text-bold" style="text-align: right;" />
                        </tr>
                        <tr>
                            <td class="text-label">Product Status</td>
                            <td>:</td>
                            <td id="product_status_summary" runat="server" class="text-bold" style="text-align: right;" />
                        </tr>
                    </table>
                </div>
          </div>
        </div>

        <div class="file-subheader hidden-sm hidden-xs">
            accepted
        </div>
        <div class="row">
            <br class="visible-sm visible-xs" />
            <div class="form-inline">

                <div class="table-responsive">
                    <asp:Table ID="tb" runat="server" CssClass="table-list">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="30" />
                            <asp:TableHeaderCell Width="110">Image</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="185">Product Item</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120">Status</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120">Net Weight</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120">Gross Weight</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>

                    <div class="form-inline">
                        <asp:CheckBoxList runat="server" ID="cbl_Product" />
                    </div>
                </div>

            </div>
        </div>

        <p runat="server" visible="false">
        <div class="file-subheader hidden-sm hidden-xs">
            broken
        </div>
        <div class="row">
            <br class="visible-sm visible-xs" />
            <div class="form-inline">

                <div class="table-responsive">
                    <asp:Table ID="Table1" runat="server" CssClass="table-list">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="30" />
                            <asp:TableHeaderCell Width="110">Image</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="185">Product Item</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120">Status</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120">Net Weight</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120">Gross Weight</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>

                    <div class="form-inline">
                        <asp:CheckBoxList runat="server" ID="cbl_Product_2" />
                    </div>
                </div>

            </div>
        </div>
        <hr />
        
        <div class="file-subheader hidden-sm hidden-xs">
            trouble / lost
        </div>
        <div class="row">
            <br class="visible-sm visible-xs" />
            <div class="form-inline">

                <div class="table-responsive">
                    <asp:Table ID="Table2" runat="server" CssClass="table-list">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="30" />
                            <asp:TableHeaderCell Width="110">Image</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="185">Product Item</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120">Status</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120">Net Weight</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120">Gross Weight</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>

                    <div class="form-inline">
                        <asp:CheckBoxList runat="server" ID="cbl_Product_3" />
                    </div>
                </div>

            </div>
        </div>
        <hr />
        </p>
        <div class="file-subheader hidden-sm hidden-xs">
            qc summary
        </div>
        <div class="row">
            <br class="visible-sm visible-xs" />
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">accepted</td>
                        <td>:</td>
                        <td id="accepted" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                       <%-- <td class="text-label">broken</td>
                        <td>:</td>--%>
                        <td id="broken" runat="server" class="text-bold" colspan="3" visible="false" />
                    </tr>
                    <tr>
                       <%-- <td class="text-label">trouble / lost</td>
                        <td>:</td>--%>
                        <td id="troublelost" runat="server" class="text-bold" colspan="3" visible="false"/>
                    </tr>

                </table>
            </div>
        </div>
        <hr />
        <div class="row">
            <br class="visible-sm visible-xs" />
            <div class="col-md-5">
                <button id="save" runat="server" class="btn btn-success" onserverclick="approve_Click">
                    <i class="icon-arrow-right"></i>
                    Approve
                </button>
                <button id="Button1" runat="server" class="btn btn-danger" onserverclick="reject_Click" visible="false">
                    <i class="icon-arrow-right"></i>
                    Reject
                </button>
            </div>
        </div>
    </form>
</asp:Content>

