<%@ page language="C#" masterpagefile="~/MasterPage.master"
autoeventwireup="true" codefile="DJ_Detail.aspx.cs" inherits="Page_DJ" %>

<asp:content contentplaceholderid="h1" runat="server">
  Diamond Jewellery Detail
</asp:content>

<asp:Content contentplaceholderid="content" runat="server">
  <script>
    $(document).ready(function () {
      $("#<%=datepicker.ClientID%>").datepicker({
        format: "dd-mm-yyyy",
      });
      $("#<%=datepicker2.ClientID%>").datepicker({
        format: "dd-mm-yyyy",
      });
    });
  </script>

  <asp:Label id="sAguarde" Text="Aguarde ..." runat="server" />
  <div id="form_area" runat="server">
    <form id="Form1" runat="server" class="form-inline">
      <div class="form-group">
        <div class="text-muted">Cabang</div>
        <asp:DropDownList
          ID="cabang"
          runat="server"
          CssClass="form-control"
          Width="175"
        >
          <asp:ListItem Value="0">All Warehouse</asp:ListItem>
        </asp:DropDownList>
      </div>
      <div class="form-group">
        <div class="text-muted">Brand</div>
        <asp:DropDownList
          ID="brand"
          runat="server"
          CssClass="form-control"
          Width="175"
        >
          <asp:ListItem Value="0">All Brand</asp:ListItem>
        </asp:DropDownList>
      </div>
      <div class="form-group">
        <div class="text-muted">Stock Keeper</div>
        <asp:DropDownList
          ID="sk"
          runat="server"
          CssClass="form-control"
          Width="175"
        >
          <asp:ListItem Value="0">All Stock Keeper</asp:ListItem>
        </asp:DropDownList>
      </div>

      <div class="form-group">
        <div class="text-muted">Dari</div>
        <asp:TextBox
          CssClass="form-control text-center"
          Width="110"
          type="text"
          placeholder="Dari"
          ID="datepicker"
          runat="server"
          data-date-format="dd-mm-yyyy"
        />
      </div>
      <div class="form-group">
        <div class="text-muted">Sampai</div>
        <asp:TextBox
          CssClass="form-control text-center"
          Width="110"
          type="text"
          placeholder="Sampai"
          ID="datepicker2"
          runat="server"
          data-date-format="dd-mm-yyyy"
        />
      </div>
      <div class="form-group">
        <div class="text-muted">PLU</div>
        <asp:TextBox
          ID="kw"
          CssClass="form-control"
          runat="server"
          placeholder="..."
        />
      </div>
      <div class="clearfix">
        <br class="hidden-xs" />
      </div>
      <div class="form-group">
        <div class="text-muted">Product Level</div>
        <asp:DropDownList
          ID="productlevel"
          runat="server"
          CssClass="form-control"
          Width="175"
        >
          <asp:ListItem Value="0">All Product Level</asp:ListItem>
        </asp:DropDownList>
      </div>
      <div class="form-group">
        <div class="text-muted">Product Category</div>
        <asp:DropDownList
          ID="productcategory"
          runat="server"
          CssClass="form-control"
          Width="175"
        >
          <asp:ListItem Value="0">All Product Category</asp:ListItem>
        </asp:DropDownList>
      </div>
      <div class="form-group">
        <div class="text-muted">Product Item</div>
        <asp:DropDownList
          ID="productitem"
          runat="server"
          CssClass="form-control"
          Width="175"
        >
          <asp:ListItem Value="0">All Product Item</asp:ListItem>
        </asp:DropDownList>
      </div>
      <div class="form-group">
        <div class="text-muted">Supplier Group</div>
        <asp:DropDownList
          ID="suppliergroup"
          runat="server"
          CssClass="form-control"
          Width="175"
          OnSelectedIndexChanged="FillSup"
          AutoPostBack="true"
        >
          <asp:ListItem Value="0">All Supplier Group</asp:ListItem>
        </asp:DropDownList>
      </div>
      <div class="form-group">
        <div class="text-muted">Supplier</div>
        <asp:DropDownList
          ID="supplier"
          runat="server"
          CssClass="form-control"
          Width="175"
        >
          <asp:ListItem Value="">All Supplier</asp:ListItem>
        </asp:DropDownList>
      </div>

      <div class="form-group">
        <div class="text-muted">Allocation Location Type</div>
        <asp:DropDownList
          ID="allc_location_type"
          runat="server"
          CssClass="form-control"
          Width="175"
          AutoPostBack="true"
          OnSelectedIndexChanged="allc_location_type_SelectedIndexChanged"
        >
          <asp:ListItem Value="-1">All Location Type</asp:ListItem>
        </asp:DropDownList>
      </div>

      <div class="form-group">
        <div class="text-muted">Allocation Location</div>
        <asp:DropDownList
          ID="allc_location"
          runat="server"
          CssClass="form-control"
          Width="175"
        >
          <asp:ListItem Value="-1">All Location</asp:ListItem>
        </asp:DropDownList>
      </div>

      <div class="clearfix">
        <br class="hidden-xs" />
      </div>
      <div class="form-group">
        <div class="text-muted">Order By</div>
        <asp:DropDownList
          ID="orderby"
          runat="server"
          CssClass="form-control"
          Width="175"
        >
          <asp:ListItem Value="1">Cabang</asp:ListItem>
          <asp:ListItem Value="2">Product Level</asp:ListItem>
          <asp:ListItem Value="3">Product Category</asp:ListItem>
          <asp:ListItem Value="4">Product Item</asp:ListItem>
        </asp:DropDownList>
      </div>
      <div class="form-group">
        <div class="hidden-xs">&nbsp;</div>
        <div>
          <button
            id="go"
            runat="server"
            class="btn btn-primary"
            onserverclick="refreshPage"
          >
            <i class="icon-search"></i>
            Display
          </button>
        </div>
      </div>
      <div class="form-group">
        <div class="hidden-xs">&nbsp;</div>
        <div>
          <button
            id="Button3"
            runat="server"
            class="btn btn-primary"
            onserverclick="Export"
          >
            <i class="icon-download-alt"></i>
            Export
          </button>
        </div>
      </div>
      <div class="form-group">
        <div class="hidden-xs">&nbsp;</div>
        <div>
          <button
            id="Button1"
            runat="server"
            class="btn btn-primary"
            onserverclick="Print"
          >
            <i class="icon-print"></i>
            Print
          </button>
        </div>
      </div>
    </form>
    <div class="table-responsive">
      <asp:Table
        ID="tb"
        runat="server"
        CssClass="table-list table-striped text-sm"
      >
      </asp:Table>
    </div>
  </div>
</asp:Content>
