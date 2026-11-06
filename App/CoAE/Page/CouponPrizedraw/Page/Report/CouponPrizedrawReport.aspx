<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CouponPrizedrawReport.aspx.cs" Inherits="WS_CouponPrizedrawApprove" %>

<%@ Register Src="~/Page/CouponPrizedraw/MenuCoupon.ascx" TagPrefix="uc1" TagName="Menu" %>

<!DOCTYPE html>
<script runat="server">

    protected void Unnamed_Click(object sender, EventArgs e)
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

    <!-- JavaScript -->
    <script type="text/javascript" src="/UI/JS/jquery.js"></script>
    <script type="text/javascript" src="/UI/JS/bootstrap.js"></script>
    <script type="text/javascript" src="/UI/JS/datepicker.js"></script>
    <script type="text/javascript" src="/UI/JS/custom.js"></script>

    <link rel="stylesheet" href="/UI/CSS/jquery-ui.css" />
    <%--<script src="/UI/JS/jquery-1.10.2.js"></script>--%>
    <script src="/UI/JS/jquery-ui.js"></script>

    <link href="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/css/bootstrap-multiselect.css" rel="stylesheet" type="text/css" />
    <script src="http://cdn.rawgit.com/davidstutz/bootstrap-multiselect/master/dist/js/bootstrap-multiselect.js" type="text/javascript"></script>
    <style>
        td {
            padding-top: 1em;
            padding-bottom: 1em;
            text-align: center;
        }

        th {
            background-color: rgba(233, 125, 125, 0.55);
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
            Report
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

        <form id="formcouponapproval" runat="server">
            <div class="form-group">
                <table>
                    <tr>
                        <td><asp:TextBox runat="server" CssClass="form-control" ID="txtSearch" Placeholder="Search" Width="300"></asp:TextBox></td>
                        <td><asp:Button runat="server" CssClass="btn btn-primary" Text="Display" ID="btnDisplay" OnClick="btnDisplay_Click" /></td>
                    </tr>
                </table>
            </div>
            <div class="form-group">
                <asp:SqlDataSource ID="CouponPrizedrawDataSource" runat="server"
                    ConnectionString="<%$ ConnectionStrings:Cnn_DB %>"
                    SelectCommand="SELECT ID, Title, DateStart,DateEnd, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, CASE WHEN [Status] = 0 THEN 'On Progress' WHEN [Status] = 1 THEN 'Submitted' WHEN [Status] = 2 THEN 'Approved' WHEN [Status] = 3 THEN 'Rejected' END [Status], CASE WHEN [DateEnd] < GETDATE() THEN 'Expired' ELSE 'Not Expired' END [Expired] FROM CouponPrizedraw"></asp:SqlDataSource>
                <asp:GridView runat="server" CssClass="table table-bordered table-striped" ID="gvCouponPrizedraw" DataSourceID="CouponPrizedrawDataSource" AutoGenerateColumns="false" DataKeyNames="ID" AllowSorting="true" OnRowDataBound="gvCouponPrizedraw_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="false" />
                        <asp:BoundField DataField="Title" HeaderText="Title" />
                        <asp:BoundField DataField="DateStart" HeaderText="Start" />
                        <asp:BoundField DataField="DateEnd" HeaderText="End" />
                        <asp:BoundField DataField="Status" HeaderText="Status" />
                        <asp:BoundField DataField="Expired" HeaderText="Expired" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" />
                        <asp:BoundField DataField="ModifiedBy" HeaderText="Modified By" />
                        <asp:BoundField DataField="ModifiedDate" HeaderText="Modified Date" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <button type="button" id="btnDetail" onclick="GoToUrl(this);" class="btn btn-default">Detail</button>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </form>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            //var rows = document.getElementById("gvCouponPrizedraw");
            $('#gvCouponPrizedraw td:nth-child(' + 4 + '),#gvCouponPrizedraw th:nth-child(' + 4 + ')').hide();
            $('#gvCouponPrizedraw td:nth-child(' + 8 + '),#gvCouponPrizedraw th:nth-child(' + 8 + ')').hide();
            $('#gvCouponPrizedraw td:nth-child(' + 10 + '),#gvCouponPrizedraw th:nth-child(' + 10 + ')').hide();
        });

        function GoToUrl(e) {
            var value = e.closest('tr').cells[0].innerHTML;
            window.location.href = '~/Page/CouponPrizedraw/Page/Report/CouponPrizedraw.aspx?id=' + value + '';
        }
    </script>
</body>
</html>
