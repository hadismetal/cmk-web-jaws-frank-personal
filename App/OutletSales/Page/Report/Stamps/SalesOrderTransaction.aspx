<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SalesOrderTransaction.aspx.cs" Inherits="Page_Report_Stamps_Transaction" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Sales Order
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
    <!-- Modal -->
    <div class="modal fade" id="myModal" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Message</h4>
                </div>
                <div class="modal-body">
                    <p class="text-center">
                        <asp:Label runat="server" ID="lblMessage"></asp:Label>
                    </p>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
    <!--Table-->
    <table>
        <tbody>
            <tr>
                <td>No. Sales Order</td>
                <td>:</td>
                <td style="padding-right: 2em">
                    <asp:TextBox runat="server" ID="SONumber" CssClass="form-control"></asp:TextBox>
                </td>
                <td>Tanggal Lunas</td>
                <td>:</td>
                <td style="padding-right: 2em">
                    <asp:TextBox runat="server" ID="tglLunas" CssClass="form-control"></asp:TextBox>
                </td>
                <td>Status</td>
                <td>:</td>
                <td style="padding-right: 0.5em">
                    <asp:DropDownList runat="server" ID="ddlStatus" CssClass="form-control">
                        <asp:ListItem Text="All" Value="0"></asp:ListItem>
                        <asp:ListItem Text="Not Sent" Value="1"></asp:ListItem>
                        <asp:ListItem Text="Sent" Value="2"></asp:ListItem>
                        <asp:ListItem Text="Deleted" Value="3"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                    <button runat="server" id="btnDisplay" class="btn btn-primary" onclick="onClickRefresh(this);">
                        <i class="icon-search"></i>Display</button>
                </td>
                <td>
                    <asp:Button runat="server" ID="btnCancel" CssClass="btn btn-default" Text="Cancel" />
                </td>
            </tr>
        </tbody>
    </table>
    <div class="table-responsive">
        <asp:GridView runat="server" ID="gvjrstransaction" AutoGenerateColumns="False" OnRowDataBound="gvjrstransaction_RowDataBound" CssClass="text-sm text-center">
            <Columns>
                <asp:BoundField DataField="ID" HeaderText="Stamps ID" />
                <asp:BoundField DataField="SONumber" HeaderText="SO Number" />
                <asp:BoundField DataField="Brand" HeaderText="Brand" />
                <asp:BoundField DataField="Customer" HeaderText="Customer" />
                <asp:BoundField DataField="Sales" HeaderText="Sales" />
                <asp:BoundField DataField="Paid" HeaderText="Paid" />
                <asp:BoundField DataField="Unpaid" HeaderText="Unpaid" />
                <asp:BoundField DataField="Total" HeaderText="Total" />
                <asp:BoundField DataField="TglLunas" HeaderText="Tanggal" />
                <asp:TemplateField ShowHeader="False">
                    <ItemTemplate>
                        <table>
                            <tr>
                                <td>
                                    <button type="button" id="btnsend" class="btn btn-primary" onclick="onClickSend(this);">Send</button>
                                </td>
                                <td>
                                    <button type="button" id="btndelete" class="btn btn-danger" onclick="onClickDelete(this);">Delete</button>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <table runat="server" id="tblNodata" visible="false" class="text-lg text-center text-primary">
            <thead>
                <tr>
                    <th></th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <th>No Data</th>
                </tr>
            </tbody>
        </table>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#<%= gvjrstransaction.ClientID %>').DataTable();
            $('#<%= tblNodata.ClientID%>').DataTable();
            Condition();

			$('.paginate_button ','.paginate_button current','.paginate_button next','.paginate_button previous').click(function(){
			gridCondition();
			});
            //$('#myModal').modal('show');
        });
        function onClickSend(e) {
            if ($('#<%= ddlStatus.ClientID %>').val() == "1") {
                var tbl = document.getElementById('<%= gvjrstransaction.ClientID %>');
                var rowindex = e.closest('table').closest('td').closest('tr').rowIndex;
                var colindex = e.closest('table').closest('td').cellIndex;
                var SONumber = tbl.rows[rowindex].cells[colindex - 8].innerHTML;
                if (confirm("Apakah Anda Yakin Ingin Mengirim Transaksi Ini ke STAMPS?")) {
                    window.location.href = "/OutletSales/Page/Report/Stamps/SalesOrderTransactionSend.aspx?SONumber=" + SONumber.replace(/[/]/g, '+') + "";
                }
            }
            else {
                document.getElementById('<%= lblMessage.ClientID%>').innerText = "Permintaan hanya diproses jika status 'Not Sent'";
                $('#myModal').modal('show');
            }
        }

        function onClickDelete(e) {
            if ($('#<%= ddlStatus.ClientID %>').val() == "1") {
                var tbl = document.getElementById('<%= gvjrstransaction.ClientID %>');
                var rowindex = e.closest('table').closest('td').closest('tr').rowIndex;
                var colindex = e.closest('table').closest('td').cellIndex;
                var SONumber = tbl.rows[rowindex].cells[colindex - 8].innerHTML;
                if (confirm("Apakah Anda Yakin Ingin Mendelete Transaksi Ini dan Tidak Dikirimkan ke STAMPS?")) {
                    window.location.href = "/OutletSales/Page/Report/Stamps/SalesOrderTransactionDelete.aspx?SONumber=" + SONumber.replace(/[/]/g, '+') + "";
                }
            }
            else {
                document.getElementById('<%= lblMessage.ClientID%>').innerText = "Permintaan hanya diproses jika status 'Not Sent'";
                $('#myModal').modal('show');
            }
        }

        function onClickRefresh(e) {
            var noso = $('#<%= SONumber.ClientID %>').val();
            var dl = $('#<%= tglLunas.ClientID %>').val();
            var status = $('#<%= ddlStatus.ClientID %>').val();
            window.location.href = "/OutletSales/Page/Report/Stamps/SalesOrderTransaction.aspx?refreshPage=1&noso=" + noso + "&dl=" + dl + "&se=" + status + ""
        }

        function Condition() {
            const urlParams = new URLSearchParams(window.location.search);
            const myParam = urlParams.get('myParam');
            var error = getParameterByName('msg');
            if (error != null) {
				if(error != ""){
					document.getElementById('<%= lblMessage.ClientID%>').innerText = error;
					$('#myModal').modal('show');
				}
            }
            gridCondition();
        }

        function getParameterByName(name, url) {
            if (!url) url = window.location.href;
            name = name.replace(/[\[\]]/g, '\\$&');
            var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
                results = regex.exec(url);
            if (!results) return null;
            if (!results[2]) return '';
            return decodeURIComponent(results[2].replace(/\+/g, ' '));
        }

        function gridCondition() {
            if ($('#<%= ddlStatus.ClientID %>').val() != "1") {
				$('table tr').each(function (){
					$('td #btnsend').attr('disabled', 'disabled');
					$('td #btndelete').attr('disabled', 'disabled');
				});
			}
			else {
				$('table tr').each(function (){
					$('td #btnsend').removeAttr('disabled');
					$('td #btndelete').removeAttr('disabled');
				});
			}
        }
    </script>
</asp:Content>
