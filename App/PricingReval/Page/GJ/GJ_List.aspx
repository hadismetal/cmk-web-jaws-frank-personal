<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="GJ_List.aspx.cs" Inherits="Page_GJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Gold Jewellery Revaluation
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
        td
        {
            vertical-align: top;
        }
    </style>
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <table cellspacing="0" cellpadding="0">
                <tr>
                    <td width="50%">
                        <div class="form-group">
                            <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="Input PLU" />
                        </div>
                        <div class="clearfix">
                            <br class="hidden-xs" />
                        </div>
                        <div class="form-group">
                            <div class="text-muted">
                                Characteristic Product :
                            </div>
                            <asp:DropDownList ID="productlevel" runat="server" CssClass="form-control" Width="175">
                                <asp:ListItem Value="0">Product Level</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <div class="text-muted">
                                &nbsp;
                            </div>
                            <asp:DropDownList ID="productcategory" runat="server" CssClass="form-control" Width="175">
                                <asp:ListItem Value="0">Product Category</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <div class="text-muted">
                                &nbsp;
                            </div>
                            <asp:DropDownList ID="productitem" runat="server" CssClass="form-control" Width="175">
                                <asp:ListItem Value="0">Product Item</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="clearfix">
                            <br class="hidden-xs" />
                        </div>
                        <div class="form-group">
                            <div class="text-muted">
                                Order By :
                            </div>
                            <asp:DropDownList ID="orderby" runat="server" CssClass="form-control" Width="175">
                                <asp:ListItem Value="1">Cabang</asp:ListItem>
                                <asp:ListItem Value="2">Product Level</asp:ListItem>
                                <asp:ListItem Value="3">Product Category</asp:ListItem>
                                <asp:ListItem Value="4">Product Item</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <div class="hidden-xs">&nbsp;</div>
                            <div>
                                <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                                    <i class="icon-search"></i>
                                    Display
                                </button>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="hidden-xs">&nbsp;</div>
                            <div>
                                <button id="Button1" runat="server" class="btn btn-primary" onserverclick="Execute">
                                    <i class="icon-search"></i>
                                    Execute
                                </button>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="hidden-xs">&nbsp;</div>
                            <div>
                                <button id="Button2" runat="server" class="btn btn-primary" onserverclick="Apply">
                                    <i class="icon-search"></i>
                                    Apply
                                </button>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="hidden-xs">&nbsp;</div>
                            <div>
                                <button id="Button3" runat="server" class="btn btn-primary" onserverclick="Export">
                                    <i class="icon-search"></i>
                                    Export
                                </button>
                            </div>
                        </div>
                    </td>
                    <td width="50%">
                        <div class="form-group">
                            <div class="text-muted">
                                Reval Option 1:
                            </div>
                            <asp:CheckBox ID="reval_M" runat="server" Text="&nbsp;M"></asp:CheckBox>
                            <asp:CheckBox ID="reval_R" runat="server" Text="&nbsp;R"></asp:CheckBox>
                        </div>
                        <div class="form-group">
                            <div class="text-muted">
                                Reval Option 2:
                            </div>
                            <asp:CheckBox ID="reval_G" runat="server" Text="&nbsp;Gold"></asp:CheckBox>
                            <asp:CheckBox ID="reval_MF" runat="server" Text="&nbsp;Mounting Fee"></asp:CheckBox>
                            <asp:CheckBox ID="reval_MU" runat="server" Text="&nbsp;Mark Up"></asp:CheckBox>
                            <asp:CheckBox ID="reval_Rate" runat="server" Text="&nbsp;Rate"></asp:CheckBox>
                        </div>
                    </td>
                </tr>
            </table>
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Mutasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">M</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">R</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Net Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Gross Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Segmen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Stone Distribution</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
    </div>
</asp:Content>

