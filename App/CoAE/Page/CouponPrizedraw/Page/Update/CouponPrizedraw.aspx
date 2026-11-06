<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CouponPrizedraw.aspx.cs" Inherits="WS_CouponPrizedraw" %>

<%@ Register Src="~/Page/CouponPrizedraw/MenuCoupon.ascx" TagName="Menu" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <!-- CSS -->
    <link rel="stylesheet" href="/UI/CSS/bootstrap.css" />
    <link rel="stylesheet" href="/UI/CSS/bootstrap.datepicker.css" />
    <link rel="stylesheet" href="/UI/CSS/bootstrap.font.css" />
    <link rel="stylesheet" href="/UI/CSS/custom.css" />
    <link href="/UI/CSS/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />

    <!-- JavaScript -->
    <script type="text/javascript" src="/UI/JS/jquery.js"></script>
    <script type="text/javascript" src="/UI/JS/bootstrap.js"></script>
    <script type="text/javascript" src="/UI/JS/datepicker.js"></script>
    <script type="text/javascript" src="/UI/JS/custom.js"></script>

    <link rel="stylesheet" href="/UI/CSS/jquery-ui.css" />
    <%--<script src="/UI/JS/jquery-1.10.2.js"></script>--%>
    <script src="/UI/JS/jquery-ui.js"></script>
    <script src="/UI/JS/bootstrap-multiselect.js" type="text/javascript"></script>

    <style>
        td {
            padding-top: 1em;
            padding-bottom: 1em;
            text-align: center;
        }

        th {
            background-color: #ffd800;
            text-align: center
        }
    </style>
