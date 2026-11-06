<%@ control language="C#" autoeventwireup="true" codefile="Menu.ascx.cs" inherits="Menu" %>
<ul class="nav navbar-nav">
	<!-- Summary -->
	<li id="a0" runat="server">
		<a runat="server" href="~/Page/Summary.aspx" class="visible-sm visible-md visible-lg" title="Summary" style="margin-left: -15px;">
			<i class="icon-info icon-large"></i>
		</a>
		<a runat="server" href="~/Page/Summary.aspx" class="visible-xs">Summary</a>
	</li>

	<!-- Module Menu -->
	<li id="a1" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Register
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">			
         	<li><a id="RG1" runat="server" href="~/Page/Register/KartuStock/">Daily Stock Card</a></li>
         	<li><a id="RG2" runat="server" href="~/Page/Register/KartuStock/Mstable.aspx">Master Table Display Outlet</a></li>
         	<li><a id="RG3" runat="server" href="~/Page/Register/KartuStock/MasterJabatanPengganti.aspx">Master Table JR on Duty</a></li>
		</ul>
	</li>
	<li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Approval
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">			
            <li><a id="AP1" runat="server" href="~/Page/Approval/KartuStock//">Approval Daily Stock Card</a></li>            
		</ul>
	</li>
    <li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Report
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a id="RE1" runat="server" href="~/Page/Report/KartuStock/DisplayTableOutlet.aspx">Master Table Display Outlet</a></li>						
			<li><a id="RE2" runat="server" href="~/Page/Report/KartuStock/">Report Daily Stock Card</a></li>
			<li><a id="RE3" runat="server" href="~/Page/Report/KartuStock/MasterJabatanPengganti.aspx">Report JR on Duty</a></li>
		</ul>
	</li>      
</ul>
<script>
document.addEventListener("keydown", function(e) {
  if (e.key === "F12" || 
      (e.ctrlKey && e.shiftKey && e.key.toLowerCase() === "i")) {
    e.preventDefault();
  }
});
document.addEventListener("contextmenu", function(e) {
  e.preventDefault();  
});


</script>
