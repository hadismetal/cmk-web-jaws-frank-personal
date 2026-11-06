<%@ page language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" codefile="Summary.aspx.cs" inherits="Page_Summary" %>

<%@ register src="Menu.ascx" tagname="Menu" tagprefix="uc1" %>

<asp:content contentplaceholderid="h1" runat="server">
	Summary
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <uc1:menu id="menu" runat="server" />
</asp:content>

