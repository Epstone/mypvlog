/// <reference path="jquery-1.5-vsdoc.js" />
//Global User Controls and formatting
var isROILoaded = false;
var isWattageLoaded = false;
$(function () {
    $(".btn_previous").button({ icons: { primary: "ui-icon-triangle-1-w"} });
    $(".btn_next").button({ icons: { secondary: "ui-icon-triangle-1-e"} });
    $(".outerbox h2").click(function () {
        $(this).next(".inner_box").toggle(function () {
            if ($(this).is(':visible')) {
                if ($(this).hasClass("has_roi_chart") && !isROILoaded) {
                    __loadGoogleChart('corechart', __initROICharts);
                } else if ($(this).hasClass("has_wattage_chart") && !isWattageLoaded) {
                    __loadGoogleChart('annotatedtimeline', __initWattageChart);
                }
                $(document).scrollTop($(this).position().top - 50);
            }
        }
);
    });
});

function __initROICharts() {
    __updateROIDayGraph();
    __updateROIMonthGraph(new Date());
    __updateROIYearGraph();
    __updateROIDecadeGraph();
    isROILoaded = true;
}

function __initWattageChart() {
    __refreshIntraDayGraph(new Date(), new Date());
    isWattageLoaded = true;
}

// Live Status Ticker and Gauges----------------------------------------------------------------------------------
$(function () {

    // Auto refresh button code
    $("#cbx_auto_refresh").button();

    //Load the UI for the first time
    __loadLiveUI();

    //Auto refresh mehtod
    var refreshId = setInterval(function () {
        if ($("#cbx_auto_refresh").is(":checked")) {
            __loadLiveUI();
        }
    }, 5000);

});

// Asks the server for the live ticker data
function __loadLiveUI() {
    $("#img_refresh").css("visibility", "visible");
    $.get("/Statistics/GetStatusTable", { plantID: $.PlantID }, function (html) {
        $("#status_table").html(html);
        $("#img_refresh").css("visibility", "hidden");
    });

//    __askServer("WebService/GetData.aspx/GetCurrentOutputWattage", function (serverData) {
//        __createLiveTickerGauge(serverData.d);
//    }, {});
}

//var gaugeData;
//var gaugeOptions = { min: 0, max: 5000, yellowFrom: 1000, yellowTo: 4000,
//    greenFrom: 4000, greenTo: 5000, minorTicks: 5
//};
//var gauge;

// Callback method for drawing the live ticker gauges
function __createLiveTickerGauge(statusData) {
    if (gaugeData == undefined) {

        gaugeData = new google.visualization.DataTable();
        for (var i in statusData) {
            gaugeData.addColumn('number', 'Inv ' + statusData[i].InverterID + ' (W)');
        }

        gaugeData.addRows(statusData.length);

        //set gauge values, initialize and draw
        __updateGaugeData(gaugeData, statusData)
        gauge = new google.visualization.Gauge(document.getElementById('gauge_div'));
        gauge.draw(gaugeData, gaugeOptions);
    } else {
        // Update values and draw
        __updateGaugeData(gaugeData, statusData)
        gauge.draw(gaugeData, gaugeOptions);
    }
}

//Sets or updates the values of the google data object for the gauges
function __updateGaugeData(gaugeData, statusData) {
    for (var i in statusData) {
        gaugeData.setValue(0, statusData[i].InverterID - 1, statusData[i].OutputWattage);
    }
}

// Wattage  Line Chart------------------------------------------------------------------------------------
$(function () {

    //User Controls
    $("#btn_previousWattage").click(function () {
        $("#date_from, #date_to").datepicker("setDate", "c-1d");
        __onSelectedDate();
        return false;
    });

    $("#btn_nextWattage").click(function () {
        $("#date_from, #date_to").datepicker("setDate", "c+1d");
        __onSelectedDate();
        return false;
    });

    //Initialization
    $("#date_from, #date_to").datepicker(
    {
        // Eventhandler for Datepicking event
        onSelect: __onSelectedDate,
        showButtonPanel: true,
        changeMonth: true,
        changeYear: true,
        defaultDate: null
    }
    //Localization
    ).datepicker($.datepicker.regional['de'])
    // Set initial time
   .datepicker("setDate", new Date());

    //Load IntraDay graph for the first time with today's date
    //__refreshIntraDayGraph(new Date(), new Date());
});

