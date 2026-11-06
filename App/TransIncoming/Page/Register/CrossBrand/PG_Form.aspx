<%@ page EnableEventValidation="false" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="PG_Form.aspx.cs" inherits="PG_Form" %>

<%@ register src="~/Page/Register/MenuPG.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Plain Gold
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">

	<uc1:menu id="menu" runat="server" />
    <hr />

        <div class="panel panel-info">
            <div class="panel-heading  hidden-sm hidden-xs">
            <h3 class="panel-title">Form Summary</h3>
            </div>
            <div class="panel-body">
                    <div class="col-md-5">
			        <table class="table-file">
		                <tr>
			                <td class="text-label">Market Brand</td>
			                <td>:</td>
			                <td id="brand" runat="server" class="text-bold" />
		                </tr>
		                <tr>
			                <td class="text-label">From <asp:Literal runat="server" ID="lit_type_from" /></td>
			                <td>:</td>
			                <td id="from" runat="server" class="text-bold" />
		                </tr>
                        <tr>
			                <td class="text-label">To <asp:Literal runat="server" ID="lit_type_to" /></td>
			                <td>:</td>
			                <td id="to" runat="server" class="text-bold" />
		                </tr>
	                </table>
		            </div>
		            <br class="visible-sm visible-xs" />
		            <div class="col-md-5">
			        <table class="table-file">
		                <tr>
			                <td class="text-label">Tanggal</td>
			                <td>:</td>
			                <td id="tgl" runat="server" class="text-bold" />
		                </tr>
		                <tr>
			                <td class="text-label">Kurir</td>
			                <td>:</td>
			                <td id="kurir" runat="server" class="text-bold" />
		                </tr>
                        <tr>
			                <td class="text-label">Keterangan</td>
			                <td>:</td>
			                <td id="keterangan" runat="server" class="text-bold" />
		                </tr>
                        <tr>
			                <td class="text-label">Tanggal ETA</td>
			                <td>:</td>
			                <td id="eta" runat="server" class="text-bold" />
		                </tr>
	                </table>
		            </div>
            </div>
        </div>

        <div class="panel panel-info">
            <div class="panel-heading hidden-sm hidden-xs">
            <h3 class="panel-title">Product List Summary</h3>
            </div>
            <div class="panel-body">
             <br class="visible-sm visible-xs" />
		        <div class="col-md-5">
                    <table class="table-file">
                        <tr>
			                <td class="text-label">Qty</td>
			                <td>:</td>
			                <td id="totalqty" runat="server" class="text-bold" style="text-align:right;" />
		                </tr>
		                <tr>
			                <td class="text-label">Gross Weight</td>
			                <td>:</td>
			                <td id="totalgross" runat="server" class="text-bold" style="text-align:right;" />
		                </tr>
		                <%--<tr>
			                <td class="text-label">Netto Weight</td>
			                <td>:</td>
			                <td id="totalnetto" runat="server" class="text-bold" style="text-align:right;" />
		                </tr>
                        <tr>
			                <td class="text-label">Butir</td>
			                <td>:</td>
			                <td id="totalbutir" runat="server" class="text-bold" style="text-align:right;" />
		                </tr>
                        <tr>
			                <td class="text-label">Carat</td>
			                <td>:</td>
			                <td id="totalcarat" runat="server" class="text-bold" style="text-align:right;" />
		                </tr>--%>
                        <tr>
			                <td class="text-label">Product Item</td>
			                <td>:</td>
			                <td id="product_item_summary" runat="server" class="text-bold" style="text-align:right;" />
		                </tr>
                        <tr>
			                <td class="text-label">Product Status</td>
			                <td>:</td>
			                <td id="product_status_summary" runat="server" class="text-bold" style="text-align:right;" />
		                </tr>
	                </table>
                </div>
            </div>
        </div>
    <form runat="server">
    <div class="panel panel-info">
          <div class="panel-heading  hidden-sm hidden-xs">
            <h3 class="panel-title">Product List</h3>
          </div>
          <div class="panel-body">
            <div class="row">
                <br class="visible-sm visible-xs" />
		        <div class="form-inline">
	           
                    <asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>
                        <div class="table-responsive" style="margin-left:30px;">
                                <asp:Table ID="tb" runat="server" CssClass="table-list">
                                    <asp:TableHeaderRow>
                                        <asp:TableHeaderCell Width="30" />
                                        <asp:TableHeaderCell Width="110">Image</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="185">Product Item</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="120">Status</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="120">Net Weight</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="120">Gross Weight</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="120">T. Butir</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="120">Carat</asp:TableHeaderCell>
                                        <asp:TableHeaderCell Width="120">Harga Jual</asp:TableHeaderCell>
                                    </asp:TableHeaderRow>
                                </asp:Table>

                                <div class="form-inline">
                                    <asp:CheckBoxList runat="server" ID="cbl_Product" />
                                </div>
                            </div>
	            
            </div>
          </div>
        </div>
    </div>

    <hr />

    <div class="panel panel-primary">
      <div class="panel-heading">
        <h3 class="panel-title">Notes</h3>
      </div>
      <div class="panel-body">
          <table class="table-file">
              <tr>
                  <td>Keterangan :</td>
                  <td>
                      <asp:TextBox ID="txketerangan" runat="server" TextMode="MultiLine" Width="600px" Height="125px"></asp:TextBox>
                  </td>
              </tr>
          </table>
      </div>
    </div>

    </form>
    <div class="row">
        <br class="visible-sm visible-xs" />
		<div class="col-md-5">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next</button>
		</div>
    </div>
</asp:content>

