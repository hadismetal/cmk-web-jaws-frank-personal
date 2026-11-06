<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Product_Print.aspx.cs" Inherits="DJ_Print" %>

<style>
    body
    {
        margin: 0px auto;
    }

    #container
    {
        margin: 0px auto;
        width: 1004px;
    }

    #title
    {
        text-align: center;
        font-size: 22px;
        font-weight: bold;
    }

    #header
    {
        margin: 40px 0px;
    }

    .location_info
    {
        float: left;
        width: 400px;
    }

    .no_pengiriman
    {
        float: left;
        width: 300px;
    }

    .date_info
    {
        float: left;
        width: 300px;
    }

    .clear
    {
        font-size: 1px;
        clear: both;
    }

    .orderno
    {
        padding: 40px 0px 0px 0px;
    }

    #content
    {
        margin: 20px 0px;
    }

    .item_list
    {
        width: 100%;
        padding: 0px;
        margin: 0px;
    }

    .item_list th
    {
        border-top: solid 1px #000;
        border-bottom: solid 1px #000;
        padding: 15px 0px;
    }

    .right
    {
        text-align: right !important;
    }

    .item_list td, .item_list th
    {
        text-align: left;
    }

    .item_list td
    {
        padding: 15px 0px;
    }

    .total
    {
        margin: 10px 0px 10px 0px !important;
        padding-top: 10px;
        border-top: solid 1px #000;
    }

    #footer
    {
        margin: 60px 0px 0px 0px;
    }

    .remarks
    {
        float: left;
        width: 300px;
    }

    .approved
    {
        float: left;
        width: 300px;
    }

    .received
    {
        float: left;
        width: 300px;
    }

    .delivered
    {
        float: left;
    }
</style>
<div id="container">
    <div id="title">TRANSFER SHIPMENT ORDER</div>
    <div id="header">
        <div class="location_info">FROM : <asp:Literal id="from" runat="server" /></div>
        <div class="no_pengiriman"><asp:Literal id="nodokumen" runat="server" /></div>
        <div class="date_info">
            Date :  <asp:Literal id="date" runat="server" /><br />
            DELIVER TO : <asp:Literal id="to" runat="server" />
        </div>
        <div class="clear">&nbsp;</div>
        <div class="orderno">ORDER NO :  <asp:Literal id="orderno" runat="server" /></div>
    </div>
    <div id="content">
        <asp:Table ID="tb" runat="server" CssClass="item_list">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="50">No.</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="300">Product Number</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="550">Description</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100" CssClass="right">Qty</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
        <%--table cellspacing="0" cellpadding="0" border="0" class="item_list">
            <tr>
                <th width="50">No.</th>
                <th width="300">Product Number</th>
                <th width="550">Description</th>
                <th width="100" class="right">Qty</th>
            </tr>
            <tr>
                <td>1.</td>
                <td>12345678</td>
                <td>Flash Disk Kingston 18Gb</td>
                <td class="right">10</td>
            </tr>
            <tr>
                <td>2.</td>
                <td>12345678</td>
                <td>Flash Disk Kingston 18Gb</td>
                <td class="right">10</td>
            </tr>
            <tr>
                <td colspan="3" class="right total">Total Quantity</td>
                <td class="right total">20</td>
            </tr>
        </--%>
    </div>
    <div id="footer">
        <div class="remarks">
            Remarks<br />
            Prepared By<br />
            <br />
            <br />
            <br />
            <br />
            <br />
            (&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)
        </div>
        <div class="approved">
            Approved By<br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />

            (&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)
        </div>
        <div class="received">
            Received By<br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />

            (&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)
        </div>
        <div class="delivered">
            Delivered By<br />
            <br />
            <br />
            <br />
            <br />
            <br />
            <br />

            (&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;)
        </div>
    </div>
</div>
