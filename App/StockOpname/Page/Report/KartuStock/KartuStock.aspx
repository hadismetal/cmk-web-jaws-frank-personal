<%@ page language="C#" masterpagefile="./MasterPage.master" autoeventwireup="true" codefile="KartuStock.aspx.cs" inherits="Page_kartustock" %>

<asp:content contentplaceholderid="h1" runat="server">
	Rekap Stock
</asp:content>

<asp:content contentplaceholderid="content" runat="server">
    <style>
        .merahPrimary { background-color: #fee2e2; } .merahNyala { background-color: #f44336; color: white; }
        .ijoPrimary { background-color: #dff0d8; } .ijoNyala{ background-color: #16A34A;color: white; }
        .biruPrimary { background-color: #d9edf7; } .kuningPrimary { background-color: #D5EDFF; }
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
            <!-- Nav tabs -->
            <div class="panel panel-success mx-3">
                <div class="panel-heading">
                    <h3 class="panel-title dark-text" style="font-size:20px !important;font-weight:500;">Detail Opname</h3>
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

        <div class="container-fluid" style="margin:40px;float:right;" id="btnAppruv">
            
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
                    url: "KartuStock.aspx/GetData",
                    data: parameters,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (response) {
                        const data = response.d;
                        const dataDisplay = data.DataDisplay;
                        const detailDJ = data.DataDJ;
                        const detailPG = data.DataPG;
                        const detailLD = data.DataLD;
                        const detailSummary = data.DataSummary;
                        const detailSistemDJ = data.DataSistemDJ;
                        const detailSistemPG = data.DataSistemPG;
                        const detailSistemLD = data.DataSistemLD;

                        let htmlDJ = '';
                        let htmlPG = '';
                        let htmlLD = '';                    
                    
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
                                                        <th>Action</th>
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
                                                        <th style="text-align:center;width:150px;">BUCK</th>
                                                        <th style="text-align:center;width:150px;">MJ</th>
                                                        <th style="text-align:center;width:150px;">NECK</th>
                                                        <th style="text-align:center;width:150px;">BRO</th>
                                                        <th style="text-align:center;width:150px;">PDT</th>
                                                        <th style="text-align:center;width:150px;">P + C</th>
                                                        <th style="text-align:center;width:150px;">WR</th>
                                                        <th style="text-align:center;width:150px;">OTH</th>
                                                        <th>Action</th>
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
                                                        <th>Action</th>
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


                        // $('#DJ').html(html1);                    
                        $('#DiamondJewellery').html(html1);
                        $('#PlainGold').html(html2);
                        $('#LooseDiamond').html(html3);


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
                                    <td class="${colorPallete}">
                                        <a  onclick="ganti(${item.ID},'update', 'DJ')" id="updateDJ${item.ID}" style="text-decoration:none;"><i class="icon-pencil"></i></a>
                                        <a  onclick="ganti(${item.ID},'oke', 'DJ')" id="okDJ${item.ID}" class="text-success" style="text-decoration:none;display:none;"><i class="icon-ok"></i></a>
                                        <a  onclick="ganti(${item.ID},'no', 'DJ')" id="noDJ${item.ID}" class="text-danger" style="text-decoration:none; display:none;"><i class="icon-remove"></i></a>
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
                                    <td class="${colorPallete}">
                                        <a  onclick="ganti(${item.ID},'update', 'PG')" id="updatePG${item.ID}" style="text-decoration:none;"><i class="icon-pencil"></i></a>
                                        <a  onclick="ganti(${item.ID},'oke', 'PG')" id="okPG${item.ID}" class="text-success" style="text-decoration:none;display:none;"><i class="icon-ok"></i></a>
                                        <a  onclick="ganti(${item.ID},'no', 'PG')" id="noPG${item.ID}" class="text-danger" style="text-decoration:none; display:none;"><i class="icon-remove"></i></a>
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
                                    <td class="${colorPallete}">
                                        <a  onclick="ganti(${item.ID},'update', 'LD')" id="updateLD${item.ID}" style="text-decoration:none;"><i class="icon-pencil"></i></a>
                                        <a  onclick="ganti(${item.ID},'oke', 'LD')" id="okLD${item.ID}" class="text-success" style="text-decoration:none;display:none;"><i class="icon-ok"></i></a>
                                        <a  onclick="ganti(${item.ID},'no', 'LD')" id="noLD${item.ID}" class="text-danger" style="text-decoration:none; display:none;"><i class="icon-remove"></i></a>
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
    
        
        function ganti(id, tipe, jenis){         
            const labelBGL = $(`label[for=BGL${jenis}${id}]`);
            const labelBRC = $(`label[for=BRC${jenis}${id}]`);
            const labelEAR = $(`label[for=EAR${jenis}${id}]`);
            const labelLR = $(`label[for=LR${jenis}${id}]`);
            const labelBUCK = $(`label[for=BUCK${jenis}${id}]`);
            const labelMJ = $(`label[for=MJ${jenis}${id}]`);
            const labelNECK = $(`label[for=NECK${jenis}${id}]`);
            const labelBRO = $(`label[for=BRO${jenis}${id}]`);
            const labelPDT = $(`label[for=PDT${jenis}${id}]`);
            const labelPDTC = $(`label[for=PDTC${jenis}${id}]`);
            const labelWR = $(`label[for=WR${jenis}${id}]`);
            const labelOTH = $(`label[for=OTH${jenis}${id}]`);
            
            if (tipe == 'update') {
                
                labelBGL.attr("style","display:none")
                labelBRC.attr("style","display:none")
                labelEAR.attr("style","display:none")
                labelLR.attr("style","display:none")
                labelBUCK.attr("style","display:none")
                labelMJ.attr("style","display:none")
                labelNECK.attr("style","display:none")
                labelBRO.attr("style","display:none")
                labelPDT.attr("style","display:none")
                labelPDTC.attr("style","display:none")
                labelWR.attr("style","display:none")
                labelOTH.attr("style","display:none")
                
                $(`.${jenis}${id}`).attr("type","text")
                $(`#update${jenis}${id}`).css('display','none')
                $(`#ok${jenis}${id}`).css('display','')
                $(`#no${jenis}${id}`).css('display','')
            }else if (tipe == 'oke') {
                Swal.fire({
                title: "Apakah anda yakin?",
                text :"Pastikan data yang tertera masukkan sudah benar!",
                showDenyButton: true,                
                icon: "question",
                confirmButtonText: "Yakin",
                }).then((result) => {
                    if (result.isConfirmed) {
                        UpdateData(id, jenis)

                        labelBGL.removeAttr("style","display:none")
                        labelBRC.removeAttr("style","display:none")
                        labelEAR.removeAttr("style","display:none")
                        labelLR.removeAttr("style","display:none")
                        labelBUCK.removeAttr("style","display:none")
                        labelMJ.removeAttr("style","display:none")
                        labelNECK.removeAttr("style","display:none")
                        labelBRO.removeAttr("style","display:none")
                        labelPDT.removeAttr("style","display:none")
                        labelPDTC.removeAttr("style","display:none")
                        labelWR.removeAttr("style","display:none")
                        labelOTH.removeAttr("style","display:none")

                        

                        $(`.${jenis}${id}`).attr("type","hidden")
                        $(`#update${jenis}${id}`).css('display','')
                        $(`#ok${jenis}${id}`).css('display','none')
                        $(`#no${jenis}${id}`).css('display','none')
                    }
                });
            }else{
                labelBGL.removeAttr("style","display:none")
                labelBRC.removeAttr("style","display:none")
                labelEAR.removeAttr("style","display:none")
                labelLR.removeAttr("style","display:none")
                labelBUCK.removeAttr("style","display:none")
                labelMJ.removeAttr("style","display:none")
                labelNECK.removeAttr("style","display:none")
                labelBRO.removeAttr("style","display:none")
                labelPDT.removeAttr("style","display:none")
                labelPDTC.removeAttr("style","display:none")
                labelWR.removeAttr("style","display:none")
                labelOTH.removeAttr("style","display:none")

                

                $(`.${jenis}${id}`).attr("type","hidden")
                $(`#update${jenis}${id}`).css('display','')
                $(`#ok${jenis}${id}`).css('display','none')
                $(`#no${jenis}${id}`).css('display','none')
            }            
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
                <tr class="kuningPrimary">
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
                <tr>
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
                <tr>
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
                let colorPallete = item.TipeUser == 1?'ijoPrimary':item.TipeUser == 2 ?'biruPrimary':'kuningPrimary';
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

            $('#btnAppruv').html(`<button class="btn btn-success" onclick="ApproveSO(${id}, ${cekWholeDJ}, ${cekWholePG}, ${cekWholeLD})" id="btnApprove">Approve</button>`)
            

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

        function UpdateData(ID, Jenis){
            const url = new URLSearchParams(window.location.search);
            const idOP = url.get("id");
            const shiftCode = url.get("KodeShift");
            let dataBGL = 0;
            let dataBRC = 0;
            let dataEAR = 0;
            let dataLR = 0;
            let dataBUCK = 0;
            let dataMJ = 0;
            let dataNECK = 0;
            let dataBRO = 0;
            let dataPDT = 0;
            let dataPDTC = 0;
            let dataWR = 0;
            let dataOTH = 0;

            if (Jenis != 'LD') {                            
                dataBGL =$(`#BGL${Jenis}${ID}`).val()
                dataBRC =$(`#BRC${Jenis}${ID}`).val()
                dataEAR =$(`#EAR${Jenis}${ID}`).val()
                dataLR =$(`#LR${Jenis}${ID}`).val()
                dataBUCK =$(`#BUCK${Jenis}${ID}`).val()
                dataMJ =$(`#MJ${Jenis}${ID}`).val()
                dataNECK =$(`#NECK${Jenis}${ID}`).val()
                dataBRO =$(`#BRO${Jenis}${ID}`).val()
                dataPDT =$(`#PDT${Jenis}${ID}`).val()
                dataPDTC =$(`#PDTC${Jenis}${ID}`).val()
                dataWR =$(`#WR${Jenis}${ID}`).val()                                
            }
            dataOTH =$(`#OTH${Jenis}${ID}`).val()
            const parameters = {
                BGL : dataBGL,
                BRC : dataBRC,
                EAR : dataEAR,
                LR : dataLR,
                BUCK : dataBUCK,
                MJ : dataMJ,
                NECK : dataNECK,
                BRO : dataBRO,
                PDT : dataPDT,
                PDTC : dataPDTC,
                WR : dataWR,
                OTH : dataOTH,
                Jenis : Jenis,
                ID : ID            
            }
            
            $.ajax({                    
                type: "POST",
                url: "KartuStock.aspx/PostUpdateData",
                data: JSON.stringify({IDataOpname: parameters}),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {  
                    let data = response['d'];
                    let datas = data[0];                    
                    if (datas.Status == 200) {
                        toastr.success(`Success Update Data Opname`);
                    } else if (datas.Status == 404){
                        toastr.error(`Data Not Found`);
                        return;
                    }else {
                        toastr.error(`Error Insert Data Opname : ${datas.Message}`);
                        return;
                    }
                    
                    getData(shiftCode, idOP)                    
                },
                error: function (error) {
                    console.error("Error fetching data:", error);
                }
                });


        }
        
        
        let isAutoSwitching = false;

        function autoSwitch(tipe, jenis) {
            if (isAutoSwitching) return;

            isAutoSwitching = true;

            if (jenis == 'header') {
                $(`#detail${tipe}`).click();
            } else {
                $(`#header${tipe}`).click();
            }

            isAutoSwitching = false;
        }
        function ApproveSO(id, cekWholeDJ, cekWholePG, cekWholeLD){
            let alertDJ = cekWholeDJ != 12 ? 1 : 0;
            let alertPG = cekWholePG != 12 ? 1 : 0;
            let alertLD = cekWholeLD != 1 ? 1 : 0;
            let icon = ``;
            let title = ``;
            let text = ``;

            if(alertDJ){
                text = `Masih terdapat selisih sebanyak ${12-cekWholeDJ} pada data DJ!`;
            }else if(alertPG){
                text = `Masih terdapat selisih sebanyak ${12-cekWholePG} pada data PG!`;
            }else if (alertLD){
                text= `Masih terdapat selisih sebanyak ${1-cekWholeLD} pada data LD!`;
            }else{
                text= `Pastikan data anda sudah benar!`;
            }

            Swal.fire({
                icon : `warning`,
                title : 'Apakah Anda Yakin?!',
                text : text,                 
                input: 'textarea',
                inputLabel: 'Keterangan : *',
                inputPlaceholder: 'Write your message here...',
                showCancelButton: true,
                confirmButtonText: 'Yakin',
                cancelButtonText: 'Cancel',
                customClass: {
                    inputLabel: 'my-input-label'
                },
                showCancelButton: true,
                confirmButtonText: "Yakin",
                cancelButtonText: "Cancel"
            }).then((result)=>{
                if (result.isConfirmed) {
                    let remark = result.value;

                    if (!remark || remark.trim() === "") {
                        Swal.fire({
                            icon: "error",
                            title: "Oops!",
                            text: "Keterangan wajib diisi untuk approval."
                        });
                        return;
                    }
                    // console.log(id)
                    ApproveDataOpname(id, remark, alertDJ, alertPG, alertLD);
                    
                }
            });
        }
        function ApproveDataOpname(ID, Keterangan, isSelisihDJ, isSelisihPG, isSelisihLD){            
            const parameters = {
                ID : ID,
                KETERANGAN : Keterangan.trim(),
                ISSELISIHDJ : isSelisihDJ,
                ISSELISIHPG : isSelisihPG,
                ISSELISIHLD : isSelisihLD
            }
            $.ajax({                
                type: "POST",
                url: "KartuStock.aspx/ApproveDataOpname",
                data: JSON.stringify(parameters),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (response) {  
                    let data = response['d'];
                    let datas = data[0];                    
                    if (datas.Status == 200) {
                        Swal.fire({
                            icon: "success",
                            title: "Success!",
                            text: `Success Approve Data Opname`
                        }).then(() => {
                            window.location.href = "Index.aspx";
                        });
                    } else if (datas.Status == 404){
                        toastr.error(`Data Not Found`);
                        return;
                    }else {
                        toastr.error(`Error Approve Data Opname : ${datas.Message}`);
                        return;
                    }                    
                    
                },
                error: function (error) {
                    console.error("Error fetching data:", error);
                }
                });
        
        }
</script>
    
</asp:content>

