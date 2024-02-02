using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Windows.Forms.Timer;

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
        public int LoopCounter
        {
            get { return loopCounter; }
        }
        public void EnablePointAdding(bool enable)
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

        private void PictureBox_MouseClick(object? sender, MouseEventArgs e)
        {
            // Ajouter le point où l'utilisateur a cliqué
            points.Add(e.Location);
            pictureBox.Invalidate(); // Demander le redessin de la PictureBox
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

            // Vous pouvez également effacer la liste de points si nécessaire
            points.Clear();
            pictureBox.Invalidate(); // Demander le redessin de la PictureBox
        }
    }
}
