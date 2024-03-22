/// La classe Joystick simule un joystick virtuel en utilisant deux PictureBox : une pour le joystick lui-même et l'autre pour sa base.
/// Elle permet de traduire les mouvements de la souris en déplacements du joystick virtuel et calcule l'angle et la magnitude du mouvement.
/// 
/// Caractéristiques principales :
/// - Gestion des interactions utilisateur : Clic et déplacement de la souris sont traduits en mouvements du joystick.
/// - Limitation des mouvements : Les déplacements du joystick sont limités à une zone circulaire définie par MaxRadius.
/// - Deadzone : Un seuil minimal de mouvement (Deadzone) est appliqué pour éviter les mouvements involontaires.
/// - Calcul de direction : La classe fournit des méthodes pour obtenir l'angle et la magnitude du mouvement du joystick, 
///   permettant ainsi une interaction précise et intuitive avec d'autres composants de l'application.
/// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationRobot
{
    internal class Joystick
    {
        public delegate void JoystickValueChangedEventHandler(float angle, float magnitude);
        public event JoystickValueChangedEventHandler JoystickValueChanged;
        private PictureBox pictureBoxJoystickBig;
        private PictureBox pictureBoxJoystickSmall;
        private ControlPacketSender? controlPacketSender = null;

        private bool isDragging = false;
        private Point dragCursorPoint;
        private Point dragPictureBoxPoint;
        private const float MaxRadius = 50.0f; // Rayon maximum du joystick
        private const float Deadzone = 10.0f; // Ajustez la valeur de la deadzone selon les besoins

        public void InitializeControlPacketSender(ControlPacketSender com)
        {
            controlPacketSender = com;
        }
        public void CenterJoystickSmall()
        {
            // Positionner le joystick small au centre du joystick big
            pictureBoxJoystickSmall.Location = new Point(
                (pictureBoxJoystickBig.Width - pictureBoxJoystickSmall.Width) / 2,
                (pictureBoxJoystickBig.Height - pictureBoxJoystickSmall.Height) / 2
            );
        }

        public Joystick(PictureBox big, PictureBox small)
        {
            pictureBoxJoystickBig = big;
            pictureBoxJoystickSmall = small;

            CenterJoystickSmall();

            // Attacher les gestionnaires d'événements
            pictureBoxJoystickSmall.MouseDown += new MouseEventHandler(pictureBoxJoystickSmall_MouseDown);
            pictureBoxJoystickSmall.MouseMove += new MouseEventHandler(pictureBoxJoystickSmall_MouseMove);
            pictureBoxJoystickSmall.MouseUp += new MouseEventHandler(pictureBoxJoystickSmall_MouseUp);
        }
        public void DetachEvents()
        {
            pictureBoxJoystickSmall.MouseDown -= pictureBoxJoystickSmall_MouseDown;
            pictureBoxJoystickSmall.MouseMove -= pictureBoxJoystickSmall_MouseMove;
            pictureBoxJoystickSmall.MouseUp -= pictureBoxJoystickSmall_MouseUp;
        }

        private void pictureBoxJoystickSmall_MouseDown(object? sender, MouseEventArgs e)
        {
            isDragging = true;
            dragCursorPoint = Cursor.Position;
            dragPictureBoxPoint = pictureBoxJoystickSmall.Location;
        }

        private void pictureBoxJoystickSmall_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point diff = Point.Subtract(Cursor.Position, new Size(dragCursorPoint));
                Point newLocation = Point.Add(dragPictureBoxPoint, new Size(diff));

                // Calculer la distance depuis le centre
                Point center = new Point(
                    (pictureBoxJoystickBig.Width - pictureBoxJoystickSmall.Width) / 2,
                    (pictureBoxJoystickBig.Height - pictureBoxJoystickSmall.Height) / 2
                );
                double distance = Math.Sqrt(Math.Pow(newLocation.X - center.X, 2) + Math.Pow(newLocation.Y - center.Y, 2));

                // Limiter le joystick à la zone circulaire
                if (distance > MaxRadius)
                {
                    double ratio = MaxRadius / distance;
                    newLocation = new Point(
                        center.X + (int)((newLocation.X - center.X) * ratio),
                        center.Y + (int)((newLocation.Y - center.Y) * ratio)
                    );
                }
                pictureBoxJoystickSmall.Location = newLocation;
                var (angle, magnitude) = GetDirection();
                if (magnitude > Deadzone)
                {
                    JoystickValueChanged?.Invoke(angle, magnitude);
                }
            }
        }

        private void pictureBoxJoystickSmall_MouseUp(object? sender, MouseEventArgs e)
        {
            isDragging = false;

            CenterJoystickSmall();
            // Remettre le joystick au centre lorsque l'utilisateur lâche le joystick
            pictureBoxJoystickSmall.Location = new Point(
                (pictureBoxJoystickBig.Width - pictureBoxJoystickSmall.Width) / 2,
                (pictureBoxJoystickBig.Height - pictureBoxJoystickSmall.Height) / 2
            );
            var (angle, magnitude) = GetDirection();
            float x = (float)(magnitude * Math.Cos(angle)); // Cast double to float
            float y = (float)(magnitude * Math.Sin(angle)); // Cast double to float
            JoystickValueChanged?.Invoke(angle, magnitude);

            // Send the coordinates
            if (controlPacketSender != null)
            {
                controlPacketSender.SendJoystickUpdate(x, y);
            }

        }

        // Méthode pour obtenir la direction du joystick
        public (float angle, float magnitude) GetDirection()
        {
            Point center = new Point(
                (pictureBoxJoystickBig.Width - pictureBoxJoystickSmall.Width) / 2,
                (pictureBoxJoystickBig.Height - pictureBoxJoystickSmall.Height) / 2
            );
            Point currentPosition = pictureBoxJoystickSmall.Location;

            Point direction = new Point(currentPosition.X - center.X, currentPosition.Y - center.Y);
            float magnitude = (float)Math.Sqrt(direction.X * direction.X + direction.Y * direction.Y);

            // Appliquer la deadzone
            if (magnitude < Deadzone)
            {
                return (0, 0); // Retourner zéro si dans la deadzone
            }

            float angle = (float)Math.Atan2(direction.Y, direction.X); // Arc tangente de Y/X
            return (angle, magnitude);
        }
        public float GetAngle()
        {
            // Utilisez directement les valeurs du tuple retourné par GetDirection
            var (angle, magnitude) = GetDirection(); // Décomposer le tuple

            return angle; // Renvoyer l'angle
        }
        public void SendJoystickData(float x, float y)
        {
            // Implementation to send data to the robot
        }

        public void OnJoystickValueChanged(float angle, float magnitude)
        {
            // Normalize the magnitude to a range of 0 to 1
            float normalizedMagnitude = magnitude / 49.0f;

            // Convert angle from radians to degrees if necessary for readability (optional)
            // float angleInDegrees = angle * (180 / (float)Math.PI);

            // Calculate x and y coordinates with the normalized magnitude
            float x = normalizedMagnitude * (float)Math.Cos(angle); // Cosine for x
            float y = normalizedMagnitude * (float)Math.Sin(angle); // Sine for y

            // Ensure x and y are within the range of -1 to 1
            x = Math.Clamp(x, -1.0f, 1.0f);
            y = Math.Clamp(y, -1.0f, 1.0f);

            // Assuming you want to display these values on a UI, or send them to a robot
            Console.WriteLine($"Joystick X: {x}, Y: {y}");

            // Send the x and y coordinates instead of angle and magnitude
            controlPacketSender?.SendJoystickUpdate(x, y);
        }
    }
}
