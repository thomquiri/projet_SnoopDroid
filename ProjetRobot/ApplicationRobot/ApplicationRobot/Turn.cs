/// La classe Turn gère la rotation d'un objet en fournissant des méthodes pour effectuer des rotations à gauche ou à droite.
/// 
/// Caractéristiques principales :
/// - Contrôle de Rotation : Permet de modifier l'angle de rotation de l'objet en appliquant des rotations à gauche ou à droite.
/// - Normalisation de l'Angle : Assure que l'angle de rotation reste dans une plage de 0 à 360 degrés pour éviter les débordements et simplifier les calculs.
/// - Accès à l'Angle Actuel : Fournit une propriété pour accéder à l'angle de rotation actuel de l'objet, permettant d'autres calculs ou affichages basés sur cet angle.
/// 

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
