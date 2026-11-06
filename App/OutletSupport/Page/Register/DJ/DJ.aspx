<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJ.aspx.cs" Inherits="Page_Create_Product" %>

<%@ Register Src="MenuPromo.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Promo Diamond Jewellery
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
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

    <uc1:Menu ID="menu" runat="server" />
    <form runat="server">
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
				<br/>
				<div class="row">
					<div class="col-xs-12 col-sm-6" id="divextraringsize" runat="server" visible="false">
						<div class="checkbox">
							<label>
								<asp:CheckBox ID="checkextraringsize" runat="server" Checked="false" onchange="oncheckextraringsize();" />
								Extra Ring Size
							</label>
                        </div>
						<div class="input-group" id="cuy" style="display:none">
                            <asp:TextBox type="number" min="0" ID="extraringsize" runat="server" CssClass="form-control text-center" MaxLength="50" />
                            <div class="input-group-addon">sz</div>
                        </div>
						<script>
							function oncheckextraringsize(){
								var remember = document.getElementById("content_checkextraringsize");
								if (remember.checked){
									document.getElementById("cuy").style.display = "";
								}
								else{
									document.getElementById("cuy").style.display = "none";
								}
							}
						</script>
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
            <div class="checkbox"><asp:RadioButton runat="server" GroupName="rdo_input" ID="rdo_kk" Text=" Kode Karet" /></div>
        </div>
        
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="next_Click">
                <i class="icon-arrow-right"></i>
                Next
            </button>
        </div>
    </form>
</asp:Content>
