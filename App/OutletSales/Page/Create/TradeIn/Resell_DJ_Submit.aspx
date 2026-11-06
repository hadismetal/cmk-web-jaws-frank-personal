<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Resell_DJ_Submit.aspx.cs" Inherits="Page_Register_DJ_Submit" %>

<%@ Register Src="MenuDJ.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server">
        <div class="text-danger text-sm">
            Tanggal Cetak :
            <asp:Literal ID="tglcetak" runat="server" />
        </div>
        <br />

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
                        <td class="text-label">Finishing</td>
                        <td>:</td>
                        <td id="finishing" runat="server" />
                    </tr>
                </table>
            </div>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            Price
        </div>
        <div class="row">
            <div class="col-md-5">
                <table class="table-file">

                    <tr>
                        <td class="text-label">Harga</td>
                        <td>:</td>
                        <td id="Td1" runat="server">
                            <asp:TextBox CssClass="form-control" runat="server" ID="harga" /></td>
                    </tr>
                </table>
            </div>
        </div>
        <div class="row">
            <div class="col-md-5">
                <table class="table-file">

                    <tr>
                        <td>Tanggal</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="tgl" runat="server" CssClass="form-control text-center" Width="110" />
                            <script>
                                $(document).ready(function () { $('#content_tgl').datepicker({ format: 'dd-mm-yyyy' }); });
                            </script>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="tgl_RF" runat="server" />
                            <span id="tgl_ERR" runat="server" class="text-validator" visible="false">Format</span>
                        </td>
                    </tr>

                </table>
            </div>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            stone
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="400">Stone</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70" CssClass="text-center">P</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70" CssClass="text-center">L</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70" CssClass="text-center">T</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass="text-right">Carat / Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass="text-center">T. Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120" CssClass="text-right">T. Carat</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            Price
        </div>
        <table class="table-file">
            <tr>
                <td class="text-label text-lg">Price Estimation</td>
                <td>:</td>
                <td class="fixed-100 text-right text-lg text-warning" id="price" runat="server" />
                <td class="text-silver text-lg"><small>IDR</small></td>
            </tr>

        </table>
        <hr />

        <div class="button-left">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-check"></i>
                Finish
            </button>
        </div>
    </form>
</asp:Content>

