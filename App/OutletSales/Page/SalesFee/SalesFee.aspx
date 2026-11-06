<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SalesFee.aspx.cs" Inherits="Page_Create_SalesFeeAccount" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Sales Fee
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Create New
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <asp:Literal ID="err_msg" runat="server" />
    <form runat="server" class="form-inline">
        <div class="form-group">
            <label>
                Input Pertahun
            </label>
            <asp:CheckBox ID="pertahun" runat="server" OnCheckedChanged="CheckBulan" AutoPostBack="true" />
        </div>
        <div class="form-group">
            <label>
                Tahun :
            </label>
            
            <asp:TextBox ID="tahun" runat="server" CssClass="form-control text-bold" MaxLength="4" Width="100" />
            <asp:RequiredFieldValidator ID="tahun_RF" runat="server" />
        </div>
        <div class="form-group">
            <label>
                Bulan :
            </label>
            
            <asp:TextBox ID="bulan" runat="server" CssClass="form-control text-bold" MaxLength="2" Width="100" AutoPostBack="true" OnTextChanged="AutoCheckBulan" />
            <asp:RequiredFieldValidator ID="bulan_RF" runat="server" />
        </div>
        <div class="clearfix">
            <br class="hidden-xs" />
        </div>
        <div class="form-group">
            <div>
                Total Point :
            </div>
            
            <asp:TextBox ID="totalpoin" runat="server" CssClass="form-control text-bold" />
            <asp:RequiredFieldValidator ID="totalpoin_RF" runat="server" />
        </div>
        <div class="clearfix">
            <br class="hidden-xs" />
        </div>
        <div class="form-group">
            <div>
                Full :
            </div>
            
            <asp:TextBox ID="full" runat="server" CssClass="form-control text-bold" />
            <asp:RequiredFieldValidator ID="full_RF" runat="server" />
        </div>
        <div class="clearfix">
            <br class="hidden-xs" />
        </div>
        <div class="form-group">
            <div>
                AE : 
            </div>
            
            <asp:TextBox ID="ae" runat="server" CssClass="form-control text-bold" />
            <asp:RequiredFieldValidator ID="ae_RF" runat="server" />
        </div>
        <div class="clearfix">
            <br class="hidden-xs" />
        </div>
        <div class="form-group">
            <div>
                DJ : 
            </div>
            
            <asp:TextBox ID="dj" runat="server" CssClass="form-control text-bold" />
            <asp:RequiredFieldValidator ID="dj_RF" runat="server" />
        </div>
        <div class="form-group">
            <div>
                PG : 
            </div>
            
            <asp:TextBox ID="pg" runat="server" CssClass="form-control text-bold" />
            <asp:RequiredFieldValidator ID="pg_RF" runat="server" />
        </div>
        <div class="form-group">
            <div>
                LM : 
            </div>
            
            <asp:TextBox ID="lm" runat="server" CssClass="form-control text-bold" />
            <asp:RequiredFieldValidator ID="lm_RF" runat="server" />
        </div>
        <div class="clearfix">
            <br class="hidden-xs" />
        </div>
        <div class="form-group width-100">
            <div>
                Pacing 1 : 
            </div>
            
            <asp:TextBox ID="pc1" runat="server" CssClass="form-control text-bold" Width="100" MaxLength="3" />
            <asp:RequiredFieldValidator ID="pc1_RF" runat="server" />
        </div>
        <div class="form-group">
            <div>
                Incentive 1 : 
            </div>
            
            <asp:TextBox ID="inc1" runat="server" CssClass="form-control text-bold" />
            <asp:RequiredFieldValidator ID="inc1_RF" runat="server" />
        </div>
        <div class="clearfix">
            <br class="hidden-xs" />
        </div>
        <div class="form-group width-100">
            <div>
                Pacing 2 : 
            </div>
            
            <asp:TextBox ID="pc2" runat="server" CssClass="form-control text-bold" Width="100" MaxLength="3" />
            <asp:RequiredFieldValidator ID="pc2_RF" runat="server" />
        </div>
        <div class="form-group">
            <div>
                Incentive 2 : 
            </div>
            
            <asp:TextBox ID="inc2" runat="server" CssClass="form-control text-bold" />
            <asp:RequiredFieldValidator ID="inc2_RF" runat="server" />
        </div>
        <div class="clearfix">
            <br class="hidden-xs" />
        </div>
        <div class="form-group width-100">
            <div>
                Pacing 3 : 
            </div>
            
            <asp:TextBox ID="pc3" runat="server" CssClass="form-control text-bold" Width="100" MaxLength="3" />
            <asp:RequiredFieldValidator ID="pc3_RF" runat="server" />
        </div>
        <div class="form-group">
            <div>
                Incentive 3 : 
            </div>
            
            <asp:TextBox ID="inc3" runat="server" CssClass="form-control text-bold" />
            <asp:RequiredFieldValidator ID="inc3_RF" runat="server" />
        </div>
        <div class="form-group">
            <div>
                Kompensasi 1 : 
            </div>
            
            <asp:TextBox ID="kom1" runat="server" CssClass="form-control text-bold" />
            <asp:RequiredFieldValidator ID="kom1_RF" runat="server" />
        </div>
        <div class="clearfix">
            <br class="hidden-xs" />
        </div>
        <div class="form-group width-100">
            <div>
                Pacing 4 : 
            </div>
            
            <asp:TextBox ID="pc4" runat="server" CssClass="form-control text-bold" Width="100" MaxLength="3" />
            <asp:RequiredFieldValidator ID="pc4_RF" runat="server" />
        </div>
        <div class="form-group">
            <div>
                Incentive 4 : 
            </div>
            
            <asp:TextBox ID="inc4" runat="server" CssClass="form-control text-bold" />
            <asp:RequiredFieldValidator ID="inc4_RF" runat="server" />
        </div>
        <div class="form-group">
            <div>
                Kompensasi 2 : 
            </div>
            
            <asp:TextBox ID="kom2" runat="server" CssClass="form-control text-bold" />
            <asp:RequiredFieldValidator ID="kom2_RF" runat="server" />
        </div>
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Save
            </button>
        </div>
    </form>
</asp:Content>

