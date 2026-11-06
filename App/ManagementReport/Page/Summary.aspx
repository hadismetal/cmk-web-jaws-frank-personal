<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Summary.aspx.cs" Inherits="Page_Summary" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Summary
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <link rel="stylesheet" href="http://yui.yahooapis.com/3.9.0/build/cssgrids/cssgrids-min.css" />
    <script src="http://yui.yahooapis.com/3.16.0/build/yui/yui-min.js"></script>
    <script type="text/javascript">
        (function () {
            YUI().use('charts', function (Y) {
                var myDataValues = [
                    <%=chart%>
                ];
                var mychart = new Y.Chart({ dataProvider: myDataValues, render: "#mychart" });

                var myDataValues2 = [
                    <%=chart2%>
                ];
                var mychart2 = new Y.Chart({ dataProvider: myDataValues2, render: "#mychart2" });
            });
        })();
    </script>
    <style>
        #mychart {
            margin: 10px 10px 10px 10px;
            width: 500px;
            max-width: 1000px;
            height: 200px;
            float: left;
        }
        #mychart2 {
            margin: 10px 10px 10px 10px;
            width: 500px;
            max-width: 1000px;
            height: 200px;
            float: left;
        }
    </style>
    <div class="table-responsive">
        <asp:Table ID="tb" runat="server" CssClass="table-list">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="400">Keterangan</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150" CssClass="text-center">Qty</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150" CssClass="text-center">Value</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
    <div>
        <div class="table-responsive" style="margin-top: 0px">
            <div id="chartbox" runat="server" style="display: none;"></div>
            <div id="mychart"></div>
        </div>
        <div class="table-responsive" style="margin-top: 0px">
            <div id="chartbox2" runat="server" style="display: none;"></div>
            <div id="mychart2"></div>
        </div>
    </div>
</asp:Content>

