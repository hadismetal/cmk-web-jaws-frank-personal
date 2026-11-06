<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SPK_DataCustomer.aspx.cs" Inherits="Page_Register_DataCustomer" %>

<%@ Register Src="MenuSPK.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Design
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
    <asp:Literal ID="idbrand" runat="server" Visible="false" />
    <hr />
    <form runat="server" class="form-inline">
        <div id="customerlist" runat="server">
             <div class="form-group">
                <div class="text-muted">
                   Customer (Nama / Kode customer / kode member )  :
                </div>
                <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
            </div>

            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                        <i class="icon-search"></i>
                        Display
                    </button>
                    <a id="cancel" runat="server" class="btn btn-default" href="SPK_Data01.aspx">
                        <i class="icon-remove"></i>
                        Cancel
                    </a>
                </div>
            </div>
           <div>
                <br />
                <asp:Literal runat="server" ID="lit_countcustomer" />
            </div>
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="90" />
                        <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="250">Customer</asp:TableHeaderCell>
                        <%--<asp:TableHeaderCell Width="320">No. PO</asp:TableHeaderCell>--%>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
            <hr />
        </div>
    </form>
</asp:Content>

