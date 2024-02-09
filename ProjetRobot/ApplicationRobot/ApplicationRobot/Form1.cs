/// Form1 est le formulaire principal de l'application Robot, agissant comme le point central de coordination pour toutes les interactions utilisateur et la logique de mouvement.
/// 
/// Fonctionnalit�s cl�s de Form1 :
/// - Gestion de plusieurs composants tels que Square, Joystick, PositionDisplayManager, Turn, Backroom et ItineraireAuto, permettant une simulation interactive de mouvement.
/// - Fournit des contr�les pour le d�placement manuel du carr� (Square), la rotation, ainsi que la gestion des itin�raires automatiques et des contr�les de joystick.
/// - Utilise des Timers pour g�rer les mises � jour continues bas�es sur le temps, telles que les mouvements de joystick.
/// - G�re l'affichage et la mise � jour des positions du carr�, ainsi que l'affichage des notifications sp�ciales lorsque le carr� atteint certaines positions (g�r� par Backroom).
/// - Permet l'ajout dynamique de points � l'itin�raire du carr� et la visualisation de ces points, ainsi que le d�marrage de mouvements automatis�s le long de ces points.
/// - Assure une interface utilisateur r�active et informe l'utilisateur des compteurs de boucles lorsqu'un itin�raire est suivi en mode boucle.
/// 

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
        private Choice choiceManager;
        private DateTime lastUpdateTime;
        private const int squareSize = 50; // Taille du carr�
        private const int moveDistance = 10; // Distance de d�placement pour chaque clic

        public Form1()
        {
            InitializeComponent();
            DisableAllButtons();
            positionDisplayManager = new PositionDisplayManager(labelPosition);
            turnManager = new Turn();
            squareImage = Image.FromFile(@"..\..\..\..\image\ScrappyIcon.png"); // Mettez � jour le chemin vers votre image
            // Initialiser le carr� au milieu de la pictureBox
            square = new Square(pictureBoxMap1.Width / 2, pictureBoxMap1.Height / 2, squareSize, turnManager);
            DrawSquare();
            backroom = new Backroom();
            choiceManager = new Choice(pictureBoxMap1);
            this.WindowState = FormWindowState.Maximized;
            joystick = new Joystick(pictureBoxJoystickBig, pictureBoxJoystickSmall);
            joystickTimer = new Timer();
            joystickTimer.Interval = 20; // Interval en millisecondes
            joystickTimer.Tick += JoystickTimer_Tick;
            joystickTimer.Start();
            itineraireAuto = new ItineraireAuto(pictureBoxMap1, square, this);
            pictureBoxMap1.Paint += (sender, e) => itineraireAuto.Draw(e.Graphics);

            lastUpdateTime = DateTime.Now;
            pictureBoxMap1.Paint += pictureBoxMap1_Paint; // Abonnement � l'�v�nement Paint     
        }

        private void pictureBoxMap1_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Dessiner l'image de fond
            if (pictureBoxMap1.BackgroundImage != null)
            {
                g.DrawImage(pictureBoxMap1.BackgroundImage, new Rectangle(0, 0, pictureBoxMap1.Width, pictureBoxMap1.Height));
            }

            // Dessiner le carr�
            if (squareImage != null && square != null)
            {
                Matrix transform = new Matrix();
                transform.RotateAt(turnManager.CurrentAngle, new PointF(square.X + squareImage.Width / 2.0f, square.Y + squareImage.Height / 2.0f));
                g.Transform = transform;
                g.DrawImage(squareImage, square.X - square.Size / 2, square.Y - square.Size / 2, squareImage.Width, squareImage.Height);
                positionDisplayManager.UpdatePosition(square.X, square.Y);
            }
            itineraireAuto.Draw(g);
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
            pictureBoxMap1.Invalidate();
            positionDisplayManager.UpdatePosition(square.X, square.Y);
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
            buttonChoice.Enabled = false;

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
            joystickTimer?.Stop();  // Par exemple, si vous utilisez un Timer pour le d�placement
            buttonMapClick.Enabled = true;

            // R�activer tous les autres boutons
            EnableAllButtons();
            buttonAddPos.Hide();
            UpdateLoopCounterLabel(itineraireAuto.LoopCounter);
        }
        private void EnableAllButtons()
        {
            // R�activer tous les boutons sauf buttonAddPos
            buttonUp.Enabled = true;
            buttonDown.Enabled = true;
            buttonLeft.Enabled = true;
            buttonRight.Enabled = true;
            buttonTurnLeft.Enabled = true;
            buttonTurnRight.Enabled = true;
            buttonStartItin.Enabled = true;
            buttonStartItinBoucle.Enabled = true;
            buttonCancel.Enabled = true;
            buttonChoice.Enabled = true;
            buttonAddPos.Enabled = false;
        }

        private void DisableAllButtons()
        {
            buttonUp.Enabled = false;
            buttonDown.Enabled = false;
            buttonLeft.Enabled = false;
            buttonRight.Enabled = false;
            buttonTurnLeft.Enabled = false;
            buttonTurnRight.Enabled = false;
            buttonStartItin.Enabled = false;
            buttonStartItinBoucle.Enabled = false;
            buttonCancel.Enabled = false;
            buttonAddPos.Enabled = false;
            buttonMapClick.Enabled = false;
            buttonChoice.Enabled = true;
        }
        public void UpdateLoopCounterLabel(int loopCounter)
        {
            if (labelLoopCounter.InvokeRequired)
            {
                labelLoopCounter.Invoke(new Action(() => labelLoopCounter.Text = $"Nombre de tour : {loopCounter}"));
            }
            else
            {
                labelLoopCounter.Text = $"Nombre de tour : {loopCounter}";
            }
        }
        private void labelLoopCounter_Click(object sender, EventArgs e)
        {

        }

        private void buttonChoice_Click(object sender, EventArgs e)
        {
            choiceManager.OpenImageDialog();
            EnableAllButtons();
            buttonMapClick.Enabled = true;
        }
    }
}