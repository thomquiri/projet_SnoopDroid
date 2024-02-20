/// La classe PositionDisplayManager est responsable de la mise à jour de l'affichage de la position actuelle d'un objet dans l'interface utilisateur.
/// 
/// Caractéristiques principales :
/// - Gestion de l'Affichage : Met à jour un contrôle Label dans l'interface utilisateur pour afficher les coordonnées actuelles d'un objet.
/// - Simplicité et Modularité : Peut être facilement intégrée avec différentes parties de l'application nécessitant l'affichage de positions, 
///   rendant le code plus modulaire et réutilisable.
/// - Mise à jour en Temps Réel : Conçue pour mettre à jour l'affichage en temps réel, offrant un retour visuel immédiat sur la position de l'objet suivi.
/// 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationRobot
{
    internal class PositionDisplayManager
    {
        private Label positionLabel;

        public PositionDisplayManager(Label positionLabel)
        {
            this.positionLabel = positionLabel;
        }

        public void UpdatePosition(int x, int y)
        {
            // Si vous modifiez le contrôle d'un autre thread, utilisez Invoke
            if (positionLabel.InvokeRequired)
            {
                positionLabel.Invoke(new Action(() => positionLabel.Text = $"Position: ({x}, {y})"));
            }
            else
            {
                positionLabel.Text = $"Position: ({x}, {y})";
            }
        }
    }
}
