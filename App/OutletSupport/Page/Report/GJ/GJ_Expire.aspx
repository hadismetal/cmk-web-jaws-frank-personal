<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="GJ_Expire.aspx.cs" inherits="GJ_Expire" %>

<asp:content contentplaceholderid="h1" runat="server">
	Promo Gold Jewellery
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Update Expire
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
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

	<form runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-4">
                <div class="form-group">
                    <label>
					    Nama :
				    </label>
				    <asp:requiredfieldvalidator id="nama_RF" runat="server" />
				    <asp:textbox id="nama" runat="server" cssclass="form-control" maxlength="50" />
                </div>
                <div class="form-group">
                    <label>
						Keterangan :
					</label>
					
                    <asp:textbox id="keterangan" runat="server" cssclass="form-control" textmode="MultiLine" rows="5" style="resize: none;" />
                </div>
                <div class="form-group">
                    <label>
						Tipe Promo:
					</label>
                    <asp:DropDownList ID="tipe" runat="server" cssclass="form-control" autopostback="true" OnSelectedIndexChanged="promoSelected">
                        <asp:ListItem Value="1">Discount</asp:ListItem>
                        <asp:ListItem Value="2">Promo Poin</asp:ListItem>
                        <asp:ListItem Value="3" Selected="True">Promo 1 Harga</asp:ListItem>
                    </asp:DropDownList>		
                </div>
                <div class="form-group">
                    <asp:CheckBox ID="statusasp" runat="server" Checked="true" />		
                    <label>
						ASP No Exchange / Resell:
					</label>
                </div>
            </div>
            <div class="col-xs-12 col-sm-4">
                <div class="form-group">
                    <label>
						Tanggal Mulai:
					</label>
                    <asp:requiredfieldvalidator id="datepicker_RF" runat="server" />		
                    <asp:TextBox CssClass="form-control text-center" type="text" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" Width="120" />			
					
                </div>
                <div class="form-group">
                    <label>
						Tanggal Expire:
					</label>
					<asp:requiredfieldvalidator id="datepicker2_RF" runat="server" />		
                    <asp:TextBox CssClass="form-control text-center" type="text" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" Width="120" />						
					
                </div>
                <div class="form-group width-500" id="discbox" runat="server" visible="false">
                    <div class="row">
                        <div class="col-xs-7">
                            <label>
                                Discount Nominal:
                            </label>
                            <asp:RequiredFieldValidator ID="discount_RF" runat="server" />
                            <asp:TextBox ID="discount" runat="server" CssClass="form-control" MaxLength="50" />
                        </div>
                        <div>
                            <div>&nbsp;</div>
                            <div>IDR</div>
                        </div>
                    </div>
                </div>
                <div class="form-group width-500" id="discpersenbox" runat="server" visible="false">
                    <div class="row">
                        <div class="col-xs-7">
                            <label>
                                Discount Persen:
                            </label>
                            <asp:RequiredFieldValidator ID="discountpersen_RF" runat="server" />
                            <asp:TextBox ID="discountpersen" runat="server" CssClass="form-control" MaxLength="50" />
                        </div>
                        <div>
                            <div>&nbsp;</div>
                            <div>%</div>
                        </div>
                    </div>
                </div>
                <div class="form-group width-500" id="poinbox" runat="server" visible="false">
                    <div class="row">
                        <div class="col-xs-7">
                            <label>
                                Poin:
                            </label>
                            <asp:RequiredFieldValidator ID="poin_RF" runat="server" />
                            <asp:TextBox ID="poin" runat="server" CssClass="form-control" MaxLength="50" />
                        </div>
                        <div>
                            <div>&nbsp;</div>
                            <div>poin</div>
                        </div>
                    </div>
                </div>
                <div class="form-group width-500" id="promohargabox" runat="server" visible="false">
                    <div class="row">
                        <div class="col-xs-7">
                            <label>
                                Harga:
                            </label>
                            <asp:RequiredFieldValidator ID="promoharga_RF" runat="server" />
                            <asp:TextBox ID="promoharga" runat="server" CssClass="form-control" MaxLength="50" />
                        </div>
                        <div>
                            <div>&nbsp;</div>
                            <div>IDR</div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-4">
                <div>
                    <asp:Literal runat="server" ID="lit_msg" />
                </div>
            </div>
        </div>
		<hr />
		<div class="button-inline">
            <button id="back" runat="server" class="btn btn-primary" onserverclick="back_Click">
				<span class="glyphicon glyphicon-arrow-left" aria-hidden="true"></span>
				Back
			</button>
			<button id="save" runat="server" class="btn btn-warning" onserverclick="update_Click">
                <span class="glyphicon glyphicon-edit" aria-hidden="true"></span>
				Update
			</button>
		</div>
	</form>
</asp:content>
