<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="LD_Detail.aspx.cs" Inherits="Page_Catalogue_LD_Detail" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Loose Diamond
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Information Detail
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div class="row">
        <div class="col-xs-12 col-sm-2">
            <p>
                <span class="text-muted">Nomor :</span><br />
                <span class="text-lg" id="header_nomor" runat="server" />
            </p>
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
                    <td class="text-label">Total Butir</td>
                    <td>:</td>
                    <td id="totalbutir" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Total Carat</td>
                    <td>:</td>
                    <td id="totalcarat" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Carat Butir</td>
                    <td>:</td>
                    <td id="caratbutir" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">GIA</td>
                    <td>:</td>
                    <td id="gia" runat="server" />
                </tr>
            </table>
        </div>
        <br class="visible-sm visible-xs" />
        <div class="col-md-5">
            <table class="table-file">
                <tr>
                    <td class="text-label">Shape</td>
                    <td>:</td>
                    <td id="shape" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Size</td>
                    <td>:</td>
                    <td id="size" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Colour</td>
                    <td>:</td>
                    <td id="colour" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Clarity</td>
                    <td>:</td>
                    <td id="clarity" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Cutting</td>
                    <td>:</td>
                    <td id="cutting" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Brand</td>
                    <td>:</td>
                    <td id="brand" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Category</td>
                    <td>:</td>
                    <td id="category" runat="server" />
                </tr>
            </table>
        </div>
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