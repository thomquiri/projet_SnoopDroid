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
using System.Collections.Generic;
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
        private Historique historique;
        private DateTime lastUpdateTime;
        private const int squareSize = 50; // Taille du carr�
        private const int moveDistance = 10; // Distance de d�placement pour chaque clic
        private bool isDrawing = false;
        private bool modeLigneActif = false;

        public Form1()
        {
            InitializeComponent();
            DisableAllButtons();
            positionDisplayManager = new PositionDisplayManager(labelPosition);
            turnManager = new Turn();
            squareImage = Image.FromFile(@"..\..\..\..\image\Baba.png"); // Mettez � jour le chemin vers votre image
            // Initialiser le carr� au milieu de la pictureBox
            square = new Square(pictureBoxMap1.Width / 2, pictureBoxMap1.Height / 2, squareSize, turnManager);
            DrawSquare();
            backroom = new Backroom();
            historique = new Historique();
            choiceManager = new Choice(pictureBoxMap1);
            this.WindowState = FormWindowState.Maximized;
            joystick = new Joystick(pictureBoxJoystickBig, pictureBoxJoystickSmall);
            joystick.JoystickValueChanged += joystick.OnJoystickValueChanged;
            joystickTimer = new Timer();
            joystickTimer.Interval = 20; // Interval en millisecondes
            joystickTimer.Tick += JoystickTimer_Tick;
            joystickTimer.Start();
            itineraireAuto = new ItineraireAuto(pictureBoxMap1, square, this);
            pictureBoxMap1.Paint += (sender, e) => itineraireAuto.Draw(e.Graphics);

            lastUpdateTime = DateTime.Now;
            pictureBoxMap1.Paint += pictureBoxMap1_Paint; // Abonnement � l'�v�nement Paint
            pictureBoxMap1.MouseDown += pictureBoxMap1_MouseDown;
            pictureBoxMap1.MouseMove += pictureBoxMap1_MouseMove;
            pictureBoxMap1.MouseUp += pictureBoxMap1_MouseUp;
            
        }

        private void pictureBoxMap1_Paint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // Dessiner l'image de fond
            if (pictureBoxMap1.BackgroundImage != null)
            {
                g.DrawImage(pictureBoxMap1.BackgroundImage, new Rectangle(0, 0, pictureBoxMap1.Width, pictureBoxMap1.Height));
            }

            // Sauvegarder l'�tat actuel des graphiques
            GraphicsState state = g.Save();
            // Dessiner le carr�
            if (squareImage != null && square != null)
            {
                Matrix transform = new Matrix();
                PointF pivotPoint = new PointF(square.X, square.Y);
                transform.RotateAt(turnManager.CurrentAngle, new PointF(square.X, square.Y));
                g.Transform = transform;
                g.DrawImage(squareImage, square.X - square.Size / 2, square.Y - square.Size / 2, squareImage.Width, squareImage.Height);
                // Restaurer l'�tat des graphiques pour retirer la transformation
                g.Restore(state);
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
            turnManager.TurnRight(30); // Tourner de 10 degr�s vers la droite
            DrawSquare();
        }

        private void buttonTurnLeft_Click(object sender, EventArgs e)
        {
            turnManager.TurnLeft(30); // Tourner de 10 degr�s vers la gauche
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
            buttonMapLine.Enabled = false;

            // Activer buttonAddPos et buttonCancel
            buttonAddPos.Enabled = true;
            buttonCancel.Enabled = true;
            // Forcer le rafra�chissement pour assurer que les anciens dessins sont effac�s
            pictureBoxMap1.Invalidate(); // Marquer la zone de dessin comme invalide pour d�clencher un �v�nement Paint
            pictureBoxMap1.Update(); // Forcer l'ex�cution imm�diate de l'�v�nement Paint
        }
        private void buttonMapLine_Click(object sender, EventArgs e)
        {
            itineraireAuto = new ItineraireAuto(pictureBoxMap1, square, this);
            modeLigneActif = true;

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
            buttonMapClick.Enabled = false;

            // Activer buttonAddPos et buttonCancel
            buttonAddPos.Enabled = true;
            buttonCancel.Enabled = true;
            // Forcer le rafra�chissement pour assurer que les anciens dessins sont effac�s
            pictureBoxMap1.Invalidate(); // Marquer la zone de dessin comme invalide pour d�clencher un �v�nement Paint
            pictureBoxMap1.Update(); // Forcer l'ex�cution imm�diate de l'�v�nement Paint
        }

        private void buttonAddPos_Click(object sender, EventArgs e)
        {
            modeLigneActif = false;
            itineraireAuto.EnablePointAdding(false);
            buttonStartItin.Show();
            buttonStartItinBoucle.Show();
            buttonItinPause.Show();
            buttonItinPlay.Show();
            buttonStartItin.Enabled = true;
            buttonStartItinBoucle.Enabled = true;

            itineraireAuto.SauvegarderItineraire(itineraireAuto.GetPoints(), $"Itineraire {DateTime.Now.ToString("yyyyMMddHHmmss")}");
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
            itineraireAuto.EnablePointAdding(false);
            modeLigneActif = false;

            //joystickTimer?.Stop();  // Par exemple, si vous utilisez un Timer pour le d�placement
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
            buttonHistorique.Enabled = true;
            buttonMapLine.Enabled = true;
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
            buttonHistorique.Enabled = false;
            buttonMapLine.Enabled = false;
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

        private void buttonItinPause_Click(object sender, EventArgs e)
        {
            itineraireAuto.PauseMoving();
        }

        private void buttonItinPlay_Click(object sender, EventArgs e)
        {
            itineraireAuto.ResumeMoving();
        }

        private void buttonShowControls_Click(object sender, EventArgs e)
        {
            if (panelControl.Visible == true)
            {
                panelControl.Visible = false;
            }
            else
            {
                panelControl.Visible = true;
            }
        }

        private void buttonHistorique_Click(object sender, EventArgs e)
        {
            historique.AfficherHistorique(itineraireAuto);
            buttonStartItin.Show();
            buttonStartItinBoucle.Show();
            buttonItinPause.Show();
            buttonItinPlay.Show();
        }
        private void pictureBoxMap1_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && modeLigneActif)
            {
                isDrawing = true;
                itineraireAuto.CommencerNouvelItineraire(e.Location);
            }
        }

        private void pictureBoxMap1_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isDrawing && modeLigneActif)
            {
                itineraireAuto.AjouterPointItineraire(e.Location);
                pictureBoxMap1.Invalidate(); // Redessiner si n�cessaire
            }
        }

        private void pictureBoxMap1_MouseUp(object? sender, MouseEventArgs e)
        {
            if (isDrawing && modeLigneActif)
            {
                isDrawing = false;
                modeLigneActif = false; // Optionnellement d�sactiver le mode apr�s avoir dessin� une ligne
            }
        }
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            switch (e.KeyCode)
            {
                case Keys.A: // Replace F1 with your desired key
                    buttonCancel.PerformClick(); // Replace button1 with the actual button you want to click
                    break;
                case Keys.E: // Another key for a different button
                    buttonTurnRight.PerformClick(); // Another button action
                    break;
                case Keys.Z: // Replace F1 with your desired key
                    buttonUp.PerformClick(); // Replace button1 with the actual button you want to click
                    break;
                case Keys.S: // Replace F1 with your desired key
                    buttonDown.PerformClick(); // Replace button1 with the actual button you want to click
                    break;
                case Keys.Q: // Replace F1 with your desired key
                    buttonLeft.PerformClick(); // Replace button1 with the actual button you want to click
                    break;
                case Keys.D: // Replace F1 with your desired key
                    buttonRight.PerformClick(); // Replace button1 with the actual button you want to click
                    break;
                    // Add more cases as needed for other keys and buttons
            }
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            string ipAddress = "10.129.20.130"; // Remplacez par l'adresse IP r�elle
            int port = 2222; // Remplacez par le port r�el
            string password = "mot_de_passe"; // Remplacez par le mot de passe r�el

            Connexion connexion = new Connexion(ipAddress, port, password);
            bool isConnected = connexion.TryConnect();

            if (isConnected)
            {
                MessageBox.Show("Connexion r�ussie !");
                joystick.InitializeControlPacketSender("10.129.20.130", 2222);
                // Ici, vous pouvez continuer avec la logique de votre application
            }
            else
            {
                MessageBox.Show("�chec de la connexion. V�rifiez si le robot est disponible et si les informations sont correctes.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}