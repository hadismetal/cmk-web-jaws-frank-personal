<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SPK_StoneUpdate.aspx.cs" Inherits="Page_Register_SPK_StoneUpdate" %>

<%@ Register Src="MenuSPK.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="HeaderSPK.ascx" TagName="Header" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <uc2:Header ID="header" runat="server" />
    <uc1:Menu ID="menu" runat="server" />

    <form runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-3">
                <p>
                    <span class="text-bold" id="stone_index" runat="server" />
                    <br />
                    <span class="text-lg text-warning" id="stone_nama" runat="server" />
                </p>
            </div>
            <div class="col-xs-12 col-sm-3">
                <p id="p_sizecontrol" runat="server" class="text-muted">
                    Size Control :<br />
                    <span id="sizecontrol" runat="server" class="text-lg text-success" />
                    <small class="text-silver">Carat</small>
                </p>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <table class="table-form">
                    <tr>
                        <td colspan="2">Total Butir :</td>
                        <td colspan="2">Total Carat :</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="totalbutir" runat="server" CssClass="form-control text-lg text-center" Width="100" />
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="totalbutir_RF" runat="server" />
                            <span id="totalbutir_ERR" runat="server" class="text-validator" visible="false">Format</span>
                            <span id="totalbutir_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
                        </td>
                        <td>
                            <div class="input-group">
                                <asp:TextBox ID="totalcarat" runat="server" CssClass="form-control text-lg text-center" Width="120" />
                                <span class="input-group-addon">Carat</span>
                            </div>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="totalcarat_RF" runat="server" />
                            <span id="totalcarat_ERR" runat="server" class="text-validator" visible="false">Format</span>
                            <span id="totalcarat_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
                        </td>
                        <td id="colterpasang" runat="server">
                            <asp:CheckBox ID="terpasang" Checked="true" runat="server" CssClass="text-lg text-danger text-right" Text="&nbsp;&nbsp;Terpasang" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">Panjang :</td>
                        <td colspan="2">Lebar :</td>
                        <td colspan="2">Tinggi :</td>
                    </tr>
                    <tr>
                        <td>
                            <div class="input-group">
                                <asp:TextBox ID="dimensip" runat="server" CssClass="form-control text-lg text-center" Width="125" />
                                <span class="input-group-addon">MM</span>
                            </div>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="dimensip_RF" runat="server" />
                            <span id="dimensip_ERR" runat="server" class="text-validator" visible="false">Format</span>
                        </td>
                        <td>
                            <div class="input-group">
                                <asp:TextBox ID="dimensil" runat="server" CssClass="form-control text-lg text-center" Width="125" />
                                <span class="input-group-addon">MM</span>
                            </div>
                        </td>
                        <td>
                            <span id="dimensil_ERR" runat="server" class="text-validator" visible="false">Format</span>
                        </td>
                        <td>
                            <div class="input-group">
                                <asp:TextBox ID="dimensit" runat="server" CssClass="form-control text-lg text-center" Width="125" />
                                <span class="input-group-addon">MM</span>
                            </div>
                        </td>
                        <td>
                            <span id="dimensit_ERR" runat="server" class="text-validator" visible="false">Format</span>
                        </td>
                    </tr>
                </table>
                <div class="form-group width-300" id="div_gia" runat="server" visible="false">
                    <br />
                    <label>
                        No. GIA :
                    </label>
                    <asp:TextBox ID="gia" runat="server" CssClass="form-control" MaxLength="50" />
                </div>
                <div class="button-inline">
                    <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                        <i class="icon-arrow-right"></i>
                        Next Step
                    </button>
                    <a id="cancel" runat="server" class="btn btn-default" href="SPK_Stone.aspx">
                        <i class="icon-remove"></i>
                        Cancel
                    </a>
                    <span id="caratbutir_ERR" runat="server" class="text-validator" visible="false">Carat / Butir : Tidak Sesuai</span>
                </div>
            </div>
        </div>

    </form>
</asp:Content>

