<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestWeb.aspx.cs" Inherits="TestWeb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Test Web</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />

    <!-- favicon -->
    <link runat="server" id="link1" rel="apple-touch-icon" sizes="180x180" />
    <link runat="server" id="link2" rel="icon" type="image/png" />
    <link runat="server" id="link3" rel="icon" type="image/png" sizes="16x16" />
    <link runat="server" id="link4" rel="manifest" />
    <link runat="server" id="link5" rel="mask-icon" />
    <link runat="server" id="link6" rel="shortcut icon" />
    <meta runat="server" id="meta1" name="apple-mobile-web-app-title" />
    <meta runat="server" id="meta2" name="application-name" />
    <meta runat="server" id="meta3" name="theme-color" />

    <!-- CSS -->
    <link rel="stylesheet" href="/UI/CSS/bootstrap.css" />
    <link rel="stylesheet" href="/UI/CSS/bootstrap.font.css" />
    <link href="/UI/CSS/bootstrap.datepicker.css" rel="stylesheet" />
    <%--<link rel="stylesheet" href="/UI/CSS/dataTables.bootstrap.min.css" />--%>
    <link rel="stylesheet" href="/UI/CSS/custom.css" />
    <link rel="stylesheet" href="/UI/CSS/custom.portal.css" />
    <link href="/UI/CSS/jquery-ui.css" rel="stylesheet" />

    <asp:Literal runat="server" ID="lit_css_event" />

    <!-- JavaScript -->
    <script type="text/javascript" src="/UI/JS/jquery.js"></script>
    <script type="text/javascript" src="/UI/JS/bootstrap.js"></script>
    <script type="text/javascript" src="/UI/JS/datepicker.js"></script>
    <%--<script type="text/javascript" src="/UI/JS/dataTables.bootstrap.min.js"></script>--%>
    <script type="text/javascript" src="/UI/JS/custom.js"></script>
    <style>
        .datepicker {
            z-index: 1600 !important; /* has to be larger than 1050 */
        }
    </style>
