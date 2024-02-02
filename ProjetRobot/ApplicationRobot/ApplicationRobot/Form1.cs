using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace ApplicationRobot
{
    public partial class Form1 : Form
    {
        private Square square;
        private PositionDisplayManager positionDisplayManager;
        private Turn turnManager;
        private Image squareImage;
        private Joystick joystick;
        private Timer joystickTimer;
        private ItineraireAuto itineraireAuto;
        private Backroom backroom;
        private DateTime lastUpdateTime;
        private const int squareSize = 50; // Taille du carré
        private const int moveDistance = 10; // Distance de déplacement pour chaque clic

        public Form1()
        {
            InitializeComponent();
            positionDisplayManager = new PositionDisplayManager(labelPosition);
            turnManager = new Turn();
            squareImage = Image.FromFile(@"..\..\..\..\image\ScrappyIcon.png"); // Mettez à jour le chemin vers votre image
            // Initialiser le carré au milieu de la pictureBox
            square = new Square(pictureBoxMap1.Width / 2, pictureBoxMap1.Height / 2, squareSize, turnManager);
            DrawSquare();
            backroom = new Backroom();
            joystick = new Joystick(pictureBoxJoystickBig, pictureBoxJoystickSmall);
            joystickTimer = new Timer();
            joystickTimer.Interval = 20; // Interval en millisecondes
            joystickTimer.Tick += JoystickTimer_Tick;
            joystickTimer.Start();
            itineraireAuto = new ItineraireAuto(pictureBoxMap1, square, this);
            pictureBoxMap1.Paint += (sender, e) => itineraireAuto.Draw(e.Graphics);

            lastUpdateTime = DateTime.Now;
        }

        private void JoystickTimer_Tick(object? sender, EventArgs e)
        {
            // Calculer le temps écoulé depuis le dernier update
            DateTime now = DateTime.Now;
            float elapsedTime = (float)(now - lastUpdateTime).TotalSeconds;
            lastUpdateTime = now;

            // Obtenir l'angle et la magnitude du joystick
            (float angle, float magnitude) = joystick.GetDirection();

            // Déplacer le carré en fonction de l'angle et de la magnitude du joystick
            square.MoveByJoystick(angle, magnitude, elapsedTime);

            // Redessiner le carré
            DrawSquare();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Arrêter et disposer le Timer
            if (joystickTimer != null)
            {
                joystickTimer.Stop();
                joystickTimer.Dispose();
            }

            // Détacher les événements du joystick
            joystick?.DetachEvents();
        }
        private void buttonUp_Click(object sender, EventArgs e)
        {
            square.Move(moveDistance, -90);
            DrawSquare();
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            square.Move(moveDistance, 90);
            DrawSquare();
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            square.Move(-moveDistance, 0);
            DrawSquare();
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            square.Move(moveDistance, 0);
            DrawSquare();
        }
        private void buttonTurnRight_Click(object sender, EventArgs e)
        {
            turnManager.TurnRight(10); // Tourner de 10 degrés vers la droite
            DrawSquare();
        }

        private void buttonTurnLeft_Click(object sender, EventArgs e)
        {
            turnManager.TurnLeft(10); // Tourner de 10 degrés vers la gauche
            DrawSquare();
        }

        private void DrawSquare()
        {
            try
            {
                if (pictureBoxMap1.BackgroundImage == null)
                {
                    throw new InvalidOperationException("pictureBoxMap1.BackgroundImage is null.");
                }

                if (square == null)
                {
                    throw new InvalidOperationException("Square is null.");
                }
                // Créer un Bitmap basé sur l'image de fond originale de la PictureBox
                Bitmap bmp = new Bitmap(pictureBoxMap1.BackgroundImage);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // Pas besoin de faire g.Clear() car nous dessinons sur l'image de fond fraîchement créée

                    // Créer une transformation pour la rotation et la translation
                    Matrix transform = new Matrix();
                    transform.RotateAt(turnManager.CurrentAngle, new PointF(square.X + squareImage.Width / 2.0f, square.Y + squareImage.Height / 2.0f));
                    g.Transform = transform;

                    // Dessiner l'image du carré
                    g.DrawImage(squareImage, square.X - square.Size / 2, square.Y - square.Size / 2, squareImage.Width, squareImage.Height);
                }
                pictureBoxMap1.Image?.Dispose();
                // Définir l'image du PictureBox avec le Bitmap mis à jour
                pictureBoxMap1.Image = bmp;
                positionDisplayManager.UpdatePosition(square.X, square.Y);

                if (backroom == null)
                {
                    throw new InvalidOperationException("Backroom is null.");
                }

                // Vérifier la position du carré et afficher le pop-up si nécessaire
                backroom.CheckSquarePosition(square);
            }
            catch (Exception ex)
            {
                // Gérer l'exception (par exemple, enregistrer dans un fichier journal ou afficher un message)
                Console.WriteLine(ex.Message);
            }
            // Mettre à jour le texte du label pour montrer la position actuelle
        }

        private void labelPosition_Click(object sender, EventArgs e)
        {

        }

        private void buttonMapClick_Click(object sender, EventArgs e)
        {
            itineraireAuto = new ItineraireAuto(pictureBoxMap1, square, this);

            itineraireAuto.ResetPoints();
            pictureBoxMap1.Refresh();
            itineraireAuto.EnablePointAdding(true);
            buttonAddPos.Show();

            buttonUp.Enabled = false;
            buttonDown.Enabled = false;
            buttonLeft.Enabled = false;
            buttonRight.Enabled = false;
            buttonTurnLeft.Enabled = false;
            buttonTurnRight.Enabled = false;
            buttonMapClick.Enabled = false;
            buttonStartItin.Enabled = false;
            buttonStartItinBoucle.Enabled = false;

            // Activer buttonAddPos et buttonCancel
            buttonAddPos.Enabled = true;
            buttonCancel.Enabled = true;
            // Forcer le rafraîchissement pour assurer que les anciens dessins sont effacés
            pictureBoxMap1.Invalidate(); // Marquer la zone de dessin comme invalide pour déclencher un événement Paint
            pictureBoxMap1.Update(); // Forcer l'exécution immédiate de l'événement Paint
        }

        private void buttonAddPos_Click(object sender, EventArgs e)
        {
            itineraireAuto.EnablePointAdding(true);
            buttonStartItin.Show();
            buttonStartItinBoucle.Show();
            buttonStartItin.Enabled = true;
            buttonStartItinBoucle.Enabled = true;
        }

        private void buttonStartItin_Click(object sender, EventArgs e)
        {
            itineraireAuto.StartMoving();
            buttonAddPos.Hide();
            EnableAllButtons();
        }
        private void buttonStartItinBoucle_Click(object sender, EventArgs e)
        {
            itineraireAuto.StartMoving(loop: true);
            buttonAddPos.Hide();
            EnableAllButtons();
            UpdateLoopCounterLabel(itineraireAuto.LoopCounter);
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            // Annuler l'action en cours
            itineraireAuto.CancelAdding();
            buttonMapClick.Enabled = true;

            // Réactiver tous les autres boutons
            EnableAllButtons();
            buttonAddPos.Hide();
            UpdateLoopCounterLabel(itineraireAuto.LoopCounter);
        }
        private void EnableAllButtons()
        {
            // Réactiver tous les boutons
            buttonUp.Enabled = true;
            buttonDown.Enabled = true;
            buttonLeft.Enabled = true;
            buttonRight.Enabled = true;
            buttonTurnLeft.Enabled = true;
            buttonTurnRight.Enabled = true;
            buttonStartItin.Enabled = true;
            buttonStartItinBoucle.Enabled = true;
            buttonCancel.Enabled = true;
            buttonAddPos.Enabled = false;
        }
        public void UpdateLoopCounterLabel(int loopCounter)
        {
            if (labelLoopCounter.InvokeRequired)
            {
                labelLoopCounter.Invoke(new Action(() => labelLoopCounter.Text = $"Boucles complétées : {loopCounter}"));
            }
            else
            {
                labelLoopCounter.Text = $"Nombre de tour : {loopCounter}";
            }
        }
        private void labelLoopCounter_Click(object sender, EventArgs e)
        {

        }
    }
}