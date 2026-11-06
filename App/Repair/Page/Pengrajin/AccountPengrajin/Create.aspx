<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Create.aspx.cs" Inherits="Page_Create" %>

<%@ Register Src="MenuCreate.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Create New User
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <form runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-6">


                <div class="row">
                    <div class="col-xs-12 col-sm-6">
                        <div class="form-group">

                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btn_search_nik">
                                <label>
                                    NIK User&nbsp;<span class="glyphicon glyphicon-asterisk" style="color: red;" aria-hidden="true"></span>
                                </label>
                                
                                <div class="input-group">
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="btn_clear"
                                            runat="server" type="button"
                                            CssClass="btn btn-danger"
                                            OnClick="btn_clear_Click">
                                <span aria-hidden="true" class="glyphicon glyphicon-remove"></span>
                                        </asp:LinkButton>
                                    </span>
                                    <asp:TextBox ID="txt_nik" runat="server" MaxLength="6" CssClass="form-control text-center" Style="text-transform: uppercase;" />
                                    <span class="input-group-btn">
                                        <asp:LinkButton ID="btn_search_nik"
                                            runat="server" type="button"
                                            CssClass="btn btn-primary"
                                            OnClick="btn_search_nik_Click">
                                <span aria-hidden="true" class="glyphicon glyphicon-arrow-right"></span>&nbsp;Check
                                        </asp:LinkButton>
                                    </span>

                                </div>
                            </asp:Panel>

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
                                NIK & Nama User:
                            </label>
                            <%--<asp:requiredfieldvalidator id="nama_RF" runat="server" />--%>
                            <div class="input-group">
                                <div class="input-group-addon">
                                    <asp:Literal runat="server" ID="lit_nik_fixed" />
                                </div>
                                <asp:TextBox ID="txt_nama" runat="server" CssClass="form-control text-bold" MaxLength="50" />
                            </div>
                        </div>

                        <div class="form-group">
                            <label>
                                Jabatan User :
                            </label>
                            <asp:TextBox ID="txt_keterangan" runat="server" CssClass="form-control" />
                        </div>

                    </div>
                </div>


            </div>
            <div class="col-xs-12 col-sm-6">
                <div class="form-group">
                    <label>
                        Email :
                    </label>
                    <asp:TextBox ID="txt_email" runat="server" CssClass="form-control text-bold" />
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

        <div class="row">
            <div class="col-xs-12">
                <asp:Literal runat="server" ID="lit_warning" />
                <asp:Literal runat="server" ID="lit_error" />
            </div>
            <div class="col-xs-12">

                <div class="button-inline">
                    <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click" visible="false">
                        <i class="icon-arrow-right"></i>
                        Next Step
                    </button>
                </div>
            </div>
        </div>


    </form>
</asp:Content>

