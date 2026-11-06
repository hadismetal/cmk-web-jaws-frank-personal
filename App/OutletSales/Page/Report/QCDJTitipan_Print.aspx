<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="QCDJTitipan_Print.aspx.cs" Inherits="Print_QC" %>

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
                        <span style="font-size:30px;">&nbsp;QC DJ&nbsp;</span><br />
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
            <h3>QC Information</h3>
            <table class="table-file">
                <tr>
                    <td class="text-label">Tanggal</td>
                    <td>:</td>
                    <td id="tgl" runat="server" />
                </tr>
                
            </table>
            <h3>Product Information</h3>
            <table class="table-file">
                <tr>
                    <td colspan="4">
                        <asp:Literal runat="server" ID="lit_image_product" />
                    </td>
                </tr>
                <tr>
                    <td class="text-label">No. PLU</td>
                    <td>:</td>
                    <td colspan="2" id="noplu" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Item</td>
                    <td>:</td>
                    <td colspan="2" id="productitem" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Gross Weight</td>
                    <td>:</td>
                    <td class="text-right" id="grossweight" runat="server" />
                    <td class="text-silver"><small>Gram</small></td>
                </tr>
                <tr>
                    <td class="text-label">Total Butir</td>
                    <td>:</td>
                    <td class="text-right" id="totalbutir" runat="server" />
                    <td class="text-silver"><small>Butir</small></td>
                </tr>
                
            </table>

        </div>
        <div class="col-xs-6">
            <div style="float:right; width:100%;">
                <div style="padding:15px; border:solid 1px black;">
                <table class="text-left" border="0" cellpadding="2" cellspacing="2">
                    <tr>
                        <td class="text-muted">Petugas QC</td>
                        <td>&nbsp;:&nbsp;</td>
                        <td>
                            <asp:Literal ID="petugasqc" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Keterangan</td>
                        <td>&nbsp;:&nbsp;</td>
                        <td>
                            <asp:Literal ID="keterangan" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Blemish untuk > 0,3</td>
                        <td>&nbsp;:&nbsp;</td>
                        <td>
                            <asp:Literal ID="blemish" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Kecocokan Jumlah Butir</td>
                        <td>&nbsp;:&nbsp;</td>
                        <td>
                            <asp:Literal ID="jlhbutir" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Kecocokan Sertifikat</td>
                        <td>&nbsp;:&nbsp;</td>
                        <td>
                            <asp:Literal ID="sertifikat" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Grafir Total Carat - Non CZ</td>
                        <td>&nbsp;:&nbsp;</td>
                        <td>
                            <asp:Literal ID="noncz" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Grafir Total Carat - CZ</td>
                        <td>&nbsp;:&nbsp;</td>
                        <td>
                            <asp:Literal ID="cz" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Grafir Logo</td>
                        <td>&nbsp;:&nbsp;</td>
                        <td>
                            <asp:Literal ID="logo" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Grafir Kadar Emas</td>
                        <td>&nbsp;:&nbsp;</td>
                        <td>
                            <asp:Literal ID="kadaremas" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Frame Color</td>
                        <td>&nbsp;:&nbsp;</td>
                        <td>
                            <asp:Literal ID="framecolor" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Grafir Merk</td>
                        <td>&nbsp;:&nbsp;</td>
                        <td>
                            <asp:Literal ID="merk" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="text-muted">Fisik Barang Keseluruhan</td>
                        <td>&nbsp;:&nbsp;</td>
                        <td>
                            <asp:Literal ID="fisik" runat="server" />
                        </td>
                    </tr>

                </table>
                </div>
            </div>
            <div style="clear:both;"></div>
        </div>
    </div>
    <div class="row">
        <br /><br />
        <div class="col-xs-12">
            <asp:Table ID="tb" runat="server" CssClass="table-report">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Stone</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tgl.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-center">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-center">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="200" CssClass="text-center">Keterangan</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </div>
    
</asp:Content>

