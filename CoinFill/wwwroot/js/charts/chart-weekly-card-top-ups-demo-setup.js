"use strict";

var _this = this;

function ownKeys(object, enumerableOnly) {
  var keys = Object.keys(object);
  if (Object.getOwnPropertySymbols) {
    var symbols = Object.getOwnPropertySymbols(object);
    if (enumerableOnly)
      symbols = symbols.filter(function (sym) {
        return Object.getOwnPropertyDescriptor(object, sym).enumerable;
      });
    keys.push.apply(keys, symbols);
  }
  return keys;
}

function _objectSpread(target) {
  for (var i = 1; i < arguments.length; i++) {
    var source = arguments[i] != null ? arguments[i] : {};
    if (i % 2) {
      ownKeys(Object(source), true).forEach(function (key) {
        _defineProperty(target, key, source[key]);
      });
    } else if (Object.getOwnPropertyDescriptors) {
      Object.defineProperties(target, Object.getOwnPropertyDescriptors(source));
    } else {
      ownKeys(Object(source)).forEach(function (key) {
        Object.defineProperty(
          target,
          key,
          Object.getOwnPropertyDescriptor(source, key)
        );
      });
    }
  }
  return target;
}

function _defineProperty(obj, key, value) {
  if (key in obj) {
    Object.defineProperty(obj, key, {
      value: value,
      enumerable: true,
      configurable: true,
      writable: true,
    });
  } else {
    obj[key] = value;
  }
  return obj;
}

/*-----------------------------------------------
|   Theme Configuration
-----------------------------------------------*/
var storage = {
  isDark: false,
};
/*-----------------------------------------------
|   Utilities
-----------------------------------------------*/

var utils = (function ($) {
  var grays = function grays() {
    var colors = {
      white: "#fff",
      100: "#f9fafd",
      200: "#edf2f9",
      300: "#d8e2ef",
      400: "#b6c1d2",
      500: "#9da9bb",
      600: "#748194",
      700: "#5e6e82",
      800: "#4d5969",
      900: "#344050",
      1000: "#232e3c",
      1100: "#0b1727",
      black: "#000",
    };

    if (storage.isDark) {
      colors = {
        white: "#0e1c2f",
        100: "#132238",
        200: "#061325",
        300: "#344050",
        400: "#4d5969",
        500: "#5e6e82",
        600: "#748194",
        700: "#9da9bb",
        800: "#b6c1d2",
        900: "#d8e2ef",
        1000: "#edf2f9",
        1100: "#f9fafd",
        black: "#fff",
      };
    }

    return colors;
  };

  var themeColors = function themeColors() {
    var colors = {
      primary: "#2ba0ff",
      secondary: "#748194",
      success: "#00d27a",
      info: "#27bcfd",
      warning: "#f5803e",
      danger: "#e63757",
      light: "#f9fafd",
      dark: "#0b1727",
      orange: "#f7931a",
      pink: "#e6007a",
      gray: '#b6c1d2'
    };

    return colors;
  };

  var Utils = {
    $window: $(window),
    $document: $(document),
    $html: $("html"),
    $body: $("body"),
    $main: $("main"),
    isRTL: function isRTL() {
      return this.$html.attr("dir") === "rtl";
    },
    location: window.location,
    nua: navigator.userAgent,
    breakpoints: {
      xs: 0,
      sm: 576,
      md: 768,
      lg: 992,
      xl: 1200,
      xxl: 1540,
    },
    colors: themeColors(),
    grays: grays(),
  };
  return Utils;
})(jQuery);
/*-----------------------------------------------
|   Chart
-----------------------------------------------*/

