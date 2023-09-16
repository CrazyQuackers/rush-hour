namespace RushHour
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.instructionsLabel = new System.Windows.Forms.Label();
            this.solveButton = new System.Windows.Forms.Button();
            this.solveMessage = new System.Windows.Forms.Label();
            this.performButton = new System.Windows.Forms.Button();
            this.restartButton = new System.Windows.Forms.Button();
            this.warning = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.warning)).BeginInit();
            this.SuspendLayout();
            // 
            // instructionsLabel
            // 
            this.instructionsLabel.Font = new System.Drawing.Font("Cascadia Mono SemiLight", 18F);
            this.instructionsLabel.Location = new System.Drawing.Point(140, 30);
            this.instructionsLabel.Name = "instructionsLabel";
            this.instructionsLabel.Size = new System.Drawing.Size(632, 135);
            this.instructionsLabel.TabIndex = 0;
            this.instructionsLabel.Text = "Click on the cars to add them to the board          Rotate the car by pressing \'R" +
    "\'         Release the car by pressing \'F\'";
            this.instructionsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // solveButton
            // 
            this.solveButton.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.solveButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.solveButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.solveButton.FlatAppearance.BorderSize = 2;
            this.solveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.solveButton.Font = new System.Drawing.Font("Cascadia Mono SemiLight", 30F);
            this.solveButton.ForeColor = System.Drawing.Color.White;
            this.solveButton.Location = new System.Drawing.Point(1209, 441);
            this.solveButton.Name = "solveButton";
            this.solveButton.Size = new System.Drawing.Size(238, 122);
            this.solveButton.TabIndex = 1;
            this.solveButton.Text = "Solve";
            this.solveButton.UseVisualStyleBackColor = false;
            this.solveButton.Click += new System.EventHandler(this.solveButton_Click);
            // 
            // solveMessage
            // 
            this.solveMessage.Font = new System.Drawing.Font("Cascadia Mono SemiLight", 36F);
            this.solveMessage.Location = new System.Drawing.Point(225, 875);
            this.solveMessage.Name = "solveMessage";
            this.solveMessage.Size = new System.Drawing.Size(451, 70);
            this.solveMessage.TabIndex = 0;
            this.solveMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // performButton
            // 
            this.performButton.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.performButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.performButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.performButton.FlatAppearance.BorderSize = 2;
            this.performButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.performButton.Font = new System.Drawing.Font("Cascadia Mono SemiLight", 19F);
            this.performButton.ForeColor = System.Drawing.Color.White;
            this.performButton.Location = new System.Drawing.Point(810, 229);
            this.performButton.Name = "performButton";
            this.performButton.Size = new System.Drawing.Size(135, 135);
            this.performButton.TabIndex = 1;
            this.performButton.Text = "Perform All Moves";
            this.performButton.UseVisualStyleBackColor = false;
            this.performButton.Visible = false;
            this.performButton.Click += new System.EventHandler(this.performButton_Click);
            // 
            // restartButton
            // 
            this.restartButton.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.restartButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.restartButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.restartButton.FlatAppearance.BorderSize = 2;
            this.restartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.restartButton.Font = new System.Drawing.Font("Cascadia Mono SemiLight", 19F);
            this.restartButton.ForeColor = System.Drawing.Color.White;
            this.restartButton.Location = new System.Drawing.Point(810, 618);
            this.restartButton.Name = "restartButton";
            this.restartButton.Size = new System.Drawing.Size(135, 135);
            this.restartButton.TabIndex = 1;
            this.restartButton.Text = "Restart";
            this.restartButton.UseVisualStyleBackColor = false;
            this.restartButton.Visible = false;
            this.restartButton.Click += new System.EventHandler(this.restartButton_Click);
            // 
            // warning
            // 
            this.warning.BackColor = System.Drawing.Color.CornflowerBlue;
            this.warning.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.warning.Cursor = System.Windows.Forms.Cursors.Hand;
            this.warning.Image = global::RushHour.Properties.Resources.warning;
            this.warning.Location = new System.Drawing.Point(12, 12);
            this.warning.Name = "warning";
            this.warning.Size = new System.Drawing.Size(100, 100);
            this.warning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.warning.TabIndex = 2;
            this.warning.TabStop = false;
            this.warning.Click += new System.EventHandler(this.warning_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.CornflowerBlue;
            this.ClientSize = new System.Drawing.Size(1791, 895);
            this.Controls.Add(this.warning);
            this.Controls.Add(this.restartButton);
            this.Controls.Add(this.performButton);
            this.Controls.Add(this.solveButton);
            this.Controls.Add(this.solveMessage);
            this.Controls.Add(this.instructionsLabel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Rush Hour";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.warning)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label instructionsLabel;
        private System.Windows.Forms.Button solveButton;
        private System.Windows.Forms.Label solveMessage;
        private System.Windows.Forms.Button performButton;
        private System.Windows.Forms.Button restartButton;
        private System.Windows.Forms.PictureBox warning;
    }
}

