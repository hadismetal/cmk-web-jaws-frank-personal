<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJInfoPLU.aspx.cs" Inherits="DJInfoPLU" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Promo Diamond Jewellery
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <style type="text/css">
        label {
            display: block;
        }
        input[type="checkbox"] {
            position: relative;
            top: 38px;
            left: 18px;

            -ms-transform: scale(2); /* IE */
            -moz-transform: scale(2); /* FF */
            -webkit-transform: scale(2); /* Safari and Chrome */
            -o-transform: scale(2); /* Opera */
        }
        .panel {
            margin-bottom: 0;
        }
        .alert {
            margin:0;
        }

        table tr td {
            vertical-align: top;
            text-align: left;
        }
        
        .text-green {
            color:green;
        }
    </style>
    <div>
        <div class="row">
            <div class="col-xs-12 col-sm-6">
                <asp:Literal runat="server" ID="lit_header1" />
            </div>
            <div class="col-xs-12 col-sm-6">
                <asp:Literal runat="server" ID="lit_header2" />
            </div>
        </div>
        
    </div>
    <hr />
    <form id="Form1" runat="server">
        <div class="form-inline">
            <asp:Panel DefaultButton="btn_inputproduct">
                
                <div class="form-group width-300">
                    <div class="text-muted">
                        PLU :
                    </div>
                    
                    <div class="input-group input-group-md">
					    <asp:TextBox ID="inputproduct" runat="server" CssClass="form-control text-bold" MaxLength="20" />
                        <span class="input-group-btn">
                            <button id="btn_inputproduct" onserverclick="btn_inputproduct_Click" runat="server" class="btn btn-primary">
				                <span class="glyphicon glyphicon-plus" aria-hidden="true"></span>
				                Add
			                </button>
                            <button id="btn_cancelproduct" onserverclick="btn_cancelproduct_Click" runat="server" class="btn btn-danger" visible="false">
				                <span class="glyphicon glyphicon-remove" aria-hidden="true"></span>
				                Cancel
			                </button>
                        </span>
				    </div>
                    

                </div>
                <div>
                    <br />
                    <asp:Literal runat="server" ID="lit_Message" />
                </div>
            </asp:Panel>
        </div>
        <br />
        <div class="row">
            <div class="col-xs-12 col-sm-6">
                <asp:Literal runat="server" ID="lit_ProductHeader" />
                <asp:CheckBoxList runat="server" ID="cbl_Product" style="width:100%;" />
            </div>
            <div class="col-xs-12 col-sm-6">
                <asp:Literal runat="server" ID="lit_DocumentPromoAffectedHeader" />
                <asp:Literal runat="server" ID="lit_PromoDocumentAffected" />
            </div>
        </div>
        <%--<div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30" />
                    <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
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

        </div>--%>

        <hr />
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Submit
            </button>
        </div>
    </form>
</asp:Content>

