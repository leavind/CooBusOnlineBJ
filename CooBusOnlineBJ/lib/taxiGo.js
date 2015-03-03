//去除左右空格
String.prototype.trim=function(){
	return this.replace(/(^\s*)|(\s*$)/g, "");
}

//去除左空格
String.prototype.ltrim=function(){
	return this.replace(/(^\s*)/g,"");
}

//去除右空格
String.prototype.rtrim=function(){
	return this.replace(/(\s*$)/g,"");
}

//鼠标点击“您的位置”输入域事件
function keywordMousedown(){
	if(!isUserInput){
		var keyword = document.getElementById("keyword");
		keywordHistory = keyword.value;
		keyword.value = "";
	}
}

//“您的位置”输入域失去焦点事件
function keywordBlur(){
	if(!isUserInput && keywordHistory == "屏幕点选位置"){
		document.getElementById("keyword").value = keywordHistory;
	}
}

//"您的位置"输入域键盘按钮事件
function keywordKeyup(){
	isUserInput=true;
	var placeDiv = document.getElementById("placeDiv");
	document.getElementById("findKeyWordBut").style.display = "";
	placeDiv.innerHTML = "<textarea name=\"textarea\" rows=\"4\" class=\"SearchInput2\"></textarea>";
}

//查询按钮事件
function searchButClick(){
	var startId = document.getElementById("startId");
	
	if((startId == null || startId.value == "") && isUserInput){
		findKeyWord()
	}else{
		searchTaxi();
	}
}

//点击您的位置搜索按钮
function findKeyWord(){
	searchPoi();
}

//查询POI点
function searchPoi(){
	var keyword = document.getElementById("keyword").value;
	keyword = keyword.trim();
	if(keyword == ""){
		alert("请填写您的位置!");
		return false;
	}

	document.getElementById("place1Tr").style.display = "";
	document.getElementById("place2Tr").style.display = "";
	var xmlHttp;
 	try{
   		 xmlHttp=new XMLHttpRequest(); 
   	}catch(e){ 
  		try{ 
  			xmlHttp=new ActiveXObject("Msxml2.XMLHTTP"); 
	    }catch(e){
			try{
			   xmlHttp=new ActiveXObject("Microsoft.XMLHTTP"); 
			}catch (e){ 
			   alert("您的浏览器不支持Ajax！"); 
			   return false;
            }
        }
    }
   	
    keyword = encodeURI(encodeURI(keyword));
	var url = rootpath+"/jsp/taxi/searchmap.jsp?keyword="+keyword;
    xmlHttp.open("post",url,false);
    xmlHttp.send(null); 
  	if(xmlHttp.readyState==4) { 
  		//获取显示区域
  		document.getElementById("placeDiv").innerHTML = xmlHttp.responseText;
  		var startId = document.getElementById("startId");
  		var options = startId.getElementsByTagName("option");
  		if(options.length == 1){
  			var option = options[0];
  			taxistartop(option.value);
  			searchTaxi();
  		}
  	} 
}

function taxiGo(){
	var lonNum = Number(document.getElementById("carstartx").value)/1000000;
    var latNum = Number(document.getElementById("carstarty").value)/1000000;
    document.getElementById("lon").value = lonNum+"";
	document.getElementById("lat").value = latNum+"";
	document.getElementById("selfDrivingIframe").contentWindow.drowPoint(lonNum+"", latNum+"", "assets/images/here.png");
	searchTaxi();
}

//位置改变
function taxistartop(str){
	var strs = new Array();
    strs = str.split(",");
    var lonNum = Number(strs[1])/1000000;
    var latNum = Number(strs[2])/1000000;
    
    document.getElementById("keyword").value = strs[0];
    document.getElementById("lon").value = lonNum+"";
	document.getElementById("lat").value = latNum+"";
	
	document.getElementById("selfDrivingIframe").contentWindow.mapClear();  //清除地图内容
	document.getElementById("selfDrivingIframe").contentWindow.drowPoint(lonNum+"", latNum+"", "assets/images/here.png");
}	


