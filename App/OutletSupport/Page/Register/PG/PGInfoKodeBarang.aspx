<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PGInfoKodeBarang.aspx.cs" Inherits="Page_Register_PG_PGInfoKodeBarang" %>

<%@ Register Src="MenuPromo.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Promo Plain Gold
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
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
            <asp:Panel DefaultButton="btn_inputproduct" runat="server">
                <div class="form-group width-300">
                    <div class="text-muted">
                        Kode Barang :
                    </div>
                    <asp:TextBox ID="inputproduct" runat="server" CssClass="form-control text-bold" MaxLength="100" />
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
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30" />
                    <asp:TableHeaderCell Width="110">Image</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="185">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Nett Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="300">Nama</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>

            <div class="form-inline">
                <asp:CheckBoxList runat="server" ID="cbl_Product" />
            </div>
        </div>

        <hr />
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Submit
            </button>
        </div>
    </form>
</asp:Content>
