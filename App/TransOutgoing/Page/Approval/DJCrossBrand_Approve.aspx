<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJCrossBrand_Approve.aspx.cs" Inherits="DJ_Finish" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewelry
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Approval
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
        form summary
    </div>
    <div class="row">
        <div class="col-md-5">
            <table class="table-file">
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
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
        product list summary
    </div>
    <div class="row">
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
        <div class="col-md-5">
            <table class="table-file">
                <tr>
                    <td class="text-label">Total Harga</td>
                    <td>:</td>
                    <td id="totalhargaM" runat="server" class="text-bold" style="text-align: right;" />
                </tr>
            </table>
        </div>
    </div>
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
        product list
    </div>
    <div class="row">
        <br class="visible-sm visible-xs" />
        <div class="form-inline">
            <form id="Form1" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <div class="table-responsive">
                    <asp:Table ID="tb" runat="server" CssClass="table-list">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="30" />
                            <asp:TableHeaderCell Width="110">Image</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="185">Product Item</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120">Status</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120">Net Weight</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120">Gross Weight</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120">T. Butir</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120">Carat</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120">Harga Jual</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>

                    <div class="form-inline">
                        <asp:CheckBoxList runat="server" ID="cbl_Product" />
                    </div>
                </div>
            </form>
        </div>
    </div>
    <hr />
    <asp:Literal ID="lit_msg" runat="server"></asp:Literal>
    <div class="row">
        <br class="visible-sm visible-xs" />
        <div class="col-md-5">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click1">
                <i class="icon-arrow-right"></i>
                Approve
            </button>
            <button id="update" runat="server" class="btn btn-info" onserverclick="update_Click">
                <i class="icon-arrow-right"></i>
                Update
            </button>
            <button id="reject" runat="server" class="btn btn-danger" onserverclick="reject_Click">
                <i class="icon-arrow-right"></i>
                Reject
            </button>
        </div>
    </div>


</asp:Content>

