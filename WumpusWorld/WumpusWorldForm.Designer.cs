namespace WumpusWorld
{
    partial class WumpusWorldForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WumpusWorldForm));
            this.worldPictureBox = new System.Windows.Forms.PictureBox();
            this.settingsGroupBox = new System.Windows.Forms.GroupBox();
            this.revealGridCheckBox = new System.Windows.Forms.CheckBox();
            this.stepButton = new System.Windows.Forms.Button();
            this.showWarningsCheckBox = new System.Windows.Forms.CheckBox();
            this.pitDensityLabel = new System.Windows.Forms.Label();
            this.startButton = new System.Windows.Forms.Button();
            this.setButton = new System.Windows.Forms.Button();
            this.aiModeRadioButton = new System.Windows.Forms.RadioButton();
            this.playerModeRadioButton = new System.Windows.Forms.RadioButton();
            this.pitDensityTrackBar = new System.Windows.Forms.TrackBar();
            this.riskyModeCheckBox = new System.Windows.Forms.CheckBox();
            this.InterfaceTimer = new System.Windows.Forms.Timer(this.components);
            this.gameInfoGroupBox = new System.Windows.Forms.GroupBox();
            this.messagesListBox = new System.Windows.Forms.ListBox();
            this.pointsLabel = new System.Windows.Forms.Label();
            this.arrowsLabel = new System.Windows.Forms.Label();
            this.timeLabel = new System.Windows.Forms.Label();
            this.GameTimer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.worldPictureBox)).BeginInit();
            this.settingsGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pitDensityTrackBar)).BeginInit();
            this.gameInfoGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // worldPictureBox
            // 
            this.worldPictureBox.Location = new System.Drawing.Point(154, 12);
            this.worldPictureBox.Name = "worldPictureBox";
            this.worldPictureBox.Size = new System.Drawing.Size(501, 501);
            this.worldPictureBox.TabIndex = 0;
            this.worldPictureBox.TabStop = false;
            // 
            // settingsGroupBox
            // 
            this.settingsGroupBox.Controls.Add(this.revealGridCheckBox);
            this.settingsGroupBox.Controls.Add(this.stepButton);
            this.settingsGroupBox.Controls.Add(this.showWarningsCheckBox);
            this.settingsGroupBox.Controls.Add(this.pitDensityLabel);
            this.settingsGroupBox.Controls.Add(this.startButton);
            this.settingsGroupBox.Controls.Add(this.setButton);
            this.settingsGroupBox.Controls.Add(this.aiModeRadioButton);
            this.settingsGroupBox.Controls.Add(this.playerModeRadioButton);
            this.settingsGroupBox.Controls.Add(this.pitDensityTrackBar);
            this.settingsGroupBox.Controls.Add(this.riskyModeCheckBox);
            this.settingsGroupBox.Location = new System.Drawing.Point(3, 242);
            this.settingsGroupBox.Name = "settingsGroupBox";
            this.settingsGroupBox.Size = new System.Drawing.Size(145, 269);
            this.settingsGroupBox.TabIndex = 1;
            this.settingsGroupBox.TabStop = false;
            this.settingsGroupBox.Text = "Settings";
            // 
            // revealGridCheckBox
            // 
            this.revealGridCheckBox.AutoSize = true;
            this.revealGridCheckBox.Checked = true;
            this.revealGridCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.revealGridCheckBox.Location = new System.Drawing.Point(28, 38);
            this.revealGridCheckBox.Name = "revealGridCheckBox";
            this.revealGridCheckBox.Size = new System.Drawing.Size(82, 17);
            this.revealGridCheckBox.TabIndex = 11;
            this.revealGridCheckBox.Text = "Reveal Grid";
            this.revealGridCheckBox.UseVisualStyleBackColor = true;
            // 
            // stepButton
            // 
            this.stepButton.Location = new System.Drawing.Point(36, 238);
            this.stepButton.Name = "stepButton";
            this.stepButton.Size = new System.Drawing.Size(75, 23);
            this.stepButton.TabIndex = 10;
            this.stepButton.Text = "Step";
            this.stepButton.UseVisualStyleBackColor = true;
            this.stepButton.Click += new System.EventHandler(this.stepButton_Click);
            // 
            // showWarningsCheckBox
            // 
            this.showWarningsCheckBox.AutoSize = true;
            this.showWarningsCheckBox.Checked = true;
            this.showWarningsCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.showWarningsCheckBox.Location = new System.Drawing.Point(28, 56);
            this.showWarningsCheckBox.Name = "showWarningsCheckBox";
            this.showWarningsCheckBox.Size = new System.Drawing.Size(101, 17);
            this.showWarningsCheckBox.TabIndex = 9;
            this.showWarningsCheckBox.Text = "Show Warnings";
            this.showWarningsCheckBox.UseVisualStyleBackColor = true;
            // 
            // pitDensityLabel
            // 
            this.pitDensityLabel.AutoSize = true;
            this.pitDensityLabel.Location = new System.Drawing.Point(23, 121);
            this.pitDensityLabel.Name = "pitDensityLabel";
            this.pitDensityLabel.Size = new System.Drawing.Size(57, 13);
            this.pitDensityLabel.TabIndex = 7;
            this.pitDensityLabel.Text = "Pit Density";
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(36, 209);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 23);
            this.startButton.TabIndex = 5;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // setButton
            // 
            this.setButton.Location = new System.Drawing.Point(36, 180);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(75, 23);
            this.setButton.TabIndex = 4;
            this.setButton.Text = "Set";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // aiModeRadioButton
            // 
            this.aiModeRadioButton.AutoSize = true;
            this.aiModeRadioButton.Checked = true;
            this.aiModeRadioButton.Location = new System.Drawing.Point(28, 80);
            this.aiModeRadioButton.Name = "aiModeRadioButton";
            this.aiModeRadioButton.Size = new System.Drawing.Size(65, 17);
            this.aiModeRadioButton.TabIndex = 2;
            this.aiModeRadioButton.TabStop = true;
            this.aiModeRadioButton.Text = "AI Mode";
            this.aiModeRadioButton.UseVisualStyleBackColor = true;
            // 
            // playerModeRadioButton
            // 
            this.playerModeRadioButton.AutoSize = true;
            this.playerModeRadioButton.Location = new System.Drawing.Point(27, 98);
            this.playerModeRadioButton.Name = "playerModeRadioButton";
            this.playerModeRadioButton.Size = new System.Drawing.Size(84, 17);
            this.playerModeRadioButton.TabIndex = 1;
            this.playerModeRadioButton.Text = "Player Mode";
            this.playerModeRadioButton.UseVisualStyleBackColor = true;
            // 
            // pitDensityTrackBar
            // 
            this.pitDensityTrackBar.LargeChange = 1;
            this.pitDensityTrackBar.Location = new System.Drawing.Point(18, 137);
            this.pitDensityTrackBar.Maximum = 3;
            this.pitDensityTrackBar.Name = "pitDensityTrackBar";
            this.pitDensityTrackBar.Size = new System.Drawing.Size(85, 45);
            this.pitDensityTrackBar.TabIndex = 0;
            // 
            // riskyModeCheckBox
            // 
            this.riskyModeCheckBox.AutoSize = true;
            this.riskyModeCheckBox.Location = new System.Drawing.Point(28, 20);
            this.riskyModeCheckBox.Name = "riskyModeCheckBox";
            this.riskyModeCheckBox.Size = new System.Drawing.Size(82, 17);
            this.riskyModeCheckBox.TabIndex = 8;
            this.riskyModeCheckBox.Text = "Risky Mode";
            this.riskyModeCheckBox.UseVisualStyleBackColor = true;
            // 
            // InterfaceTimer
            // 
            this.InterfaceTimer.Interval = 500;
            this.InterfaceTimer.Tick += new System.EventHandler(this.InterfaceTimer_Tick);
            // 
            // gameInfoGroupBox
            // 
            this.gameInfoGroupBox.Controls.Add(this.messagesListBox);
            this.gameInfoGroupBox.Controls.Add(this.pointsLabel);
            this.gameInfoGroupBox.Controls.Add(this.arrowsLabel);
            this.gameInfoGroupBox.Controls.Add(this.timeLabel);
            this.gameInfoGroupBox.Location = new System.Drawing.Point(3, 6);
            this.gameInfoGroupBox.Name = "gameInfoGroupBox";
            this.gameInfoGroupBox.Size = new System.Drawing.Size(145, 230);
            this.gameInfoGroupBox.TabIndex = 3;
            this.gameInfoGroupBox.TabStop = false;
            this.gameInfoGroupBox.Text = "Game Info";
            // 
            // messagesListBox
            // 
            this.messagesListBox.FormattingEnabled = true;
            this.messagesListBox.Location = new System.Drawing.Point(6, 69);
            this.messagesListBox.Name = "messagesListBox";
            this.messagesListBox.Size = new System.Drawing.Size(133, 121);
            this.messagesListBox.TabIndex = 14;
            // 
            // pointsLabel
            // 
            this.pointsLabel.AutoSize = true;
            this.pointsLabel.Location = new System.Drawing.Point(11, 15);
            this.pointsLabel.Name = "pointsLabel";
            this.pointsLabel.Size = new System.Drawing.Size(42, 13);
            this.pointsLabel.TabIndex = 13;
            this.pointsLabel.Text = "Points: ";
            // 
            // arrowsLabel
            // 
            this.arrowsLabel.AutoSize = true;
            this.arrowsLabel.Location = new System.Drawing.Point(11, 29);
            this.arrowsLabel.Name = "arrowsLabel";
            this.arrowsLabel.Size = new System.Drawing.Size(45, 13);
            this.arrowsLabel.TabIndex = 12;
            this.arrowsLabel.Text = "Arrows: ";
            // 
            // timeLabel
            // 
            this.timeLabel.AutoSize = true;
            this.timeLabel.Location = new System.Drawing.Point(11, 43);
            this.timeLabel.Name = "timeLabel";
            this.timeLabel.Size = new System.Drawing.Size(36, 13);
            this.timeLabel.TabIndex = 11;
            this.timeLabel.Text = "Time: ";
            // 
            // GameTimer
            // 
            this.GameTimer.Interval = 1000;
            this.GameTimer.Tick += new System.EventHandler(this.GameTimer_Tick);
            // 
            // WumpusWorldForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 512);
            this.Controls.Add(this.gameInfoGroupBox);
            this.Controls.Add(this.settingsGroupBox);
            this.Controls.Add(this.worldPictureBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(675, 550);
            this.Name = "WumpusWorldForm";
            this.Text = "www.prozhe.com Wumpus World";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.WumpusWorldForm_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.worldPictureBox)).EndInit();
            this.settingsGroupBox.ResumeLayout(false);
            this.settingsGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pitDensityTrackBar)).EndInit();
            this.gameInfoGroupBox.ResumeLayout(false);
            this.gameInfoGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox worldPictureBox;
        private System.Windows.Forms.GroupBox settingsGroupBox;
        private System.Windows.Forms.RadioButton aiModeRadioButton;
        private System.Windows.Forms.RadioButton playerModeRadioButton;
        private System.Windows.Forms.TrackBar pitDensityTrackBar;
        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.Button setButton;
        private System.Windows.Forms.Label pitDensityLabel;
        private System.Windows.Forms.Timer InterfaceTimer;
        private System.Windows.Forms.CheckBox riskyModeCheckBox;
        private System.Windows.Forms.CheckBox showWarningsCheckBox;
        private System.Windows.Forms.Button stepButton;
        private System.Windows.Forms.GroupBox gameInfoGroupBox;
        private System.Windows.Forms.Label pointsLabel;
        private System.Windows.Forms.Label arrowsLabel;
        private System.Windows.Forms.Label timeLabel;
        private System.Windows.Forms.ListBox messagesListBox;
        private System.Windows.Forms.Timer GameTimer;
        private System.Windows.Forms.CheckBox revealGridCheckBox;
    }
}

