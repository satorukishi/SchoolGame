using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace SchoolGame
{
    public partial class frmSettings : Form
    {
        private Team TeamPlayer;
        private const int MAX_RANDOM = 10;
        private int iRandom = 0;
        private bool IsThisWindowClosedByGameWindow = false;
        private bool IsRandomed = false;
        private const string DEFAULT_PROFILE_IMG_PATH = @"img\profile\";
        private const string NO_IMAGE_FILE = "nophoto.jpg";

        public frmSettings()
        {
            InitializeComponent();
        }

        private void frmPlayerSelect_Load(object sender, EventArgs e)
        {
            AddStyle();
        }

        private void AddStyle()
        {
            Font fontDefault = new Font("Segoue UI", 10, FontStyle.Regular);

            cboPlayer.Font = fontDefault;
            cboPlayer.ForeColor = Color.Black;

            txtClassName.Font = fontDefault;
            txtClassName.ForeColor = Color.Black;


            lblPlayerName.Font = fontDefault;
            lblPlayerName.ForeColor = Color.Black;

            lblClassName.Font = fontDefault;
            lblClassName.ForeColor = Color.Black;

            btnStartGame.Visible = false;
        }

        private void cboPlayer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string playerName = cboPlayer.Text.Trim();
                AddPlayer(playerName);
                ResetPlayerName();
            }
        }

        private void ResetPlayerName()
        {
            cboPlayer.Text = string.Empty;
            cboPlayer.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            AddPlayer(cboPlayer.Text.Trim());
            ResetPlayerName();
        }

        private void AddPlayer(string playerName_)
        {
            if (playerName_ != string.Empty)
            {
                if (!cboPlayer.Items.Contains(playerName_))
                {
                    cboPlayer.Items.Add(playerName_);
                }
            }
            else
            {
                cboPlayer.Focus();
            }

            if (cboPlayer.Items.Count > 0 && txtClassName.Text != string.Empty)
            {
                btnStartGame.Visible = true;
            }
        }

        private void txtClassName_TextChanged(object sender, EventArgs e)
        {
            if (txtClassName.Text == string.Empty)
            {
                btnStartGame.Visible = false;
            }
            else if (cboPlayer.Items.Count > 0)
            {
                btnStartGame.Visible = true;
            }
        }

        private void btnStartGame_Click(object sender, EventArgs e)
        {
            string[] players = new string[cboPlayer.Items.Count];
            TeamPlayer = new Team(txtClassName.Text, players);
            picLoading.Visible = true;

            timerSorting.Start();
        }

        private void timerSorting_Tick(object sender, EventArgs e)
        {
            if (IsRandomed && TeamPlayer.IsTurnChanged)
            {
                cboPlayer.SelectedIndex = TeamPlayer.Turn;
                TeamPlayer.IsTurnChanged = false;
            }
            else if (!IsRandomed)
            {
                Random();
                if (++iRandom >= MAX_RANDOM)
                {
                    IsRandomed = true;
                    iRandom = 0;
                    Form frmGame = new frmGame(TeamPlayer);
                    frmGame.Location = new Point(Location.X + Width, Location.Y);
                    picLoading.Visible = false;
                    IsThisWindowClosedByGameWindow = frmGame.ShowDialog() == DialogResult.Cancel;
                }
            }
            if (IsThisWindowClosedByGameWindow)
            {
                this.Close();
            }
        }

        private void frmSettings_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!IsThisWindowClosedByGameWindow)
            {
                DialogResult dr = MessageBox.Show("Do you want to quit?", "School Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dr == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }
        }

        private void cboPlayer_SelectedIndexChanged(object sender, EventArgs e)
        {
            string nomeAluno = cboPlayer.SelectedItem.ToString();
            string imgPath = String.Format(@"{0}{1}\{2}.jpg", DEFAULT_PROFILE_IMG_PATH, txtClassName.Text, nomeAluno);
            if (File.Exists(imgPath))
            {
                picPlayer.Image = Image.FromFile(imgPath);
            }
            else
            {
                picPlayer.Image = Image.FromFile(String.Format(@"{0}{1}", DEFAULT_PROFILE_IMG_PATH, NO_IMAGE_FILE));
            }
            lblSettings.Text = nomeAluno;
        }

        private void Random()
        {
            int quant = cboPlayer.Items.Count;
            Random rnd = new Random(DateTime.Now.Millisecond);
            object temp;
            for (int i = 0; i < quant; i++)
            {
                int random = rnd.Next(i, quant);

                temp = cboPlayer.Items[i];
                cboPlayer.Items[i] = cboPlayer.Items[random];
                cboPlayer.Items[random] = temp;

                cboPlayer.Items.CopyTo(TeamPlayer.Players, 0);
            }

            btnStartGame.Enabled = false;
        }
    }
}
