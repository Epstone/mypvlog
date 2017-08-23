/// <reference path="jquery-1.7.1-vsdoc.js" />

var updateLegendTimeout = null;
var latestPosition = null;

var pvChart = {

    options: {

        wattageLineChartOptions: {
            lines: {
                lineWidth: 0.5,
                shadowSize: 0
            },
            xaxis: {
                mode: "time",
                timeformat: "%h:%M Uhr",
                minTickSize: [10, "minute"]

            },
            crosshair: { mode: "x" },
            grid: { hoverable: true, autoHighlight: false },
            legend: {
                backgroundOpacity: 0,
                backgroundColor: null,
                position: "nw"
            }
        },

        wattageGaugeOptions: {
            width: 400, height: 120,
            minorTicks: 5, max: 15000

        },

        temperatureGaugeOptions: {
            width: 400, height: 120,
            redFrom: 90, redTo: 100,
            yellowFrom: 75, yellowTo: 90,
            minorTicks: 5, max: 100
        }
    },

    loadGoogleChart: function (name, callback) {
        google.load('visualization', '1.0', { packages: name, "callback": callback });
    },


    loadGauges: function () {

        $.getJSON("/WebService/GaugeData", { plantId: $("body").data("plant-id") }, function (serverData) {

            // iterate through the given inverter information 
            for (var i in serverData) {

                var inverterInfo = serverData[i];

                //get parent gauge container
                var parentBox = $("#inverter-" + inverterInfo.inverterId);
                var acWattageMax = parentBox.data("ac-wattage");
                pvChart.options.wattageGaugeOptions.max = (acWattageMax == 0) ? 15000 : acWattageMax;

                //get gauge placeholders
                var wattageGaugeDiv = parentBox.children(".wattage");
                var temperatureGaugeDiv = parentBox.children(".temperature");

                //render and update gauges

                pvChart.renderGauges(wattageGaugeDiv, "W",
                                            inverterInfo.wattage,
                                            pvChart.options.wattageGaugeOptions);

                pvChart.renderGauges(temperatureGaugeDiv, "°C",
                                            inverterInfo.temperature,
                                            pvChart.options.temperatureGaugeOptions);

                // update measure time
                parentBox.find(".measure-time").text(inverterInfo.time);
            }

        });
    },

    renderGauges: function (chartDiv, label, value, options) {

        var gaugeData = chartDiv.data("gauge-data");

        // if this is the first time that the gauges are painted
        if (!gaugeData) {

            //Create datatable
            var chartData = new google.visualization.DataTable();
            chartData.addColumn('string', 'Label');
            chartData.addColumn('number', 'Value');

            chartData.addRow([label, value]);

            //create chart
            var chart = new google.visualization.Gauge(chartDiv.get()[0]);

            //save gauge and data for later updates
            chartDiv.data("gauge-data", chartData);
            chartDiv.data("gauge-chart", chart);

            //draw the gauge
            chart.draw(chartData, options);

        } else {

            // just update the gauges
            gaugeData.setValue(0, 1, value);
            chartDiv.data("gauge-chart").draw(gaugeData, options);
        }
    },

    wattageLineChart: {

        legends: {},
        plot: {},

        loadWattageChart: function (serverData) {

            //server response could be either a single object or an array, put it into array if necessary
            var data = $.isArray(serverData) ? serverData : [serverData];

            // iterate through all wattage line chart tables and add the P caption
            for (var i in data) {

                data[i].label += " P = ######"

            }

            var resultWattageChart = $.plot($("#wattage-chart"), data,
                                              pvChart.options.wattageLineChartOptions
                                              );

            //get all plot legends and fix the width
            pvChart.wattageLineChart.legends = $("#wattage-chart .legendLabel");
            $("#wattage-chart .legendLabel").first().each(function () {

                // fix the widths so they don't jump around
                $(this).css('width', $(this).width() + 20);
            });

            return resultWattageChart;
        },

        bindHoverEvent: function () {

            $("#wattage-chart").bind("plothover", function (event, pos, item) {
                latestPosition = pos;

                if (!updateLegendTimeout)
                    updateLegendTimeout = setTimeout(function () {
                        pvChart.wattageLineChart.updateLegend();
                    }, 100);
            });

        },

        updateLegend: function () {
            updateLegendTimeout = null;

            var pos = latestPosition;

            var axes = pvChart.wattageLineChart.plot.getAxes();
            if (pos.x < axes.xaxis.min || pos.x > axes.xaxis.max ||
                  pos.y < axes.yaxis.min || pos.y > axes.yaxis.max)
                return;

            var i, j, dataset = pvChart.wattageLineChart.plot.getData();
            for (i = 0; i < dataset.length; ++i) {
                var series = dataset[i];

                // find the nearest points, x-wise
                for (j = 0; j < series.data.length; ++j)
                    if (series.data[j][0] > pos.x)
                        break;

                //set wattage value (y)
                if (series.data[j] !== undefined) {
                    y = series.data[j][1];

                    pvChart.wattageLineChart.legends.eq(i).text(series.label.replace(/=.*/, "= " + y.toFixed(0) + " W"));
                }
            }
        }
    },
    roiChart: {
        chartFractionDigits: 2,
        dataTable: {},
        cumulatedTable: {},
        currentSuffix: "kwh",
        currentViewMode: "cumulated",
        tablePlaceholder: {},
        columnChartPlaceholder: {},


        /* setup the target placeholders for the column chart and the table chart */
        setup: function (chartSelector, tableSelector) {
            pvChart.roiChart.columnChartPlaceholder = $(chartSelector);
            pvChart.roiChart.tablePlaceholder = $(tableSelector);
        },

        /* creates two google data tables by the server generated table content. The detailed 
        table contains all inverter data seperated. One column for each inverter. The cumulated
        view summarizes all inverter into one summarized column*/
        initializeGoogleData: function (tableContent, suffix) {

            //set kwh or € as suffix
            pvChart.roiChart.currentSuffix = suffix;

            //create datatable
            pvChart.roiChart.dataTable = new google.visualization.DataTable(tableContent, 0.5);

            //create cumulated view
            var view = new google.visualization.DataView(pvChart.roiChart.dataTable);
            view.setColumns([0, { calc: cumulate, type: 'number', label: 'Gesamt' }]);
            pvChart.roiChart.cumulatedTable = view.toDataTable(); // new google.visualization.DataTable(view.toJSON());

            //cumulate all column values
            function cumulate(dataTable, rowNum) {

                var cumulated = 0.0;
                for (var i = 1; i < dataTable.getNumberOfColumns() ; i++) {
                    cumulated += dataTable.getValue(rowNum, i);
                }

                return cumulated;
            }

        },

        /* renders the detailed view where all inverters are seperately listed */
        renderDetailedView: function () {
            pvChart.roiChart.renderColumnChart(pvChart.roiChart.dataTable);
            pvChart.roiChart.renderTable(pvChart.roiChart.dataTable);
            pvChart.roiChart.currentViewMode = "detailed";
        },

        /* renders the cumulated view where all inverters are summarized into one */
        renderCumulatedView: function () {
            pvChart.roiChart.renderColumnChart(pvChart.roiChart.cumulatedTable);
            pvChart.roiChart.renderTable(pvChart.roiChart.cumulatedTable);
            pvChart.roiChart.currentViewMode = "cumulated";
        },

        /* renders the column chart by the given google data table or data view */
        renderColumnChart: function (googleData) {

            //format the chart data
            pvChart.googleChartUtils.formatColumns(googleData,
                                                          pvChart.roiChart.chartFractionDigits,
                                                          pvChart.roiChart.currentSuffix);

            var chart = new google.visualization.ColumnChart(pvChart.roiChart.columnChartPlaceholder.get()[0]);

            chart.draw(googleData, pvChart.roiChart.options);

        },

        /* renders a new google table chart by the given data table */
        renderTable: function (data) {

            //render kwh table
            var visualization = new google.visualization.Table(pvChart.roiChart.tablePlaceholder.get()[0])

            //draw inverted table
            var invertedTable = pvChart.googleChartUtils.invertTable(data, pvChart.roiChart.currentSuffix);

            //reformat data before drawing the table
            pvChart.googleChartUtils.formatColumns(invertedTable, 1, "");
            visualization.draw(invertedTable, pvChart.roiChart.tableOptions);

        },

        /* Updates all charts by a given google data table content and a suffix (€ or kwh) */
        updateView: function (dataTableContent, suffix) {

            //reinitialize the google data tables
            pvChart.roiChart.initializeGoogleData(dataTableContent, suffix);

            // redraw the charts based on the latest view mode
            if (pvChart.roiChart.currentViewMode === "cumulated")
                pvChart.roiChart.renderCumulatedView();
            else
                pvChart.roiChart.renderDetailedView();
        },

        /* column charts options */
        options: {
            width: '100%', height: 400, chartArea: { width: '100%', left: 50, top: 20 }
        },

        tableOptions: { sort: 'disable' }
    },

    /* Utility functions for google charts tools */
    googleChartUtils: {

        /*creates an inverted table where the rows of the original table 
        are transformed to columns of the new table */
        invertTable: function (dt, dataType) {

            var invertedTable = new google.visualization.DataTable();

            var colCount = dt.getNumberOfRows();
            var rowCount = dt.getNumberOfColumns();

            //create label column
            invertedTable.addColumn('string', dataType);

            //create value columns
            for (var i = 1; i <= colCount; i++) {
                invertedTable.addColumn('number', dt.getValue(i - 1, 0));
            }

            //insert data
            for (var i = 1; i < rowCount; i++) {
                // add a new row for each inverter
                invertedTable.addRow();
                invertedTable.setValue(i - 1, 0, dt.getColumnLabel(i));

                for (var j = 1; j <= colCount; j++) {
                    invertedTable.setValue(i - 1, j, dt.getValue(j - 1, i));
                }
            }

            //format the inverted table
            pvChart.googleChartUtils.formatColumns(invertedTable, pvChart.roiChart.fractionDigits);

            return invertedTable;
        },

        /* formats all table columns typeof 'number' by a given frationDigits parameter */
        formatColumns: function (dataTable, fractionDigits, suffix) {

            //format all columns
            var formatter = new google.visualization.NumberFormat({
                fractionDigits: fractionDigits,
                suffix: suffix
            });

            for (var i = 0; i < dataTable.getNumberOfColumns() ; i++) {

                if (dataTable.getColumnType(i) === 'number')
                    formatter.format(dataTable, i);
            }
        }
    }

};