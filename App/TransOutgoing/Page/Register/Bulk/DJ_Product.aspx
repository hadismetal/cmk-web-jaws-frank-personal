<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_Product.aspx.cs" Inherits="DJ_Product" %>

<%@ Register Src="MenuDJ.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewelry
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <uc1:Menu ID="menu" runat="server" />
    <table class="table-file">
        <tr>
            <td class="text-label">Market Brand</td>
            <td>:</td>
            <td id="brand" runat="server" class="text-bold" />
        </tr>
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
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30" />
                    <asp:TableHeaderCell Width="110">Image</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="185">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">T. Butir</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Carat</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Harga Jual</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>

            <div class="form-inline">
                <asp:CheckBoxList runat="server" ID="cbl_Product" />
            </div>
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