</head>
<body>
    <nav class="navbar navbar-default navbar-fixed-top navbar-offset">
        <div class="container">
            <div class="navbar-header">
                <button type="button" class="navbar-toggle collapsed" data-toggle="collapse" data-target="#bs-example-navbar-collapse-1" aria-expanded="false" aria-controls="bs-example-navbar-collapse-1">
                    <span class="sr-only">Toggle navigation</span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                    <span class="icon-bar"></span>
                </button>
                <a class="navbar-brand" href="/Default.aspx">Home</a>
            </div>
            <div class="collapse navbar-collapse" id="bs-example-navbar-collapse-1">
                <ul class="nav navbar-nav">
                    <li class="dropdown">
                        <a href="#" class="dropdown-toggle" id="navbarDropdown" data-toggle="dropdown" role="button" aria-haspopup="true" aria-expanded="false">Action <span class="caret"></span></a>
                        <ul class="dropdown-menu" aria-labelledby="navbarDropdown">
                            <li><a href="#" id="btnSignOut">Sign Out</a></li>
                            <li><a href="#">Change Password</a></li>
                            <%--<li><a href="#">Another action</a></li>
                            <li><a href="#">Something else here</a></li>
                            <li role="separator" class="divider"></li>
                            <li><a href="#">Separated link</a></li>
                            <li role="separator" class="divider"></li>
                            <li><a href="#">One more separated link</a></li>--%>
                        </ul>
                    </li>
                </ul>
                <div></div>
                <%--<form class="navbar-form navbar-left text-center">
                    <div class="form-group">
                        <input type="text" class="form-control" placeholder="Search" />
                    </div>
                    <button type="button" class="btn btn-default">Submit</button>
                </form>--%>
                <div class="navbar-nav navbar-right" style="margin-bottom: 20px">
                    <img runat="server" id="img0" src="/Images/web/logo_web_mondial.png" class="img-responsive" border="0" style="max-height: 60px; margin: 0 auto;" visible="true" />
                    <img runat="server" id="img1" src="/Images/web/logo_web_missmondial.png" class="img-responsive" border="0" style="max-height: 60px; margin: 0 auto;" visible="true" />
                    <img runat="server" id="img2" src="/Images/web/logo_web_cmk_2017.jpg" class="img-responsive" border="0" style="max-height: 120px; margin: 0 auto;" visible="false" />
                </div>
            </div>
        </div>
    </nav>
    <div style="margin-top: 75px">
        <h1 class="text-center">Testing Untuk Filter Data</h1>
    </div>
    <hr />
    <form id="formtesting" runat="server" style="text-align: center">
        <div class="container">
            <div class="row">
                <div class="col-xs-3"></div>
                <div class="col-xs-6 text-center">
                    <table class="table table-bordered table-striped  text-center" id="myTable">
                        <thead>
                            <tr id="headerTable">
                                <th class="text-center">ID</th>
                                <th class="text-center">Name</th>
                                <th class="text-center">Email</th>
                                <th class="text-center">Action</th>
                            </tr>
                        </thead>
                        <tbody id="bodyTable">
                        </tbody>
                    </table>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-3"></div>
                <div class="col-xs-6">
                    <div class="form-group">
                        <div class="col-xs-0">
                            <button id="btnAdd" class="btn btn-info pull-left" type="button" data-action='add' data-toggle='modal' data-target='#myModalAdd' onclick='onclickbutton(this);'><i class="glyphicon glyphicon-plus"></i></button>
                        </div>
                        <div class="col-xs-1">
                            <div class="input-group">
                                <asp:TextBox ID="textSearch" CssClass="form-control" runat="server" Width="200"></asp:TextBox>
                                <span class="input-group-btn">
                                    <%--<asp:Button ID="btnSearch" CssClass="btn btn-success" Text="Display" runat="server" OnClick="btnSearch_Click"/>--%>
                                    <button runat="server" onserverclick="refreshPage" class="btn btn-success">Display</button>
                                </span>
                            </div>
                        </div>
                    </div>
                    <asp:Table ID="tb" runat="server" CssClass="table table-bordered text-center">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="40" ID="HeaderID" runat="server" CssClass="text-center">ID</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="40" CssClass="text-center">No.</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="90" CssClass="text-center">DataTable</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="90" CssClass="text-center">Tanggal</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100" CssClass="text-center">Author</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="100" CssClass="text-center">Nama File</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="250" CssClass="text-center">Action</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                    <button type="button" class="btn btn-link" runat="server" onclick="onclickcreatetext(this);" data-id="txt">Create txt file</button>
                    <button type="button" class="btn btn-link" runat="server" onclick="onclickcreatetext(this);" data-id="excel">Create excel file</button>
                    <span class="control-fileupload">
                        <input type="file" id="file" style="width:75px"/>
                    </span>
                    <button type="button" class="btn btn-link" runat="server">Upload</button>
                </div>
            </div>
        </div>
        <div class="container">
            <!-- Modal for Add-->
            <div class="modal fade" id="myModalAdd" role="dialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content for Add-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Add Data</h4>
                        </div>
                        <div class="modal-body">
                            <!--Create Your Html Body-->
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <label class="control-label col-xs-3">DataTable</label>
                                    <p class="control-label col-xs-2">:</p>
                                    <div class="col-xs-6">
                                        <input runat="server" type="text" id="txtDataTableAdd" class="form-control" placeholder="Must Fill This Record." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-3">Tanggal</label>
                                    <p class="control-label col-xs-2">:</p>
                                    <div class="col-xs-6">
                                        <input runat="server" type="text" id="txtdateAdd" class="form-control" />
                                        <input runat="server" type="text" id="txtTglAdd" class="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-3">Author</label>
                                    <p class="control-label col-xs-2">:</p>
                                    <div class="col-xs-6">
                                        <input runat="server" type="text" id="txtAuthorAdd" class="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-3">Nama File</label>
                                    <p class="control-label col-xs-2">:</p>
                                    <div class="col-xs-6">
                                        <input runat="server" type="text" id="txtNamaFileAdd" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" runat="server" class="btn btn-default" id="btnOKAdd" onserverclick="btnOKAdd_ServerClick">OK</button>
                            <button type="button" class="btn btn-default" data-dismiss="modal" onclick="onclickcanceladd();">Close</button>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Modal for Update-->
            <div class="modal fade" id="myModalUpdate" role="dialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content for Update-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Update Data</h4>
                        </div>
                        <div class="modal-body">
                            <!--Create Your Html Body-->
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <asp:HiddenField ID="idTable" runat="server"></asp:HiddenField>
                                    <label class="control-label col-xs-3">DataTable</label>
                                    <p class="control-label col-xs-2">:</p>
                                    <div class="col-xs-6">
                                        <input runat="server" type="text" id="txtDataTableUpdate" class="form-control" placeholder="Must Fill This Record." />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-3">Tanggal</label>
                                    <p class="control-label col-xs-2">:</p>
                                    <div class="col-xs-6">
                                        <input runat="server" type="text" id="txtdateUpdate" class="form-control" readonly="readonly" disabled="disabled" />
                                        <input runat="server" type="text" id="txtTglUpdate" class="form-control" readonly="readonly" disabled="disabled" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-3">Author</label>
                                    <p class="control-label col-xs-2">:</p>
                                    <div class="col-xs-6">
                                        <input runat="server" type="text" id="txtAuthorUpdate" class="form-control" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-3">Nama File</label>
                                    <p class="control-label col-xs-2">:</p>
                                    <div class="col-xs-6">
                                        <input runat="server" type="text" id="txtNamaFileUpdate" class="form-control" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" id="btnOKUpdate" runat="server" onserverclick="btnOKUpdate_ServerClick" data-dismiss="modal">OK</button>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- Modal for Delete-->
            <div class="modal fade" id="myModalDelete" role="dialog">
                <div class="modal-dialog modal-lg">

                    <!-- Modal content for Delete-->
                    <div class="modal-content">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal">&times;</button>
                            <h4 class="modal-title">Delete Data</h4>
                        </div>
                        <div class="modal-body">
                            <!--Create Your Html Body-->
                            <div class="form-horizontal">
                                <div class="form-group">
                                    <asp:HiddenField ID="idTable1" runat="server"></asp:HiddenField>
                                    <label class="control-label col-xs-3">DataTable</label>
                                    <p class="control-label col-xs-2">:</p>
                                    <div class="col-xs-6">
                                        <input runat="server" type="text" id="txtDataTableDelete" class="form-control" placeholder="Must Fill This Record." readonly="readonly" disabled="disabled" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-3">Tanggal</label>
                                    <p class="control-label col-xs-2">:</p>
                                    <div class="col-xs-6">
                                        <input runat="server" type="text" id="txtdateDelete" class="form-control" readonly="readonly" disabled="disabled" />
                                        <input runat="server" type="text" id="txtTglDelete" class="form-control" readonly="readonly" disabled="disabled" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-3">Author</label>
                                    <p class="control-label col-xs-2">:</p>
                                    <div class="col-xs-6">
                                        <input runat="server" type="text" id="txtAuthorDelete" class="form-control" readonly="readonly" disabled="disabled" />
                                    </div>
                                </div>
                                <div class="form-group">
                                    <label class="control-label col-xs-3">Nama File</label>
                                    <p class="control-label col-xs-2">:</p>
                                    <div class="col-xs-6">
                                        <input runat="server" type="text" id="txtNamaFileDelete" class="form-control" readonly="readonly" disabled="disabled" />
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" id="btnOKDelete" runat="server" onserverclick="btnDelete_ServerClick" data-dismiss="modal">OK</button>
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
        </asp:ScriptManager>
    </form>
    <hr />
    <footer class="container">
        <p id="DateTime" onload="updateClock();"></p>
    </footer>
    <script type="text/javascript">
        $(function () {
            setInterval(updateClock(), '1000');
            drawTable(data);

            $('#txtDatePicker').datepicker({
                format: "dd/mm/yyyy",
                startDate: "01-01-2015",
                endDate: "01-01-2020",
                todayBtn: "linked",
                autoclose: true,
                todayHighlight: true
            });

            $('#myModalUpdate').on('shown.bs.modal', function (e) {
                $("#txtdateUpdate,#txtdateAdd").datepicker({
                    format: "dd/mm/yyyy",
                    startDate: "01-01-2015",
                    endDate: "01-01-2020",
                    todayBtn: "linked",
                    autoclose: true,
                    todayHighlight: true
                });
            });
        });

        var data = [
            {
                id: 1,
                Nama: "Andri",
                Email: "andri@cmk.co.id"
            },
            {
                id: 2,
                Nama: "Priyono",
                Email: "priyono@cmk.co.id"
            }];

        function drawTable(data) {
            for (var i = 0; i < data.length; i++) {
                var row = $("<tr>")
                row.append($("<td>" + data[i].id + "</td>"));
                row.append($("<td>" + data[i].Nama + "</td>"));
                row.append($("<td>" + data[i].Email + "</td>"));
                row.append($("<td><button class='btn btn-default' id='btnEdit" + i + "' type='button'>Edit</button class='btn btn-default'><button class='btn btn-default' type='button'>Cancel</button></td>"))
                row.append("</tr>");
                $("#bodyTable").append(row); //this will append tr element to table... keep its reference for a while since we will add cels into it
            }
        }

        //for tic toc clock
        function updateClock() {
            var currentTime = new Date();
            var currentHours = currentTime.getHours();
            var currentMinutes = currentTime.getMinutes();
            var currentSeconds = currentTime.getSeconds();

            // Pad the minutes and seconds with leading zeros, if required
            currentMinutes = (currentMinutes < 10 ? "0" : "") + currentMinutes;
            currentSeconds = (currentSeconds < 10 ? "0" : "") + currentSeconds;

            // Choose either "AM" or "PM" as appropriate
            var timeOfDay = (currentHours < 12) ? "AM" : "PM";

            // Convert the hours component to 12-hour format if needed
            currentHours = (currentHours > 12) ? currentHours - 12 : currentHours;

            // Convert an hours component of "0" to "12"
            currentHours = (currentHours == 0) ? 12 : currentHours;

            // Compose the string for display
            var currentTimeString = currentHours + "." + currentMinutes + "." + currentSeconds + " " + timeOfDay;

            $("#DateTime").html(currentTimeString);
            var t = setTimeout(updateClock, 500);
        }
        function onclickbutton(e) {
            if (e.id == 'btnAdd') {
                var d = new Date();
                var day = (d.getDate() < 10 ? "0" : "") + d.getDate();
                var month = (parseInt(d.getMonth()) + 1 < 10 ? "0" : "") + (parseInt(d.getMonth()) + 1).toString();
                var year = d.getFullYear();
                var Tanggal = day + "/" + month + "/" + year;
                $("#txtdateAdd").val(Tanggal);
                $("#txtTglAdd").val($("#DateTime").html());
            }
            else if (e.id == 'btnDelete') {
                var id = e.getAttribute("data-id");
                var tbl = document.getElementById("tb");
                if (tbl != null) {
                    $("#txtDataTableDelete").val(tbl.rows[e.parentNode.parentNode.rowIndex].cells[2].innerHTML);
                    $("#txtdateDelete").val(tbl.rows[e.parentNode.parentNode.rowIndex].cells[3].innerHTML.split(" ")[0]);
                    $("#txtTglDelete").val(tbl.rows[e.parentNode.parentNode.rowIndex].cells[3].innerHTML.split(" ")[1]);
                    $("#txtAuthorDelete").val(tbl.rows[e.parentNode.parentNode.rowIndex].cells[4].innerHTML);
                    $("#txtNamaFileDelete").val(tbl.rows[e.parentNode.parentNode.rowIndex].cells[5].innerHTML);
                }
                $("#idTable1").val(id);
            }
            else {
                var id = e.getAttribute("data-id");
                var tbl = document.getElementById("tb");
                if (tbl != null) {
                    $("#txtDataTableUpdate").val(tbl.rows[e.parentNode.parentNode.rowIndex].cells[2].innerHTML);
                    $("#txtdateUpdate").val(tbl.rows[e.parentNode.parentNode.rowIndex].cells[3].innerHTML.split(" ")[0]);
                    $("#txtTglUpdate").val(tbl.rows[e.parentNode.parentNode.rowIndex].cells[3].innerHTML.split(" ")[1]);
                    $("#txtAuthorUpdate").val(tbl.rows[e.parentNode.parentNode.rowIndex].cells[4].innerHTML);
                    $("#txtNamaFileUpdate").val(tbl.rows[e.parentNode.parentNode.rowIndex].cells[5].innerHTML);
                }
                $("#idTable").val(id);
            }
        }

        function onclickcreatetext(e) {
            if ('Blob' in window) {
                var fileName = prompt('Please enter file name to save', (e.getAttribute('data-id') == 'txt') ? 'Untitled.txt' : 'Untitled.xls');
                if (fileName) {
                    //var textToWrite = JSON.stringify(data);
                    var textToWrite = "";
                    var tbl = document.getElementById("tb");
                    var lengthData = tbl.rows.length;
                    if (lengthData > 0) {
                        for (i = 0; i < lengthData; i++) {
                            var lengthCol = tbl.rows.item(i).cells.length;
                            for (j = 1; j < lengthCol - 1; j++) {
                                textToWrite = textToWrite + tbl.rows[i].cells[j].innerHTML + '|';
                            }
                            textToWrite = textToWrite.substring(0, textToWrite.length - 1) + '\r\n';
                        }
                    }
                    alert(textToWrite);
                    if (e.getAttribute('data-id') != 'txt')
                        textToWrite = tbl.outerHTML;

                    var typeFile = (e.getAttribute('data-id') == 'txt') ? 'text/plain' : 'data:application/vnd.ms-excel';
                    var textFileAsBlob = new Blob([textToWrite], { type: typeFile });

                    if ('msSaveOrOpenBlob' in navigator) {
                        navigator.msSaveOrOpenBlob(textFileAsBlob, fileName);
                    } else {
                        var downloadLink = document.createElement('a');
                        downloadLink.download = fileName;
                        downloadLink.innerHTML = 'Download File';

                        if ('webkitURL' in window) {
                            // Chrome allows the link to be clicked without actually adding it to the DOM.
                            downloadLink.href = window.webkitURL.createObjectURL(textFileAsBlob);
                        } else {
                            // Firefox requires the link to be added to the DOM before it can be clicked.
                            downloadLink.href = window.URL.createObjectURL(textFileAsBlob);
                            downloadLink.click(function () {
                                document.body.removeChild(event.target);
                            });

                            downloadLink.style.display = 'none';
                            document.body.appendChild(downloadLink);
                        }
                        downloadLink.click();
                    }
                }
            } else {
                alert('Your browser does not support the HTML5 Blob.');
            }
        }
        function WriteToFile(passForm) {
            var fso = new ActiveXObject("Scripting.FileSystemObject");
            var s = fso.CreateTextFile("C:\\Test.txt", true);
            s.WriteLine(document.passForm.input.value);
            s.Close();
        }
        function onclickcanceladd() {
            $("#txtDataTableAdd").val("");
            $("#txtdateAdd").val("");
            $("#txtTglAdd").val("");
            $("#txtAuthorAdd").val("");
            $("#txtNamaFileAdd").val("");
        }
        function confirmEvent(e) {
            if (confirm("Are you sure?")) {
                return true;
            }
            else {
                alert("I'm not sure!");
                return false;
            }
        }
    </script>
</body>
</html>
