<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"
AutoEventWireup="true" CodeFile="PG_Form.aspx.cs"
Inherits="Page_Register_PG_Form" %>

<asp:Content ContentPlaceHolderID="h1" runat="server"> Plain Gold </asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
  Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <form runat="server" id="form">
    <table class="table-form">
      <tr>
        <td width="350">
          <div class="block spacing-top">
            <div class="control-group">
              <label class="control-label">Warehouse</label>
              <div class="controls">
                <asp:DropDownList
                  CssClass="form-control"
                  runat="server"
                  ID="ddl_Warehouse"
                />
              </div>
            </div>
          </div>
          <div class="block spacing-top">
            <div class="control-group">
              <label class="control-label">Kadar Tukaran Jual</label>
              <div class="controls">
                <asp:TextBox
                  CssClass="form-control"
                  runat="server"
                  ID="txtTukaranJual"
                />
              </div>
            </div>

            <div class="control-group">
              <label class="control-label">Nomor Validasi</label>
              <div class="controls">
                <asp:TextBox
                  CssClass="form-control"
                  runat="server"
                  ID="txtNomorValidasi"
                />
              </div>
            </div>
          </div>
          <div class="block spacing-top">
            <div class="control-group">
              <label class="control-label">Gold Rate</label>
              <div class="controls">
                <asp:DropDownList
                  CssClass="form-control"
                  runat="server"
                  ID="ddl_rate"
                  OnSelectedIndexChanged="LoadModel"
                  AutoPostBack="true"
                >
                  <asp:ListItem
                    Text="TGP"
                    Value="0"
                    Selected="True"
                  ></asp:ListItem>
                  <asp:ListItem Text="Fix Rate" Value="1"></asp:ListItem>
                </asp:DropDownList>
              </div>
            </div>
          </div>
        </td>
        <td width="400">
          <div class="block spacing-top">
            <div class="control-group">
              <label class="control-label">Supplier</label>
              <div class="controls">
                <asp:DropDownList
                  CssClass="form-control"
                  runat="server"
                  ID="ddl_Supplier"
                />
              </div>
            </div>
          </div>
          <div class="block spacing-top">
            <div class="control-group">
              <label class="control-label">Model</label>
              <div class="controls">
                <asp:DropDownList
                  CssClass="form-control"
                  runat="server"
                  ID="ddl_Model"
                />
              </div>
            </div>
            <div class="control-group">
              <label class="control-label">Kode Material</label>
              <div class="controls">
                <asp:TextBox
                  CssClass="form-control"
                  runat="server"
                  ID="txtKodeMaterial"
                />
              </div>
            </div>
          </div>
          <!-- <div class="block spacing-top">
						<div class="control-group">
							<label></label>
							<div class="controls" style="padding: 11px;">
								<asp:CheckBox ID="ck_consignment" runat="server"/>
								<label class="control-label">Consignment</label>
							</div>
						</div>
					</div> -->
        </td>
      </tr>
      <tr>
        <div class="clearfix"></div>
        <hr />
      </tr>
      <tr>
        <td>
          <div class="input-group">
            <asp:FileUpload
              ID="FileUpload1"
              runat="server"
              class="form-control"
            />
          </div>
        </td>
        <td>
          <asp:Button
            ID="btnUpload"
            runat="server"
            Text="Upload"
            class="btn btn-primary"
            OnClick="uploadData"
          />
          <asp:Button
            runat="server"
            Text="Download Template"
            class="btn btn-primary"
            OnClick="btnTemplate_Click"
          />
        </td>
      </tr>
      <tr>
        <td>
          <asp:Label
            runat="server"
            ID="error"
            Visible="false"
            ForeColor="Red"
          ></asp:Label>
        </td>
      </tr>
      <tr>
        <td>
          <asp:Label
            runat="server"
            ID="success"
            Visible="false"
            ForeColor="Green"
          ></asp:Label>
        </td>
      </tr>
    </table>
  </form>
</asp:Content>
