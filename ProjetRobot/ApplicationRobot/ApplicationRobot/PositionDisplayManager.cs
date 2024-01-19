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
            positionLabel.Text = $"Position: ({x}, {y})";
        }
    }
}
