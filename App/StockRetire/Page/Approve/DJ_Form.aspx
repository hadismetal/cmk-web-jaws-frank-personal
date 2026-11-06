<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="DJ_Form.aspx.cs" Inherits="Page_Approval_DJ_Retirement" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewelry
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Approval
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server">
        <p>
            <span class="text-muted">Nomor :</span><br />
            <span class="text-lg" id="header_nomor" runat="server" />
        </p>
        <hr />
        <table class="table-file" width="500">
            <tr>
                <td class="text-muted">Petugas Lebur</td>
                <td>
                    <asp:Literal ID="petugaslebur" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Keterangan</td>
                <td>
                    <asp:Literal ID="keterangan" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="text-muted">Alasan</td>
                <td>
                    <asp:Literal ID="alasan" runat="server" />
                </td>
            </tr>
        </table>
        <asp:Button ID="btnspoiler" runat="server" Text="Show / Hide" OnClick="btnspoiler_Click" />
        <hr />

        <span id="spoiler" runat="server" visible="false">
            <div class="file-subheader hidden-sm hidden-xs">
                frame
            </div>
            <table class="table-file">
                <tr>
                    <td class="text-label">Netto Weight</td>
                    <td>:</td>
                    <td class="text-bold text-right fixed-150" id="netto" runat="server" />
                    <td><small class="text-silver">Gram</small></td>
                </tr>
                <tr>
                    <td class="text-label">Susut Bongkar</td>
                    <td>:</td>
                    <td class="text-bold text-right fixed-150">
                        <asp:Literal ID="susutbongkar" runat="server" />
                    </td>
                    <td><small class="text-silver">Gram</small></td>
                </tr>
                <tr>
                    <td class="text-label">Berat Setelah Bongkar</td>
                    <td>:</td>
                    <td class="text-bold text-right fixed-150">
                        <asp:Literal ID="nettobongkar" runat="server" />
                    </td>
                    <td><small class="text-silver">Gram</small></td>
                </tr>
                <tr>
                    <td class="text-label">Susut Lebur</td>
                    <td>:</td>
                    <td class="text-bold text-right fixed-150">
                        <asp:Literal ID="susutlebur" runat="server" />
                    </td>
                    <td><small class="text-silver">Gram</small></td>
                </tr>
                <tr>
                    <td class="text-label">Berat Akhir</td>
                    <td>:</td>
                    <td class="text-bold text-right fixed-150">
                        <asp:Literal ID="beratakhir" runat="server" />
                    </td>
                    <td><small class="text-silver">Gram</small></td>
                </tr>
                <tr>
                    <td class="text-label">Kadar Tukaran</td>
                    <td>:</td>
                    <td class="text-bold text-right fixed-150" id="kadar" runat="server" />
                    <td><small class="text-silver">%</small></td>
                </tr>
            </table>
            <hr />
            <div class="file-subheader hidden-sm hidden-xs">
                transaction data
            </div>
            <div class="row">
                <div class="col-md-5">
                    <table class="table-file">
                        <tr>
                            <td class="text-label">No. Prototype</td>
                            <td>:</td>
                            <td id="protonomor" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">No. Product Order</td>
                            <td>:</td>
                            <td id="productordernomor" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Supplier</td>
                            <td>:</td>
                            <td id="suppliernama" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">No. Supplier</td>
                            <td>:</td>
                            <td id="suppliernomor" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Req. Outlet</td>
                            <td>:</td>
                            <td id="requestoutletnama" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Req. Customer</td>
                            <td>:</td>
                            <td id="requestcustomernama" runat="server" />
                        </tr>
                    </table>
                </div>
                <br class="visible-sm visible-xs" />
                <div class="col-md-5">
                    <table class="table-file">
                        <tr>
                            <td class="text-label">Designer</td>
                            <td>:</td>
                            <td id="humandesignernama" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Stock Keeper</td>
                            <td>:</td>
                            <td id="humanstocknama" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Expired Product</td>
                            <td>:</td>
                            <td id="expiredproduct" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Expired Request</td>
                            <td>:</td>
                            <td id="expiredrequest" runat="server" />
                        </tr>
                    </table>
                </div>
            </div>
            <hr />
            <div class="file-subheader hidden-sm hidden-xs">
                product characteristics
            </div>
            <div class="row">
                <div class="col-md-5">
                    <table class="table-file">
                        <tr>
                            <td class="text-label">Product Item</td>
                            <td>:</td>
                            <td id="productitem" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Product Category</td>
                            <td>:</td>
                            <td id="productcategory" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Product Level</td>
                            <td>:</td>
                            <td id="productlevel" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Stone Distribution</td>
                            <td>:</td>
                            <td id="stonedist" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Frame Material</td>
                            <td>:</td>
                            <td id="framematerial" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Frame Finishing</td>
                            <td>:</td>
                            <td id="framefinishing" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Frame Color</td>
                            <td>:</td>
                            <td id="framecolor" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Cons. Process</td>
                            <td>:</td>
                            <td id="processcons" runat="server" />
                        </tr>
                    </table>
                </div>

                <br class="visible-sm visible-xs" />
                <div class="col-md-5">
                    <table class="table-file">
                        <tr>
                            <td class="text-label">Gross Weight</td>
                            <td>:</td>
                            <td class="fixed-100 text-right" id="grossweight" runat="server" />
                            <td class="text-silver"><small>Gram</small></td>
                        </tr>
                        <tr>
                            <td class="text-label">Stone Carat</td>
                            <td>:</td>
                            <td class="fixed-100 text-right" id="stonecarat" runat="server" />
                            <td class="text-silver"><small>Carat</small></td>
                        </tr>
                        <tr>
                            <td class="text-label">Stone Weight</td>
                            <td>:</td>
                            <td class="fixed-100 text-right" id="stoneweight" runat="server" />
                            <td class="text-silver"><small>Gram</small></td>
                        </tr>
                        <tr>
                            <td class="text-label">Netto Weight</td>
                            <td>:</td>
                            <td class="fixed-100 text-right" id="nettoweight" runat="server" />
                            <td class="text-silver"><small>Gram</small></td>
                        </tr>
                    </table>
                    <table class="table-file table-file-nomargin">
                        <tr>
                            <td class="text-label">Diameter</td>
                            <td>:</td>
                            <td class="fixed-75 text-right" id="dimensid" runat="server" />
                            <td class="text-silver"><small>MM</small></td>
                        </tr>
                        <tr>
                            <td class="text-label">Panjang</td>
                            <td>:</td>
                            <td class="fixed-75 text-right" id="dimensip" runat="server" />
                            <td class="text-silver"><small>MM</small></td>
                        </tr>
                        <tr>
                            <td class="text-label">Lebar</td>
                            <td>:</td>
                            <td class="fixed-75 text-right" id="dimensil" runat="server" />
                            <td class="text-silver"><small>MM</small></td>
                        </tr>
                        <tr>
                            <td class="text-label">Ring Size</td>
                            <td>:</td>
                            <td class="fixed-75 text-right" id="dimensir" runat="server" />
                            <td class="text-silver"><small>MM</small></td>
                        </tr>
                    </table>
                </div>
            </div>
            <hr />
            <div class="file-subheader hidden-sm hidden-xs">
                design characteristics
            </div>
            <div class="row">
                <div class="col-md-5">
                    <table class="table-file">
                        <tr>
                            <td class="text-label">Design Category</td>
                            <td>:</td>
                            <td id="designcategory" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Design Concept</td>
                            <td>:</td>
                            <td id="designconcept" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Design Process</td>
                            <td>:</td>
                            <td id="designprocess" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Target Age</td>
                            <td>:</td>
                            <td id="targetage" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Target Gender</td>
                            <td>:</td>
                            <td id="targetgender" runat="server" />
                        </tr>
                    </table>
                </div>
                <br class="visible-sm visible-xs" />
                <div class="col-md-5">
                    <table class="table-file">
                        <tr>
                            <td class="text-label">Naming Product</td>
                            <td>:</td>
                            <td id="naming" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Keterangan</td>
                            <td>:</td>
                            <td id="designket" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Grafir</td>
                            <td>:</td>
                            <td id="grafir" runat="server" />
                        </tr>
                        <tr>
                            <td class="text-label">Finishing</td>
                            <td>:</td>
                            <td id="finishing" runat="server" />
                        </tr>
                    </table>
                </div>
            </div>
        </span>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            stone
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-report">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Stone</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tgl.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass="text-center">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass="text-center">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass="text-center">Total Butir diterima</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass="text-center">Total Carat diterima</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass="text-center">Total Butir rusak</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass="text-center">Total Carat rusak</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass="text-center">Total Butir Selisih</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass="text-center">Total Carat Selisih</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>

        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            price
        </div>
        <div class="row">

            <div class="col-md-5">
                <p>
                    <span class="text-muted">Final Price (IDR) :</span><br />
                    <span class="text-lg text-orange" id="hargaidr" runat="server" />
                    <small class="text-silver">IDR</small><br />
                </p>
            </div>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            picture
        </div>
        <asp:Image ID="imgpicture" runat="server" CssClass="img-thumbnail" Width="500" />
        <hr />

        <div class="button-left">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-check"></i>
                Approve
            </button>
            <button id="reject" runat="server" class="btn btn-danger" onserverclick="reject_Click">
                <i class="icon-trash"></i>
                Reject
            </button>
        </div>
    </form>
</asp:Content>

