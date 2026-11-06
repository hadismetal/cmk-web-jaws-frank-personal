<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sales_Form.aspx.cs" Inherits="Page_Register_Sales_Form" %>

<%@ Register Src="MenuSales.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Sales Order
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <p>
        <span class="text-muted">Location</span><br />
        <span class="text-bold" id="location" runat="server" />
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
                                    <a href="Sales_DataCustomer.aspx" class="btn btn-info" id="search_customer" runat="server">
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
                                    <a href="Sales_DataAgent.aspx" class="btn btn-info" id="search_agent" runat="server">
                                        <i class="icon-search"></i>
                                    </a>
                                </div>
                                <asp:TextBox ID="agent" Enabled="false" runat="server" CssClass="form-control" MaxLength="50" Width="200" />
                            </div>
                        </td>
                    </tr>
					<tr>
                        <td class="fixed-150">JP / JR (Cross SBU)
                        </td>
                        <td>:</td>
                        <td>
							<div class="form-inline">
								<div class="form-group">
									<div class="input-group">
										<div class="input-group-btn">
											<a href="Sales_DataAgent_CrossBrand.aspx" class="btn btn-info" id="search_agentcb" runat="server">
												<i class="icon-search"></i>
											</a>
										</div>
										<asp:TextBox ID="agentcb" Enabled="false" runat="server" CssClass="form-control" MaxLength="50" Width="200" />
									</div>
								</div>
							<div>
                        </td>
                    </tr>
					<tr>
						<td class="fixed-150">
                        </td>
                        <td></td>
						<td style="padding-top:2.5px">
							<div class="form-group">
								<asp:TextBox ID="storecb" Enabled="false" runat="server" CssClass="form-control" MaxLength="50" Width="240" Placeholder="Store Cross Brand" style="margin-bottom:2.5px"/>
								<asp:TextBox ID="sbucb" Enabled="false" runat="server" CssClass="form-control" MaxLength="50" Width="240" Placeholder="SBU Cross Brand" />
							</div>
						</td>
					</tr>
					<tr>
                        <td class="fixed-150">Given To</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox runat="server" ID="givento" CssClass="form-control" MaxLength="50" Width="200"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Tanggal</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="tgl" runat="server" CssClass="form-control text-center" Width="110" />
                            <script>
                                $(document).ready(function () { $('#content_tgl').datepicker({ format: 'dd-mm-yyyy' }); });
                            </script>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="tgl_RF" runat="server" />
                            <span id="tgl_ERR" runat="server" class="text-validator" visible="false">Format</span>
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
				<div class="form-group width-300">
                    <label>
                        Event :
                    </label>
                    <asp:DropDownList ID="ddlevent" runat="server" CssClass="form-control"></asp:DropDownList>
                </div>
            </div>
        </div>


        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
        </div>
    </form>
</asp:Content>
