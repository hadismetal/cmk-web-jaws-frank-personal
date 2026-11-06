<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Souvenir_DJ.aspx.cs" Inherits="Page_Create_ProductAddr" %>

<%@ Register Src="MenuPromo.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Souvenir
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Create New
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <hr />
    <form runat="server">
        <div class="form-inline">
            <asp:Panel DefaultButton="btn_inputproduct">
                <div class="form-group width-300">
                    <div class="text-muted">
                        Product Code :
                    </div>
                    <asp:TextBox ID="inputproduct" runat="server" CssClass="form-control text-bold" placeholder="PLU / Kode Karet" MaxLength="20" />
                </div>
                <div class="form-group">
                    <div class="hidden-xs">&nbsp;</div>
                    <div>
                        <asp:Button ID="btn_inputproduct" OnClick="btn_inputproduct_Click" Text="Enter" runat="server" class="btn btn-primary">
                            <%--<i class="icon-arrow-right"></i>--%>
                        </asp:Button>
                        &nbsp;
                        <asp:Literal runat="server" ID="lit_Message" />
                    </div>
                </div>
            </asp:Panel>
        </div>
        <br />
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

        <hr />
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next
            </button>
            <button id="skip" runat="server" class="btn btn-success" onserverclick="skip_Click">
                <i class="icon-remove"></i>
                Skip
            </button>
        </div>
    </form>
</asp:Content>

