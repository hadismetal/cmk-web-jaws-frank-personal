<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="4.aspx.cs" Inherits="Page_Sync_4" %>


<asp:Content ContentPlaceHolderID="h1" runat="server">
    Semi Precious
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Synchronize
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">

    
    

    <div id="zero" runat="server" visible="false">
        <i class="icon-ok icon-5x pull-left text-success"></i>
        <h4>Proses Selesai</h4>
        <p class="text-muted">
            Tidak ada transaksi penjualan
        </p>
        <div class="clearfix"></div>
        <hr />
    </div>
    <div id="form_area" runat="server">
        <form id="Form1" runat="server" class="form-inline">         
            <div class="form-group">
                <div class="hidden-xs">&nbsp;</div>
                <div>
                      <div class="form-group">
                            <div>
                             <button id="go" runat="server" class="btn btn-primary" onserverclick="refreshPage">
                                <i class="icon-search"></i>
                                Display
                            </button>
                             </div>
                       </div>

                  

                    <div class="form-group" >
                           <asp:LinkButton ID="lbtnSyncAll" runat="server" 
                               class="btn btn-warning"
                            onclick="lbtnSyncAll_Click" 
                               OnClientClick="if(!confirm('Sync All Stone?')) return false;"
                           
                            ToolTip="Link to another page">
                                  <span class="glyphicon glyphicon-saved" aria-hidden="true"></span>
                               Sync All Stone</asp:LinkButton>
                        </div>

                </div>
            </div>
        </form>
   

        <div>
            
            <asp:Literal runat="server" ID="lit_display"  />
            <asp:Literal runat="server" ID="lit_syncresult"  />
            <asp:Literal runat="server" ID="lit_errmsg" />
        </div>
        <div>
          
            <asp:Literal runat="server" ID="lit_result" />
        </div>
        
    </div>


   

   
</asp:Content>