function __onSelectedDate() {
    var selectedFromDate = $("#date_from").datepicker("getDate");
    var selectedToDate = $("#date_to").datepicker("getDate");
    if ((selectedFromDate != undefined) && (selectedToDate != undefined))
        __refreshIntraDayGraph(selectedFromDate, selectedToDate);
}

// Asks the server for the OutputWattage data for the line chart. The user defines the timespan for the included data. 
function __refreshIntraDayGraph(fromDate, toDate) {
    var params = {};
    params.startDay = fromDate.getDate();
    params.startMonth = fromDate.getMonth() + 1;
    params.startYear = fromDate.getFullYear();
    params.endDay = toDate.getDate();
    params.endMonth = toDate.getMonth() + 1;
    params.endYear = toDate.getFullYear();
    __askServer("WebService/GetData.aspx/GetDataByDay", __drawIntraDay, params);
}

// Callback method for drawing the OutputWattage Line Chart
function __drawIntraDay(serverData) {

    var data = __convertToGoogleDataTable(serverData);
    var chart = new google.visualization.AnnotatedTimeLine(document.getElementById('wattage_chart'));
    chart.draw(data, { displayAnnotations: false
                    , wmode: 'opaque'
                    , displayRangeSelector: false
                    , fill: 10
                    , max: 5000
    });
}

// ROI Day Graph ---------------------------------------------------------------------------
$(function () {

    //Read the user selected modes
    $("#tbx_roi_day").datepicker({ onSelect: __updateROIDayGraph })
    //localize
    .datepicker($.datepicker.regional['de'])
    // Set initial time
   .datepicker("setDate", new Date()); ;
    $("#btns_day").buttonset().change(__updateROIDayGraph);
    $("#btn_roi_day_prev").click(function () { $("#tbx_roi_day").datepicker("setDate", "c-1d"); __updateROIDayGraph(); return false });
    $("#btn_roi_day_next").click(function () { $("#tbx_roi_day").datepicker("setDate", "c+1d"); __updateROIDayGraph(); return false });
    //__updateROIDayGraph();
});


function __updateROIDayGraph() {
    yMode = $("input[name='roi_day_yMode']:checked").val();
    var selectedDate = $("#tbx_roi_day").datepicker("getDate");
    var params = __createRoiParams("hour", yMode, selectedDate);

    __callRoiPageMethod(params, "graph_roi_day");
    return false;
}


// ROI Month Graph ---------------------------------------------------------------------------
$(function () {

    $("#btns_month").buttonset().change(__updateROIYearGraph);
    $("#monthpicker").monthpicker(__monthpickerCallback);
    /*  //Read the user selected modes
    $("#tbx_roi_month").datepicker({ onSelect: __updateROIMonthGraph })
    //localize
    .datepicker($.datepicker.regional['de'])
    // Set initial time
    .datepicker("setDate", new Date()); */

    $("#btns_month").buttonset().change(function () { __updateROIMonthGraph(lastDate); });
    //__updateROIMonthGraph(new Date());
});

var lastDate = new Date();
function __monthpickerCallback(data, $e) {

    selectedDate = new Date(parseInt(data.year), parseInt(data.month) - 1, 1);
    lastDate = selectedDate;
    __updateROIMonthGraph(selectedDate);
}

function __updateROIMonthGraph(selectedDate) {
    yMode = $("input[name='roi_month_yMode']:checked").val();
    var params = __createRoiParams("day", yMode, selectedDate);

    __callRoiPageMethod(params, "graph_roi_month");

    return false;
}

// ROI Year Graph ---------------------------------------------------------------------------
$(function () {

    //Add the years to the listbox
    for (var i = 2011; i < 2030; i++) {
        $("#yearSelect").
          append($("<option></option>").
          attr("value", i.toString()).
          text(i.toString()));
    }
    //select the current year
    var currentYear = new Date().getFullYear().toString()
    jQuery("#yearSelect " + currentYear).attr('selected', 'selected');

    $("#btns_year").buttonset().change(__updateROIYearGraph);
    jQuery("#yearSelect").change(__updateROIYearGraph);

    //draw the graph for the first time
    // __updateROIYearGraph();
});


