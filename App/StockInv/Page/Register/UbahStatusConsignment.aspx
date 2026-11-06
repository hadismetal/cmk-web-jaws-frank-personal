<%@ Page Language="C#" MasterPageFile="~/MasterPage.master"
AutoEventWireup="true" CodeFile="UbahStatusConsignment.aspx.cs"
Inherits="Page_Register_UbahStatusConsignment" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Ubah Status Consignment
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
  <form runat="server" id="form">
    <table class="table-form">      
      <tr>
        <div class="clearfix"></div>
        <hr />
      </tr>
      <tr>
        <td>
          <div class="input-group">
            <asp:FileUpload
              ID="FileUpload1"
              runat="server"
              class="form-control"
              onchange="checkExcelData()"              
            />
          </div>
          <div id="message"></div>
        </td>
        <td>
          <asp:Button
            ID="btnUpload"
            runat="server"
            Text="Upload"
            class="btn btn-primary"
            OnClick="uploadData"
            disabled
          />
          <asp:Button
            runat="server"
            Text="Download Template"
            class="btn btn-primary"
            OnClick="btnTemplate_Click"
          />
        </td>
      </tr>
      <tr>
        <td>
          <asp:Label
            runat="server"
            ID="error"
            Visible="false"
            ForeColor="Red"
          ></asp:Label>
        </td>
      </tr>
      <tr>
        <td>
          <asp:Label
            runat="server"
            ID="success"            
            Visible="false"
            ForeColor="Green"
          ></asp:Label>
        </td>
      </tr>
      <tr>
        <td>
            <asp:Label
            runat="server"
            ID="berhasil"            
            style="display: none;"
            ForeColor="Green"
          ></asp:Label>
        </td>
      </tr>
    </table>
  </form>
  
<script type="text/javascript">
  toastr.options = {
    			closeButton: true,
    			progressBar: true,
    			positionClass: "toast-bottom-center",
    			showDuration: "300",
    			hideDuration: "1000",
    			timeOut: "3000",
    		};
function checkExcelData() {
    var fileUpload = document.getElementById("<%= FileUpload1.ClientID %>").files[0];    
    var btnUpload = document.getElementById("<%= btnUpload.ClientID %>");
    var label = document.getElementById("<%= berhasil.ClientID %>");
    // if (!fileUpload) {
    //     document.getElementById("message").innerText = "Please upload an Excel file.";
    //     return;
    // }

    var reader = new FileReader();

    var validMimeTypes = ['text/csv', 'application/csv'];
    var mimeType = fileUpload.type;    
   if (validMimeTypes.includes(mimeType)) {
    if ((fileUpload.name).substring(0,29) == 'TemplateUbahStatusConsignment') {      
        reader.onload = function (e) {
        var csvData = e.target.result;

        // Ensure csvData is a string
        if (typeof csvData === "string") {
            // Split by new line to get rows
            var rows = csvData.split("\n");
            
            var seen = new Set(); // To track duplicates
            var duplicates = [];            
            var fileData = [];

            // Loop through the rows, starting from the second row (assuming first row is a header)
            for (var i = 1; i < rows.length; i++) {
                // Split by comma to get columns (for CSV)
                var row = rows[i].split(",");

                if (row.length === 0 || row[0].trim() === '') {
                    // Skip empty rows
                    continue;
                }

                var cellValue = row[0]; // Assuming the first column holds the value to check
                fileData.push(cellValue); // Add value to fileData array for server check
                if (seen.has(cellValue)) {
                    duplicates.push(cellValue);
                } else {
                    seen.add(cellValue);
                }
            }            
            // Check if the file has only a header row
            if (rows.length === 1 || (rows.length === 2 && rows[1].trim() === '')) {                
                toastr.error(`Tidak ada data didalam file tersebut.`);                
                btnUpload.disabled=true;                
            }else{
              if (duplicates.length > 0) {
              toastr.error(`Terdapat data duplikat: ${duplicates.join(", ")}.`);                              
              }else{                
                checkDataOnServer(fileData, btnUpload,label);                
              }
            }                            
        } else {
          toastr.error(`File tidak dapat diproses.`);
        }
      };
      }else{
        toastr.error(`Template tidak sesuai.`);
      } 
  }else{
    toastr.error(`File yang di upload bukan dengan format CSV. Harap upload file dengan format CSV.`);
   }
    // Read file as text (ensure it is plain text)
    reader.readAsText(fileUpload);
}

function checkDataOnServer(fileData, btnUpload,label) {    
     $.ajax({
        type: "POST",
        url: "UbahStatusConsignment.aspx/CheckDataExistence", // URL to your WebMethod
        data: JSON.stringify({ items: fileData }), // Send extracted data
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        success: function (response) {
        // Parse the response
        let parsed = JSON.parse(response.d);
        let notExists=[];
        let notExists1=[];
        let notExists2=[];
        let SupplierSJ=[];
        
        $.each(parsed.plu,(e,i)=>{          
          if (i.Count == 0) {
            notExists.push(i.Item);            
          }          
        })

        $.each(parsed.IE,(e,i)=>{              
          if (i.Count > 0) {
            notExists1.push(i.Item);            
          }
        })        
        $.each(parsed.SJ,(e,i)=>{          
            SupplierSJ.push(i.Supplier);          
        })              
          if (notExists.length > 0) {
              label.innerHtml = ``; 
              label.style.display = 'none';
              toastr.error(`Beberapa data tidak ditemukan : ${notExists.join(", ")}.`);
          }else if (notExists1.length > 0) {
              label.innerHtml = ``; 
              label.style.display = 'none';
              toastr.error(`data sudah pernah di upload: ${notExists1.join(", ")}.`);
          } else {                        
              label.innerText = `Surat Jalan : ${SupplierSJ.join(", ")}.`; 
              label.style.display = 'block';
              btnUpload.disabled=false;
          }        
    }    
    });
}




</script>
</asp:Content>

