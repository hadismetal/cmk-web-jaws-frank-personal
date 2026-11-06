<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="TradeIn_Form.aspx.cs" Inherits="Page_Register_Sales_Form" %>

<%@ Register Src="MenuTradeIn.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    TradeIn
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <p>
        <span class="text-muted">Location :</span><br />
        <span class="text-bold" id="warehouse" runat="server" />
    </p>
    <hr />
    <form runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-6">
                <table class="table-form">
                    <tr>
                        <td class="fixed-150">Customer
                        </td>
                        <td>:</td>
                        <td>
                            <div class="input-group">
                                <div class="input-group-btn">
                                    <a href="TradeIn_DataCustomer.aspx" class="btn btn-info" id="search_customer" runat="server">
                                        <i class="icon-search"></i>
                                    </a>
                                </div>
                                <asp:TextBox ID="customer" Enabled="false" runat="server" CssClass="form-control" MaxLength="50" Width="200" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="fixed-150">JP / JR
                        </td>
                        <td>:</td>
                        <td>
                            <div class="input-group">
                                <div class="input-group-btn">
                                    <a href="TradeIn_DataAgent.aspx" class="btn btn-info" id="search_agent" runat="server">
                                        <i class="icon-search"></i>
                                    </a>
                                </div>
                                <asp:TextBox ID="agent" Enabled="false" runat="server" CssClass="form-control" MaxLength="50" Width="200" />
                            </div>
                        </td>
                    </tr>
					<tr>
						<td class="fixed-150">Given To</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="givento" runat="server" CssClass="form-control" MaxLength="50" Width="239"/>
                        </td>
					</tr>
					<tr>
                        <td class="fixed-150">Kode Customer Lama</td>
                        <td>:</td>
                        <td>
                            <div class="input-group">
                                <div class="input-group-btn">
                                    <button class="btn btn-info" id="search_oldcustumercode" runat="server" onserverclick="search_oldcustumercode_ServerClick">
                                        <i class="icon-search"></i>
                                    </button>
                                </div>
                                <asp:TextBox ID="txtoldcustomercode" runat="server" CssClass="form-control" MaxLength="50" Width="200"/>
                            </div>
                        </td>
                        <td>
                            <asp:TextBox ID="txtnameoldcustomercode" runat="server" CssClass="form-control" MaxLength="50" Width="200" ReadOnly="true"/>
                            <asp:Label ID="lblcheckoldcustumer" runat="server" Visible="false" style="color:red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>Tanggal</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="tgl" runat="server" CssClass="form-control text-center" Width="110" Enabled="false" />
                            <script>
                                $(document).ready(function () { $('#content_tgl').datepicker({ format: 'dd-mm-yyyy' }); });
                            </script>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="tgl_RF" runat="server" />
                            <span id="tgl_ERR" runat="server" class="text-validator" visible="false">Format</span>
                        </td>
                    </tr>
					<tr>
						<td>Send E-receipt</td>
                        <td>:</td>
						<td>
							<label class="radio-inline">
								<asp:RadioButton runat="server" GroupName="rdo_ereceipt" ID="rdo_ereceipt_no" Checked="false" onclick="onclickradiosendreceipt(this);" />
								No
							</label>
							<label class="radio-inline">
								<asp:RadioButton runat="server" GroupName="rdo_ereceipt" ID="rdo_ereceipt_yes" Checked="true" onclick="onclickradiosendreceipt(this);" />
								Yes
							</label>
							<div id="div_receipt" style="display:none">
								<div class="form-group">
									<asp:DropDownList runat="server" ID="ddl_erecipt" onchange="onchangeddl(this);" CssClass="form-control">
										<asp:ListItem Value="2">Gift</asp:ListItem>
										<asp:ListItem Value="1">Incognito</asp:ListItem>
										<asp:ListItem Value="3">Others</asp:ListItem>
									</asp:DropDownList>
									<asp:TextBox TextMode="MultiLine" runat="server" ID="ereceipt_light" CssClass="form-control" Height="200" />
								</div>
							</div>
							<script type="text/javascript">
								$(document).ready(function(){
									$('#content_ereceipt_light').css('display','none');
									if($('#content_rdo_ereceipt_no').is(':checked')) { onclickradiosendreceipt($('#content_rdo_ereceipt_no')); }
									if($('#content_rdo_ereceipt_yes').is(':checked')) { onclickradiosendreceipt($('#content_rdo_ereceipt_yes')); }
								});
								function onclickradiosendreceipt(e){
									if(e.value == 'rdo_ereceipt_no')
									{
										$('#div_receipt').css('display','');
									}
									else if(e.value == 'rdo_ereceipt_yes')
									{
										$('#div_receipt').css('display','none');
									}
								}
								
								function onchangeddl(e){
									if(e.value == '2' || e.value == '1')
									{
										$('#content_ereceipt_light').css('display','none');
									}
									else
									{
										$('#content_ereceipt_light').css('display','');
									}
								}
								
							</script>
						</td>
					</tr>
					<tr runat="server" id="rownpwp" visible="false">
                        <td>NPWP</td>
                        <td>:</td>
                        <td>
                            <asp:RadioButtonList runat="server" ID="isanynpwp" CssClass="table table-bordered">
                                <asp:ListItem Value="0" Selected="True">Tidak Ada</asp:ListItem>
                                <asp:ListItem Value="1">Ada</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>

                </table>
            </div>
            <div class="col-xs-12 col-sm-6">
                <div class="form-group width-500">
                    <label>
                        Keterangan :
                    </label>
                    <asp:TextBox ID="keterangan" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                </div>
            </div>
        </div>


        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click" onclick="if (!onconfirmereceipt()) return false;">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
			<script type="text/javascript">
				function onconfirmereceipt() {
					var cuy = $('#content_ddl_erecipt :selected').val();
					var cuy_banget = $('#content_ereceipt_light').val();
					if (cuy == '3' && cuy_banget == '') {
						alert('Jika pilih Others maka wajib isi keterangan...');
						return false;
					}
					else{
						return true;
					}
				}
			</script>
        </div>
    </form>
</asp:Content>
