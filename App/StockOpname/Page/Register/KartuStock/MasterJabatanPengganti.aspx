<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="MasterJabatanPengganti.aspx.cs" Inherits="Page_MasterJabatanPengganti" %>
<asp:Content ContentPlaceHolderID="h1" runat="server">
  Register
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Master JR on Duty
</asp:Content>


<asp:Content ContentPlaceHolderID="content" runat="server">

    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">            
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>
            <div class="form-group">
                <div class="text-muted">
                    Location :
                </div>
                <asp:DropDownList ID="location" runat="server" CssClass="form-control" Width="175">                    
                </asp:DropDownList>
            </div>            
            <div class="clearfix">
                <br class="hidden-xs" />
            </div>            
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                    <button id="go" runat="server" class="btn btn-primary" onserverclick="NextStep">
                        <i class="icon-search"></i>
                        Register
                    </button>
                </div>
            </div>            
        </form>                
    </div>
</asp:Content>

