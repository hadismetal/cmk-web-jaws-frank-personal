<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PromoDetail.aspx.cs" Inherits="Page_Register_LainLain_PromoDetail" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Promo Lain Lain (Promo Bank, Event)
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Detail
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <div class="container-fluid">
        <form runat="server">
            <div class="panel panel-default width-800">
                <div class="panel-heading">
                    Promo
                </div>
                <div class="panel-body">
                    <div runat="server" id="divheader">
                        <table class="table">
                            <tr>
                                <td>Nama Promo</td>
                                <td><label runat="server" id="promoname"></label></td>
                            </tr>
                            <tr>
                                <td>Start Date</td>
                                <td><label runat="server" id="start"></label></td>
                            </tr>
                            <tr>
                                <td>End Date</td>
                                <td><label runat="server" id="end"></label></td>
                            </tr>
                            <tr>
                                <td>Status</td>
                                <td><label runat="server" id="status"></label></td>
                            </tr>
                            <tr>
                                <td>Keterangan</td>
                                <td><label runat="server" id="keterangan"></label></td>
                            </tr>
                        </table>
                    </div>
                    <div runat="server" id="divdetail">
                        <asp:Table runat="server" id="tb" CssClass="table table-bordered">
                            <asp:TableHeaderRow>
                                <asp:TableHeaderCell>No.</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Name</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Minimum Value</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Maximum Value</asp:TableHeaderCell>
                                <asp:TableHeaderCell>Discount</asp:TableHeaderCell>
                            </asp:TableHeaderRow>
                        </asp:Table>
                    </div>
                </div>
            </div>
        </form>
    </div>
</asp:Content>
