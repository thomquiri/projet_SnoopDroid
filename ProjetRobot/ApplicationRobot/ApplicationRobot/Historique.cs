using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ApplicationRobot
{
    internal class Historique
    {
        private string filePath = "@\"..\\..\\..\\..\\Historique\\itineraires.xml"; // Chemin d'accès au fichier XML

        public void AfficherHistorique(ItineraireAuto itineraireAuto)
        {
            Form historiqueForm = new Form();
            ListBox listeItineraires = new ListBox();
            listeItineraires.Dock = DockStyle.Fill;

            Button deleteButton = new Button
            {
                Text = "Delete",
                Dock = DockStyle.Bottom
            };

            if (System.IO.File.Exists(filePath))
            {
                XElement root = XElement.Load(filePath);
                var itineraires = root.Elements("Itineraire");

                foreach (var itineraire in itineraires)
                {
                    string? nomItineraire = itineraire.Attribute("Nom").Value;
                    listeItineraires.Items.Add(nomItineraire);
                }
            }

            listeItineraires.DoubleClick += (sender, e) =>
            {
                if (listeItineraires.SelectedItem != null)
                {
                    string? nomItineraire = listeItineraires.SelectedItem.ToString();
                    if (nomItineraire != null)
                    {
                        List<Point> points = ChargerItineraireDepuisXML(nomItineraire);
                        itineraireAuto.ChargerItineraire(points); // Méthode pour charger l'itinéraire
                    }
                    historiqueForm.Close();
                }
            };

            historiqueForm.Controls.Add(listeItineraires);
            historiqueForm.Controls.Add(deleteButton);
            historiqueForm.Text = "Historique des Itinéraires";
            historiqueForm.Size = new Size(500, 300);
            
            deleteButton.Click += (sender, e) =>
            {
                if (listeItineraires.SelectedItem != null)
                {
                    string? nomItineraire = listeItineraires.SelectedItem.ToString();
                    if(nomItineraire != null) SupprimerItineraire(nomItineraire);
                    listeItineraires.Items.Remove(listeItineraires.SelectedItem);
                }
            };
            historiqueForm.ShowDialog();
        }

        private List<Point> ChargerItineraireDepuisXML(string nomItineraire)
        {
            List<Point> points = new List<Point>();
            XElement root = XElement.Load(filePath);
            var itineraire = root.Elements("Itineraire").FirstOrDefault(i => i.Attribute("Nom").Value == nomItineraire);

            if (itineraire != null)
            {
                foreach (XElement point in itineraire.Elements("Point"))
                {
                    int x = int.Parse(point.Attribute("X").Value);
                    int y = int.Parse(point.Attribute("Y").Value);
                    points.Add(new Point(x, y));
                }
            }

            return points;
        }
        public void SupprimerItineraire(string nomItineraire)
        {
            XElement root = XElement.Load(filePath);
            var itineraire = root.Elements("Itineraire").FirstOrDefault(i => i.Attribute("Nom").Value == nomItineraire);

            if (itineraire != null)
            {
                itineraire.Remove(); // Remove the itinerary element
                root.Save(filePath); // Save the updated XML document
            }
        }
    }
}
