namespace SchoolGame
{
    partial class frmSettings
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.lblSettings = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lblPlayerName = new System.Windows.Forms.Label();
            this.cboPlayer = new System.Windows.Forms.ComboBox();
            this.lblClassName = new System.Windows.Forms.Label();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.btnStartGame = new System.Windows.Forms.Button();
            this.picLoading = new System.Windows.Forms.PictureBox();
            this.timerSorting = new System.Windows.Forms.Timer(this.components);
            this.picPlayer = new System.Windows.Forms.PictureBox();
            this.lblDeveloped = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picLoading)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPlayer)).BeginInit();
            this.SuspendLayout();
            // 
            // lblSettings
            // 
            this.lblSettings.BackColor = System.Drawing.Color.Transparent;
            this.lblSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSettings.Location = new System.Drawing.Point(-1, 27);
            this.lblSettings.Name = "lblSettings";
            this.lblSettings.Size = new System.Drawing.Size(285, 40);
            this.lblSettings.TabIndex = 0;
            this.lblSettings.Text = "Settings";
            this.lblSettings.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(186, 377);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(86, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "&Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lblPlayerName
            // 
            this.lblPlayerName.AutoSize = true;
            this.lblPlayerName.Location = new System.Drawing.Point(12, 357);
            this.lblPlayerName.Name = "lblPlayerName";
            this.lblPlayerName.Size = new System.Drawing.Size(70, 13);
            this.lblPlayerName.TabIndex = 4;
            this.lblPlayerName.Text = "Player Name:";
            // 
            // cboPlayer
            // 
            this.cboPlayer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.Simple;
            this.cboPlayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboPlayer.FormattingEnabled = true;
            this.cboPlayer.Location = new System.Drawing.Point(12, 379);
            this.cboPlayer.Name = "cboPlayer";
            this.cboPlayer.Size = new System.Drawing.Size(168, 254);
            this.cboPlayer.TabIndex = 1;
            this.cboPlayer.SelectedIndexChanged += new System.EventHandler(this.cboPlayer_SelectedIndexChanged);
            this.cboPlayer.KeyDown += new System.Windows.Forms.KeyEventHandler(this.cboPlayer_KeyDown);
            // 
            // lblClassName
            // 
            this.lblClassName.AutoSize = true;
            this.lblClassName.Location = new System.Drawing.Point(12, 135);
            this.lblClassName.Name = "lblClassName";
            this.lblClassName.Size = new System.Drawing.Size(66, 13);
            this.lblClassName.TabIndex = 7;
            this.lblClassName.Text = "Class Name:";
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(12, 158);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(168, 20);
            this.txtClassName.TabIndex = 0;
            this.txtClassName.TextChanged += new System.EventHandler(this.txtClassName_TextChanged);
            // 
            // btnStartGame
            // 
            this.btnStartGame.Font = new System.Drawing.Font("Segoe UI Semibold", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartGame.Location = new System.Drawing.Point(12, 639);
            this.btnStartGame.Name = "btnStartGame";
            this.btnStartGame.Size = new System.Drawing.Size(168, 49);
            this.btnStartGame.TabIndex = 3;
            this.btnStartGame.Text = "Start Game";
            this.btnStartGame.UseVisualStyleBackColor = true;
            this.btnStartGame.Click += new System.EventHandler(this.btnStartGame_Click);
            // 
            // picLoading
            // 
            this.picLoading.BackColor = System.Drawing.Color.Transparent;
            this.picLoading.Image = global::SchoolGame.Properties.Resources.loading;
            this.picLoading.ImageLocation = "";
            this.picLoading.Location = new System.Drawing.Point(186, 654);
            this.picLoading.Name = "picLoading";
            this.picLoading.Size = new System.Drawing.Size(16, 16);
            this.picLoading.TabIndex = 8;
            this.picLoading.TabStop = false;
            this.picLoading.Visible = false;
            // 
            // timerSorting
            // 
            this.timerSorting.Tick += new System.EventHandler(this.timerSorting_Tick);
            // 
            // picPlayer
            // 
            this.picPlayer.BackColor = System.Drawing.Color.Transparent;
            this.picPlayer.Location = new System.Drawing.Point(80, 206);
            this.picPlayer.Name = "picPlayer";
            this.picPlayer.Size = new System.Drawing.Size(100, 100);
            this.picPlayer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPlayer.TabIndex = 9;
            this.picPlayer.TabStop = false;
            // 
            // lblDeveloped
            // 
            this.lblDeveloped.AutoSize = true;
            this.lblDeveloped.BackColor = System.Drawing.Color.Transparent;
            this.lblDeveloped.Location = new System.Drawing.Point(18, 9);
            this.lblDeveloped.Name = "lblDeveloped";
            this.lblDeveloped.Size = new System.Drawing.Size(246, 13);
            this.lblDeveloped.TabIndex = 10;
            this.lblDeveloped.Text = "Developed by teacher Satoru Kishi - Nov/21/2012";
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImage = global::SchoolGame.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(284, 700);
            this.Controls.Add(this.lblDeveloped);
            this.Controls.Add(this.picPlayer);
            this.Controls.Add(this.picLoading);
            this.Controls.Add(this.btnStartGame);
            this.Controls.Add(this.lblClassName);
            this.Controls.Add(this.txtClassName);
            this.Controls.Add(this.cboPlayer);
            this.Controls.Add(this.lblPlayerName);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lblSettings);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(300, 738);
            this.Name = "frmSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Player Select";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSettings_FormClosing);
            this.Load += new System.EventHandler(this.frmPlayerSelect_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picLoading)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picPlayer)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblSettings;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lblPlayerName;
        private System.Windows.Forms.ComboBox cboPlayer;
        private System.Windows.Forms.Label lblClassName;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.Button btnStartGame;
        private System.Windows.Forms.PictureBox picLoading;
        private System.Windows.Forms.Timer timerSorting;
        private System.Windows.Forms.PictureBox picPlayer;
        private System.Windows.Forms.Label lblDeveloped;
    }
}