import {
  ApexAxisChartSeries,
  ApexChart,
  ApexXAxis,
  ApexTitleSubtitle,
  ApexLegend,
  ApexDataLabels,
  ApexFill,
  ApexStroke,
  ApexYAxis,
  ApexStates,
  ApexTooltip,
  ApexGrid,
  ApexTheme,
  ApexAnnotations,
  ApexResponsive,
  ApexPlotOptions,
  ApexMarkers,
} from 'ng-apexcharts';

export type ChartOptions = {
  annotations: ApexAnnotations;
  chart: ApexChart;
  colors: string[];
  dataLabels: ApexDataLabels;
  fill: ApexFill;
  grid: ApexGrid;
  legend: ApexLegend;
  markers: ApexMarkers;
  plotOptions: ApexPlotOptions;
  responsive: ApexResponsive[];
  series: ApexAxisChartSeries;
  states: ApexStates;
  stroke: ApexStroke;
  subtitle: ApexTitleSubtitle;
  theme: ApexTheme;
  title: ApexTitleSubtitle;
  tooltip: ApexTooltip;
  xaxis: ApexXAxis;
  yaxis: ApexYAxis;
};

export function HSLToHex(color: string): string {
  const colorArray = color.split('%').join('').split(' ');
  const colorHSL = colorArray.map(Number);
  const hsl = {
    h: colorHSL[0],
    s: colorHSL[1],
    l: colorHSL[2],
  };
  const { h, s, l } = hsl;

  const hDecimal = l / 100;
  const a = (s * Math.min(hDecimal, 1 - hDecimal)) / 100;
  const f = (n: number) => {
    const k = (n + h / 30) % 12;
    const color = hDecimal - a * Math.max(Math.min(k - 3, 9 - k, 1), -1);

    // Convert to Hex and prefix with "0" if required
    return Math.round(255 * color)
      .toString(16)
      .padStart(2, '0');
  };
  return `#${f(0)}${f(8)}${f(4)}`;
}
