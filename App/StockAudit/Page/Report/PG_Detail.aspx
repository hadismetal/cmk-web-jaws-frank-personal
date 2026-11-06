<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PG_Detail.aspx.cs" Inherits="Page_Register_PG_Submit" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Plain Gold
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
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
                <tr>
                    <td class="text-label">Kode Supplier</td>
                    <td>:</td>
                    <td id="suppliernomor" runat="server" />
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
                    <td class="text-label">Product Level</td>
                    <td>:</td>
                    <td id="productlevel" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Model</td>
                    <td>:</td>
                    <td id="model" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Target Age</td>
                    <td>:</td>
                    <td id="kelompokusia" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Frame Color</td>
                    <td>:</td>
                    <td id="framecolor" runat="server" />
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
                    <td class="text-label">Netto Weight</td>
                    <td>:</td>
                    <td class="fixed-100 text-right" id="nettoweight" runat="server" />
                    <td class="text-silver"><small>Gram</small></td>
                </tr>
            </table>
            
        </div>
    </div>
    <hr />

    <div class="file-subheader hidden-sm hidden-xs">
        price
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

