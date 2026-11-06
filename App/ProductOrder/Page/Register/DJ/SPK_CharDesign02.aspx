<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SPK_CharDesign02.aspx.cs" Inherits="Page_Register_SPK_CharDesign02" %>

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
                <div class="form-group width-300">
                    <label>
                        Naming Product :
                    </label>
                    <asp:TextBox ID="naming" runat="server" CssClass="form-control" MaxLength="50" />
                </div>
                <div class="form-group cb-single">
                    <label>
                        Keterangan :
                    </label>
                    <br />
                    <asp:CheckBox ID="setitem" runat="server" />
                    <label for="content_setitem">
                        <strong>Set Item</strong>
                    </label>
                    <br />
                    <asp:CheckBox ID="special" runat="server" />
                    <label for="content_special">
                        <strong>Special Design</strong>
                    </label>
                </div>
            </div>
            <div class="col-xs-12 col-sm-3">
                <div class="form-group width-300">
                    <div class="well">
                        <label>
                            Grafir 1
                        </label>
                        <asp:TextBox ID="grafir" runat="server" CssClass="form-control" MaxLength="38" />
                        <i class="text-muted pull-right">Max. 38 karakter</i>
                        <br />
                    </div>
                </div>
                <div class="form-group width-300">
                    <div class="well">
                        <label>
                            Grafir 2
                        </label>
                        <asp:TextBox ID="grafir2" runat="server" CssClass="form-control" MaxLength="38" />
                        <i class="text-muted pull-right">Max. 38 karakter</i>
                        <br />
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-3">
                <div class="form-group cb-single">
                    <label for="content_setitem">
                        Font Type :
                    </label>
                    <div class="radio">
                        <label>
                            <input type="radio" name="optionsRadios1" runat="server" id="rdo1_ic" value="Italic Cursive" />
                            <i>Italic Cursive</i>
                        </label>
                    </div>
                    <div class="radio">
                        <label>
                            <input type="radio" name="optionsRadios1" runat="server" id="rdo1_bt" value="Block Title" />
                            Block Title
                        </label>
                    </div>
                    <div class="radio">
                        <label>
                            <input type="radio" name="optionsRadios1" runat="server" id="rdo1_bc" value="BLOCK CAPITAL" />
                            BLOCK CAPITAL
                        </label>
                    </div>
                    <div class="radio">
                        <label>
                            <input type="radio" name="optionsRadios1" runat="server" id="rdo1_mc" value="MONOTYPE" />
                            MONOTYPE
                        </label>
                    </div>
                    <div class="radio">
                        <label>
                            <input type="radio" name="optionsRadios1" runat="server" id="rdo1_vv" value="VIVALDI" />
                            VIVALDI
                        </label>
                    </div>
                    <div class="radio">
                        <label>
                            <input type="radio" name="optionsRadios1" runat="server" id="rdo1_bs" value="BRUSH SCRIPT" />
                            BRUSH SCRIPT
                        </label>
                    </div>
                    <div class="radio">
                        <label>
                            <input type="radio" name="optionsRadios1" runat="server" id="rdo1_ale" value="ALLEGRO" />
                            ALLEGRO
                        </label>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-3">
                <div class="form-group cb-multi">
                    <p>
                        Finishing :
                    </p>
                    <p>
                        <a href="javascript:checkCtrl('content_cb','true')">Check</a>&nbsp;
						<a href="javascript:checkCtrl('content_cb','false')">Uncheck</a>
                    </p>
                    <asp:CheckBoxList ID="cb" runat="server" />
                </div>
            </div>

        </div>
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
        </div>
    </form>
</asp:Content>

