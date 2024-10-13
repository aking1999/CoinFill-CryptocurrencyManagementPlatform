"use strict";

var _this = this;

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

    return colors;
  };

  var themeColors = function themeColors() {
    var colors = {
      primary: "#2c7be5",
      secondary: "#748194",
      success: "#00d27a",
      info: "#27bcfd",
      warning: "#f5803e",
      danger: "#e63757",
      light: "#f9fafd",
      dark: "#0b1727",
    };

    return colors;
  };

  var pluginSettings = function pluginSettings() {
    var settings = {
      tinymce: {
        theme: "oxide",
      },
      chart: {
        borderColor: "rgba(255, 255, 255, 0.8)",
      },
    };
    
    return settings;
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
    offset: function offset(element) {
      var rect = element.getBoundingClientRect();
      var scrollLeft =
        window.pageXOffset || document.documentElement.scrollLeft;
      var scrollTop = window.pageYOffset || document.documentElement.scrollTop;
      return {
        top: rect.top + scrollTop,
        left: rect.left + scrollLeft,
      };
    },
    hexToRgb: function hexToRgb(hexValue) {
      var hex;
      hexValue.indexOf("#") === 0
        ? (hex = hexValue.substring(1))
        : (hex = hexValue); // Expand shorthand form (e.g. "03F") to full form (e.g. "0033FF")

      var shorthandRegex = /^#?([a-f\d])([a-f\d])([a-f\d])$/i;
      var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(
        hex.replace(shorthandRegex, function (m, r, g, b) {
          return r + r + g + g + b + b;
        })
      );
      return result
        ? [
            parseInt(result[1], 16),
            parseInt(result[2], 16),
            parseInt(result[3], 16),
          ]
        : null;
    },
    rgbColor: function rgbColor(color) {
      if (color === void 0) {
        color = "#fff";
      }

      return "rgb(" + this.hexToRgb(color) + ")";
    },
    rgbaColor: function rgbaColor(color, alpha) {
      if (color === void 0) {
        color = "#fff";
      }

      if (alpha === void 0) {
        alpha = 0.5;
      }

      return "rgba(" + this.hexToRgb(color) + ", " + alpha + ")";
    },
    rgbColors: function rgbColors() {
      var _this3 = this;

      return Object.keys(this.colors).map(function (color) {
        return _this3.rgbColor(_this3.colors[color]);
      });
    },
    rgbaColors: function rgbaColors() {
      var _this4 = this;

      return Object.keys(this.colors).map(function (color) {
        return _this4.rgbaColor(_this4.colors[color]);
      });
    },
    settings: pluginSettings(_this),
    isIterableArray: function isIterableArray(array) {
      return Array.isArray(array) && !!array.length;
    }
  };
  return Utils;
})(jQuery);

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
    ECHART_LINE_TOTAL_ORDER: ".echart-line-total-order",
    ECHART_BAR_WEEKLY_SALES: ".echart-bar-weekly-sales",
    ECHART_LINE_TOTAL_SALES: ".echart-line-total-sales",
    SELECT_MONTH: ".select-month",
    ECHART_BAR_TOP_PRODUCTS: ".echart-bar-top-products",
    ECHART_WORLD_MAP: ".echart-world-map",
    ECHART_DOUGHNUT: ".echart-doughnut",
  }; //
  // ─── TOTAL ORDER CHART ──────────────────────────────────────────────────────────
  //

  var $echartLineTotalOrder = document.querySelector(
    Selector.ECHART_LINE_TOTAL_ORDER
  );

  if ($echartLineTotalOrder) {
    var $this = $($echartLineTotalOrder); // Get options from data attribute

    var userOptions = $this.data("options");
    var chart = window.echarts.init($echartLineTotalOrder); // Default options

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
        data: ["Week 4", "Week 5"],
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
          data: [20, 130],
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
  } //
  // ─── WEEKLY SALES CHART ─────────────────────────────────────────────────────────
  //

  var $echartBarWeeklySales = document.querySelector(
    Selector.ECHART_BAR_WEEKLY_SALES
  );

  if ($echartBarWeeklySales) {
    var _$this = $($echartBarWeeklySales); // Get options from data attribute

    var _userOptions = _$this.data("options");

    var data = [120, 200, 150, 80, 70, 110, 120]; // Max value of data

    var yMax = Math.max.apply(Math, data);
    var dataBackground = data.map(function () {
      return yMax;
    });

    var _chart = window.echarts.init($echartBarWeeklySales); // Default options

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
          animation: false,
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

  var $echartsLineTotalSales = document.querySelector(
    Selector.ECHART_LINE_TOTAL_SALES
  );
  var months = [
    "Jan",
    "Feb",
    "Mar",
    "Apr",
    "May",
    "Jun",
    "Jul",
    "Aug",
    "Sep",
    "Oct",
    "Nov",
    "Dec",
  ];

  var iTemp = -100;

  function getFormatter(params) {
    var _params$ = params[0],
      name = _params$.name,
      value = _params$.value;
    var date = new Date(name);
    return months[0] + " " + date.getDate() + ", " + value;
  }

  const RANDOM_NUM = (min, max) => Math.floor(Math.random() * (max - min + 1) + min)

  if ($echartsLineTotalSales) {
    var _$this2 = $($echartsLineTotalSales); // Get options from data attribute

    var _userOptions2 = _$this2.data("options");

    var _chart2 = window.echarts.init($echartsLineTotalSales);

    var monthsnumber = [
        [RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500) ],
        [RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500) ],
        [RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500) ],
        [RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500) ],
        [RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500) ],
        [RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500) ],
        [RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500) ],
        [RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500) ],
        [RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500) ],
        [RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500) ],
        [RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500) ],
        [RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500), RANDOM_NUM(200, 1500) ],
    ];
    var _defaultOptions2 = {
      color: utils.grays.white,
      tooltip: {
        trigger: "axis",
        padding: [7, 10],
        backgroundColor: utils.grays.white,
        borderColor: utils.grays["300"],
        borderWidth: 1,
        textStyle: {
          color: utils.colors.dark,
        },
        formatter: function formatter(params) {
          return getFormatter(params);
        },
        transitionDuration: 0,
        position: function position(pos, params, dom, rect, size) {
          return getPosition(pos, params, dom, rect, size);
        },
      },
      xAxis: {
        type: "category",
        data: [
          "2024-01-01",
          "2024-01-02",
          "2024-01-03",
          "2024-01-04",
          "2024-01-05",
          "2024-01-06",
          "2024-01-07",
          "2024-01-08",
          "2024-01-09",
          "2024-01-10",
          "2024-01-11",
          "2024-01-12",
          "2024-01-13",
          "2024-01-14",
          "2024-01-15",
          "2024-01-16",
          "2024-01-17",
          "2024-01-18",
          "2024-01-19",
          "2024-01-20",
          "2024-01-21",
          "2024-01-22",
          "2024-01-23",
          "2024-01-24",
          "2024-01-25",
          "2024-01-26",
          "2024-01-27",
          "2024-01-28",
          "2024-01-29",
          "2024-01-30",
          "2024-01-31"
        ],
        boundaryGap: false,
        axisPointer: {
          lineStyle: {
            color: utils.grays["300"],
            type: "dashed",
          },
        },
        splitLine: {
          show: false,
        },
        axisLine: {
          lineStyle: {
            // color: utils.grays['300'],
            color: utils.rgbaColor("#000", 0.01),
            type: "dashed",
          },
        },
        axisTick: {
          show: false,
        },
        axisLabel: {
          color: utils.grays["400"],
          formatter: function formatter(value) {
            var date = new Date(value);
            return months[date.getMonth()] + " " + date.getDate();
          },
          margin: 15,
        },
      },
      yAxis: {
        type: "value",
        data: ['$  0', '$ 100', '$ 200', '$ 300', '$ 400', '$ 500', '$ 600'],
        axisPointer: {
          show: false,
        },
        splitLine: {
          lineStyle: {
            color: utils.grays["300"],
            type: "dashed",
          },
        },
        boundaryGap: false,
        axisLabel: {
          show: true,
          color: utils.grays["800"],
          margin: 15,
        },
        axisTick: {
          show: false,
        },
        axisLine: {
          show: false,
        },
      },
      series: [
        {
          type: "line",
          data: monthsnumber[0],
          lineStyle: {
            color: utils.colors.primary,
          },
          itemStyle: {
            borderColor: utils.colors.primary,
            borderWidth: 2,
          },
          symbol: "circle",
          symbolSize: 10,
          smooth: false,
          hoverAnimation: true,
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
                  color: utils.rgbaColor(utils.colors.primary, 0.2),
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
        right: "28px",
        left: "49px",
        bottom: "15%",
        top: "5%",
      },
    };

    var _options2 = window._.merge(_defaultOptions2, _userOptions2);

    _chart2.setOption(_options2); // Change chart options accordiong to the selected month

    utils.$document.on(Events.CHANGE, Selector.SELECT_MONTH, function (e) {
      var $field = $(e.target);
      var month = $field.val();
      var data = monthsnumber[month]; 

      _chart2.setOption({
        tooltip: {
          formatter: function formatter(params) {
            var _params$2 = params[0],
              name = _params$2.name,
              value = _params$2.value;
            var date = new Date(name);
            return months[month] + " " + date.getDate() + ", " + value;
          },
        },
        xAxis: {
          axisLabel: {
            formatter: function formatter(value) {
              var date = new Date(value);
              return months[$field.val()] + " " + date.getDate();
            },
            margin: 15,
          },
        },
        series: [
          {
            data: data //data.map(function (item) { return item / 10; }) // !!! here we normalize data
          },
        ],
      });
    });
  } //
  // ─── BAR CHART TOP PRODUCTS ──────────────────────────────────────────────────────────────────
  //

  var $echartBarTopProducts = document.querySelector(
    Selector.ECHART_BAR_TOP_PRODUCTS
  );

  if ($echartBarTopProducts) {
    var _data = [
      ["product", "2019", "2018"],
      ["Boots4", 43, 85],
      ["Reign Pro", 83, 73],
      ["Slick", 86, 62],
      ["Falcon", 72, 53],
      ["Sparrow", 80, 50],
      ["Hideway", 50, 70],
      ["Freya", 80, 90], // ['Raven Pro', 60, 70],
      // ['Posh', 80, 70],
    ];

    var _$this3 = $($echartBarTopProducts);

    var _userOptions3 = _$this3.data("options");

    var _chart3 = window.echarts.init($echartBarTopProducts);

    var _defaultOptions3 = {
      color: [utils.colors.primary, utils.grays["300"]],
      dataset: {
        source: _data,
      },
      tooltip: {
        trigger: "item",
        padding: [7, 10],
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
        formatter: function formatter(params) {
          return (
            '<div class="font-weight-semi-bold">' +
            params.seriesName +
            '</div><div class="fs--1 text-600"><strong>' +
            params.name +
            ":</strong> " +
            params.value[params.componentIndex + 1] +
            "</div>"
          );
        },
      },
      legend: {
        data: ["2019", "2018"],
        left: "left",
        itemWidth: 10,
        itemHeight: 10,
        borderRadius: 0,
        icon: "circle",
        inactiveColor: utils.grays["500"],
        textStyle: {
          color: utils.grays["700"],
        },
      },
      xAxis: {
        type: "category",
        axisLabel: {
          color: utils.grays["400"],
        },
        axisLine: {
          lineStyle: {
            color: utils.grays["300"],
            type: "dashed",
          },
        },
        axisTick: false,
        boundaryGap: true,
      },
      yAxis: {
        axisPointer: {
          type: "none",
        },
        axisTick: "none",
        splitLine: {
          lineStyle: {
            color: utils.grays["300"],
            type: "dashed",
          },
        },
        axisLine: {
          show: false,
        },
        axisLabel: {
          color: utils.grays["400"],
        },
      },
      series: [
        {
          type: "bar",
          barWidth: "12%",
          barGap: "30%",
          label: {
            normal: {
              show: false,
            },
          },
          z: 10,
          itemStyle: {
            normal: {
              barBorderRadius: [10, 10, 0, 0],
              color: utils.colors.primary,
            },
          },
        },
        {
          type: "bar",
          barWidth: "12%",
          barGap: "30%",
          label: {
            normal: {
              show: false,
            },
          },
          itemStyle: {
            normal: {
              barBorderRadius: [4, 4, 0, 0],
              color: utils.grays[300],
            },
          },
        },
      ],
      grid: {
        right: "0",
        left: "30px",
        bottom: "10%",
        top: "20%",
      },
    };

    var _options3 = window._.merge(_defaultOptions3, _userOptions3);

    _chart3.setOption(_options3);
  } //
  // ─── PIE CHART ──────────────────────────────────────────────────────────────────
  //

  var $pieChartRevenue = document.querySelector(Selector.ECHART_DOUGHNUT);

  if ($pieChartRevenue) {
    var _$this4 = $($pieChartRevenue);

    var _userOptions4 = _$this4.data("options");

    var _chart4 = window.echarts.init($pieChartRevenue);

    var _defaultOptions4 = {
      color: [utils.colors.primary, utils.colors.info, utils.grays[300]],
      tooltip: {
        trigger: "item",
        padding: [7, 10],
        backgroundColor: utils.grays.white,
        textStyle: {
          color: utils.grays.black,
        },
        transitionDuration: 0,
        borderColor: utils.grays["300"],
        borderWidth: 1,
        formatter: function formatter(params) {
          return (
            "<strong>" + params.data.name + ":</strong> " + params.percent + "%"
          );
        },
      },
      position: function position(pos, params, dom, rect, size) {
        return getPosition(pos, params, dom, rect, size);
      },
      legend: {
        show: false,
      },
      series: [
        {
          type: "pie",
          radius: ["100%", "87%"],
          avoidLabelOverlap: false,
          hoverAnimation: false,
          itemStyle: {
            borderWidth: 2,
            borderColor: utils.grays.white,
          },
          label: {
            normal: {
              show: false,
              position: "center",
              textStyle: {
                fontSize: "20",
                fontWeight: "500",
                color: utils.grays["700"],
              },
            },
            emphasis: {
              show: false,
            },
          },
          labelLine: {
            normal: {
              show: false,
            },
          },
          data: [
            {
              value: 5300000,
              name: "Samsung",
            },
            {
              value: 1900000,
              name: "Huawei",
            },
            {
              value: 2000000,
              name: "Apple",
            },
          ],
        },
      ],
    };

    var _options4 = window._.merge(_defaultOptions4, _userOptions4);

    _chart4.setOption(_options4);
  } //
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