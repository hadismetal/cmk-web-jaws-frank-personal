<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="RepairInvoice_Print.aspx.cs" Inherits="Page_Register_Repair_Submit" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
        h2 { margin-top : 0px; }
        td { vertical-align: top; }
        .row { margin-left: 0px; margin-right: 0px; }
    </style>
    <div class="row">
        <div class="col-xs-12">
            <table border="0" cellpadding="0" cellspacing="0" style="width:100%;">
                <tr>
                    <td style="width:33%; text-align:left;">
                        <img src="/Images/logo_web.jpg" border="0" runat="server" id="img_logo_web" style="height:50px;" />
                    </td>
                    <td style="width:33%; text-align:center;">
                        <span style="font-size:30px;">&nbsp;Repair Invoice&nbsp;</span><br />
                        <span style="font-size:20px;"><asp:Literal runat="server" ID="lit_nomor_repair" /></span>
                    </td>
                    <td style="width:33%; text-align:right;">
                        <asp:Literal id="cabang" runat="server"></asp:Literal>
                    </td>
                </tr>
            </table>
            <hr />
        </div>
    </div>
    <div class="row">
        <div class="col-xs-6">
            <h3>Repair Information</h3>
            <table class="table-file">
                <tr>
                    <td class="text-label">Tanggal Masuk</td>
                    <td>:</td>
                    <td id="tgl" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Tanggal Selesai</td>
                    <td>:</td>
                    <td id="tglselesai" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Customer</td>
                    <td>:</td>
                    <td id="customer" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">JR</td>
                    <td>:</td>
                    <td id="jpjr" runat="server" />
                </tr>
            </table>
            <h3>Product Information</h3>
            <table class="table-file">
                <tr>
                    <td colspan="5">
                        <div style="float:left;">
                            <span class="text-muted">Original :</span><br />
                            <asp:Image ID="imgpicturebefore" runat="server" CssClass="img-thumbnail" Height="100" />
                        </div>
                        <div style="float:left; padding-left:10px;">
                            <span class="text-muted">Repair :</span><br />
                            <asp:Image ID="imgpicturerepair" runat="server" CssClass="img-thumbnail" Height="100" />
                        </div>
                        <div style="clear:both;"></div>
                    </td>
                </tr>
                <tr>
                    <td class="text-label">No. PLU</td>
                    <td>:</td>
                    <td colspan="2" id="noplu" runat="server" />
                    <td></td>
                </tr>
                <tr>
                    <td class="text-label">Item</td>
                    <td>:</td>
                    <td colspan="2" id="productitem" runat="server" />
                    <td></td>
                </tr>
                <tr>
                    <td class="text-label">Gross Weight</td>
                    <td>:</td>
                    <td class="text-right" style="width:110px;" id="td_grossweight" runat="server" />
                    <td class="text-silver"><small>Gram</small></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="text-label">Total Butir (+)</td>
                    <td>:</td>
                    <td class="text-right" id="totalbutir" runat="server" />
                    <td class="text-silver"><small>Butir</small></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="text-label">Diameter</td>
                    <td>:</td>
                    <td class="fixed-75 text-right" id="dimensid" runat="server" />
                    <td class="text-silver"><small>MM</small></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="text-label">Panjang</td>
                    <td>:</td>
                    <td class="fixed-75 text-right" id="dimensip" runat="server" />
                    <td class="text-silver"><small>MM</small></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="text-label">Lebar</td>
                    <td>:</td>
                    <td class="fixed-75 text-right" id="dimensil" runat="server" />
                    <td class="text-silver"><small>MM</small></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="text-label">Ring Size</td>
                    <td>:</td>
                    <td class="fixed-75 text-right" id="dimensir" runat="server" />
                    <td class="text-silver"><small></small></td>
                    <td></td>
                </tr>
                <tr>
                    <td class="text-label">Keterangan</td>
                    <td>:</td>
                    <td colspan="3" id="ket" style="border:solid 1px black; padding:3px;" runat="server" />
                </tr>
                
            </table>

        </div>
        <div class="col-xs-6">
            <div style="float:right;">
                <div style="padding-bottom:20px; width:100%;">
                    <div class="pull-right">
                    <table class="text-right" border="0" cellpadding="5" cellspacing="0" style="font-size:18px;">
                        <tr>
                            <td>Harga</td>
                            <td style="border:solid 1px black; width:100px;"><span id="span_harga" runat="server"></span></td>
                        </tr>
                        <tr>
                            <td>Discount</td>
                            <td style="border:solid 1px black; width:100px;"><span id="span_discount" runat="server"></span></td>
                        </tr>
                        <tr>
                            <td>Total Harga</td>
                            <td style="border:solid 1px black; width:100px;"><span id="span_totalharga" runat="server"></span></td>
                        </tr>
                    </table>
                    </div>
                    <div style="clear:both;"></div>
                </div>
                <div style="clear:both;"></div>
            </div>
            <div style="clear:both;"></div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <hr />
            <h3>Rincian Biaya</h3>
            <asp:Table ID="tb2" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="300">Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Biaya</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <h3>Material Batu</h3>
            <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="110" CssClass="text-right">Carat / Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass="text-center">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" CssClass="text-center">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="140" CssClass="text-center">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <h3>Material Emas</h3>
            <asp:Table ID="tb3" runat="server" CssClass="table-list text-sm">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="110" CssClass="text-right">Berat Emas</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass="text-center">Harga</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12">
            <br /><br /><br />
            <div style="float: right;">
                <%--<div style="width: 300px; text-align: center; padding: 30px; float: right;">
                    <u><b>
                        <asp:Literal ID="adminsales" runat="server"></asp:Literal></b></u><br />
                    Admin Sales<br />
                    <i>Staff Penjualan</i>
                </div>--%>

                <div style="width: 300px; text-align: center; padding: 30px; float: right;">
                    <u><b>
                        <asp:Literal ID="jp" runat="server"></asp:Literal></b></u><br />
                    Jewellery Representative
                </div>
                <%--<div style="width: 300px; text-align: center; padding: 30px; float: right;">
                    <u><b>
                        <asp:Literal ID="customer2" runat="server"></asp:Literal></b></u><br />
                    Customer /<i> Pelanggan </i>
                </div>--%>
            </div>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12" style="font-size:9px;">
            catatan:
            <ol>
                <li>Tanda terima reparasi tidak berlaku jika tidak ditandatangani oleh, JR dan berstempel.</li>
                <li>Pengambilan barang reparasi harus membawa tanda terima reparasi yang asli dan berstempel.</li>
            </ol>
        </div>
    </div>
</asp:Content>

