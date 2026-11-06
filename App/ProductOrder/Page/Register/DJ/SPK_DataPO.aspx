<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SPK_DataPO.aspx.cs" Inherits="Page_Register_DJ_SPK_DataPO" %>

<%@ Register Src="MenuSPK.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Design
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">

    <form runat="server" class="form-inline">
        <div id="customerlist" runat="server">
             <div class="form-group">
                <div class="text-muted">
                  No. PO :
                </div>
                <asp:TextBox ID="kw_po" runat="server" CssClass="form-control" Width="150"  />
            </div>
            <div class="form-group">
                <div class="text-muted">
                  Kode Customer:
                </div>
                <asp:TextBox ID="kw_customer" runat="server" CssClass="form-control" Width="150"  />
            </div>
            <div class="form-group">
                <div class="text-muted">
                  Nama Store :
                </div>
                <asp:TextBox ID="kw_store" runat="server" CssClass="form-control" Width="250"  />
            </div>
             <div class="form-group">
                <div class="text-muted">
                  Nama Exhibition :
                </div>
                <asp:TextBox ID="kw_exh" runat="server" CssClass="form-control" Width="250"  />
            </div>
            <div>
                <br />
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
                        <asp:TableHeaderCell Width="50" />
                        <asp:TableHeaderCell Width="40" />
                        <asp:TableHeaderCell Width="200">
                            <span class='pull-center' style='text-align:center;'>NO. PO</span>
                            <div style='clear:both; border-bottom:solid 1px;'></div>
                            <span class='pull-center' style='text-align:center;'>Tgl PO</span>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="250">
                            <span class='pull-center' style='text-align:center;'>Kode Customer</span>
                            <div style='clear:both; border-bottom:solid 1px;'></div>
                            <span class='pull-center' style='text-align:center;'>Nama Customer</span>
                        </asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="250">Location</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
            <hr />
        </div>
    </form>
</asp:Content>
