<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="3.aspx.cs" inherits="Page_Create_3" %>

<%@ register src="Menu3.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Precious Stone
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Create New
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<uc1:menu id="menu" runat="server" />
	<form runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-4 col-md-4">
                <table class="table-form">
			        <tr>
				        <td>Item</td>
				        <td>:</td>
				        <td>
					        <asp:dropdownlist id="parcel01" runat="server" cssclass="form-control text-bold" width="275" />
				        </td>
				        <td>
					        <span id="parcel01_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				        </td>
			        </tr>
			        <tr>
				        <td>Shape</td>
				        <td>:</td>
				        <td>
					        <asp:dropdownlist id="parcel02" runat="server" cssclass="form-control text-bold" width="275" />
				        </td>
				        <td>
					        <span id="parcel02_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				        </td>
			        </tr>
                    <tr>
				        <td>Size</td>
				        <td>:</td>
				        <td>
					        <asp:dropdownlist id="parcel03" runat="server" cssclass="form-control text-bold" width="275" />
				        </td>
				        <td>
					        <span id="parcel03_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				        </td>
			        </tr>
	
                    <tr>
				        <td>Colour</td>
				        <td>:</td>
				        <td>
					        <asp:dropdownlist id="parcel04" runat="server" cssclass="form-control text-bold" width="275" />
				        </td>
				        <td>
					        <span id="parcel04_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				        </td>
			        </tr>
            
                    <tr>
				        <td>Clarity</td>
				        <td>:</td>
				        <td>
					        <asp:dropdownlist id="parcel06" runat="server" cssclass="form-control text-bold" width="275" />
				        </td>
				        <td>
					        <span id="parcel06_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				        </td>
			        </tr>
                </table>
            </div>
            <div class="col-xs-12 col-sm-4 col-md-4">
                <table class="table-form">
                    <tr style="visibility:hidden">
				        <td>Colour Grading</td>
				        <td>:</td>
				        <td>
					        <asp:dropdownlist id="parcel05" runat="server" cssclass="form-control text-bold" width="275" />
				        </td>
				        <td>
					        <span id="parcel05_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				        </td>
			        </tr>
            
                    <tr style="visibility:hidden">
				        <td>Colour Comment</td>
				        <td>:</td>
				        <td>
					        <asp:dropdownlist id="parcel09" runat="server" cssclass="form-control text-bold" width="275" />
				        </td>
				        <td>
					        <span id="parcel09_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				        </td>
			        </tr>
            
                    <tr style="visibility:hidden">
				        <td>Brilliance</td>
				        <td>:</td>
				        <td>
					        <asp:dropdownlist id="parcel10" runat="server" cssclass="form-control text-bold" width="275" />
				        </td>
				        <td>
					        <span id="parcel10_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				        </td>
			        </tr>
            
                    <tr style="visibility:hidden">
				        <td>Treatment</td>
				        <td>:</td>
				        <td>
					        <asp:dropdownlist id="parcel07" runat="server" cssclass="form-control text-bold" width="275" />
				        </td>
				        <td>
					        <span id="parcel07_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				        </td>
			        </tr>
            
                    <tr style="visibility:hidden">
				        <td>Proportions</td>
				        <td>:</td>
				        <td>
					        <asp:dropdownlist id="parcel11" runat="server" cssclass="form-control text-bold" width="275" />
				        </td>
				        <td>
					        <span id="parcel11_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				        </td>
			        </tr>
			
		        </table>
            </div>
            <div class="col-xs-12 col-sm-4 col-md-4">
                <table class="table-form">
                    
                    <tr style="visibility:hidden">
				        <td>Country of Origin</td>
				        <td>:</td>
				        <td>
					        <asp:dropdownlist id="parcel08" runat="server" cssclass="form-control text-bold" width="275" />
				        </td>
				        <td>
					        <span id="parcel08_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				        </td>
			        </tr>
            
                    <tr style="visibility:hidden">
				        <td>Supplier</td>
				        <td>:</td>
				        <td>
					        <asp:dropdownlist id="parcel12" runat="server" cssclass="form-control text-bold" width="275" />
				        </td>
				        <td>
					        <span id="parcel12_ERR" runat="server" class="text-validator" visible="false">Belum Setup</span>
				        </td>
			        </tr>
                </table>
            </div>
        </div>
		
        
		<hr />
		<div class="button-inline">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<span id="kodeparcel_ERR" runat="server" class="text-validator" visible="false" style="color:red;">Kode Parcel Sudah Digunakan</span>
		</div>
	</form>
</asp:content>
