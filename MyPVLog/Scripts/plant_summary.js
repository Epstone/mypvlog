﻿

$(function () {
    var _currentPlantId = $("body").data("plant-id");

    styleUi();

    // load the wattage line chart for the first time
    loadWattageLineChart(new Date());

    //cumulated or inverterwise wattag line chart radio event handlers
    $(".inverter-mode").change(function () {

        var date = getSelectedDate();
        loadWattageLineChart(date);

    });

    /* updates the wattage line chart for the current plant
    takes a timestamp as parameter for the requested date.*/
    function loadWattageLineChart(date) {

        $.getJSON("/WebService/MinuteWiseWattageDay",
                {
                    plantId: _currentPlantId,
                    timeStamp: date.getTime(),
                    mode: $(".inverter-mode:checked").val()
                }, function (serverData) {

                    //draw the chart  and bind to the plot hover event for updating the legend 
                    pvChart.wattageLineChart.plot = pvChart.wattageLineChart.loadWattageChart(serverData);
                    pvChart.wattageLineChart.bindHoverEvent();

                });
    }

    //gauges
    pvChart.loadGoogleChart(["gauge"], function () {

        var callbacks = function(data) {
            dataVisualizer.showGauges(data);
            dataVisualizer.showTitle(data);
        };

        // directly draw gauges at first
        pvChart.loadInverterData(callbacks);

        //Updates the live ticker on a Xsec basis
        setInterval(function () {

            pvChart.loadInverterData(callbacks);

        }, 5000);
    });

    // wattage line chart datepicker
    $(".datepicker").datepicker({
        onSelect: function (dateText) {
            var date = getSelectedDate();
            loadWattageLineChart(date);
        }
    });

    function getSelectedDate() {
        return $(".datepicker").datepicker('getDate');
    }

    function styleUi() {

        $("#toggle-buttons").buttonset().css("margin-right", 20);
        // wattage line chart buttons
        $("#btn-previous").button({
            icons: {
                primary: "ui-icon-triangle-1-w"
            },
            text: false
        }).click(function () {
            $(".datepicker").datepicker("setDate", "c-1d");
            var date = $(".datepicker").datepicker('getDate');
            loadWattageLineChart(date);
        });
        $("#btn-next").button({
            icons: {
                primary: "ui-icon-triangle-1-e"
            },
            text: false
        }).click(function () {
            $(".datepicker").datepicker("setDate", "c+1d")
            var date = $(".datepicker").datepicker('getDate');
            loadWattageLineChart(date);
        });
    }
});