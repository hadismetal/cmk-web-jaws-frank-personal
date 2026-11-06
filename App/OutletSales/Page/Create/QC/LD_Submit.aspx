<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="LD_Submit.aspx.cs" Inherits="Page_Approval_LD_PricingSubmit" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Loose Diamond
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    QC
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <form id="Form1" runat="server">
			<div class="panel panel-default width-500">
				<div class="panel-heading">
					Information PLU
				</div>
				<div class="panel-body">
					<table class="table-file">
						<tr>
							<td class="text-muted">PLU</td>
							<td style="padding-left:50px;padding-right:20px">:</td>
							<td><label class="text-lg" id="header_nomor" runat="server" /></td>
						</tr>
						<tr>
							<td class="text-muted">No. Invoice</td>
							<td style="padding-left:50px;padding-right:20px">:</td>
							<td><label class="text-lg" id="header_invoice" runat="server" /></td>
						</tr>
						<tr>
							<td class="text-muted">Nama Customer</td>
							<td style="padding-left:50px;padding-right:20px">:</td>
							<td><label class="text-lg" id="header_cust" runat="server" /></td>
						</tr>
						<tr>
							<td class="text-muted">Store Asal</td>
							<td style="padding-left:50px;padding-right:20px">:</td>
							<td><label class="text-lg" id="header_storeasal" runat="server" /></td>
						</tr>
						<tr>
							<td class="text-muted">GIA</td>
							<td style="padding-left:50px;padding-right:20px">:</td>
							<td><label class="text-lg" id="header_gia" runat="server" /></td>
						</tr>
						<tr>
							<td class="text-muted">Butir</td>
							<td style="padding-left:50px;padding-right:20px">:</td>
							<td><label class="text-lg" id="header_butir" runat="server" /></td>
						</tr>
						<tr>
							<td class="text-muted">Carat</td>
							<td style="padding-left:50px;padding-right:20px">:</td>
							<td><label class="text-lg" id="header_carat" runat="server" /></td>
						</tr>
						<tr>
							<td class="text-muted">Harga Beli</td>
							<td style="padding-left:50px;padding-right:20px">:</td>
							<td><label class="text-lg" id="header_hg_beli" runat="server" /></td>
						</tr>
					</table>
				</div>
			</div>
			<div class="panel panel-default">
				<div class="panel-heading">
					QC
				</div>
				<div class="panel-body">
					<div class="col-xs-6">
						<table class="table-file">
							<!--<tr>
								<td class="text-muted">Petugas QC</td>
								<td>
									<div class="input-group">
										<asp:TextBox ID="petugasqcsearch" runat="server" CssClass="form-control" placeholder="Isi Dengan NIK" />
										<div class="input-group-btn">
											<asp:Button ID="btnsearchnik" runat="server" CssClass="btn btn-info" Text="Search" OnClick="btnSearchNIK_Click" />
										</div>
									</div>
								</td>
							</tr>-->
							<tr>
								<td class="text-muted">Petugas QC</td>
								<td>
									<asp:TextBox ID="petugasqc" runat="server" CssClass="form-control" Enabled="false" />
									<small runat="server" id="lblerrorpetugasqc" class="text-danger" visible="false"></small>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Certificate No.</td>
								<td>
									<asp:DropDownList ID="sertifikat" runat="server" CssClass="form-control">
										<asp:ListItem Value="Sesuai">Sesuai</asp:ListItem>
										<asp:ListItem Value="Tidak Sesuai">Tidak Sesuai</asp:ListItem>
									</asp:DropDownList>
								</td>
							</tr>
							<tr style="display:none">
								<td class="text-muted">Value (IDR)</td>
								<td>
									<asp:TextBox ID="val" runat="server" CssClass="form-control" />
									<small class="text-info">*penulisan menggunakan koma tanpa titik (e.g. 1,00 atau 1000000,00) </small>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Item</td>
								<td>
									<asp:TextBox ID="item" runat="server" CssClass="form-control" readonly />
								</td>
							</tr>
							<tr>
								<td class="text-muted">Shape</td>
								<td>
									<%--<asp:DropDownList ID="shape" runat="server" CssClass="form-control" />--%>
									<asp:Label ID="shape" runat="server"></asp:Label>
								</td>
							</tr>
						</table>
					</div>
					<div class="col-xs-6">
						<table class="table-file">
							<tr>
								<td style="width:250px" class="text-muted">Clarity</td>
								<td>
									<%--<asp:DropDownList ID="cla" runat="server" CssClass="form-control" />--%>
									<asp:Label ID="cla" runat="server"></asp:Label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Color</td>
								<td>
									<%--<asp:DropDownList ID="col" runat="server" CssClass="form-control" />--%>
									<asp:Label ID="col" runat="server"></asp:Label>
								</td>
							</tr>
							<tr>
								<td class="text-muted">Cutting</td>
								<td>
									<%--<asp:DropDownList ID="cut" runat="server" CssClass="form-control" />--%>
									<asp:Label ID="cut" runat="server"></asp:Label>
								</td>
							</tr>
							<tr style="display:none">
								<td class="text-muted">Brand</td>
								<td>
									<%--<asp:DropDownList ID="brand" runat="server" CssClass="form-control" />--%>
									<asp:Label ID="brand" runat="server"></asp:Label>
								</td>
							</tr>
							<tr>
								<td class="text-muted"></td>
								<td>
									
								</td>
							</tr>
							<tr>
								<td class="text-muted">Keterangan</td>
								<td>
									<asp:TextBox ID="keterangan" runat="server" TextMode ="MultiLine" CssClass="form-control" Width=500 Height=200 />
								</td>
							</tr>
						</table>
					</div>
					<div style="margin-top:20px;" class="col-md-6" runat="server" id="divsm">
						<div style="margin-top:10px;margin-bottom:10px;border-style: groove">
							<b>Hasil Physical Checking Oleh SM/BM dan AM</b>
							<hr style="border-style: groove"/>
							<table style="margin-bottom:10px;" class="table-file">
								<tbody>
									<tr>
										<td style="width:250px">Sertifikat Asli</td>
										<td>
											<asp:DropDownList runat="server" ID="isanycertificate" CssClass="form-control">
												<asp:ListItem Value="Ada">Ada</asp:ListItem>
												<asp:ListItem Value="Tidak Ada">Tidak Ada</asp:ListItem>
											</asp:DropDownList>
										</td>
									</tr>
									<tr>
										<td>Invoice Asli</td>
										<td>
											<asp:DropDownList runat="server" ID="isanyinvoice" CssClass="form-control">
												<asp:ListItem Value="Ada">Ada</asp:ListItem>
												<asp:ListItem Value="Tidak Ada">Tidak Ada</asp:ListItem>
											</asp:DropDownList>
										</td>
									</tr>
									<tr>
										<td>Laser Incription LD</td>
										<td>
											<asp:TextBox runat="server" ID="laserincription" CssClass="form-control"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<td>Measurement</td>
										<td>
											<asp:TextBox runat="server" ID="measurement" CssClass="form-control"></asp:TextBox>
										</td>
									</tr>
									<tr>
										<td>Kondisi Fisik Batu</td>
										<td>
											<asp:DropDownList runat="server" ID="kondisibatu" CssClass="form-control">
												<asp:ListItem Value="Baik">Baik</asp:ListItem>
												<asp:ListItem Value="Tidak Baik">Tidak Baik</asp:ListItem>
											</asp:DropDownList>
										</td>
									</tr>
								</tbody>
							</table>
						</div>
					</div>
					<div class="col-md-8">
						<button type="button" class="btn btn-sm btn-success" onclick="clickaddextension();">Add</button>
						<table runat="server" ID="tb_extension" class="table table-bordered" ClientIDMode="Static">
							<thead>
								<tr>
									<th>Clarity Characteristic</th>
									<th>Size</th>
									<th>Number</th>
									<th>Position</th>
									<th>Action</th>
								</tr>
							<thead>
						</table>
						<asp:HiddenField runat="server" ID="listclarity" ClientIDMode="Static"></asp:HiddenField>
						<asp:HiddenField runat="server" ID="listsize" ClientIDMode="Static"></asp:HiddenField>
						<asp:HiddenField runat="server" ID="listposition" ClientIDMode="Static"></asp:HiddenField>
						<asp:HiddenField runat="server" ID="finalextension" ClientIDMode="Static"></asp:HiddenField>
						<script type="text/javascript">
							$(document).ready(function(){
							
							});
							function clickaddextension()
							{
								var tbr = document.getElementById("tb_extension").tBodies[0];
								var totalRowCount = document.getElementById("tb_extension").rows.length;
								var lc = JSON.parse(document.getElementById("listclarity").value);
								var ls = JSON.parse(document.getElementById("listsize").value);
								var lp = JSON.parse(document.getElementById("listposition").value);
								
								var parrow = document.createElement("tr");
								
								var para = document.createElement("td");
								var awek = document.createElement("select");
								awek.setAttribute('class', 'form-control');
								awek.setAttribute('onchange', 'onchangeclar();');
								
								let aweks = [];
								let n;
								for(let i = 0; i <lc.length; i++){
									aweks[i] = document.createElement("option");
									aweks[i].setAttribute('value', lc[i].ID);
									 
									n= document.createTextNode(lc[i].Nama);
									aweks[i].appendChild(n);
									awek.appendChild(aweks[i])
								}
								
								para.appendChild(awek);
								parrow.appendChild(para);
								tbr.appendChild(parrow);
								
								var para1 = document.createElement("td");
								var awek1 = document.createElement("select");
								awek1.setAttribute('class', 'form-control');
								awek1.setAttribute('onchange', 'onchangesiz();');
								
								let aweks1 = [];
								let n1;
								for(let i = 0; i <ls.length; i++){
									aweks1[i] = document.createElement("option");
									aweks1[i].setAttribute('value', ls[i].ID);
									 
									n1 = document.createTextNode(ls[i].Nama);
									aweks1[i].appendChild(n1);
									awek1.appendChild(aweks1[i])
								}
								
								para1.appendChild(awek1);
								parrow.appendChild(para1);
								tbr.appendChild(parrow);
								
								var para0 = document.createElement("td");
								var awek0 = document.createElement("input");
								awek0.setAttribute('type','text');
								awek0.setAttribute('class','form-control');
								awek0.setAttribute('onblur', 'onkeyupnum();');
								para0.appendChild(awek0);
								parrow.appendChild(para0);
								tbr.appendChild(parrow);
								
								var para2 = document.createElement("td");
								var awek2 = document.createElement("select");
								awek2.setAttribute('class', 'form-control');
								awek2.setAttribute('onchange', 'onchangepos();');
								
								let aweks2 = [];
								let n2;
								for(let i = 0; i <lp.length; i++){
									aweks2[i] = document.createElement("option");
									aweks2[i].setAttribute('value', lp[i].ID);
									 
									n2= document.createTextNode(lp[i].Nama);
									aweks2[i].appendChild(n2);
									awek2.appendChild(aweks2[i])
								}
								
								para2.appendChild(awek2);
								parrow.appendChild(para2);
								tbr.appendChild(parrow);
								
								var para3 = document.createElement("td");
								var awek3 = document.createElement("button");
								awek3.setAttribute('type','button');
								awek3.setAttribute('class','btn btn-danger');
								awek3.innerHTML = 'Delete';
								awek3.setAttribute('onclick','removerowextension(this);');
								para3.appendChild(awek3);
								parrow.appendChild(para3);
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
							function extensionLD(clarity, position, number, size) {
								this.clarity = clarity;
								this.position = position;
								this.number = number;
								this.size = size;
							}
							
							function updaterecordhidden(tbl)
							{
								let extensionLDs = [];
								let i = 1
								for (i = 1; i <= tbl.rows.length - 1; i++) 
								{
									var row = tbl.rows[i];
									var cl = row.cells[0].children[0];
									var cltext = cl.options[cl.selectedIndex].text;
									
									var pos = row.cells[1].children[0];
									var postext = pos.options[pos.selectedIndex].text;
									
									var num = row.cells[2].children[0];
									var numtext = num.value;
									
									var siz = row.cells[3].children[0];
									var siztext = siz.options[siz.selectedIndex].text;
									
									
									let extensionLDz = new extensionLD(cltext,postext,numtext,siztext);
									extensionLDs.push(extensionLDz);
									//console.log(cltext);
								}
								//console.log(extensionLDs);
								finalextension.value = JSON.stringify(extensionLDs);
								//alert(finalextension.value);
							}
							
							function onchangeclar()
							{
								updaterecordhidden(document.getElementById("tb_extension"));
							}
							
							function onchangepos()
							{
								updaterecordhidden(document.getElementById("tb_extension"));
							}
							
							function onchangesiz()
							{
								updaterecordhidden(document.getElementById("tb_extension"));
							}
							
							function onkeyupnum()
							{
								updaterecordhidden(document.getElementById("tb_extension"));
							}
							
						</script>
					</div>
				</div>
			</div>
			<div class="button-left">
				<button id="save" runat="server" class="btn btn-success" onserverclick="save_Click">
					<i class="icon-check"></i>
					Submit
				</button>
				<small runat="server" id="lblwarning" class="text-danger" visible="false"></small>
			</div>
    </form>
</asp:Content>

