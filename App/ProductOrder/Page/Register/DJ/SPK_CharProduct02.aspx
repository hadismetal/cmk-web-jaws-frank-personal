<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SPK_CharProduct02.aspx.cs" Inherits="Page_Register_SPK_CharProduct02" %>

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

    <form id="form1" runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-4">
                <table class="table-form">
                    <tr>
                        <td>Gross Weight</td>
                        <td>:</td>
                        <td>
                            <div class="input-group">
                                <asp:TextBox ID="grossweight" runat="server" CssClass="form-control text-lg text-center" Width="120" />
                                <span class="input-group-addon">Gram</span>
                            </div>
                        </td>
                        <td>
                            <span id="grossweight_ERR" runat="server" class="text-validator" visible="false">Format</span>
                            <span id="grossweight_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
                            <asp:RequiredFieldValidator ID="grossweight_RF" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>Kadar Logam</td>
                        <td>:</td>
                        <td>
                            <div class="input-group">
                                <asp:TextBox ID="kadarlogam" runat="server" CssClass="form-control text-lg text-center" Width="110" />
                                <span class="input-group-addon">%</span>
                            </div>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="kadarlogam_RF" runat="server" />
                            <span id="kadarlogam_ERR" runat="server" class="text-validator" visible="false">Format</span>
                            <span id="kadarlogam_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
                        </td>
                    </tr>
                    <tr>
                        <td>Kadar Tukaran</td>
                        <td>:</td>
                        <td>
                            <div class="input-group">
                                <asp:TextBox ID="kadartukaran" runat="server" CssClass="form-control text-lg text-center" Width="110" />
                                <span class="input-group-addon">%</span>
                            </div>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="kadartukaran_RF" runat="server" />
                            <span id="kadartukaran_ERR" runat="server" class="text-validator" visible="false">Format</span>
                            <span id="kadartukaran_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="col-xs-12 col-sm-4">

                <table class="table-form">
                    <%--<tr>
                        <td>Diameter</td>
                        <td>:</td>
                        <td>
                            <div class="input-group">
                                <asp:TextBox ID="dimensid" Text="0" runat="server" CssClass="form-control text-lg text-center" Enabled="false" Width="125" />
                                
                            </div>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="dimensid_RF" runat="server" />
                            <span id="dimensid_ERR" runat="server" class="text-validator" visible="false">Format</span>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>Panjang</td>
                        <td>:</td>
                        <td>
                            <div class="input-group">
                                <asp:TextBox ID="dimensip" Text="0" runat="server" CssClass="form-control text-lg text-center" Width="125" />
                                <%--span class="input-group-addon">MM</--%>
                            </div>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="dimensip_RF" runat="server" />
                            <span id="dimensip_ERR" runat="server" class="text-validator" visible="false">Format</span>
                        </td>
                    </tr>
                    <tr>
                        <td>Lebar</td>
                        <td>:</td>
                        <td>
                            <div class="input-group">
                                <asp:TextBox ID="dimensil" Text="0" runat="server" CssClass="form-control text-lg text-center" Width="125" />
                                <%--span class="input-group-addon">MM</--%>
                            </div>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="dimensil_RF" runat="server" />
                            <span id="dimensil_ERR" runat="server" class="text-validator" visible="false">Format</span>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="col-xs-12 col-sm-4">

                <table class="table-form">
                    <tr>
                        <td>Ring Size 1</td>
                        <td>:</td>
                        <td>
                            <div class="input-group">
                                <asp:TextBox ID="dimensir" Text="0" runat="server" CssClass="form-control text-lg text-center" Width="125" />
                                <%--span class="input-group-addon">MM</--%>
                            </div>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="dimensir_RF" runat="server" />
                            <span id="dimensir_ERR" runat="server" class="text-validator" visible="false">Format</span>
                        </td>
                    </tr>
                    <tr>
                        <td>Ring Size 2</td>
                        <td>:</td>
                        <td>
                            <div class="input-group">
                                <asp:TextBox ID="dimensir2" Text="0" runat="server" CssClass="form-control text-lg text-center" Width="125" />
                                <%--span class="input-group-addon">MM</--%>
                            </div>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="dimensir2_RF" runat="server" />
                            <span id="dimensir2_ERR" runat="server" class="text-validator" visible="false">Format</span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:Button 
            ID="save"
            runat="server"
            CssClass="btn btn-primary"
            Text="Next Step"
            OnClick="save_Click" />
    </form>
    </form>
</asp:Content>

