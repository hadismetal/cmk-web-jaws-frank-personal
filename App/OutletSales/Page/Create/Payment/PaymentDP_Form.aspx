<%@ Page Language="C#" MasterPageFile="~/MasterPageDP.master" Debug="true" AutoEventWireup="true" CodeFile="PaymentDP_Form.aspx.cs" Inherits="Page_Register_Sales_Form" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Down Payment
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <p>
        <span class="text-muted">Location</span><br />
        <span class="text-bold" id="warehouse" runat="server" />
    </p>
    <hr />
    <form runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-5 col-md-5">

                <table class="table-form">
                    <tr>
                        <td style="width:120px;"></td>
                        <td style="width:15px;"></td>
                        <td></td>
                    </tr>
                                        
                    <tr>
                        <td>Customer
                        </td>
                        <td>:</td>
                        <td>
                            <div class="input-group">
                                <div class="input-group-btn">
                                    <a href="PaymentDP_DataCustomer.aspx" class="btn btn-info" id="search_customer" runat="server">
                                        <i class="icon-search"></i>
                                    </a>
                                </div>
                                <asp:TextBox ID="customer" Enabled="false" runat="server" CssClass="form-control" MaxLength="50" Width="250" />
                                <asp:Label ID="lbvalidcustomer" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>JPJR
                        </td>
                        <td>:</td>
                        <td>
                            <div class="input-group">
                                <div class="input-group-btn">
                                    <a href="PaymentDP_DataAgent.aspx" class="btn btn-info" id="search_agent" runat="server">
                                        <i class="icon-search"></i>
                                    </a>
                                </div>
                                <asp:TextBox ID="agent" Enabled="false" runat="server" CssClass="form-control" MaxLength="50" Width="250" />
                                <asp:Label ID="lbvalidjr" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>Previous DP
                        </td>
                        <td>:</td>
                        <td>
                            <p id="p_input1" runat="server"><span style="color:blue;"><i>Silahkan menginput Customer dan JPJR terlebih dahulu</i></span></p>
                            <div class="input-group" runat="server" id="div_previous_dp">
                                <div class="input-group-btn">
                                    <a href="PaymentDP_Form_Add.aspx" class="btn btn-info" id="search_transaction" runat="server">
                                        <i class="icon-search"></i>
                                    </a>
                                </div>
                                <asp:TextBox ID="transac" Enabled="false" runat="server" CssClass="form-control" MaxLength="50" Width="250" />
                                <div class="input-group-btn">
                                    <asp:LinkButton ID="btn_cancel_previous_dp"
                                        runat="server" type="button"
                                        CssClass="btn btn-danger"
                                        OnClick="btn_cancel_previous_dp_Click">
                                                <span aria-hidden="true" class="glyphicon glyphicon-remove"></span>
                                    </asp:LinkButton>

                                </div>
                            </div>
                        </td>
                    </tr>

                    <tr>
                        <td>Down Payment
                        </td>
                        <td>:</td>
                        <td>
                            <p id="p_input2" runat="server"><span style="color:blue;"><i>Silahkan menginput Customer dan JPJR terlebih dahulu</i></span></p>
                            <div runat="server" id="div_presentation_tab">
                                <ul class="nav nav-pills" role="tablist">
                                    <li runat="server" id="li_dp_stock" role="presentation" class="active"><a style="padding: 8px 13px;" href="#content_dpst" aria-controls="dpst" role="tab" data-toggle="tab"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span>&nbsp;&nbsp;Stock</a></li>
                                    <li runat="server" id="li_dp_po" role="presentation"><a style="padding: 8px 13px;" href="#content_dppo" aria-controls="dppo" role="tab" data-toggle="tab"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span>&nbsp;&nbsp;Purchase Order</a></li>
                                </ul>
                            </div>
                            
                        </td>
                    </tr>
                </table>

            </div>
            <div class="col-xs-12 col-sm-4 col-md-4">
                <table class="table-form">
                    <tr>
                        <td style="width:120px;"></td>
                        <td style="width:15px;"></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>Date</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="tgl" runat="server" CssClass="form-control text-center" Width="110" />
                            <script>
                                $(document).ready(function () { $('#content_tgl').datepicker({ format: 'dd-mm-yyyy' }); });
                            </script>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="tgl_RF" runat="server" />
                            <span id="tgl_ERR" runat="server" class="text-validator" visible="false">Format</span>
                        </td>
                    </tr>
                    <tr>
                        <td>Price Estimation</td>
                        <td>:</td>
                        <td>
                            <div class="input-group">
                                <asp:TextBox ID="price_estimates" runat="server" CssClass="form-control text-center" Width="110" />
                                <span style="background-color:#EEEEEE;" class="input-group-addon">IDR</span>
                                <span runat="server" style="background-color:#EEEEEE;" id="span_plu_added" class="input-group-addon"><asp:Literal runat="server" ID="lit_plu_added" /></span>
                                <span runat="server" style="background-color:#EEEEEE;" id="span_plu_added_type" class="input-group-addon"><asp:Literal runat="server" ID="lit_plu_added_type" /></span>
                            </div>
                        </td>
                        <td>
                            <asp:RequiredFieldValidator ID="price_estimates_RF" runat="server" />
                            <span id="price_estimates_ERR" runat="server" class="text-validator" visible="false">Format</span>
                            <%--<asp:Label ID="lbprice" runat="server" Text="Please fill price estimates" ForeColor="Red" Visible="false"></asp:Label>--%>
                        </td>
                    </tr>

                    <tr>
                        <td>Keterangan</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="keterangan" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                        </td>
                    </tr>
					<tr>
                        <td>Free GWP (<small style="color:red">for Wedding Ring</small>)</td>
                        <td>:</td>
                        <td>
                            <asp:TextBox ID="txtletterwr" runat="server" CssClass="form-control" cols="20" OnTextChanged="setLetterWR" AutoPostBack="true"></asp:TextBox>
                        </td>
                    </tr>
                </table>

            </div>
            <div class="col-xs-12 col-sm-3 col-md-3">
                <div class="alert alert-danger" role="alert">
                    <i>Notes :</i>
                    <ul>
                        <li><i>Untuk menghindari terjadi lebih bayar ketika pelunasan, 1x transaksi pembayaran Down Payment hanya untuk 1 barang (baik untuk barang pesanan ataupun stock)</i></li>
                        <li><i>Hindari segala kemungkinan akan terjadinya pembayaran Down Payment lebih bayar terhadap Sales Order (pelunasan).</i></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="row">

            <div class="col-xs-12 col-sm-12 col-md-12" style="padding-bottom: 15px;">
                <hr />
                <div style="padding-top: 15px;" runat="server" id="div_dp_form">
                    <div class="tab-content">
                        <div role="tabpanel" class="tab-pane active" id="dpst" runat="server">
                            <div class="row">
                                
                                <div class="col-xs-12 col-sm-6 col-md-6">
                                    <asp:Panel runat="server" ID="pnl_addplu" DefaultButton="btn_search_cplu">
                                        <table class="table-form">

                                            <tr>
                                                <td>
                                                    Add PLU<br />
                                                    <asp:Label ID="Label1" runat="server" Text="(DJ / PG / GJ / LD)" CssClass="text-muted"></asp:Label>
                                                    <asp:Label ID="lbtipeplu" runat="server" CssClass="text-muted" Visible="false"></asp:Label>
                                                </td>
                                                <td>:</td>
                                                <td>
                                                    <div class="input-group">
                                                        <div class="input-group-btn">
                                                            <asp:LinkButton ID="btn_cancel_plu_stock"
                                                                runat="server" type="button"
                                                                CssClass="btn btn-danger"
                                                                OnClick="btn_cancel_plu_stock_Click">
                                                                        <span aria-hidden="true" class="glyphicon glyphicon-remove"></span>
                                                            </asp:LinkButton>

                                                        </div>
                                                        <asp:TextBox ID="cplu" runat="server" class="form-control text-center" Style="text-transform: uppercase;" aria-label="text input" />
                                                        <div class="input-group-btn">
                                                            <asp:LinkButton ID="btn_search_cplu"
                                                                runat="server" type="button"
                                                                CssClass="btn btn-primary"
                                                                OnClick="btn_search_cplu_Click">
                                                                        <span aria-hidden="true" class="glyphicon glyphicon-search"></span>
                                                            </asp:LinkButton>

                                                        </div>
                                                    </div>
                                                </td>
                                                <td>
                                                    <asp:Literal runat="server" ID="lit_status_found" />
                                                </td>
                                            </tr>
                                        
                                        </table>
                                    </asp:Panel>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-6">
                                    
                                    <asp:Literal ID="lbprice" runat="server"></asp:Literal>
                                    
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                                        <i class="icon-arrow-right"></i>&nbsp;Next Step

                                    </button>
                                    <a href="PaymentDP.aspx" class="btn btn-default">
                                        <i class="icon-remove"></i>&nbsp;Cancel

                                    </a>
                                </div>
                            </div>
                        </div>
                        <div role="tabpanel" class="tab-pane" id="dppo" runat="server">
                            <div class="row">
                                <div class="col-xs-12 col-sm-6 col-md-6">
                                    <table class="table-form">
                                        <tr>
                                            <td style="width:120px;"></td>
                                            <td style="width:15px;"></td>
                                            <td></td>
                                        </tr>
                                        
                                        <tr>
                                            <td>Frame Color</td>
                                            <td>:</td>
                                            <td>
                                                <asp:DropDownList ID="framecolour" runat="server" CssClass="form-control" Width="350" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Gold Weight</td>
                                            <td>:</td>
                                            <td>
                                                <div class="input-group">
                                                    <asp:Label ID="Label3" runat="server" style="background-color:#EEEEEE;" Text="Female" CssClass="form-control text-center"></asp:Label>
                                                    <asp:TextBox ID="gwf" runat="server" Text="0" CssClass="form-control text-center" Width="110"></asp:TextBox>

                                                    <asp:Label ID="Label4" runat="server" style="background-color:#EEEEEE;" Text="Male" CssClass="form-control text-center"></asp:Label>
                                                    <asp:TextBox ID="gwm" runat="server" Text="0" CssClass="form-control text-center" Width="110"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Diamond Weight</td>
                                            <td>:</td>
                                            <td>
                                                <div class="input-group">
                                                    <asp:Label ID="Label5" runat="server" style="background-color:#EEEEEE;" Text="Female" CssClass="form-control text-center"></asp:Label>
                                                    <asp:TextBox ID="dwf" runat="server" Text="0" CssClass="form-control text-center" Width="110"></asp:TextBox>

                                                    <asp:Label ID="Label6" runat="server" style="background-color:#EEEEEE;" Text="Male" CssClass="form-control text-center"></asp:Label>
                                                    <asp:TextBox ID="dwm" runat="server" Text="0" CssClass="form-control text-center" Width="110"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Ring Size</td>
                                            <td>:</td>
                                            <td>
                                                <div class="input-group">
                                                    <asp:Label ID="Label7" runat="server" style="background-color:#EEEEEE;" Text="Female" CssClass="form-control text-center"></asp:Label>
                                                    <asp:TextBox ID="rsf" runat="server" Text="0" CssClass="form-control text-center" Width="110"></asp:TextBox>

                                                    <asp:Label ID="Label8" runat="server" style="background-color:#EEEEEE;" Text="Male" CssClass="form-control text-center"></asp:Label>
                                                    <asp:TextBox ID="rsm" runat="server" Text="0" CssClass="form-control text-center" Width="110"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Inscription</td>
                                            <td>:</td>
                                            <td>
                                                <div>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="insf" runat="server" style="text-transform:uppercase;" CssClass="form-control text-left" MaxLength="20" placeholder="..." Width="250" />
                                                        <asp:Label ID="Label9" runat="server" style="background-color:#EEEEEE;" Text="Female" CssClass="form-control text-center"></asp:Label>
                                                    </div>
                                                </div>
                                                <div>
                                                    <div class="input-group">
                                                        <asp:TextBox ID="insm" runat="server" style="text-transform:uppercase;" CssClass="form-control text-left" MaxLength="20" placeholder="..." Width="250" />
                                                        <asp:Label ID="Label10" runat="server" style="background-color:#EEEEEE;" Text="Male" CssClass="form-control text-center"></asp:Label>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Font type</td>
                                            <td>:</td>
                                            <td>
                                                <asp:DropDownList ID="dfont" runat="server" CssClass="form-control"></asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Date of Delivery</td>
                                            <td>:</td>
                                            <td>
                                                <asp:TextBox ID="txdod" runat="server" CssClass="form-control text-center" Width="150" />

                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="col-xs-12 col-sm-6 col-md-6">
                                    <table class="table-form" style="width:100%;">
                                        <tr>
                                            <td style="width:140px;"></td>
                                            <td style="width:15px;"></td>
                                            <td></td>
                                        </tr>
                                
                                        <tr>
                                            <td>Sample PLU 1
                                                            <br />
                                                <asp:Label ID="Label2" runat="server" Text="(DJ / PG / GJ / LD)" CssClass="text-muted"></asp:Label></td>
                                            <td>:</td>
                                            <td>
                                                <div class="input-group col-md-6 no-padding">
                                                    <div class="input-group-btn">
                                                        <asp:LinkButton ID="btn_cancel_plu_po1"
                                                            runat="server" type="button"
                                                            CssClass="btn btn-danger"
                                                            OnClick="btn_cancel_plu_po1_Click">
                                                                    <span aria-hidden="true" class="glyphicon glyphicon-remove"></span>
                                                        </asp:LinkButton>

                                                    </div>
                                                    <asp:TextBox ID="cplu1" runat="server" CssClass="form-control text-center" Style="text-transform: uppercase;" />
                                                    <span class="input-group-btn">
                                                        <asp:LinkButton ID="btn_search_cplu1"
                                                            runat="server" type="button"
                                                            CssClass="btn btn-primary"
                                                            OnClick="btn_search_cplu1_Click">
                                                                        <span aria-hidden="true" class="glyphicon glyphicon-search"></span>
                                                        </asp:LinkButton>

                                                    </span>

                                                </div>
                                                <asp:Label ID="lbtipeplu1" runat="server" CssClass="text-muted" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td><asp:Literal ID="lbprice1" runat="server" /></td>
                                        </tr>
                                        <tr>
                                            <td>Sample PLU 2
                                                            <br />
                                                <asp:Label ID="Label11" runat="server" Text="(DJ / PG / GJ / LD)" CssClass="text-muted"></asp:Label></td>
                                            <td>:</td>
                                            <td>
                                                <div class="input-group col-md-6 no-padding">
                                                    <div class="input-group-btn">
                                                        <asp:LinkButton ID="btn_cancel_plu_po2"
                                                            runat="server" type="button"
                                                            CssClass="btn btn-danger"
                                                            OnClick="btn_cancel_plu_po2_Click">
                                                                    <span aria-hidden="true" class="glyphicon glyphicon-remove"></span>
                                                        </asp:LinkButton>

                                                    </div>
                                                    <asp:TextBox ID="cplu2" runat="server" CssClass="form-control text-center" Style="text-transform: uppercase;" />
                                                    <span class="input-group-btn">
                                                        <asp:LinkButton ID="btn_search_cplu2"
                                                            runat="server" type="button"
                                                            CssClass="btn btn-primary"
                                                            OnClick="btn_search_cplu2_Click">
                                                                        <span aria-hidden="true" class="glyphicon glyphicon-search"></span>
                                                        </asp:LinkButton>
                                                    </span>

                                                </div>
                                                <asp:Label ID="lbtipeplu2" runat="server" CssClass="text-muted" Visible="false"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                            <td></td>
                                            <td><asp:Literal ID="lbprice2" runat="server"></asp:Literal></td>
                                        </tr>
                                        <tr runat="server" id="tr_upload">
                                            <td>Sketch</td>
                                            <td>:</td>
                                            <td>
                                                <div class="input-group col-md-12 no-padding">
                                                    
                                                    <asp:FileUpload ID="ske_up" runat="server" CssClass="form-control" />
                                                    <span class="input-group-btn">
                                                        <button id="rev" runat="server" class="btn btn-success" onserverclick="uploadsketch_click">
                                                            <i class="icon-arrow-up"></i>
                                                            Upload
                                                        </button>
                                                    </span>
                                                </div>
                                                <div>
                                                    <p>
                                                        <span id="ske_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                                        <span id="ske_format" runat="server" class="text-muted text-sm" />
                                                        <span id="dir_ERR" runat="server" class="text-validator" visible="false">Folder Error : </span>
                                                    </p>
                                                </div>
                                            </td>
                                            
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Image ID="imgpicture" runat="server" CssClass="img-thumbnail" style="max-width:100%;" />
                                            </td>
                                        </tr>
                                        
                                    </table>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-12 col-md-12">
                                    <button id="save2" runat="server" class="btn btn-primary" onserverclick="save_Click">
                                        <i class="icon-arrow-right"></i>&nbsp;Next Step</button>
                                    <a href="PaymentDP.aspx" class="btn btn-default">
                                        <i class="icon-remove"></i>&nbsp;Cancel</a>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </form>
    <label id="lbmode" runat="server" style="color: transparent"></label>
    <asp:Label ID="lbid" runat="server" Style="color: transparent"></asp:Label>
    <asp:Label ID="lbstatus" runat="server" Style="color: transparent"></asp:Label>
    <asp:Image ID="imgdata" runat="server" />
    <asp:Label ID="lbdataimage" runat="server"></asp:Label>
    <image id="img_exp1"></image>
    <script type="text/javascript">
        // Send the canvas image to the server.

        function cekimage() {
            var image = document.getElementById("graph").toDataURL("image/png");
            image = image.replace('data:image/png;base64,', '');

            $.ajax({

                type: 'POST',

                url: '/Page/Create/Payment/PaymentDP_Form.aspx?id=' + id + '&status=' + status + '/UploadSketch',

                data: '{ "imageData" : "' + image + '" }',

                contentType: 'application/json; charset=utf-8',

                dataType: 'json',

                success: function (msg) {

                    alert('Image saved successfully !');

                }
            });
            alert('save =' + image);
        }
        function tes() {
            var image = document.getElementById("graph").toDataURL("image/png");
            //document.getElementById("img_exp1").src = image;
            $.ajax({
                type: 'POST',
                url: 'PaymentDP_Form.aspx?id=' + id + '&status=' + status + '/UploadSketch',
                data: '{ "imageData :" "' + image + '" }',
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                success: function (msg) {
                    alert('Image saved successfully !');
                }
            });
            alert("save image =" + image);
            // document.getElementById("img_exp").src(image);
        }
    </script>
    <script type="text/javascript">


        var currentDate = new Date();
        var day = currentDate.getDate() + 30;
        var month = currentDate.getMonth() + 1;
        var year = currentDate.getFullYear();
        var datenow = (year + "-" + month + "-" + day + "</b>");

        $(document).ready(function () {
            $('#<%=txdod.ClientID%>').datepicker({
            format: 'dd-mm-yyyy',
            startDate: '+30d'
        });
        $('#<%=txdod.ClientID%>').datepicker('setStartDate', 'datenow');


    });
    </script>
    <script src="/UI/JS/signature_pad.js"></script>
    <script src="/UI/JS/app.js"></script>

    <script type="text/javascript">
        $('#myTabs a').click(function (e) {
            e.preventDefault()
            $(this).tab('show')
        })

        $('#myTabs a[href="#profile"]').tab('show') // Select tab by name
        $('#myTabs a:first').tab('show') // Select first tab
        $('#myTabs a:last').tab('show') // Select last tab
        $('#myTabs li:eq(2) a').tab('show') // Select third tab (0-indexed)
    </script>
</asp:Content>
