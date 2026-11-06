<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CouponPrizedraw.aspx.cs" Inherits="WS_CouponPrizedraw" %>

<%@ Register Src="~/Page/CouponPrizedraw/MenuCoupon.ascx" TagName="Menu" TagPrefix="uc1" %>

<!DOCTYPE html>
<script runat="server">

    protected void ddlCompanyBrand_TextChanged(object sender, EventArgs e)
    {

    }
</script>


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
    <link rel="stylesheet" href="/UI/CSS/jquery-ui.css" />

    <!-- JavaScript -->
    <script type="text/javascript" src="/UI/JS/jquery.js"></script>
    <script type="text/javascript" src="/UI/JS/bootstrap.js"></script>
    <script type="text/javascript" src="/UI/JS/datepicker.js"></script>
    <script type="text/javascript" src="/UI/JS/custom.js"></script>

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
            background-color: #5ee226;
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
            New
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
                        <td style="padding-left: 1em">
                            <input runat="server" type="text" id="txtTitle" class="form-control" maxlength="80" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 3em; padding-right: 1em">Date Start*</td>
                        <td>:</td>
                        <td style="padding-left: 1em">
                            <input runat="server" type="text" id="txtDateStart" class="form-control" readonly="true" style="background-color: #ffffff"/>
                        </td>

                        <td style="padding-left: 3em; padding-right: 1em">Date End*</td>
                        <td>:</td>
                        <td style="padding-left: 1em">
                            <input runat="server" type="text" id="txtDateEnd" class="form-control" readonly="true" style="background-color: #ffffff"/>
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
                            <asp:DropDownList runat="server" ID="ddlCompanyBrand" CssClass="form-control" OnSelectedIndexChanged="ddlCompanyBrand_TextChanged" />
                        </td>--%>

                        <td style="padding-left: 3em; padding-right: 1em">Location</td>
                        <td>:</td>
                        <td style="padding-left: 1em; text-align: left" colspan="2">
                            <asp:ListBox runat="server" ID="ddlLokasi" CssClass="form-control" SelectionMode="Single"></asp:ListBox>
                            <button runat="server" id="btnAddLoc" type="button" class="btn btn-success"><i class="glyphicon glyphicon-plus"></i></button>
                        </td>
                    </tr>
                </table>
                <div class="col-lg-8" style="padding-top: 1em; padding-left: 2em">
                    <input type="hidden" id="hfcountlocationtable" runat="server" value="0" /><!--Untuk Parsing Count Tabel Ke Server-->
                    <input type="hidden" id="hfjsonlocationtable" runat="server" />
                    <label style="margin: 0px,auto"><b>Table of Locations*</b></label>
                    <table id="tabledetaillocation" class="table table-bordered table-hover">
                        <tr>
                            <th style="display: none">IDBrand</th>
                            <th style="display: none">Brand</th>
                            <th>Location</th>
                            <th>Location Code</th>
                            <th></th>
                        </tr>
                        <tbody id="bodyLocationTable">
                        </tbody>
                    </table>
                </div>
            </div>
            <hr style="border: 1px solid thin" />
            <div class="form-group">
                <table>
                    <tr>
                        <td style="padding-left: 3em; padding-right: 1em">Payment Type</td>
                        <td>:</td>
                        <td style="padding-left: 1em">
                            <asp:DropDownList runat="server" ID="ddlPaymentType" CssClass="form-control" onchange="onchangePaymentType();"/>
                        </td>

                        <td style="padding-left: 3em; padding-right: 1em">Bank</td>
                        <td>:</td>
                        <td style="padding-left: 1em">
                            <asp:DropDownList runat="server" ID="ddlBank" CssClass="form-control" />
                        </td>

                        <td style="padding-left: 3em; padding-right: 1em">Program Cicilan</td>
                        <td>:</td>
                        <td style="padding-left: 1em">
                            <asp:DropDownList runat="server" ID="ddlProgramCicilan" CssClass="form-control" />
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
                                <input type="text" id="txtMultiple" class="form-control" runat="server" aria-label="Amount (to the nearest dollar)" style="height: 33px" />
                                <div class="input-group-addon">
                                    <span class="input-group-text">,00</span>
                                </div>
                            </div>
                        </td>

                        <td style="padding-left: 3em; padding-right: 1em">Coupon Received</td>
                        <td>:</td>
                        <td style="padding-left: 1em">
                            <input type="number" id="txtCouponReceived" runat="server" class="form-control" />
                        </td>
                        <td>
                            <button runat="server" type="button" id="btnAddPayment" class="btn btn-success"><i class="glyphicon glyphicon-plus"></i></button>
                        </td>
                    </tr>
                </table>
                <div class="col-lg-8" style="padding-top: 1em; padding-left: 2em">
                    <input type="hidden" id="hfcountpaymenttable" runat="server" value="0" />
                    <input type="hidden" id="hfjsonpaymenttable" runat="server" />
                    <label style="margin: 0px,auto"><b>Table of Payments*</b></label>
                    <table id="tabledetailpayment" class="table table-bordered table-hover">
                        <tr>
                            <th style="display: none">IDPayment Type</th>
                            <th>Payment Type</th>
                            <th style="display: none">IDBank Issuer</th>
                            <th>Bank</th>
                            <th style="display: none">IDProgram Cicilan</th>
                            <th>Program Cicilan</th>
                            <th>Multiple</th>
                            <th>Coupon Received</th>
                            <th></th>
                        </tr>
                        <tbody id="bodyPaymentTable">
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="form-group">
                <div style="padding-left: 2em">
                    <button runat="server" id="btnSave" type="button" class="btn btn-primary" onserverclick="btnSave_ServerClick">Save</button>
                    <button runat="server" id="btnSubmit" type="button" class="btn btn-primary" onserverclick="btnSubmit_ServerClick">Submit</button>
                    <button runat="server" id="btnCancel" type="button" class="btn btn-primary" onclick="onclickbtnCancel();">Cancel</button>
                </div>
            </div>
        </form>
    </div>

    <!--Javascript-->

    <script type="text/javascript">
        $(function () {
            $('#txtDateStart, #txtDateEnd').datepicker({
                format: "dd/mm/yyyy",
                //startDate: "01-01-2015",
                //endDate: "01-05-2022",
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
                //includeSelectAllOption: true
            });

            $('#ddlLokasi').change(function () {
                var textarea = $("#ddlLokasi").find(':selected').text();
                if (textarea != "") {
                    //alert(textarea);
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

                var programCicilanId = $('#ddlProgramCicilan').find(':selected').val();
                var programCicilan = $('#ddlProgramCicilan').find(':selected').text();

                var multiple = $('#txtMultiple').val();
                var couponrec = $('#txtCouponReceived').val();

                //console.log("1." + paymentType + ",2." + bankIssuer + ",3." + multiple + ",4." + couponrec);

                if (paymentId != "0" && bankIssuerId != "0" && multiple != "" && couponrec != "") {
                    drawPaymentTable(paymentId, paymentType, bankIssuerId, bankIssuer, programCicilanId, programCicilan, multiple, couponrec, "bodyPaymentTable");
                }
                else if (paymentId == "1" && multiple != "" && couponrec != "") {
                    drawPaymentTable(paymentId, paymentType, bankIssuerId, bankIssuer, programCicilanId, programCicilan, multiple, couponrec, "bodyPaymentTable");
                }
                else {
                    document.getElementById("hfcountpaymenttable").value = 0;
                    alert("Payment Type, Bank, Multipe, Couponrec must be filled");
                }
            });

            $("#bodyLocationTable, #bodyPaymentTable").on('click', '#btnDelete', function () {
                $(this).closest('tr').remove();
            });

            onchangePaymentType();
        });
        function onclickbtnCancel() {
            //window.location.href = "../Home.aspx";
            window.close();
        }
        function drawLocationTable(Idbrand, brand, location, locationcode, id) {
            var row = $("<tr>")
            row.append($("<td style='display:none'>" + Idbrand + "</td>"));
            row.append($("<td style='display:none'>" + brand + "</td>"));
            row.append($("<td>" + location + "</td>"));
            row.append($("<td>" + locationcode + "</td>"));
            row.append($("<td><button class='btn btn-default' type='button' id='btnDelete''>Delete</button class='btn btn-danger'>"))
            row.append("</tr>");
            $("#" + id).append(row); //this will append tr element to table... keep its reference for a while since we will add cells into it

            GetLocationTableValue();
        }
        function drawPaymentTable(paymentId, payment, bankIssuerId, bankIssuer, programCicilanId, programCicilan, multiple, couponrec, id) {
            var row = $("<tr>")
            row.append($("<td style='display:none'>" + paymentId + "</td>"))
            row.append($("<td>" + payment + "</td>"));
            row.append($("<td style='display:none'>" + bankIssuerId + "</td>"));
            row.append($("<td>" + bankIssuer + "</td>"));
            row.append($("<td style='display:none'>" + programCicilanId + "</td>"));
            row.append($("<td>" + programCicilan + "</td>"));
            row.append($("<td>" + multiple + "</td>"));
            row.append($("<td>" + couponrec + "</td>"));
            row.append($("<td><button class='btn btn-default' type='button' id='btnDelete''>Delete</button class='btn btn-danger'>"))
            row.append("</tr>");
            $("#" + id).append(row); //this will append tr element to table... keep its reference for a while since we will add cells into it

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
                payment.programCicilanId = row.cells[4].innerHTML.trim();
                payment.programCicilan = row.cells[5].innerHTML.trim();
                payment.multiple = row.cells[6].innerHTML.trim();
                payment.couponrec = row.cells[7].innerHTML.trim();
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
                $("#ddlProgramCicilan").val(1);

                $("#ddlBank").attr("disabled", "disabled");
                $("#ddlProgramCicilan").attr("disabled", "disabled");
            }
            else {
                $("#ddlBank").removeAttr("disabled");
                $("#ddlProgramCicilan").removeAttr("disabled");
            }
        }
    </script>
</body>
</html>
