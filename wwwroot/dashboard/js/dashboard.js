// Radial Chart
var options = {
  chart: {
    type: "radialBar",
    height: "100%",
    width: 380,
  },

  series: [70, 55, 11],
  labels: ["Website", "Ticketmaster", "Eventbrite"],
  colors: ["#7d3ef3", "#41c9ff", "#ff6b6b"],

  plotOptions: {
    radialBar: {
      hollow: { size: "45%" },
      track: {
        background: "#333",
        strokeWidth: "80%",
      },
      dataLabels: {
        show: true,
        name: {
          show: !false,
        },
        value: {
          show: !false,
        },
      },
      startAngle: -180,
      endAngle: 120,
    },
  },

  legend: {
    show: true,
    position: "right",
    offsetY: 0,
    offsetX: 0,
    markers: {
      size: 8,
      shape: "square",
      strokeWidth: 0,
      offsetX: -5,
      offsetY: 0,
    },
    labels: {
      colors: "#ddd",
    },
  },
  stroke: {
    lineCap: "round",
  },
  responsive: [
    {
      breakpoint: 100000,
      options: {
        chart: {
          height: "100%",
        },
        legend: {
          position: "right",
        },
      },
    },
  ],
};

var chart = new ApexCharts(document.querySelector("#donut-chart"), options);
chart.render();

// ! LineChart

var lineOptions = {
  chart: {
    type: "line",
    height: "80%",

    toolbar: { show: false },
    dropShadow: {
      enabled: true,
      top: 5,
      left: 0,
      blur: 10,
      opacity: 0.2,
    },
  },
  series: [
    {
      name: "Website",
      data: [120, 200, 300, 450, 350, 500, 420, 460, 390],
    },
    {
      name: "Ticketmaster",
      data: [100, 190, 260, 300, 400, 380, 350, 420, 440],
    },
    {
      name: "Eventbrite",
      data: [150, 230, 200, 280, 260, 300, 370, 350, 380],
    },
  ],
  colors: ["#41c9ff", "#7d3ef3", "#ffda5a"],
  stroke: {
    width: 4,
    curve: "smooth",
    lineCap: "round",
  },
  markers: {
    size: 4,
    strokeWidth: 0,
    hover: { size: 6 },
  },
  grid: {
    borderColor: "rgba(255,255,255,.05)",
    strokeDashArray: 6,
  },
  xaxis: {
    categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep"],
    labels: {
      style: {
        colors: "#9ca3af",
        fontSize: "13px",
      },
    },
    axisBorder: { show: false },
    axisTicks: { show: false },
  },
  yaxis: {
    labels: {
      style: {
        colors: "#9ca3af",
        fontSize: "12px",
      },
      offsetX: -10,
      formatter: (val) => Math.round(val / 100) + "k",
    },
  },
  fill: {
    type: "gradient",
    gradient: {
      shadeIntensity: 1,
      opacityFrom: 0.45,
      opacityTo: 0.45,
    },
  },
  legend: {
    show: true,
    position: "top",
    horizontalAlign: "right",
    offsetY: 0,
    labels: {
      colors: "#fff",
    },
    markers: {
      size: 8,
      shape: "square",
      strokeWidth: 0,
      offsetX: -5,
      offsetY: 0,
    },
  },
  tooltip: {
    theme: "dark",
  },
};
var lineChart = new ApexCharts(
  document.querySelector("#line-chart"),
  lineOptions
);
lineChart.render();

// Area Chart
var areaOptions = {
  chart: {
    type: "area",
    // width: "100%",
    height: "100%",
    toolbar: { show: false },
    zoom: { enabled: false },
  },
  series: [
    {
      name: "Website",
      data: [120, 200, 300, 450, 350, 500, 420, 460, 390],
    },
  ],
  stroke: {
    curve: "smooth",
    width: 3,
  },
  fill: {
    type: "gradient",
    gradient: {
      shade: "light",
      type: "horizontal",
      shadeIntensity: 1,
      opacityFrom: 1,
      opacityTo: 1,
      colorStops: [
        { offset: 0, color: "#474e7c" },
        { offset: 33, color: "#454098" },
        { offset: 66, color: "#b72898" },
        { offset: 100, color: "#d3bf7e" },
      ],
    },
  },
  title: {
    text: "Fiction Books Sales",
    style: {
      fontSize: "22px",
      fontWeight: "bold",
      color: "#ccc",
    },
  },
  grid: {
    show: !false,
    borderColor: "rgba(255,255,255,.05)",
    strokeDashArray: 7,
    yaxis: { lines: { show: true } },
  },
  dataLabels: {
    enabled: false,
  },
  xaxis: {
    categories: ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep"],
    labels: { style: { colors: "#9ca3af", fontSize: "13px" } },
    axisBorder: { show: false },
    axisTicks: { show: false },
  },
  yaxis: {
    min: 0,
    labels: {
      style: { colors: "#9ca3af", fontSize: "12px" },
      formatter: (val) => val,
    },
  },
  tooltip: { theme: "dark" },
  markers: { size: 0 },
  legend: { show: false },
};

var areaChart = new ApexCharts(
  document.querySelector("#area-chart"),
  areaOptions
);
areaChart.render();

// Bar Chart

var barOptions = {
  chart: {
    type: "bar",
    height: 300,
    stacked: true,
    toolbar: { show: false },
  },
  series: [
    {
      name: "Online Sales",
      data: [20, 30, 40, 25, 35, 45, 30],
    },
    {
      name: "Offline Sales",
      data: [15, 25, 35, 20, 30, 40, 25],
    },
  ],
  xaxis: {
    categories: ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"],
    labels: {
      style: { colors: "#9ca3af" },
    },
    axisBorder: { show: false },
    axisTicks: { show: false },
  },
  yaxis: {
    labels: { style: { colors: "#9ca3af" } },
  },
  grid: {
    show: false,
  },
  fill: {
    type: "gradient",
    gradient: {
      shade: "dark",
      type: "vertical",
      shadeIntensity: 0.5,
      gradientToColors: ["#7d3ef3", "#ffda5a"],
      inverseColors: false,
      opacityFrom: 1,
      opacityTo: 1,
      stops: [0, 100],
    },
  },
  plotOptions: {
    bar: {
      borderRadius: 4,
      borderRadiusApplication: "end",
      borderRadiusWhenStacked: "last",
      columnWidth: "70%",
      colors: {
        backgroundBarColors: ["rgba(0,0,0)"],
        backgroundBarOpacity: 0.4,
        backgroundBarRadius: 4,
      },
    },
  },
  colors: ["#4310a0", "#ffc802"],
  legend: {
    show: true,
    position: "top",
    horizontalAlign: "right",
    labels: { colors: "#fff" },
    markers: {
      size: 8,
      shape: "square",
      strokeWidth: 0,
    },
  },
  dataLabels: {
    enabled: false,
  },
  tooltip: {
    theme: "dark",
  },
};

var barChart = new ApexCharts(document.querySelector("#bar-chart"), barOptions);
barChart.render();
