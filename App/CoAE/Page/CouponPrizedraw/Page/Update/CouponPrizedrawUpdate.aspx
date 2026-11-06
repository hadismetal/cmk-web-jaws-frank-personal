<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CouponPrizedrawUpdate.aspx.cs" Inherits="WS_Page_Update_CouponPrizedrawUpdate" %>

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
            background-color: #2892de;
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
        <form id="form1" runat="server">
            <div class="form-group">
                <table>
                    <tr>
                        <td><asp:TextBox runat="server" CssClass="form-control" ID="txtSearch" Placeholder="Search" Width="300"></asp:TextBox></td>
                        <td><asp:Button runat="server" CssClass="btn btn-primary" Text="Display" ID="btnDisplay" OnClick="btnDisplay_Click" /></td>
                        <td><asp:Button runat="server" CssClass="btn btn-success" Text="Create New" ID="btnCreateNew" OnClick="btnCreateNew_Click"/></td>
                    </tr>
                </table>
            </div>
            <div class="form-group">
                <asp:SqlDataSource ID="CouponPrizedrawDataSource" runat="server"
                    ConnectionString="<%$ ConnectionStrings:Cnn_DB %>"
                    SelectCommand="
                        SELECT ID, Title, DateStart,DateEnd, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate FROM CouponPrizedraw"></asp:SqlDataSource>
                <asp:GridView runat="server" CssClass="table table-bordered table-striped" ID="gvCouponPrizedraw" DataSourceID="CouponPrizedrawDataSource" AutoGenerateColumns="false" DataKeyNames="ID" AllowSorting="true" OnRowDataBound="gvCouponPrizedraw_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="false"/>
                        <asp:BoundField DataField="Title" HeaderText="Title" />
                        <asp:BoundField DataField="DateStart" HeaderText="Start" />
                        <asp:BoundField DataField="DateEnd" HeaderText="End" />
                        <asp:BoundField DataField="CreatedBy" HeaderText="Created By" />
                        <asp:BoundField DataField="CreatedDate" HeaderText="Created Date" />
                        <asp:BoundField DataField="ModifiedBy" HeaderText="Modified By" />
                        <asp:BoundField DataField="ModifiedDate" HeaderText="Modified Date" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <button type="button" id="btnDetail" onclick="GoToUrl(this);" class="btn btn-default">Update</button>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </form>
    </div>
    <script type="text/javascript">
        $(function () {
            $('#gvCouponPrizedraw td:nth-child(' + 4 + '),#gvCouponPrizedraw th:nth-child(' + 4 + ')').hide();
            $('#gvCouponPrizedraw td:nth-child(' + 6 + '),#gvCouponPrizedraw th:nth-child(' + 6 + ')').hide();
            $('#gvCouponPrizedraw td:nth-child(' + 8 + '),#gvCouponPrizedraw th:nth-child(' + 8 + ')').hide();
        });

        function GoToUrl(e) {
            var value = e.closest('tr').cells[0].innerHTML;
            window.location.href = '~/Page/CouponPrizedraw/Page/Update/CouponPrizedraw.aspx?id=' + value + '';
        }
    </script>
</body>
</html>
