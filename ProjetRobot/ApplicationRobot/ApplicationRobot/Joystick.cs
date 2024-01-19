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

                // Limiter le mouvement à l'intérieur de pictureBoxJoystickBig
                newLocation.X = Math.Max(newLocation.X, 0);
                newLocation.Y = Math.Max(newLocation.Y, 0);
                newLocation.X = Math.Min(newLocation.X, pictureBoxJoystickBig.Width - pictureBoxJoystickSmall.Width);
                newLocation.Y = Math.Min(newLocation.Y, pictureBoxJoystickBig.Height - pictureBoxJoystickSmall.Height);

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
        public Point GetDirection()
        {
            Point center = new Point(
                (pictureBoxJoystickBig.Width - pictureBoxJoystickSmall.Width) / 2,
                (pictureBoxJoystickBig.Height - pictureBoxJoystickSmall.Height) / 2
            );
            Point currentPosition = pictureBoxJoystickSmall.Location;

            return new Point(currentPosition.X - center.X, currentPosition.Y - center.Y);
        }
    }
}
