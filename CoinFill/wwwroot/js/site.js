function GetRgbValue(rgb) {
  if (rgb) {
    const rgbStringArray = rgb.replace(/[^\d,]/g, "").split(",");
    return {
      r: Number(rgbStringArray[0]),
      g: Number(rgbStringArray[1]),
      b: Number(rgbStringArray[2]),
    };
  }
}

function HexToRgbColor(hex) {
  if (hex) {
    // Expand shorthand form (e.g. "03F") to full form (e.g. "0033FF")
    var shorthandRegex = /^#?([a-f\d])([a-f\d])([a-f\d])$/i;
    hex = hex.replace(shorthandRegex, function (m, r, g, b) {
      return r + r + g + g + b + b;
    });

    var result = /^#?([a-f\d]{2})([a-f\d]{2})([a-f\d]{2})$/i.exec(hex);
    return result
      ? {
          r: parseInt(result[1], 16),
          g: parseInt(result[2], 16),
          b: parseInt(result[3], 16),
        }
      : null;
  }
}

function RgbToHexColor(r, g, b) {
  if (r && g && b) {
    return "#" + ((1 << 24) | (r << 16) | (g << 8) | b).toString(16).slice(1);
  }
}

function RgbToHsl(r, g, b) {
  (r /= 255), (g /= 255), (b /= 255);
  var max = Math.max(r, g, b),
    min = Math.min(r, g, b);
  var h,
    s,
    l = (max + min) / 2;

  if (max == min) {
    h = s = 0; // achromatic
  } else {
    var d = max - min;
    s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
    switch (max) {
      case r:
        h = (g - b) / d + (g < b ? 6 : 0);
        break;
      case g:
        h = (b - r) / d + 2;
        break;
      case b:
        h = (r - g) / d + 4;
        break;
    }
    h /= 6;
  }

  return {
    h,
    s,
    l,
  };
}

function HslToRgb(h, s, l) {
  var r, g, b;

  if (s == 0) {
    r = g = b = l; // achromatic
  } else {
    var hue2rgb = function hue2rgb(p, q, t) {
      if (t < 0) t += 1;
      if (t > 1) t -= 1;
      if (t < 1 / 6) return p + (q - p) * 6 * t;
      if (t < 1 / 2) return q;
      if (t < 2 / 3) return p + (q - p) * (2 / 3 - t) * 6;
      return p;
    };

    var q = l < 0.5 ? l * (1 + s) : l + s - l * s;
    var p = 2 * l - q;
    r = hue2rgb(p, q, h + 1 / 3);
    g = hue2rgb(p, q, h);
    b = hue2rgb(p, q, h - 1 / 3);
  }

  return {
    r: Math.round(r * 255),
    g: Math.round(g * 255),
    b: Math.round(b * 255),
  };
}

function HslToHslSoft(h, s, l) {
  return {
    h: h,
    s: s,
    l: 0.92,
  };
}

document.addEventListener("DOMContentLoaded", function () {
  $(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="popover"]').popover({
      boundary: "window",
      trigger: "hover focus",
    });
    $('[data-toggle="set-color-and-background-color"]').each(function () {
      try {
        const hexColor = $(this).attr("data-color");
        $(this).find("i").css("color", hexColor);

        let rgb = HexToRgbColor(hexColor);
        const hsl = RgbToHsl(rgb.r, rgb.g, rgb.b);
        const hslSoft = HslToHslSoft(hsl.h, hsl.s, hsl.l);
        const backgroundColorTemp = HslToRgb(hslSoft.h, hslSoft.s, hslSoft.l);
        const backgroundColor = `rgb(${backgroundColorTemp.r}, ${backgroundColorTemp.g}, ${backgroundColorTemp.b})`;

        $(this).css("background-color", backgroundColor);
      } catch (error) {
        return;
      }
    });
    $(".tiktok").attr("href", "https://tiktok.com/@coinfill.crypto");
    $(".instagram").remove();
  });
});

$(document).on("keyup keydown keypress", function (e) {
  var keyCode = e.keyCode || e.which;
  if (keyCode == 13) {
    e.preventDefault();
    return false;
  }
});

/**
 * Preloader
 */
const preloader = $("#preloader");

function RemovePreloader() {
  setTimeout(function () {
    preloader.fadeOut("slow", function () {
      $(this).remove();
    });
  }, 1000);
}

if (preloader) {
  window.addEventListener("load", RemovePreloader());
}
