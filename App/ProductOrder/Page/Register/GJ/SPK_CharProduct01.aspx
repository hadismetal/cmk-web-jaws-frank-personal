<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SPK_CharProduct01.aspx.cs" Inherits="Page_Register_SPK_CharProduct01" %>

<%@ Register Src="MenuSPK.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Gold Jewellery
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <p>
        <span class="text-muted">Nomor :</span><br />
        <span class="text-lg" id="header_nomor" runat="server" />
    </p>
    <hr />
    <form runat="server">
        <div class="row">
            <div class="col-md-6">
                <table class="table-form table-form-nomargin">
                    <tr>
                        <td class="fixed-150">Product Item</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="productitem" runat="server" CssClass="form-control" Width="275" />
                        </td>
                        <td>
                            <span id="productitem_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Product Category</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="productcategory" runat="server" CssClass="form-control" Width="275"
                                AutoPostBack="true" OnSelectedIndexChanged="fillDist" />
                        </td>
                        <td>
                            <span id="productcategory_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Product Level</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="productlevel" runat="server" CssClass="form-control" Width="275"
                                AutoPostBack="true" OnSelectedIndexChanged="fillDist" />
                        </td>
                        <td>
                            <span id="productlevel_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Stone Distribution</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="stonedist" runat="server" CssClass="form-control" Width="275" />
                        </td>
                        <td>
                            <span id="stonedist_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                        </td>
                    </tr>
                </table>
            </div>
            <br class="visible-sm visible-xs" />
            <div class="col-md-6">
                <table class="table-form table-form-nomargin">
                    <tr>
                        <td class="fixed-150">Frame Material</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="framematerial" runat="server" CssClass="form-control" Width="275" />
                        </td>
                        <td>
                            <span id="framematerial_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Frame Finishing</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="framefinishing" runat="server" CssClass="form-control" Width="275" />
                        </td>
                        <td>
                            <span id="framefinishing_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Frame Color</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="framecolor" runat="server" CssClass="form-control" Width="275" />
                        </td>
                        <td>
                            <span id="framecolor_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Construction Process</td>
                        <td>:</td>
                        <td>
                            <asp:DropDownList ID="processcons" runat="server" CssClass="form-control" Width="275" />
                        </td>
                        <td>
                            <span id="processcons_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <br />
        <div class="row">
            <div class="col-md-6">
                <table class="table-form table-form-nomargin">
                    <tr>
                        <td class="fixed-150">Kode Komponen / Poin</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="komponen" runat="server" CssClass="form-control text-center" Width="100" />
                            
                        </td>
                        <td><asp:TextBox ID="poin" runat="server" CssClass="form-control text-center" Width="110" />
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Kode Komponen 1</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="komponen1" runat="server" CssClass="form-control text-center" Width="110" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Kode Komponen 2</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="komponen2" runat="server" CssClass="form-control text-center" Width="110" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Kode Komponen 3</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="komponen3" runat="server" CssClass="form-control text-center" Width="110" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Kode Komponen 4</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="komponen4" runat="server" CssClass="form-control text-center" Width="110" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Kode Komponen 5</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="komponen5" runat="server" CssClass="form-control text-center" Width="110" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Kode Komponen 6</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="komponen6" runat="server" CssClass="form-control text-center" Width="110" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
            </div>
            <br class="visible-sm visible-xs" />
            <div class="col-md-6">
                <table class="table-form table-form-nomargin">
                    <tr>
                        <td class="fixed-150">Beban Susut</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="bebansusut" runat="server" CssClass="form-control text-center" Width="110" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Jumlah</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="pk1" runat="server" CssClass="form-control text-center" Width="110" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Jumlah</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="pk2" runat="server" CssClass="form-control text-center" Width="110" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Jumlah</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="pk3" runat="server" CssClass="form-control text-center" Width="110" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Jumlah</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="pk4" runat="server" CssClass="form-control text-center" Width="110" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Jumlah</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="pk5" runat="server" CssClass="form-control text-center" Width="110" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">Jumlah</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="pk6" runat="server" CssClass="form-control text-center" Width="110" />
                        </td>
                        <td>&nbsp;
                        </td>
                    </tr>
                </table>
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

