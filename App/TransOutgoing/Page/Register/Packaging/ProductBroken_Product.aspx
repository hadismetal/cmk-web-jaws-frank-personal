<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ProductBroken_Product.aspx.cs" Inherits="Product_Product" %>

<%@ Register Src="MenuProduct.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Packaging Broken
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <script>
        $(document).ready(function () {
            $("select").select2();
        });
    </script>
    <uc1:Menu ID="menu" runat="server" />
    <table class="table-file">
        <%--tr>
            <td class="text-label">Market Brand</td>
            <td>:</td>
            <td id="brand" runat="server" class="text-bold" />
        </--%>
        <tr>
            <td class="text-label">From
                <asp:Literal runat="server" ID="lit_type_from" /></td>
            <td>:</td>
            <td id="from" runat="server" class="text-bold" />
        </tr>
        <tr>
            <td class="text-label">To
                <asp:Literal runat="server" ID="lit_type_to" /></td>
            <td>:</td>
            <td id="to" runat="server" class="text-bold" />
        </tr>
    </table>
    <hr />
    <form runat="server">
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="upl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
        <div class="form-inline">
            <asp:Panel DefaultButton="btn_inputproduct">
                <div class="form-group width-300">
                    <div class="text-muted">
                        Product Code :
                    </div>
                    <%--<asp:TextBox ID="inputproduct" runat="server" CssClass="form-control text-bold" autocomplete="off" MaxLength="25" />--%>
                    <asp:DropDownList ID="ddl_ProductCode" runat="server" Width="250" />
                </div>
                <div class="form-group width-300">
                    <div class="text-muted">
                        Quantity :
                    </div>
                    <asp:TextBox ID="qty" runat="server" CssClass="form-control text-bold" MaxLength="20" />
                </div>
                <div class="form-group">
                    <div class="hidden-xs">&nbsp;</div>
                    <div>
                        <asp:Button ID="btn_inputproduct" OnClick="btn_inputproduct_Click" Text="Enter" runat="server" class="btn btn-primary">
                            <%--<i class="icon-arrow-right"></i>--%>
                        </asp:Button>
                        &nbsp;
                        <asp:Literal runat="server" ID="lit_Message" />
                        <asp:Label runat="server" ID="err" />
                    </div>
                </div>
            </asp:Panel>
        </div>
        <br />
        <div class="form-inline">
            <asp:CheckBoxList runat="server" ID="cbl_Product" RepeatDirection="Horizontal" RepeatColumns="10" />
            <style type="text/css">
                label
                {
                    display: block;
                    border: solid 1px black;
                    border-collapse: collapse;
                }
            </style>
        </div>
        <%--</ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_inputproduct" EventName="Click" />
        </Triggers>
        </asp:UpdatePanel>--%>
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
        </div>
    </form>
</asp:Content>

