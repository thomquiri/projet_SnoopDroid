/// La classe Backroom surveille la position d'un carré (Square) et affiche un popup si le carré sort des limites définies de l'espace de jeu.
/// 
/// Caractéristiques principales :
/// - Surveillance de Position : Vérifie si la position du carré est en dehors des limites spécifiées (plus petite que -10000 ou plus grande que 10000 en X ou Y).
/// - Affichage de Popup : Si le carré sort de ces limites, un popup avec une image est affiché, symbolisant une 'sortie de la réalité'.
/// - Gestion de Flag : Utilise un drapeau (flagOn) pour s'assurer que le popup ne s'affiche qu'une seule fois et évite les affichages répétés.
/// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationRobot
{
    internal class Backroom
    {
        private readonly string imagePath = @"..\..\..\..\image\backrooms.png"; // Chemin vers votre image
        private bool flagOn;
        public void CheckSquarePosition(Square square)
        {
            if (square.X < -10000 || square.X > 10000 || square.Y < -10000 || square.Y > 10000)
            {
                ShowPopup();
            }
        }

        private void ShowPopup()
        {
            if (flagOn) return;
            flagOn = true;
            // Créer une nouvelle form pour le pop-up
            Form popup = new Form
            {
                Width = 500,
                Height = 500,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                StartPosition = FormStartPosition.CenterScreen,
                Text = "Vous avez glitch en dehors de la réalité !"
            };

            // Charger l'image
            PictureBox pictureBox = new PictureBox
            {
                Image = Image.FromFile(imagePath),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Dock = DockStyle.Fill
            };

            // Ajouter PictureBox à la form
            popup.Controls.Add(pictureBox);

            // Afficher le pop-up
            popup.ShowDialog();
        }
    }
}
