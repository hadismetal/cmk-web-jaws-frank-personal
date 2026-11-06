<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LD_List.aspx.cs" Inherits="Page_LD" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Loose LD Stock Return
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="form-group">
                <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="Input PLU" />
            </div>
            <div class="form-group">
                <div>
                    <button id="save" runat="server" class="btn btn-primary" onserverclick="Add">
                        <i class="icon-search"></i>
                        Add
                    </button>
                </div>
            </div>
            <div class="form-group">
                <span id="err" class="text-danger" runat="server" />
            </div>

            <div class="table-responsive">
                <table class="table-form" cssclass="table-list text-sm" height="200">
                    <tr>
                        <td>Product Picture</td>
                        <td>:</td>
                        <td>
                            <asp:Image ID="imgpicture" runat="server" CssClass="img-thumbnail" Width="200" />
                        </td>
                    </tr>
                </table>
            </div>

            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="120"></asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">No. PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="120">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Butir</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Carat</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>

            <div class="form-group">
                <div>
                    <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Finish">
                        <i class="icon-search"></i>
                        Finish
                    </button>
                </div>
            </div>
        </form>
    </div>
</asp:Content>

