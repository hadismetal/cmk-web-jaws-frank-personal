<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PLU_Reborn.aspx.cs" Inherits="Page_Register_PLU_Reborn" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    PLU Reborn
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form runat="server" class="container">
        <div class="form-horizontal">
            <div class="form-group">
                <label class="col-md-1 control-label" for="txtPLU">Search PLU :</label>
                <div class="col-sm-2">
                    <asp:TextBox runat="server" ID="txtPLU" CssClass="form-control"/>
                </div>
                <label class="col-sm-1 control-label" for="selectStore">Status :</label>
                <div class="col-sm-2">
                    <asp:DropDownList runat="server" ID="selectStore" CssClass="form-control">
                        <asp:ListItem Value="0" Text="All" ></asp:ListItem>
                        <asp:ListItem Value="1" Text="Exchange"></asp:ListItem>
                        <asp:ListItem Value="2" Text="Resell"></asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="col-sm-2">
                    <asp:Button runat="server" ID="btnCheck" OnClick="refreshPage" CssClass="btn btn-primary" Text="Check"/>
                </div>
            </div>
            <div class="form-group">
                <asp:Table runat="server" CssClass="table table-bordered table-responsive" ID="tblMasterPLU">
                    <asp:TableHeaderRow BackColor="LightBlue">
                        <asp:TableHeaderCell Text ="PLU" CssClass="text-center"/>
                        <asp:TableHeaderCell Text ="Kode Karet" CssClass="text-center"/>
                        <asp:TableHeaderCell Text ="Supplier Number" CssClass="text-center"/>
                        <asp:TableHeaderCell Text ="Supplier Name" CssClass="text-center"/>
                        <asp:TableHeaderCell Text="Warehouse" CssClass="text-center"/>
                        <asp:TableHeaderCell Text="Product" CssClass="text-center"/>
                    </asp:TableHeaderRow>
                </asp:Table>
                <asp:Table runat="server" CssClass="table table-bordered table-responsive" ID="tblDetailPLU">
                </asp:Table>
                <asp:Table runat="server" CssClass="table table-bordered table-responsive" ID="tblDetailStone">
                </asp:Table>
            </div>
        </div>
		<div class="form-group">
            <asp:Label runat="server" ID="harga" Visible="false"></asp:Label>
        </div>
        <div class="form-group">
            <asp:Button runat="server" ID="btnReborn" OnClick="Save" CssClass="btn btn-success" Text="Reborn"/>
        </div>
    </form>
    <script>

    </script>
</asp:Content>
