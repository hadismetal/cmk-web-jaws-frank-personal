<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_CharProduct.aspx.cs" Inherits="Page_Register_DJ_CharProduct01" %>

<%@ Register Src="MenuDJ.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewelry
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
                    <tr>
                        <td colspan="2">Gross Weight :</td>
                    
                        <td>
                            <div class="input-group">
                                <asp:TextBox ID="grossweight" runat="server" CssClass="form-control text-lg text-center" Width="275" />
                                <span class="input-group-addon">Gram</span>
                            </div>
                        </td>
                        <td>
                            <span id="grossweight_ERR" runat="server" class="text-validator" visible="false">Format</span>
                            <span id="grossweight_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
                            <asp:RequiredFieldValidator ID="grossweight_RF" runat="server" />
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
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
        </div>
    </form>
</asp:Content>

