<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Report_EDC.aspx.cs" Inherits="Page_Report_Bank_Report_EDC" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Report Approval Code Bank
</asp:Content>
<asp:Content ContentPlaceHolderID="content" runat="server">
	<!-- Modal -->
    <div class="modal fade" id="postingModal" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Message</h4>
                </div>
                <div class="modal-body">
                    <center>
						<h2>
						<asp:Label runat="server" ID="lblMessage1" Text="Proses Posting Berhasil!!!" ></asp:Label>
						</h2>
					</center>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>

        </div>
    </div>
	<!-- Modal -->
    <div class="modal fade" id="deleteModal" role="dialog">
        <div class="modal-dialog">

            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Message</h4>
                </div>
                <div class="modal-body">
                    <center>
						<h2>
                        <asp:Label runat="server" ID="lblMessage2" Text="Proses Delete Berhasil!!!" CssClass="text-center"></asp:Label>
						</h2>
					</center>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
	<!-- Modal -->
    <div class="modal fade" id="updateModal" role="dialog">
        <div class="modal-dialog modal-sm">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    <h4 class="modal-title">Update Status Approval Code</h4>
                </div>
                <div class="modal-body">
					<div class="form-horizontal container">
                      <input type="hidden" id="txtidtable" />
					  <div class="form-group">
						<label class="control-label col-sm-4" for="noso">Bank:</label>
						<div class="col-sm-8">
						  <input type="text" class="form-control" id="txtbank" placeholder="Enter number" disabled />
						</div>
					  </div>
					  <div class="form-group">
						<label class="control-label col-sm-4" for="email">Approval Code:</label>
						<div class="col-sm-8">
						  <input type="text" class="form-control" id="txtappcodemodal" placeholder="Enter approval code" disabled />
						</div>
					  </div>
					  <div class="form-group">
						<label class="control-label col-sm-4" for="pwd">Nominal:</label>
						<div class="col-sm-8">
						  <input type="text" class="form-control" id="txtnominalmodal" placeholder="Enter nominal" disabled/>
						</div>
					  </div>
					  <div class="form-group">
						<label class="control-label col-sm-4" for="pwd">Status Cair:</label>
						<div class="col-sm-8">
						  <div class="checkbox">
							<label><input type="checkbox" id="checkboxupdatemodal" checked="checked" disabled /> Cair</label>
						  </div>
						</div>
					  </div>
					</div>
                </div>
                <div class="modal-footer">
					<button type="button" id="btnDeleteAppCode" class="btn btn-danger" data-dismiss="modal" onclick="onClickUpdateAppCode();">Delete</button>
                    <button type="button" id="btnPostAppCode" class="btn btn-success" data-dismiss="modal" onclick="onClickPostToGL();">Post</button>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
					<script type='text/javascript'>
						function onClickUpdateAppCode(){
							var bank = document.getElementById("txtbank").value;
							var appcode = document.getElementById("txtappcodemodal").value;
							var nominale = document.getElementById("txtnominalmodal").value.split(".").join("").replace(",",".");
                            var cekcair = document.getElementById("checkboxupdatemodal").checked;
                            var id = document.getElementById("txtidtable").value;
                            //alert(id);
							window.location.href = "Report_EDC_Update.aspx?id="+id;
                        }

                        function onClickPostToGL() {
                            var bank = document.getElementById("txtbank").value;
							var appcode = document.getElementById("txtappcodemodal").value;
							var nominale = document.getElementById("txtnominalmodal").value.split(".").join("").replace(",",".");
                            var cekcair = document.getElementById("checkboxupdatemodal").checked;
                            var id = document.getElementById("txtidtable").value;

                            //alert(id);

                            window.location.href = "Report_EDC_PostGL.aspx?id="+id;
                        }
					</script>
                </div>
            </div>
        </div>
    </div>
    <form runat="server">
		<!-- Modal -->
		<div class="modal fade" id="myModalPostingGL" role="dialog">
			<div class="modal-dialog">

				<!-- Modal content-->
				<div class="modal-content">
					<div class="modal-header">
						<button type="button" class="close" data-dismiss="modal">&times;</button>
						<h4 class="modal-title">Message</h4>
					</div>
					<div class="modal-body">
						<center>
							<h2>
								<asp:Label runat="server" ID="lblMessagePostGL" Text="Are You Sure???"></asp:Label>
							</h2>
						</center>
					</div>
					<div class="modal-footer">
						<asp:Button runat="server" ID="btnSync" CssClass="btn btn-success" Text="Post"></asp:Button>
						<button type="button" class="btn btn-default" data-dismiss="modal">Cancel</button>
					</div>
				</div>

			</div>
		</div>
        <!--Table-->
        <div class="col-md-12">
            <table class="table-responsive">
                <tbody>
                    <tr>
                        <td>Approval Code</td>
                        <td>:</td>
                        <td style="padding-right: 2em">
                            <asp:TextBox runat="server" ID="txtAppCode" CssClass="form-control"></asp:TextBox>
                        </td>
                        <td>SO Number</td>
                        <td>:</td>
                        <td style="padding-right: 2em">
                            <asp:TextBox runat="server" ID="txtSONumber" CssClass="form-control"></asp:TextBox>
                        </td>
						<td>Store</td>
                        <td>:</td>
                        <td style="padding-right: 2em">
                            <asp:DropDownList runat="server" ID="ddlStore" CssClass="form-control">
                            </asp:DropDownList>
                        </td>
                        <td>Status</td>
                        <td>:</td>
                        <td style="padding-right: 2em">
                            <asp:DropDownList runat="server" ID="ddlStatus" CssClass="form-control">
                                <asp:ListItem Text="All" Value="0"></asp:ListItem>
                                <asp:ListItem Text="Cair" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Belum Cair" Value="2"></asp:ListItem>
								<asp:ListItem Text="Post GL" Value="3"></asp:ListItem>
                                <asp:ListItem Text="Belum Post GL" Value="4"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnDisplay" CssClass="btn btn-primary" Text="Display" OnClick="refreshPage"></asp:Button>
                            <asp:Button runat="server" ID="btnExport" CssClass="btn btn-primary" Text="Export Excel" OnClick="btnExport_Click"></asp:Button>
							<!--<button type="button" class="btn btn-success" id="btnPopUpSync">Posting To GL</button>-->
                        </td>
                    </tr>
                    <tr>
                        <td>Bank</td>
                        <td>:</td>
                        <td style="padding-right: 2em">
                            <asp:DropDownList runat="server" ID="ddlEDCBank" CssClass="form-control">
                            </asp:DropDownList>
                        </td>
                        <td>Tgl Upload</td>
                        <td>:</td>
                        <td style="padding-right: 2em">
                            <asp:TextBox runat="server" ID="tglUpload" CssClass="form-control"></asp:TextBox>
                            <script>
                                $(document).ready(function () {
                                    $('#<%=tglUpload.ClientID%>').datepicker({
                                        format: 'yyyy-mm-dd'
                                    });
								});
                            </script>
                        </td>
                    </tr>
					<tr>
						<td>Periode SO</td>
                        <td>:</td>
                        <td style="padding-right: 2em">
                            <asp:TextBox runat="server" ID="tglSOstart" CssClass="form-control"></asp:TextBox>
						</td>
						<td>
							<p>To</p>
						</td>
						<td>
						</td>
						<td style="padding-right: 2em">
							<asp:TextBox runat="server" ID="tglSOend" CssClass="form-control"></asp:TextBox>
                            <script>
                                $(document).ready(function () {
                                    $('#<%=tglSOstart.ClientID%>').datepicker({
                                        format: 'yyyy-mm-dd'
                                    });
									$('#<%=tglSOend.ClientID%>').datepicker({
                                        format: 'yyyy-mm-dd'
                                    });
								});
                            </script>
                        </td>
					</tr>
                </tbody>
            </table>

        </div>
        <div class="col-md-12" style="display:none">
            <h3>File Upload</h3>
            <table class="table-responsive">
                <tbody>
                    <tr>
                        <td>
                            <asp:FileUpload runat="server" ID="fileexcel" CssClass="form-control" onchange="ValidatorFile();"/>
                        </td>
                        <td>
                            <asp:DropDownList runat="server" ID="ddlEDC" CssClass="form-control" onchange="ValidatorFile();"/>
                        </td>
						<td style="display:none">
							<asp:DropDownList runat="server" ID="ddlTipePayment" CssClass="form-control" onchange="ValidatorFile();">
                                <asp:ListItem Text="-" Value="0"></asp:ListItem>
                                <asp:ListItem Text="SO" Value="1"></asp:ListItem>
                                <asp:ListItem Text="DP" Value="2"></asp:ListItem>
                            </asp:DropDownList>
						</td>
                        <td>
                            <button type="button" class="btn btn-primary" id="btnTemplate" onclick="tableHtmlToExcel('format-excel','Form-Upload-App-Code')" style="display:none">Template</button>
                            <asp:Button runat="server" ID="btnUpload" class="btn btn-primary" Text="Upload" disabled="disabled" OnClick="btnUpload_Click" AutoPostBack="false"></asp:Button>
                        </td>
                        <td>
                            <asp:Button runat="server" ID="btnTemplates" class="btn btn-primary" Text="Download Template" OnClick="btnTemplateExcel_Click"></asp:Button>
                        </td>
                    </tr>
                </tbody>
            </table>
            <br/>
        </div>
        <table id="format-excel" style="display: none">
            <thead>
                <tr>
                    <th>Nomor EDC</th>
                    <th>Nominal</th>
                </tr>
            </thead>
        </table>
        <script type="text/javascript">
			function ValidatorFile() {
				var filename = document.getElementById('<%=fileexcel.ClientID %>').value.split("\\").pop();
				var extension = filename.split(".").pop();
				var edcdrop = document.getElementById('<%=ddlEDC.ClientID%>');
				var edc = edcdrop.options[edcdrop.selectedIndex].value;
				if (extension == "xls" || extension == "xlsx") {
					if (edc == "0") {
						alert("Bank must choosed.");
						document.getElementById('<%=btnUpload.ClientID %>').setAttribute("disabled", "disabled");
					}
					else {
						document.getElementById('<%=btnUpload.ClientID %>').removeAttribute("disabled");
					}
				}
				else {
					alert("File extension must be 'xls' or 'xlsx'.");
					document.getElementById('<%=fileexcel.ClientID %>').value = null;
					document.getElementById('<%=btnUpload.ClientID %>').setAttribute("disabled", "disabled");
					
				}
			}
			
			function tableHtmlToExcel(tableID, filename = '') {
				var downloadLink;
				var dataType = 'application/vnd.ms-excel';
				var tableSelect = document.getElementById(tableID);
				var tableHTML = tableSelect.outerHTML.replace(/ /g, '%20');

				filename = filename ? filename + '.xls' : 'excel_data.xls';

				downloadLink = document.createElement("a");

				document.body.appendChild(downloadLink);

				if (navigator.msSaveOrOpenBlob) {
					var blob = new Blob(['\ufeff', tableHTML], {
						type: dataType
					});
					navigator.msSaveOrOpenBlob(blob, filename);
				} else {
					downloadLink.href = 'data:' + dataType + ', ' + tableHTML;

					downloadLink.download = filename;

					downloadLink.click();
				}
			}
        </script>
        <br />
		<hr />
        <asp:Table runat="server" ID="tbl" CssClass="table table-bordered table-responsive">
            <asp:TableHeaderRow CssClass="table-primary" BackColor="PowderBlue">
				<asp:TableHeaderCell><p style="font-family:consolas"></p></asp:TableHeaderCell>
                <asp:TableHeaderCell><p style="font-family:consolas">No.</p></asp:TableHeaderCell>
                <asp:TableHeaderCell><p style="font-family:consolas">SO Number</p></asp:TableHeaderCell>
				<asp:TableHeaderCell><p style="font-family:consolas">Lokasi</p></asp:TableHeaderCell>
				<asp:TableHeaderCell><p style="font-family:consolas">Tgl SO</p></asp:TableHeaderCell>
                <asp:TableHeaderCell><p style="font-family:consolas">Approval Code</p></asp:TableHeaderCell>
				<asp:TableHeaderCell><p style="font-family:consolas">Nominal SO</p></asp:TableHeaderCell>
				<asp:TableHeaderCell><p style="font-family:consolas">EDC Bank</p></asp:TableHeaderCell>
				<asp:TableHeaderCell Visible="false"><p style="font-family:consolas">Nominal Gesek</p></asp:TableHeaderCell>
				<asp:TableHeaderCell><p style="font-family:consolas">Nominal Cair</p></asp:TableHeaderCell>
				<asp:TableHeaderCell><p style="font-family:consolas">Nominal MDR</p></asp:TableHeaderCell>
                <asp:TableHeaderCell><p style="font-family:consolas">Status</p></asp:TableHeaderCell>
                <asp:TableHeaderCell><p style="font-family:consolas">Status Post GL</p></asp:TableHeaderCell>
				<asp:TableHeaderCell><p style="font-family:consolas">Credit Card Number</p></asp:TableHeaderCell>
				<asp:TableHeaderCell><p style="font-family:consolas">Credit Card Name</p></asp:TableHeaderCell>
                <asp:TableHeaderCell><p style="font-family:consolas">Customer</p></asp:TableHeaderCell>
				<asp:TableHeaderCell><p style="font-family:consolas">Operator</p></asp:TableHeaderCell>
				<asp:TableHeaderCell Visible="false"></asp:TableHeaderCell>
                <asp:TableHeaderCell Visible="false"></asp:TableHeaderCell>
            </asp:TableHeaderRow>
        </asp:Table>
		<script type="text/javascript">
        $(document).ready(function () {
			const queryString = window.location.search;
			let params = new URLSearchParams(queryString);
            var miseg = params.get('msgupload');
            var miseg1 = params.get('msgpost');
			if (miseg == "Proses Delete Berhasil!!!") {
				//$('#deleteModal').modal('show');
            }
            if (miseg1 == "Proses Posting Berhasil!!!") {
                //$('#postingModal').modal('show');
            }
		});
    </script>
    </form>
</asp:Content>
