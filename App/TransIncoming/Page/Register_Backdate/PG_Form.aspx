<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PG_Form.aspx.cs" Inherits="PG_Form" %>

<%@ Register Src="MenuPG.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Plain Gold
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server">
        <uc1:Menu ID="menu" runat="server" />
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            form summary
        </div>
        <div class="row">
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Market Brand</td>
                        <td>:</td>
                        <td id="brand" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">From
                            <asp:Literal runat="server" ID="lit_type_from" /></td>
                        <td>:</td>
                        <td id="from" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">To
                            <asp:Literal runat="server" ID="lit_type_to" /></td>
                        <td>:</td>
                        <td id="to" runat="server" class="text-bold" />
                    </tr>
                </table>
            </div>
            <br class="visible-sm visible-xs" />
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Tanggal</td>
                        <td>:</td>
                        <td id="tgl" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Kurir</td>
                        <td>:</td>
                        <td id="kurir" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Keterangan</td>
                        <td>:</td>
                        <td id="keterangan" runat="server" class="text-bold" />
                    </tr>
                    <tr>
                        <td class="text-label">Tanggal ETA</td>
                        <td>:</td>
                        <td id="eta" runat="server" class="text-bold" />
                    </tr>
                </table>
            </div>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            product list summary
        </div>
        <div class="row">
            <br class="visible-sm visible-xs" />
            <div class="col-md-5">
                <table class="table-file">
                    <tr>
                        <td class="text-label">Qty</td>
                        <td>:</td>
                        <td id="totalqty" runat="server" class="text-bold" style="text-align: right;" />
                    </tr>
                    <tr>
                        <td class="text-label">Gross Weight</td>
                        <td>:</td>
                        <td id="totalgross" runat="server" class="text-bold" style="text-align: right;" />
                    </tr>
                    <%--<tr>
			        <td class="text-label">Netto Weight</td>
			        <td>:</td>
			        <td id="totalnetto" runat="server" class="text-bold" style="text-align:right;" />
		        </tr>
                <tr>
			        <td class="text-label">Butir</td>
			        <td>:</td>
			        <td id="totalbutir" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">Carat</td>
			        <td>:</td>
			        <td id="totalcarat" runat="server" class="text-bold" />
		        </tr>--%>
                    <tr>
                        <td class="text-label">Product Item</td>
                        <td>:</td>
                        <td id="product_item_summary" runat="server" class="text-bold" style="text-align: right;" />
                    </tr>
                    <tr>
                        <td class="text-label">Product Status</td>
                        <td>:</td>
                        <td id="product_status_summary" runat="server" class="text-bold" style="text-align: right;" />
                    </tr>
                    <tr>
                        <td class="text-label">Product Model</td>
                        <td>:</td>
                        <td id="product_model_summary" runat="server" class="text-bold" style="text-align: right;" />
                    </tr>
                </table>
            </div>
        </div>
        <hr />
        <div class="file-subheader hidden-sm hidden-xs">
            product list
        </div>
        <table class="table-form">
            <tr>
                <td>Tanggal</td>
                <td>:</td>
                <td>
                    <asp:TextBox ID="tgl2" runat="server" CssClass="form-control text-center" Width="110" />
                    <script>
                        $(document).ready(function () { $('#content_tgl2').datepicker({ format: 'dd-mm-yyyy' }); });
                    </script>
                </td>
                <td>
                    <asp:RequiredFieldValidator ID="tgl2_RF" runat="server" />
                    <span id="tgl2_ERR" runat="server" class="text-validator" visible="false">Format</span>
                </td>
            </tr>
        </table>
        <div class="row">
            <br class="visible-sm visible-xs" />
            <div class="col-md-5">

                <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
                <div class="form-inline">
                    <asp:CheckBoxList runat="server" ID="cbl_Product" />
                    <style type="text/css">
                        label {
                            display: block;
                            border: solid 1px black;
                            border-collapse: collapse;
                        }
                    </style>
                </div>

            </div>
        </div>
        <hr />
        <div class="row">
            <br class="visible-sm visible-xs" />
            <div class="col-md-5">
                <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                    <i class="icon-arrow-right"></i>
                    Next
                </button>
            </div>
        </div>
    </form>
</asp:Content>

