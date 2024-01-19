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

        public Square(int x, int y, int size, Turn turnManager)
        {
            X = x;
            Y = y;
            Size = size;
            this.turnManager = turnManager;
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
        public void MoveByJoystick(Point direction, int sensitivity)
        {
            // Utiliser 'direction.X' et 'direction.Y' pour déterminer comment déplacer le carré.
            // 'sensitivity' est un facteur pour ajuster la quantité de mouvement en fonction de l'entrée du joystick.
            X += direction.X * sensitivity;
            Y += direction.Y * sensitivity;

            // Vous pouvez également ajouter une logique pour s'assurer que le carré ne sorte pas de ses limites.
        }
    }
}
