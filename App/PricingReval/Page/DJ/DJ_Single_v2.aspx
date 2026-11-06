<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ_Single_v2.aspx.cs" Inherits="DJ_Single_v2" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery
</asp:Content>

<asp:content contentplaceholderid="h1_st" runat="server">
	(Single)
</asp:content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
        td {
            vertical-align: top;
        }
    </style>
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">
            <div class="row">
                
                <div class="col-xs-12 col-sm-6">
                    <div>
                        <div class="form-group">
                            <div class="text-muted">
                                PLU :
                            </div>
                            <asp:TextBox ID="kw" CssClass="form-control" runat="server" placeholder="..." Width="175" />
                        </div>
                        
                    </div>
                    <div style="padding-top:15px;">
                        <div class="form-group">
                            <div>
                                <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                                    <i class="icon-search"></i>
                                    Display
                                </button>
                            </div>
                        </div>
                        
                        <div class="form-group">
                            <button id="Button1" runat="server" class="btn btn-warning" onserverclick="Execute">
                                <span class="glyphicon glyphicon-saved" aria-hidden="true"></span> Execute
                            </button>
                        </div>
                        
                    
                    </div>
                </div>
                <div class="col-xs-12 col-sm-6">
                    <div class="col-xs-12">
                        <div class="panel panel-default">
                            <div class="panel-body">
                            
                                <div class="form-group">
                                    <a class="btn btn-default" href="javascript:checkAll_Option(true)"><span class="glyphicon glyphicon-check" aria-hidden="true"></span> Check All Option</a>
                                </div>
                                <div class="form-group">
                                    <a class="btn btn-default" href="javascript:checkAll_Option(false)"><span class="glyphicon glyphicon-unchecked" aria-hidden="true"></span> Uncheck All Option</a>
                                </div>
                                    
                                <div class="clearfix">
                                    <br class="hidden-xs" />
                                </div>
                                <div class="form-group">
                                    <div class="text-muted">
                                        Reval Option 1:
                                    </div>
                                    <asp:CheckBox ID="reval_M" runat="server" Text="&nbsp;M&nbsp;&nbsp;"></asp:CheckBox>
                                    <asp:CheckBox ID="reval_R" runat="server" Text="&nbsp;R&nbsp;&nbsp;"></asp:CheckBox>
                                </div>
                                    
                                <div class="clearfix">
                                </div>
                                <div class="form-group">
                                    <div class="text-muted">
                                        Reval Option 2:
                                    </div>
                                    <asp:CheckBox ID="reval_G" runat="server" Text="&nbsp;Gold&nbsp;&nbsp;"></asp:CheckBox>
                                    <asp:CheckBox ID="reval_SK" runat="server" Text="&nbsp;Stone Reguler&nbsp;&nbsp;"></asp:CheckBox>
                                    <asp:CheckBox ID="reval_SB" runat="server" Text="&nbsp;Stone Rapaport&nbsp;&nbsp;"></asp:CheckBox>
                                    <asp:CheckBox ID="reval_MF" runat="server" Text="&nbsp;Mounting Fee&nbsp;&nbsp;"></asp:CheckBox>
                                    <asp:CheckBox ID="reval_MU" runat="server" Text="&nbsp;Mark Up&nbsp;&nbsp;"></asp:CheckBox>
                                    <asp:CheckBox ID="reval_Rate" runat="server" Text="&nbsp;Rate&nbsp;&nbsp;"></asp:CheckBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>


            </div>
            <div class="row" style="padding-top:15px;">
                <div class="col-xs-12">
                    <div class="form-group">
                        <a class="btn btn-default" href="javascript:checkAll(true)"><span class="glyphicon glyphicon-check" aria-hidden="true"></span> Check All Data</a>
                    </div>
                    <div class="form-group">
                        <a class="btn btn-default" href="javascript:checkAll(false)"><span class="glyphicon glyphicon-unchecked" aria-hidden="true"></span> Uncheck All Data</a>
                    </div>
                    <div class="form-group">
                        <button id="Button4" runat="server" class="btn btn-danger" onserverclick="Delete">
                            <span class="glyphicon glyphicon-trash" aria-hidden="true"></span> Delete Checked
                        </button>
                    </div>
                    <div class="form-group">
                        <button id="Button2" runat="server" class="btn btn-success" onserverclick="Apply">
                            <span class="glyphicon glyphicon-ok" aria-hidden="true"></span> Apply Checked
                        </button>
                    </div>
                </div>
                <div class="col-xs-12">
                    <br />
                    <span style="color:red;"><asp:Literal ID="err" runat="server" /></span>
                </div>
            </div>

            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list text-sm">
                    <asp:TableHeaderRow TableSection="TableHeader">
                        <asp:TableHeaderCell Width="70"></asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="70">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="90">No. PLU</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="140">Kode Karet</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="80">Item</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga Sebelum</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Harga Sesudah</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Mutasi</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">M Awal</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">M</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">M Reval</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">R</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">R Reval</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="130">Net Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Gross Weight</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Butir</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Carat</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Segmen</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Stone Distribution</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Product Level</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
        </form>
        <script>
            function checkAll(checktoggle) {
                var checkboxes = new Array();
                checkboxes = document.getElementsByTagName('input');

                for (var i = 0; i < checkboxes.length; i++) {
                    if (checkboxes[i].type == 'checkbox') {
                        checkboxes[i].checked = checktoggle
                    }
                }

                $('#content_reval_M').prop('checked', false);
                $('#content_reval_R').prop('checked', false);
                $('#content_reval_G').prop('checked', false);
                $('#content_reval_SK').prop('checked', false);
                $('#content_reval_SB').prop('checked', false);
                $('#content_reval_MF').prop('checked', false);
                $('#content_reval_MU').prop('checked', false);
                $('#content_reval_Rate').prop('checked', false);

            }

            function checkAll_Option(checktoggle) {
                $('#content_reval_M').prop('checked', checktoggle);
                $('#content_reval_R').prop('checked', checktoggle);
                $('#content_reval_G').prop('checked', checktoggle);
                $('#content_reval_SK').prop('checked', checktoggle);
                $('#content_reval_SB').prop('checked', checktoggle);
                $('#content_reval_MF').prop('checked', checktoggle);
                $('#content_reval_MU').prop('checked', checktoggle);
                $('#content_reval_Rate').prop('checked', checktoggle);
            }
        </script>
    </div>
</asp:Content>

