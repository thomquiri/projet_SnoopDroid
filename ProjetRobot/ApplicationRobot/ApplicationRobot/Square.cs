﻿/// La classe Square représente un carré dans l'application et gère son positionnement et ses mouvements.
/// 
/// Cette classe fournit des méthodes pour déplacer le carré dans l'espace 2D, en tenant compte de divers facteurs tels que :
/// - Les angles de rotation, gérés par une instance de la classe Turn.
/// - Les déplacements déclenchés par un joystick virtuel, en tenant compte de l'angle et de la magnitude du mouvement.
/// - La gestion des limites de déplacement pour éviter que le carré ne sorte de l'aire de jeu définie.
/// La classe maintient également une instance de Backroom pour des interactions spécifiques non détaillées ici.
/// 


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationRobot
{
    internal class Square
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Size { get; set; }
        private Turn turnManager;
        private Backroom backroom;


        public Square(int x, int y, int size, Turn turnManager)
        {
            X = x;
            Y = y;
            Size = size;
            this.turnManager = turnManager;
            backroom = new Backroom();
        }

        public void Move(int distance, float angleAdjustment)
        {
            // Convertir l'angle total (angle du carré + ajustement + ajustement face) en radians pour le calcul trigonométrique
            double totalAngleInRadians = (turnManager.CurrentAngle + angleAdjustment ) * -(Math.PI / 180);

            // Calculer le déplacement en X et en Y en fonction de l'angle
            int deltaX = (int)(distance * Math.Cos(totalAngleInRadians));
            int deltaY = (int)(distance * Math.Sin(totalAngleInRadians));

            
            // Appliquer le déplacement
            X += deltaX;
            Y -= deltaY; // Soustraire car l'axe Y est inversé dans la plupart des systèmes graphiques
        }
        public void MoveByJoystick(float angle, float magnitude, float elapsedTime)
        {
            // S'assurer que le mouvement est hors de la deadzone
            if (magnitude == 0)
            {
                return; // Aucun mouvement si la magnitude est zéro
            }

            // Calculer le déplacement maximal autorisé en fonction du temps écoulé
            float maxDisplacement = 50 * elapsedTime; // 50 pixels par seconde

            // Calculer le déplacement en X et en Y en fonction de l'angle
            float displacementX = maxDisplacement * (float)Math.Cos(angle);
            float displacementY = maxDisplacement * (float)Math.Sin(angle);

            // Appliquer le déplacement
            X += (int)Math.Round(displacementX);
            Y += (int)Math.Round(displacementY);

            // Vous pouvez également ajouter une logique pour s'assurer que le carré ne sorte pas de ses limites.
        }
    }
}
