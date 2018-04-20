using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.IO;
using System.Drawing;

public partial class chart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        using (var ch = new Chart())
        {
            ch.ChartAreas.Add(new ChartArea("Valor"));
            var s = new Series();
            ch.Width = 2190;
            ch.Height = 1190;
            ch.Series.Add("Series2");
            ch.Series["Series2"].ChartType = SeriesChartType.Column;
            ch.Series["Series2"].YValueType = ChartValueType.Double;
            ch.Series["Series2"].Points.AddXY("hola quea!!!", 35);
            ch.Series["Series2"].Points.AddXY("4", 120);
            ch.Series["Series2"].Points.AddXY("weke", 15);
            ch.Series["Series2"].Points.AddXY("essdsd", 15);
            ch.Series["Series2"].Points.AddXY("hola quea!!!", 35);
            ch.Series["Series2"].Points.AddXY("4", 120);
            ch.Series["Series2"].Points.AddXY("4", 120);
            ch.Series["Series2"].Points.AddXY("4", 120);
            ch.Series["Series2"].Points.AddXY("4", 420);
            ch.Series["Series2"].ChartArea = "Valor";
            ch.Series["Series2"].IsValueShownAsLabel = true;
            ch.Series["Series2"].Font = new System.Drawing.Font("Arial", 25, FontStyle.Bold);

            ch.Series.Add("Series1");
            ch.Series["Series1"].ChartType = SeriesChartType.Line;
            ch.Series["Series1"].BorderWidth = 10;
            ch.Series["Series1"].YValueType = ChartValueType.Double;
            ch.Series["Series1"].Points.AddY(10);
            ch.Series["Series1"].Points.AddY(15);
            ch.Series["Series1"].Points.AddY(55);
            ch.Series["Series1"].Points.AddY(45);
            ch.Series["Series1"].Points.AddY(15);
            ch.Series["Series1"].Points.AddY(45);
            ch.Series["Series1"].Points.AddY(15);
            ch.Series["Series1"].Points.AddY(45);
            ch.Series["Series1"].Points.AddY(20);
            ch.Series["Series1"].ChartArea = "Valor";
            // ch.Series["Series1"].AxisLabel = "Meta";

            //ch.ChartAreas["Valor"].AxisX.LabelStyle.Format = "yyyy-MM-dd HH:mm";
            ch.ChartAreas["Valor"].AxisX.LabelStyle.Font = new Font("Arial", 35);
            ch.ChartAreas["Valor"].AxisX.MajorGrid.Enabled = false;
            ch.ChartAreas["Valor"].AxisX.LabelStyle.Angle = 75;
            ch.ChartAreas["Valor"].AxisY.LabelStyle.Font = new Font("Arial", 30);
            ch.ChartAreas["Valor"].RecalculateAxesScale();

            foreach (DataPoint Point in ch.Series[0].Points)
            {
                if (Point.YValues[0] >= 1 && Point.YValues[0] <= 30)
                {
                    Point.Color = Color.Red;
                }
                else
                {
                    Point.Color = Color.Green;
                }
            }

            ch.Series.Add(s);
            ch.SaveImage(@"D:\My Web Sites\Homer\Homer Theme\webapplayers.com\homer_admin-v1.9.1\Log\test.png", ChartImageFormat.Jpeg);
        }
    }
}