//查询后台并获取出租车信息
function searchTaxi(){
	var xmlHttp = "";
	try{
		xmlHttp=new XMLHttpRequest();
	} catch (e)  {
		try {
			xmlHttp=new ActiveXObject("Msxml2.XMLHTTP");
		}catch(e){
			try{
				xmlHttp=new ActiveXObject("Microsoft.XMLHTTP");
			}catch(e){
				alert("您的浏览器不支持Ajax！");
				return false;
			}
		}
	}
	
	var meter = document.getElementById("meter").value;
	var lon = document.getElementById("lon").value;
	var lat = document.getElementById("lat").value;
    if (lon =="" || lat==""){
    	alert("请输入您的位置， 或点击按钮在地图中选择您的位置。");
    	document.getElementById("keyword").value = "";
    	return false;
    }
    
	var url = rootpath+"/taxi/action/taxiList.do?lon="+lon+"&lat="+lat+"&meter="+meter; 
	
    xmlHttp.open("post",url,false);
    xmlHttp.setRequestHeader("Content-type","application/x-www-form-urlencoded");
    xmlHttp.send(null); 

	if(xmlHttp.readyState==4) {       
		var taxiHtmlDiv = document.getElementById("taxiHtmlDiv");
		var str = xmlHttp.responseText; 
		var thestr = new Array();
		thestr = str.split("$");
		
		var htmlText="<table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>";
		var distanceInfo = thestr[0].split("&");
		tdArray = new Array();
		for(var i=0; i<distanceInfo.length; i++){
			var taxisNumInfo = distanceInfo[i].split(",");
			if(i>0){
				htmlText += "<tr><td style='height:5px'></td></tr>";
			}
			var meterNum = Number(taxisNumInfo[0]);
			tdArray[tdArray.length] = "taxiInfoTd"+meterNum;
			htmlText += "<tr><td id='taxiInfoTd"+meterNum+"' onmouseover='changeTdBgColor(this,1,"+meterNum+")'"
				+ " 	onmouseout='changeTdBgColor(this,0,"+meterNum+")' onclick='changeTdBgColor(this,2,"+meterNum+")'>"
				+ "  <table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>"
				+ "<tr><td align='left' sytel='padding-left:36pt'><a href='javascript:drowMapCircle("+meterNum+")' target='_self'><span class='font12 STYLE8'><b>"
			if(meterNum >= 1000){
				htmlText += (meterNum/1000)+"公里范围内";
			}else{
				htmlText += meterNum+"米范围内";
			}
			htmlText += "</b></span></a></td></tr>"
				+ "<tr><td align='center'><img src='"+rootpath+"/image/taxi.gif' width='100' height='70' border='0'/></td><td>"
				+ "  <table width='100%' border='0' align='center' cellpadding='0' cellspacing='0'>"
				+ "  <tr><td colspan='2'><span class='font12 STYLE8'>空车"+taxisNumInfo[1]+"辆</span></td></tr>"
				+ "  <tr><td valign='top'>&nbsp;</td></tr></table>"
				+ "</td></tr></table></td><tr>";
		}
		htmlText += "</table>"
		taxiHtmlDiv.innerHTML = htmlText;
		
		document.getElementById("selfDrivingIframe").contentWindow.taxiLayerClear();  //清除出租车图层地图内容
		var color = "0xFF0000";
		document.getElementById("selfDrivingIframe").contentWindow.drowCircle(lon,lat,meter, color, false);  //画圆
		document.getElementById("selfDrivingIframe").contentWindow.showPOISearchResult(thestr[1]);   //显示POI点
		
		
		if(meter == '200'){
			document.getElementById("selfDrivingIframe").contentWindow.maplevel(7);
		}else if(meter == '500'){
			document.getElementById("selfDrivingIframe").contentWindow.maplevel(6);
		}else if(meter == '1000'){
			document.getElementById("selfDrivingIframe").contentWindow.maplevel(5);
		}else if(meter == '1500'){
			document.getElementById("selfDrivingIframe").contentWindow.maplevel(5);
		}else if(meter == '2000'){
			document.getElementById("selfDrivingIframe").contentWindow.maplevel(4);
		}else if(meter == '3000'){
			document.getElementById("selfDrivingIframe").contentWindow.maplevel(4);
		}else if(meter == '5000'){
			document.getElementById("selfDrivingIframe").contentWindow.maplevel(3);
		}
	}
}
//改变td颜色
function changeTdBgColor(tdObj, state, meterNum){
	if(state == 0 && (tdObj.style.backgroundColor == "#f6f7fc" || tdObj.style.backgroundColor == "rgb(246, 247, 252)")){
		tdObj.style.backgroundColor = "#ffffff";
	}else if(state == 1 && tdObj.style.backgroundColor != "#e7ebfa" && tdObj.style.backgroundColor != "rgb(231, 235, 250)"){
		tdObj.style.backgroundColor = "#f6f7fc";
	}else if(state == 2){
		for(var i=0; i<tdArray.length; i++){
			var taxiInfoTd = document.getElementById(tdArray[i]);
			taxiInfoTd.style.backgroundColor = "#ffffff";
		}
		tdObj.style.backgroundColor = "#e7ebfa";
		drowMapCircle(meterNum);
	}
}
//在地图上画圆并填充
function drowMapCircle(meterNum){
	var color = "0x0000FF";
	var lon = document.getElementById("lon").value;
	var lat = document.getElementById("lat").value;
	document.getElementById("selfDrivingIframe").contentWindow.drowCircle(lon,lat,meterNum+"", color, true);  //画圆
}

//点击此处并在地图中选择您的位置
function seleteMapPoint(){
	document.getElementById("selfDrivingIframe").contentWindow.mapClear();  //清除地图内容
	document.getElementById("selfDrivingIframe").contentWindow.getMousePointLonLat();   //获取地图上鼠标位置的经纬度
}
//设置经纬度坐标点
function setPonitLonLat(lon, lat){
	document.getElementById("lon").value = lon;
	document.getElementById("lat").value = lat;
	document.getElementById("keyword").value = "屏幕点选位置";
	isUserInput = false;
	document.body.style.cursor = "";
	
	var startId = document.getElementById("startId");
	if(startId != null && startId != ""){
		startId.value = "";
	}
	document.getElementById("findKeyWordBut").style.display = "none";
	document.getElementById("place1Tr").style.display = "none";
	document.getElementById("place2Tr").style.display = "none";
}