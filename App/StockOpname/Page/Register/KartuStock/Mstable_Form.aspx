<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Mstable_Form.aspx.cs" Inherits="Page_Mstable_Form" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Master Display Outlet
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
<div class="container" style="width: 100%;height: auto;">
    <form id="Form" runat="server" class="form-horizontal">
        <div class="row">
            <div class="col-xs-12">
                <div id="errorMsg" class="alert alert-danger" style="display:none; margin-top:10px;"></div>
                <div id="successMsg" class="alert alert-success" style="display:none; margin-top:10px;"></div>
            </div>
        </div>
        <input type="hidden" class="form-control" id="IDLokasi" runat="server" />
        <div class="container">
            <h4>Display Name</h4>
            <div id="formRows" class="row">
                <!-- Default row -->
                <div class="col-xs-12 form-group row-item">                                                        
                    <div class="col-xs-4">
                        <div class="input-group">                            
                             <span class="input-group-text numberAwal">1</span>
                            <input type="text" class="form-control name-input" name="displayName[]" placeholder="Display Name" />                        
                        </div>
                    </div>    
                    <div class="col-xs-2">                                            
                        <div class="input-group">                                                                                     
                            <input type="number" style="width: 10px;" class="form-control quantity-input" name="quantity[]" placeholder="Quantity" min="1" step="1" onchange="oncheckmaxmin(event)" onkeypress="return onlyNumbersAndCommas(event)" max="50" value="1" />
                        </div>
                    </div>                                    
                </div>
            </div>

            <!-- Add Row Button -->

            <div id="formRows1" class="row">
                <!-- Default row -->
                <div class="col-xs-12 form-group row-item">
                    <div class="col-xs-6">
                        <button type="button" id="btnAddRow" class="btn btn-success" >
                            Add Row
                        </button>
                        <button type="button" id="btnSave" class="btn btn-primary" >
                            Save
                        </button>                        
                    </div>                    
                </div>
            </div>
        </div>        
    </form>

</div>

<script type="text/javascript">
    $(document).ready(()=>{        
        toastr.options = {
            closeButton: true,
            progressBar: true,
            positionClass: "toast-bottom-center",
            showDuration: "300",
            hideDuration: "1000",
            timeOut: "5000",
        };

         function renumberRows() {
            $('#formRows .row-item').each(function (index) {
                $(this).find('.numberAwal').text(index + 1);
            });
        }
    


        document.getElementById("btnAddRow").addEventListener("click", function () {
            var container = document.getElementById("formRows");

            // Create a new row
            var newRow = document.createElement("div");            
            newRow.className = "col-xs-12 form-group row-item";
            newRow.innerHTML = `
            
                <div class="col-xs-4">
                    <div class="input-group">
                        <span class="input-group-text numberAwal"></span>
                        <input type="text" class="form-control name-input" name="displayName[]" placeholder="Display Name" />
                    </div>
                </div>
                <div class="col-xs-2">                                            
                    <div class="input-group">                                                                                     
                            <input type="number" style="width: 10px;" class="form-control quantity-input" name="quantity[]" placeholder="Quantity" min="1" step="1" onchange="oncheckmaxmin(event)" onkeypress="return onlyNumbersAndCommas(event)" max="50" value="1" />
                        </div>
                    </div>                                    
                <div class="col-xs-2">
                    <button type="button" class="btn btn-danger btn-delete">Delete</button>
                </div>
            `;

            container.appendChild(newRow);
            renumberRows();
        });

        // Delegate delete button click
        document.addEventListener("click", function (e) {
            if (e.target && e.target.classList.contains("btn-delete")) {
                e.target.closest(".row-item").remove();
                // Re-numbering
                renumberRows();
            }
        });
        $("#btnSave").click(function () {
            var emails = [];
            var quantity = [];
            var hasEmpty = true;
            // var IDLokasi = 14;
            var IDLokasi = document.getElementById("<%= IDLokasi.ClientID %>").value;

            $("#errorMsg").hide();
            $("#successMsg").hide();

            // $(".name-input").each(function () {
            //     var val = $(this).val().trim();
            //     if (val.trim() == "") {
            //         hasEmpty = true;
            //         $(this).focus();                    
            //         return false; // stop loop
            //     }
            //     emails.push(val);
            // });  
            let data =[];
            
            $(".name-input").each(function (index) {

                let email = $(this).val().trim();
                let qty = $(".quantity-input").eq(index).val().trim();

                if (!email || !qty) {
                    hasEmpty = true;
                    $(this).focus();
                    return false; // stop loop
                }

                // gabungkan jadi satu string
                data.push([email, qty]);

            });            
     
            if (hasEmpty == false) {
                $("#errorMsg").text("Please fill out all fields before saving.").show();
                toastr.error(`Please fill out all fields before saving or remove the empty fields.`);
                return;
            }           
            console.log(JSON.stringify({ data: data, IDLokasi: IDLokasi}))
            // Send to WebMethod
            $.ajax({
                type: "POST",
                url: "Mstable_Form.aspx/Save",
                data: JSON.stringify({ data: data, IDLokasi: IDLokasi}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {                    
                    toastr.success(`${response.d}`);
                    setTimeout(() => {
                        window.location.href = "Mstable.aspx";
                    }, 500);
                },
                error: function (xhr, status, error) {
                    $("#errorMsg").text("Error saving : " + error).show();
                    toastr.error(`Error saving : ${error}`);
                    
                }
            });
        
        });    
    });

    function onlyNumbersAndCommas(event) {
        // Get the ASCII code of the pressed key
        var charCode = event.which ? event.which : event.keyCode;

        // Allow only numbers (0–9)
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
            return false;
        }

        // Remove leading zeros
        let input = event.target.value.replace(/^0+/, '');
        event.target.value = input;

        return true;
        
    }
    function oncheckmaxmin(event) {
        var input = event.target;
        var value = parseInt(input.value);
        if (isNaN(value) || value < 1) {
            input.value = 1;
        } else if (value > 50) {
            input.value = 50;
        }
    }

</script>

</asp:Content>

