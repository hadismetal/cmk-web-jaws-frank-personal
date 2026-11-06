<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DJDO_Detail_Wholesale.aspx.cs" Inherits="Page_Register_DJDO_DJDO_Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="Server">
    Diamond Jewelry
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="Server">
    Register
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="Server">
    <div runat="server" id="div_info">
        <div class="row">
            <div class="col-xs-12 col-sm-12 col-md-12">
                <div class="col-xs-12 col-sm-6 col-md-3">
                    <span class="text-muted">Delivery Order Number</span><br />
                    <span class="text-lg" id="number_deliveryorder" runat="server" />

                </div>
                <div class="col-xs-12 col-sm-6 col-md-3">
                    <span class="text-muted">Section Number</span><br />
                    <span class="text-lg" id="number_section" runat="server" />
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3">
                    <span class="text-muted">Validation Number</span><br />
                    <span class="text-lg" id="number_validation" runat="server" />
                </div>
                <div class="col-xs-12 col-sm-6 col-md-3">
                    <span class="text-muted">Item Number</span><br />
                    <span class="text-lg" id="number_item" runat="server" />
                </div>

            </div>
        </div>
    </div>

    <form runat="server">

        <div class="button-inline">
            <a href="DJDO.aspx" id="btn_back" runat="server" class="btn btn-info">
                <i class="icon-arrow-left"></i>
                Back to Delivery Order List
            </a>
            <br />
            <br />
        </div>
        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="panel panel-info" id="div_pinfo">
                    <div class="panel-heading">
                        <h3 class="panel-title">Product Information</h3>
                    </div>
                    <div class="panel-body">
                                <table class="table-form">
                                    <tr>
                                        <td class="fixed-150">No. Prototype<br />
                                            <i class="text-muted">Kode Karet</i>
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="protonomor" runat="server" CssClass="form-control" MaxLength="50" Width="200" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="protonomor_RF" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fixed-150">No. Product Order<br />
                                            <i class="text-muted">No. SPK</i>
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="productordernomor" runat="server" CssClass="form-control" MaxLength="50" Width="200" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fixed-150">No. Supplier<br />
                                            <i class="text-muted">Kode Barang</i>
                                        </td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="suppliernomor" runat="server" CssClass="form-control" MaxLength="50" Width="200" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fixed-150">Expired Product</td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="expiredproduct" runat="server" CssClass="form-control text-center" Width="110" />
                                            <script>
                                                $(document).ready(function () { $('#content_expiredproduct').datepicker({ format: 'dd-mm-yyyy' }); });
                                            </script>
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="expiredproduct_RF" runat="server" />
                                            <span id="expiredproduct_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fixed-150">Expired Request</td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="expiredrequest" runat="server" CssClass="form-control text-center" Width="110" />
                                            <script>
                                                $(document).ready(function () { $('#content_expiredrequest').datepicker({ format: 'dd-mm-yyyy' }); });
                                            </script>
                                        </td>
                                        <td>
                                            <span id="expiredrequest_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fixed-150">Supplier <span style="color:red;">*</span></td>
                                        <td>:</td>
                                        <td>
                                            <div class="input-group">
                                                <div class="input-group-btn">
                                                    <a href="DJDO_DataSupplier.aspx" class="btn btn-info" id="search_supplier" runat="server">
                                                        <i class="icon-search"></i>
                                                    </a>
                                                </div>
                                                <asp:TextBox ID="suppliernama" runat="server" CssClass="form-control" MaxLength="50" Width="160" Enabled="false" />
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fixed-150">Request Outlet</td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="requestoutletnama" runat="server" CssClass="form-control" Enabled="false" MaxLength="50" Width="200" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fixed-150">Request Customer</td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="requestcustomernama" runat="server" CssClass="form-control" Enabled="false" MaxLength="50" Width="200" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fixed-150">Designer</td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="humandesignernama" runat="server" CssClass="form-control" Enabled="false" MaxLength="50" Width="200" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="fixed-150">Stock Keeper</td>
                                        <td>:</td>
                                        <td>
                                            <asp:TextBox ID="humanstocknama" runat="server" CssClass="form-control" MaxLength="50" Width="200" />
                                        </td>
                                        <td>
                                            <asp:RequiredFieldValidator ID="humanstocknama_RF" runat="server" />
                                        </td>
                                    </tr>

                                </table>


                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="panel panel-info" id="div_picture">
                    <div class="panel-heading">
                        <h3 class="panel-title">Picture</h3>
                    </div>
                    <div class="panel-body">
                        <div>

                            <table class="table-form table-form-nomargin">
                            
                                <tr>
                                    <td class="fixed-150">Frame Color</td>
                                    <td>:</td>
                                    <td>
                                        <asp:DropDownList ID="framecolor" runat="server" CssClass="form-control" Width="275" OnSelectedIndexChanged="framecolor_SelectedIndexChanged" AutoPostBack="true"/>
                                    </td>
                                    <td>
                                        <span id="framecolor_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                                    </td>
                                </tr>
                            
                            </table>
                            <br />
                            <button id="btn_framecolor_update" runat="server" class="btn btn-warning" onserverclick="framecolorupdate_Click" disabled style="display:none">
                                <%--<i class="icon-arrow-right"></i>--%>
				                Update
                            </button>
                        </div>
                        <hr />

                        <div class="row">
                            <div class="col-xs-12 col-sm-12 col-md-6">
                                <table class="table-form">
                                    <tr>
                                        <td>1.</td>
                                        <td>
                                            <asp:Literal runat="server" ID="lit_filename_1" /></td>
                                        <td>
                                            <asp:Literal runat="server" ID="lit_fileexist_1" />
                                            <button id="btn_filename_1" runat="server" style="border:none; padding:1px 4px;" class="btn btn-success" onserverclick="btn_filename_1_Click">
                                                <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                                            </button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>2.</td>
                                        <td>
                                            <asp:Literal runat="server" ID="lit_filename_2" /></td>
                                        <td>
                                            <asp:Literal runat="server" ID="lit_fileexist_2" />
                                            <button id="btn_filename_2" runat="server" style="border:none; padding:1px 4px;" class="btn btn-success" onserverclick="btn_filename_2_Click">
                                                <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                                            </button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>3.</td>
                                        <td>
                                            <asp:Literal runat="server" ID="lit_filename_3" /></td>
                                        <td>
                                            <asp:Literal runat="server" ID="lit_fileexist_3" />
                                            <button id="btn_filename_3" runat="server" style="border:none; padding:1px 4px;" class="btn btn-success" onserverclick="btn_filename_3_Click">
                                                <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                                            </button>
                                        </td>
                                    </tr>
                                </table>
                                
                            </div>
                            <div class="col-xs-12 col-sm-12 col-md-6">
                                <table class="table-form">
                                    <tr>
                                        <td>4.</td>
                                        <td>
                                            <asp:Literal runat="server" ID="lit_filename_4" /></td>
                                        <td>
                                            <asp:Literal runat="server" ID="lit_fileexist_4" />
                                            <button id="btn_filename_4" runat="server" style="border:none; padding:1px 4px;" class="btn btn-success" onserverclick="btn_filename_4_Click">
                                                <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                                            </button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>5.</td>
                                        <td>
                                            <asp:Literal runat="server" ID="lit_filename_5" /></td>
                                        <td>
                                            <asp:Literal runat="server" ID="lit_fileexist_5" />
                                            <button id="btn_filename_5" runat="server" style="border:none; padding:1px 4px;" class="btn btn-success" onserverclick="btn_filename_5_Click">
                                                <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                                            </button>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>6.</td>
                                        <td>
                                            <asp:Literal runat="server" ID="lit_filename_6" /></td>
                                        <td>
                                            <asp:Literal runat="server" ID="lit_fileexist_6" />
                                            <button id="btn_filename_6" runat="server" style="border:none; padding:1px 4px;" class="btn btn-success" onserverclick="btn_filename_6_Click">
                                                <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                                            </button>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                        <hr />

                        <p class="text-muted">
                            Max. Size : 
			                <span id="kb" runat="server" class="text-bold text-danger" />
                        </p>
                        <table class="table-form">
                            <tr>
                                <td><asp:FileUpload ID="imgpicture" runat="server" CssClass="form-control" /></td>
                                <td>
                                    <button id="btn_Picture_save" runat="server" class="btn btn-info" onserverclick="btn_Picture_save_Click">
                                        <span class="glyphicon glyphicon-open" aria-hidden="true"></span>
                                    </button>
                                </td>
                                <td>
                                    <span id="imgpicture_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                </td>
                                <td id="imgpicture_format" runat="server" class="text-muted text-sm" />
                            </tr>
                        </table>
                        
                        <span id="dir_ERR" runat="server" class="text-validator" visible="false">Folder Error : </span>
                        <hr />
                        
                        <p class="text-muted">
                            Filename : 
			                <span id="img_filename" runat="server" class="text-bold text-danger" />
                        </p>
                        <div runat="server" id="div_pic" visible="false">
                            <asp:Image ID="imgpicture1" runat="server" CssClass="img-thumbnail img-responsive" />
                        </div>
                        <span id="img_ERR" runat="server" class="text-validator" visible="false">Image not found. Please upload the image file with same filename (as shown above) to the server using FTP.</span>
                    </div>
                </div>
            </div>
        </div>

        
        <div class="panel panel-info" id="div_stone">
            <div class="panel-heading">
                <h3 class="panel-title">Stone</h3>
            </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-md-3">
                        <p>
                            <span class="text-muted">Gross Weight</span><br />
                            <span class="text-lg" id="grossweight" runat="server" />
                            <small class="text-silver">Gram</small>
                        </p>
                    </div>
                    <div class="col-md-3">
                        <p>
                            <span class="text-muted">Stone Carat</span><br />
                            <span class="text-lg" id="stonecarat" runat="server" />
                            <small class="text-silver">Carat</small>
                        </p>
                    </div>
                    <div class="col-md-3">
                        <p>
                            <span class="text-muted">Stone Weight</span><br />
                            <span class="text-lg" id="stoneweight" runat="server" />
                            <small class="text-silver">Gram</small>
                        </p>
                    </div>
                    <div class="col-md-3">
                        <p>
                            <span class="text-muted">Netto Weight</span><br />
                            <span class="text-lg" id="nettoweight" runat="server" />
                            <small class="text-silver">Gram</small>
                        </p>
                    </div>
                </div>
                <div class="table-responsive">
                    <asp:Table ID="tb" runat="server" CssClass="table-list">
                        <asp:TableHeaderRow>
                            <asp:TableHeaderCell Width="180">
                                <a id="add" runat="server" class="btn btn-success" href="DJDO_StoneAdd.aspx">
                                    <i class="icon-plus"></i>
                                    Add Stone
                                </a>
                            </asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="250">Stone</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="70" CssClass="text-center">P</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="70" CssClass="text-center">L</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="70" CssClass="text-center">T</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="130" CssClass="text-right">Carat / Butir</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="130" CssClass="text-center">Total Butir</asp:TableHeaderCell>
                            <asp:TableHeaderCell Width="130" CssClass="text-right">Total Carat</asp:TableHeaderCell>
                        </asp:TableHeaderRow>
                    </asp:Table>
                    <asp:Label ID="lbbutir" runat="server" Visible="false"></asp:Label>
                </div>
            </div>
        </div>


        <div class="panel panel-info">
            <div class="panel-heading">
                <h3 class="panel-title">Product Design</h3>
            </div>
            <div class="panel-body">
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <table class="table-form table-form-nomargin">
                            <tr>
                                <td class="fixed-150">Design Category</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="designcategory" runat="server" CssClass="form-control" Width="275" />
                                </td>
                                <td>
                                    <span id="designcategory_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="fixed-150">Design Concept</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="designconcept" runat="server" CssClass="form-control" Width="275" />
                                </td>
                                <td>
                                    <span id="designconcept_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="fixed-150">Design Process</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="designprocess" runat="server" CssClass="form-control" Width="275" />
                                </td>
                                <td>
                                    <span id="designprocess_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="fixed-150">Target Age</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="targetage" runat="server" CssClass="form-control" Width="275" />
                                </td>
                                <td>
                                    <span id="targetage_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="fixed-150">Target Gender</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="targetgender" runat="server" CssClass="form-control" Width="275" />
                                </td>
                                <td>
                                    <span id="targetgender_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <div class="form-group width-300">
                            <label>
                                Naming Product :
                            </label>
                            <asp:TextBox ID="naming" runat="server" CssClass="form-control" MaxLength="50" />
                        </div>
                        <div class="form-group cb-single">
                            <label>
                                Keterangan :
                            </label>
                            <br />
                            <asp:CheckBox ID="setitem" runat="server" />
                            <label for="content_setitem">
                                <strong>Set Item</strong>
                            </label>
                            <br />
                            <asp:CheckBox ID="special" runat="server" />
                            <label for="content_special">
                                <strong>Special Design</strong>
                            </label>
                        </div>
                        <div class="form-group width-300">
                            <label>
                                Grafir :
                            </label>
                            <asp:TextBox ID="grafir" runat="server" CssClass="form-control" MaxLength="38" />
                            <i class="text-muted">Max. 38 karakter</i>
                        </div>
                    </div>
                </div>

            </div>
        </div>


        <div class="panel panel-info" id="div_pchar">
            <div class="panel-heading">
                <h3 class="panel-title">Product Charateristic</h3>
            </div>
            <div class="panel-body">
                <%--<asp:UpdatePanel ID="upPrinceOffuce" runat="server">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="ddlPrintOffice" EventName="SelectedIndexChanged" />
                    </Triggers>
                    <ContentTemplate>
                         <asp:DropDownList ID="ddlPrintOffice" runat="server" ClientIDMode="Static" AutoPostBack="true" ...blah blah /></asp:DropDownList>
                    </ContentTemplate>
                
                </asp:UpdatePanel>--%>
                <div class="row">

                    <div class="col-md-6">
                        <table class="table-form table-form-nomargin">
                            <tr>
                                <td class="fixed-150">Product Item</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="productitem" runat="server" CssClass="form-control" Width="275" />
                                </td>
                                <td>
                                    <span id="productitem_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="fixed-150">Product Category</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="productcategory" runat="server" CssClass="form-control" Width="275"
                                        AutoPostBack="true" OnSelectedIndexChanged="fillDist_charproduct01" />
                                </td>
                                <td>
                                    <span id="productcategory_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="fixed-150">Product Level</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="productlevel" runat="server" CssClass="form-control" Width="275"
                                        AutoPostBack="true" OnSelectedIndexChanged="fillDist_charproduct01" />
                                </td>
                                <td>
                                    <span id="productlevel_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="fixed-150">Stone Distribution</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="stonedist" runat="server" CssClass="form-control" Width="275" />
                                </td>
                                <td>
                                    <span id="stonedist_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br class="visible-sm visible-xs" />
                    <div class="col-md-6">
                        <table class="table-form table-form-nomargin">
                            <tr>
                                <td class="fixed-150">Frame Material</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="framematerial" runat="server" CssClass="form-control" Width="275" />
                                </td>
                                <td>
                                    <span id="framematerial_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                                </td>
                            </tr>
                            <tr>
                                <td class="fixed-150">Frame Finishing</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="framefinishing" runat="server" CssClass="form-control" Width="275" />
                                </td>
                                <td>
                                    <span id="framefinishing_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                                </td>
                            </tr>
                            
                            <tr>
                                <td class="fixed-150">Construction Process</td>
                                <td>:</td>
                                <td>
                                    <asp:DropDownList ID="processcons" runat="server" CssClass="form-control" Width="275" />
                                </td>
                                <td>
                                    <span id="processcons_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
                    
                <br />
                <hr />
                <div class="row">
                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <table class="table-form">
                            <tr>
                                <td>Gross Weight</td>
                                <td>:</td>
                                <td>
                                    <div class="input-group">
                                        <asp:TextBox ID="gweight" runat="server" CssClass="form-control text-lg text-center" Width="110" />
                                        <span class="input-group-addon">Gram</span>
                                    </div>
                                </td>
                                <td>
                                    <span id="grossweight_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                    <span id="grossweight_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
                                    <asp:RequiredFieldValidator ID="gweight_RF" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>Kadar Logam</td>
                                <td>:</td>
                                <td>
                                    <div class="input-group">
                                        <asp:TextBox ID="kadarlogam" runat="server" CssClass="form-control text-lg text-center" Width="110" />
                                        <span class="input-group-addon">%</span>
                                    </div>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="kadarlogam_RF" runat="server" />
                                    <span id="kadarlogam_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                    <span id="kadarlogam_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Kadar Tukaran</td>
                                <td>:</td>
                                <td>
                                    <div class="input-group">
                                        <asp:TextBox ID="kadartukaran" runat="server" CssClass="form-control text-lg text-center" Width="110" />
                                        <span class="input-group-addon">%</span>
                                    </div>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="kadartukaran_RF" runat="server" />
                                    <span id="kadartukaran_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                    <span id="kadartukaran_VAL" runat="server" class="text-validator" visible="false">Nilai Positif</span>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div class="col-xs-12 col-sm-6 col-md-6">
                        <table class="table-form">
                            <tr>
                                <td>Diameter</td>
                                <td>:</td>
                                <td>
                                    <div class="input-group">
                                        <asp:TextBox ID="dimensid" Text="0" runat="server" CssClass="form-control text-lg text-center" Width="125" />
                                        <%--span class="input-group-addon">MM</--%>
                                    </div>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="dimensid_RF" runat="server" />
                                    <span id="dimensid_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Panjang</td>
                                <td>:</td>
                                <td>
                                    <div class="input-group">
                                        <asp:TextBox ID="dimensip" Text="0" runat="server" CssClass="form-control text-lg text-center" Width="125" />
                                        <%--span class="input-group-addon">MM</--%>
                                    </div>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="dimensip_RF" runat="server" />
                                    <span id="dimensip_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Lebar</td>
                                <td>:</td>
                                <td>
                                    <div class="input-group">
                                        <asp:TextBox ID="dimensil" Text="0" runat="server" CssClass="form-control text-lg text-center" Width="125" />
                                        <%--span class="input-group-addon">MM</--%>
                                    </div>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="dimensil_RF" runat="server" />
                                    <span id="dimensil_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Ring Size / Panjang Chain</td>
                                <td>:</td>
                                <td>
                                    <div class="input-group">
                                        <asp:TextBox ID="dimensir" Text="0" runat="server" CssClass="form-control text-lg text-center" Width="125" />
                                        <%--span class="input-group-addon">MM</--%>
                                    </div>
                                </td>
                                <td>
                                    <span id="dimensir_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>


        <div class="row">
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        <h3 class="panel-title">Costing</h3>
                    </div>
                    <div class="panel-body">
                        
                        <table class="table-form">
                            <tr id="unitprice_Row" runat="server">
                                <td>Unit Price</td>
                                <td>:</td>
                                <td>
                                    <div class="input-group">
                                        <asp:TextBox ID="unitprice" runat="server" CssClass="form-control text-lg text-right" Width="175" />
                                        <span class="input-group-addon">IDR</span>
                                    </div>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="unitprice_RF" runat="server" />
                                    <span id="unitprice_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Mounting NO</td>
                                <td>:</td>
                                <td>
                                    <div class="input-group">
                                        <asp:TextBox ID="mountingno" Text="0" runat="server" CssClass="form-control text-lg text-right" Width="175" />
                                        <span class="input-group-addon">IDR</span>
                                    </div>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="mountingno_RF" runat="server" />
                                    <span id="mountingno_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Mounting RO</td>
                                <td>:</td>
                                <td>
                                    <div class="input-group">
                                        <asp:TextBox ID="mountingro" Text="0" runat="server" CssClass="form-control text-lg text-right" Width="175" />
                                        <span class="input-group-addon">IDR</span>
                                    </div>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="mountingro_RF" runat="server" />
                                    <span id="mountingro_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Pembagi</td>
                                <td>:</td>
                                <td>
                                    <div class="input-group">
                                        <asp:TextBox ID="pembagi" runat="server" CssClass="form-control text-lg text-center" Width="100" />
                                    </div>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="pembagi_RF" runat="server" />
                                    <span id="pembagi_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Pasang Batu</td>
                                <td>:</td>
                                <td>
                                    <div class="input-group">
                                        <asp:TextBox ID="setting" runat="server" Text="0" CssClass="form-control text-lg text-right" Width="175" />
                                        <span class="input-group-addon">IDR</span>
                                    </div>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="setting_RF" runat="server" />
                                    <span id="setting_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                </td>
                            </tr>

                            <tr>
                                <td>Ongkos Lain-lain
                            <br />
                                    <small class="text-muted">COGS</small></td>
                                <td>:</td>
                                <td>
                                    <div class="input-group">
                                        <asp:TextBox ID="ongkoslaincogs" runat="server" Text="0" CssClass="form-control text-lg text-right" Width="175" />
                                        <span class="input-group-addon">IDR</span>
                                    </div>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="ongkoslaincogs_RF" runat="server" />
                                    <span id="ongkoslaincogs_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                </td>
                            </tr>
                            <tr>
                                <td>Ongkos Lain-lain</td>
                                <td>:</td>
                                <td>
                                    <div class="input-group">
                                        <asp:TextBox ID="ongkoslain" runat="server" Text="0" CssClass="form-control text-lg text-right" Width="175" />
                                        <span class="input-group-addon">IDR</span>
                                    </div>
                                </td>
                                <td>
                                    <asp:RequiredFieldValidator ID="ongkoslain_RF" runat="server" />
                                    <span id="ongkoslain_ERR" runat="server" class="text-validator" visible="false">Format</span>
                                </td>
                            </tr>
                        </table>

                    </div>
                </div>
            </div>
            <div class="col-xs-12 col-sm-6 col-md-6">
                <div class="panel panel-info">
                    <div class="panel-heading">
                        <h3 class="panel-title">Finishing</h3>
                    </div>
                    <div class="panel-body">
                        <div>
                            <i>Notes :</i>
                            <ul>
                                <li><i>Default kolom kosong untuk tidak menghitung ongkos finishing</i></li>
                                <li><i>Berikan angka 0 untuk menghitung ongkos jual secara otomatis</i></li>
                            </ul>
                        </div>
                        <hr />
                        <asp:Table ID="tb_finishing" runat="server" class="table-form"></asp:Table>
                    </div>
                </div>
            </div>
        </div>



        <div>
            <asp:Literal runat="server" ID="lit_test_msg" />
        </div>

        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-warning" onserverclick="confirm_Click">
                <%--<i class="icon-arrow-right"></i>--%>
				Confirm
            </button>
            <span id="supplier_ERR" runat="server" class="text-validator" visible="false">Missing required field : Supplier</span>
        </div>

        <div runat="server" id="div_confirm" visible="false">
            <div class="alert alert-danger" role="alert">
                <div class="row">
                    <div class="col-xs-12 col-sm-6" style="padding-bottom: 5px;">
                        <b>Are you sure this data was valid ?</b>
                    </div>

                    <div class="col-xs-12 col-sm-4">
                        <div class="col-xs-12" style="padding: 0 0 5px 0;">
                            <button id="cancel" runat="server" style="width: 100%;" class="btn btn-danger" onserverclick="cancel_Click">
                                <span class="glyphicon glyphicon-remove" aria-hidden="true"></span>
                                &nbsp;&nbsp;No, I'd like to revise my action.
                            </button>
                        </div>
                        <div class="col-xs-12" style="padding: 0 0 5px 0;">
                            <button id="confirm" runat="server" style="width: 100%;" class="btn btn-success" onserverclick="save_Click">
                                <span class="glyphicon glyphicon-ok" aria-hidden="true"></span>
                                &nbsp;&nbsp;Yes, I'm sure. Please continue.
                            </button>
                        </div>
                    </div>
                </div>

            </div>

        </div>

    </form>
    <script type="text/javascript">
        function goDelete(fooid, fosi, footid, fodo) {
            txtConfirm =
				"Lanjutkan Proses DELETE ?\n\n" +
				"Penghapusan data / transaksi sifatnya adalah permanen.\n" +
				"Dianjurkan untuk melakukan backup terlebih dahulu.";

            if (confirm(txtConfirm))
                location.href = 'DJDO_StoneDelete.aspx?id=' + fooid + '&nodo=' + fodo + '&si=' + fosi + '&tid=' + footid + '#div_stone';
        }
    </script>
</asp:Content>