function __updateROIYearGraph() {
    yMode = $("input[name='roi_year_yMode']:checked").val();
    var selectedYear = parseInt($('#yearSelect option:selected').text());
    var selectedDate = new Date(new Date().setYear(selectedYear));
    var params = __createRoiParams("month", yMode, selectedDate);

    __callRoiPageMethod(params, "graph_roi_year");
    return false;
}


// ROI Decade Graph ---------------------------------------------------------------------------
$(function () {
    $("#btns_decade").buttonset().change(__updateROIDecadeGraph);

    //__updateROIDecadeGraph();
});

function __updateROIDecadeGraph() {
    yMode = $("input[name='roi_decade_yMode']:checked").val();
    var params = __createRoiParams("year", yMode, new Date());

    __callRoiPageMethod(params, "graph_roi_decade");

    return false;
}

function __drawRoiGraph(serverData, placeHolder) {
    var myData = __convertToGoogleDataTable(serverData);

    //format the data and draw the chart
    myData = __formatData(myData, yMode)
    __drawChart(myData, placeHolder);
}

// Utils------------------------------------------------------------------------------------
function __createRoiParams(xMode, yMode, date) {
    //    date = __getDateAsParameterString(date, "day", "month", "year");
    //    var params = date
    //                + ", 'yMode':'" + yMode + "'"
    //                + ", 'xMode':'" + xMode + "'";

    var params = {};
    params.day = date.getDate();
    params.month = date.getMonth() + 1;
    params.year = date.getFullYear();
    params.xMode = xMode;
    params.yMode = yMode;
    return params;
}

function __callRoiPageMethod(params, placeHolder) {
    __askServer("WebService/GetData.aspx/GetRateOfReturn", function (serverData) {
        __drawRoiGraph(serverData, placeHolder);
    }, params);

    //    $().runParaServerMethod("Default.aspx/GetRateOfReturn", function (serverData) {
    //        __drawRoiGraph(serverData, placeHolder);
    //    }, params);
}

function __drawChart(myData, placeholder) {
    var chart = new google.visualization.ColumnChart(document.getElementById(placeholder));
    chart.draw(myData,
           {
               width: 1200
               , height: 500
               , chartArea: { left: 50, top: 20 }

           }
      );
}

function __formatData(myData, yMode) {
    var formatter;
    if (yMode == "money")
        formatter = new google.visualization.NumberFormat({ suffix: '€' });
    else if (yMode == "kwh")
        formatter = new google.visualization.NumberFormat({ suffix: 'kWh', fractionDigits: 1 });

    // Apply formatter all 4 inverter columns
    formatter.format(myData, 1);
    formatter.format(myData, 2);
    formatter.format(myData, 3);
    formatter.format(myData, 4);

    return myData;
}

//Converts the serverdata with a string representation of a gdata Table into a gdata table object
function __convertToGoogleDataTable(serverData) {
    var temp;
    eval("temp = " + serverData.d);
    return new google.visualization.DataTable(temp, 0.5);

}

//Converts a given date object into a json string TODO
function __getDateAsParameterString(currentTime, dayTerm, monthTerm, yearTerm) {
    var month = currentTime.getMonth() + 1;
    var day = currentTime.getDate();
    var year = currentTime.getFullYear();
    var parameters = "'" + dayTerm + "':'" + day + "',";
    parameters += "'" + monthTerm + "':'" + month + "',";
    parameters += "'" + yearTerm + "':'" + year + "'";

    return parameters;
}

//Converts the strange asp.net DateTime format (Ticks string) into an js DateTime object
function __convertToDateObj(ticksString) {
    return eval('new' + ticksString.replace(/\//g, ' '));
}

function __askServer(url, callback, parameters) {
    parameters.plantID = $.PlantID;
    var myParams = $.toJSON(parameters);
    //$().runParaServerMethod(url, callback, myParams);
}

function __loadGoogleChart(name, callback) {
    google.load('visualization', '1.0', { packages: [name], "callback": callback });
}