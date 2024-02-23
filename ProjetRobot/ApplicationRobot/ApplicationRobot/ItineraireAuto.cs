/// La classe ItineraireAuto gère un itinéraire automatique pour un objet Square dans une application de simulation de mouvement.
/// Elle permet d'ajouter des points de passage, de déplacer le carré à travers ces points et de gérer un mode de déplacement en boucle.
/// 
/// Fonctionnalités principales :
/// - Ajout de Points : Permet aux utilisateurs d'ajouter des points de passage en cliquant sur la PictureBox.
/// - Déplacement Automatique : Déplace le carré automatiquement d'un point à l'autre en suivant l'itinéraire défini.
/// - Mode Boucle : Offre la possibilité de faire tourner le carré en boucle sur l'itinéraire défini, avec un compteur de boucles.
/// - Gestion de la Vitesse : Contrôle la vitesse de déplacement du carré pour assurer un mouvement fluide et constant.
/// - Affichage de l'Itinéraire : Dessine visuellement l'itinéraire et les points sur la PictureBox pour une meilleure compréhension de la trajectoire.
/// - Réinitialisation et Annulation : Offre des méthodes pour réinitialiser l'itinéraire, annuler l'ajout de points et arrêter le déplacement automatique.
/// 

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;
using System.Xml.Linq;
using System.IO;
using Microsoft.VisualBasic;

namespace ApplicationRobot
{
    internal class ItineraireAuto
    {
        private List<Point> points = new List<Point>();
        private PictureBox pictureBox;
        private Square square;
        private Form1 form;
        private int loopCounter = 0;
        private Timer moveTimer;
        private int currentPointIndex = 0;
        private const int moveInterval = 20; // Millisecondes
        private bool loopMode = false; // Pour suivre si le mode boucle est activé
        private bool LigneMode = false;

        public ItineraireAuto(PictureBox pictureBox, Square square, Form1 form)
        {
            this.pictureBox = pictureBox;
            this.square = square;
            this.form = form;

            // Initialiser le Timer pour le déplacement du carré
            moveTimer = new Timer();
            moveTimer.Interval = moveInterval;
            moveTimer.Tick += MoveTimer_Tick;
        }
        public List<Point> GetPoints()
        {
            return new List<Point>(this.points);
        }
        public int LoopCounter
        {
            get { return loopCounter; }
        }
        public void EnablePointAdding(bool enable)
        {
            if (LigneMode == false)
            {
                if (enable)
                {
                    pictureBox.MouseClick += PictureBox_MouseClick;
                }
                else
                {
                    pictureBox.MouseClick -= PictureBox_MouseClick;
                }
            }
        }

        private void PictureBox_MouseClick(object? sender, MouseEventArgs e)
        {
            if (LigneMode == false) 
            { 
                // Ajouter le point où l'utilisateur a cliqué
                points.Add(e.Location);
                pictureBox.Invalidate(); // Demander le redessin de la PictureBox
            }
        }

        public void StartMoving(bool loop = false)
        {
            loopMode = loop; // Définir le mode boucle en fonction du paramètre
            currentPointIndex = 0; // Commencer au premier point
            if (points.Count > 0)
            {
                moveTimer.Start(); // Commencer à bouger vers le premier point
            }
        }

        private void MoveTimer_Tick(object? sender, EventArgs e)
        {
            if (currentPointIndex >= points.Count)
            {
                moveTimer.Stop(); // Arrêter le Timer si tous les points ont été atteints
                return;
            }

            Point currentPoint = points[currentPointIndex];
            Point squarePosition = new Point(square.X, square.Y);

            // Calculer le vecteur de déplacement
            Point moveVector = new Point(currentPoint.X - squarePosition.X, currentPoint.Y - squarePosition.Y);

            double distanceToCurrentPoint = Math.Sqrt(moveVector.X * moveVector.X + moveVector.Y * moveVector.Y);

            if (distanceToCurrentPoint < 5) // Seuil pour considérer que le point est atteint
            {
                currentPointIndex++; // Passer au point suivant
                if (currentPointIndex >= points.Count)
                {
                    if (loopMode)
                    {
                        currentPointIndex = 0; // Recommencer depuis le début si en mode boucle
                        loopCounter++;
                        form.UpdateLoopCounterLabel(loopCounter);
                    }
                    else
                    {
                        moveTimer.Stop(); // Arrêter le Timer si tous les points ont été atteints et pas en mode boucle
                        return;
                    }
                }
                currentPoint = points[currentPointIndex]; // Mettre à jour le point courant
                moveVector = new Point(currentPoint.X - squarePosition.X, currentPoint.Y - squarePosition.Y);
            }

            // Normaliser le vecteur de déplacement pour obtenir une vitesse constante
            double distance = Math.Sqrt(moveVector.X * moveVector.X + moveVector.Y * moveVector.Y);
            int moveDistance = 5; // Ajustez selon la vitesse souhaitée
            square.X += (int)(moveVector.X / distance * moveDistance);
            square.Y += (int)(moveVector.Y / distance * moveDistance);
            pictureBox.Invalidate(); // Demander le redessin de la PictureBox
        }

