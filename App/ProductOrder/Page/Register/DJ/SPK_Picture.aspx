<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="SPK_Picture.aspx.cs" inherits="Page_Register_SPK_Picture" %>

<%@ Register Src="MenuSPK.ascx" TagName="Menu" TagPrefix="uc1" %>
<%@ Register Src="HeaderSPK.ascx" TagName="Header" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="h1" runat="server">
    Diamond Jewellery
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="h1_st" runat="server">
    Register
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <uc2:Header ID="header" runat="server" />
    <uc1:Menu ID="menu" runat="server" />

	<form runat="server">
		<table class="table-form">
			<tr>
				<td>Product Picture</td>
				<td>:</td>
				<td>
					<%-- asp:fileupload id="imgpicture" runat="server" cssclass="form-control" /--%>
                    <%--<asp:image id="imgpicture" runat="server" cssclass="img-thumbnail" width="500" />--%>
                    Based on product design / Kode Karet
				</td>
                <%--<td><asp:image id="imgreplace" runat="server" cssclass="img-thumbnail" width="500" /></td>--%>
                <td><label id="lb_status" runat="server" visible="false">Image Replaced</label></td>
			</tr>
            <tr>
                <td colspan="2"></td>
                <td><asp:image id="imgpicture" runat="server" cssclass="img-thumbnail" width="450" /></td>
                <td><asp:image id="imgreplace" runat="server" cssclass="img-thumbnail" width="450" /></td>
            </tr>
		</table>

        <p class="text-muted">
			Max. Size : 
			<span id="kb" runat="server" class="text-bold text-danger" />
		</p> 
        <table class="table-form">
			<tr>
				<td>Product Picture Replace</td>
				<td>:</td>
				<td>
					<asp:fileupload id="imgpicture_up" runat="server" cssclass="form-control" />
				</td>
				<td>
					<span id="imgpicture_ERR" runat="server" class="text-validator" visible="false">Format</span>
				</td>
				<td id="imgpicture_format" runat="server" class="text-muted text-sm" />
                <td>
                   <button id="rev" runat="server" class="btn btn-success" onserverclick="review_click">
				    <i class="icon-arrow-up"></i>
				        Upload
			        </button>
                </td>
			</tr>             
            <%--<tr>
                <td colspan="6">
                    <asp:image id="imgreplace" runat="server" cssclass="img-thumbnail" width="500" />
                </td>
            </tr>--%>
		</table>      

		
		<div class="button-inline">
            	
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next Step
			</button>
			<span id="dir_ERR" runat="server" class="text-validator" visible="false">Folder Error : </span>
		</div>
	</form>
</asp:content>

