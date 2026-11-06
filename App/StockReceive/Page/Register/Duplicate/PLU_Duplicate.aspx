<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PLU_Duplicate.aspx.cs" Inherits="Page_Register_PLU_Reborn" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    PLU DJ Duplicate
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
                <div class="col-sm-1">
                    <asp:Button runat="server" ID="btnCheck" OnClick="refreshPage" CssClass="btn btn-primary" Text="Check"/>
                </div>
                <label class="col-md-1 control-label" for="txtPLU" runat="server" id="labelCopy" visible="false">Total Copy :</label>
                <div class="col-sm-1">
                    <asp:TextBox runat="server" ID="qtyCopy"  visible="false" CssClass="form-control"/>
                </div>
                <div class="col-sm-2">
                    <asp:Button runat="server" ID="btnCopy"  visible="false" OnClick="Save" CssClass="btn btn-primary" Text="Execute"/>
                </div>
            </div>
            <div class="form-group">
                
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
    </form>
    <script>

    </script>
</asp:Content>
