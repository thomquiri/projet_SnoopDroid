namespace ApplicationRobot
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            pictureBoxMap1 = new PictureBox();
            buttonUp = new Button();
            buttonDown = new Button();
            buttonLeft = new Button();
            buttonRight = new Button();
            labelPosition = new Label();
            buttonTurnLeft = new Button();
            buttonTurnRight = new Button();
            pictureBoxJoystickBig = new PictureBox();
            pictureBoxJoystickSmall = new PictureBox();
            panel1 = new Panel();
            buttonMapClick = new Button();
            buttonAddPos = new Button();
            buttonStartItin = new Button();
            buttonCancel = new Button();
            buttonStartItinBoucle = new Button();
            labelLoopCounter = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBoxMap1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxJoystickBig).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxJoystickSmall).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // pictureBoxMap1
            // 
            pictureBoxMap1.BackgroundImage = Properties.Resources.map;
            pictureBoxMap1.Location = new Point(12, 12);
            pictureBoxMap1.Name = "pictureBoxMap1";
            pictureBoxMap1.Size = new Size(499, 500);
            pictureBoxMap1.TabIndex = 0;
            pictureBoxMap1.TabStop = false;
            // 
            // buttonUp
            // 
            buttonUp.Location = new Point(621, 12);
            buttonUp.Name = "buttonUp";
            buttonUp.Size = new Size(75, 61);
            buttonUp.TabIndex = 1;
            buttonUp.Text = "Haut";
            buttonUp.UseVisualStyleBackColor = true;
            buttonUp.Click += buttonUp_Click;
            // 
            // buttonDown
            // 
            buttonDown.Location = new Point(621, 185);
            buttonDown.Name = "buttonDown";
            buttonDown.Size = new Size(75, 61);
            buttonDown.TabIndex = 2;
            buttonDown.Text = "Bas";
            buttonDown.UseVisualStyleBackColor = true;
            buttonDown.Click += buttonDown_Click;
            // 
            // buttonLeft
            // 
            buttonLeft.Location = new Point(530, 97);
            buttonLeft.Name = "buttonLeft";
            buttonLeft.Size = new Size(75, 61);
            buttonLeft.TabIndex = 3;
            buttonLeft.Text = "Gauche";
            buttonLeft.UseVisualStyleBackColor = true;
            buttonLeft.Click += buttonLeft_Click;
            // 
            // buttonRight
            // 
            buttonRight.Location = new Point(719, 97);
            buttonRight.Name = "buttonRight";
            buttonRight.Size = new Size(75, 61);
            buttonRight.TabIndex = 4;
            buttonRight.Text = "Droite";
            buttonRight.UseVisualStyleBackColor = true;
            buttonRight.Click += buttonRight_Click;
            // 
            // labelPosition
            // 
            labelPosition.AutoSize = true;
            labelPosition.Location = new Point(530, 431);
            labelPosition.Name = "labelPosition";
            labelPosition.Size = new Size(100, 15);
            labelPosition.TabIndex = 5;
            labelPosition.Text = "Position actuelle :";
            labelPosition.Click += labelPosition_Click;
            // 
            // buttonTurnLeft
            // 
            buttonTurnLeft.Location = new Point(530, 12);
            buttonTurnLeft.Name = "buttonTurnLeft";
            buttonTurnLeft.Size = new Size(75, 61);
            buttonTurnLeft.TabIndex = 6;
            buttonTurnLeft.Text = "Tourner vers la gauche";
            buttonTurnLeft.UseVisualStyleBackColor = true;
            buttonTurnLeft.Click += buttonTurnLeft_Click;
            // 
            // buttonTurnRight
            // 
            buttonTurnRight.Location = new Point(719, 12);
            buttonTurnRight.Name = "buttonTurnRight";
            buttonTurnRight.Size = new Size(75, 61);
            buttonTurnRight.TabIndex = 7;
            buttonTurnRight.Text = "Tourner vers la droite";
            buttonTurnRight.UseVisualStyleBackColor = true;
            buttonTurnRight.Click += buttonTurnRight_Click;
            // 
            // pictureBoxJoystickBig
            // 
            pictureBoxJoystickBig.BackgroundImage = (Image)resources.GetObject("pictureBoxJoystickBig.BackgroundImage");
            pictureBoxJoystickBig.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBoxJoystickBig.Location = new Point(0, 0);
            pictureBoxJoystickBig.Name = "pictureBoxJoystickBig";
            pictureBoxJoystickBig.Size = new Size(97, 98);
            pictureBoxJoystickBig.TabIndex = 8;
            pictureBoxJoystickBig.TabStop = false;
            // 
            // pictureBoxJoystickSmall
            // 
            pictureBoxJoystickSmall.BackColor = Color.Transparent;
            pictureBoxJoystickSmall.BackgroundImage = Properties.Resources.carreBleu;
            pictureBoxJoystickSmall.BackgroundImageLayout = ImageLayout.Stretch;
            pictureBoxJoystickSmall.Location = new Point(33, 37);
            pictureBoxJoystickSmall.Name = "pictureBoxJoystickSmall";
            pictureBoxJoystickSmall.Size = new Size(28, 29);
            pictureBoxJoystickSmall.TabIndex = 9;
            pictureBoxJoystickSmall.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(pictureBoxJoystickSmall);
            panel1.Controls.Add(pictureBoxJoystickBig);
            panel1.Location = new Point(611, 79);
            panel1.Name = "panel1";
            panel1.Size = new Size(102, 100);
            panel1.TabIndex = 10;
            // 
            // buttonMapClick
            // 
            buttonMapClick.Location = new Point(530, 291);
            buttonMapClick.Name = "buttonMapClick";
            buttonMapClick.Size = new Size(111, 39);
            buttonMapClick.TabIndex = 11;
            buttonMapClick.Text = "Placer un ou plusieurs points";
            buttonMapClick.UseVisualStyleBackColor = true;
            buttonMapClick.Click += buttonMapClick_Click;
            // 
            // buttonAddPos
            // 
            buttonAddPos.Location = new Point(666, 291);
            buttonAddPos.Name = "buttonAddPos";
            buttonAddPos.Size = new Size(108, 39);
            buttonAddPos.TabIndex = 12;
            buttonAddPos.Text = "Terminer";
            buttonAddPos.UseVisualStyleBackColor = true;
            buttonAddPos.Visible = false;
            buttonAddPos.Click += buttonAddPos_Click;
            // 
            // buttonStartItin
            // 
            buttonStartItin.Location = new Point(666, 348);
            buttonStartItin.Name = "buttonStartItin";
            buttonStartItin.Size = new Size(108, 43);
            buttonStartItin.TabIndex = 13;
            buttonStartItin.Text = "Suivre l'itinéraire une fois";
            buttonStartItin.UseVisualStyleBackColor = true;
            buttonStartItin.Visible = false;
            buttonStartItin.Click += buttonStartItin_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(684, 620);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(111, 39);
            buttonCancel.TabIndex = 14;
            buttonCancel.Text = "Annuler";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonStartItinBoucle
            // 
            buttonStartItinBoucle.Location = new Point(530, 348);
            buttonStartItinBoucle.Name = "buttonStartItinBoucle";
            buttonStartItinBoucle.Size = new Size(111, 43);
            buttonStartItinBoucle.TabIndex = 15;
            buttonStartItinBoucle.Text = "Suivre l'itinéraire en continue";
            buttonStartItinBoucle.UseVisualStyleBackColor = true;
            buttonStartItinBoucle.Visible = false;
            buttonStartItinBoucle.Click += buttonStartItinBoucle_Click;
            // 
            // labelLoopCounter
            // 
            labelLoopCounter.AutoSize = true;
            labelLoopCounter.Location = new Point(530, 458);
            labelLoopCounter.Name = "labelLoopCounter";
            labelLoopCounter.Size = new Size(107, 15);
            labelLoopCounter.TabIndex = 16;
            labelLoopCounter.Text = "Nombre de tour : 0";
            labelLoopCounter.Click += labelLoopCounter_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(807, 671);
            Controls.Add(labelLoopCounter);
            Controls.Add(buttonStartItinBoucle);
            Controls.Add(buttonCancel);
            Controls.Add(buttonStartItin);
            Controls.Add(buttonAddPos);
            Controls.Add(buttonMapClick);
            Controls.Add(panel1);
            Controls.Add(buttonTurnRight);
            Controls.Add(buttonTurnLeft);
            Controls.Add(labelPosition);
            Controls.Add(buttonRight);
            Controls.Add(buttonLeft);
            Controls.Add(buttonDown);
            Controls.Add(buttonUp);
            Controls.Add(pictureBoxMap1);
            Name = "Form1";
            Text = "Application Robot";
            ((System.ComponentModel.ISupportInitialize)pictureBoxMap1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxJoystickBig).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxJoystickSmall).EndInit();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox pictureBoxMap1;
        private Button buttonUp;
        private Button buttonDown;
        private Button buttonLeft;
        private Button buttonRight;
        private Label labelPosition;
        private Button buttonTurnLeft;
        private Button buttonTurnRight;
        private PictureBox pictureBoxJoystickBig;
        private PictureBox pictureBoxJoystickSmall;
        private Panel panel1;
        private Button buttonMapClick;
        private Button buttonAddPos;
        private Button buttonStartItin;
        private Button buttonCancel;
        private Button buttonStartItinBoucle;
        private Label labelLoopCounter;
    }
}