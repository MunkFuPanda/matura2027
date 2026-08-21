using DataModels;
using LinqToDB;
using System.ComponentModel;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Dijkstra___Algorithmen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private CitiesDB db;

        private List<Place> places;

        private List<Edge> connections = new();

        private Dictionary<Place, Node> nodes = new();

        private List<Place> visitedPlaces = new();

        public MainWindow()
        {
            InitializeComponent();

            db = new CitiesDB(new DataOptions().UseSQLite(@"Data Source=./model/cities.sqlite"));

            cb_country_select.Items.Add("Austria");
            cb_country_select.Items.Add("Germany");
            cb_country_select.Items.Add("Switzerland");

            ImageBrush imageBrush = new ImageBrush();
            imageBrush.ImageSource = new BitmapImage(new Uri("pack://application:,,,/DACH.png"));
            cv_map.Background = imageBrush;


        }

        private void cb_country_select_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // schauen welches Land, das als Liste laden und dann funktionen zum Zeichnen der Orte und der Linien machen
            // am schluss nochmal die überprüfungen zwei for schleifen ineinander zum zeichnen und prüfen

            cv_map.Children.Clear();

            // Klassenvariable aktualisieren
            places = db.Places
                .Where(x => x.Adm0name == cb_country_select.SelectedItem)
                .ToList();

            // Nodes neu aufbauen
            nodes.Clear();

            foreach (Place place in places)
            {
                nodes[place] = new Node()
                {
                    Place = place
                };
            }

            // Comboboxen neu befüllen
            cb_first_city.Items.Clear();
            cb_second_city.Items.Clear();

            foreach (Place place in places)
            {
                cb_first_city.Items.Add(place);
                cb_second_city.Items.Add(place);
            }

            DrawPlace(places);

            connections.Clear();

            List<Edge> edges = new();

            for (int i = 0; i < places.Count; i++)
            {
                for (int j = i + 1; j < places.Count; j++)
                {
                    double dist = Distance(
                        (double)places[i].Latitude, (double)places[i].Longitude,
                        (double)places[j].Latitude, (double)places[j].Longitude);

                    if (dist <= 300)
                        edges.Add(new Edge(places[i], places[j], dist));
                }
            }

            // kürzeste zuerst
            edges = edges.OrderBy(e => e.Distance).ToList();

            foreach (var d in edges)
            {
                Point p1 = ToCanvas(d.From);
                Point p2 = ToCanvas(d.To);

                bool ok = true;

                foreach (var c in connections)
                {
                    Point p3 = ToCanvas(c.From);
                    Point p4 = ToCanvas(c.To);

                    // das schaut damit wir nicht von einem punkt zu demselben punkt zeichnen

                    if (d.From == c.From || d.From == c.To || d.To == c.From || d.To == c.To)
                    {
                        continue;
                    }

                    // eduvidual funktionen einbauen

                    if (LinesIntersect(p1.X, p1.Y, p2.X, p2.Y,
                                       p3.X, p3.Y, p4.X, p4.Y))
                    {
                        ok = false;
                        break;
                    }
                }

                if (ok)
                {
                    connections.Add(d);

                    Node nodeA = nodes[d.From];
                    Node nodeB = nodes[d.To];

                    nodeA.Edges.Add(new Edge(d.From, d.To, d.Distance));
                    nodeB.Edges.Add(new Edge(d.To, d.From, d.Distance));
                }
                    

                    
            }

            foreach (var c in connections)
            {
                DrawLine((double)c.From.Latitude, (double)c.From.Longitude, (double)c.To.Latitude, (double)c.To.Longitude);
            }
        }
                
            
            
        

        private void DrawPlace(List<Place> places)
        {

            cv_map.Children.Clear();

            foreach (var place in places)
            {

                Point point = ToCanvas(place);

                Ellipse ellipse = new Ellipse
                {
                    Width = 8,
                    Height = 8,
                    Fill = Brushes.Red
                };

                Canvas.SetLeft(ellipse, point.X - 4);
                Canvas.SetTop(ellipse, point.Y - 4);

                cv_map.Children.Add(ellipse);
            }
        }

        private void DrawLine(double lat1, double lon1, double lat2, double lon2)
        {
            

            double north = 55.1;
            double south = 45.7;
            double west = 5.5;
            double east = 17.2;

            double x1 = (double)((lon1 - west) / (east - west) * cv_map.ActualWidth);
            double y1 = (double)(north - lat1) / (north - south) * cv_map.ActualHeight;

            double x2 = (double)((lon2 - west) / (east - west) * cv_map.ActualWidth);
            double y2 = (double)(north - lat2) / (north - south) * cv_map.ActualHeight;

            Line line = new Line
            {
                X1 = x1,
                Y1 = y1,
                X2 = x2,
                Y2 = y2,
                Stroke = Brushes.Black,
                StrokeThickness = 1
            };

            cv_map.Children.Add(line);
        }

        private double Distance(double lat1, double lon1,
                        double lat2, double lon2)
        {
            double R = 6371;

            double dLat = ToRad(lat2 - lat1);
            double dLon = ToRad(lon2 - lon1);

            lat1 = ToRad(lat1);
            lat2 = ToRad(lat2);

            double a =
                Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1) * Math.Cos(lat2) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Asin(Math.Sqrt(a));

            return R * c;
        }

        private double ToRad(double value)
        {
            return value * Math.PI / 180;
        }

        private Point ToCanvas(Place p)
        {
            double north = 55.1;
            double south = 45.7;
            double west = 5.5;
            double east = 17.2;

            double x = (double)(p.Longitude - west) / (east - west) * cv_map.ActualWidth;
            double y = (double)(north - p.Latitude) / (north - south) * cv_map.ActualHeight;

            return new Point(x, y);
        }

        private bool LinesIntersect(double x1, double y1, double x2, double y2,
                            double x3, double y3, double x4, double y4)
        {
            double denom =
                (x1 - x2) * (y3 - y4) -
                (y1 - y2) * (x3 - x4);

            if (Math.Abs(denom) < 0.000001)
                return false;

            double t =
                ((x1 - x3) * (y3 - y4) -
                 (y1 - y3) * (x3 - x4)) / denom;

            double u =
                ((x1 - x3) * (y1 - y2) -
                 (y1 - y3) * (x1 - x2)) / denom;

            return t > 0.001 && t < 0.999 && u > 0.001 && u < 0.999;
        }


        // VORGABE EDUVIDUAL CODE UMBAUEN TSP FUNKTIONEN
        double Measure(Vector2 p1, Vector2 p2)
        {
            var R = 6378.137; // Radius of earth in km
            var dLat = p2.Y * Math.PI / 180 - p1.Y * Math.PI / 180;
            var dLon = p2.X * Math.PI / 180 - p1.X * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Cos(p1.Y * Math.PI / 180) *
                    Math.Cos(p2.Y * Math.PI / 180) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // km
        }

        bool LineSegmentIntersection(Vector2 l1p1, Vector2 l1p2, Vector2 l2p1, Vector2 l2p2)
        {
            Vector2 a = l1p2 - l1p1;
            Vector2 b = l2p1 - l2p2;
            Vector2 c = l1p1 - l2p1;
            float alpha = b.Y * c.X - b.X * c.Y;
            float beta = a.X * c.Y - a.Y * c.X;
            float den = a.Y * b.X - a.X * b.Y;
            if (den == 0)
            {
                return false;
            }
            else if (den > 0)
            {
                if (alpha < 0 || alpha > den || beta < 0 || beta > den)
                {
                    return false;
                }
            }
            else if (alpha > 0 || alpha < den || beta > 0 || beta < den)
            {
                return false;
            }
            return true;
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            if (cb_first_city.SelectedItem != cb_second_city.SelectedItem)
            {
                List<Place> erg = Dijkstra((Place)cb_first_city.SelectedItem, (Place)cb_second_city.SelectedItem);

                DrawPath(erg);
            }
        }

        // Dijkstra Alogrithmus bekommt Start und Ziel (end) Place übergeben
        private List<Place> Dijkstra(Place start, Place end)
        {
            Dictionary<Place, double> dist = new();
            Dictionary<Place, Place> prev = new();
            List<Place> unvisited = new();

            // für anzeige der besuchten Orte (Gelb in der Karte markiert)
            visitedPlaces.Clear();

            foreach (var node in nodes.Keys)
            {
                dist[node] = double.MaxValue;
                unvisited.Add(node);
            }

            dist[start] = 0;

            while (unvisited.Count > 0)
            {
                Place current = unvisited.OrderBy(p => dist[p]).First();
                // Besuchten Platz einfügen für die Anzeige später
                visitedPlaces.Add(current);
                unvisited.Remove(current);

                if (current == end)
                {
                    break;
                }

                foreach (Edge edge in nodes[current].Edges)
                {
                    Place neighbor = edge.To;

                    if (!unvisited.Contains(neighbor)) {
                        continue;
                    }

                    double newDistance = dist[current] + edge.Distance;

                    if (newDistance < dist[neighbor])
                    {
                        dist[neighbor] = newDistance;
                        prev[neighbor] = current;
                    }
                }
            }

            List<Place> path = new();

            if (start != end && !prev.ContainsKey(end))
            {
                return path;
            }

            // vom Ende zum Start in die Liste path
            Place step = end;
            path.Add(end);

            while (step != start)
            {
                step = prev[step];
                path.Add(step);
            }

            // Liste verkehrt zurückgeben, damit man vom Start zum Ziel kommt
            path.Reverse();

            return path;
        }

        // Zuerst werden die Orte Gelb (Gold) eingezeichnet
        // dann werden die Linien vom Start bis zum Ziel gezeichnet
        private void DrawPath(List<Place> path)
        {
            foreach (Place place in visitedPlaces)
            {
                Point point = ToCanvas(place);

                Ellipse ellipse = new Ellipse
                {
                    Width = 12,
                    Height = 12,
                    Fill = Brushes.Gold
                };

                Canvas.SetLeft(ellipse, point.X - 6);
                Canvas.SetTop(ellipse, point.Y - 6);

                cv_map.Children.Add(ellipse);
            }

            for (int i = 0; i < path.Count - 1; i++)
            {
                Point p1 = ToCanvas(path[i]);
                Point p2 = ToCanvas(path[i + 1]);

                Line line = new Line
                {
                    X1 = p1.X,
                    Y1 = p1.Y,
                    X2 = p2.X,
                    Y2 = p2.Y,
                    Stroke = Brushes.Blue,
                    StrokeThickness = 4
                };

                cv_map.Children.Add(line);
            }
        }
    }




}