utils.$document.ready(function () {
  /*-----------------------------------------------
    |   Helper functions and Data
    -----------------------------------------------*/
  var chartData = [
    3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5, 8, 9, 7, 9, 3, 2, 3, 8, 4, 6, 2, 6, 4, 3,
    3, 8, 3, 2, 7, 9, 5, 0, 2, 8, 8, 4, 1, 9, 7,
  ];
  var labels = [
    "9:00 AM",
    "10:00 AM",
    "11:00 AM",
    "12:00 PM",
    "1:00 PM",
    "2:00 PM",
    "3:00 PM",
    "4:00 PM",
    "5:00 PM",
    "6:00 PM",
    "7:00 PM",
    "8:00 PM",
  ];
  /*-----------------------------------------------
    |   Chart Initialization
    -----------------------------------------------*/

  var newChart = function newChart(chart, config) {
    var ctx = chart.getContext("2d");
    return new window.Chart(ctx, config);
  };
  /*-----------------------------------------------
    |   Line Chart
    -----------------------------------------------*/

  var chartLine = document.getElementById("chart-line");

  if (chartLine) {
    var getChartBackground = function getChartBackground(chart) {
      var ctx = chart.getContext("2d");

      if (storage.isDark) {
        var _gradientFill = ctx.createLinearGradient(
          0,
          0,
          0,
          ctx.canvas.height
        );

        _gradientFill.addColorStop(
          0,
          utils.rgbaColor(utils.colors.primary, 0.5)
        );

        _gradientFill.addColorStop(1, "transparent");

        return _gradientFill;
      }

      var gradientFill = ctx.createLinearGradient(0, 0, 0, 250);
      gradientFill.addColorStop(0, "rgba(255, 255, 255, 0.3)");
      gradientFill.addColorStop(1, "rgba(255, 255, 255, 0)");
      return gradientFill;
    };

    var dashboardLineChart = newChart(chartLine, {
      type: "line",
      data: {
        labels: labels.map(function (label) {
          return label.substring(0, label.length - 3);
        }),
        datasets: [
          {
            borderWidth: 2,
            data: chartData.map(function (d) {
              return (d * 3.14).toFixed(2);
            }),
            borderColor: utils.settings.chart.borderColor,
            backgroundColor: getChartBackground(chartLine),
          },
        ],
      },
      options: {
        legend: {
          display: false,
        },
        tooltips: {
          mode: "x-axis",
          xPadding: 20,
          yPadding: 10,
          displayColors: false,
          callbacks: {
            label: function label(tooltipItem) {
              return (
                labels[tooltipItem.index] + " - " + tooltipItem.yLabel + " USD"
              );
            },
            title: function title() {
              return null;
            },
          },
        },
        hover: {
          mode: "label",
        },
        scales: {
          xAxes: [
            {
              scaleLabel: {
                show: true,
                labelString: "Month",
              },
              ticks: {
                fontColor: utils.rgbaColor("#fff", 0.7),
                fontStyle: 600,
              },
              gridLines: {
                color: utils.rgbaColor("#fff", 0.1),
                zeroLineColor: utils.rgbaColor("#fff", 0.1),
                lineWidth: 1,
              },
            },
          ],
          yAxes: [
            {
              display: false,
            },
          ],
        },
      },
    });
    $("#dashboard-chart-select").on("change", function (e) {
      var LineDB = {
        all: [4, 1, 6, 2, 7, 12, 4, 6, 5, 4, 5, 10].map(function (d) {
          return (d * 3.14).toFixed(2);
        }),
        successful: [3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5, 8].map(function (d) {
          return (d * 3.14).toFixed(2);
        }),
        failed: [1, 0, 2, 1, 2, 1, 1, 0, 0, 1, 0, 2].map(function (d) {
          return (d * 3.14).toFixed(2);
        }),
      };
      dashboardLineChart.data.datasets[0].data = LineDB[e.target.value];
      dashboardLineChart.update();
    });
  }
  /*-----------------------------------------------
    |   Bar Chart
    -----------------------------------------------*/

  var chartBar = document.getElementById("chart-bar");

  if (chartBar) {
    newChart(chartBar, {
      type: "bar",
      data: {
        labels: labels.slice(0, 2),
        datasets: [
          {
            label: "First dataset",
            backgroundColor: [
              utils.rgbaColor(utils.colors.info),
              utils.rgbaColor(utils.colors.warning),
            ],
            borderColor: [
              utils.rgbColor(utils.colors.info),
              utils.rgbColor(utils.colors.warning),
            ],
            borderWidth: 2,
            data: [6, 10],
          },
          {
            label: "Second dataset",
            backgroundColor: [
              utils.rgbaColor(utils.colors.success),
              utils.rgbaColor(utils.colors.danger),
            ],
            borderColor: [
              utils.rgbColor(utils.colors.success),
              utils.rgbColor(utils.colors.danger),
            ],
            borderWidth: 2,
            data: [3, 7],
          },
        ],
      },
      options: {
        scales: {
          yAxes: [
            {
              ticks: {
                beginAtZero: true,
              },
            },
          ],
        },
      },
    });
  }
  /*-----------------------------------------------
    |   Radar Chart
    -----------------------------------------------*/

  var chartRadar = document.getElementById("chart-radar");

  if (chartRadar) {
    newChart(chartRadar, {
      type: "radar",
      data: {
        labels: labels,
        datasets: [
          {
            label: "First dataset",
            backgroundColor: utils.rgbaColor(utils.colors.warning),
            borderColor: utils.rgbColor(utils.colors.warning),
            borderWidth: 2,
            data: chartData.slice(0, 12),
            fill: 1,
          },
          {
            label: "Second dataset",
            backgroundColor: utils.rgbaColor(utils.colors.danger),
            borderColor: utils.rgbColor(utils.colors.danger),
            borderWidth: 2,
            data: chartData.slice(12, 24),
            fill: 1,
          },
        ],
      },
      options: {
        maintainAspectRatio: true,
        spanGaps: false,
        elements: {
          line: {
            tension: 0.000001,
          },
        },
      },
    });
  }
  /*-----------------------------------------------
    |   Doughnut Chart
    -----------------------------------------------*/

  var chartDoughnut = document.getElementById("chart-doughnut");

  if (chartDoughnut) {
    newChart(chartDoughnut, {
      type: "doughnut",
      data: {
        labels: labels.slice(0, 3),
        datasets: [
          {
            backgroundColor: utils.rgbColors(),
            borderColor: utils.rgbColors(),
            data: chartData.slice(0, 3),
          },
        ],
      },
      options: {
        responsive: true,
      },
    });
  }
});
/*-----------------------------------------------
|  Echarts
-----------------------------------------------*/

