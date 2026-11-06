<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"
    AutoEventWireup="true" CodeFile="Publish.aspx.cs" Inherits="Page_Export" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Export Label
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="h1_st" runat="server">
    Download
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server">
        <div class="row">
            <div class="col-xs-12">
                <div id="err" runat="server" visible="false"></div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-3 col-md-3">
                <!-- Download -->
                <div class="panel panel-info">
                    <div class="panel-heading">Download</div>
                    <div class="panel-body">
                        <p>
                            New Version :<br />
                            <a href="/JAWSZPL/ZPL-QRCode-250225.zip">JAWS ZPL With QRCode (2025-02-25)</a><br />
                        </p>
                        <p>
                            New Version :<br />
                            <a href="/JAWSZPL/JAWS_ZPL_2.3_QR.zip">JAWS ZPL With QRCode (v2.3.0.2)</a><br />
                        </p>
                        <p>
                            Beta Version :<br />
                            <a href="/JAWSZPL/JAWS_ZPL_2.3.zip">JAWS ZPL (v2.3.0.2)</a><br />
                        </p>
                        <p>
                            Stable Version :<br />
                            <a href="/JAWSZPL/JAWS_ZPL_2.1.zip">JAWS ZPL (v2.1.0.1)</a><br />
                        </p>
                        <p>
                            Other Version :<br />
                            <a href="/JAWSZPL/JAWS_ZPL_2.1SGD9800.zip">JAWS ZPL (SGD9800) (v2.1.0.0)</a><br />
                        </p>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-9 col-md-9">
                <!-- Download -->
                <div class="panel panel-default">
                    <div class="panel-heading">Changelog</div>
                    <div class="panel-body">
                        <div>
                            <p>Version 2.3 New Version:</p>
                            <ul>
                                <li>Penambahan QRCode untuk scan Kode PLU</li>
                            </ul>
                        </div>
                        <div>
                            <p>Version 2.3 :</p>
                            <ul>
                                <li>Penambahan Kalibrasi Posisi X dan Posisi Y untuk printer
                  Citizen
                                </li>
                                <li>Pemisahan kolom Qty</li>
                                <li>Revisi tombol Print menjadi menggunakan Icon Print</li>
                            </ul>
                        </div>
                        <div>
                            <p>Version 2.2 :</p>
                            <ul>
                                <li>Penambahan Opsi cetak Logo All Brand SBU</li>
                            </ul>
                        </div>
                        <div>
                            <p>Version 2.1 :</p>
                            <ul>
                                <li>Penyederhanaan fungsi cetak per jenis item (eliminasi tombol
                  DJ / PG / GJ / LD dari versi sebelumnya) menjadi 1 tombol
                                </li>
                                <li>Penambahan kolom informasi download export : jenis item, user,
                  tanggal dan jam, total qty
                                </li>
                                <li>Publish SGD version untuk event SBU Mondial rate 10000 IDR &
                  9800 IDR
                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </form>
</asp:Content>
