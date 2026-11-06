<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Sales_Submit.aspx.cs" Inherits="Page_Register_Sales_Submit" %>

<%@ Register Src="MenuSales.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Sales Order
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <p>
        <span class="text-muted">Nomor :</span><br />
        <span class="text-lg" id="header_nomor" runat="server" />
    </p>
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
        transaction form
    </div>
    <div class="row">
        <div class="col-md-5">
            <table class="table-file">
                <tr>
                    <td class="text-label">Market Brand</td>
                    <td>:</td>
                    <td id="brand" runat="server" />
                </tr>
            </table>
        </div>
        <br class="visible-sm visible-xs" />
        <div class="col-md-5">
            <table class="table-file">
                <tr>
                    <td class="text-label">Customer</td>
                    <td>:</td>
                    <td id="customer" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Keterangan</td>
                    <td>:</td>
                    <td id="keterangan" runat="server" />
                </tr>
                <tr>
                    <td class="text-label">Tanggal</td>
                    <td>:</td>
                    <td id="tgl" runat="server" />
                </tr>
            </table>
        </div>
    </div>
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
        transaction data
    </div>

    <div class="table-responsive">
        <asp:Table ID="tb" runat="server" CssClass="table-list">
            <asp:TableHeaderRow>
                <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">No. Invoice</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">PLU</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="200">Product Item</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Harga</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Discount %</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="100">Discount Program %</asp:TableHeaderCell>
                <asp:TableHeaderCell Width="150">Total Bayar</asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
    </div>
    <hr />
    <form runat="server">
        <div class="table-responsive">
            <table cellspacing="0" cellpadding="0" border="0" class="table-form">
                <tr>
                    <td>Total Sebelum Discount</td>
                    <td>:</td>
                    <td>
                        <div class="input-group">
                            <asp:TextBox ID="totalbeforedisc" runat="server" ReadOnly="true" CssClass="form-control text-lg text-right"></asp:TextBox>
                            <span class="input-group-addon">IDR</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>Discount</td>
                    <td>:</td>
                    <td>
                        <div class="input-group">
                            <asp:TextBox ID="discount" runat="server" Text="0" AutoPostBack="true" OnTextChanged="CalcTotalBayar" CssClass="form-control text-lg text-right"></asp:TextBox>
                            <span class="input-group-addon">&nbsp;&nbsp;%</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>Discount Program</td>
                    <td>:</td>
                    <td>
                        <div class="input-group">
                            <asp:TextBox ID="discountprogram" Text="0" runat="server" AutoPostBack="true" OnTextChanged="CalcTotalBayar" CssClass="form-control text-lg text-right"></asp:TextBox>
                            <span class="input-group-addon">&nbsp;&nbsp;%</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>Discount Voucher Gift</td>
                    <td>:</td>
                    <td>
                        <div class="input-group">
                            <asp:TextBox ID="discountgift" Text="0" runat="server" AutoPostBack="true" OnTextChanged="CalcTotalBayar" CssClass="form-control text-lg text-right"></asp:TextBox>
                            <span class="input-group-addon">IDR</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>Discount Nominal</td>
                    <td>:</td>
                    <td>
                        <div class="input-group">
                            <asp:TextBox ID="discountround" Text="0" runat="server" AutoPostBack="true" OnTextChanged="CalcTotalBayar2" CssClass="form-control text-lg text-right"></asp:TextBox>
                            <span class="input-group-addon">IDR</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>Total Discount</td>
                    <td>:</td>
                    <td>
                        <div class="input-group">
                            <asp:TextBox ID="totaldiscount" runat="server" ReadOnly="true" CssClass="form-control text-lg text-right"></asp:TextBox>
                            <span class="input-group-addon">IDR</span>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>Total Bayar</td>
                    <td>:</td>
                    <td>
                        <div class="input-group">
                            <asp:TextBox ID="totalbayar" runat="server" ReadOnly="true" CssClass="form-control text-lg text-right"></asp:TextBox>
                            <span class="input-group-addon">IDR</span>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <hr />

        <div class="button-left">
            <button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
                <i class="icon-check"></i>
                Finish
            </button>
        </div>
    </form>
</asp:Content>

