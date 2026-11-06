<%@ Page Language="C#" MasterPageFile="~/MasterFile.master" AutoEventWireup="true" CodeFile="UserUpdate.aspx.cs" Inherits="Page_UserUpdate" %>

<%@ Register Src="MenuFile.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    User Account
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    <asp:Literal ID="h1_st" runat="server" />
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_id" runat="server">
    ID:<asp:Literal ID="h1_id" runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <form runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-6">


                <div class="row">
                    <div class="col-xs-12 col-sm-6">
                        <div class="form-group">

                            <label>
                                NIK :
                            </label>
                            <asp:TextBox ID="nik" runat="server" CssClass="form-control" Style="text-transform: uppercase;" />


                        </div>
                    </div>
                    <div class="col-xs-12 col-sm-6">
                        <div class="form-group">
                            <label>
                                SMS :
                            </label>
                            <asp:TextBox ID="sms" runat="server" CssClass="form-control" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-12">
                        <div class="form-group">
                            <label>
                                Nama :
                            </label>
                            <asp:TextBox ID="nama" runat="server" CssClass="form-control text-bold" MaxLength="50" />
                        </div>

                        <div class="form-group">
                            <label>
                                Keterangan :
                            </label>
                            <asp:TextBox ID="keterangan" runat="server" CssClass="form-control" />
                        </div>

                    </div>
                </div>


            </div>
            <div class="col-xs-12 col-sm-6">
                <div class="form-group">
                    <label>
                        Email :
                    </label>
                    <%--<asp:requiredfieldvalidator id="email_RF" runat="server" />
			        <asp:regularexpressionvalidator id="email_RE" runat="server" skinid="mail" />--%>
                    <span id="email_ERR" runat="server" class="text-validator" visible="false">Sudah Digunakan</span>
                    <asp:TextBox ID="email" runat="server" CssClass="form-control text-bold" />
                </div>
                <div class="alert alert-info">
                    Valid Email per 9 Okt 2017 :
                    <ul>
                        <li><i>@centralmegakencana.com</i></li>
                        <li><i>@frankncojewellery.com</i></li>
                        <li><i>@miss-mondial.com</i></li>
                        <li><i>@mondialjeweler.com</i></li>
                        <li><i>@palacejeweler.com</i></li>
                    </ul>

                </div>


            </div>
        </div>

        <div class="form-group cb-single">
            <div id="div_draft" runat="server">
                <label>
                    <strong class="text-draft">Status : Draft</strong>
                    <small>Data tidak dimunculkan di dalam sistem</small>
                </label>
            </div>
            <div id="div_disable" runat="server">
                <asp:CheckBox ID="disable" runat="server" />
                <label for="content_disable">
                    <strong class="text-warning">Status : Disable</strong>
                    <small>Data tidak dimunculkan di dalam sistem</small>
                </label>
            </div>
        </div>
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-ok"></i>
                Save
            </button>
            <asp:Button ID="del" runat="server" CssClass="btn btn-danger" Text="Delete" Width="80" OnClick="del_Click" />
            <span id="nodel" runat="server" class="text-validator" visible="false">Disable Delete : File Sudah Digunakan</span>
        </div>
    </form>
</asp:Content>

