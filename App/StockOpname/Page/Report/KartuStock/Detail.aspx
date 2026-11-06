<%@ page language="C#" masterpagefile="./MasterPage.master" autoeventwireup="true" codefile="Detail.aspx.cs" inherits="Page_detail" %>

<asp:content contentplaceholderid="h1" runat="server">
	Detail Stock Opname 
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <style>
        .merahPrimary { background-color: #fee2e2; } .merahNyala { background-color: #f44336; color: white; }
        .ijoPrimary { background-color: #dff0d8; } .ijoNyala{ background-color: #16A34A;color: white; }
        .biruPrimary { background-color: #d9edf7; } .biruTua { background-color: #FFEDD5; }
        .kuningPrimary { background-color: #c5cae9; }
        .unguPrimary { background-color:#c5cae9; } .amberPrimary { background-color:#fffbeb;}
        .amberNyala { background-color:#B45309; color:white; } .emeraldPrimary { background-color: #DCFCE7;}
        .sapphierePrimary { background-color: #dbeafe;} .rubyPrimary { background-color: #dc2626;}
        .ametystPrimary { background-color: #F3E8FF;} 
        .white-text {color : white;} .dark-text {color : #333333;} .topaz-text {color : #B45309;} 
        .sapphiere-text{color:#015AAE;}


        #loader {
            width: 50px;
            height: 50px;
            border: 6px solid #f3f3f3;
            border-top: 6px solid #3498db;
            border-radius: 50%;
            animation: spin 1s linear infinite;
            display: none; /* hidden initially */
            margin: 50px auto;
        }

        @keyframes spin {
        100% { transform: rotate(360deg); }
        }

    </style>    

    <div id="loader"></div>
    <div id="stock-opname">
        <div class="container-fluid" style="margin:20px;">        
            <h3 id="lokasinya" class="text-right"></h3>
            <div class="panel panel-success mx-3">            
                <div class="panel-heading">
                    <h3 class="panel-title dark-text" style="font-size:20px !important;font-weight:500;">Summary Opname</h3>
                </div>
                <div class="panel-body">
                    <ul class="nav nav-tabs nav-justified" role="tablist">
                        <li role="presentation" class="active">
                            <a href="#sumDJ" id="headerDJ" onclick="autoSwitch('DJ','header')" aria-controls="Diamond Jewellery" role="tab" data-toggle="tab">Diamond Jewellery <span class="badge" id="badgeSumDJ">0/12 matches</span></a>
                        </li>
                        <li role="presentation" ><a href="#sumPG" id="headerPG" onclick="autoSwitch('PG','header')" aria-controls="Plain Gold" role="tab" data-toggle="tab">Plain Gold <span class="badge" id="badgeSumPG">0/12 matches</span></a></li>
                        <li role="presentation" ><a href="#sumLD" id="headerLD" onclick="autoSwitch('LD','header')" aria-controls="Loose Diamond" role="tab" data-toggle="tab">Loose Diamond <span class="badge" id="badgeSumLD">0/1 matches</span></a></li>                 
                    </ul>

                    <div class="tab-content" style="padding-top: 20px;">
                        <div role="tabpanel" class="tab-pane fade in active" id="sumDJ">
                            <div class="panel-body" id="bodySumDJ" style="background-color:white; border-top:1px solid #ddd; padding:15px;">
                                <div class="d-flex flex-column gap-1">
                                    <div class="col-12">                                        
                                        <table class="table-responsive table ">
                                        <thead>
                                            <tr>
                                                <th style="width:500px;">Operator</th>
                                                <th style="text-align:center;width:150px;">BGL</th>
                                                <th style="text-align:center;width:150px;">BRC</th>
                                                <th style="text-align:center;width:150px;">EAR</th>
                                                <th style="text-align:center;width:150px;">LR</th>
                                                <th style="text-align:center;width:150px;">BUCK</th>
                                                <th style="text-align:center;width:150px;">MJ</th>
                                                <th style="text-align:center;width:150px;">NECK</th>
                                                <th style="text-align:center;width:150px;">BRO</th>
                                                <th style="text-align:center;width:150px;">PDT</th>
                                                <th style="text-align:center;width:150px;">P + C</th>
                                                <th style="text-align:center;width:150px;">WR</th>
                                                <th style="text-align:center;width:150px;">OTH</th>
                                                <th style="text-align:center;width:150px;">Total</th>
                                            </tr>
                                        </thead>
                                        <tbody id="tbody-sumDJ"></tbody>
                                        <tfoot id="tfoot-sumDJ"></tfoot>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div role="tabpanel" class="tab-pane fade" id="sumPG">
                            <div class="panel-body" id="bodySumPG" style="background-color:white; border-top:1px solid #ddd; padding:15px;">
                                <div class="d-flex flex-column gap-1">
                                    <div class="col-12">
                                        <table class="table-responsive table ">
                                            <thead>
                                                <tr>
                                                    <th style="width:500px;">Operator</th>
                                                    <th style="text-align:center;width:150px;">BGL</th>
                                                    <th style="text-align:center;width:150px;">BRC</th>
                                                    <th style="text-align:center;width:150px;">EAR</th>
                                                    <th style="text-align:center;width:150px;">LR</th>
                                                    <th style="text-align:center;width:150px;">ANK</th>
                                                    <th style="text-align:center;width:150px;">MJ</th>
                                                    <th style="text-align:center;width:150px;">NECK</th>
                                                    <th style="text-align:center;width:150px;">CH</th>
                                                    <th style="text-align:center;width:150px;">PDT</th>
                                                    <th style="text-align:center;width:150px;">P + C</th>
                                                    <th style="text-align:center;width:150px;">WR</th>
                                                    <th style="text-align:center;width:150px;">OTH</th>
                                                    <th style="text-align:center;width:150px;">Total</th>
                                                </tr>
                                            </thead>
                                            <tbody id="tbody-sumPG"></tbody>
                                            <tfoot id="tfoot-sumPG"></tfoot>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div role="tabpanel" class="tab-pane fade" id="sumLD">
                            <div class="panel-body" id="bodySumLD" style="background-color:white; border-top:1px solid #ddd; padding:15px;">
                                <div class="d-flex flex-column gap-1">
                                    <div class="col-12">
                                        <table class="table-responsive table ">
                                        <thead>
                                            <tr>
                                                <th style="width:500px;">Operator</th>
                                                <th style="text-align:center;width:150px;">OTH</th>
                                                <th style="text-align:center;width:150px;">Total</th>
                                            </tr>
                                        </thead>
                                        <tbody id="tbody-sumLD"></tbody>
                                        <tfoot id="tfoot-sumLD"></tfoot>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>                    
                    </div>
                </div>
            </div>        
        </div>
        <div class="container-fluid" style="margin:20px;">            
            <ul class="nav nav-tabs nav-justified" role="tablist">
                <li role="presentation" class="active"><a href="#Display" id="detailDisplay"  aria-controls="Diamond Jewellery" role="tab" data-toggle="tab">Barang Display</a></li>
                <li role="presentation" ><a href="#Dalam" id="detailDalam"  aria-controls="Plain Gold" role="tab" data-toggle="tab">Barang Dalam</a></li>
                <li role="presentation" ><a href="#Titipan" id="detailTitipan"  aria-controls="Plain Gold" role="tab" data-toggle="tab">Barang Titipan</a></li>
                <li role="presentation" ><a href="#Luar" id="detailLuar"  aria-controls="Plain Gold" role="tab" data-toggle="tab">Barang Luar Sistem</a></li>
            </ul>

            <div class="tab-content" style="padding-top: 20px;">
                <div role="tabpanel" class="tab-pane fade in active" id="Display">                                  
                    <div class="panel panel-success mx-3">
                        <div class="panel-heading">
                            <h3 class="panel-title dark-text" style="font-size:20px !important;font-weight:500;">Barang Display</h3>
                        </div>
                        <div class="panel-body">
                            <ul class="nav nav-tabs nav-justified" role="tablist">
                                <li role="presentation" class="active"><a href="#DiamondJewellery" id="detailDJ" onclick="autoSwitch('DJ','detail')" aria-controls="Diamond Jewellery" role="tab" data-toggle="tab">Diamond Jewellery</a></li>
                                <li role="presentation" ><a href="#PlainGold" id="detailPG" onclick="autoSwitch('PG','detail')" aria-controls="Plain Gold" role="tab" data-toggle="tab">Plain Gold</a></li>
                                <li role="presentation" ><a href="#LooseDiamond" id="detailLD" onclick="autoSwitch('LD','detail')" aria-controls="Loose Diamond" role="tab" data-toggle="tab">Loose Diamond</a></li>
                                <!-- <li role="presentation"><a >Profile</a></li>
                                <li role="presentation"><a >Messages</a></li> -->
                            </ul>

                            <div class="tab-content" style="padding-top: 20px;">
                                <div role="tabpanel" class="tab-pane fade in active" id="DiamondJewellery">                                  
                                    <div class="panel-group" id="DJ" role="tablist" aria-multiselectable="true">

                                    </div>
                                </div>

                                <div role="tabpanel" class="tab-pane fade" id="PlainGold">                       
                                </div>

                                <div role="tabpanel" class="tab-pane fade" id="LooseDiamond">            
                                </div>
                            </div>         
                        </div> 
                    </div> 
                </div>

                <div role="tabpanel" class="tab-pane fade" id="Dalam">
                    <div class="panel panel-success mx-3">
                        <div class="panel-heading">
                            <h3 class="panel-title dark-text" style="font-size:20px !important;font-weight:500;">Barang Dalam</h3>
                        </div>
                        <div class="panel-body">
                            <ul class="nav nav-tabs nav-justified" role="tablist">
                                <li role="presentation" class="active"><a href="#DiamondJewelleryD" id="detailDJD" onclick="autoSwitch('DJ','detail')" aria-controls="Diamond Jewellery" role="tab" data-toggle="tab">Diamond Jewellery</a></li>
                                <li role="presentation" ><a href="#PlainGoldD" id="detailPGD" onclick="autoSwitch('PG','detail')" aria-controls="Plain Gold" role="tab" data-toggle="tab">Plain Gold</a></li>
                                <li role="presentation" ><a href="#LooseDiamondD" id="detailLDD" onclick="autoSwitch('LD','detail')" aria-controls="Loose Diamond" role="tab" data-toggle="tab">Loose Diamond</a></li>
                                <!-- <li role="presentation"><a >Profile</a></li>
                                <li role="presentation"><a >Messages</a></li> -->
                            </ul>

                            <div class="tab-content" style="padding-top: 20px;">
                                <div role="tabpanel" class="tab-pane fade in active" id="DiamondJewelleryD">                                        
                                </div>

                                <div role="tabpanel" class="tab-pane fade" id="PlainGoldD">                       
                                </div>

                                <div role="tabpanel" class="tab-pane fade" id="LooseDiamondD">            
                                </div>
                            </div>         
                        </div> 
                    </div> 
                </div>    
                
                <div role="tabpanel" class="tab-pane fade" id="Titipan">
                    <div class="panel panel-success mx-3">
                        <div class="panel-heading">
                            <h3 class="panel-title dark-text" style="font-size:20px !important;font-weight:500;">Barang Titipan</h3>
                        </div>
                        <div class="panel-body">
                            <ul class="nav nav-tabs nav-justified" role="tablist">
                                <li role="presentation" class="active"><a href="#DiamondJewelleryT" id="detailDJT" aria-controls="Diamond Jewellery" role="tab" data-toggle="tab">Diamond Jewellery</a></li>
                                <li role="presentation" ><a href="#PlainGoldT" id="detailPGT"  aria-controls="Plain Gold" role="tab" data-toggle="tab">Plain Gold</a></li>
                                <li role="presentation" ><a href="#LooseDiamondT" id="detailLDT"  aria-controls="Loose Diamond" role="tab" data-toggle="tab">Loose Diamond</a></li>                                
                            </ul>

                            <div class="tab-content" style="padding-top: 20px;">
                                <div role="tabpanel" class="tab-pane fade in active" id="DiamondJewelleryT">                                        
                                </div>

                                <div role="tabpanel" class="tab-pane fade" id="PlainGoldT">                       
                                </div>

                                <div role="tabpanel" class="tab-pane fade" id="LooseDiamondT">            
                                </div>
                            </div>         
                        </div> 
                    </div> 
                </div>    

                <div role="tabpanel" class="tab-pane fade" id="Luar">
                    <div class="panel panel-success mx-3">
                        <div class="panel-heading">
                            <h3 class="panel-title dark-text" style="font-size:20px !important;font-weight:500;">Barang Luar Sistem</h3>
                        </div>
                        <div class="panel-body">
                            <ul class="nav nav-tabs nav-justified" role="tablist">
                                <li role="presentation" class="active"><a href="#DiamondJewelleryL" id="detailDJL" onclick="autoSwitch('DJ','detail')" aria-controls="Diamond Jewellery" role="tab" data-toggle="tab">Diamond Jewellery</a></li>
                                <li role="presentation" ><a href="#PlainGoldL" id="detailPGL" onclick="autoSwitch('PG','detail')" aria-controls="Plain Gold" role="tab" data-toggle="tab">Plain Gold</a></li>
                                <li role="presentation" ><a href="#LooseDiamondL" id="detailLDL" onclick="autoSwitch('LD','detail')" aria-controls="Loose Diamond" role="tab" data-toggle="tab">Loose Diamond</a></li>
                                <!-- <li role="presentation"><a >Profile</a></li>
                                <li role="presentation"><a >Messages</a></li> -->
                            </ul>

                            <div class="tab-content" style="padding-top: 20px;">
                                <div role="tabpanel" class="tab-pane fade in active" id="DiamondJewelleryL">                                        
                                </div>

                                <div role="tabpanel" class="tab-pane fade" id="PlainGoldL">                       
                                </div>

                                <div role="tabpanel" class="tab-pane fade" id="LooseDiamondL">            
                                </div>
                            </div>         
                        </div> 
                    </div> 
                </div>     
            </div>
        </div>
    </div>   


    <script type="text/javascript">
        function showLoader() {
            document.getElementById("loader").style.display = "block";
            document.getElementById("stock-opname").style.display = "none";
        }

        function hideLoader() {
            document.getElementById("loader").style.display = "none";
            document.getElementById("stock-opname").style.display = "block";
        }

        // Example usage:
        showLoader();        

        $(document).ready(function () {
            const urlParams = new URLSearchParams(window.location.search);
            const id = urlParams.get("id");
            const kodeShift = urlParams.get("KodeShift");   
            const namaLokasi = urlParams.get("NamaLoc")

            $('#lokasinya').html(namaLokasi +` (${kodeShift})`)
            let cekWholeDJ=0;
            let cekWholePG=0;
            let cekWholeLD=0;            
            
            let sistemBGLDJ = 0;
            let sistemBRCDJ = 0;
            let sistemEARDJ = 0;
            let sistemLRDJ = 0;
            let sistemBUCKDJ = 0;
            let sistemMJDJ = 0;
            let sistemNECKDJ = 0;
            let sistemBRODJ = 0;
            let sistemPDTDJ = 0;
            let sistemPDTCDJ = 0;
            let sistemWRDJ = 0;
            let sistemOTHDJ = 0;

            let sistemBGLPG = 0;
            let sistemBRCPG = 0;
            let sistemEARPG = 0;
            let sistemLRPG = 0;
            let sistemBUCKPG = 0;
            let sistemMJPG = 0;
            let sistemNECKPG = 0;
            let sistemBROPG = 0;
            let sistemPDTPG = 0;
            let sistemPDTCPG = 0;
            let sistemWRPG = 0;
            let sistemOTHPG = 0;    

            let sistemOTHLD = 0;    


            let totalSistemDJ = 0;
            let totalSistemPG = 0;
            let totalSistemLD = 0;   

            let totalMatchesDJ=0;                                              
            let totalMatchesPG=0;                                              
            let totalMatchesLD=0;                                              

              
            
            getData(kodeShift, id);   
            
            const style = document.createElement('style');
            style.textContent = `
                .my-input-label {
                    color: red;
                    font-weight: bold;
                    margin-bottom: 5px;
                    display: block;
                    text-align:left;
                    width : 85%;
                }
            `;
            document.head.appendChild(style);
        
            
        });

        function getData(kodeShift, id){            
                const parameters = {
                        ID: id,
                        KodeShift: `'${kodeShift}'`
                    };

                $.ajax({                    
                    type: "GET",
                    url: "Detail.aspx/GetData",
                    data: parameters,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        const data = response.d;
                        const dataDisplay = data.DataDisplay;
                        const dataDalam = data.DataDalam;

                        const detailDJ = data.DataDJ;
                        const detailPG = data.DataPG;
                        const detailLD = data.DataLD;
                        
                        const detailDalamDJ = data.DataDalamDJ;
                        const detailDalamPG = data.DataDalamPG;
                        const detailDalamLD = data.DataDalamLD;

                        const detailTitipanDJ = data.DataTitipanDJ;
                        const detailTitipanPG = data.DataTitipanPG;
                        const detailTitipanLD = data.DataTitipanLD;
                        
                        const detailLuarDJ = data.DataLuarDJ;
                        const detailLuarPG = data.DataLuarPG;
                        const detailLuarLD = data.DataLuarLD;

                        const detailSummary = data.DataSummary;
                        const detailSistemDJ = data.DataSistemDJ;
                        const detailSistemPG = data.DataSistemPG;
                        const detailSistemLD = data.DataSistemLD;

                        let htmlDJ = '';
                        let htmlPG = '';
                        let htmlLD = '';                    
                        let htmlDalamDJ = '';
                        let htmlDalamPG = '';
                        let htmlDalamLD = '';                                                            
                        let htmlTitipanDJ = '';
                        let htmlTitipanPG = '';
                        let htmlTitipanLD = '';                    
                        let htmlLuarDJ = '';
                        let htmlLuarPG = '';
                        let htmlLuarLD = '';                  
                    
                        refreshSummary(detailSummary, detailSistemDJ, detailSistemPG, detailSistemLD, id)
                        
                        dataDisplay.forEach(item => {                                           
                            htmlDJ +=`            
                            <div class="panel panel-default">                        
                                <a data-toggle="collapse" data-parent="#accordion" role="tab" href="#DJ${item.ID}" style="text-decoration:none !important;">                                    
                                    <div class="panel-heading sapphierePrimary sapphiere-text" 
                                        style="display:flex; justify-content:space-between; align-items:center; width:auto; border-bottom:1px solid #ebccd1; padding:10px 15px; cursor:pointer;">
                                        <h4 class="panel-title" style="margin:0;" >
                                            ${item.NamaDisplay}
                                        </h4>                                        
                                        <span class="caret"></span>
                                    </div>                                    
                                </a>

                                <div id="DJ${item.ID}" class="panel-collapse collapse" role="tabpanel" aria-labelledby="${item.ID}">
                                    <div class="panel-body" id="bodyDJ-${item.ID}" 
                                        style="background-color:white; border-top:1px solid #ddd; padding:15px;">
                                        <div class="d-flex flex-column gap-1">
                                            <div class="col-12 ">
                                                <table class="table-responsive table ">
                                                <thead>
                                                    <tr>
                                                        <th style="width:500px;">Operator</th>
                                                        <th style="text-align:center;width:150px;">BGL</th>
                                                        <th style="text-align:center;width:150px;">BRC</th>
                                                        <th style="text-align:center;width:150px;">EAR</th>
                                                        <th style="text-align:center;width:150px;">LR</th>
                                                        <th style="text-align:center;width:150px;">BUCK</th>
                                                        <th style="text-align:center;width:150px;">MJ</th>
                                                        <th style="text-align:center;width:150px;">NECK</th>
                                                        <th style="text-align:center;width:150px;">BRO</th>
                                                        <th style="text-align:center;width:150px;">PDT</th>
                                                        <th style="text-align:center;width:150px;">P + C</th>
                                                        <th style="text-align:center;width:150px;">WR</th>
                                                        <th style="text-align:center;width:150px;">OTH</th>                                                        
                                                    </tr>
                                                </thead>
                                                <tbody id="tbody-stockDJ${item.ID}"></tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>                                 
                            </div>

                            `;
                            htmlPG +=`            
                            <div class="panel panel-default">                        
                                <a data-toggle="collapse" data-parent="#accordion" role="tab" href="#PG${item.ID}" style="text-decoration:none !important;">                                    
                                    <div class="panel-heading kuningPrimary sapphiere-text" 
                                        style="display:flex; justify-content:space-between; align-items:center; width:auto; border-bottom:1px solid #ebccd1; padding:10px 15px; cursor:pointer;">
                                        <h4 class="panel-title" style="margin:0;">
                                            ${item.NamaDisplay}
                                        </h4>                                        
                                        <span class="caret"></span>
                                    </div>                                    
                                </a>

                                <div id="PG${item.ID}" class="panel-collapse collapse" role="tabpanel" aria-labelledby="${item.ID}">
                                    <div class="panel-body" id="bodyPG-${item.ID}" 
                                        style="background-color:white; border-top:1px solid #ddd; padding:15px;">
                                        <div class="d-flex flex-column gap-1">
                                            <div class="col-12">
                                                <table class="table-responsive table ">
                                                <thead>
                                                    <tr>
                                                        <th style="width:500px;">Operator</th>
                                                        <th style="text-align:center;width:150px;">BGL</th>
                                                        <th style="text-align:center;width:150px;">BRC</th>
                                                        <th style="text-align:center;width:150px;">EAR</th>
                                                        <th style="text-align:center;width:150px;">LR</th>
                                                        <th style="text-align:center;width:150px;">ANK</th>
                                                        <th style="text-align:center;width:150px;">MJ</th>
                                                        <th style="text-align:center;width:150px;">NECK</th>
                                                        <th style="text-align:center;width:150px;">CH</th>
                                                        <th style="text-align:center;width:150px;">PDT</th>
                                                        <th style="text-align:center;width:150px;">P + C</th>
                                                        <th style="text-align:center;width:150px;">WR</th>
                                                        <th style="text-align:center;width:150px;">OTH</th>                                                        
                                                    </tr>
                                                </thead>
                                                <tbody id="tbody-stockPG${item.ID}"></tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>                                 
                            </div>                                                                    
                            `;
                            htmlLD +=`            
                            <div class="panel panel-default">                        
                                <a data-toggle="collapse" data-parent="#accordion" role="tab" href="#LD${item.ID}" style="text-decoration:none !important;">                                    
                                    <div class="panel-heading emeraldPrimary sapphiere-text" 
                                        style="display:flex; justify-content:space-between; align-items:center; width:auto;
                                        border-bottom:1px solid #ebccd1; padding:10px 15px; cursor:pointer;">
                                        <h4 class="panel-title" style="margin:0;">
                                            ${item.NamaDisplay}
                                        </h4>                                        
                                        <span class="caret"></span>
                                    </div>                                    
                                </a>

                                <div id="LD${item.ID}" class="panel-collapse collapse" role="tabpanel" aria-labelledby="${item.ID}">
                                    <div class="panel-body" id="bodyLD-${item.ID}" 
                                        style="background-color:white; border-top:1px solid #ddd; padding:15px;">
                                        <div class="d-flex flex-column gap-1">
                                            <div class="col-12">
                                                <table class="table-responsive table ">
                                                <thead>
                                                    <tr>
                                                        <th style="width:500px;">Operator</th>
                                                        <th style="text-align:center;width:150px;">OTH</th>                                                        
                                                    </tr>
                                                </thead>
                                                <tbody id="tbody-stockLD${item.ID}"></tbody>
                                                </table>
                                            </div>
                                        </div>
                                    </div>
                                </div>                                 
                            </div>                                                                      
                            `;
                        });

                         dataDalam.forEach(item => { 
                            if (item.IDMasterTableOpname == 2) {
                                                            
                                htmlDalamDJ +=`                                        
                                <div class="panel panel-default">                        
                                    <a data-toggle="collapse" data-parent="#accordion" role="tab" href="#DalamDJ${item.ID}" style="text-decoration:none !important;">                                    
                                        <div class="panel-heading sapphierePrimary sapphiere-text" 
                                            style="display:flex; justify-content:space-between; align-items:center; width:auto; border-bottom:1px solid #ebccd1; padding:10px 15px; cursor:pointer;">
                                            <h4 class="panel-title" style="margin:0;" >
                                                ${item.Nama}
                                            </h4>                                        
                                            <span class="caret"></span>
                                        </div>                                    
                                    </a>

                                    <div id="DalamDJ${item.ID}" class="panel-collapse collapse" role="tabpanel" aria-labelledby="${item.ID}">
                                        <div class="panel-body" id="bodyDalamDJ-${item.ID}" 
                                            style="background-color:white; border-top:1px solid #ddd; padding:15px;">
                                            <div class="d-flex flex-column gap-1">
                                                <div class="col-12 ">
                                                    <table class="table-responsive table ">
                                                    <thead>
                                                        <tr>
                                                            <th style="width:500px;">Operator</th>
                                                            <th style="text-align:center;width:150px;">BGL</th>
                                                            <th style="text-align:center;width:150px;">BRC</th>
                                                            <th style="text-align:center;width:150px;">EAR</th>
                                                            <th style="text-align:center;width:150px;">LR</th>
                                                            <th style="text-align:center;width:150px;">BUCK</th>
                                                            <th style="text-align:center;width:150px;">MJ</th>
                                                            <th style="text-align:center;width:150px;">NECK</th>
                                                            <th style="text-align:center;width:150px;">BRO</th>
                                                            <th style="text-align:center;width:150px;">PDT</th>
                                                            <th style="text-align:center;width:150px;">P + C</th>
                                                            <th style="text-align:center;width:150px;">WR</th>
                                                            <th style="text-align:center;width:150px;">OTH</th>                                                        
                                                            
                                                        </tr>
                                                    </thead>
                                                    <tbody id="tbody-stockDalamDJ${item.ID}"></tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                 
                                </div>

                                `;
                                htmlDalamPG +=`            
                                <div class="panel panel-default">                        
                                    <a data-toggle="collapse" data-parent="#accordion" role="tab" href="#DalamPG${item.ID}" style="text-decoration:none !important;">                                    
                                        <div class="panel-heading kuningPrimary sapphiere-text" 
                                            style="display:flex; justify-content:space-between; align-items:center; width:auto; border-bottom:1px solid #ebccd1; padding:10px 15px; cursor:pointer;">
                                            <h4 class="panel-title" style="margin:0;">
                                                ${item.Nama}
                                            </h4>                                        
                                            <span class="caret"></span>
                                        </div>                                    
                                    </a>

                                    <div id="DalamPG${item.ID}" class="panel-collapse collapse" role="tabpanel" aria-labelledby="${item.ID}">
                                        <div class="panel-body" id="bodyDalamPG-${item.ID}" 
                                            style="background-color:white; border-top:1px solid #ddd; padding:15px;">
                                            <div class="d-flex flex-column gap-1">
                                                <div class="col-12">
                                                    <table class="table-responsive table ">
                                                    <thead>
                                                        <tr>
                                                            <th style="width:500px;">Operator</th>
                                                            <th style="text-align:center;width:150px;">BGL</th>
                                                            <th style="text-align:center;width:150px;">BRC</th>
                                                            <th style="text-align:center;width:150px;">EAR</th>
                                                            <th style="text-align:center;width:150px;">LR</th>
                                                            <th style="text-align:center;width:150px;">ANK</th>
                                                            <th style="text-align:center;width:150px;">MJ</th>
                                                            <th style="text-align:center;width:150px;">NECK</th>
                                                            <th style="text-align:center;width:150px;">CH</th>
                                                            <th style="text-align:center;width:150px;">PDT</th>
                                                            <th style="text-align:center;width:150px;">P + C</th>
                                                            <th style="text-align:center;width:150px;">WR</th>
                                                            <th style="text-align:center;width:150px;">OTH</th>                                                                                                              
                                                        </tr>
                                                    </thead>
                                                    <tbody id="tbody-stockDalamPG${item.ID}"></tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                 
                                </div>                                                                    
                                `;
                                htmlDalamLD +=`            
                                <div class="panel panel-default">                        
                                    <a data-toggle="collapse" data-parent="#accordion" role="tab" href="#DalamLD${item.ID}" style="text-decoration:none !important;">                                    
                                        <div class="panel-heading emeraldPrimary sapphiere-text" 
                                            style="display:flex; justify-content:space-between; align-items:center; width:auto;
                                            border-bottom:1px solid #ebccd1; padding:10px 15px; cursor:pointer;">
                                            <h4 class="panel-title" style="margin:0;">
                                                ${item.Nama}
                                            </h4>                                        
                                            <span class="caret"></span>
                                        </div>                                    
                                    </a>

                                    <div id="DalamLD${item.ID}" class="panel-collapse collapse" role="tabpanel" aria-labelledby="${item.ID}">
                                        <div class="panel-body" id="bodyDalamLD-${item.ID}" 
                                            style="background-color:white; border-top:1px solid #ddd; padding:15px;">
                                            <div class="d-flex flex-column gap-1">
                                                <div class="col-12">
                                                    <table class="table-responsive table ">
                                                    <thead>
                                                        <tr>
                                                            <th style="width:500px;">Operator</th>
                                                            <th style="text-align:center;width:150px;">OTH</th>                                                                                                              
                                                        </tr>
                                                    </thead>
                                                    <tbody id="tbody-stockDalamLD${item.ID}"></tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                 
                                </div>                                                                      
                                `;                                
                            }

                            if (item.IDMasterTableOpname == 3) {                                                                                            
                                htmlTitipanDJ +=`                                        
                                <div class="panel panel-default">                        
                                    <a data-toggle="collapse" data-parent="#accordion" role="tab" href="#TitipanDJ${item.ID}" style="text-decoration:none !important;">                                    
                                        <div class="panel-heading sapphierePrimary sapphiere-text" 
                                            style="display:flex; justify-content:space-between; align-items:center; width:auto; border-bottom:1px solid #ebccd1; padding:10px 15px; cursor:pointer;">
                                            <h4 class="panel-title" style="margin:0;" >
                                                ${item.Nama}
                                            </h4>                                        
                                            <span class="caret"></span>
                                        </div>                                    
                                    </a>

                                    <div id="TitipanDJ${item.ID}" class="panel-collapse collapse" role="tabpanel" aria-labelledby="${item.ID}">
                                        <div class="panel-body" id="bodyTitipanDJ-${item.ID}" 
                                            style="background-color:white; border-top:1px solid #ddd; padding:15px;">
                                            <div class="d-flex flex-column gap-1">
                                                <div class="col-12 ">
                                                    <table class="table-responsive table ">
                                                    <thead>
                                                        <tr>
                                                            <th style="width:500px;">Operator</th>
                                                            <th style="text-align:center;width:150px;">BGL</th>
                                                            <th style="text-align:center;width:150px;">BRC</th>
                                                            <th style="text-align:center;width:150px;">EAR</th>
                                                            <th style="text-align:center;width:150px;">LR</th>
                                                            <th style="text-align:center;width:150px;">BUCK</th>
                                                            <th style="text-align:center;width:150px;">MJ</th>
                                                            <th style="text-align:center;width:150px;">NECK</th>
                                                            <th style="text-align:center;width:150px;">BRO</th>
                                                            <th style="text-align:center;width:150px;">PDT</th>
                                                            <th style="text-align:center;width:150px;">P + C</th>
                                                            <th style="text-align:center;width:150px;">WR</th>
                                                            <th style="text-align:center;width:150px;">OTH</th>                                                                                                              
                                                        </tr>
                                                    </thead>
                                                    <tbody id="tbody-stockTitipanDJ${item.ID}"></tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                 
                                </div>

                                `;
                                htmlTitipanPG +=`            
                                <div class="panel panel-default">                        
                                    <a data-toggle="collapse" data-parent="#accordion" role="tab" href="#TitipanPG${item.ID}" style="text-decoration:none !important;">                                    
                                        <div class="panel-heading kuningPrimary sapphiere-text" 
                                            style="display:flex; justify-content:space-between; align-items:center; width:auto; border-bottom:1px solid #ebccd1; padding:10px 15px; cursor:pointer;">
                                            <h4 class="panel-title" style="margin:0;">
                                                ${item.Nama}
                                            </h4>                                        
                                            <span class="caret"></span>
                                        </div>                                    
                                    </a>

                                    <div id="TitipanPG${item.ID}" class="panel-collapse collapse" role="tabpanel" aria-labelledby="${item.ID}">
                                        <div class="panel-body" id="bodyTitipanPG-${item.ID}" 
                                            style="background-color:white; border-top:1px solid #ddd; padding:15px;">
                                            <div class="d-flex flex-column gap-1">
                                                <div class="col-12">
                                                    <table class="table-responsive table ">
                                                    <thead>
                                                        <tr>
                                                            <th style="width:500px;">Operator</th>
                                                            <th style="text-align:center;width:150px;">BGL</th>
                                                            <th style="text-align:center;width:150px;">BRC</th>
                                                            <th style="text-align:center;width:150px;">EAR</th>
                                                            <th style="text-align:center;width:150px;">LR</th>
                                                            <th style="text-align:center;width:150px;">ANK</th>
                                                            <th style="text-align:center;width:150px;">MJ</th>
                                                            <th style="text-align:center;width:150px;">NECK</th>
                                                            <th style="text-align:center;width:150px;">CH</th>
                                                            <th style="text-align:center;width:150px;">PDT</th>
                                                            <th style="text-align:center;width:150px;">P + C</th>
                                                            <th style="text-align:center;width:150px;">WR</th>
                                                            <th style="text-align:center;width:150px;">OTH</th>                                                                                                              
                                                        </tr>
                                                    </thead>
                                                    <tbody id="tbody-stockTitipanPG${item.ID}"></tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                 
                                </div>                                                                    
                                `;
                                htmlTitipanLD +=`            
                                <div class="panel panel-default">                        
                                    <a data-toggle="collapse" data-parent="#accordion" role="tab" href="#TitipanLD${item.ID}" style="text-decoration:none !important;">                                    
                                        <div class="panel-heading emeraldPrimary sapphiere-text" 
                                            style="display:flex; justify-content:space-between; align-items:center; width:auto;
                                            border-bottom:1px solid #ebccd1; padding:10px 15px; cursor:pointer;">
                                            <h4 class="panel-title" style="margin:0;">
                                                ${item.Nama}
                                            </h4>                                        
                                            <span class="caret"></span>
                                        </div>                                    
                                    </a>

                                    <div id="TitipanLD${item.ID}" class="panel-collapse collapse" role="tabpanel" aria-labelledby="${item.ID}">
                                        <div class="panel-body" id="bodyTitipanLD-${item.ID}" 
                                            style="background-color:white; border-top:1px solid #ddd; padding:15px;">
                                            <div class="d-flex flex-column gap-1">
                                                <div class="col-12">
                                                    <table class="table-responsive table ">
                                                    <thead>
                                                        <tr>
                                                            <th style="width:500px;">Operator</th>
                                                            <th style="text-align:center;width:150px;">OTH</th>                                                                                                              
                                                        </tr>
                                                    </thead>
                                                    <tbody id="tbody-stockTitipanLD${item.ID}"></tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                 
                                </div>                                                                      
                                `;                                
                            }

                            if (item.IDMasterTableOpname == 4) {                                                                                            
                                htmlLuarDJ +=`                                        
                                <div class="panel panel-default">                        
                                    <a data-toggle="collapse" data-parent="#accordion" role="tab" href="#LuarDJ${item.ID}" style="text-decoration:none !important;">                                    
                                        <div class="panel-heading sapphierePrimary sapphiere-text" 
                                            style="display:flex; justify-content:space-between; align-items:center; width:auto; border-bottom:1px solid #ebccd1; padding:10px 15px; cursor:pointer;">
                                            <h4 class="panel-title" style="margin:0;" >
                                                ${item.Nama}
                                            </h4>                                        
                                            <span class="caret"></span>
                                        </div>                                    
                                    </a>

                                    <div id="LuarDJ${item.ID}" class="panel-collapse collapse" role="tabpanel" aria-labelledby="${item.ID}">
                                        <div class="panel-body" id="bodyLuarDJ-${item.ID}" 
                                            style="background-color:white; border-top:1px solid #ddd; padding:15px;">
                                            <div class="d-flex flex-column gap-1">
                                                <div class="col-12 ">
                                                    <table class="table-responsive table ">
                                                    <thead>
                                                        <tr>
                                                            <th style="width:500px;">Operator</th>
                                                            <th style="text-align:center;width:150px;">BGL</th>
                                                            <th style="text-align:center;width:150px;">BRC</th>
                                                            <th style="text-align:center;width:150px;">EAR</th>
                                                            <th style="text-align:center;width:150px;">LR</th>
                                                            <th style="text-align:center;width:150px;">BUCK</th>
                                                            <th style="text-align:center;width:150px;">MJ</th>
                                                            <th style="text-align:center;width:150px;">NECK</th>
                                                            <th style="text-align:center;width:150px;">BRO</th>
                                                            <th style="text-align:center;width:150px;">PDT</th>
                                                            <th style="text-align:center;width:150px;">P + C</th>
                                                            <th style="text-align:center;width:150px;">WR</th>
                                                            <th style="text-align:center;width:150px;">OTH</th>                                                                                                              
                                                        </tr>
                                                    </thead>
                                                    <tbody id="tbody-stockLuarDJ${item.ID}"></tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                 
                                </div>

                                `;
                                htmlLuarPG +=`            
                                <div class="panel panel-default">                        
                                    <a data-toggle="collapse" data-parent="#accordion" role="tab" href="#LuarPG${item.ID}" style="text-decoration:none !important;">                                    
                                        <div class="panel-heading kuningPrimary sapphiere-text" 
                                            style="display:flex; justify-content:space-between; align-items:center; width:auto; border-bottom:1px solid #ebccd1; padding:10px 15px; cursor:pointer;">
                                            <h4 class="panel-title" style="margin:0;">
                                                ${item.Nama}
                                            </h4>                                        
                                            <span class="caret"></span>
                                        </div>                                    
                                    </a>

                                    <div id="LuarPG${item.ID}" class="panel-collapse collapse" role="tabpanel" aria-labelledby="${item.ID}">
                                        <div class="panel-body" id="bodyLuarPG-${item.ID}" 
                                            style="background-color:white; border-top:1px solid #ddd; padding:15px;">
                                            <div class="d-flex flex-column gap-1">
                                                <div class="col-12">
                                                    <table class="table-responsive table ">
                                                    <thead>
                                                        <tr>
                                                            <th style="width:500px;">Operator</th>
                                                            <th style="text-align:center;width:150px;">BGL</th>
                                                            <th style="text-align:center;width:150px;">BRC</th>
                                                            <th style="text-align:center;width:150px;">EAR</th>
                                                            <th style="text-align:center;width:150px;">LR</th>
                                                            <th style="text-align:center;width:150px;">ANK</th>
                                                            <th style="text-align:center;width:150px;">MJ</th>
                                                            <th style="text-align:center;width:150px;">NECK</th>
                                                            <th style="text-align:center;width:150px;">CH</th>
                                                            <th style="text-align:center;width:150px;">PDT</th>
                                                            <th style="text-align:center;width:150px;">P + C</th>
                                                            <th style="text-align:center;width:150px;">WR</th>
                                                            <th style="text-align:center;width:150px;">OTH</th>                                                                                                              
                                                        </tr>
                                                    </thead>
                                                    <tbody id="tbody-stockLuarPG${item.ID}"></tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                 
                                </div>                                                                    
                                `;
                                htmlLuarLD +=`            
                                <div class="panel panel-default">                        
                                    <a data-toggle="collapse" data-parent="#accordion" role="tab" href="#LuarLD${item.ID}" style="text-decoration:none !important;">                                    
                                        <div class="panel-heading emeraldPrimary sapphiere-text" 
                                            style="display:flex; justify-content:space-between; align-items:center; width:auto;
                                            border-bottom:1px solid #ebccd1; padding:10px 15px; cursor:pointer;">
                                            <h4 class="panel-title" style="margin:0;">
                                                ${item.Nama}
                                            </h4>                                        
                                            <span class="caret"></span>
                                        </div>                                    
                                    </a>

                                    <div id="LuarLD${item.ID}" class="panel-collapse collapse" role="tabpanel" aria-labelledby="${item.ID}">
                                        <div class="panel-body" id="bodyLuarLD-${item.ID}" 
                                            style="background-color:white; border-top:1px solid #ddd; padding:15px;">
                                            <div class="d-flex flex-column gap-1">
                                                <div class="col-12">
                                                    <table class="table-responsive table ">
                                                    <thead>
                                                        <tr>
                                                            <th style="width:500px;">Operator</th>
                                                            <th style="text-align:center;width:150px;">OTH</th>                                                                                                              
                                                        </tr>
                                                    </thead>
                                                    <tbody id="tbody-stockLuarLD${item.ID}"></tbody>
                                                    </table>
                                                </div>
                                            </div>
                                        </div>
                                    </div>                                 
                                </div>                                                                      
                                `;                                
                            }
                        });
                        let html1 = `
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                        ${htmlDJ}
                                        </div>
                                    </div>
                        `;
                        let html2 = `
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                        ${htmlPG}
                                        </div>
                                    </div>
                        `;
                        let html3 = `
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                        ${htmlLD}
                                        </div>
                                    </div>
                        `;

                        let html4 = `
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                        ${htmlDalamDJ}
                                        </div>
                                    </div>
                        `;
                        let html5 = `
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                        ${htmlDalamPG}
                                        </div>
                                    </div>
                        `;
                        let html6 = `
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                        ${htmlDalamLD}
                                        </div>
                                    </div>
                        `;

                        let html7 = `
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                        ${htmlTitipanDJ}
                                        </div>
                                    </div>
                        `;
                        let html8 = `
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                        ${htmlTitipanPG}
                                        </div>
                                    </div>
                        `;
                        let html9 = `
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                        ${htmlTitipanLD}
                                        </div>
                                    </div>
                        `;

                        let html10 = `
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                        ${htmlLuarDJ}
                                        </div>
                                    </div>
                        `;
                        let html11 = `
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                        ${htmlLuarPG}
                                        </div>
                                    </div>
                        `;
                        let html12 = `
                                    <div class="panel panel-default">
                                        <div class="panel-body">
                                        ${htmlLuarLD}
                                        </div>
                                    </div>
                        `;


                        // $('#DJ').html(html1);                    
                        $('#DiamondJewellery').html(html1);
                        $('#PlainGold').html(html2);
                        $('#LooseDiamond').html(html3);

                        $('#DiamondJewelleryD').html(html4);
                        $('#PlainGoldD').html(html5);
                        $('#LooseDiamondD').html(html6);

                        $('#DiamondJewelleryT').html(html7);
                        $('#PlainGoldT').html(html8);
                        $('#LooseDiamondT').html(html9);
                        console.log([$('#DiamondJewelleryL'),html10])
                        $('#DiamondJewelleryL').html(html10);
                        $('#PlainGoldL').html(html11);
                        $('#LooseDiamondL').html(html12);

                        detailDalamDJ.forEach((item,index)=>{
                            let colorPallete = '';
                            /* let colorPallete = item.TipeUser == 1?'ijoPrimary':item.TipeUser == 2 ?'biruPrimary':'kuningPrimary'; */
                            var htmlDataDalamDJ = `
                                <tr >
                                    <td class="${colorPallete}">
                                        ${item.Created_by} (${item.TipeUser == 1?'AE':item.TipeUser == 2 ?'JR':'Other'})</td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">
                                        <label for="BGLdalamDJ${item.ID}">${item.BGLDJ}</label> 
                                        <input class="form-control dalamDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BGLdalamDJ${item.ID}" value="${item.BGLDJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">
                                        <label for="BRCdalamDJ${item.ID}">${item.BRCDJ}</label> 
                                        <input class="form-control dalamDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BRCdalamDJ${item.ID}" value="${item.BRCDJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">
                                        <label for="EARdalamDJ${item.ID}">${item.EARDJ}</label> 
                                        <input class="form-control dalamDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="EARdalamDJ${item.ID}"  value="${item.EARDJ}" />
                                    </td>
                                    <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                        <label for="LRdalamDJ${item.ID}">${item.LRDJ}</label> 
                                        <input class="form-control dalamDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="LRdalamDJ${item.ID}" value="${item.LRDJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                        <label for="BUCKdalamDJ${item.ID}">${item.BUCKDJ}</label> 
                                        <input class="form-control dalamDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BUCKdalamDJ${item.ID}"  value="${item.BUCKDJ}" />
                                    </td>
                                    <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                        <label for="MJdalamDJ${item.ID}">${item.MJDJ}</label> 
                                        <input class="form-control dalamDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="MJdalamDJ${item.ID}"  value="${item.MJDJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                        <label for="NECKdalamDJ${item.ID}">${item.NECKDJ}</label> 
                                        <input class="form-control dalamDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="NECKdalamDJ${item.ID}" value="${item.NECKDJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                        <label for="BROdalamDJ${item.ID}">${item.BRODJ}</label> 
                                        <input class="form-control dalamDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BROdalamDJ${item.ID}"  value="${item.BRODJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                        <label for="PDTdalamDJ${item.ID}">${item.PDTDJ}</label> 
                                        <input class="form-control dalamDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="PDTdalamDJ${item.ID}"  value="${item.PDTDJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                        <label for="PDTCdalamDJ${item.ID}">${item.PDTCDJ}</label> 
                                        <input class="form-control dalamDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="PDTCdalamDJ${item.ID}" value="${item.PDTCDJ}" />
                                    </td>
                                    <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                        <label for="WRdalamDJ${item.ID}">${item.WRDJ}</label> 
                                        <input class="form-control dalamDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="WRdalamDJ${item.ID}"  value="${item.WRDJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                        <label for="OTHdalamDJ${item.ID}">${item.OTHDJ}</label> 
                                        <input class="form-control dalamDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="OTHdalamDJ${item.ID}"  value="${item.OTHDJ}" />
                                    </td>                                      
                                </tr>`;                                   
                            $(`#tbody-stockDalamDJ${item.IDTipeOpname}`).append(htmlDataDalamDJ);
                        })

                        detailDalamPG.forEach((item,index)=>{                        
                            let colorPallete = '';
                            /* let colorPallete = item.TipeUser == 1?'ijoPrimary':item.TipeUser == 2 ?'biruPrimary':'kuningPrimary'; */
                            var htmlDataDalamPG = `
                                 <tr >
                                    <td class="${colorPallete}">
                                        ${item.Created_by} (${item.TipeUser == 1?'AE':item.TipeUser == 2 ?'JR':'Other'})</td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">
                                        <label for="BGLDalamPG${item.ID}">${item.BGLPG}</label> 
                                        <input class="form-control dalamPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BGLdalamPG${item.ID}" value="${item.BGLPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">
                                        <label for="BRCdalamPG${item.ID}">${item.BRCPG}</label> 
                                        <input class="form-control dalamPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BRCdalamPG${item.ID}" value="${item.BRCPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">
                                        <label for="EARdalamPG${item.ID}">${item.EARPG}</label> 
                                        <input class="form-control dalamPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="EARdalamPG${item.ID}"  value="${item.EARPG}" />
                                    </td>
                                    <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                        <label for="LRdalamPG${item.ID}">${item.LRPG}</label> 
                                        <input class="form-control dalamPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="LRdalamPG${item.ID}" value="${item.LRPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                        <label for="BUCKdalamPG${item.ID}">${item.BUCKPG}</label> 
                                        <input class="form-control dalamPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BUCKdalamPG${item.ID}"  value="${item.BUCKPG}" />
                                    </td>
                                    <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                        <label for="MJdalamPG${item.ID}">${item.MJPG}</label> 
                                        <input class="form-control dalamPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="MJdalamPG${item.ID}"  value="${item.MJPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                        <label for="NECKdalamPG${item.ID}">${item.NECKPG}</label> 
                                        <input class="form-control dalamPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="NECKdalamPG${item.ID}" value="${item.NECKPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                        <label for="BROdalamPG${item.ID}">${item.BROPG}</label> 
                                        <input class="form-control dalamPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BROdalamPG${item.ID}"  value="${item.BROPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                        <label for="PDTdalamPG${item.ID}">${item.PDTPG}</label> 
                                        <input class="form-control dalamPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="PDTdalamPG${item.ID}"  value="${item.PDTPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                        <label for="PDTCdalamPG${item.ID}">${item.PDTCPG}</label> 
                                        <input class="form-control dalamPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="PDTCdalamPG${item.ID}" value="${item.PDTCPG}" />
                                    </td>
                                    <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                        <label for="WRdalamPG${item.ID}">${item.WRPG}</label> 
                                        <input class="form-control dalamPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="WRdalamPG${item.ID}"  value="${item.WRPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                        <label for="OTHdalamPG${item.ID}">${item.OTHPG}</label> 
                                        <input class="form-control dalamPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="OTHdalamPG${item.ID}"  value="${item.OTHPG}" />
                                    </td>                                           
                                </tr>`;                                   
                            $(`#tbody-stockDalamPG${item.IDTipeOpname}`).append(htmlDataDalamPG);
                        })

                        detailDalamLD.forEach((item,index)=>{                        
                            let colorPallete = '';
                            /* let colorPallete = item.TipeUser == 1?'ijoPrimary':item.TipeUser == 2 ?'biruPrimary':'kuningPrimary'; */
                            var htmlDataDalamLD = `
                                 <tr>
                                    <td class="${colorPallete}">
                                        ${item.Created_by} (${item.TipeUser == 1?'AE':item.TipeUser == 2 ?'JR':'Other'})
                                    </td>                                    
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                        <label for="OTHdalamLD${item.ID}">${item.OTHLD}</label> 
                                        <input class="form-control dalamLD${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="OTHdalamLD${item.ID}"  value="${item.OTHLD}" />
                                    </td>                                       
                                </tr>`;
                            $(`#tbody-stockDalamLD${item.IDTipeOpname}`).append(htmlDataDalamLD);
                        })

                        // Start Data Titipan
                            detailTitipanDJ.forEach((item,index)=>{
                                let colorPallete = '';
                                /* let colorPallete = item.TipeUser == 1?'ijoPrimary':item.TipeUser == 2 ?'biruPrimary':'kuningPrimary'; */
                                var htmlDataTitipanDJ = `
                                    <tr >
                                        <td class="${colorPallete}">
                                            ${item.Created_by} (${item.TipeUser == 1?'AE':item.TipeUser == 2 ?'JR':'Other'})</td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">
                                            <label for="BGLTitipanDJ${item.ID}">${item.BGLDJ}</label> 
                                            <input class="form-control TitipanDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BGLTitipanDJ${item.ID}" value="${item.BGLDJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">
                                            <label for="BRCTitipanDJ${item.ID}">${item.BRCDJ}</label> 
                                            <input class="form-control TitipanDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BRCTitipanDJ${item.ID}" value="${item.BRCDJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">
                                            <label for="EARTitipanDJ${item.ID}">${item.EARDJ}</label> 
                                            <input class="form-control TitipanDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="EARTitipanDJ${item.ID}"  value="${item.EARDJ}" />
                                        </td>
                                        <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                            <label for="LRTitipanDJ${item.ID}">${item.LRDJ}</label> 
                                            <input class="form-control TitipanDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="LRTitipanDJ${item.ID}" value="${item.LRDJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                            <label for="BUCKTitipanDJ${item.ID}">${item.BUCKDJ}</label> 
                                            <input class="form-control TitipanDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BUCKTitipanDJ${item.ID}"  value="${item.BUCKDJ}" />
                                        </td>
                                        <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                            <label for="MJTitipanDJ${item.ID}">${item.MJDJ}</label> 
                                            <input class="form-control TitipanDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="MJTitipanDJ${item.ID}"  value="${item.MJDJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                            <label for="NECKTitipanDJ${item.ID}">${item.NECKDJ}</label> 
                                            <input class="form-control TitipanDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="NECKTitipanDJ${item.ID}" value="${item.NECKDJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                            <label for="BROTitipanDJ${item.ID}">${item.BRODJ}</label> 
                                            <input class="form-control TitipanDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BROTitipanDJ${item.ID}"  value="${item.BRODJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                            <label for="PDTTitipanDJ${item.ID}">${item.PDTDJ}</label> 
                                            <input class="form-control TitipanDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="PDTTitipanDJ${item.ID}"  value="${item.PDTDJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                            <label for="PDTCTitipanDJ${item.ID}">${item.PDTCDJ}</label> 
                                            <input class="form-control TitipanDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="PDTCTitipanDJ${item.ID}" value="${item.PDTCDJ}" />
                                        </td>
                                        <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                            <label for="WRTitipanDJ${item.ID}">${item.WRDJ}</label> 
                                            <input class="form-control TitipanDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="WRTitipanDJ${item.ID}"  value="${item.WRDJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                            <label for="OTHTitipanDJ${item.ID}">${item.OTHDJ}</label> 
                                            <input class="form-control TitipanDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="OTHTitipanDJ${item.ID}"  value="${item.OTHDJ}" />
                                        </td>                                             
                                    </tr>`;                                   
                                $(`#tbody-stockTitipanDJ${item.IDTipeOpname}`).append(htmlDataTitipanDJ);
                            })

                            detailTitipanPG.forEach((item,index)=>{                        
                                let colorPallete = '';
                                /* let colorPallete = item.TipeUser == 1?'ijoPrimary':item.TipeUser == 2 ?'biruPrimary':'kuningPrimary'; */
                                var htmlDataTitipanPG = `
                                    <tr >
                                        <td class="${colorPallete}">
                                            ${item.Created_by} (${item.TipeUser == 1?'AE':item.TipeUser == 2 ?'JR':'Other'})</td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">
                                            <label for="BGLTitipanPG${item.ID}">${item.BGLPG}</label> 
                                            <input class="form-control TitipanPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BGLTitipanPG${item.ID}" value="${item.BGLPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">
                                            <label for="BRCTitipanPG${item.ID}">${item.BRCPG}</label> 
                                            <input class="form-control TitipanPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BRCTitipanPG${item.ID}" value="${item.BRCPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">
                                            <label for="EARTitipanPG${item.ID}">${item.EARPG}</label> 
                                            <input class="form-control TitipanPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="EARTitipanPG${item.ID}"  value="${item.EARPG}" />
                                        </td>
                                        <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                            <label for="LRTitipanPG${item.ID}">${item.LRPG}</label> 
                                            <input class="form-control TitipanPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="LRTitipanPG${item.ID}" value="${item.LRPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                            <label for="BUCKTitipanPG${item.ID}">${item.BUCKPG}</label> 
                                            <input class="form-control TitipanPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BUCKTitipanPG${item.ID}"  value="${item.BUCKPG}" />
                                        </td>
                                        <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                            <label for="MJTitipanPG${item.ID}">${item.MJPG}</label> 
                                            <input class="form-control TitipanPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="MJTitipanPG${item.ID}"  value="${item.MJPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                            <label for="NECKTitipanPG${item.ID}">${item.NECKPG}</label> 
                                            <input class="form-control TitipanPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="NECKTitipanPG${item.ID}" value="${item.NECKPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                            <label for="BROTitipanPG${item.ID}">${item.BROPG}</label> 
                                            <input class="form-control TitipanPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BROTitipanPG${item.ID}"  value="${item.BROPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                            <label for="PDTTitipanPG${item.ID}">${item.PDTPG}</label> 
                                            <input class="form-control TitipanPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="PDTTitipanPG${item.ID}"  value="${item.PDTPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                            <label for="PDTCTitipanPG${item.ID}">${item.PDTCPG}</label> 
                                            <input class="form-control TitipanPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="PDTCTitipanPG${item.ID}" value="${item.PDTCPG}" />
                                        </td>
                                        <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                            <label for="WRTitipanPG${item.ID}">${item.WRPG}</label> 
                                            <input class="form-control TitipanPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="WRTitipanPG${item.ID}"  value="${item.WRPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                            <label for="OTHTitipanPG${item.ID}">${item.OTHPG}</label> 
                                            <input class="form-control TitipanPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="OTHTitipanPG${item.ID}"  value="${item.OTHPG}" />
                                        </td>                                         
                                    </tr>`;                                   
                                $(`#tbody-stockTitipanPG${item.IDTipeOpname}`).append(htmlDataTitipanPG);
                            })

                            detailTitipanLD.forEach((item,index)=>{                        
                                let colorPallete = '';
                                /* let colorPallete = item.TipeUser == 1?'ijoPrimary':item.TipeUser == 2 ?'biruPrimary':'kuningPrimary'; */
                                var htmlDataTitipanLD = `
                                    <tr>
                                        <td class="${colorPallete}">
                                            ${item.Created_by} (${item.TipeUser == 1?'AE':item.TipeUser == 2 ?'JR':'Other'})
                                        </td>                                    
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                            <label for="OTHTitipanLD${item.ID}">${item.OTHLD}</label> 
                                            <input class="form-control TitipanLD${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="OTHTitipanLD${item.ID}"  value="${item.OTHLD}" />
                                        </td>                                                                            
                                    </tr>`;
                                $(`#tbody-stockTitipanLD${item.IDTipeOpname}`).append(htmlDataTitipanLD);
                            })
                        // End Data Titipan

                        // Start Data Luar
                            detailLuarDJ.forEach((item,index)=>{
                                let colorPallete = '';
                                /* let colorPallete = item.TipeUser == 1?'ijoPrimary':item.TipeUser == 2 ?'biruPrimary':'kuningPrimary'; */
                                var htmlDataLuarDJ = `
                                    <tr >
                                        <td class="${colorPallete}">
                                            ${item.Created_by} (${item.TipeUser == 1?'AE':item.TipeUser == 2 ?'JR':'Other'})</td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">
                                            <label for="BGLLuarDJ${item.ID}">${item.BGLDJ}</label> 
                                            <input class="form-control LuarDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BGLLuarDJ${item.ID}" value="${item.BGLDJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">
                                            <label for="BRCLuarDJ${item.ID}">${item.BRCDJ}</label> 
                                            <input class="form-control LuarDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BRCLuarDJ${item.ID}" value="${item.BRCDJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">
                                            <label for="EARLuarDJ${item.ID}">${item.EARDJ}</label> 
                                            <input class="form-control LuarDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="EARLuarDJ${item.ID}"  value="${item.EARDJ}" />
                                        </td>
                                        <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                            <label for="LRLuarDJ${item.ID}">${item.LRDJ}</label> 
                                            <input class="form-control LuarDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="LRLuarDJ${item.ID}" value="${item.LRDJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                            <label for="BUCKLuarDJ${item.ID}">${item.BUCKDJ}</label> 
                                            <input class="form-control LuarDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BUCKLuarDJ${item.ID}"  value="${item.BUCKDJ}" />
                                        </td>
                                        <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                            <label for="MJLuarDJ${item.ID}">${item.MJDJ}</label> 
                                            <input class="form-control LuarDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="MJLuarDJ${item.ID}"  value="${item.MJDJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                            <label for="NECKLuarDJ${item.ID}">${item.NECKDJ}</label> 
                                            <input class="form-control LuarDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="NECKLuarDJ${item.ID}" value="${item.NECKDJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                            <label for="BROLuarDJ${item.ID}">${item.BRODJ}</label> 
                                            <input class="form-control LuarDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BROLuarDJ${item.ID}"  value="${item.BRODJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                            <label for="PDTLuarDJ${item.ID}">${item.PDTDJ}</label> 
                                            <input class="form-control LuarDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="PDTLuarDJ${item.ID}"  value="${item.PDTDJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                            <label for="PDTCLuarDJ${item.ID}">${item.PDTCDJ}</label> 
                                            <input class="form-control LuarDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="PDTCLuarDJ${item.ID}" value="${item.PDTCDJ}" />
                                        </td>
                                        <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                            <label for="WRLuarDJ${item.ID}">${item.WRDJ}</label> 
                                            <input class="form-control LuarDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="WRLuarDJ${item.ID}"  value="${item.WRDJ}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                            <label for="OTHLuarDJ${item.ID}">${item.OTHDJ}</label> 
                                            <input class="form-control LuarDJ${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="OTHLuarDJ${item.ID}"  value="${item.OTHDJ}" />
                                        </td>                                           
                                    </tr>`;                                   
                                $(`#tbody-stockLuarDJ${item.IDTipeOpname}`).append(htmlDataLuarDJ);
                            })

                            detailLuarPG.forEach((item,index)=>{                        
                                let colorPallete = '';
                                /* let colorPallete = item.TipeUser == 1?'ijoPrimary':item.TipeUser == 2 ?'biruPrimary':'kuningPrimary'; */
                                var htmlDataLuarPG = `
                                    <tr >
                                        <td class="${colorPallete}">
                                            ${item.Created_by} (${item.TipeUser == 1?'AE':item.TipeUser == 2 ?'JR':'Other'})</td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">
                                            <label for="BGLLuarPG${item.ID}">${item.BGLPG}</label> 
                                            <input class="form-control LuarPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BGLLuarPG${item.ID}" value="${item.BGLPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">
                                            <label for="BRCLuarPG${item.ID}">${item.BRCPG}</label> 
                                            <input class="form-control LuarPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BRCLuarPG${item.ID}" value="${item.BRCPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">
                                            <label for="EARLuarPG${item.ID}">${item.EARPG}</label> 
                                            <input class="form-control LuarPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="EARLuarPG${item.ID}"  value="${item.EARPG}" />
                                        </td>
                                        <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                            <label for="LRLuarPG${item.ID}">${item.LRPG}</label> 
                                            <input class="form-control LuarPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="LRLuarPG${item.ID}" value="${item.LRPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                            <label for="BUCKLuarPG${item.ID}">${item.BUCKPG}</label> 
                                            <input class="form-control LuarPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BUCKLuarPG${item.ID}"  value="${item.BUCKPG}" />
                                        </td>
                                        <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                            <label for="MJLuarPG${item.ID}">${item.MJPG}</label> 
                                            <input class="form-control LuarPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="MJLuarPG${item.ID}"  value="${item.MJPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                            <label for="NECKLuarPG${item.ID}">${item.NECKPG}</label> 
                                            <input class="form-control LuarPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="NECKLuarPG${item.ID}" value="${item.NECKPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                            <label for="BROLuarPG${item.ID}">${item.BROPG}</label> 
                                            <input class="form-control LuarPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="BROLuarPG${item.ID}"  value="${item.BROPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                            <label for="PDTLuarPG${item.ID}">${item.PDTPG}</label> 
                                            <input class="form-control LuarPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="PDTLuarPG${item.ID}"  value="${item.PDTPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                            <label for="PDTCLuarPG${item.ID}">${item.PDTCPG}</label> 
                                            <input class="form-control LuarPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="PDTCLuarPG${item.ID}" value="${item.PDTCPG}" />
                                        </td>
                                        <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                            <label for="WRLuarPG${item.ID}">${item.WRPG}</label> 
                                            <input class="form-control LuarPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="WRLuarPG${item.ID}"  value="${item.WRPG}" />
                                        </td>
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                            <label for="OTHLuarPG${item.ID}">${item.OTHPG}</label> 
                                            <input class="form-control LuarPG${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="OTHLuarPG${item.ID}"  value="${item.OTHPG}" />
                                        </td>                                        
                                    </tr>`;                                   
                                $(`#tbody-stockLuarPG${item.IDTipeOpname}`).append(htmlDataLuarPG);
                            })

                            detailLuarLD.forEach((item,index)=>{                        
                                let colorPallete = '';
                                /* let colorPallete = item.TipeUser == 1?'ijoPrimary':item.TipeUser == 2 ?'biruPrimary':'kuningPrimary'; */
                                var htmlDataLuarLD = `
                                    <tr>
                                        <td class="${colorPallete}">
                                            ${item.Created_by} (${item.TipeUser == 1?'AE':item.TipeUser == 2 ?'JR':'Other'})
                                        </td>                                    
                                        <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                            <label for="OTHLuarLD${item.ID}">${item.OTHLD}</label> 
                                            <input class="form-control LuarLD${item.ID}" type="hidden" onkeypress="return onlyNumbersAndCommas(event)" onpaste="return false;" id="OTHLuarLD${item.ID}"  value="${item.OTHLD}" />
                                        </td>                                             
                                    </tr>`;
                                $(`#tbody-stockLuarLD${item.IDTipeOpname}`).append(htmlDataLuarLD);
                            })
                        // End Data Titipan

                        detailDJ.forEach((item,index)=>{
                            let colorPallete = item.TipeUser == 1?'ijoPrimary':item.TipeUser == 2 ?'biruPrimary':'kuningPrimary';
                            var htmlDataDJ = `
                                <tr >
                                    <td class="${colorPallete}">
                                        ${item.Created_by} (${item.TipeUser == 1?'AE':item.TipeUser == 2 ?'JR':'Other'})</td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">
                                        <label for="BGLDJ${item.ID}">${item.BGLDJ}</label> 
                                        <input class="form-control DJ${item.ID}" type="hidden" id="BGLDJ${item.ID}" value="${item.BGLDJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">
                                        <label for="BRCDJ${item.ID}">${item.BRCDJ}</label> 
                                        <input class="form-control DJ${item.ID}" type="hidden" id="BRCDJ${item.ID}" value="${item.BRCDJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">
                                        <label for="EARDJ${item.ID}">${item.EARDJ}</label> 
                                        <input class="form-control DJ${item.ID}" type="hidden" id="EARDJ${item.ID}"  value="${item.EARDJ}" />
                                    </td>
                                    <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                        <label for="LRDJ${item.ID}">${item.LRDJ}</label> 
                                        <input class="form-control DJ${item.ID}" type="hidden" id="LRDJ${item.ID}" value="${item.LRDJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                        <label for="BUCKDJ${item.ID}">${item.BUCKDJ}</label> 
                                        <input class="form-control DJ${item.ID}" type="hidden" id="BUCKDJ${item.ID}"  value="${item.BUCKDJ}" />
                                    </td>
                                    <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                        <label for="MJDJ${item.ID}">${item.MJDJ}</label> 
                                        <input class="form-control DJ${item.ID}" type="hidden" id="MJDJ${item.ID}"  value="${item.MJDJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                        <label for="NECKDJ${item.ID}">${item.NECKDJ}</label> 
                                        <input class="form-control DJ${item.ID}" type="hidden" id="NECKDJ${item.ID}" value="${item.NECKDJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                        <label for="BRODJ${item.ID}">${item.BRODJ}</label> 
                                        <input class="form-control DJ${item.ID}" type="hidden" id="BRODJ${item.ID}"  value="${item.BRODJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                        <label for="PDTDJ${item.ID}">${item.PDTDJ}</label> 
                                        <input class="form-control DJ${item.ID}" type="hidden" id="PDTDJ${item.ID}"  value="${item.PDTDJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                        <label for="PDTCDJ${item.ID}">${item.PDTCDJ}</label> 
                                        <input class="form-control DJ${item.ID}" type="hidden" id="PDTCDJ${item.ID}" value="${item.PDTCDJ}" />
                                    </td>
                                    <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                        <label for="WRDJ${item.ID}">${item.WRDJ}</label> 
                                        <input class="form-control DJ${item.ID}" type="hidden" id="WRDJ${item.ID}"  value="${item.WRDJ}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                        <label for="OTHDJ${item.ID}">${item.OTHDJ}</label> 
                                        <input class="form-control DJ${item.ID}" type="hidden" id="OTHDJ${item.ID}"  value="${item.OTHDJ}" />
                                    </td>                                    
                                </tr>`;                                   
                            $(`#tbody-stockDJ${item.IDDisplay}`).append(htmlDataDJ);
                        })

                        detailPG.forEach((item,index)=>{                        
                             let colorPallete = item.TipeUser == 1?'ijoPrimary':item.TipeUser == 2 ?'biruPrimary':'kuningPrimary';
                            var htmlDataPG = `
                                 <tr >
                                    <td class="${colorPallete}">
                                        ${item.Created_by} (${item.TipeUser == 1?'AE':item.TipeUser == 2 ?'JR':'Other'})</td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">
                                        <label for="BGLPG${item.ID}">${item.BGLPG}</label> 
                                        <input class="form-control PG${item.ID}" type="hidden" id="BGLPG${item.ID}" value="${item.BGLPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">
                                        <label for="BRCPG${item.ID}">${item.BRCPG}</label> 
                                        <input class="form-control PG${item.ID}" type="hidden" id="BRCPG${item.ID}" value="${item.BRCPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">
                                        <label for="EARPG${item.ID}">${item.EARPG}</label> 
                                        <input class="form-control PG${item.ID}" type="hidden" id="EARPG${item.ID}"  value="${item.EARPG}" />
                                    </td>
                                    <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                        <label for="LRPG${item.ID}">${item.LRPG}</label> 
                                        <input class="form-control PG${item.ID}" type="hidden" id="LRPG${item.ID}" value="${item.LRPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                        <label for="BUCKPG${item.ID}">${item.BUCKPG}</label> 
                                        <input class="form-control PG${item.ID}" type="hidden" id="BUCKPG${item.ID}"  value="${item.BUCKPG}" />
                                    </td>
                                    <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                        <label for="MJPG${item.ID}">${item.MJPG}</label> 
                                        <input class="form-control PG${item.ID}" type="hidden" id="MJPG${item.ID}"  value="${item.MJPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                        <label for="NECKPG${item.ID}">${item.NECKPG}</label> 
                                        <input class="form-control PG${item.ID}" type="hidden" id="NECKPG${item.ID}" value="${item.NECKPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                        <label for="BROPG${item.ID}">${item.BROPG}</label> 
                                        <input class="form-control PG${item.ID}" type="hidden" id="BROPG${item.ID}"  value="${item.BROPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                        <label for="PDTPG${item.ID}">${item.PDTPG}</label> 
                                        <input class="form-control PG${item.ID}" type="hidden" id="PDTPG${item.ID}"  value="${item.PDTPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;x;" class="${colorPallete}">                                    
                                        <label for="PDTCPG${item.ID}">${item.PDTCPG}</label> 
                                        <input class="form-control PG${item.ID}" type="hidden" id="PDTCPG${item.ID}" value="${item.PDTCPG}" />
                                    </td>
                                    <td style="text-align:center;width:15x;" class="${colorPallete}">                                    
                                        <label for="WRPG${item.ID}">${item.WRPG}</label> 
                                        <input class="form-control PG${item.ID}" type="hidden" id="WRPG${item.ID}"  value="${item.WRPG}" />
                                    </td>
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                        <label for="OTHPG${item.ID}">${item.OTHPG}</label> 
                                        <input class="form-control PG${item.ID}" type="hidden" id="OTHPG${item.ID}"  value="${item.OTHPG}" />
                                    </td>                                    
                                </tr>`;                                   
                            $(`#tbody-stockPG${item.IDDisplay}`).append(htmlDataPG);
                        })

                        detailLD.forEach((item,index)=>{                        
                             let colorPallete = item.TipeUser == 1?'ijoPrimary':item.TipeUser == 2 ?'biruPrimary':'kuningPrimary';
                            var htmlDataLD = `
                                 <tr>
                                    <td class="${colorPallete}">
                                        ${item.Created_by} (${item.TipeUser == 1?'AE':item.TipeUser == 2 ?'JR':'Other'})
                                    </td>                                    
                                    <td style="text-align:center;width:150px;" class="${colorPallete}">                                    
                                        <label for="OTHLD${item.ID}">${item.OTHLD}</label> 
                                        <input class="form-control LD${item.ID}" type="hidden" id="OTHLD${item.ID}"  value="${item.OTHLD}" />
                                    </td>                                    
                                </tr>`;
                            $(`#tbody-stockLD${item.IDDisplay}`).append(htmlDataLD);
                        })

                    },
                    error: function (error) {
                        console.error("Error fetching data:", error);
                    },
                    complete: function (data) {
                        hideLoader() // Hide after 3 seconds
                    }
                });
                
            }
    
                
        function refreshSummary(DataSummary, DataSistemDJ, DataSistemPG, DataSistemLD, id){
            let htmlSumDJ = '';  
            let totalSumDJ = 0;
            let htmlSumPG = '';             
            let totalSumPG = 0;
            let htmlSumLD = '';
            let totalSumLD = 0;              

            let htmlFootSumDJ = '';
            let htmlFootSumPG = '';
            let htmlFootSumLD = '';

            DataSistemDJ.forEach((item,i)=>{
                sistemBGLDJ = item.BGLDJ;
                sistemBRCDJ = item.BRCDJ;
                sistemEARDJ = item.EARDJ;
                sistemLRDJ = item.LRDJ;
                sistemBUCKDJ = item.BUCKDJ;
                sistemMJDJ = item.MJDJ;
                sistemNECKDJ = item.NECKDJ;
                sistemBRODJ = item.BRODJ;
                sistemPDTDJ = item.PDTDJ;
                sistemPDTCDJ = item.PDTCDJ;
                sistemWRDJ = item.WRDJ;
                sistemOTHDJ = item.OTHDJ;
                totalSistemDJ = sistemBGLDJ + sistemBRCDJ + sistemEARDJ + sistemLRDJ + sistemBUCKDJ + sistemMJDJ + sistemNECKDJ + sistemBRODJ + sistemPDTDJ + sistemPDTCDJ + sistemWRDJ + sistemOTHDJ;                
               htmlFootSumDJ +=`               
                <tr class="biruTua">
                    <th>Data Sistem</th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.BGLDJ}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.BRCDJ}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.EARDJ}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.LRDJ}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.BUCKDJ}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.MJDJ}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.NECKDJ}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.BRODJ}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.PDTDJ}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.PDTCDJ}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.WRDJ}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.OTHDJ}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${totalSistemDJ}</label>
                    </th>
                </tr>
               `;
            });

            DataSistemPG.forEach((item,i)=>{
                sistemBGLPG = item.BGLPG;
                sistemBRCPG = item.BRCPG;
                sistemEARPG = item.EARPG;
                sistemLRPG = item.LRPG;
                sistemBUCKPG = item.BUCKPG;
                sistemMJPG = item.MJPG;
                sistemNECKPG = item.NECKPG;
                sistemBROPG = item.BROPG;
                sistemPDTPG = item.PDTPG;
                sistemPDTCPG = item.PDTCPG;
                sistemWRPG = item.WRPG;
                sistemOTHPG = item.OTHPG;
                totalSistemPG = sistemBGLPG + sistemBRCPG + sistemEARPG + sistemLRPG + sistemBUCKPG + sistemMJPG + sistemNECKPG + sistemBROPG + sistemPDTPG + sistemPDTCPG + sistemWRPG + sistemOTHPG;
               htmlFootSumPG +=`
                <tr class="biruTua">
                    <th>Data Sistem</th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.BGLPG}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.BRCPG}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.EARPG}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.LRPG}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.BUCKPG}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.MJPG}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.NECKPG}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.BROPG}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.PDTPG}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.PDTCPG}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.WRPG}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${item.OTHPG}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${totalSistemPG}</label>
                    </th>
                </tr>
               `;
            });
            DataSistemLD.forEach((item,i)=>{
                sistemOTHLD = item.OTHLD;
                totalSistemLD  = sistemOTHLD;
               htmlFootSumLD +=`
                <tr class="biruTua">
                    <th>Data Sistem</th>                    
                    <th style="text-align:center;width:150px;" >
                        <label>${item.OTHLD}</label>
                    </th>
                    <th style="text-align:center;width:150px;">
                        <label>${totalSistemLD}</label>
                    </th>
                </tr>
               `;
            });        

            DataSummary.forEach((item,i)=>{      
                totalSumDJ = item.BGLDJ + item.BRCDJ + item.EARDJ + item.LRDJ + item.BUCKDJ + item.MJDJ + item.NECKDJ + item.BRODJ + item.PDTDJ + item.PDTCDJ + item.WRDJ + item.OTHDJ;
                totalSumPG = item.BGLPG + item.BRCPG + item.EARPG + item.LRPG + item.BUCKPG + item.MJPG + item.NECKPG + item.BROPG + item.PDTPG + item.PDTCPG + item.WRPG + item.OTHPG;
                totalSumLD = item.OTHLD;
                // let isSelisih = totalSumDJ != totalSistemDJ ? "background-color:#f2dede;":"ijoPrimary";                
                let colorPallete = item.TipeUser == 1?'ijoPrimary':item.TipeUser == 2 ?'biruPrimary':'biruTua';
                htmlSumDJ += `                
                    <tr class="summaryDJ">
                        <td class="${colorPallete}">
                            ${item.TipeUser == 1?'Administrative Executive':item.TipeUser == 2 ?'Jewellery Representative':'Others'}
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.BGLDJ != sistemBGLDJ ? "merahPrimary":colorPallete}">
                            <label for="SumBGLDJ">${item.BGLDJ}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.BRCDJ != sistemBRCDJ ? "merahPrimary":colorPallete}">
                            <label for="SumBRCDJ">${item.BRCDJ}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.EARDJ != sistemEARDJ ? "merahPrimary":colorPallete}">
                            <label for="SumEARDJ">${item.EARDJ}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.LRDJ != sistemLRDJ ? "merahPrimary":colorPallete}">
                            <label for="SumLRDJ">${item.LRDJ}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.BUCKDJ != sistemBUCKDJ ? "merahPrimary":colorPallete}">
                            <label for="SumBUCKDJ">${item.BUCKDJ}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.MJDJ != sistemMJDJ ? "merahPrimary":colorPallete}">
                            <label for="SumMJDJ">${item.MJDJ}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.NECKDJ != sistemNECKDJ ? "merahPrimary":colorPallete}">
                            <label for="SumNECKDJ">${item.NECKDJ}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.BRODJ != sistemBRODJ ? "merahPrimary":colorPallete}">
                            <label for="SumBRODJ">${item.BRODJ}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.PDTDJ != sistemPDTDJ ? "merahPrimary":colorPallete}">
                            <label for="SumPDTDJ">${item.PDTDJ}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.PDTCDJ != sistemPDTCDJ ? "merahPrimary":colorPallete}">
                            <label for="SumPDTCDJ">${item.PDTCDJ}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.WRDJ != sistemWRDJ ? "merahPrimary":colorPallete}">
                            <label for="SumWRDJ">${item.WRDJ}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.OTHDJ != sistemOTHDJ ? "merahPrimary":colorPallete}">
                            <label for="SumOTHDJ">${item.OTHDJ}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${totalSumDJ != totalSistemDJ ? "merahPrimary":colorPallete}">
                            <label for="SumTotalDJ">${totalSumDJ}</label>                             
                        </td>                       
                    </tr>
                `;                               
                htmlSumPG += `
                    <tr class="summaryPG">
                        <td class="${colorPallete}">
                            ${item.TipeUser == 1?'Administrative Executive':item.TipeUser == 2 ?'Jewellery Representative':'Others'}
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.BGLPG != sistemBGLPG ? "merahPrimary":colorPallete}">
                            <label for="SumBGLPG">${item.BGLPG}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.BRCPG != sistemBRCPG ? "merahPrimary":colorPallete}">
                            <label for="SumBRCPG">${item.BRCPG}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.EARPG != sistemEARPG ? "merahPrimary":colorPallete}">
                            <label for="SumEARPG">${item.EARPG}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.LRPG != sistemLRPG ? "merahPrimary":colorPallete}">
                            <label for="SumLRPG">${item.LRPG}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.BUCKPG != sistemBUCKPG ? "merahPrimary":colorPallete}">
                            <label for="SumBUCKPG">${item.BUCKPG}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.MJPG != sistemMJPG ? "merahPrimary":colorPallete}">
                            <label for="SumMJPG">${item.MJPG}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.NECKPG != sistemNECKPG ? "merahPrimary":colorPallete}">
                            <label for="SumNECKPG">${item.NECKPG}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.BROPG != sistemBROPG ? "merahPrimary":colorPallete}">
                            <label for="SumBROPG">${item.BROPG}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.PDTPG != sistemPDTPG ? "merahPrimary":colorPallete}">
                            <label for="SumPDTPG">${item.PDTPG}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.PDTCPG != sistemPDTCPG ? "merahPrimary":colorPallete}">
                            <label for="SumPDTCPG">${item.PDTCPG}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.WRPG != sistemWRPG ? "merahPrimary":colorPallete}">
                            <label for="SumWRPG">${item.WRPG}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${item.OTHPG != sistemOTHPG ? "merahPrimary":colorPallete}">
                            <label for="SumOTHPG">${item.OTHPG}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${totalSumPG != totalSistemPG ? "merahPrimary":colorPallete}">
                            <label for="SumTotalPG">${totalSumPG}</label>                             
                        </td>                       
                    </tr>
                `;                               
                htmlSumLD += `
                    <tr class="summaryLD">
                        <td class="${colorPallete}">
                            ${item.TipeUser == 1?'Administrative Executive':item.TipeUser == 2 ?'Jewellery Representative':'Others'}</td>                        
                        <td style="text-align:center;width:150px;" class="${totalSumLD != totalSistemLD ? "merahPrimary":colorPallete}">                                    
                            <label for="SumOTHLD">${item.OTHLD}</label>                             
                        </td>
                        <td style="text-align:center;width:150px;" class="${totalSumLD != totalSistemLD ? "merahPrimary":colorPallete}">                                    
                            <label for="SumTotalLD">${totalSumLD}</label>                             
                        </td>                       
                    </tr>
                `;                               
            })

            $('#tbody-sumDJ').html(htmlSumDJ);
            $('#tbody-sumPG').html(htmlSumPG);
            $('#tbody-sumLD').html(htmlSumLD);

            $('#tfoot-sumDJ').html(htmlFootSumDJ)
            $('#tfoot-sumPG').html(htmlFootSumPG)
            $('#tfoot-sumLD').html(htmlFootSumLD)

            let sumLabelDJ = [];
            let sumLabelPG = [];
            let sumLabelLD = [];
            let totalDJ = 0;
            let totalPG = 0;
            let totalLD = 0;

            $('tr.summaryDJ label').each(function () {
                let key = $(this).attr('for');
                if (!sumLabelDJ.includes(key) && key !== 'SumTotalDJ') {
                    sumLabelDJ.push(key);
                }
            });
            $('tr.summaryPG label').each(function () {
                let key = $(this).attr('for');
                if (!sumLabelPG.includes(key) && key !== 'SumTotalPG') {
                    sumLabelPG.push(key);
                }
            });
            $('tr.summaryLD label').each(function () {
                let key = $(this).attr('for');
                if (!sumLabelLD.includes(key) && key !== 'SumTotalLD') {
                    sumLabelLD.push(key);
                }
            });
            

            sumLabelDJ.forEach(labelId => {                
                totalDJ += checkMatches(labelId, 'DJ');
            });            
            sumLabelPG.forEach(labelId => {                
                totalPG += checkMatches(labelId, 'PG');
            });            
            sumLabelLD.forEach(labelId => {                
                totalLD += checkMatches(labelId, 'LD');
            });            


            
            $(`#badgeSumDJ`).addClass(totalDJ == 12 ? 'ijoNyala' : 'merahNyala');
            $(`#badgeSumDJ`).text(totalDJ+'/12 Matches');
            cekWholeDJ=totalDJ;              

            $(`#badgeSumPG`).addClass(totalPG == 12 ? 'ijoNyala' : 'merahNyala');
            $(`#badgeSumPG`).text(totalPG+'/12 Matches');
            cekWholePG=totalPG;

            $(`#badgeSumLD`).addClass(totalLD == 1 ? 'ijoNyala' : 'merahNyala');
            $(`#badgeSumLD`).text(totalLD+'/1 Matches');            
            cekWholeLD=totalLD            
            

        }

        function checkMatches(id, jenis){            
            const pink = 'rgb(254, 226, 226)'; // Pink color in RGB format
            let totalan = 0;

            let rows = $(`tr.summary${jenis}`);      // your summary rows
            let row1 = rows.eq(0);             // admin
            let row2 = rows.eq(1);             // jewellery

            let matches = 0;            

            let td1 = row1.find(`label[for="${id}"]`).closest('td');
            let td2 = row2.find(`label[for="${id}"]`).closest('td');

            if (td1.length && td2.length) {
                let bg1 = td1.css('background-color') || '';
                let bg2 = td2.css('background-color') || '';

                if (bg1 !== pink && bg2 !== pink) {
                    totalan+=1;                 
                }
            }
            
            // let totalMatches = jenis == 'DJ' ? totalMatchesDJ : jenis == 'PG' ? totalMatchesPG : totalMatchesLD;
            return totalan;
            // update UI
            
        }
       
</script>
    
</asp:content>

