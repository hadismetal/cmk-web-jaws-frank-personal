<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Create_Certificate.aspx.cs" Inherits="Page_Create_Create_Certificate" Debug="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="Server">
    Print Certificate
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="Server">
    Register
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="Server">
    <form id="Form1" runat="server">
        <div id="header" runat="server">
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="form-horizontal">
                        <div class="form-group">
                            <label class="col-sm-2 control-label">Nama Event</label>
                            <div class="col-sm-5">
                                <asp:TextBox ID="txnama" runat="server" CssClass="form-control text-left"></asp:TextBox>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="inputPassword3" class="col-sm-2 control-label">Keterangan</label>
                            <div class="col-sm-10">
                                <asp:TextBox ID="txketerangan" runat="server" CssClass="form-control text-left"></asp:TextBox>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">

                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <asp:Literal runat="server" ID="lit_msg_header" />
                    <div class="button-inline">
                        <button id="next2" runat="server" class="btn btn-success" onserverclick="next_Click">
                            Next
                            <i class="icon-arrow-right"></i>
                        </button>
                    </div>
                </div>
            </div>

        </div>



        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-4">
                <div id="content1" runat="server" visible="false">
                    <table class="table-form" style="display: block;">
                        <tr>
                            <td class="fixed-150">Nama Event
                            </td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lbnama" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>Keterangan
                            </td>
                            <td>:</td>
                            <td>
                                <asp:Label ID="lbket" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="fixed-150">Date Create
                            </td>
                            <td>:</td>
                            <td colspan="2">
                                <asp:Label ID="ldc" runat="server"></asp:Label></td>
                        </tr>
                        <tr>
                            <td class="fixed-150">Date Expired
                            </td>
                            <td>:</td>
                            <td colspan="2">
                                <asp:Label ID="lde" runat="server"></asp:Label></td>
                        </tr>
                    </table>
                </div>

                <div id="content2" runat="server">
                    <table class="table-form" style="display: block;">
                        <tr>
                            <td class="fixed-150">
                                PLU
                                &nbsp;
                                <asp:Label ID="lbplu" runat="server" Text="(DJ / PG)" CssClass="text-muted"></asp:Label><br />
                                <asp:Label ID="lbtipeplu" runat="server" CssClass="text-muted" Visible="false"></asp:Label>
                            </td>
                            <td>:</td>
                            <td>
                                <asp:Panel runat="server" DefaultButton="btn_add_cplu">
                                    <div class="input-group">

                                        <asp:TextBox ID="iplu" runat="server" class="form-control text-center" Style="text-transform: uppercase;" aria-label="text input" />
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="btn_add_cplu"
                                                runat="server" type="button"
                                                CssClass="btn btn-primary"
                                                OnClick="btn_add_cplu_Click">
                                                    <span aria-hidden="true" class="glyphicon glyphicon-plus-sign"></span>
                                            </asp:LinkButton>

                                        </div>

                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="3" style="text-align:center;">
                                <asp:Literal runat="server" ID="lit_msg_warning" />
                            </td>
                            <%--<td></td>
                            <td>
                                <div class="input-group-addon">
                                    <asp:Label ID="lbinfo" runat="server"></asp:Label>
                                </div>
                            </td>--%>
                        </tr>
                    </table>
                </div>
            </div>
            
            <div class="col-xs-12 col-sm-6 col-md-3">
                <asp:Table ID="tb1" runat="server" CssClass="table-list" style="width:100%;">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="50">No</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="200">Product Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="50">Qty</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-5">
                
                <div id="content" runat="server" visible="false">
                    
                        <asp:Table ID="tb" runat="server" CssClass="table-list">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell Width="50">No</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="150">PLU</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="200">Product Item</asp:TableHeaderCell>
                                <asp:TableHeaderCell Width="50"></asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    

                </div>
            </div>
        </div>
        <div class="row">
            <div class="col-xs-12">
                
                <div class="button-inline">
                    <button id="btrev" runat="server" class="btn btn-warning" onserverclick="btrev_ServerClick">
                        Review&nbsp;<span class="glyphicon glyphicon-arrow-right" aria-hidden="true"></span>
                    </button>
                    <button id="btcancel" runat="server" class="btn btn-info" onserverclick="btcancel_ServerClick" visible="false">
                        <span class="glyphicon glyphicon-arrow-left" aria-hidden="true"></span>&nbsp;Back
                    </button>
                    <button id="btsubmit" runat="server" class="btn btn-success" onserverclick="save_Click" visible="false">
                        <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>&nbsp;Submit
                    </button>
                    
                </div>
            </div>
        </div>
    </form>
    
    <script type="text/javascript">

        //window.onload = function () {
        //    var head = document.getElementById("header");
        //    var con = document.getElementById("content");
        //    head.style.display = "block";
        //    con.style.display = "none";
        //}

        function HideDiv() {
            var toggle = document.getElementById("next");
            var head = document.getElementById("header");
            var con = document.getElementById("content");

            head.style.display = "none";
            con.style.display = "block";
        }

        function goDelete(pcid, tipe, idpro) {
            txtConfirm =
				"Are you sure to void this PLU ?\n\n";

            if (confirm(txtConfirm))
                location.href = 'Create_Certificate_Delete.aspx?id=' + pcid + '&tipe=' + tipe + '&idpro=' + idpro;
        }
    </script>
</asp:Content>

