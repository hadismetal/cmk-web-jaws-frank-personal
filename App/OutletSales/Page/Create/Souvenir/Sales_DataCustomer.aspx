<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sales_DataCustomer.aspx.cs" Inherits="Page_Register_DataCustomer" %>

<%@ Register Src="MenuSales.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Souvenir
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">

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
    </style>

    <uc1:Menu ID="menu" runat="server" />
    <hr />
    <form id="Form1" runat="server" class="form-inline">
        <div id="customerlist" runat="server">

            <asp:Panel ID="p" runat="server" DefaultButton="go">
                <div class="form-group">
                    <div class="text-muted">
                        &nbsp;
                    </div>
                    <button id="Button1" runat="server" class="btn btn-success" onserverclick="AddCustomer_Click">
                        <i class="icon-plus"></i>
                        &nbsp;New Customer
                    </button>
                </div>
                <div class="form-group">
                    <div class="text-muted">
                        Customer (Nama / Kode / Member) :
                    </div>
                    <asp:TextBox ID="kw_nama" runat="server" CssClass="form-control" Width="250" />
                </div>
                <div class="form-group">
                    <div class="text-muted">
                        Alamat :
                    </div>
                    <asp:TextBox ID="kw_alamat" runat="server" CssClass="form-control" Width="200" />
                </div>
                <div class="form-group">
                    <div class="text-muted">
                        Kota :
                    </div>
                    <asp:TextBox ID="kw_kota" runat="server" CssClass="form-control" Width="120" />
                </div>
                <div class="form-group">
                    <div class="text-muted">
                        Telepon :
                    </div>
                    <asp:TextBox ID="kw_telp" runat="server" CssClass="form-control" Width="120" />
                </div>
                <div class="form-group">
                    <div class="text-muted">
                        Lahir :
                    </div>
                    <asp:TextBox ID="kw_birth_date" runat="server" MaxLength="2" CssClass="form-control" Width="60" placeholder="Tgl" />
                    <asp:TextBox ID="kw_birth_month" runat="server" MaxLength="2" CssClass="form-control" Width="60" placeholder="Bln" />
                </div>
                <div class="form-group">
                    <div class="hidden-xs">&nbsp;</div>
                    <div>
                        <asp:LinkButton ID="go" CssClass="btn btn-primary btn-md" runat="server" OnClick="refreshPage">
                        <span class="glyphicon glyphicon-search"></span>&nbsp;Search
                        </asp:LinkButton>
                        <a id="cancel" runat="server" class="btn btn-default">
                            <i class="icon-remove"></i>
                            Cancel
                        </a>
                    </div>
                </div>
            </asp:Panel>
            <div>
                <br />
                <asp:Literal runat="server" ID="lit_countcustomer" />
            </div>
            <div class="table-responsive">
                <asp:Table ID="tb" runat="server" CssClass="table-list" Style="width: 100%;">
                    <asp:TableHeaderRow>
                        <asp:TableHeaderCell Width="50" />
                        <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="230">Customer</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="150">Telp & HP</asp:TableHeaderCell>
                        <asp:TableHeaderCell Width="100">Birthdate</asp:TableHeaderCell>
                        <asp:TableHeaderCell>KTP, Email, Alamat</asp:TableHeaderCell>
                    </asp:TableHeaderRow>
                </asp:Table>
            </div>
            <hr />
        </div>

        <div id="addcustomer" runat="server" visible="false">
            <div class="row">
                <div class="col-xs-12 col-sm-6 col-md-6">
                    
                    <div class="row">
                        <div class="col-xs-12 col-sm-12 col-md-6" style="padding-bottom:15px;">
                            <asp:Panel ID="Panel1" runat="server" DefaultButton="btn_search_notelp">
                            No. Handphone&nbsp;<span class="glyphicon glyphicon-asterisk" style="color: red;" aria-hidden="true"></span><br />
                            <div class="input-group">
                                <asp:TextBox ID="addrnotelp1" runat="server" CssClass="form-control" Text="62" Style="text-transform: uppercase;" />
                                <span class="input-group-btn">
                                    <asp:LinkButton ID="btn_search_notelp" 
                                                runat="server"  type="button"
                                                CssClass="btn btn-primary"    
                                                OnClick="btn_search_notelp_Click">
                                        <span aria-hidden="true" class="glyphicon glyphicon-arrow-right"></span>&nbsp;Check
                                    </asp:LinkButton>
                                </span>
                            </div>
                            </asp:Panel>
                        </div>
                        <div class="col-xs-12 col-sm-12 col-md-6" runat="server" id="div_cust_nama" style="padding-bottom:15px;">
                            Nama Customer&nbsp;<span class="glyphicon glyphicon-asterisk" style="color: red;" aria-hidden="true"></span><br />
                            <asp:TextBox ID="nama" runat="server" CssClass="form-control text-bold" MaxLength="50" Style="text-transform: uppercase;" />
                            
                        </div>
                    </div>
                    <asp:Panel ID="Panel2" runat="server" DefaultButton="savedummy" >
                        <asp:Button ID="savedummy" runat="server" OnClick="save_Click" CssClass="invisible" style="width:1px; height:1px;" />
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12" runat="server" id="div_cust_alamat" style="padding-bottom:15px;">
                                Alamat&nbsp;<span class="glyphicon glyphicon-asterisk" style="color: red;" aria-hidden="true"></span><br />
                                <asp:TextBox ID="addralamat" runat="server" CssClass="form-control" Style="text-transform: uppercase;" />
                            

                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-6 col-sm-6 col-md-6" runat="server" id="div_cust_kota" style="padding-bottom:15px;">
                                Kota&nbsp;<span class="glyphicon glyphicon-asterisk" style="color: red;" aria-hidden="true"></span><br />
                                <asp:DropDownList ID="kota" runat="server" />
                            </div>
                            <div class="col-xs-6 col-sm-6 col-md-6" runat="server" id="div_cust_agama" style="padding-bottom:15px;">
                                Agama<br />
                                <asp:DropDownList ID="agama" runat="server" />
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-9" runat="server" id="div_cust_email" style="padding-bottom:15px;">
                                Email<br />
                                <asp:TextBox ID="email" runat="server" CssClass="form-control" Style="text-transform: uppercase;" />
                            
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-3" runat="server" id="div_cust_tgllahir" style="padding-bottom:15px;">
                                Tgl Lahir<br />
                                <asp:TextBox ID="tgllahir" runat="server" CssClass="form-control text-center" Width="110" Style="text-transform: uppercase;" />
                            
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-12">
                                <asp:Literal runat="server" ID="lit_msg_nomor_hp" />
                                <button runat="server" id="btn_new_cust_form" onserverclick="btn_new_cust_form_Click" class="btn btn-primary" type="button" visible="false">
                                    <i class="icon-arrow-right"></i>
                                    New Customer Form

                                </button>
                            </div>
                        </div>
                    </asp:Panel>
                        
                </div>
                <div class="col-xs-12 col-sm-6 col-md-6">
                    <div class="alert alert-warning">
                        <i>Notes :</i>
                        <ul>
                            <li><i>Semua kolom inputan tidak dapat menggunakan petik satu ('), inputan petik satu akan otomatis digantikan dengan spasi.</i></li>
                            <li>
                                <i>Format inputan nomor handphone :</i>
                                <ul>
                                    <li><i>Semua karakter ditulis menggunakan angka</i></li>
                                    <li><i>Semua karakter ditulis menyambung (tanpa spasi, strip, dll)</i></li>
                                    <li><i>Jumlah karakter minimal 9 maksimal 13</i></li>
                                </ul>

                            </li>
                            <li>
                                <i>Format inputan email :</i>
                                <ul>
                                    <li><i>Inputan ditulis lengkap menggunakan "@" dan domain email</i></li>
                                    <li><i>Jumlah karakter minimal 5 maksimal 100</i></li>
                                </ul>

                            </li>
                            <li><i>Harap periksa kembali penginputan nama customer. Jika terjadi kesalahan dalam penginputan nama customer, revisi dapat dilakukan menggunakan modul Data Customer di hari yang sama.</i></li>
                        </ul>
                    </div>

                </div>
            </div>
            <div class="row">
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <asp:Literal runat="server" ID="lit_message" />

                </div>
                <div class="col-xs-12 col-sm-12 col-md-12">
                    <hr />
                    <asp:LinkButton ID="save" 
                                runat="server"  type="button"
                                CssClass="btn btn-primary"    
                                OnClick="save_Click">
                        <span aria-hidden="true" class="glyphicon glyphicon-arrow-right"></span>&nbsp;Save
                    </asp:LinkButton>
                    <button id="Button2" runat="server" class="btn btn-default" onserverclick="Cancel_Click">
                        <i class="icon-remove"></i>
                        Cancel
                    </button>
                </div>
            </div>

        </div>
    </form>
    <script>
        $(document).ready(function () {
            $('#<%=tgllahir.ClientID%>').datepicker({
                startDate: '-17y',
                format: 'dd-mm-yyyy'
            });
        });
    </script>
    <script>
        $(document).ready(function () {
            $("select").select2({ width: '100%' });
        });

    </script>

</asp:Content>

