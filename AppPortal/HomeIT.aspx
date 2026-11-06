<%@ page language="C#" masterpagefile="~/MasterIT.master" autoeventwireup="true" codefile="HomeIT.aspx.cs" inherits="HomeIT" %>

<asp:content contentplaceholderid="h1" runat="server">
	JAWS Warning System
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <div class="row">
        <asp:Literal runat="server" ID="lit_issue" />
    </div>
    
	<div id="footer">
		
	</div>
</asp:content>
