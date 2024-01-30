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
                Text = "Vous avez glitch de la réalité !"
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
