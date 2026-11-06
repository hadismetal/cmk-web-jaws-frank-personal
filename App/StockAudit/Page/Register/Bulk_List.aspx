<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Bulk_List.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Stock Audit
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="text-muted">
                Cabang :
            </div>
            <div class="form-group">
                <asp:DropDownList ID="cabang" runat="server" CssClass="form-control" Width="175">
                    <asp:ListItem Value="0">Cabang :</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div class="form-group">
                <div>
                    <button id="save" runat="server" class="btn btn-primary" onserverclick="Next">
                        <i class="icon-search"></i>
                        Next
                    </button>
                </div>
            </div>
        </form>
    </div>
</asp:Content>

