<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="DesignerAccount.aspx.cs" Inherits="Page_Create_DesignerAccount" %>

<%@ Register Src="MenuDesigner.ascx" TagName="Menu" TagPrefix="uc1" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Designer
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Create New
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <uc1:Menu ID="menu" runat="server" />
    <table class="table-file">
        <tr>
            <td class="text-label">Group</td>
            <td>:</td>
            <td id="group" runat="server" class="text-bold" />
        </tr>
        <tr>
            <td class="text-label">Market Brand</td>
            <td>:</td>
            <td id="brand" runat="server" class="text-bold" />
        </tr>
    </table>
    <hr />
    <form runat="server">
        <div class="row">
            <div class="col-xs-12 col-sm-6">
                <div class="form-group width-300">
            
                    <asp:Panel ID="Panel1" runat="server" DefaultButton="btn_search_nik">
                    NIK&nbsp;<span class="glyphicon glyphicon-asterisk" style="color: red;" aria-hidden="true"></span><br />
                    <div class="input-group">
                        <span class="input-group-btn">
                            <asp:LinkButton ID="btn_clear" 
                                        runat="server"  type="button"
                                        CssClass="btn btn-danger" 
                                        OnClick="btn_clear_Click">
                                <span aria-hidden="true" class="glyphicon glyphicon-remove"></span>
                            </asp:LinkButton>
                        </span>
                        <asp:TextBox ID="txt_nik" runat="server" MaxLength="6" CssClass="form-control text-center" Style="text-transform: uppercase;" />
                        <span class="input-group-btn">
                            <asp:LinkButton ID="btn_search_nik" 
                                        runat="server"  type="button"
                                        CssClass="btn btn-primary" 
                                        OnClick="btn_search_nik_Click">
                                <span aria-hidden="true" class="glyphicon glyphicon-arrow-right"></span>&nbsp;Check
                            </asp:LinkButton>
                        </span>
                
                    </div>
                    </asp:Panel>
            
                </div>
                <div class="form-group width-300">
                    <label>
                        Nama :
                    </label>
                    <%--<asp:RequiredFieldValidator ID="nama_RF" runat="server" />--%>
                    <asp:TextBox ID="nama" runat="server" CssClass="form-control text-bold" MaxLength="50" />
                </div>
            </div>
            <div class="col-xs-12 col-sm-6">
                
                <div class="form-group width-500">
                    <label>
                        Keterangan :
                    </label>
                    <asp:TextBox ID="keterangan" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                </div>
            </div>
        </div>
        
        <hr />
        <div class="button-inline">
            <button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
                <i class="icon-arrow-right"></i>
                Next Step
            </button>
            <a href="Designer.aspx" class="btn btn-default">
                <i class="icon-remove"></i>
                Cancel
            </a>
        </div>
    </form>
</asp:Content>

