using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;

namespace ApplicationRobot
{
    internal class Echelle
    {
        private List<Point> echellePoints = new List<Point>();
        private float distanceSaisieEnCentimetres = 0; // Pour stocker la distance saisie
        private PictureBox pictureBox;
        private Form1 form;

        public Echelle(PictureBox pictureBox, Form1 form)
        {
            this.pictureBox = pictureBox;
            this.form = form;
        }
        public List<Point> GetEchellePoints()
        {
            return new List<Point>(this.echellePoints);
        }
        public void EnableEchellePointAdding(bool enable)
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
            if (echellePoints.Count == 0)
            {
                echellePoints.Add(e.Location);
            }
            else if (echellePoints.Count == 1)
            {
                // Calcule la direction préférée basée sur la différence entre les axes X et Y
                Point premierPoint = echellePoints[0];
                Point pointClique = e.Location;

                // Calcul de la différence entre les deux points
                int diffX = Math.Abs(pointClique.X - premierPoint.X);
                int diffY = Math.Abs(pointClique.Y - premierPoint.Y);

                Point deuxiemePoint;

                if (diffX > diffY)
                {
                    // Alignement horizontal
                    deuxiemePoint = new Point(pointClique.X, premierPoint.Y);
                }
                else
                {
                    // Alignement vertical
                    deuxiemePoint = new Point(premierPoint.X, pointClique.Y);
                }

                echellePoints.Add(deuxiemePoint);
                pictureBox.MouseClick -= PictureBox_MouseClick; // Empêcher l'ajout de points supplémentaires

                // Demande à l'utilisateur de saisir la distance en centimètres
                string input = Interaction.InputBox("Entrez la distance en centimètres entre les deux points :", "Distance", "0", -1, -1);
                if (float.TryParse(input, out float distance) && distance > 0)
                {
                    distanceSaisieEnCentimetres = distance;
                    // Calculez la distance en pixels entre les deux points
                    float distanceEnPixels = (float)Math.Sqrt(Math.Pow(echellePoints[1].X - echellePoints[0].X, 2) + Math.Pow(echellePoints[1].Y - echellePoints[0].Y, 2));
                    // Calculez l'échelle en cm par pixel
                    float echelleCmParPixel = distance / distanceEnPixels;

                    // Mettez à jour le label avec l'échelle
                    form.UpdateLabelEchelle(echelleCmParPixel);
                }
                else
                {
                    MessageBox.Show("La distance entrée n'est pas valide. Veuillez entrer un nombre positif.", "Erreur de saisie", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    echellePoints.Clear(); // Optionnel: Réinitialiser si la saisie est invalide
                }

                pictureBox.Invalidate(); // Provoque le redessin pour afficher la distance et les points
            }
        }
        public void ResetEchellePoints()
        {
            echellePoints.Clear(); // Effacer la liste des points
            pictureBox.Invalidate(); // Demander le redessin de la PictureBox pour effacer les points anciens
        }
        public void CancelEchelleAdding()
        {
            // Annuler l'ajout de points
            EnableEchellePointAdding(false);

            // Vous pouvez également effacer la liste de points si nécessaire
            echellePoints.Clear();
            pictureBox.Invalidate(); // Demander le redessin de la PictureBox
        }
        public void EchelleDraw(Graphics g)
        {
            if (echellePoints.Count == 2)
            {
                // Utilisez la distance réelle en pixels entre les deux points pour définir la longueur de l'échelle.
                float distanceEnPixels = (float)Math.Sqrt(Math.Pow((float)echellePoints[1].X - (float)echellePoints[0].X, 2) + Math.Pow((float)echellePoints[1].Y - (float)echellePoints[0].Y, 2));

                // Coordonnées pour positionner l'échelle en bas à gauche.
                int startX = 20; // 20 pixels du bord gauche.
                int startY = pictureBox.Height - 20; // 20 pixels du bord inférieur.

                // Dessiner le trait de l'échelle basé directement sur la distance en pixels entre les deux points.
                g.DrawLine(new Pen(Color.Blue, 2), startX, startY, startX + distanceEnPixels, startY);

                // Ajouter la distance en centimètres à côté du trait d'échelle.
                g.DrawString($"{distanceSaisieEnCentimetres} cm", new Font("Arial", 10), Brushes.Black, startX, startY + 5);
            }
        }
    }
}
