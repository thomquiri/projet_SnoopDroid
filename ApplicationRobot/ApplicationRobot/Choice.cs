using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationRobot
{
    internal class Choice
    {
        private PictureBox pictureBox;
        private string selectedImagePath; // Pour stocker le chemin de l'image sélectionnée
        Size minimumSize = new Size (1075, 650); // Taille minimum de pictureBoxMap1
        Size maximumSize = new Size(1075, 650); // Taille maximum, ajustez en fonction de la taille de l'écran et de l'espace nécessaire pour les autres UI

        public Choice(PictureBox pictureBox)
        {
            this.pictureBox = pictureBox;
            this.selectedImagePath = string.Empty; // Initialiser avec une chaîne vide
        }

        public string SelectedImagePath
        {
            get { return selectedImagePath; }
        }

        public void OpenImageDialog()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Choisir une image";
                openFileDialog.Filter = "Images Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        using (FileStream stream = new FileStream(openFileDialog.FileName, FileMode.Open, FileAccess.Read))
                        {
                            Image img = Image.FromStream(stream);
                            pictureBox.Invoke(new MethodInvoker(() =>
                            {
                                // Libérer l'image précédemment chargée, si elle existe
                                pictureBox.Image?.Dispose();

                                // Charger la nouvelle image
                                Image newImage = new Bitmap(img);

                                // Calculer la nouvelle taille en respectant les limites minimum et maximum
                                int newWidth = Math.Max(minimumSize.Width, Math.Min(newImage.Width, maximumSize.Width));
                                int newHeight = Math.Max(minimumSize.Height, Math.Min(newImage.Height, maximumSize.Height));

                                // Ajuster la taille de pictureBoxMap1
                                pictureBox.Size = new Size(newWidth, newHeight);

                                pictureBox.Image = newImage;

                                // Optionnel : ajuster l'image de fond si nécessaire
                                pictureBox.BackgroundImage = newImage;

                                // Redessiner si nécessaire
                                pictureBox.Invalidate();
                            }));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Une erreur est survenue lors du chargement de l'image : {ex.Message}");
                    }
                }
            }
        }
    }
}