var getPosition = function getPosition(pos, params, dom, rect, size) {
  return {
    top: pos[1] - size.contentSize[1] - 10,
    left: pos[0] - size.contentSize[0] / 2,
  };
};

utils.$document.ready(function () {
  var Events = {
    CHANGE: "change",
  };
    var Selector = {
    ECHART_BAR_WEEKLY_CARD_TOP_UPS: ".echart-bar-weekly-card-top-ups",
    ECHART_LINE_WEEKLY_CARD_SPENDING: ".echart-line-weekly-card-spending",
    ECHART_LINE_TOTAL_CARDS_BALANCE: ".echart-line-total-cards-balance",
    SELECT_MONTH: ".select-month",
    ECHART_BAR_TOP_PRODUCTS: ".echart-bar-top-products",
    ECHART_WORLD_MAP: ".echart-world-map",
    ECHART_DOUGHNUT: ".echart-doughnut",
  }; //
  // ─── TOTAL ORDER CHART ──────────────────────────────────────────────────────────
  //

    let num11 = 242;
    let num22 = 279;
    let num33 = 167;
    let num44 = 406;
    let num55 = 213;
    let num66 = 179;
    let num77 = 320;
    let totalWeeklySpendings =
        num11 + num22 + num33 + num44 + num55 + num66 + num77;
    $("#total-weekly-spending").text(
        "$" + (totalWeeklySpendings / 1000).toFixed(1) + "K"
    );

  var $echartLineWeeklyCardSpending = document.querySelector(
    Selector.ECHART_LINE_WEEKLY_CARD_SPENDING
  );

    if ($echartLineWeeklyCardSpending) {
        var _$this = $($echartLineWeeklyCardSpending);

        var _userOptions = _$this.data("options");

        var data = [num11, num22, num33, num44, num55, num66, num77];

        var yMax = Math.max.apply(Math, data);
        var dataBackground = data.map(function () {
            return yMax;
        });

        var _chart = window.echarts.init($echartLineWeeklyCardSpending);

        var _defaultOptions = {
            tooltip: {
                trigger: "axis",
                padding: [7, 10],
                formatter: "{b1}: {c1}",
                backgroundColor: utils.grays.white,
                borderColor: utils.grays["300"],
                borderWidth: 1,
                textStyle: {
                    color: utils.colors.dark,
                },
                transitionDuration: 0,
                position: function position(pos, params, dom, rect, size) {
                    return getPosition(pos, params, dom, rect, size);
                },
            },
            xAxis: {
                type: "category",
                data: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
                boundaryGap: false,
                axisLine: {
                    show: false,
                },
                axisLabel: {
                    show: false,
                },
                axisTick: {
                    show: false,
                },
                axisPointer: {
                    type: "none",
                },
            },
            yAxis: {
                type: "value",
                splitLine: {
                    show: false,
                },
                axisLine: {
                    show: false,
                },
                axisLabel: {
                    show: false,
                },
                axisTick: {
                    show: false,
                },
                axisPointer: {
                    type: "none",
                },
            },
            series: [
                {
                    type: "bar",
                    barWidth: "5px",
                    barGap: "-100%",
                    itemStyle: {
                        color: utils.grays["200"],
                        barBorderRadius: 10,
                    },
                    data: dataBackground,
                    animation: true,
                    emphasis: {
                        itemStyle: {
                            color: utils.grays["200"],
                        },
                    },
                },
                {
                    type: "bar",
                    barWidth: "5px",
                    itemStyle: {
                        color: '#e62e52',
                        barBorderRadius: 10,
                    },
                    data: data,
                    emphasis: {
                        itemStyle: {
                            color: '#e62e52',
                        },
                    },
                    z: 10,
                },
            ],
            grid: {
                right: 5,
                left: 10,
                top: 0,
                bottom: 0,
            },
        }; // Merge user options with lodash

        var _options = window._.merge(_defaultOptions, _userOptions);

        _chart.setOption(_options);
    } //
  // ─── WEEKLY SALES CHART ─────────────────────────────────────────────────────────
  //

  let num111 = 347 * 2;
  let num222 = 696 * 2;
  let num333 = 423 * 2;
  let num444 = 284 * 2;
  let num555 = 390 * 2;
  let num666 = 116 * 2;
  let num777 = 472 * 2;
  let totalWeeklyTopUps =
    num111 + num222 + num333 + num444 + num555 + num666 + num777;
  $("#total-weekly-top-ups").text(
      "$" + (totalWeeklyTopUps / 1000).toFixed(1) + "K"
  );

  var $echartBarWeeklyCardTopUps = document.querySelector(
    Selector.ECHART_BAR_WEEKLY_CARD_TOP_UPS
  );

  if ($echartBarWeeklyCardTopUps) {
    var _$this = $($echartBarWeeklyCardTopUps);

    var _userOptions = _$this.data("options");

    var data = [num111, num222, num333, num444, num555, num666, num777];

    var yMax = Math.max.apply(Math, data);
    var dataBackground = data.map(function () {
      return yMax;
    });

    var _chart = window.echarts.init($echartBarWeeklyCardTopUps);

    var _defaultOptions = {
      tooltip: {
        trigger: "axis",
        padding: [7, 10],
        formatter: "{b1}: {c1}",
        backgroundColor: utils.grays.white,
        borderColor: utils.grays["300"],
        borderWidth: 1,
        textStyle: {
          color: utils.colors.dark,
        },
        transitionDuration: 0,
        position: function position(pos, params, dom, rect, size) {
          return getPosition(pos, params, dom, rect, size);
        },
      },
      xAxis: {
        type: "category",
        data: ["Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat"],
        boundaryGap: false,
        axisLine: {
          show: false,
        },
        axisLabel: {
          show: false,
        },
        axisTick: {
          show: false,
        },
        axisPointer: {
          type: "none",
        },
      },
      yAxis: {
        type: "value",
        splitLine: {
          show: false,
        },
        axisLine: {
          show: false,
        },
        axisLabel: {
          show: false,
        },
        axisTick: {
          show: false,
        },
        axisPointer: {
          type: "none",
        },
      },
      series: [
        {
          type: "bar",
          barWidth: "5px",
          barGap: "-100%",
          itemStyle: {
            color: utils.grays["200"],
            barBorderRadius: 10,
          },
          data: dataBackground,
          animation: true,
          emphasis: {
            itemStyle: {
              color: utils.grays["200"],
            },
          },
        },
        {
          type: "bar",
          barWidth: "5px",
          itemStyle: {
            color: utils.colors.primary,
            barBorderRadius: 10,
          },
          data: data,
          emphasis: {
            itemStyle: {
              color: utils.colors.primary,
            },
          },
          z: 10,
        },
      ],
      grid: {
        right: 5,
        left: 10,
        top: 0,
        bottom: 0,
      },
    }; // Merge user options with lodash

    var _options = window._.merge(_defaultOptions, _userOptions);

    _chart.setOption(_options);
  } //
  // ─── EHCART LINE TOTAL SALES ────────────────────────────────────────────────────────────────
  //

  var $echartsLineTotalCardsBalance = document.querySelector(
    Selector.ECHART_LINE_TOTAL_CARDS_BALANCE
  );

    if ($echartsLineTotalCardsBalance) {
        var $this = $($echartsLineTotalCardsBalance); // Get options from data attribute

        var userOptions = $this.data("options");
        var chart = window.echarts.init($echartsLineTotalCardsBalance); // Default options

        var defaultOptions = {
            tooltip: {
                triggerOn: "mousemove",
                trigger: "axis",
                padding: [7, 10],
                formatter: "{b0}: {c0}",
                backgroundColor: utils.grays.white,
                borderColor: utils.grays["300"],
                borderWidth: 1,
                transitionDuration: 0,
                position: function position(pos, params, dom, rect, size) {
                    return getPosition(pos, params, dom, rect, size);
                },
                textStyle: {
                    color: utils.colors.dark,
                },
            },
            xAxis: {
                type: "category",
                data: ["3 weeks ago", "2 weeks ago", "A week ago"],
                boundaryGap: false,
                splitLine: {
                    show: false,
                },
                axisLine: {
                    show: false,
                    lineStyle: {
                        color: utils.grays["300"],
                        type: "dashed",
                    },
                },
                axisLabel: {
                    show: false,
                },
                axisTick: {
                    show: false,
                },
                axisPointer: {
                    type: "none",
                },
            },
            yAxis: {
                type: "value",
                splitLine: {
                    show: false,
                },
                axisLine: {
                    show: false,
                },
                axisLabel: {
                    show: false,
                },
                axisTick: {
                    show: false,
                },
                axisPointer: {
                    show: false,
                },
            },
            series: [
                {
                    type: "line",
                    lineStyle: {
                        color: utils.colors.primary,
                        width: 3,
                    },
                    itemStyle: {
                        color: utils.grays.white,
                        borderColor: utils.colors.primary,
                        borderWidth: 2,
                    },
                    hoverAnimation: true,
                    data: [6870, 25060, 40050],
                    connectNulls: true,
                    smooth: 0.6,
                    smoothMonotone: "x",
                    symbol: "circle",
                    symbolSize: 8,
                    areaStyle: {
                        color: {
                            type: "linear",
                            x: 0,
                            y: 0,
                            x2: 0,
                            y2: 1,
                            colorStops: [
                                {
                                    offset: 0,
                                    color: utils.rgbaColor(utils.colors.primary, 0.25),
                                },
                                {
                                    offset: 1,
                                    color: utils.rgbaColor(utils.colors.primary, 0),
                                },
                            ],
                        },
                    },
                },
            ],
            grid: {
                bottom: "2%",
                top: "0%",
                right: "10px",
                left: "10px",
            },
        }; // Merge options using lodash

        var options = window._.merge(defaultOptions, userOptions);

        chart.setOption(options);
    }
 
  // ─── ECHART FIX ON WINDOW RESIZE ────────────────────────────────────────────────
  //

  var $echarts = document.querySelectorAll("[data-echart-responsive]");

  window.onresize = function () {
    if ($echarts.length) {
      $.each($echarts, function (item, value) {
        if ($(value).data("echart-responsive")) {
          window.echarts.init(value).resize();
        }
      });
    }
  };
});
