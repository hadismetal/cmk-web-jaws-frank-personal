<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="index.aspx.cs" inherits="Page_Report_CalcPricingStone_index" %>

<asp:content contentplaceholderid="h1" runat="server">
	Kalkulator Pricing Stone
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Report
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
	<form runat="server" class="form-inline">		
		<script type="text/javascript">
			toastr.options = {
    			closeButton: true,
    			progressBar: true,
    			positionClass: "toast-bottom-center",
    			showDuration: "300",
    			hideDuration: "1000",
    			timeOut: "3000",
    		};
			
		
		function f_oclBtn(){
			var sBrand = document.getElementById('<%= parcel06.ClientID %>');
			var btnSave = document.getElementById("<%= go.ClientID %>");
			 
			if (sBrand.value == "Stone Brand :") {
					toastr.error(`Stone Brand Harap diisi`);
			}else{
					btnSave.click();
			}
		}

		function f_ocCarat(){
			var dropdown = document.getElementById('<%= parcel02.ClientID %>');
            var textbox = document.getElementById('<%= kw.ClientID %>');            
    		var selectedValue = dropdown.options[dropdown.selectedIndex].value;			
            var txt = dropdown.options[dropdown.selectedIndex].text;
			var btnSave = document.getElementById("<%= go.ClientID %>");
			var btnLimbat = document.getElementById("limbat");
			console.log(btnLimbat)
			var caratValue = textbox.value;
			
			var brand = document.getElementById('<%= BrandC.ClientID %>');
			var selectBrand = brand.options[brand.selectedIndex].value;
			
			caratValue = caratValue.replace(/,/g, '.');
			console.log([parseFloat('10.999').toFixed(3),parseFloat(caratValue).toFixed(3)])
            if (selectedValue === "Size :") {				
				if (parseFloat(caratValue).toFixed(3) > parseFloat('10.999')) {	
					btnSave.disabled=true;
					btnLimbat.disabled=true;
					toastr.error(`Carat tidak boleh lebih besar dari 10.999`);
				}else if(parseFloat(caratValue).toFixed(3) < parseFloat('0.900')){
					btnSave.disabled=true;
					btnLimbat.disabled=true;
					toastr.error(`Carat tidak boleh lebih kecil dari 0.900`);
				}else{
					if (selectBrand == 'Brand :') {
						toastr.error(`Brand Tidak Boleh Kosong`);
						btnSave.disabled=true;
						btnLimbat.disabled=true;
					}else{
						btnSave.disabled = false;
						btnLimbat.disabled=false;
					}
				}
            } else {
                var range = txt.split('-');
                var min = parseFloat(range[0].trim()).toFixed(3);				
                var max = parseFloat(range[1].trim()).toFixed(3);                            
				if (parseFloat(caratValue).toFixed(3) > max) {					
					btnSave.disabled=true;
					btnLimbat.disabled=true;
					toastr.error(`Carat tidak boleh lebih besar dari ${max}`);
				}else if(parseFloat(caratValue).toFixed(3) < min){
					btnSave.disabled=true;
					btnLimbat.disabled=true;
					toastr.error(`Carat tidak boleh lebih kecil dari ${min}`);
				}else{					
					if (selectBrand == 'Brand :') {
						toastr.error(`Brand Tidak Boleh Kosong`);
						btnSave.disabled=true;
						btnLimbat.disabled=true;
					}else{
						btnSave.disabled = false;
						btnLimbat.disabled=false;
					}					
				}
            }		
						
		}

		

		  function restrictToNumbers(input) {
        // Remove non-numeric characters using regex
        	input.value = input.value .replace(/[^\d,]/g, '').replace(/\./g, ','); 
    	  }
		</script>

		<div class="clearfix">
			<br class="hidden-xs" />
		</div>
		<div class="text-muted">
			Filter :
		</div>
		<div class="form-group">
			<asp:dropdownlist id="BrandC" runat="server" cssclass="form-control" width="175">
				<asp:listitem>Brand :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel01" runat="server" cssclass="form-control" width="175">
				<asp:listitem>Shape :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel02" runat="server" cssclass="form-control" width="175" >
				<asp:listitem>Size :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel03" runat="server" cssclass="form-control" width="175">
				<asp:listitem>Colour :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel04" runat="server" cssclass="form-control" width="175">
				<asp:listitem>Clarity :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="clearfix"></div>
		<div class="form-separator hidden-xs"></div>
		<div class="form-group">
			<asp:dropdownlist id="parcel05" runat="server" cssclass="form-control" width="175">
				<asp:listitem>Cutting :</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
			<asp:dropdownlist id="parcel06" runat="server" cssclass="form-control" width="175">
				<asp:listitem>Stone Brand :</asp:listitem>
			</asp:dropdownlist>
		</div>		
		<div class="form-group">
			<asp:dropdownlist id="parcel07" runat="server" cssclass="form-control" width="175">
				<asp:listitem>Category :</asp:listitem>
			</asp:dropdownlist>
		</div>		
		
		<div class="clearfix">
			<br class="hidden-xs" />
		</div>	
			<div class="form-group">
			<div class="text-muted">
				Diurutkan :
			</div>
			<asp:dropdownlist id="order" runat="server" cssclass="form-control" width="150">				
				<asp:listitem value="PLU">PLU</asp:listitem>			
				<asp:listitem value="NamaLokasi">Lokasi</asp:listitem>			
				<asp:listitem value="Harga">Price</asp:listitem>			
			</asp:dropdownlist>
			<div class="text-00 visible-xs">&nbsp;</div>
			<asp:dropdownlist id="orderby" runat="server" cssclass="form-control" width="100">
				<asp:listitem>ASC</asp:listitem>
				<asp:listitem>DESC</asp:listitem>
			</asp:dropdownlist>
		</div>
		<div class="form-group">
                <div class="text-muted">
                  Carat
                </div>
				<asp:TextBox ID="kw" CssClass="form-control" Width="250" placeholder="Carat" runat="server" onchange="f_ocCarat();" oninput="restrictToNumbers(this);" />
				
            </div>		
		<div class="form-group">
			<div class="hidden-xs">&nbsp;</div>
			<div>
				<button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage" style="display: none;!important">
					<i class="icon-search"></i>
					Display
				</button>				
				<button id="limbat" type="button" class="btn btn-primary" onclick="f_oclBtn()" disabled>
					<i class="icon-search"></i>
					Display
				</button>				

                &nbsp;                
			</div>
		</div>
	</form>


	<div class="table-responsive1">
		<asp:Table id="tb1" runat="server" cssclass="table-list">
			<asp:TableRow>								
				<asp:TableHeaderCell width="50">No.</asp:tableheadercell>
				<asp:TableHeaderCell width="250">Category</asp:tableheadercell>
				<asp:TableHeaderCell width="250"><small>USD</small> Rapaport Value</asp:tableheadercell>
				<asp:TableHeaderCell width="250"><small>Percentage(%)</small> Discount</asp:tableheadercell>
				<asp:TableHeaderCell width="150" ><small>USD</small> Price/Carat</asp:tableheadercell>																			
				<asp:TableHeaderCell width="200" ><small>USD</small> Value</asp:tableheadercell>																			
			</asp:TableRow>
		</asp:Table>
	</div>
	<div class="table-responsive">
		<asp:Table id="tb" runat="server" cssclass="table-list">
			<asp:TableRow>				
				<asp:TableHeaderCell width="40">No.</asp:tableheadercell>
				<asp:TableHeaderCell width="250">No. PLU</asp:tableheadercell>
				<asp:TableHeaderCell width="250">Category</asp:tableheadercell>
				<asp:TableHeaderCell width="250">GIA</asp:tableheadercell>
				<asp:TableHeaderCell width="150">Status</asp:tableheadercell>
				<asp:TableHeaderCell width="175">Lokasi</asp:tableheadercell>
				<asp:TableHeaderCell width="150">Shape</asp:tableheadercell>
				<asp:TableHeaderCell width="175">Size</asp:tableheadercell>				
				<asp:TableHeaderCell width="75">Colour</asp:tableheadercell>
				<asp:TableHeaderCell width="100">Clarity</asp:tableheadercell>
				<asp:TableHeaderCell width="175">Brand</asp:tableheadercell>
				<asp:TableHeaderCell width="50">Qty</asp:tableheadercell>
				<asp:TableHeaderCell width="100">Carat</asp:tableheadercell>				
				<asp:TableHeaderCell width="130" cssclass="text-right"><small>USD</small> Harga</asp:tableheadercell>
				<asp:TableHeaderCell width="130" cssclass="text-right">Price</asp:tableheadercell>								
			</asp:TableRow>
		</asp:Table>
	</div>
</asp:content>

