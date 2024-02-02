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
        private const int squareSize = 50; // Taille du carr�
        private const int moveDistance = 10; // Distance de d�placement pour chaque clic

        public Form1()
        {
            InitializeComponent();
            positionDisplayManager = new PositionDisplayManager(labelPosition);
            turnManager = new Turn();
            squareImage = Image.FromFile(@"..\..\..\..\image\ScrappyIcon.png"); // Mettez � jour le chemin vers votre image
            // Initialiser le carr� au milieu de la pictureBox
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
            // Calculer le temps �coul� depuis le dernier update
            DateTime now = DateTime.Now;
            float elapsedTime = (float)(now - lastUpdateTime).TotalSeconds;
            lastUpdateTime = now;

            // Obtenir l'angle et la magnitude du joystick
            (float angle, float magnitude) = joystick.GetDirection();

            // D�placer le carr� en fonction de l'angle et de la magnitude du joystick
            square.MoveByJoystick(angle, magnitude, elapsedTime);

            // Redessiner le carr�
            DrawSquare();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            // Arr�ter et disposer le Timer
            if (joystickTimer != null)
            {
                joystickTimer.Stop();
                joystickTimer.Dispose();
            }

            // D�tacher les �v�nements du joystick
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
            turnManager.TurnRight(10); // Tourner de 10 degr�s vers la droite
            DrawSquare();
        }

        private void buttonTurnLeft_Click(object sender, EventArgs e)
        {
            turnManager.TurnLeft(10); // Tourner de 10 degr�s vers la gauche
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
                // Cr�er un Bitmap bas� sur l'image de fond originale de la PictureBox
                Bitmap bmp = new Bitmap(pictureBoxMap1.BackgroundImage);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    // Pas besoin de faire g.Clear() car nous dessinons sur l'image de fond fra�chement cr��e

                    // Cr�er une transformation pour la rotation et la translation
                    Matrix transform = new Matrix();
                    transform.RotateAt(turnManager.CurrentAngle, new PointF(square.X + squareImage.Width / 2.0f, square.Y + squareImage.Height / 2.0f));
                    g.Transform = transform;

                    // Dessiner l'image du carr�
                    g.DrawImage(squareImage, square.X - square.Size / 2, square.Y - square.Size / 2, squareImage.Width, squareImage.Height);
                }
                pictureBoxMap1.Image?.Dispose();
                // D�finir l'image du PictureBox avec le Bitmap mis � jour
                pictureBoxMap1.Image = bmp;
                positionDisplayManager.UpdatePosition(square.X, square.Y);

                if (backroom == null)
                {
                    throw new InvalidOperationException("Backroom is null.");
                }

                // V�rifier la position du carr� et afficher le pop-up si n�cessaire
                backroom.CheckSquarePosition(square);
            }
            catch (Exception ex)
            {
                // G�rer l'exception (par exemple, enregistrer dans un fichier journal ou afficher un message)
                Console.WriteLine(ex.Message);
            }
            // Mettre � jour le texte du label pour montrer la position actuelle
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
            // Forcer le rafra�chissement pour assurer que les anciens dessins sont effac�s
            pictureBoxMap1.Invalidate(); // Marquer la zone de dessin comme invalide pour d�clencher un �v�nement Paint
            pictureBoxMap1.Update(); // Forcer l'ex�cution imm�diate de l'�v�nement Paint
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

            // R�activer tous les autres boutons
            EnableAllButtons();
            buttonAddPos.Hide();
            UpdateLoopCounterLabel(itineraireAuto.LoopCounter);
        }
        private void EnableAllButtons()
        {
            // R�activer tous les boutons
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
                labelLoopCounter.Invoke(new Action(() => labelLoopCounter.Text = $"Boucles compl�t�es : {loopCounter}"));
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