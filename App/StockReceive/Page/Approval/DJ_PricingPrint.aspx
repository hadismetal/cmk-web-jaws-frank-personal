<%@ Page Language="C#" MasterPageFile="~/MasterPagePrint.master" AutoEventWireup="true" CodeFile="DJ_PricingPrint.aspx.cs" Inherits="Page_Approval_DJ_PricingSubmit" %>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div style="max-width: 700px">
        <h1>Diamond Jewelry</h1>

        <h2>Approval</h2>
        <p>
            <span class="text-muted">Nomor :</span><br />
            <span class="text-lg" id="header_nomor" runat="server" />
        </p>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            frame
        </div>
        <table class="table-file">
            <tr>
                <td class="text-label">Gold Rate</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150" id="goldrate" runat="server" />
                <td><small class="text-silver">USD</small></td>
                <td id="efektifgold" runat="server" />
            </tr>
            <tr>
                <td class="text-label">Netto Weight</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150" id="netto" runat="server" />
                <td><small class="text-silver">Gram</small></td>
            </tr>
            <tr>
                <td class="text-label">Kadar Tukaran</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150" id="kadar" runat="server" />
                <td><small class="text-silver">%</small></td>
            </tr>
            <tr>
                <td class="text-label">Total</td>
                <td>:</td>
                <td class="text-bold text-right fixed-150" id="totalproduct" runat="server" />
                <td><small class="text-silver">USD</small></td>
            </tr>
        </table>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            stone
        </div>
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-report" Font-Size="11px">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Stone</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60">Tgl.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="130" CssClass="text-right"><small>USD</small> Harga Satuan</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="60" CssClass="text-center">Total Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="70" CssClass="text-center">Total Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="100" CssClass="text-right"><small>USD</small> Harga Total</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            product summary
        </div>
        <div class="row" style="font-size: 14px">
            <div class="col-md-3" style="max-width: 180px">
                <p>
                    <span class="text-muted">Frame :</span><br />
                    <span class="text-lg" id="sum_frame" runat="server" style="font-size: 14px;" />
                    <small class="text-silver">USD</small>
                </p>
            </div>
            <div class="col-md-3" style="max-width: 180px">
                <p>
                    <span class="text-muted">Stone :</span><br />
                    <span class="text-lg" id="sum_stone" runat="server" style="font-size: 14px;" />
                    <small class="text-silver">USD</small>
                </p>
            </div>
            <div class="col-md-3" style="max-width: 180px">
                <p>
                    <span class="text-muted">Total :</span><br />
                    <span class="text-lg" id="sum_total" runat="server" style="font-size: 14px;" />
                    <small class="text-silver">USD</small>
                </p>
            </div>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            mounting fee
        </div>
        <div class="row">
            <div class="col-md-5">
                <table class="table-file" style="font-size: 14px;">
                    <tr>
                        <td class="text-label">Brand</td>
                        <td>:</td>
                        <td id="brand1" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Product Item</td>
                        <td>:</td>
                        <td id="item1" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Cons. Process</td>
                        <td>:</td>
                        <td id="cons1" runat="server" />
                    </tr>
                </table>
            </div>
            <br class="visible-sm visible-xs" />
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Biaya NO</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-sm" id="no1" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                    </tr>
                    <tr>
                        <td class="text-label">Biaya RO</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-sm" id="ro1" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                    </tr>
                    <tr>
                        <td class="text-label">Total Biaya</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-bold text-sm" id="total1" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                        <td id="efektifmounting" runat="server" />
                    </tr>
                </table>
            </div>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            finishing fee
        </div>
        <div class="table-responsive">
            <asp:Table ID="tbff" runat="server" CssClass="table-report" Style="max-width: 600px">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="250">Finishing</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="90">Tgl.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="180" CssClass="text-right"><small>IDR</small> Biaya</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            fee summary
        </div>
        <div class="row">
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Mounting</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-sm" id="fee10" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                        <td id="fee11" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Setting</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-sm" id="fee20" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                        <td id="fee21" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Finishing</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-sm" id="fee30" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                    </tr>
                    <tr>
                        <td class="text-label">Ongkos Lain-lain</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-sm" id="fee31" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                    </tr>
                    <tr>
                        <td class="text-label">Total Biaya</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-bold text-sm" id="feerupiah" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                    </tr>
                </table>
            </div>
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">USD Rate</td>
                        <td>:</td>
                        <td class="fixed-180 text-right" id="feerate" runat="server" />
                        <td class="text-silver"></td>
                        <td id="efektifrate" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Total Biaya</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-bold" id="feetotal" runat="server" />
                        <td class="text-silver"><small>USD</small></td>
                    </tr>
                </table>
            </div>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            mark-up
        </div>
        <div class="row">
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Category</td>
                        <td>:</td>
                        <td id="cat1" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Level</td>
                        <td>:</td>
                        <td id="lvl1" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Stone Dist.</td>
                        <td>:</td>
                        <td id="dist1" runat="server" />
                    </tr>
                </table>
                <br />
                <table class="table-file">
                    <tr>
                        <td class="text-label">Base Price (USD)</td>
                        <td>:</td>
                        <td class="fixed-180 text-right" id="modal" runat="server" />
                        <td class="text-silver"><small>USD</small></td>
                    </tr>
                    <%--tr>
                        <td class="text-label">USD Rate</td>
                        <td>:</td>
                        <td class="fixed-180 text-right" id="modalrate" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                        <td id="murate" runat="server" />
                    </--%>
                    <tr>
                        <td class="text-label">Rate Jual</td>
                        <td>:</td>
                        <td class="fixed-180 text-right" id="modalrate" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                        <td id="murate" runat="server" />
                    </tr>
                    <%--tr>
                        <td class="text-label">Base Price (IDR)</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-bold" id="modalrupiah" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                    </--%>
                    <tr id="unitbox" runat="server">
                        <td class="text-label">Unit Price (IDR)</td>
                        <td>:</td>
                        <td class="fixed-180 text-right text-bold" id="unitprice" runat="server" />
                        <td class="text-silver"><small>IDR</small></td>
                    </tr>
                </table>
            </div>
            <br class="visible-sm visible-xs" />
            <div class="col-md-5">
                <p>
                    <span class="text-xl" id="mu_persen" runat="server" />
                    <small class="text-silver">%</small>
                </p>
                <br />
                <p>
                    <span class="text-muted">Final Price (USD) :</span><br />
                    <span class="text-lg" id="hargausd" runat="server" />
                    <small class="text-silver">USD</small><br />

                </p>
                <br />
                <p>
                    <span class="text-muted">Final Price (IDR) :</span><br />
                    <span class="text-lg text-orange" id="hargaidr" runat="server" />
                    <small class="text-silver">IDR</small><br />
                </p>
            </div>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            segmen
        </div>
        <div class="row">
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Category</td>
                        <td>:</td>
                        <td id="cat2" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Level</td>
                        <td>:</td>
                        <td id="lvl2" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Stone Dist.</td>
                        <td>:</td>
                        <td id="dist2" runat="server" />
                    </tr>
                </table>
                <br />
                <table class="table-file">
                    <tr>
                        <td class="text-label">Segmen Sistem</td>
                        <td>:</td>
                        <td id="segmensistem" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Minimum</td>
                        <td>:</td>
                        <td id="minimum" runat="server" />
                        <td id="efektifsegmen" runat="server" />
                    </tr>
                </table>
                <br />
                <table class="table-file">
                    <tr>
                        <td class="text-label">Carat</td>
                        <td>:</td>
                        <td id="carat" runat="server" />
                    </tr>
                    <tr>
                        <td class="text-label">Harga (USD)</td>
                        <td>:</td>
                        <td id="hargasegmen" runat="server" />
                    </tr>
                </table>
            </div>
            <br class="visible-sm visible-xs" />
            <div class="col-md-5">
                <p>
                    <span class="text-xl" id="segmen" runat="server" />
                </p>
                <br />
            </div>
        </div>
    </div>
</asp:Content>

