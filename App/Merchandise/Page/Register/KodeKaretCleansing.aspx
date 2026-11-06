<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="KodeKaretCleansing.aspx.cs" Inherits="KodeKaretCleansingHome" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Kode Karet Cleansing
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    
    <style type="text/css">
        .form-group .select2-container {
            position: relative;
            z-index: 2;
            float: left;
            width: 100%;
            margin-bottom: 0;
            display: table;
            table-layout: fixed;
        }

        .form-group .select2-container {
            border:none;
            padding:0;
        
        }
    </style>
    <form id="Form1" runat="server" class="form-inline">
        <div class="row">
            <div class="col-xs-12">
                <a href="KodeKaretCleansingAdd.aspx" class="btn btn-success" style="text-align: left;">
                    <i class="icon-plus"></i>
                    Create New Batch
                </a>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                <div style="padding-top:30px;">
                    Showing only last of 3 months transaction.
                </div>
                <br />
                <asp:Table ID="tbl" runat="server" CssClass="table-list" Style="width: 100%;">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="50" />
                        <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Sumber</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Product Item</asp:TableHeaderCell>
                        <%--<asp:TableHeaderCell Width="50">Total</asp:TableHeaderCell>--%>
                        <asp:TableHeaderCell Width="100">Status</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Operator</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Approval</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>

        </div>
    </form>
    <script>
        $(document).ready(function () {
            $("select").select2({ width: '100%' });
        });

    </script>
</asp:Content>

