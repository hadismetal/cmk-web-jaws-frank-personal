<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_Detail.aspx.cs" Inherits="Page_Register_DJ_Submit" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Detail
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    
    <p>
        <span class="text-muted">Nomor :</span><br />
        <span class="text-lg" id="header_nomor" runat="server" />
    </p>
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
        picture
    </div>
    <asp:Image ID="imgpicture" runat="server" CssClass="img-thumbnail" Width="500" />
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
        transaction form
    </div>
    <div class="row">
        <div class="col-md-5">
            <table class="table-file">
                
                <tr>
                    <td class="text-label">Market Brand</td>
                    <td>:</td>
                    <td id="brand" runat="server" />
                </tr>
            </table>
        </div>
        <br class="visible-sm visible-xs" />
        <div class="col-md-5">
            <table class="table-file">
                <tr>
                    <td class="text-label">Keterangan</td>
                    <td>:</td>
                    <td id="keterangan" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Tanggal</td>
                    <td>:</td>
                    <td id="tgl" runat="server" />
                </tr>
            </table>
        </div>
    </div>
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
        transaction data
    </div>
    <div class="row">
        <div class="col-md-5">
            <table class="table-file">
                <tr>
                    <td class="text-label">Kode Product</td>
                    <td>:</td>
                    <td id="designnomor" runat="server" />
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
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
        stone
    </div>
    <div class="table-responsive">
        <asp:Table ID="tb" runat="server" CssClass="table-list">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="20">No.</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="80" CssClass="text-center">Shape</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="80" CssClass="text-center">Colour</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="80" CssClass="text-center">Clarity</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100" CssClass="text-right">Carat / Butir</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100" CssClass="text-center">Total Butir</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100" CssClass="text-right">Total Carat</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
        price estimation
    </div>
    <div class="table-responsive">

        <table class="table-file">
            <tr>
                <td class="text-label">Harga
                </td>
                <td>:</td>
                <td id="harga" runat="server" />
            </tr>

        </table>
    </div>
    <hr />

    
</asp:Content>

