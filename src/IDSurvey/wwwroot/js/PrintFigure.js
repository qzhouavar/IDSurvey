(function () {
    var beforePrint = function () {
        
                for (var i = 0; i < AmCharts.charts.length-3; i++) {
                    var chart = AmCharts.charts[i];
                    chart.div.style.width = "1000px";
                    chart.validateNow();
                }
               
            };

            var afterPrint = function () {
                for (var i = 0; i < AmCharts.charts.length; i++) {
                    var chart = AmCharts.charts[i];
                    chart.div.style.width = "100%";
                    chart.validateNow();
                }
            };

            if (window.matchMedia) {
                var mediaQueryList = window.matchMedia('print');
                mediaQueryList.addListener(function (mql) {
                    if (mql.matches) {
                        beforePrint();
                    } else {
                        afterPrint();
                    }
                });
            }

            window.onbeforeprint = beforePrint;
            window.onafterprint = afterPrint();
        }());
