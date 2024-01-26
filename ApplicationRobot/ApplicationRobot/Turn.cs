using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationRobot
{
    internal class Turn
    {
        private float angle;

        public Turn()
        {
            angle = 0.0f; // Initialiser l'angle de rotation à 0
        }

        public void TurnLeft(float deltaAngle)
        {
            angle -= deltaAngle; // Tourner à gauche diminue l'angle
            NormalizeAngle();
        }

        public void TurnRight(float deltaAngle)
        {
            angle += deltaAngle; // Tourner à droite augmente l'angle
            NormalizeAngle();
        }

        public float CurrentAngle
        {
            get { return angle; }
        }

        private void NormalizeAngle()
        {
            // S'assurer que l'angle reste entre 0 et 360 degrés
            angle = (angle % 360 + 360) % 360;
        }
    }
}
