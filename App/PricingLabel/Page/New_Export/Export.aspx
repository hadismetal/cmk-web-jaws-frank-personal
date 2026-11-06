<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Export.aspx.cs" Inherits="Page_Export" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Export Product
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server">
        <div class="row">
            <div class="col-xs-12"><div id="err" runat="server" visible="false"></div></div>
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 form-inline">
                <div class="form-group">
                    <div class="text-muted">
                        &nbsp;
                    </div>
                    <asp:DropDownList ID="cabang" runat="server" CssClass="form-control" Width="175">
                        <asp:ListItem Value="0">Cabang :</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group">
                    <div class="text-muted">
                        &nbsp;
                    </div>
                    <asp:DropDownList ID="clarity" runat="server" CssClass="form-control" Width="175">
                        <asp:ListItem Value="0">Clarity :</asp:ListItem>
                        <asp:ListItem Value="1">VS</asp:ListItem>
                        <asp:ListItem Value="2">VVS</asp:ListItem>
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
                <div class="text-muted">
                    No. PLU :
                </div>
                <div class="form-group">
                    <asp:TextBox ID="from" CssClass="form-control" runat="server" placeholder="Start" />
                </div>
                <div class="form-group">
                    <asp:TextBox ID="to" CssClass="form-control" runat="server" placeholder="End" />
                </div>

                <div class="clearfix">
                    <br class="hidden-xs" />
                </div>
                
            </div>
            <div class="col-xs-12 col-sm-6">
                <%--<div class="panel panel-default">
                    <div class="panel-body">
                        

                    </div>
                </div>--%>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <div class="form-group">
                    <div class="hidden-xs">&nbsp;</div>
                        <div>
                            <button id="btndj" runat="server" class="btn btn-primary" onserverclick="exportDJ">
                                <i class="icon-tags"></i>
                                Diamond Jewellery
                            </button>
                            &nbsp;
                            <button id="btndj2" runat="server" class="btn btn-warning" onserverclick="exportDJ2">
                                <i class="icon-tags"></i>
                                Diamond Jewellery (V2.1)
                            </button>
                            &nbsp;
                            <button id="btnpg" runat="server" class="btn btn-primary" onserverclick="exportPG">
                                <i class="icon-tags"></i>
                                Plain Gold
                            </button>
                            &nbsp;
                            <button id="btngj" runat="server" class="btn btn-primary" onserverclick="exportGJ">
                                <i class="icon-tags"></i>
                                Gold Jewellery
                            </button>
                            &nbsp;
                            <button id="btnld" runat="server" class="btn btn-primary" onserverclick="exportLD">
                                <i class="icon-tags"></i>
                                Loose Diamond
                            </button>
                    </div>
                </div>
            </div>
        </div>
        
    </form>

</asp:Content>

