<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SPK_Form.aspx.cs" Inherits="Page_Register_DJ_Form" %>

<%@ Register Src="MenuSPK.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="HeaderSPK.ascx" TagName="Header" TagPrefix="uc2" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc2:Header ID="header" runat="server" />
    <uc1:Menu ID="menu" runat="server" />
    
    <form runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="form-group">
                    <label>
                        SPK Brand :
                    </label>
                    <asp:DropDownList ID="spkbrand" runat="server" CssClass="form-control" Width="175">
                    </asp:DropDownList>
                </div>
                <div class="form-group width-400">
                    <label>
                        Keterangan :
                    </label>
                    <asp:TextBox ID="keterangan" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                </div>
                
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-xs-12" style="padding-bottom:15px;">
                        <span>Tanggal Jatuh Tempo</span>
                    </div>
                    <div class="col-xs-6 col-sm-6 col-md-6" style="padding-bottom:15px;">
                        <label>
                            Rangka
                        </label>
                        <asp:TextBox ID="tglrangka" runat="server" CssClass="form-control text-center" Width="120" />
                        <script>
                            $(document).ready(function () { $('#<%=tglrangka.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                        </script>
                    </div>
                    <div class="col-xs-6 col-sm-6 col-md-6" style="padding-bottom:15px;">
                        <label>
                            Setting
                        </label>
                        <asp:TextBox ID="tglsetting" runat="server" CssClass="form-control text-center" Width="120" />
                        <script>
                            $(document).ready(function () { $('#<%=tglsetting.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                        </script>
                    </div>
                    <div class="col-xs-6 col-sm-6 col-md-6" style="padding-bottom:15px;">
                        <label>
                            BRJ
                        </label>
                        <asp:TextBox ID="tgldj" runat="server" CssClass="form-control text-center" Width="120" />
                        <script>
                            $(document).ready(function () { $('#<%=tgldj.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                        </script>
                    </div>
                    <div class="col-xs-6 col-sm-6 col-md-6" style="padding-bottom:15px;">
                        <label>
                            Konsumen
                        </label>
                        <asp:TextBox ID="tglkonsumen" runat="server" CssClass="form-control text-center" Width="120" />
                            <script>
                                $(document).ready(function () { $('#<%=tglkonsumen.ClientID%>').datepicker({ format: 'dd-mm-yyyy' }); });
                            </script>
                    </div>
                </div>
                
            </div>
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div class="row">
                    <div class="col-xs-12 col-sm-12 col-xs-12">
                        <div class="row">
                            <div class="col-xs-6 col-sm-6 col-md-6">
                                <label>
                                    No. Invoice
                                </label>
                                <asp:TextBox ID="noinvoice" runat="server" CssClass="form-control text-center" Width="200" />
                            </div>
                            <div class="col-xs-6 col-sm-6 col-md-6">
                                <label>
                                    No. Titipan
                                </label>
                                <asp:TextBox ID="notitipan" runat="server" CssClass="form-control text-center" Width="200" />
                            </div>
                        </div>
                        
                        
                    </div>
                    <div class="col-xs-12 col-sm-12 col-xs-12">
                        <table class="table-form">
                    
                            <tr>
                                <td>
                                    <div class="form-group">
                                        <label>
                                            Kode Karet Lama <span class="glyphicon glyphicon-asterisk text-red" aria-hidden="true"></span>
                                        </label>
                                        <asp:TextBox ID="kodekaretlama" runat="server" CssClass="form-control text-center text-bold" Width="150" />
                                    </div>
                                
                                </td>
                                <td></td>
                                <td>
                                    <div class="form-group">
                                        <label>
                                            Kode Repeat
                                        </label>
                                        <asp:TextBox ID="koderepeatlama" runat="server" CssClass="form-control text-center text-bold" Width="100" />
                                    </div>
                                
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div class="col-xs-12 col-sm-12 col-xs-12">
                        <div class="form-group">
                            <label>
                                Qty
                            </label>
                            <asp:TextBox ID="jlhcopy" Text="1" runat="server" CssClass="form-control text-center" Width="80" />
                            <br />
                            <asp:RequiredFieldValidator ID="jlhcopy_RF" runat="server" />
                            <span id="jlhcopy_ERR" runat="server" class="text-validator" visible="false">Format</span>
                        </div>
                    </div>
                    
                </div>
            </div>
            
        </div>
        <div>
            <asp:Literal runat="server" ID="lit_msg_err" />
        </div>
        <div class="button-inline" style="margin-top:0;">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
            <a href="DJ.aspx" class="btn btn-default">
                <i class="icon-remove"></i>
                Cancel
            </a>
        </div>
    </form>
</asp:Content>
