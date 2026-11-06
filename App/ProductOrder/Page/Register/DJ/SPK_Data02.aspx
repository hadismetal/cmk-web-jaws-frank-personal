<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SPK_Data02.aspx.cs" Inherits="Page_Register_DJ_Default" %>

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
            <div class="col-xs-12 col-sm-4">
                <div class="form-group">
                    <label for="content_setitem">
                        Status Barang :
                    </label>
                    <div class="radio">
                        <label>
                            <input type="radio" name="optionsRadios1" runat="server" id="rdo1_pe" value="PESANAN" />
                            PESANAN
                        </label>
                    </div>
                    <div class="radio">
                        <label>
                            <input type="radio" name="optionsRadios1" runat="server" id="rdo1_pr" value="PROYEK" />
                            PROYEK
                        </label>
                    </div>
                    <div class="radio">
                        <label>
                            <input type="radio" name="optionsRadios1" runat="server" id="rdo1_stt" value="STOCK TOKO" />
                            STOCK TOKO
                        </label>
                    </div>
                    <div class="radio">
                        <label>
                            <input type="radio" name="optionsRadios1" runat="server" id="rdo1_st" value="STOCK" />
                            STOCK
                        </label>
                    </div>

                </div>
            </div>
            <div class="col-xs-12 col-sm-8">

                <div class="form-group">
                    <div class="well">
                        <label>
                            Keterangan Status Barang:
                        </label>
                        <asp:TextBox ID="ketstatus" runat="server" CssClass="form-control" MaxLength="30" />
                        <i class="text-muted pull-right">Max. 30 karakter</i>
                        <br />
                    </div>
                </div>

            </div>
        </div>
        <div>
            <asp:Literal runat="server" ID="lit_errormsg" />
        </div>

        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
        </div>
    </form>
</asp:Content>

