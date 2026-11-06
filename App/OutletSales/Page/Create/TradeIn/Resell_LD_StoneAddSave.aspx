<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Resell_LD_StoneAddSave.aspx.cs" Inherits="Page_Register_DJ_StoneAddSave" %>

<%@ Register Src="MenuDJ.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Diamond Jewelry
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <p>
        <span class="text-muted">Nomor :</span><br />
        <span class="text-lg" id="header_nomor" runat="server" />
    </p>
    <hr />
    <form id="Form1" runat="server">
        <table class="table-form">
            <tr>
                <td>Tanggal Invoice Lama</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tgl" runat="server" CssClass="form-control text-center" Width="110" />
                    <script>
                        $(document).ready(function () { $('#content_tgl').datepicker({ format: 'dd-mm-yyyy' }); });
                    </script>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="tgl_RF" runat="server" />
                    <span id="tgl_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
            </tr>
            <tr>
                <td colspan="2">PLU :</td>

                <td>
                    <div class="input-group">
                        <asp:TextBox ID="plu" runat="server" CssClass="form-control text-lg text-center" Width="275" />
                    </div>
                </td>
                <td>
                    <span id="err_plu" runat="server" />
                    <asp:RequiredFieldValidator ID="plu_RF" runat="server" />
                </td>
            </tr>
        </table>
        <table class="table-form">
            <tr>
                <td colspan="2">Harga Invoice Lama:</td>

                <td>
                    <div class="input-group">
                        <asp:TextBox ID="harga" runat="server" CssClass="form-control text-lg text-center" Width="275" Text="0" />
                    </div>
                </td>
                <td></td>
            </tr>
        </table>
        <div class="row">
            <div class="col-md-3">
                <p>
                    <span class="text-bold" id="stone_index" runat="server" />
                    <br />
                    <span class="text-lg text-warning" id="stone_nama" runat="server" />
                </p>
            </div>
            <div class="col-md-3">
                <p id="p_sizecontrol" runat="server" class="text-muted">
                    Size Control :<br />
                    <span id="sizecontrol" runat="server" class="text-lg text-success" />
                    <small class="text-silver">Carat</small>
                </p>
            </div>
        </div>
        <br />
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
                        <asp:TextBox ID="totalcarat" runat="server" CssClass="form-control text-lg text-right" Width="120" />
                        <span class="input-group-addon">Carat</span>
                    </div>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="totalcarat_RF" runat="server" />
                    <span id="totalcarat_ERR" runat="server" class="text-validator" visible="false">Format</span>
                    <span id="totalcarat_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
                </td>
            </tr>
        </table>
        <br />
        <table class="table-form">
            <tr>
                <td colspan="2">Panjang :</td>
                <td colspan="2">Lebar :</td>
                <td colspan="2">Tinggi :</td>
            </tr>
            <tr>
                <td>
                    <div class="input-group">
                        <asp:TextBox ID="dimensip" Text="0" runat="server" CssClass="form-control text-lg text-center" Width="125" />
                        <span class="input-group-addon">MM</span>
                    </div>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="dimensip_RF" runat="server" />
                    <span id="dimensip_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
                <td>
                    <div class="input-group">
                        <asp:TextBox ID="dimensil" Text="0" runat="server" CssClass="form-control text-lg text-center" Width="125" />
                        <span class="input-group-addon">MM</span>
                    </div>
                </td>
                <td>
                    <span id="dimensil_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
                <td>
                    <div class="input-group">
                        <asp:TextBox ID="dimensit" Text="0" runat="server" CssClass="form-control text-lg text-center" Width="125" />
                        <span class="input-group-addon">MM</span>
                    </div>
                </td>
                <td>
                    <span id="dimensit_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
            </tr>
        </table>
        <div class="form-group width-300" id="div_gia" runat="server">
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
            <a id="cancel" runat="server" class="btn btn-default" href="Resell_ItemAdd.aspx">
                <i class="icon-remove"></i>
                Cancel
            </a>
            <span id="caratbutir_ERR" runat="server" class="text-validator" visible="false">Carat / Butir : Tidak Sesuai</span>
        </div>
    </form>
</asp:Content>

