<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Promo.aspx.cs" Inherits="Page_Register_LainLain_Promo" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Promo Lain Lain (Promo Bank, Event)
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <script>
        $(document).ready(function () {
            $('#<%=datepicker.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
            $('#<%=datepicker2.ClientID%>').datepicker({
                format: 'dd-mm-yyyy'
            });
        });
    </script>
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
                    <h2 class="text-center">
                        <asp:Label runat="server" ID="lblMessage">Proses Berhasil</asp:Label>
                    </h2>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                </div>
            </div>
        </div>
    </div>
    <div class="container-fluid">
        <form runat="server">
            <div class="panel panel-default width-800">
                <div class="panel-heading">
                    New Promo
                </div>
                <div class="panel-body">
					<div class="col-md-6">
						<div class="form-group">
							<label>Nama</label>
							<asp:RequiredFieldValidator ID="nama_RF" runat="server" />
							<asp:TextBox ID="nama" runat="server" CssClass="form-control" MaxLength="100" placeholder="Input Nama Promo" />								
						</div>
						<div class="form-group">
							<label>Tanggal Mulai</label>
							<asp:RequiredFieldValidator ID="datepicker_RF" runat="server" />
							<asp:TextBox CssClass="form-control" type="text" ID="datepicker" runat="server" data-date-format="dd-mm-yyyy" placeholder="Input Tanggal Mulai" />
						</div>
						<div class="form-group">
							<label>Tanggal Expire</label>
							<asp:RequiredFieldValidator ID="datepicker2_RF" runat="server" />
							<asp:TextBox CssClass="form-control" type="text" ID="datepicker2" runat="server" data-date-format="dd-mm-yyyy" placeholder="Input Tanggal Expire" />
						</div>
					</div>
					<div class="col-md-6">
						<div class="form-group">
							<label>Keterangan</label>
							<asp:TextBox ID="keterangan" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" placeholder="Input Keterangan" />
						</div>
						<div class="form-group">
							<label>Tipe Promo</label>
							<asp:DropDownList ID="tipe" runat="server" CssClass="form-control" ClientIDMode="Static" onchange="onchangepromo(this);">
								<asp:ListItem Value="1">Promo Bank</asp:ListItem>
								<asp:ListItem Value="2">Event</asp:ListItem>
							</asp:DropDownList>
							<script type="text/javascript">
								function onchangepromo(e){
									if(e.value == "1"){
										document.getElementById('divpromobank').style.display = "block";
									}
									else{
										document.getElementById('divpromobank').style.display = "none";
									}
								}
							</script>
						</div>
					</div>
					<div class="col-md-11" id="divpromobank">
						<div class="form-group">
							<button type="button" class="btn btn-sm btn-success" onclick="clickaddextension();">Add</button>
							<table runat="server" id="tb_extension" class="table table-bordered" ClientIDMode="Static">
								<thead>
									<tr>
										<th>Kode Promo</th>
										<th>Minimum Value</th>
										<th>Maximum Value</th>
										<th>Discount</th>
										<th>Action</th>
									</tr>
								</thead>
							</table>
							<asp:HiddenField runat="server" ID="finalextension" ClientIDMode="Static"></asp:HiddenField>
						</div>
						<script type="text/javascript">
							$(document).ready(function(){
							
							});
							function clickaddextension()
							{
								var tbr = document.getElementById("tb_extension").tBodies[0];
								var totalRowCount = document.getElementById("tb_extension").rows.length;
								
								var parrow = document.createElement("tr");
								
								var para0 = document.createElement("td");
								var awek0 = document.createElement("input");
								awek0.setAttribute('type','text');
								awek0.setAttribute('class','form-control');
								awek0.setAttribute('onblur', 'onkeyupnum();');
								para0.appendChild(awek0);
								parrow.appendChild(para0);
								tbr.appendChild(parrow);
								
								var para1 = document.createElement("td");
								var awek1 = document.createElement("input");
								awek1.setAttribute('type','number');
								awek1.setAttribute('class','form-control');
								awek1.setAttribute('onblur', 'onkeyupnum();');
								para1.appendChild(awek1);
								parrow.appendChild(para1);
								tbr.appendChild(parrow);
								
								var para2 = document.createElement("td");
								var awek2 = document.createElement("input");
								awek2.setAttribute('type','number');
								awek2.setAttribute('class','form-control');
								awek2.setAttribute('onblur', 'onkeyupnum();');
								para2.appendChild(awek2);
								parrow.appendChild(para2);
								tbr.appendChild(parrow);
								
								var para3 = document.createElement("td");
								var awek3 = document.createElement("input");
								awek3.setAttribute('type','number');
								awek3.setAttribute('class','form-control');
								awek3.setAttribute('onblur', 'onkeyupnum();');
								para3.appendChild(awek3);
								parrow.appendChild(para3);
								tbr.appendChild(parrow);
																
								var para4 = document.createElement("td");
								var awek4 = document.createElement("button");
								awek4.setAttribute('type','button');
								awek4.setAttribute('class','btn btn-danger');
								awek4.innerHTML = 'Delete';
								awek4.setAttribute('onclick','removerowextension(this);');
								para4.appendChild(awek4);
								parrow.appendChild(para4);
								tbr.appendChild(parrow);
								
								updaterecordhidden(document.getElementById("tb_extension"));
							}
							
							function removerowextension(e)
							{
								var i = e.parentNode.parentNode.rowIndex;
								document.getElementById("tb_extension").deleteRow(i);
								updaterecordhidden(document.getElementById("tb_extension"));
							}
							
							// Constructor function for extensionLD objects
							function extensionLD(kodepromo, minvalue, maxvalue, discount) {
								this.kodepromo = kodepromo;
								this.minvalue = minvalue;
								this.maxvalue = maxvalue;
								this.discount = discount;
							}
							
							function updaterecordhidden(tbl)
							{
								let extensionLDs = [];
								let i = 1
								for (i = 1; i <= tbl.rows.length - 1; i++) 
								{
									var row = tbl.rows[i];
									var kpromoname = row.cells[0].children[0];
									var kpromonametext = kpromoname.value;
									
									var minval = row.cells[1].children[0];
									var minvaltext = minval.value;
									
									var maxval = row.cells[2].children[0];
									var maxvaltext = maxval.value;
									
									var disc = row.cells[3].children[0];
									var disctext = disc.value;
									
									
									let extensionLDunit = new extensionLD(kpromonametext,minvaltext,maxvaltext,disctext);
									extensionLDs.push(extensionLDunit);
								}
								finalextension.value = JSON.stringify(extensionLDs);
								console.log(finalextension.value);
							}
							
							function onkeyupnum()
							{
								updaterecordhidden(document.getElementById("tb_extension"));
							}
						</script>
					</div>
                </div>
                <div class="panel-footer">
                    <div class="button">
                        <asp:Button id="save" runat="server" CssClass="btn btn-primary" OnClientClick="return onsave();" OnClick="save_Click" Text="Save">
                        </asp:Button>
                        <script type="text/javascript">
                            function onsave() {
                                var nama = document.getElementById('content_nama').value;
                                var keterangan = document.getElementById('content_keterangan').value;
                                var startdate = document.getElementById('content_datepicker').value;
                                var expiredate = document.getElementById('content_datepicker2').value;
								var finalextension = document.getElementById('finalextension').value;
								var tipe = document.getElementById('tipe').value;
                                if (nama == "" || startdate == "" || expiredate == "") {
                                    if (nama == "") {
                                        alert("Nama Tidak Boleh Kosong.");
                                        $('#content_nama').focus();
                                    }
                                    else if (startdate == "") {
                                        alert("Tanggal Mulai Tidak Boleh Kosong.");
                                        $('#content_datepicker').focus();
                                    }
                                    else if (expiredate == "") {
                                        alert("Tanggal Expire Tidak Boleh Kosong.");
                                        $('#content_datepicker2').focus();
                                    }
                                    return false;
                                }
								if(finalextension == "" && tipe == "1"){
									alert("Promo Bank Wajib menambah list Kode Promo.");
									return false;
								}
                            }
                        </script>
                    </div>
                </div>
            </div>
        </form>
        <hr />
    </div>
</asp:Content>
