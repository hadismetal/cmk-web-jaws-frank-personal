<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="PG.aspx.cs" inherits="Page_Create_Product" %>

<%@ register src="MenuPromo.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Promo Plain Gold
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content ID="Content1" contentplaceholderid="content" runat="server">
    <script>
        $(document).ready(function () {
            $('#<%=datepicker.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
            $('#<%=datepicker2.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
        });
    </script>

	<uc1:menu id="menu" runat="server" />
	<form id="Form1" runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-4">
                <div class="form-group">
                    <label>
                        Nama Promo:
                    </label>
                    <asp:RequiredFieldValidator ID="nama_RF" runat="server" />
                    <asp:TextBox ID="nama" runat="server" CssClass="form-control" MaxLength="50" />
                </div>
                <div class="form-group">
                    <label>
                        Keterangan:
                    </label>

                    <asp:TextBox ID="keterangan" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" Style="resize: none;" />
                </div>


            </div>
            <div class="col-xs-12 col-sm-4">
                <div class="row">
                    <div class="col-xs-12 col-sm-12">
                        <div>
                            <div class="form-group col-xs-12 col-sm-6" style="padding-left: 0; padding-right: 0;">
                                <label>
                                    Tanggal Mulai:
                                </label>
                                <asp:RequiredFieldValidator ID="datepicker_RF" runat="server" />
                                <asp:TextBox CssClass="form-control text-center" type="text" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" Width="120" />

                            </div>
                            <div class="form-group col-xs-12 col-sm-6" style="padding-left: 0; padding-right: 0;">
                                <label>
                                    Tanggal Expire:
                                </label>
                                <asp:RequiredFieldValidator ID="datepicker2_RF" runat="server" />
                                <asp:TextBox CssClass="form-control text-center" type="text" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" Width="120" />

                            </div>
                        </div>


                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-12">
                        <div class="form-group">
                            <label>
                                Tipe Promo:
                            </label>
                            <asp:DropDownList ID="tipe" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="promoSelected">
                            </asp:DropDownList>
                        </div>
                    </div>

                </div>
                <div class="row">
                    <div class="col-xs-12 col-sm-6" id="discbox" runat="server" visible="false">
                        <label>
                            Discount Nominal:
                        </label>
                        <div class="input-group">
                            <asp:TextBox ID="discount" runat="server" CssClass="form-control text-right" MaxLength="50" />
                            <div class="input-group-addon">IDR</div>
                        </div>
                        <asp:RequiredFieldValidator ID="discount_RF" runat="server" />

                    </div>

                    <div class="col-xs-12 col-sm-6" id="discpersenbox" runat="server" visible="false">
                        <label>
                            Discount Persen:
                        </label>
                        <div class="input-group">
                            <asp:TextBox ID="discountpersen" runat="server" CssClass="form-control text-right" MaxLength="50" />
                            <div class="input-group-addon">%</div>
                        </div>
                        <asp:RequiredFieldValidator ID="discountpersen_RF" runat="server" />
                    </div>

                    <div class="col-xs-12 col-sm-6" id="poinbox" runat="server" visible="false">
                        <label>
                            Poin:
                        </label>
                        <div class="input-group">
                            <asp:TextBox ID="poin" runat="server" CssClass="form-control text-right" MaxLength="50" />
                            <div class="input-group-addon">Poin</div>
                        </div>
                        <asp:RequiredFieldValidator ID="poin_RF" runat="server" />
                    </div>

                    <div class="col-xs-12 col-sm-6" id="promohargabox" runat="server" visible="false">
                        <label>
                            1 Harga:
                        </label>
                        <div class="input-group">
                            <asp:TextBox ID="promoharga" runat="server" CssClass="form-control text-right" MaxLength="50" />
                            <div class="input-group-addon">IDR</div>
                        </div>
                        <asp:RequiredFieldValidator ID="promoharga_RF" runat="server" />

                    </div>
                
                </div>
            </div>
            <div class="col-xs-12 col-sm-4">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <label>
                            Modifikasi:
                        </label>
                        <div class="form-group">

                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox ID="statusasp" runat="server" Checked="true" />
                                    No Exchange / No Resell (ASP)
                                </label>
                            </div>
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox ID="statusmembership" runat="server" Checked="true" />
                                    No Discount Membership
                                </label>
                            </div>
                            <div class="checkbox">
                                <label>
                                    <asp:CheckBox ID="statusdiscnorm" runat="server" Checked="true" />
                                    No Discount Normal
                                </label>
                            </div>
                        </div>
                    </div>
                </div>

            </div>
            <div class="col-xs-12">
                <div>
                    <asp:Literal runat="server" ID="lit_msg" />
                </div>
            </div>

        </div>
		<div>
            <label>Tipe Input :</label><br />
            <div class="checkbox"><asp:RadioButton runat="server" GroupName="rdo_input" ID="rdo_plu" Text=" PLU" Checked="true" /></div>
            <div class="checkbox"><asp:RadioButton runat="server" GroupName="rdo_input" ID="rdo_kk" Text=" Kode Barang" /></div>
        </div>
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="next_Click">
                <i class="icon-arrow-right"></i>
                Next
            </button>
        </div>
    </form>
</asp:Content>