</head>
<body>
    <!-- Menu -->
    <div class="navbar navbar-default navbar-fixed-top">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
            </div>
            <div class="collapse navbar-collapse">
                <uc1:Menu ID="menu" runat="server" />
                <ul class="nav navbar-nav navbar-right">
                    <li>
                        <a runat="server" href="~/Home.aspx">
                            <span class="visible-sm visible-md visible-lg"><i class="icon-home icon-large" title="Homepage"></i></span>
                            <span class="visible-xs">Homepage</span>
                        </a>
                    </li>
                    <li>
                        <a runat="server" href="~/?so=2">
                            <span class="visible-sm visible-md visible-lg"><i class="icon-signout icon-large" title="Sign-Out"></i></span>
                            <span class="visible-xs">Sign-Out</span>
                        </a>
                    </li>
                </ul>
            </div>
        </div>
    </div>

    <!-- Content -->
    <div class="container">
        <h1 style="padding-top: 1.5em">
            <small style="color: orange">Coupon Prizedown
            </small>
            <br />
            Update
        </h1>
        <!-- Sistem Notifikasi -->
        <div id="callout" runat="server" class="alert alert-success">
            <i class="icon-spinner icon-spin icon-large"></i>
            Proses Berhasil
        </div>
        <asp:Literal ID="callout_script" runat="server" Visible="false">
		<script type="text/javascript">
            $('#callout').show();
            $('#callout').fadeOut(5000);
		</script>
        </asp:Literal>

        <form id="formcoupon" runat="server" class="form-horizontal">
            <div class="form-group">
                <table>
                    <tr>
                        <td style="padding-left: 3em; padding-right: 1em">Title*</td>
                        <td>:</td>
                        <td style="padding-left: 1em" colspan="6">
                            <input runat="server" type="text" id="txtTitle" class="form-control" maxlength="80" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 3em; padding-right: 1em">Date Start*</td>
                        <td>:</td>
                        <td style="padding-left: 1em">
                            <input runat="server" type="text" id="txtDateStart" class="form-control" readonly="true" style="background-color: #ffffff" />
                        </td>

                        <td style="padding-left: 3em; padding-right: 1em">Date End*</td>
                        <td>:</td>
                        <td style="padding-left: 1em">
                            <input runat="server" type="text" id="txtDateEnd" class="form-control" readonly="true" style="background-color: #ffffff" />
                        </td>
                    </tr>
                </table>
            </div>
            <hr style="border: 0.5px solid thin" />
            <div class="form-group">
                <table>
                    <tr>
                        <%--<td style="padding-left: 3em; padding-right: 1em">Brand</td>
                        <td>:</td>
                        <td style="padding-left: 1em">
                            <asp:DropDownList runat="server" ID="ddlCompanyBrand" CssClass="form-control" Style="background-color: #ffffff" />
                        </td>--%>

                        <td style="padding-left: 3em; padding-right: 1em">Location</td>
                        <td>:</td>
                        <td style="padding-left: 1em;text-align:left"">
                            <asp:ListBox runat="server" ID="ddlLokasi" CssClass="form-control" SelectionMode="Single" Style="background-color: #ffffff"></asp:ListBox>
                            <button runat="server" id="btnAddLoc" type="button" class="btn btn-success" onserverclick="btnAddLoc_ServerClick"><i class="glyphicon glyphicon-plus"></i></button>
                        </td>
                    </tr>
                </table>
                <div class="col-lg-8" style="padding-top: 1em; padding-left: 2em">
                    <input type="hidden" id="hfcountlocationtable" runat="server" value="0" /><!--Untuk Parsing Count Tabel Ke Server-->
                    <input type="hidden" id="hfjsonlocationtable" runat="server" />
                    <label style="margin: 0px,auto; text-align: center"><b>Table of Locations*</b></label>
                    <asp:GridView runat="server" ID="gvLocations" CssClass="table table-bordered"
                        OnRowEditing="gvLocations_RowEditing" OnRowCancelingEdit="gvLocations_RowCancelingEdit"
                        OnRowDeleting="gvLocations_RowDeleting" OnRowUpdating="gvLocations_RowUpdating" AutoGenerateColumns="false" DataKeyNames="ID">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" />
                            <asp:TemplateField HeaderText="IDBrand" ControlStyle-Width="200px">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtIDBrandEdit" Text='<%#Eval("IDBrand") %>' runat="server" CssClass="form-control" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIDBrand" Text='<%#Eval("IDBrand") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="newIDBrand" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvSubjectName" runat="server" Text="*" ControlToValidate="newSubjectName" ValidationGroup="vgAdd" ForeColor="Red" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Nama Brand" ControlStyle-Width="200px">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlNamaBrand" runat="server" CssClass="form-control" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNamaBrand" Text='<%#Eval("NamaBrand") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="newNamaBrand" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvSubjectName" runat="server" Text="*" ControlToValidate="newSubjectName" ValidationGroup="vgAdd" ForeColor="Red" />
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Location" ControlStyle-Width="100px">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlNamaLokasi" runat="server" CssClass="form-control" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblNamaLokasi" runat="server" Text='<%#Eval("NamaLokasi") %>' CssClass="stytext" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="newLocCode" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvMarks" runat="server" Text="*" ControlToValidate="newMarks" ForeColor="Red" ValidationGroup="vgAdd" />
                                    <%--<asp:RegularExpressionValidator ID="revMarks" runat="server" ForeColor="Red" ControlToValidate="newMarks" ValidationExpression="^[0-9]*$" Text="*Numbers" ValidationGroup="vgAdd" />--%>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Location Code" ControlStyle-Width="100px">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtLocCode" runat="server" Text='<%#Eval("LocationCode") %>' CssClass="form-control" ReadOnly="true" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblLocCode" runat="server" Text='<%#Eval("LocationCode") %>' CssClass="stytext" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="newLocCode" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvMarks" runat="server" Text="*" ControlToValidate="newMarks" ForeColor="Red" ValidationGroup="vgAdd" />
                                    <%--<asp:RegularExpressionValidator ID="revMarks" runat="server" ForeColor="Red" ControlToValidate="newMarks" ValidationExpression="^[0-9]*$" Text="*Numbers" ValidationGroup="vgAdd" />--%>
                                </FooterTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Actions" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ControlStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-info"></asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?')" CausesValidation="false" CssClass="btn btn-info"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update" CssClass="btn btn-info"></asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-info"></asp:LinkButton>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton ID="btnAdd" runat="server" CommandName="Add" Text="Add New" ValidationGroup="vgAdd" CssClass="btn btn-info" />
                                </FooterTemplate>
                            </asp:TemplateField>

                        </Columns>
                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                        <%--<HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                        <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                        <RowStyle BackColor="White" ForeColor="#003399" />
                        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                        <SortedAscendingCellStyle BackColor="#EDF6F6" />
                        <SortedAscendingHeaderStyle BackColor="#0D4AC4" />
                        <SortedDescendingCellStyle BackColor="#D6DFDF" />
                        <SortedDescendingHeaderStyle BackColor="#002876" />--%>
                    </asp:GridView>
                </div>
            </div>
            <hr style="border: 1px solid thin" />
            <div class="form-group">
                <table>
                    <tr>
                        <td style="padding-left: 3em; padding-right: 1em">Payment Type</td>
                        <td>:</td>
                        <td style="padding-left: 1em">
                            <asp:DropDownList runat="server" ID="ddlPaymentType" CssClass="form-control" Style="background-color: #ffffff" onchange="onchangePaymentType();"/>
                        </td>

                        <td style="padding-left: 3em; padding-right: 1em">Bank</td>
                        <td>:</td>
                        <td style="padding-left: 1em">
                            <asp:DropDownList runat="server" ID="ddlBank" CssClass="form-control" />
                        </td>

                        <td style="padding-left: 3em; padding-right: 1em">Program Cicilan</td>
                        <td>:</td>
                        <td style="padding-left: 1em">
                            <asp:DropDownList runat="server" ID="ddlProgramCicilan" CssClass="form-control"/>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 3em; padding-right: 1em">Multiple</td>
                        <td>:</td>
                        <td style="padding-left: 1em">
                            <div class="input-group">
                                <div class="input-group-addon">
                                    <span class="input-group-text">Rp</span>
                                </div>
                                <input type="number" id="txtMultiple" class="form-control" runat="server" aria-label="Amount (to the nearest rupiah)" style="height: 33px" min="0" oninput="validity.valid||(value='');" />
                                <div class="input-group-addon">
                                    <span class="input-group-text">,00</span>
                                </div>
                            </div>
                        </td>

                        <td style="padding-left: 3em; padding-right: 1em">Coupon Received</td>
                        <td>:</td>
                        <td style="padding-left: 1em">
                            <input type="number" id="txtCouponReceived" runat="server" class="form-control" min="1" oninput="validity.valid||(value='');" />
                        </td>
                        <td>
                            <button runat="server" type="button" id="btnAddPayment" class="btn btn-success" onserverclick="btnAddPayment_ServerClick"><i class="glyphicon glyphicon-plus"></i></button>
                        </td>
                    </tr>
                </table>
                <div class="col-lg-8" style="padding-top: 1em; padding-left: 2em">
                    <input type="hidden" id="hfcountpaymenttable" runat="server" value="0" />
                    <input type="hidden" id="hfjsonpaymenttable" runat="server" />
                    <label style="margin: 0px,auto"><b>Table of Payments*</b></label>
                    <asp:GridView runat="server" ID="gvPayment" CssClass="table table-bordered" AutoGenerateColumns="false" OnRowEditing="gvPayment_RowEditing" OnRowCancelingEdit="gvPayment_RowCancelingEdit"
                        OnRowDeleting="gvPayment_RowDeleting" OnRowUpdating="gvPayment_RowUpdating" DataKeyNames="ID">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderText="ID" />
                            <asp:TemplateField HeaderText="ID Payment Type" ControlStyle-Width="200px">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtIDPaymentType" Text='<%#Eval("IDPaymentType") %>' runat="server" CssClass="form-control" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIDPaymentType" Text='<%#Eval("IDPaymentType") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="newIDPaymentType" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvSubjectName" runat="server" Text="*" ControlToValidate="newSubjectName" ValidationGroup="vgAdd" ForeColor="Red" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Payment Type" ControlStyle-Width="200px">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlPaymentTypeEdit" runat="server" CssClass="form-control" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPaymentType" Text='<%#Eval("NamaPayment") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="newPaymentType" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvSubjectName1" runat="server" Text="*" ControlToValidate="newSubjectName" ValidationGroup="vgAdd" ForeColor="Red" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID Bank Issuer" ControlStyle-Width="200px">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtIDBankIssuer" Text='<%#Eval("IDBankIssuer") %>' runat="server" CssClass="form-control" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIDBankIssuer" Text='<%#Eval("IDBankIssuer") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="newIDBankIssuer" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvSubjectName2" runat="server" Text="*" ControlToValidate="newSubjectName" ValidationGroup="vgAdd" ForeColor="Red" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Bank" ControlStyle-Width="100px">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlBankIssuer" runat="server" CssClass="form-control" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBankIssuer" Text='<%#Eval("NamaBank") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="newBankIssuer" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvSubjectName3" runat="server" Text="*" ControlToValidate="newSubjectName" ValidationGroup="vgAdd" ForeColor="Red" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID Program Cicilan" ControlStyle-Width="200px">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtIDProgramCicilan" Text='<%#Eval("IDProgramCicilan") %>' runat="server" CssClass="form-control" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIDProgramCicilan" Text='<%#Eval("IDProgramCicilan") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="newIDProgramCicilan" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvSubjectName2" runat="server" Text="*" ControlToValidate="newSubjectName" ValidationGroup="vgAdd" ForeColor="Red" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Program Cicilan" ControlStyle-Width="100px">
                                <EditItemTemplate>
                                    <asp:DropDownList ID="ddlProgramCicilanEdit" runat="server" CssClass="form-control" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblProgramCicilan" Text='<%#Eval("NamaProgramCicilan") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="newProgramCicilan" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvSubjectName3" runat="server" Text="*" ControlToValidate="newSubjectName" ValidationGroup="vgAdd" ForeColor="Red" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Multiple" ControlStyle-Width="100px">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtMultiple" Text='<%#Eval("Multiple") %>' runat="server" CssClass="form-control" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblMultiple" Text='<%#Eval("Multiple") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="newMultiple" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvSubjectName4" runat="server" Text="*" ControlToValidate="newSubjectName" ValidationGroup="vgAdd" ForeColor="Red" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Coupon Received" ControlStyle-Width="100px">
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtCouponReceived" Text='<%#Eval("CouponReceived") %>' runat="server" CssClass="form-control" />
                                </EditItemTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCouponReceived" Text='<%#Eval("CouponReceived") %>' runat="server" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:TextBox ID="newCouponReceived" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvSubjectName5" runat="server" Text="*" ControlToValidate="newSubjectName" ValidationGroup="vgAdd" ForeColor="Red" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Actions" FooterStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ControlStyle-Width="100px">
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnEdit" runat="server" CommandName="Edit" Text="Edit" CssClass="btn btn-info"></asp:LinkButton>
                                    <asp:LinkButton ID="btnDelete" runat="server" CommandName="Delete" Text="Delete" OnClientClick="return confirm('Are you sure you want to delete this record?')" CausesValidation="false" CssClass="btn btn-info"></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:LinkButton ID="btnUpdate" runat="server" CommandName="Update" Text="Update" CssClass="btn btn-info"></asp:LinkButton>
                                    <asp:LinkButton ID="btnCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="btn btn-info"></asp:LinkButton>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <asp:LinkButton runat="server" CommandName="Add" Text="Add New" ValidationGroup="vgAdd" />
                                </FooterTemplate>
                            </asp:TemplateField>

                        </Columns>
                    </asp:GridView>
                </div>
            </div>
            <div class="form-group">
                <div style="padding-left: 1.5em">
                    <button runat="server" id="btnSave" type="button" class="btn btn-primary" onserverclick="btnSave_ServerClick">Save</button>
                    <button runat="server" id="btnSubmit" type="button" class="btn btn-primary" onserverclick="btnSubmit_ServerClick">Submit</button>
                    <button runat="server" id="btnCancel" type="button" class="btn btn-primary" onclick="onclickbtnCancel();">Cancel</button>
                </div>
                <input type="hidden" id="bankIssuerBefore"/>
                <input type="hidden" id="programCicilanBefore"/>
            </div>
        </form>
    </div>

    <!--Javascript-->

    <script type="text/javascript">
        $(function () {
            $('#txtDateStart, #txtDateEnd').datepicker({
                format: "dd/MM/yy",
                startDate: new Date().getDate(),
                //endDate: "30/05/2019",
                todayBtn: "linked",
                autoclose: true,
                todayHighlight: true
            });

            $('#ddlLokasi').multiselect({
                includeSelectAllOption: true,
                enableCaseInsensitiveFiltering: true,
                numberDisplayed: 0,
                nonSelectedText: 'Select Facility',
                maxHeight: 350
            });

            $('#ddlLokasi').change(function () {
                var textarea = $("#ddlLokasi").find(':selected').text();
                if (textarea != "") {
                    //alert(textarea);
                }
            });

            $('[id^=gvLocations_ddlNamaLokasi]').change(function () {
                var textarea = $(this).find(':selected').val();
                $('[id^=gvLocations_txtLocCode]').val(textarea);
            });

            $('[id^=gvLocations_ddlNamaBrand]').change(function () {
                var textarea = $(this).find(':selected').val();
                $('[id^=gvLocations_txtIDBrandEdit]').val(textarea);
            });

            $('[id^=gvPayment_ddlPaymentTypeEdit]').change(function () {
                var textarea = $(this).find(':selected').text();
                if (textarea == "Cash" || textarea == "None Selected") {
                    $('[id^=gvPayment_ddlBankIssuer]').attr('readonly', 'readonly');
                    $('[id^=gvPayment_ddlProgramCicilanEdit]').attr('readonly', 'readonly');

                    $('#bankIssuerBefore').val($('[id^=gvPayment_ddlBankIssuer]').val());
                    $('#programCicilanBefore').val($('[id^=gvPayment_ddlProgramCicilanEdit]').val());

                    $('[id^=gvPayment_ddlBankIssuer]').val(0);
                    $('[id^=gvPayment_ddlProgramCicilanEdit]').val(1);
                }
                else {
                    $('[id^=gvPayment_ddlBankIssuer]').removeAttr('readonly');
                    $('[id^=gvPayment_ddlProgramCicilanEdit]').removeAttr('readonly');

                    if (textarea != "Cash" && ($('#bankIssuerBefore').val() != "" || $('#programCicilanBefore').val() != "")) {
                        $('[id^=gvPayment_ddlBankIssuer]').val($('#bankIssuerBefore').val());
                        $('[id^=gvPayment_ddlProgramCicilanEdit]').val($('#programCicilanBefore').val());
                    }
                }
            });

            $('#btnAddLoc').click(function () {
                //var brandId = $('#ddlCompanyBrand').val();
                //var brand = $('#ddlCompanyBrand').text();

                //var locations = $('#ddlLokasi').find(':selected').text();
                var kodeloc = $('#ddlLokasi').find(':selected').val();

                var arrayList = kodeloc.split("--");
                //console.log(brandId + "-" + brand + "-" + locations + "-" + kodeloc);

                if (arrayList[0] != "0" && arrayList[1] != "None Selected" && arrayList[2] != "0") {
                    drawLocationTable(arrayList[0], arrayList[0], arrayList[1], arrayList[2], "bodyLocationTable");
                }
                else {
                    document.getElementById("hfcountlocationtable").value = 0;
                    alert("Brand or Location must be filled");
                }
            });

            $('#btnAddPayment').click(function () {
                var paymentId = $('#ddlPaymentType').find(':selected').val();
                var paymentType = $('#ddlPaymentType').find(':selected').text();

                var bankIssuerId = $('#ddlBank').find(':selected').val();
                var bankIssuer = $('#ddlBank').find(':selected').text();

                var multiple = $('#txtMultiple').val();
                var couponrec = $('#txtCouponReceived').val();

                //console.log("1." + paymentType + ",2." + bankIssuer + ",3." + multiple + ",4." + couponrec);

                if (paymentId != null && bankIssuerId != null && multiple != "" && couponrec != "") {
                    drawPaymentTable(paymentId, paymentType, bankIssuerId, bankIssuer, multiple, couponrec, "bodyPaymentTable");
                }
                else {
                    document.getElementById("hfcountpaymenttable").value = 0;
                    alert("Payment Type, Bank, Multipe, Coupon Received must be filled");
                }
            });

            $("#bodyLocationTable, #bodyPaymentTable").on('click', '#btnDelete', function () {
                $(this).closest('tr').remove();
            });

            //Hide Column ID
            $('#gvLocations td:nth-child(' + 1 + '),#gvLocations th:nth-child(' + 1 + ')').hide();
            $('#gvLocations td:nth-child(' + 2 + '),#gvLocations th:nth-child(' + 2 + ')').hide();
            $('#gvLocations td:nth-child(' + 3 + '),#gvLocations th:nth-child(' + 3 + ')').hide();

            $('#gvPayment td:nth-child(' + 1 + '),#gvPayment th:nth-child(' + 1 + ')').hide();
            $('#gvPayment td:nth-child(' + 2 + '),#gvPayment th:nth-child(' + 2 + ')').hide();
            $('#gvPayment td:nth-child(' + 4 + '),#gvPayment th:nth-child(' + 4 + ')').hide();
            $('#gvPayment td:nth-child(' + 6 + '),#gvPayment th:nth-child(' + 6 + ')').hide();

            onchangePaymentType();
        });
        function onclickbtnCancel() {
            //window.location.href = "../Home.aspx";
            window.close();
        }
        function drawLocationTable(Idbrand, brand, location, locationcode, id) {
            var row = $("<tr>")
            row.append($("<td style='display:none'>" + Idbrand + "</td>"));
            row.append($("<td>" + brand + "</td>"));
            row.append($("<td>" + location + "</td>"));
            row.append($("<td>" + locationcode + "</td>"));
            row.append($("<td><button class='btn btn-default' type='button' id='btnDelete''>Delete</button class='btn btn-danger'>"))
            row.append("</tr>");
            $("#" + id).append(row); //this will append tr element to table... keep its reference for a while since we will add cels into it

            GetLocationTableValue();
        }
        function drawPaymentTable(paymentId, payment, bankIssuerId, bankIssuer, multiple, couponrec, id) {
            var row = $("<tr>")
            row.append($("<td style='display:none'>" + paymentId + "</td>"))
            row.append($("<td>" + payment + "</td>"));
            row.append($("<td style='display:none'>" + bankIssuerId + "</td>"));
            row.append($("<td>" + bankIssuer + "</td>"));
            row.append($("<td>" + multiple + "</td>"));
            row.append($("<td>" + couponrec + "</td>"));
            row.append($("<td><button class='btn btn-default' type='button' id='btnDelete''>Delete</button class='btn btn-danger'>"))
            row.append("</tr>");
            $("#" + id).append(row); //this will append tr element to table... keep its reference for a while since we will add cels into it

            GetPaymentTableValue();
        }
        function GetLocationTableValue() {
            var locations = new Array();

            //Reference the Table.
            var table = document.getElementById("tabledetaillocation");

            //Loop through Table Rows.
            for (var i = 1; i < table.rows.length; i++) {
                //Reference the Table Row.
                var row = table.rows[i];

                //Copy values from Table Cell to JSON object.
                var location = {};
                location.IDBrand = row.cells[0].innerHTML.trim();
                location.Brand = row.cells[1].innerHTML.trim();
                location.Location = row.cells[2].innerHTML.trim();
                location.LocCode = row.cells[3].innerHTML.trim();
                locations.push(location);
            }

            //Convert the JSON object to string and assign to Hidden Field.
            document.getElementById("hfjsonlocationtable").value = JSON.stringify(locations);
            document.getElementById("hfcountlocationtable").value = table.rows.length - 1;
            console.log($('#hfcountlocationtable').val() + "-" + $('#hfjsonlocationtable').val());
        }
        function GetPaymentTableValue() {
            var payments = new Array();

            //Reference the Table.
            var table = document.getElementById("tabledetailpayment");

            //Loop through Table Rows.
            for (var i = 1; i < table.rows.length; i++) {
                //Reference the Table Row.
                var row = table.rows[i];

                //Copy values from Table Cell to JSON object.
                var payment = {};
                payment.paymentId = row.cells[0].innerHTML.trim();
                payment.payment = row.cells[1].innerHTML.trim();
                payment.bankIssuerId = row.cells[2].innerHTML.trim();
                payment.bankIssuer = row.cells[3].innerHTML.trim();
                payment.multiple = row.cells[4].innerHTML.trim();
                payment.couponrec = row.cells[5].innerHTML.trim();
                payments.push(payment);
            }

            //Convert the JSON object to string and assign to Hidden Field.
            document.getElementById("hfjsonpaymenttable").value = JSON.stringify(payments);
            document.getElementById("hfcountpaymenttable").value = table.rows.length - 1;
            console.log($('#hfcountpaymenttable').val() + "-" + $('#hfjsonpaymenttable').val());
        }

        function onchangePaymentType() {
            //console.log(e.find('option:selected').innerHTML);
            if ($('#ddlPaymentType').find(':selected').text() == "Cash" || $('#ddlPaymentType').find(':selected').text() == "None Selected") {
                $("#ddlBank").val(0);
                $("#ddlProgramCicilan").val(0);

                $("#ddlBank").attr("disabled", "disabled");
                $("#ddlProgramCicilan").attr("disabled", "disabled");
            }
            else {
                $("#ddlBank").removeAttr("disabled");
                $("#ddlProgramCicilan").removeAttr("disabled");
            }

            if ($('#ddlPaymentType').find(':selected').text() == "None Selected") 
                $('#btnAddPayment').prop('disabled',true);
            else
                $('#btnAddPayment').prop('disabled',false);
        }
    </script>
</body>
</html>
