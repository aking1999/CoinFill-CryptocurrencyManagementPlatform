"use strict";

function ownKeys(object, enumerableOnly) {
  var keys = Object.keys(object);
  if (Object.getOwnPropertySymbols) {
    var symbols = Object.getOwnPropertySymbols(object);
    if (enumerableOnly) {
      symbols = symbols.filter(function (sym) {
        return Object.getOwnPropertyDescriptor(object, sym).enumerable;
      });
    }
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

function _typeof(obj) {
  "@babel/helpers - typeof";
  if (typeof Symbol === "function" && typeof Symbol.iterator === "symbol") {
    _typeof = function _typeof(obj) {
      return typeof obj;
    };
  } else {
    _typeof = function _typeof(obj) {
      return obj &&
        typeof Symbol === "function" &&
        obj.constructor === Symbol &&
        obj !== Symbol.prototype
        ? "symbol"
        : typeof obj;
    };
  }
  return _typeof(obj);
}

/* -------------------------------------------------------------------------- */

/*                                    Utils                                   */

/* -------------------------------------------------------------------------- */
var docReady = function docReady(fn) {
  // see if DOM is already available
  if (document.readyState === "loading") {
    document.addEventListener("DOMContentLoaded", fn);
  } else {
    setTimeout(fn, 1);
  }
};

var resize = function resize(fn) {
  return window.addEventListener("resize", fn);
};

var isIterableArray = function isIterableArray(array) {
  return Array.isArray(array) && !!array.length;
};

var camelize = function camelize(str) {
  var text = str.replace(/[-_\s.]+(.)?/g, function (_, c) {
    return c ? c.toUpperCase() : "";
  });
  return "".concat(text.substr(0, 1).toLowerCase()).concat(text.substr(1));
};

var getData = function getData(el, data) {
  try {
    return JSON.parse(el.dataset[camelize(data)]);
  } catch (e) {
    return el.dataset[camelize(data)];
  }
};
/* ----------------------------- Colors function ---------------------------- */

var hexToRgb = function hexToRgb(hexValue) {
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
};

var rgbaColor = function rgbaColor() {
  var color =
    arguments.length > 0 && arguments[0] !== undefined ? arguments[0] : "#fff";
  var alpha =
    arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : 0.5;
  return "rgba(".concat(hexToRgb(color), ", ").concat(alpha, ")");
};
/* --------------------------------- Colors --------------------------------- */

var getColor = function getColor(name) {
  var dom =
    arguments.length > 1 && arguments[1] !== undefined
      ? arguments[1]
      : document.documentElement;
  return getComputedStyle(dom)
    .getPropertyValue("--falcon-".concat(name))
    .trim();
};

var getColors = function getColors(dom) {
  return {
    primary: getColor("primary", dom),
    secondary: getColor("secondary", dom),
    success: getColor("success", dom),
    info: getColor("info", dom),
    warning: getColor("warning", dom),
    danger: getColor("danger", dom),
    light: getColor("light", dom),
    dark: getColor("dark", dom),
  };
};

var getSoftColors = function getSoftColors(dom) {
  return {
    primary: getColor("soft-primary", dom),
    secondary: getColor("soft-secondary", dom),
    success: getColor("soft-success", dom),
    info: getColor("soft-info", dom),
    warning: getColor("soft-warning", dom),
    danger: getColor("soft-danger", dom),
    light: getColor("soft-light", dom),
    dark: getColor("soft-dark", dom),
  };
};

var getGrays = function getGrays(dom) {
  return {
    white: getColor("white", dom),
    100: getColor("100", dom),
    200: getColor("200", dom),
    300: getColor("300", dom),
    400: getColor("400", dom),
    500: getColor("500", dom),
    600: getColor("600", dom),
    700: getColor("700", dom),
    800: getColor("800", dom),
    900: getColor("900", dom),
    1000: getColor("1000", dom),
    1100: getColor("1100", dom),
    black: getColor("black", dom),
  };
};

var getOffset = function getOffset(el) {
  var rect = el.getBoundingClientRect();
  var scrollLeft = window.pageXOffset || document.documentElement.scrollLeft;
  var scrollTop = window.pageYOffset || document.documentElement.scrollTop;
  return {
    top: rect.top + scrollTop,
    left: rect.left + scrollLeft,
  };
};

/* --------------------------------- Cookie --------------------------------- */

var settings = {
  tinymce: {
    theme: "oxide",
  },
  chart: {
    borderColor: "rgba(255, 255, 255, 0.8)",
  },
};
/* -------------------------- Chart Initialization -------------------------- */

var newChart = function newChart(chart, config) {
  var ctx = chart.getContext("2d");
  return new window.Chart(ctx, config);
};

/* get Dates between */

var getDates = function getDates(startDate, endDate) {
  var interval =
    arguments.length > 2 && arguments[2] !== undefined
      ? arguments[2]
      : 1000 * 60 * 60 * 24;
  var duration = endDate - startDate;
  var steps = duration / interval;
  return Array.from(
    {
      length: steps + 1,
    },
    function (v, i) {
      return new Date(startDate.valueOf() + interval * i);
    }
  );
};

var getPastDates = function getPastDates(duration) {
  var days;

  switch (duration) {
    case "week":
      days = 7;
      break;

    case "month":
      days = 30;
      break;

    case "year":
      days = 365;
      break;

    default:
      days = duration;
  }

  var date = new Date();
  var endDate = date;
  var startDate = new Date(new Date().setDate(date.getDate() - (days - 1)));
  return getDates(startDate, endDate);
};
/* Get Random Number */

var getRandomNumber = function getRandomNumber(min, max) {
  return Math.floor(Math.random() * (max - min) + min);
};

var utils = {
  docReady: docReady,
  resize: resize,
  isIterableArray: isIterableArray,
  camelize: camelize,
  getData: getData,
  hexToRgb: hexToRgb,
  rgbaColor: rgbaColor,
  getColor: getColor,
  getColors: getColors,
  getSoftColors: getSoftColors,
  getGrays: getGrays,
  getOffset: getOffset,
  newChart: newChart,
  settings: settings,
  getDates: getDates,
  getPastDates: getPastDates,
  getRandomNumber: getRandomNumber,
};
/* eslint-disable */

var getPosition = function getPosition(pos, params, dom, rect, size) {
  return {
    top: pos[1] - size.contentSize[1] - 10,
    left: pos[0] - size.contentSize[0] / 2,
  };
};

var echartSetOption = function echartSetOption(
  chart,
  userOptions,
  getDefaultOptions
) {
  var themeController = document.body; // Merge user options with lodash

  chart.setOption(window._.merge(getDefaultOptions(), userOptions));
  themeController.addEventListener("clickControl", function (_ref) {
    var control = _ref.detail.control;

    if (control === "theme") {
      chart.setOption(window._.merge(getDefaultOptions(), userOptions));
    }
  });
};

var tooltipFormatter = function tooltipFormatter(params) {
  var tooltipItem = "";
  params.forEach(function (el) {
    tooltipItem =
      tooltipItem +
      '<div class=\'ms-1\'> \n        <h6 class="text-700"><span class="fas fa-circle me-1 fs--2" style="color:'
        .concat(
          el.borderColor ? el.borderColor : el.color,
          '"></span>\n          '
        )
        .concat(el.seriesName, " : ")
        .concat(
          _typeof(el.value) === "object" ? el.value[1] : el.value,
          "\n        </h6>\n      </div>"
        );
  });
  return "<div>\n            <p class='mb-2 text-600'>\n              "
    .concat(
      window.dayjs(params[0].axisValue).isValid()
        ? window.dayjs(params[0].axisValue).format("MMMM DD")
        : params[0].axisValue,
      "\n            </p>\n            "
    )
    .concat(tooltipItem, "\n          </div>");
};
/* -------------------------------------------------------------------------- */

/*                             Echarts Bar Chart                             */

/* -------------------------------------------------------------------------- */

var echartsBarLineChartInit = function echartsBarLineChartInit() {
  var $barLineChartEl = document.querySelector(
    ".echart-bar-line-chart-example"
  );

  if ($barLineChartEl) {
    // Get options from data attribute
    var camelize2 = function camelize2(str) {
      var text = str.replace(/[-_\s.]+(.)?/g, function (_, c) {
        return c ? c.toUpperCase() : "";
      });
      return "".concat(text.substr(0, 1).toLowerCase()).concat(text.substr(1));
    };

    var getData2 = function getData2(el, data) {
      try {
        return JSON.parse(el.dataset[camelize2(data)]);
      } catch (e) {
        return el.dataset[camelize2(data)];
      }
    };
    var userOptions = getData2($barLineChartEl, "options");
    var chart = window.echarts.init($barLineChartEl);
    var months = [
      "January",
      "February",
      "March",
      "April",
      "May",
      "June",
      "July",
      "August",
      "September",
      "October",
      "November",
      "December",
    ];

    var getDefaultOptions = function getDefaultOptions() {
      return {
        tooltip: {
          trigger: "axis",
          axisPointer: {
            type: "cross",
            crossStyle: {
              color: getGrays()["500"],
            },
            label: {
              show: true,
              backgroundColor: getGrays()["600"],
              color: getGrays()["100"],
            },
          },
          padding: [7, 10],
          backgroundColor: getGrays()["100"],
          borderColor: getGrays()["300"],
          textStyle: {
            color: getColors().dark,
          },
          borderWidth: 1,
          transitionDuration: 0,
          formatter: tooltipFormatter,
        },
        toolbox: {
          top: 0,
          feature: {
            dataView: {
              show: false,
            },
            magicType: {
              show: true,
              type: ["line", "bar"],
            },
            restore: {
              show: true,
            },
            saveAsImage: {
              show: true,
            },
          },
          iconStyle: {
            borderColor: getGrays()["700"],
            borderWidth: 1,
          },
          emphasis: {
            iconStyle: {
              textFill: getGrays()["600"],
            },
          },
        },
        legend: {
          top: 40,
          data: ["Top-ups", "Spendings", "Total Balance"],
          textStyle: {
            color: getGrays()["600"],
          },
        },
        xAxis: [
          {
            type: "category",
            data: months,
            axisLabel: {
              color: getGrays()["600"],
              formatter: function formatter(value) {
                return value.slice(0, 3);
              },
            },
            axisPointer: {
              type: "shadow",
            },
            axisLine: {
              show: true,
              lineStyle: {
                color: getGrays()["300"],
              },
            },
          },
        ],
        yAxis: [
          {
            type: "value",
            min: 0,
            max: 10000,
            interval: 2000,
            axisLabel: {
              color: getGrays()["600"],
              formatter: "$ {value}",
            },
            splitLine: {
              show: true,
              lineStyle: {
                color: getGrays()["200"],
              },
            },
          },
          {
            type: "value",
            min: 0,
            max: 50000,
            interval: 10000,
            axisLabel: {
              color: getGrays()["600"],
              formatter: "$ {value}",
            },
            splitLine: {
              show: true,
              lineStyle: {
                color: getGrays()["200"],
              },
            },
          },
        ],
        series: [
          {
            name: "Top-ups",
            type: "bar",
            data: [
              3046.11, 5496.54, 4765.59, 1785.62, 7284.47, 7340.23, 6848.72, 4911.20, 6046.86, 7506.76, 7738.35,
              6439.01,
            ],
            itemStyle: {
              color: getColor("primary"),
              barBorderRadius: [3, 3, 0, 0],
            },
          },
          {
            name: "Spendings",
            type: "bar",
            data: [
              1641.43, 1521.56, 2209.76, 2676.74, 2711.81, 1500.23, 1568.60, 6820.02, 2808.17, 1870.58, 1798.24,
              2032.32,
            ],
            itemStyle: {
              color: getColor("danger"),
              barBorderRadius: [3, 3, 0, 0],
            },
          },
          {
            name: "Total Balance",
            type: "line",
            yAxisIndex: 1,
            data: [
                1404.68, 5379.66, 7935.49, 7044.37, 11617.03, 17457.03, 22737.15, 20828.33, 24067.02, 29703.2, 35643.31, 40050
            ],
            lineStyle: {
                color: getColor("success"),
            },
            itemStyle: {
              color: getGrays().white,
                borderColor: getColor("success"),
              borderWidth: 2,
            },
            symbol: "circle",
            symbolSize: 10,
          },
        ],
        grid: {
          right: 5,
          left: 5,
          bottom: 5,
          top: "23%",
          containLabel: true,
        },
      };
    };

    echartSetOption(chart, userOptions, getDefaultOptions);
  }
};
/* -------------------------------------------------------------------------- */

/*                             Echarts Doughnut Chart                         */

/* -------------------------------------------------------------------------- */

var data1 = [
  {
    value: 1048,
    name: "Starter",
    itemStyle: {
      color: utils.getColor("danger"),
    },
  },
  {
    value: 735,
    name: "Basic",
    itemStyle: {
      color: utils.getColor("primary"),
    },
  },
  {
    value: 580,
    name: "Optimal",
    itemStyle: {
      color: utils.getColor("secondary"),
    },
  },
  {
    value: 484,
    name: "Business",
    itemStyle: {
      color: utils.getColor("warning"),
    },
  },
  {
    value: 300,
    name: "Premium",
    itemStyle: {
      color: utils.getColor("success"),
    },
  },
  {
    value: 300,
    name: "Platinum",
    itemStyle: {
      color: utils.getColor("info"),
    },
  },
];
var data2 = [
  {
    value: 1048,
    name: "Facebook",
    itemStyle: {
      color: utils.getColor("primary"),
    },
  },
  {
    value: 735,
    name: "Youtube",
    itemStyle: {
      color: utils.getColor("danger"),
    },
  },
  {
    value: 580,
    name: "Twitter",
    itemStyle: {
      color: utils.getColor("info"),
    },
  },
  {
    value: 484,
    name: "Linkedin",
    itemStyle: {
      color: utils.getColor("success"),
    },
  },
  {
    value: 300,
    name: "Github",
    itemStyle: {
      color: utils.getColor("warning"),
    },
  },
];
var defaultRadius = {
  radius: "55%",
};
var smallRadius = {
  radius: "48%",
};
/* -------------------------------------------------------------------------- */

/*                            Theme Initialization                            */

/* -------------------------------------------------------------------------- */
docReady(echartsBarLineChartInit);
