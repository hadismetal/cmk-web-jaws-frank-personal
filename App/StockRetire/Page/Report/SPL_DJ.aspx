<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SPL_DJ.aspx.cs" Inherits="Page_Summary" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>SPL</title>
    <link rel="stylesheet" href="SPL.css" />
</head>
<body>
    <div id="judul1">
        <div class="judul_kiri">
            <h2 style="margin-bottom: 0px;">SURAT PERINTAH LEBUR (SPL)</h2>
        </div>
    </div>
    <div class="clear"></div>
    <div class="sub_jdul">
        <table class="table_header">
            <tr style="font-size:small;">
                <td class="header">No</td>
                <td>:</td>
                <td style="width: 200px" id="no" runat="server">&nbsp</td>
                <td class="ttd">PM/Mg.Merch</td>
                <td class="space">&nbsp</td>
                <td class="ttd">GM SBU</td>
                <td class="space">&nbsp</td>
                <td class="ttd">COO</td>
                <td class="space">&nbsp</td>
                <td class="ttd">IC</td>
                <td class="space">&nbsp</td>
                <td rowspan="5" style="text-align: center; border-width: 2px; border-color: black; border-style: solid; width: 139px">
                    <asp:Image runat="server" ID="imgpicture" Style="width: 139px; max-height: 139px; border: none;" /></td>
            </tr>
            <tr style="font-size:small;">
                <td>Tanggal</td>
                <td>:</td>
                <td style="width: 200px" id="tgl" runat="server">&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
            </tr>
            <tr style="font-size:small;">
                <td>Divisi</td>
                <td>:</td>
                <td style="width: 200px" id="divisi" runat="server">&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
            </tr>
            <tr style="font-size:small;">
                <td>Kode</td>
                <td>:</td>
                <td style="width: 200px" id="kobar" runat="server">&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
            </tr>
              <tr style="font-size:small;">
                <td style="font-size:small;">PLU</td>
                <td>:</td>
                <td style="width: 200px; font-size:small;" id="kplu" runat="server">&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
            </tr>
             <tr style="font-size:small;">
                <td style="font-size:small;">Item</td>
                <td>:</td>
                <td style="width: 200px; font-size:small;" id="pitem" runat="server">&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
            </tr>
             <tr style="font-size:small;">
                <td >Alasan</td>
                <td>:</td>
                <td style="width: 200px; font-size:small;" id="alasan" runat="server">&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
                <td></td>
                <td>&nbsp</td>
            </tr>
            <tr style="font-size:small;">
                <td>Tgl. Setor BRJ</td>
                <td>:</td>
                <td style="width: 200px" id="tglbrj" runat="server">&nbsp</td>
                <td class="ttd">_________</td>
                <td>&nbsp</td>
                <td class="ttd">_________</td>
                <td>&nbsp</td>
                <td class="ttd">_________</td>
                <td>&nbsp</td>
                <td class="ttd">_________</td>
                <td>&nbsp</td>
            </tr>
        </table>
    </div>
    <div class="clear"></div>
    <div id="menu1">
        <table class="table_main">
            <tr class="tb">
                <td rowspan="6" style="vertical-align: top;">
                    <table class="table_header">
                        <tr>
                            <td style="width: 120px; font-size:small;">Berat Bruto (Gr)</td>
                            <td>:</td>
                            <td id="bruto" runat="server" style="font-size:small; text-align:left;">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="font-size:small;">Berat Netto (Gr)</td>
                            <td>:</td>
                            <td id="netto" runat="server" style="font-size:small; text-align:left;">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="font-size:small;">Berat Bongkaran</td>
                            <td>:</td>
                            <td id="bsb" runat="server" style="font-size:small; text-align:left;">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="font-size:small;">Susut Bongkar</td>
                            <td>:</td>
                            <td id="sb" runat="server" style="font-size:small; text-align:left;">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="font-size:small;">Susut Lebur</td>
                            <td>:</td>
                            <td id="sl" runat="server" style="font-size:small; text-align:left;">&nbsp;</td>
                        </tr>
                        <tr>
                            <td style="font-size:small;">Berat Akhir</td>
                            <td>:</td>
                            <td id="ba" runat="server" style="font-size:small; text-align:left;">&nbsp;</td>
                        </tr>
                        <tr>
                            <td colspan="3">
                                
                                <table style="width: 100%">
                                    <tr>
                                        <td colspan="2" style="font-size:small;">Ongkos Bongkar
                                            <br /><br /><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>Penyetor</td>
                                        <td>Penerima</td>

                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Literal runat="server" ID="penyetor" /></td>
                                        <td>(&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)</td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td colspan="3" class="subtitle" style="height: 30px;">Data Awal SK</td>
                <td colspan="2" class="subtitle" style="height: 30px;">Setor ke Stone</td>
                <td colspan="2" class="subtitle" style="height: 30px;">Setor Pecah</td>
                <td colspan="2" class="subtitle" style="height: 30px;">Selisih</td>
                <td colspan="2" class="subtitle" style="height: 30px;">Keterangan</td>
            </tr>
            <tr>
                <td colspan="3" style="width: 160px; vertical-align: top; height: 150px; font-size:small;">
                    <asp:Table ID="tb1" runat="server">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="100">Parcel</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100">Butir</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100">Karat</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                     <p style="vertical-align:text-bottom">
                    <asp:Table ID="Table1" runat="server" >
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="100"></asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100"></asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100"></asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                    </p>
                </td>
                <td colspan="2" style="width: 160px; vertical-align: top; font-size:small;">
                    <asp:Table ID="tb2" runat="server">
                        <asp:TableHeaderRow>
                            <%--<asp:TableHeaderCell Width="100">Parcel</asp:TableHeaderCell>--%>
                            <asp:TableHeaderCell Width="100">Butir</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100">Karat</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                     <p style="vertical-align:text-bottom">
                    <asp:Table ID="Table2" runat="server" >
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="100"></asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100"></asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                    </p>
                </td>
                <td colspan="2" style="width: 160px; vertical-align: top; font-size:small;">
                    <asp:Table ID="tb3" runat="server">
                        <asp:TableHeaderRow>
                            <%--<asp:TableHeaderCell Width="100">Parcel</asp:TableHeaderCell>--%>
                            <asp:TableHeaderCell Width="100">Butir</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100">Karat</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                     <p style="vertical-align:text-bottom">
                    <asp:Table ID="Table3" runat="server" >
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="100"></asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100"></asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                    </p>
                </td>
                <td colspan="2" style="width: 160px; vertical-align: top; font-size:small;">
                    <asp:Table ID="tb4" runat="server">
                        <asp:TableHeaderRow>
                            <%--<asp:TableHeaderCell Width="100">Parcel</asp:TableHeaderCell>--%>
                            <asp:TableHeaderCell Width="100">Butir</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100">Karat</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                     <p style="vertical-align:text-bottom">
                    <asp:Table ID="Table4" runat="server" >
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="100"></asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100"></asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                    </p>
                </td>
                 <td colspan ="2" style="width: 160px; vertical-align: top; font-size:small;">
                     <asp:Table ID="tb5" runat="server">
                        <asp:TableHeaderRow>
                            <%--<asp:TableHeaderCell Width="100">Parcel</asp:TableHeaderCell>--%>
                            <asp:TableHeaderCell Width="100">GIA</asp:TableHeaderCell>
                             <asp:TableHeaderCell Width="100">Keterangan</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                </td>
            </tr>
            <%--tr class="subtitle">
                <td>Parcel</td>
                <td>Butir</td>
                <td>Karat</td>
                <td>Parcel</td>
                <td>Butir</td>
                <td>Karat</td>
                <td>Parcel</td>
                <td>Butir</td>
                <td>Karat</td>
                <td>Parcel</td>
                <td>Butir</td>
                <td>Karat</td>
            </%tr>
            <tr class="td">
                <td></td>
                <td style="text-align: right">10</td>
                <td></td>
                <td></td>
                <td style="text-align: right">8</td>
                <td></td>
                <td></td>
                <td style="text-align: right">1</td>
                <td></td>
                <td></td>
                <td style="text-align: right">1</td>
                <td></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
                <td></td>
            </tr--%>
            <tr>
                <td colspan="3" rowspan="6" style="vertical-align: super; border-top: 0px; text-align: center; height:80px;">Yang Mengerjakan</td>
                <td colspan="3" rowspan="6" style="vertical-align: super; border-top: 0px; text-align: center">Penerima SK stone</td>
                <td colspan="3" rowspan="6" style="vertical-align: super; border-top: 0px; text-align: center">Penerima SK B.C</td>
                <td colspan="3" rowspan="6" style="vertical-align: super; border-top: 0px; text-align: center">Penerima SK P</td>
            </tr>
        </table>
    </div>
    <div class="clear"></div>
    <div class="mnu_bwh"></div>
    <div class="clear"></div>
</body>
    <script>
        window.print();
    </script>
</html>
