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
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Business Rule
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Business/ProductItem.aspx">Product Item</a></li>
            
			<li><a runat="server" href="~/Page/Business/ProductCategory.aspx">Product Category</a></li>
			<li><a runat="server" href="~/Page/Business/ProductLevel.aspx">Product Level</a></li>
			<li><a runat="server" href="~/Page/Business/ProductSegmen.aspx">Product Segmen</a></li>
			<li><a runat="server" href="~/Page/Business/ProcessCons.aspx">Construction Process</a></li>
			<li><a runat="server" href="~/Page/Business/ProcessFinishing.aspx">Finishing Process</a></li>
			<li><a runat="server" href="~/Page/Business/StoneDist.aspx">Stone Distribution</a></li>
            <li><a runat="server" href="~/Page/Business/GoldModel.aspx">Model (PG)</a></li>
            <li><a id="A4" runat="server" href="~/Page/Business/ProcessRepair.aspx">Repair Process</a></li>
		</ul>
	</li>
	<li id="a2" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Information
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Information/DesignCategory.aspx">Design Category</a></li>
			<li><a runat="server" href="~/Page/Information/DesignConcept.aspx">Design Concept</a></li>
			<li><a runat="server" href="~/Page/Information/DesignProcess.aspx">Design Process</a></li>
			<li><a runat="server" href="~/Page/Information/StoneFluor.aspx">Stone Fluorescence</a></li>
			<li><a runat="server" href="~/Page/Information/StoneSetting.aspx">Stone Setting</a></li>
			<li><a runat="server" href="~/Page/Information/FrameMaterial.aspx">Frame Material</a></li>
			<li><a runat="server" href="~/Page/Information/FrameFinishing.aspx">Frame Finishing</a></li>
            <li><a runat="server" href="~/Page/Information/FrameColor.aspx">Frame Color</a></li>
            <li><a runat="server" href="~/Page/Information/TargetAge.aspx">Target Age</a></li>
            <li><a runat="server" href="~/Page/Information/TargetGender.aspx">Target Gender</a></li>
            <li><a runat="server" href="~/Page/Information/ProductLevelPG.aspx">Product Level (PG)</a></li>
		</ul>
	</li>
	<li id="a3" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Settings
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Settings/ProductCategory.aspx">Product Category</a></li>
			<li><a runat="server" href="~/Page/Settings/ProductLevel.aspx">Product Level</a></li>
			<li><a runat="server" href="~/Page/Settings/Segmen.aspx">Segmen System</a></li>
			<li><a runat="server" href="~/Page/Settings/StoneDist.aspx">Stone Distribution</a></li>
		</ul>
	</li>
	<li id="a4x" runat="server" class="dropdown">
		<a href="#" class="dropdown-toggle" data-toggle="dropdown">Settings
			<b class="caret"></b>
		</a>
		<ul class="dropdown-menu">
			<li><a runat="server" href="~/Page/Master/ListPayments/">Setup Payment</a></li>
			<li><a runat="server" href="~/Page/Master/LocationServicePoints">Setup Service Point</a></li>
			<li><a runat="server" href="~/Page/Master/SettingRepair">Business Rule</a></li>
		</ul>
	</li>
</ul>
