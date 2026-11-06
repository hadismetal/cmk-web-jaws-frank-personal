<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="PG_View.aspx.cs" Inherits="Page_Register_PG_Submit" %>

<asp:Content ContentPlaceHolderID="h1" runat="server">
    Plain Gold
</asp:Content>
<asp:Content ContentPlaceHolderID="h1_st" runat="server">
    Catalogue Detail
</asp:Content>

<asp:Content ContentPlaceHolderID="content" runat="server">
    <style>
        .table_lokasi td
        {
            vertical-align: top;
        }
        .lokasi
        {
            padding: 10px;
        }
    </style>
    <p>
        <span class="text-muted">Nomor :</span><br />
        <span class="text-lg" id="header_nomor" runat="server" />
    </p>
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
        picture
    </div>
    <table cellspacing="0" cellpadding="0" border="0" class="table_lokasi">
        <tr>
            <td>
                <asp:Image ID="imgpicture" runat="server" CssClass="img-thumbnail" Width="300" />
            </td>
            <td>
                <div class="lokasi text-sm"><asp:Literal ID="lokasi" runat="server" /></div>
            </td>
        </tr>
    </table>
     <div class="row">
          <asp:Literal ID="literalsp"  runat="server"></asp:Literal>
                        </div>
        
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
        transaction form
    </div>
    <div class="row">
        <div class="col-md-5">
            <table class="table-file">

                <tr>
                    <td class="text-label">Market Brand</td>
                    <td>:</td>
                    <td id="brand" runat="server" />
                </tr>
            </table>
        </div>
        <br class="visible-sm visible-xs" />
        <div class="col-md-5">
            <table class="table-file">
                <tr>
                    <td class="text-label">Tanggal</td>
                    <td>:</td>
                    <td id="tgl" runat="server" />
                </tr>
            </table>
        </div>
    </div>
    <hr />

     
     <div class="file-subheader hidden-sm hidden-xs">
        size estimation
    </div>
    <div class="table-responsive">

        <table class="table-file">
                       <tr>
                <td><span class="text-muted">Size From</span></td>
                <td></td>
                <td><span class="text-muted">Up To</span></td>
            </tr>
            <tr>
                <td><span class="text-lg" id="ringsizefrom" runat="server" /></td>
                <td><span style="font-size:22px;" class="glyphicon glyphicon-resize-horizontal" aria-hidden="true"></span></td>
                <td><span class="text-lg" id="ringsizeto" runat="server" /></td>
            </tr>
           

        </table>
    </div>
    <hr />
    
    <div class="file-subheader hidden-sm hidden-xs">
        gold level estimation
    </div>
    <div class="table-responsive">

        <table class="table-file">
                       <tr>
                <td><span class="text-muted">Kadar From</span></td>
                <td></td>
                <td><span class="text-muted">Up To</span></td>
            </tr>
            <tr>
                <td><span class="text-lg" id="kadarfrom" runat="server" /></td>
                <td><span style="font-size:22px;" class="glyphicon glyphicon-resize-horizontal" aria-hidden="true"></span></td>
                <td><span class="text-lg" id="kadarto" runat="server" /></td>
            </tr>
           

        </table>
    </div>
    <hr />
  

  
      <div class="file-subheader hidden-sm hidden-xs">
        gross weight estimation
    </div>
    <div class="table-responsive">

        <table class="table-file">
                       <tr>
                <td><span class="text-muted">Gross Weight From</span></td>
                <td></td>
                <td><span class="text-muted">Up To</span></td>
            </tr>
            <tr>
                <td><span class="text-lg" id="weightfrom" runat="server" /></td>
                <td><span style="font-size:22px;" class="glyphicon glyphicon-resize-horizontal" aria-hidden="true"></span></td>
                <td><span class="text-lg" id="weightto" runat="server" /></td>
            </tr>
           

        </table>
    </div>
    <hr />
   
    
    
    <div class="file-subheader hidden-sm hidden-xs">
        price estimation
    </div>
    <div class="table-responsive">

        <table class="table-file">
  
            <tr>
                <td><span class="text-muted">Price From</span></td>
                <td></td>
                <td><span class="text-muted">Up To</span></td>
            </tr>
            <tr>
                <td><span class="text-lg" id="pricefrom" runat="server" /></td>
                <td><span style="font-size:22px;" class="glyphicon glyphicon-resize-horizontal" aria-hidden="true"></span></td>
                <td><span class="text-lg" id="priceupto" runat="server" /></td>
            </tr>

        </table>
    </div>
    <hr 



</asp:Content>

