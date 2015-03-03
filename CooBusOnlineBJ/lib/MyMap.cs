using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Microsoft.Phone.Maps.Controls;
using System.Device.Location;

public class MyMap
{
    public static void AddMark(Map target, GeoCoordinate[] geo, string[] textMark, double fontSize, 
        Color textColor, Color ShapeColor, double offset)
    {
        target.Layers.Add(GetMapLayers(geo, textMark, fontSize, textColor, ShapeColor, offset));
    }

    public static void AddMark(Map target, GeoCoordinate geo, string textMark, double fontSize, 
        Color textColor, Color ShapeColor, double offset)
    {
        MapOverlay MyOverlay = new MapOverlay();
        MyOverlay.Content = GetGrid(fontSize, textMark, textColor, ShapeColor, offset);
        MyOverlay.GeoCoordinate = new GeoCoordinate(geo.Latitude, geo.Longitude);
        MyOverlay.PositionOrigin = new Point(0, 0.5);
        MapLayer MyLayer = new MapLayer();
        MyLayer.Add(MyOverlay);
        target.Layers.Add(MyLayer);
    }

    private static MapLayer GetMapLayers(GeoCoordinate[] geo, string[] textMark, double fontSize, 
        Color textColor, Color ShapeColor, double offset)
    {
        MapLayer MyLayer = new MapLayer();
        for (int i = 0; i < geo.Length; i++)
        {
            MapOverlay MyOverlay = new MapOverlay();
            MyOverlay.Content = GetGrid(fontSize, textMark[i], textColor, ShapeColor, offset);
            MyOverlay.GeoCoordinate = new GeoCoordinate(geo[i].Latitude, geo[i].Longitude);
            MyOverlay.PositionOrigin = new Point(0, 0.5);
            MyLayer.Add(MyOverlay);
        }
        return MyLayer;
    }

    private static Grid GetGrid(double fontSize,string text,Color textColor,Color ShapeColor,double offset)
    {
        Grid MyGrid = new Grid();
        MyGrid.RowDefinitions.Add(new RowDefinition());
        MyGrid.RowDefinitions.Add(new RowDefinition());
        MyGrid.Background = new SolidColorBrush(Colors.Transparent);

        TextBlock MyTextBlock = new TextBlock();
        MyTextBlock.Text = text;
        MyTextBlock.FontSize = fontSize;
        MyTextBlock.Foreground = new SolidColorBrush(textColor);
        MyTextBlock.Margin = new Thickness((0D - fontSize / offset * text.Length), 0, 0, 0);
        MyTextBlock.SetValue(Grid.RowProperty, 0);
        MyTextBlock.SetValue(Grid.ColumnProperty, 0);
        //Adding to the Grid
        MyGrid.Children.Add(MyTextBlock);

        //Rectangle MyRectangle = new Rectangle();
        //MyRectangle.Fill = new SolidColorBrush(Colors.Black);
        //MyRectangle.Height = 20;
        //MyRectangle.Width = 20;
        //MyRectangle.SetValue(Grid.RowProperty, 1);
        //MyRectangle.SetValue(Grid.ColumnProperty, 0);
        ////Adding the Rectangle to the Grid
        //MyGrid.Children.Add(MyRectangle);

        Polygon MyPolygon = new Polygon();
        MyPolygon.Points.Add(new Point(-7, 0));
        MyPolygon.Points.Add(new Point(7, 0));
        MyPolygon.Points.Add(new Point(0, 14));
        MyPolygon.Stroke = new SolidColorBrush(ShapeColor);
        MyPolygon.Fill = new SolidColorBrush(ShapeColor);
        MyPolygon.SetValue(Grid.RowProperty, 1);
        MyPolygon.SetValue(Grid.ColumnProperty, 0);
        //Adding the Polygon to the Grid
        MyGrid.Children.Add(MyPolygon);
        return MyGrid;
    }
}
