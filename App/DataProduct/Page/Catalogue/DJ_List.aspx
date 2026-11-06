<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_List.aspx.cs" Inherits="Page_DJ" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="h1_st" runat="server">
    Catalogue
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
        .product_catalog {
            width: 180px;
            float: left;
            padding: 0px 0px 20px 0px;
            text-align: center;
            height: 250px;
            overflow: hidden;
        }

        .proto {
            text-align: center;
            padding: 10px;
        }

        .qty {
            font-weight: bold;
            text-align: center;
            padding: 0px 10px 10px 10px;
        }

        a.aproduct {
            text-decoration: none;
            color:black;
        }
        
        a.aproduct:hover {
            text-decoration: none;
            color:blue;
        }

    </style>
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="row">
                <div class="col-xs-12 col-sm-3 col-md-2">
                    <div class="form-group">
                        <div class="text-muted">
                            Search (PLU / Kode Karet) :
                        </div>
                        <asp:TextBox ID="kw" runat="server" CssClass="form-control text-center" placeholder="Input Keyword..." />
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Price (From) :
                        </div>
                        <asp:TextBox ID="dari" runat="server" CssClass="form-control text-center" placeholder="..." onkeypress="return functionx(event)" />
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Price (Up To) :
                        </div>
                        <asp:TextBox ID="sampai" runat="server" CssClass="form-control text-center" placeholder="..." onkeypress="return functionx(event)" />
                    </div>
                    <div class="form-group">
                        <div class="hidden-xs">&nbsp;</div>
                        <div>
                            <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                                <i class="icon-search"></i>
                                Display
                            </button>
                        </div>
                        <br />
                    </div>
                    <hr />
                    <div class="form-group">
                        <div class="text-muted">
                            Stone Brand :
                        </div>
                        <asp:DropDownList ID="stonebrand" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">...</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    
                    <div class="form-group">
                        <div class="text-muted">
                            Stock Status
                        </div>
                        <asp:DropDownList ID="stockstatus" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">...</asp:ListItem>
                            <asp:ListItem Value="1">Ready to sell only</asp:ListItem>
                            <asp:ListItem Value="2">Ready to sell only (with price filter)</asp:ListItem>
                            <asp:ListItem Value="3">Out of Stock</asp:ListItem>
                            <asp:ListItem Value="4">Out of Stock (with price filter)</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Basic / Non-Basic Item
                        </div>
                        <asp:DropDownList ID="statusbasic" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">...</asp:ListItem>
                            <asp:ListItem Value="1">Basic only</asp:ListItem>
                            <asp:ListItem Value="2">Non-Basic only</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Product Item :
                        </div>
                        <asp:DropDownList ID="productitem" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">...</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Product Category :
                        </div>
                        <asp:DropDownList ID="productcategory" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">...</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Product Level :
                        </div>
                        <asp:DropDownList ID="productlevel" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">...</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Stone Distribution :
                        </div>
                        <asp:DropDownList ID="stonedistribution" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">...</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Frame Material :
                        </div>
                        <asp:DropDownList ID="framematerial" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">...</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <div class="text-muted">
                            Frame Color :
                        </div>
                        <asp:DropDownList ID="framecolor" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">...</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    
                    <div class="form-group" style="visibility:hidden;">
                        <div class="text-muted">
                            Design Category :
                        </div>
                        <asp:DropDownList ID="designcategory" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">...</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group" style="visibility:hidden;">
                        <div class="text-muted">
                            Design Concept :
                        </div>
                        <asp:DropDownList ID="designconcept" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">...</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group" style="visibility:hidden;">
                        <div class="text-muted">
                            Design Process :
                        </div>
                        <asp:DropDownList ID="designprocess" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">...</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group" style="visibility:hidden;">
                        <div class="text-muted">
                            Target Age :
                        </div>
                        <asp:DropDownList ID="targetage" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">...</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group" style="visibility:hidden;">
                        <div class="text-muted">
                            Target Gender :
                        </div>
                        <asp:DropDownList ID="targetgender" runat="server" CssClass="form-control" Width="175">
                            <asp:ListItem Value="0">...</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    
                    <br /><br /><br /><br /><br />
                </div>
                <div class="col-xs-12 col-sm-9 col-md-10">
                    <asp:Literal ID="prod" runat="server" />

                    <div runat="server" id="div_paging" visible="false">
                        <asp:Panel ID="Panel1" DefaultButton="btn_paging" runat="server">  
                            <asp:Literal ID="paging_text_1" runat="server" />  
                            <asp:TextBox ID="txt_paging" runat="server" Style="text-align:center; width:50px;" />
                            <asp:Literal ID="paging_text_2" runat="server" />
                            <br /><asp:Button runat="server" ID="btn_paging" Style="visibility:hidden; width:0; height:0;"  OnClick="btn_paging_Click" />
                            <asp:TextBox ID="txt_totalpaging" runat="server" Style="visibility:hidden; width:0; height:0;" />
                        </asp:Panel>
                    </div>
                    
                </div>
                
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <asp:Literal ID="lit_test_message" runat="server" />

                </div>
            </div>

        </form>

<%--        <script>
            function gotoPage(e, page, url) {
                if (e.keyCode == 13) {
                    window.location.href = '?refreshPage=1' + url + "&page=" + page;
                }

            }

        </script>--%>
    </div>
</asp:Content>

