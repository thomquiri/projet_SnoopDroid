using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationRobot
{
    internal class Joystick
    {
        private PictureBox pictureBoxJoystickBig;
        private PictureBox pictureBoxJoystickSmall;
        private bool isDragging = false;
        private Point dragCursorPoint;
        private Point dragPictureBoxPoint;
        private const float MaxRadius = 50.0f; // Rayon maximum du joystick
        private const float Deadzone = 10.0f; // Ajustez la valeur de la deadzone selon les besoins

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
    }
}
