<%@ page EnableEventValidation="false" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="ProductBroken_Form.aspx.cs" inherits="DJ_Form" %>

<%@ register src="MenuBroken.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Souvenir Broken
</asp:content>
<asp:content contentplaceholderid="h1_st" runat="server">
	Register
</asp:content>

<asp:content contentplaceholderid="content" runat="server">

	<uc1:menu id="menu" runat="server" />
    <hr />
    <div class="file-subheader hidden-sm hidden-xs">
		form summary
	</div>
	<div class="row">
        <div class="col-md-5">
			<table class="table-file">
		        <%--tr>
			        <td class="text-label">Market Brand</td>
			        <td>:</td>
			        <td id="brand" runat="server" class="text-bold" />
		        </--%>
		        <tr>
			        <td class="text-label">From <asp:Literal runat="server" ID="lit_type_from" /></td>
			        <td>:</td>
			        <td id="from" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">To <asp:Literal runat="server" ID="lit_type_to" /></td>
			        <td>:</td>
			        <td id="to" runat="server" class="text-bold" />
		        </tr>
	        </table>
		</div>
		<br class="visible-sm visible-xs" />
		<div class="col-md-5">
			<table class="table-file">
		        <tr>
			        <td class="text-label">Tanggal</td>
			        <td>:</td>
			        <td id="tgl" runat="server" class="text-bold" />
		        </tr>
		        <tr>
			        <td class="text-label">Kurir</td>
			        <td>:</td>
			        <td id="kurir" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">Keterangan</td>
			        <td>:</td>
			        <td id="keterangan" runat="server" class="text-bold" />
		        </tr>
                <tr>
			        <td class="text-label">Tanggal ETA</td>
			        <td>:</td>
			        <td id="eta" runat="server" class="text-bold" />
		        </tr>
	        </table>
		</div>
    </div>
	<hr />
    <div class="file-subheader hidden-sm hidden-xs">
		product list
	</div>
	<div class="row">
        <br class="visible-sm visible-xs" />
		<div class="col-md-5">
	    <form runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
            <div class="form-inline">
                <asp:CheckBoxList runat="server" ID="cbl_Product" RepeatDirection="Horizontal" RepeatColumns="10"  />
                <style type="text/css">
                    label {
                        display: block;
                        border:solid 1px black;
                        border-collapse:collapse;
                    }
                </style>
		    </div>
	    </form>
        </div>
    </div>
    <hr />
    <div class="row">
        <br class="visible-sm visible-xs" />
		<div class="col-md-5">
			<button id="save" runat="server" class="btn btn-primary" onserverclick="save_Click">
				<i class="icon-arrow-right"></i>
				Next
			</button>
		</div>
    </div>
</asp:content>

