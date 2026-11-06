<%@ Page EnableEventValidation="false" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PG_Product.aspx.cs" Inherits="PG_Product" %>

<%@ Register Src="~/Page/Register/MenuPG.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Plain Gold
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">

    <uc1:Menu ID="menu" runat="server" />
    <table class="table-file">
        <tr>
            <td class="text-label">Market Brand (From)</td>
            <td>:</td>
            <td id="brandFrom" runat="server" class="text-bold" />
        </tr>
        <tr>
            <td class="text-label">From
                <asp:Literal runat="server" ID="lit_type_from" /></td>
            <td>:</td>
            <td id="from" runat="server" class="text-bold" />
        </tr>
        <tr>
            <td></td>
        </tr>
        <tr>
            <td class="text-label">Market Brand (To)</td>
            <td>:</td>
            <td id="brandTo" runat="server" class="text-bold" />
        </tr>
        <tr>
            <td class="text-label">To
                <asp:Literal runat="server" ID="lit_type_to" /></td>
            <td>:</td>
            <td id="to" runat="server" class="text-bold" />
        </tr>
    </table>
    <hr />
    <form runat="server">
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:UpdatePanel ID="upl" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
        <div class="form-inline">
            <asp:Panel DefaultButton="btn_inputproduct">
                <div>
                    <div class="form-group width-300">
                        <div class="text-muted">
                            Product Code :
                        </div>
                        <asp:TextBox ID="inputproduct" runat="server" CssClass="form-control text-bold" MaxLength="100" />
                        <asp:TextBox ID="IDAsal" runat="server" CssClass="text-bold hidden" />
                        <asp:TextBox ID="TipeAsal" runat="server" CssClass="text-bold hidden" />
                    </div>
                    <div class="form-group">
                        <div class="hidden-xs">&nbsp;</div>
                        <div>
                            <button type="button" class="btn btn-primary" id="btn_search_plu" onclick="validasiPLU()">Enter</button>
                            <asp:Button ID="btn_inputproduct" OnClick="btn_inputproduct_Click1" runat="server" Text="Enter" class="btn btn-primary hidden">
                                <%--<i class="icon-arrow-right"></i>--%>
                            </asp:Button>
                        </div>
                    </div>
                </div>
                <div>
                    <br />
                    <asp:Literal runat="server" ID="lit_Message" />
                    <asp:Label runat="server" ID="lit_Message1" />
                </div>
            </asp:Panel>
        </div>
        <br />
        <div class="table-responsive">
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="30" />
                    <asp:TableHeaderCell Width="110">Image</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="185">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Status</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Net Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="120">Gross Weight</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Gold Model</asp:TableHeaderCell>                    
                </asp:TableHeaderRow>
            </asp:Table>

            <div class="form-inline">
                <asp:CheckBoxList runat="server" ID="cbl_Product" />
            </div>
        </div>
        <%--</ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btn_inputproduct" EventName="Click" />
        </Triggers>
        </asp:UpdatePanel>--%>
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
        </div>
    </form>
    <script>
        document.addEventListener("DOMContentLoaded", function () {
            const saveBtn = document.getElementById("<%= save.ClientID %>");

            function updateButtonState() {
                const checkedCount = document.querySelectorAll(
                    '#<%= cbl_Product.ClientID %> input[type="checkbox"]:checked'
                ).length;

                // Disable if no checkbox is checked
                saveBtn.disabled = (checkedCount === 0);
            }

            // Attach to each checkbox in the list
            document.querySelectorAll('#<%= cbl_Product.ClientID %> input[type="checkbox"]')
                .forEach(cb => {
                    cb.addEventListener("change", updateButtonState);
                });

            // Run on page load to set initial state
            updateButtonState();
        });
        function validasiPLU(){
            var inputProduct = document.getElementById("<%= inputproduct.ClientID %>").value;            
            var IDLokasi = document.getElementById("<%= IDAsal.ClientID %>").value;            
            var TipeLokasi = document.getElementById("<%= TipeAsal.ClientID %>").value;            
            var buttonSearch = document.getElementById("<%= btn_inputproduct.ClientID %>");                        
            var Message = document.getElementById("<%= lit_Message1.ClientID %>");

            Message.innerHTML = '';

            $.ajax({
                type: "POST",
                url: "./PG_Product.aspx/CheckDataExistence",
                data: JSON.stringify({PLU : inputProduct, IDAsal:IDLokasi, TipeLokasi : TipeLokasi}),
                contentType: "application/json; charset=utf-8",
                success: function (data) {                    
                    const response = data["d"];
                    let html=``;
                    
                    if (response.Status == 200) {                                                                        
                        // __doPostBack('<%= btn_inputproduct.UniqueID %>', '');                        
                        document.getElementById("<%= btn_inputproduct.ClientID %>").click();
                    }else{                                                
                        html += `<div class='alert alert-danger' role='alert'>${response.Message}</div>`;                                                
                        Message.innerHTML = html;                        
                        // Message.innerHTML = html;


                        // Message.Text(html);
                    }
                },
                error: function (error) {
                    console.error(error);
                },
                complete: function (data) {
                },
            });
        }
    </script>
</asp:Content>

