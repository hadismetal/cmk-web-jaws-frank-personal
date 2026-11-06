<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TinyTan.aspx.cs" Inherits="DJ_Product" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Karet TinyTan
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
                        Kode Karet :
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
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
        </div>
    </form>
</asp:Content>