        public void Draw(Graphics g)
        {
            // Dessiner les points
            for (int i = 0; i < points.Count; i++)
            {
                g.FillEllipse(Brushes.Red, points[i].X - 5, points[i].Y - 5, 10, 10); // Dessiner un cercle pour chaque point
                g.DrawString((i + 1).ToString(), SystemFonts.DefaultFont, Brushes.Black, points[i]);
            }
        }
        public void ChargerItineraire(List<Point> nouvelItineraire)
        {
            // Remplacer l'itinéraire actuel par le nouvel itinéraire
            this.points = new List<Point>(nouvelItineraire);

            // Réinitialiser l'état pour commencer le mouvement depuis le début du nouvel itinéraire
            this.currentPointIndex = 0;
            this.loopCounter = 0;

            // Optionnel : Réinitialiser d'autres états si nécessaire
            // Exemple : Arrêter le mouvement actuel, si le carré est en déplacement
            this.moveTimer.Stop();

            // Demander la mise à jour de l'affichage si nécessaire
            this.pictureBox.Invalidate();
        }
        public void ResetPoints()
        {
            points.Clear(); // Effacer la liste des points
            loopCounter = 0;
            pictureBox.Invalidate(); // Demander le redessin de la PictureBox pour effacer les points anciens
        }
        public void CancelAdding()
        {
            // Annuler l'ajout de points
            EnablePointAdding(false);
            loopMode = false;
            LigneMode = false;

            moveTimer?.Stop();
            // Vous pouvez également effacer la liste de points si nécessaire
            points.Clear();
            pictureBox.Invalidate(); // Demander le redessin de la PictureBox
        }
        public void PauseMoving()
        {
            // Stops the timer and keeps the current position in the route.
            moveTimer.Stop();
        }

        public void ResumeMoving()
        {
            // Starts the timer from the current position in the route.
            moveTimer.Start();
        }
        public void CommencerNouvelItineraire(Point debut)
        {
            // Initialiser une nouvelle liste de points pour l'itinéraire
            this.points.Clear();
            this.points.Add(debut);
        }

        public void AjouterPointItineraire(Point point)
        {
            LigneMode = true;
            if (points.Count > 0)
            {
                var lastPoint = points.Last();
                if (Math.Sqrt(Math.Pow(lastPoint.X - point.X, 2) + Math.Pow(lastPoint.Y - point.Y, 2)) < 10)
                {
                    // The point is too close to the last point, so don't add it
                    return;
                }
            }

            this.points.Add(point);
            this.pictureBox.Invalidate(); // Redraw the PictureBox to show the new point
        }

        public void SauvegarderItineraire(List<Point> points, string nomItineraire)
        {
            LigneMode = false;
            string filePath = "@\"..\\..\\..\\..\\Historique\\itineraires.xml";
            XElement root;

            if (File.Exists(filePath))
            {
                root = XElement.Load(filePath);
            }
            else
            {
                root = new XElement("Itineraires");
            }

            int numeroItineraire = root.Elements("Itineraire").Count() + 1; // Déterminer le numéro du nouvel itinéraire
            nomItineraire = Microsoft.VisualBasic.Interaction.InputBox("Entrez le nom de l'itinéraire :", "Nom de l'Itinéraire", "Itineraire X");
            
            if (!string.IsNullOrWhiteSpace(nomItineraire))
            {
                XElement itineraire = new XElement("Itineraire",
                new XAttribute("Nom", nomItineraire),
                new XAttribute("Date", DateTime.Now.ToString("yyyy-MM-dd")));

                foreach (var point in points)
                {
                    itineraire.Add(new XElement("Point",
                        new XAttribute("X", point.X),
                        new XAttribute("Y", point.Y)));
                }

                root.Add(itineraire);
                root.Save(filePath);
            }
            else
            {
               XElement itineraire = new XElement("Itineraire",
               new XAttribute("Nom", $"Itineraire {numeroItineraire}"),
               new XAttribute("Date", DateTime.Now.ToString("yyyy-MM-dd")));

                foreach (var point in points)
                {
                    itineraire.Add(new XElement("Point",
                        new XAttribute("X", point.X),
                        new XAttribute("Y", point.Y)));
                }

                root.Add(itineraire);
                root.Save(filePath);
            }
        }
    }
}
