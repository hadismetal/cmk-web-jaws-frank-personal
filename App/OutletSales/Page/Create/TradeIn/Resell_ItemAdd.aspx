<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Resell_ItemAdd.aspx.cs" Inherits="Page_Register_Resell" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Resell
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <style>
        .overlay {
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0,0,0,0.5);
            z-index: 9999;
            display: none;
            align-items: center;
            justify-content: center;
            color: white;
            font-size: 24px;
        }

        .spinner {
            border: 6px solid #f3f3f3;
            border-top: 6px solid #3498db;
            border-radius: 50%;
            width: 50px;
            height: 50px;
            animation: spin 1s linear infinite;
        }

        @keyframes spin {
            0% { transform: rotate(0deg); }
            100% { transform: rotate(360deg); }
        }
    </style>
    <div id="overlay" class="overlay">
        <div class="spinner"></div>
        <div style="margin-top: 10px;">Loading...</div>
    </div>
    <form id="Form1" runat="server" class="form-inline">
        <div class="form-group">
            <div class="text-muted">
                Search (PLU / Invoice / Sales Order) :
            </div>
            <asp:TextBox ID="kw" runat="server" CssClass="form-control" Width="350" placeholder="Input Keyword..." />
        </div>
        <div class="form-group">
            <div class="hidden-xs">&nbsp;</div>
            <div>
                <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage" onclick="showOverlaybtn(this);">
                    <i class="icon-search"></i>
                    Display
                </button>
				<button id="btnoldcustomercode" runat="server" class="btn btn-success" onserverclick="btnoldcustomercode_ServerClick">
                    Insert Old Customer Code
                </button>		 
                <a id="cancel" runat="server" class="btn btn-default" href="TradeIn_Data.aspx">
                    <i class="icon-remove"></i>
                    Cancel
                </a>
            </div>
        </div>

        <div class="form-group">

            <div class="hidden-xs">&nbsp;</div>
            <div>
                <nav aria-label="Page navigation" style="padding-top:4px;">
                    <ul class="pagination">
                        <li>
                            <a href="Resell_ItemAddPLU.aspx" id="btnadd" runat="server">
                                <i class="icon-plus"></i>
                                Add PLU DJ LAMA
                            </a>
                        </li>
                        <li>
                            <a href="Resell_ItemAddPLUPG.aspx" id="btnaddpg" runat="server">
                                <i class="icon-plus"></i>
                                Add PLU PG LAMA
                            </a>
                        </li>
                        <li>
                            <a href="Resell_ItemAddPLULD.aspx" id="btnaddld" runat="server">
                                <i class="icon-plus"></i>
                                Add PLU LD LAMA
                            </a>
                        </li>

                    </ul>
                </nav>

            </div>
        </div>
        <div>
            <div class="row">
                <asp:Literal runat="server" ID="lit_product_list" />
            </div>
        </div>
        <div class="table-responsive">
            <h3>Diamond Jewellery</h3>
            <asp:Table ID="tb" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90" />
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Image</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Sales Order</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Asal</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
            <br />
            <h3>Plain Gold</h3>
            <asp:Table ID="tb2" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90" />
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Image</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Sales Order</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Asal</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
            <br />
            <h3>Gold Jewellery</h3>
            <asp:Table ID="tb3" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90" />
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Image</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Sales Order</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Asal</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
            <br />
            <h3>Loose Diamond</h3>
            <asp:Table ID="tb4" runat="server" CssClass="table-list">
                <asp:TableHeaderRow>
                    <asp:TableHeaderCell Width="90" />
                    <asp:TableHeaderCell Width="40">No.</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Image</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Sales Order</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">PLU</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Product Item</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Harga</asp:TableHeaderCell>
                    <asp:TableHeaderCell Width="150">Asal</asp:TableHeaderCell>
                </asp:TableHeaderRow>
            </asp:Table>
        </div>
    </form>
    <script type="text/javascript">
        function showOverlay() {
            $('button').prop("disabled", true);
            document.getElementById("overlay").style.display = "flex";
        }

        function showOverlaybtn(btn) {
            btn.disabled = true;
            $('button').prop("disabled", true);
            document.getElementById("overlay").style.display = "flex";
        }
    </script>
</asp:Content>

