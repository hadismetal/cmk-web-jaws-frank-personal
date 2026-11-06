<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PaymentSummary.ascx.cs" Inherits="UC_PaymentSummary" %>
           
   <div class="panel panel-default">
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-xs-12">
                                <div class="file-subheader hidden-sm hidden-xs">
                                    Payment
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <hr />
                            </div>
                        </div>
                        <div class="row">
                                <div class="col-xs-12 col-sm-4">
                                <span class="text-muted">Sebelum Discount</span><br />
                                <span class="text-lg" id="summary_nilaiso" runat="server" />
                            </div>
                            <div class="col-xs-12 col-sm-4">
                                <span class="text-muted">Total Discount</span><br />
                                <span class="text-lg" id="summary_totaldiscount" runat="server" />
                            </div>
                            <div class="col-xs-12 col-sm-4">
                                <span class="text-muted">Total Harus Dibayar</span><br />
                                <span class="text-lg" style="color: black; font-weight: bold;" id="summary_totalharusdibayar" runat="server" />
                            </div>
                        </div>
                        <div class="row" runat="server" id="div_row_tradein">
                            <div class="col-xs-12">
                                <hr />
                            </div>
                            <div class="col-xs-12 col-sm-4">
                                <span class="text-muted">Sebelum TradeIn</span><br />
                                <span class="text-lg" id="summary_sebelum_tradein" runat="server" />
                            </div>
                            <div class="col-xs-12 col-sm-4">
                                <span class="text-muted">Nilai TradeIn</span><br />
                                <span class="text-lg" id="summary_resell_tradein" runat="server" />
                            </div>
                            <div class="col-xs-12 col-sm-4">
                                <span class="text-muted">Sisa Harus Dibayar</span><br />
                                <span class="text-lg" style="color: blue; font-weight: bold;" id="summary_totalharusdibayar_setelahtradein" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <hr />
                            </div>
                        </div>
                        <div class="row">

                            <div class="col-xs-12 col-sm-4">
                                <span class="text-muted">Total Terbayar</span><br />
                                <span class="text-lg" style="color: green; font-weight: bold;" id="summary_totalpembayaran" runat="server" />
                            </div>
                            <div class="col-xs-12 col-sm-4">
                                <span class="text-muted" >Status Pembayaran</span><br />
                                <span class="text-lg"  fstyle="font-weight: bold;" id="jmlh_status_pembayaran" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <hr />
                            </div>
                        </div>
                    </div>
                </div>
           
