<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJLD_ProductDJ.aspx.cs" Inherits="DJ_Product" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery & Loose Diamond
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">


    <form runat="server">
        <div class="form-inline">
            <asp:Panel DefaultButton="btn_inputproduct">
                <div class="form-group width-300">
                    <div class="text-muted">
                        Diamond Jewellery PLU :
                    </div>
                    <asp:TextBox ID="inputproduct" runat="server" CssClass="form-control text-bold" MaxLength="20" />
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
        <div class="form-inline">
            <asp:CheckBoxList runat="server" ID="cbl_Product" RepeatDirection="Horizontal" RepeatColumns="10" />
            <style type="text/css">
                label {
                    display: block;
                    border: solid 1px black;
                    border-collapse: collapse;
                }
            </style>
        </div>
        <div class="form-inline">
            <asp:Panel DefaultButton="btn_inputproduct">
                <div class="form-group width-300">
                    <div class="text-muted">
                        Loose Diamond PLU :
                    </div>
                    <asp:TextBox ID="inputproduct_ld" runat="server" CssClass="form-control text-bold" MaxLength="20" />
                </div>
                <div class="form-group">
                    <div class="hidden-xs">&nbsp;</div>
                    <div>
                        <asp:Button ID="btn_inputproductld" OnClick="btn_inputproductld_Click" Text="Enter" runat="server" class="btn btn-primary">
                            <%--<i class="icon-arrow-right"></i>--%>
                        </asp:Button>
                        &nbsp;
                        <asp:Literal runat="server" ID="lit_Message_ld" />
                    </div>
                </div>
            </asp:Panel>
        </div>
        <br />
        <div class="form-inline">
            <asp:CheckBoxList runat="server" ID="cbl_Product_ld" />
            <style type="text/css">
                label {
                    display: block;
                    border: solid 1px black;
                    border-collapse: collapse;
                }
            </style>
        </div>
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
        </div>
    </form>
</asp:Content>

