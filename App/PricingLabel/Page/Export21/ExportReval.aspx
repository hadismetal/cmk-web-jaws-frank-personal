<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"
AutoEventWireup="true" CodeFile="ExportReval.aspx.cs" Inherits="Page_Export" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
  Export Label
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="h1_st" runat="server">
  Reval
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
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

    .popover-content {
      font-size: 12px;
    }

    table.table-list td {
      padding: 5px;
    }

    .form-group .select2-container {
      border: none;
      padding: 0;
    }
  </style>
  <form id="Form1" runat="server">
    <div class="row">
      <div class="col-xs-12">
        <div id="errx" runat="server" visible="false"></div>
        <asp:Literal runat="server" ID="lit_test" />
      </div>
    </div>
    <div class="row">
      <div class="col-xs-12 col-sm-3 col-md-3">
        <!-- Standard Label -->
        <div class="panel panel-danger">
          <div class="panel-heading"><i class="icon-tags"></i>&nbsp;Reval</div>
          <div class="panel-body">
            <div class="form-inline">
              <div class="form-group">
                <div class="text-muted">Product Type</div>
                <asp:DropDownList
                  ID="ddl_tipeproduk"
                  runat="server"
                  CssClass="form-control"
                  Width="175"
                >
                  <asp:ListItem Value="DJ">Diamond Jewellery</asp:ListItem>
                  <asp:ListItem Value="PG">Plain Gold</asp:ListItem>
                </asp:DropDownList>
              </div>
              <div class="form-group">
                <div class="text-muted">Location</div>
                <asp:DropDownList
                  ID="ddl_location"
                  runat="server"
                  CssClass="form-control"
                  Width="175"
                >
                </asp:DropDownList>
              </div>
              <div class="form-group">
                <div class="checkbox">
                  <asp:RadioButton
                    runat="server"
                    GroupName="rdo_loc"
                    ID="rdo_loc_1"
                    Text=" by Location"
                    Checked="true"
                  />
                </div>
                <div class="checkbox">
                  <asp:RadioButton
                    runat="server"
                    GroupName="rdo_loc"
                    ID="rdo_loc_w"
                    Text=" All Warehouse"
                  />
                </div>
              </div>
              <div class="form-group">
                <div class="text-muted">Product Item</div>
                <asp:DropDownList
                  ID="ddl_productitem"
                  runat="server"
                  CssClass="form-control"
                  Width="175"
                >
                  <asp:ListItem Value="0">...</asp:ListItem>
                </asp:DropDownList>
              </div>

              <div class="form-group">
                <div class="text-muted">Product Category (DJ/GJ only)</div>
                <asp:DropDownList
                  ID="ddl_productcategory"
                  runat="server"
                  CssClass="form-control"
                  Width="175"
                >
                  <asp:ListItem Value="0">...</asp:ListItem>
                </asp:DropDownList>
              </div>

              <div class="form-group">
                <div class="text-muted">Product Level (DJ/GJ only)</div>
                <asp:DropDownList
                  ID="ddl_productlevel"
                  runat="server"
                  CssClass="form-control"
                  Width="175"
                >
                  <asp:ListItem Value="0">...</asp:ListItem>
                </asp:DropDownList>
              </div>

              <div class="form-group">
                <div class="text-muted">Clarity (DJ only)</div>
                <asp:DropDownList
                  ID="ddl_clarity"
                  runat="server"
                  CssClass="form-control"
                  Width="175"
                >
                  <asp:ListItem Value="0">...</asp:ListItem>
                  <asp:ListItem Value="1">VVS</asp:ListItem>
                  <asp:ListItem Value="2">VS</asp:ListItem>
                  <asp:ListItem Value="3">RED</asp:ListItem>
                </asp:DropDownList>
              </div>
            </div>
            <div>
              <div class="clearfix">
                <br class="hidden-xs" />
              </div>
              <div style="padding-bottom: 5px">
                <br />
                <button
                  id="btnDisplay"
                  runat="server"
                  class="btn btn-primary"
                  style="text-align: left"
                  onserverclick="btnDisplay_Click"
                >
                  <i class="icon-search"></i>
                  Display
                </button>
              </div>
            </div>
            <div runat="server" id="err" visible="false">
              <div class="clearfix">
                <br class="hidden-xs" />
              </div>
              <div
                class="alert alert-xs alert-warning text-center"
                role="alert"
              >
                <asp:Literal runat="server" ID="lit_err" />
              </div>
            </div>
          </div>
        </div>
      </div>

      <div class="col-xs-12 col-sm-9 col-md-9" runat="server" id="div_table">
        <!-- Table Information -->
        <div class="row">
          <div class="col-xs-12 col-sm-12 col-md-12">
            <div class="panel panel-danger">
              <div class="panel-heading">
                <i class="icon-tags"></i>&nbsp;<asp:Literal
                  runat="server"
                  ID="heading_tipeproduk"
                />
              </div>
              <div class="panel-body">
                <div class="row">
                  <div class="col-xs-12 col-sm-6 col-md-6">
                    <asp:Literal runat="server" ID="lit_header1" />
                  </div>
                  <div class="col-xs-12 col-sm-6 col-md-6">
                    <asp:Literal runat="server" ID="lit_header2" />
                  </div>
                </div>
              </div>
            </div>
          </div>
          <div class="col-xs-12 col-sm-12 col-md-12">
            <ul class="nav nav-tabs nav-justified" role="tablist">
              <li role="presentation" class="active">
                <a
                  class="tabcustom"
                  href="#rv_valid"
                  aria-controls="rv_valid"
                  role="tab"
                  data-toggle="tab"
                >
                  <span
                    class="glyphicon glyphicon-ok"
                    aria-hidden="true"
                  ></span>
                  Valid <asp:Literal runat="server" ID="lit_count_valid"
                /></a>
              </li>
              <li role="presentation">
                <a
                  class="tabcustom"
                  href="#rv_promo"
                  aria-controls="rv_promo"
                  role="tab"
                  data-toggle="tab"
                >
                  <span
                    class="glyphicon glyphicon-warning-sign"
                    aria-hidden="true"
                  ></span>
                  Promo <asp:Literal runat="server" ID="lit_count_promo"
                /></a>
              </li>
              <li role="presentation">
                <a
                  class="tabcustom"
                  href="#rv_pending"
                  aria-controls="rv_pending"
                  role="tab"
                  data-toggle="tab"
                >
                  <span
                    class="glyphicon glyphicon-question-sign"
                    aria-hidden="true"
                  ></span>
                  Pending <asp:Literal runat="server" ID="lit_count_pending"
                /></a>
              </li>
              <li role="presentation">
                <a
                  class="tabcustom"
                  href="#rv_sold"
                  aria-controls="rv_sold"
                  role="tab"
                  data-toggle="tab"
                >
                  <span
                    class="glyphicon glyphicon-remove"
                    aria-hidden="true"
                  ></span>
                  Sold <asp:Literal runat="server" ID="lit_count_sold"
                /></a>
              </li>
              <li role="presentation">
                <a
                  class="tabcustom"
                  href="#rv_invalid"
                  aria-controls="rv_invalid"
                  role="tab"
                  data-toggle="tab"
                >
                  <span
                    class="glyphicon glyphicon-remove"
                    aria-hidden="true"
                  ></span>
                  Invalid <asp:Literal runat="server" ID="lit_count_invalid"
                /></a>
              </li>
            </ul>
            <div class="panel panel-default">
              <div class="panel-body">
                <div class="row">
                  <div class="col-xs-12">
                    <div class="tab-content">
                      <div
                        role="tabpanel"
                        class="tab-pane active"
                        id="rv_valid"
                      >
                        <div class="row">
                          <div class="col-xs-12 col-sm-12 col-md-12">
                            <asp:Literal runat="server" ID="lit_rv_valid" />
                          </div>
                          <div class="col-xs-12 col-sm-12 col-md-12">
                            <br />
                            <button
                              id="btnPrint1"
                              runat="server"
                              class="btn btn-primary"
                              style="text-align: left"
                              onserverclick="btnPrint1_Click"
                            >
                              <i class="icon-download-alt"></i>
                              Export
                            </button>
                            <br /><br />
                          </div>
                          <div class="col-xs-12 col-sm-12 col-md-12">
                            <asp:Table
                              ID="tbrv_valid"
                              runat="server"
                              CssClass="table-list"
                              Style="width: 100%;"
                            >
                              <asp:TableHeaderRow>
                                <asp:TableHeaderCell Width="50" />
                                <asp:TableHeaderCell Width="50"
                                  >No.</asp:TableHeaderCell
                                >
                                <asp:TableHeaderCell Width="100"
                                  >PLU</asp:TableHeaderCell
                                >
                                <asp:TableHeaderCell Width="100"
                                  >Item</asp:TableHeaderCell
                                >
                                <asp:TableHeaderCell Width="100"
                                  >Category</asp:TableHeaderCell
                                >
                                <asp:TableHeaderCell
                                  >Location</asp:TableHeaderCell
                                >
                                <asp:TableHeaderCell Width="120"
                                  ><span style="color: blue"
                                    >Harga Standard</span
                                  ></asp:TableHeaderCell
                                >
                                <asp:TableHeaderCell Width="120"
                                  ><span style="color: red"
                                    >Harga Reval</span
                                  ></asp:TableHeaderCell
                                >
                              </asp:TableHeaderRow>
                            </asp:Table>
                          </div>
                        </div>
                      </div>
                      <div role="tabpanel" class="tab-pane" id="rv_invalid">
                        <asp:Literal
                          runat="server"
                          ID="lit_rv_invalid"
                        /><br /><br />
                        <asp:Table
                          ID="tbrv_invalid"
                          runat="server"
                          CssClass="table-list"
                          Style="width: 100%;"
                        >
                          <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="50"
                              >No.</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="100"
                              >PLU</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="100"
                              >Item</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="100"
                              >Category</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell>Location</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120"
                              ><span style="color: blue"
                                >Harga Standard</span
                              ></asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="120"
                              ><span style="color: red"
                                >Harga Reval</span
                              ></asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="120"
                              >Harga Promo</asp:TableHeaderCell
                            >
                          </asp:TableHeaderRow>
                        </asp:Table>
                      </div>
                      <div role="tabpanel" class="tab-pane" id="rv_promo">
                        <asp:Literal
                          runat="server"
                          ID="lit_rv_promo"
                        /><br /><br />
                        <asp:Table
                          ID="tbrv_promo"
                          runat="server"
                          CssClass="table-list"
                          Style="width: 100%;"
                        >
                          <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="50"
                              >No.</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="100"
                              >PLU</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="100"
                              >Item</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="100"
                              >Category</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell>Location</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120"
                              ><span style="color: blue"
                                >Harga Standard</span
                              ></asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="120"
                              ><span style="color: red"
                                >Harga Reval</span
                              ></asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="120"
                              >Harga Promo</asp:TableHeaderCell
                            >
                          </asp:TableHeaderRow>
                        </asp:Table>
                      </div>
                      <div role="tabpanel" class="tab-pane" id="rv_pending">
                        <asp:Literal
                          runat="server"
                          ID="lit_rv_pending"
                        /><br /><br />
                        <asp:Table
                          ID="tbrv_pending"
                          runat="server"
                          CssClass="table-list"
                          Style="width: 100%;"
                        >
                          <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="50"
                              >No.</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="100"
                              >PLU</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="100"
                              >Item</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="100"
                              >Category</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell>Location</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120"
                              ><span style="color: blue"
                                >Harga Standard</span
                              ></asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="120"
                              ><span style="color: red"
                                >Harga Reval</span
                              ></asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="120"
                              >Harga Promo</asp:TableHeaderCell
                            >
                          </asp:TableHeaderRow>
                        </asp:Table>
                      </div>
                      <div role="tabpanel" class="tab-pane" id="rv_sold">
                        <asp:Literal
                          runat="server"
                          ID="lit_rv_sold"
                        /><br /><br />
                        <asp:Table
                          ID="tbrv_sold"
                          runat="server"
                          CssClass="table-list"
                          Style="width: 100%;"
                        >
                          <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="50"
                              >No.</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="100"
                              >PLU</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="100"
                              >Item</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="100"
                              >Category</asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell>Location</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="120"
                              ><span style="color: blue"
                                >Harga Standard</span
                              ></asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="120"
                              ><span style="color: red"
                                >Harga Reval</span
                              ></asp:TableHeaderCell
                            >
                            <asp:TableHeaderCell Width="120"
                              >Harga Promo</asp:TableHeaderCell
                            >
                          </asp:TableHeaderRow>
                        </asp:Table>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </form>
  <%--
  <script>
    $(document).ready(function () {
      $("#<%=datepicker.ClientID%>").datepicker({
        format: "dd-mm-yyyy",
      });
    });
  </script>
  --%>
  <script>
    $(document).ready(function () {
      $("select").select2({ width: "100%" });
    });

    $(function () {
      $('[data-toggle="popover"]').popover((html = true));
    });

    $("#myTabs a").click(function (e) {
      e.preventDefault();
      $(this).tab("show");
    });

    var rdo1 = document.getElementById("content_rdo_loc_1");
    var rdow = document.getElementById("content_rdo_loc_w");

    $("#content_rdo_loc_1").on("click", function () {
      $("#content_ddl_location").prop("disabled", false);
    });

    $("#content_rdo_loc_w").on("click", function () {
      $("#content_ddl_location").prop("disabled", true);
    });
  </script>
</asp:Content>
