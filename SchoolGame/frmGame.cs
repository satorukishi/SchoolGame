using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Media;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Diagnostics;

namespace SchoolGame
{
    public partial class frmGame : Form
    {
        private const int ALTURA_MAXIMA_SALTO = 380;
        private const int HEIGHT = 700;
        private const int TAB_MARIO = 144;
        private const int TAB_QUESTION = 20;
        private const int WIDTH = 2016;
        private const int WIDTH_HALF = 1008;
        private const int WINDOWY_BORDER = 38;
        private const int WINDOWX_BORDER = 16;
        private const string GAME_OVER = "GAME OVER";
        private const string GAME_START = "PLEASE SELECT A CHARACTER";
        private const string MSG_END_GAME = "THANK YOU {0}!\r\n\r\nYOUR QUEST IS OVER.\r\n\r\nCLICK TO RESTART THE GAME";

        private readonly Color COR_CEU = Color.CornflowerBlue;
        private readonly Color COR_CASTELO = Color.Black;


        private Character mario;
        private List<Question> questions;
        private XmlNodeList xnPlayer;
        private XmlNodeList xnQuestion;
        private int questionIndex = -1;
        SoundPlayer sPlayer = new SoundPlayer();
        private bool IsMarioSubindo;
        private bool IsKoopaSubindo;
        private CharacterSituation initialSituation;
        private Team team;

        private frmGame()
        {
            InitializeComponent();
        }

        public frmGame(Team team_)
            : this()
        {
            team = team_;
        }

        private void frmGame_Load(object sender, EventArgs e)
        {
            try
            {
                Start();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.StackTrace);
                throw;
            }

        }

        private void frmGame_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult dr = MessageBox.Show("Do you want to quit?", "School Game", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                sPlayer.Stop();
                sPlayer.Dispose();
            }
        }

        private void lblAnswer_Click(object sender, EventArgs e)
        {
            Label l = (Label)sender;
            if (l.ForeColor == Color.White)
            {
                for (int i = 0; i < questions[questionIndex].Answers.Count; i++)
                {
                    Answer a = questions[questionIndex].Answers[i];
                    if (l.Text == a.Text)
                    {
                        if (a.IsCorrect)
                        {
                            NextStage();
                            break;
                        }
                        else
                        {
                            WrongAnswer(l);
                            break;
                        }
                    }
                }
                //if (l == lblAnswer1 && questions[questionIndex].Answers[0].IsCorrect)
                //{
                //    NextStage();
                //}
                //else if (l == lblAnswer2 && questions[questionIndex].Answers[1].IsCorrect)
                //{
                //    NextStage();
                //}
                //else if (l == lblAnswer3 && questions[questionIndex].Answers[2].IsCorrect)
                //{
                //    NextStage();
                //}
                //else if (l == lblAnswer4 && questions[questionIndex].Answers[3].IsCorrect)
                //{
                //    NextStage();
                //}
                //else if (l == lblAnswer5 && questions[questionIndex].Answers[4].IsCorrect)
                //{
                //    NextStage();
                //}
                //else
                //{
                //    WrongAnswer(l);
                //}
            }
        }

        private void lblAnswer_MouseEnter(object sender, EventArgs e)
        {
            lblAnswer1.BorderStyle = BorderStyle.None;
            lblAnswer2.BorderStyle = BorderStyle.None;
            lblAnswer3.BorderStyle = BorderStyle.None;
            lblAnswer4.BorderStyle = BorderStyle.None;
            lblAnswer5.BorderStyle = BorderStyle.None;

            Label l = (Label)sender;
            l.BorderStyle = BorderStyle.Fixed3D;
        }

        private void lblAnswer_MouseLeave(object sender, EventArgs e)
        {
            Label l = (Label)sender;
            l.BorderStyle = BorderStyle.None;
        }

        private void lblMensagem_StartClick(object sender, EventArgs e)
        {
            Restart();
        }

        private void picCharacter_Click(object sender, EventArgs e)
        {
            picMario.Click -= picCharacter_Click;
            picPrincess.Click -= picCharacter_Click;
            if (sender == picMario)
            {
                mario.Name = CharacterName.Mario;
            }
            else if (sender == picPrincess)
            {
                mario.Name = CharacterName.Luigi;
            }

            picPrincess.Visible = false;
            picMario.Image = mario.Show();
            NextStage();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //Se tiver na tela de GameStatus()
            if (pnlPreto.Visible == true)
            {
                if (lblMensagem.Text == GAME_OVER)
                {
                    PlayMusic("SMB_gameover.wav", 5000);
                    timer.Stop();
                    Restart();
                }
                else
                {
                    team.NextTurn();

                    ShowQuestion();
                    CopyToInfoToFirstHalfPanel();
                    HideGameStatus();
                }
                timer.Stop();
            }
            // Se Mario esta andando
            else if (mario.Action == CharacterAction.Run)
            {
                picMario.Location = new Point(picMario.Location.X + 4, picMario.Location.Y);

                if (picMario.Location.X >= WIDTH_HALF + TAB_MARIO)
                {
                    mario.Action = CharacterAction.Stop;
                    picMario.Image = mario.Show();
                    timerCharacter.Stop();

                    if (questionIndex == questions.Count)
                    {
                        timer.Stop();
                        Adjustment();
                    }
                }


                if (questionIndex == questions.Count &&
                    pnlFase.Location.X <= WIDTH_HALF * -1)
                {
                    timer.Stop();
                    picCasco.Visible = true;
                }
                else if (picMario.Location.X >= WIDTH_HALF / 4)
                {
                    pnlFase.Location = new Point(pnlFase.Location.X - 4, pnlFase.Location.Y);
                }
            }
            // Se Panel esta andando mesmo depois que o Mario já parou
            else if (pnlFase.Location.X < 0)
            {
                pnlFase.Location = new Point(pnlFase.Location.X - 4, pnlFase.Location.Y);
                if (pnlFase.Location.X == WIDTH_HALF * -1)
                {
                    Adjustment();
                    CopyToInfoToFirstHalfPanel();
                    timer.Stop();
                    picCasco.Visible = true;
                }
            }
            // Salvou o jogo
            else if (questionIndex == questions.Count)
            {
                // Se koopa subiu até o limite
                if (picKoopa.Location.Y <= ALTURA_MAXIMA_SALTO && IsKoopaSubindo)
                {
                    System.Threading.Thread.Sleep(100);
                    IsKoopaSubindo = false;
                    picKoopa.Image = Image.FromFile(@"img\koopa_180.PNG");
                    picKoopa.Location = new Point(picKoopa.Location.X, picKoopa.Location.Y + 4);
                }
                else if (IsKoopaSubindo)
                {
                    picKoopa.Location = new Point(picKoopa.Location.X, picKoopa.Location.Y - 4);
                }
                else if (picKoopa.Location.Y >= HEIGHT)
                {
                    System.Threading.Thread.Sleep(2500);
                    CharWalk();
                }
                else if (!IsKoopaSubindo && questionIndex == questions.Count)
                {
                    picKoopa.Location = new Point(picKoopa.Location.X, picKoopa.Location.Y + 4);
                }
            }
            // Se casco acertou Mario
            else if (picCasco.Location.X == picMario.Location.X + 24)
            {
                if (mario.Situation == CharacterSituation.Flower)
                {
                    team.NextTurn();
                    ChangeMarioImage(CharacterSituation.Big);
                    PlayMusic("SMB_small.wav", 800, false, true);
                }
                else if (mario.Situation == CharacterSituation.Big)
                {
                    team.NextTurn();
                    ChangeMarioImage(CharacterSituation.Small);
                    PlayMusic("SMB_small.wav", 800, false, true);
                }
                else if (mario.Situation == CharacterSituation.Small)
                {
                    //Se o Mario estiver pequeno não devemos mudar o turno
                    ChangeMarioImage(CharacterSituation.Dead);
                    IsMarioSubindo = true;
                    PlayMusic("SMB_dead.wav", 400, false, false);
                }

                picCasco.Location = new Point(picCasco.Location.X - 4, picCasco.Location.Y);
            }
            // Se Mario morreu
            else if (mario.Situation == CharacterSituation.Dead)
            {
                picCasco.Location = new Point(WIDTH_HALF, picMario.Location.Y + 48);
                // Mario sobe
                if (IsMarioSubindo)
                {
                    picMario.Location = new Point(picMario.Location.X, picMario.Location.Y - 7);
                    if (picMario.Location.Y <= ALTURA_MAXIMA_SALTO)
                    {
                        System.Threading.Thread.Sleep(100);
                        IsMarioSubindo = false;
                    }
                }
                // Mario começa a cair
                else if (IsMarioSubindo == false)
                {
                    picMario.Location = new Point(picMario.Location.X, picMario.Location.Y + 7);

                    //Se Mario caiu até o final
                    if (picMario.Location.Y >= HEIGHT)
                    {
                        System.Threading.Thread.Sleep(1200);
                        timer.Stop();

                        //Se acabaram as vidas...
                        if (--mario.Life == -1)
                        {
                            ShowGameStatus(GAME_OVER);
                        }
                        else
                        {
                            ChangeMarioImage(initialSituation);
                            Adjustment();
                            ShowGameStatus();
                        }
                    }
                }
            }

            // Se o casco chegou até o final da tela
            else if (picCasco.Location.X <= 0)
            {
                picCasco.Location = new Point(WIDTH_HALF, picMario.Location.Y + 48);
                ShowQuestion();
                timer.Stop();
            }
            // Se resposta errada
            else if (lblAnswer1.Visible == false)
            {
                picCasco.Location = new Point(picCasco.Location.X - 8, picCasco.Location.Y);
            }
        }

        private void timerCharacter_Tick(object sender, EventArgs e)
        {
            picCasco.Visible = false;
            mario.Action = CharacterAction.Run;
            picMario.Image = mario.Show();
        }

        private void Adjustment()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);

            picKoopa.Visible = false;
            lblQuestion.Size = new Size(WIDTH_HALF, 24);
            this.Size = new Size(WIDTH_HALF + WINDOWX_BORDER, HEIGHT + WINDOWY_BORDER);
            pnlFloor1.Location = new Point(0, HEIGHT - 72);
            pnlFloor1.Size = new Size(WIDTH, HEIGHT);

            pnlFloor2.Location = new Point(WIDTH_HALF, HEIGHT - 72);
            pnlFloor2.Size = new Size(WIDTH, HEIGHT);

            lblMensagem.Size = new Size(WIDTH_HALF, 24);
            lblMensagem.Location = new Point(0, (HEIGHT / 2));
            pnlPreto.Size = new Size(WIDTH_HALF, HEIGHT);
            picMario.Location = new Point(TAB_MARIO, this.pnlFloor1.Location.Y - 96);
            picCasco.Location = new Point(WIDTH_HALF, picMario.Location.Y + 48);
            picCasco.Visible = true;

            pnlFase.Location = new Point(0, 0);
            // GAME START
            if (questionIndex == -1)
            {
                Stage stage = new Stage(StageType.Ordinary);
                pnlFase.BackColor = COR_CEU;
                pnlFloor1.BackgroundImage = stage.Floor;
                PlayMusic("SMB_1.wav", 0, true);
                picMario.Image = Image.FromFile(@"img\mario.PNG");
                picPrincess.Image = Image.FromFile(@"img\luigi.PNG");

                picMario.Location = new Point(250, this.pnlFloor1.Location.Y - 96);
                picPrincess.Location = new Point(WIDTH_HALF - 250 - 48 - 15, this.pnlFloor1.Location.Y - 96);
            }
            // Fim do jogo
            else if (questionIndex == questions.Count)
            {
                Stage stage = new Stage(StageType.Boss);
                pnlFase.BackColor = COR_CASTELO;
                this.pnlFloor2.BackgroundImage = stage.Floor;
                PlayMusic("SMB_end.wav", 0, true);

                picPrincess.Image = Image.FromFile(@"img\peach.PNG");
                picPrincess.Location = new Point(WIDTH_HALF / 2 + 24, picMario.Location.Y);
                picMario.Location = new Point(WIDTH_HALF / 2 - 24, this.pnlFloor1.Location.Y - 96);
                picPrincess.Visible = true;

                lblMensagem.Text = string.Format(MSG_END_GAME, mario.Name.ToString().ToUpper());
                lblMensagem.Size = new Size(lblMensagem.Size.Width, lblMensagem.Size.Height * 5);
                lblMensagem.Click += new EventHandler(lblMensagem_StartClick);
                lblMensagem.Cursor = Cursors.Hand;
                //HideGameStatus();
                lblMensagem.Visible = true;
                // timer.Stop();
            }
            // Última fase
            else if (questionIndex == questions.Count - 1)
            {
                Stage stage = new Stage(StageType.Boss);
                pnlFase.BackColor = COR_CASTELO;
                pnlFloor2.BackgroundImage = stage.Floor;
                PlayMusic("SMB_8.wav", 0, true);
                picCasco.Image = Image.FromFile(String.Format(@"img\attack_8_1.PNG", rnd.Next(2)));
                picKoopa.Image = Image.FromFile(@"img\koopa.PNG");
                picKoopa.Location = new Point(WIDTH - 96, picMario.Location.Y);
                picKoopa.Visible = true;

            }
            // Penultima fase
            else if (questionIndex == questions.Count - 2)
            {
                Stage stage = new Stage(StageType.Boss);
                pnlFase.BackColor = COR_CASTELO;
                pnlFloor2.BackgroundImage = stage.Floor;
                PlayMusic("SMB_8.wav", 0, true);
                picCasco.Image = Image.FromFile(String.Format(@"img\casco_{0}.PNG", rnd.Next(2)));
            }
            else if (questionIndex >= questions.Count / 2)
            {
                Stage stage = new Stage(StageType.Underworld);
                pnlFase.BackColor = COR_CASTELO;
                pnlFloor2.BackgroundImage = stage.Floor;
                PlayMusic("SMB_2.wav", 0, true);
                picCasco.Image = Image.FromFile(@"img\casco_2.png");
            }
            else
            {
                Stage stage = new Stage(StageType.Ordinary);
                pnlFase.BackColor = COR_CEU;
                pnlFloor2.BackgroundImage = stage.Floor;
                PlayMusic("SMB_1.wav", 0, true);
                picCasco.Image = Image.FromFile(String.Format(@"img\casco_{0}.png", rnd.Next(2)));
            }
        }

        private void ChangeMarioImage(CharacterAction CharacterAction)
        {
            ChangeMarioImage(mario.Situation, CharacterAction);
        }

        private void ChangeMarioImage(CharacterSituation CharacterSituation)
        {
            ChangeMarioImage(CharacterSituation, mario.Action);
        }

        private void ChangeMarioImage(CharacterSituation CharacterSituation, CharacterAction CharacterAction)
        {
            mario.Situation = CharacterSituation;
            mario.Action = CharacterAction;
            picMario.Image = mario.Show();
        }

        private void CharWalk()
        {
            mario.Action = CharacterAction.Run;
            timerCharacter.Interval = 100;
            timerCharacter.Enabled = true;
            timer.Interval = 1;
            timer.Enabled = true;
        }

        private void CopyToInfoToFirstHalfPanel()
        {
            lblQuestion.Text = lbl2Question.Text;
            lblAnswer1.Text = lbl2Answer1.Text;
            lblAnswer2.Text = lbl2Answer2.Text;
            lblAnswer3.Text = lbl2Answer3.Text;
            lblAnswer4.Text = lbl2Answer4.Text;
            lblAnswer5.Text = lbl2Answer5.Text;

            lblQuestion.BackColor = lbl2Question.BackColor;
            lblAnswer1.BackColor = lbl2Answer1.BackColor;
            lblAnswer2.BackColor = lbl2Answer2.BackColor;
            lblAnswer3.BackColor = lbl2Answer3.BackColor;
            lblAnswer4.BackColor = lbl2Answer4.BackColor;
            lblAnswer5.BackColor = lbl2Answer5.BackColor;

            lblAnswer1.ForeColor = lbl2Answer1.ForeColor;
            lblAnswer2.ForeColor = lbl2Answer2.ForeColor;
            lblAnswer3.ForeColor = lbl2Answer3.ForeColor;
            lblAnswer4.ForeColor = lbl2Answer4.ForeColor;
            lblAnswer5.ForeColor = lbl2Answer5.ForeColor;

            pnlFloor1.BackgroundImage = pnlFloor2.BackgroundImage;
            picKoopa.Location = new Point(WIDTH_HALF - 96, picMario.Location.Y);
        }

        private void HideGameStatus()
        {
            pnlPreto.Visible = false;
            lblMensagem.Visible = false;
        }

        private void HideQuestions()
        {

            lblAnswer1.Visible = false;
            lblAnswer2.Visible = false;
            lblAnswer3.Visible = false;
            lblAnswer4.Visible = false;
            lblAnswer5.Visible = false;

            lbl2Answer1.Visible = false;
            lbl2Answer2.Visible = false;
            lbl2Answer3.Visible = false;
            lbl2Answer4.Visible = false;
            lbl2Answer5.Visible = false;

            lblAnswer1.BorderStyle = BorderStyle.None;
            lblAnswer2.BorderStyle = BorderStyle.None;
            lblAnswer3.BorderStyle = BorderStyle.None;
            lblAnswer4.BorderStyle = BorderStyle.None;
            lblAnswer5.BorderStyle = BorderStyle.None;

            lblQuestion.Visible = false;
            lbl2Question.Visible = false;
        }

        private void LoadPlayer()
        {
            int life = Int32.Parse(xnPlayer[0].Attributes["life"].Value);
            string situation = xnPlayer[0].Attributes["situation"].Value;
            switch (situation)
            {
                case "2":
                    mario = new Character(life, initialSituation = CharacterSituation.Flower);
                    break;
                case "1":
                    mario = new Character(life, initialSituation = CharacterSituation.Big);
                    break;
                default:
                    mario = new Character(life, initialSituation = CharacterSituation.Small);
                    break;
            }
        }

        private void LoadQuestion()
        {
            if (team.Name != "free")
            {

                for (int i = 0; i < xnQuestion.Count; i++)
                {
                    Question q = new Question(xnQuestion[i].Attributes["text"].Value);
                    XmlNodeList xnAnswer = xnQuestion[i].ChildNodes;
                    for (int j = 0; j < xnAnswer.Count; j++)
                    {
                        bool isCorrect = (xnAnswer[j].Attributes["correct"] != null ? xnAnswer[j].Attributes["correct"].Value == "1" : false);
                        string aText = xnAnswer[j].InnerText;
                        q.AddAnswer(new Answer(isCorrect, aText));
                    }

                    questions.Add(q);
                }
            }
            else
            {
                Question q;
                Answer a1;
                Answer a2;
                Answer a3;
                Answer a4;
                Answer a5;

                q = new Question("I have many friends because...");
                a1 = new Answer(true, "I like to speak with my friends very much");
                a2 = new Answer(false, "I don't like to read books");
                a3 = new Answer(false, "I work until five thirty");
                a4 = new Answer(false, "He comes to school");
                a5 = new Answer(false, "We eat cheese, but we don't eat bread");
                q.AddAnswer(a1);
                q.AddAnswer(a2);
                q.AddAnswer(a3);
                q.AddAnswer(a4);
                q.AddAnswer(a5);

                questions.Add(q);

                q = new Question("I read good Spanish books because...");
                a1 = new Answer(true, "I want to go to Spain");
                a2 = new Answer(false, "I don't know how to speak French");
                a3 = new Answer(false, "I need to study English");
                a4 = new Answer(false, "You have to go to the park");
                a5 = new Answer(false, "I write an email");
                q.AddAnswer(a1);
                q.AddAnswer(a2);
                q.AddAnswer(a3);
                q.AddAnswer(a4);
                q.AddAnswer(a5);

                questions.Add(q);

                q = new Question("We want to come to school next class because...");
                a1 = new Answer(true, "We want to study with the teacher");
                a2 = new Answer(false, "She wants to study English at Wizard");
                a3 = new Answer(false, "We don't know how to write an email to your boss");
                a4 = new Answer(false, "They have many homeworks");
                a5 = new Answer(false, "We understand English");
                q.AddAnswer(a1);
                q.AddAnswer(a2);
                q.AddAnswer(a3);
                q.AddAnswer(a4);
                q.AddAnswer(a5);

                questions.Add(q);

                q = new Question("I need to work by before dinner because...");
                a1 = new Answer(true, "My wife wants to have dinner with me");
                a2 = new Answer(false, "They go to Germany on Christmas");
                a3 = new Answer(false, "I have to lunch at two o'clock every day");
                a4 = new Answer(false, "I like to play the guitar in the morning");
                a5 = new Answer(false, "They want to have a snack with the boss");
                q.AddAnswer(a1);
                q.AddAnswer(a2);
                q.AddAnswer(a3);
                q.AddAnswer(a4);
                q.AddAnswer(a5);

                questions.Add(q);

                q = new Question("I need to go to the park after lunch because...");
                a1 = new Answer(true, "I need to play soccer with my friends this afternoon");
                a2 = new Answer(false, "I like to read a magazine with my manager");
                a3 = new Answer(false, "You stay in the office in the morning");
                a4 = new Answer(false, "I need to write a letter to my girlfriend");
                a5 = new Answer(false, "They know how to play basketball");
                q.AddAnswer(a1);
                q.AddAnswer(a2);
                q.AddAnswer(a3);
                q.AddAnswer(a4);
                q.AddAnswer(a5);

                questions.Add(q);

                q = new Question("I go to the USA on Christmas because...");
                a1 = new Answer(true, "My father lives there");
                a2 = new Answer(false, "I doesn't work on Christmas");
                a3 = new Answer(false, "My family wants some ice cream");
                a4 = new Answer(false, "My mother likes to eat pasta on weekends");
                a5 = new Answer(false, "My mom doesn't likes to stay in Brazil on Christmas");
                q.AddAnswer(a1);
                q.AddAnswer(a2);
                q.AddAnswer(a3);
                q.AddAnswer(a4);
                q.AddAnswer(a5);

                questions.Add(q);
            }
        }

        private void NextStage()
        {
            questionIndex++;
            // Se for a primeira fase...
            if (questionIndex == 0)
            {
                Adjustment();
                HideQuestions();
                ShowGameStatus();
            }
            // Salvou o jogo
            else if (questionIndex == questions.Count)
            {
                HideQuestions();
                PlayMusic("SMB_win.wav", 0);
                picPrincess.Visible = true;
                picPrincess.Image = Image.FromFile(@"img\peach.png");
                picPrincess.Location = new Point(WIDTH_HALF + TAB_MARIO + 48, picPrincess.Location.Y);
                IsKoopaSubindo = true;
                timer.Interval = 1;
                timer.Enabled = true;
            }
            else
            {
                team.NextTurn();
                Adjustment();
                ShowQuestion();
                CharWalk();
            }
        }

        private void PlayMusic(string musicFile_, int sleep_)
        {
            PlayMusic(musicFile_, sleep_, false, false);
        }

        private void PlayMusic(string musicFile_, int sleep_, bool repeat_)
        {
            PlayMusic(musicFile_, sleep_, repeat_, false);
        }

        private void PlayMusic(string musicFile_, int sleep_, bool repeat_, bool continuePlaying_)
        {
            musicFile_ = String.Format(@"wave\{0}", musicFile_);
            string temp = sPlayer.SoundLocation;
            if (temp != musicFile_)
            {
                sPlayer.SoundLocation = musicFile_;
                if (repeat_)
                {
                    System.Threading.Thread.Sleep(sleep_);
                    sPlayer.PlayLooping();
                }
                else
                {
                    // Toca o som uma única vez e volta a tocar o som de fundo
                    sPlayer.Play();
                    System.Threading.Thread.Sleep(sleep_);
                    if (continuePlaying_)
                    {
                        sPlayer.SoundLocation = temp;
                        sPlayer.PlayLooping();
                    }
                }
            }
        }

        private void ReadConfigFile()
        {
            XmlDocument document = new XmlDocument();
            try
            {
                document.Load(string.Format("{0}\\config.xml", Application.StartupPath));
                xnPlayer = document.GetElementsByTagName("player");
                xnQuestion = document.GetElementsByTagName("question");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading configuration file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.Print(ex.StackTrace);
                MessageBox.Show(string.Format("{0}\\config.xml", Application.StartupPath), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Restart()
        {
            questionIndex = -1;
            pnlPreto.Visible = false;
            lblMensagem.BackColor = COR_CEU;
            lblMensagem.Text = GAME_START;

            LoadPlayer();

            lblMensagem.Cursor = Cursors.Default;
            lblMensagem.Click -= lblMensagem_StartClick;

            ChangeMarioImage(mario.Action);
            picMario.Click += new EventHandler(picCharacter_Click);
            picMario.Cursor = Cursors.Hand;
            //Luigi pegou emprestado o "picPrincess" só na tela de GAME START
            picPrincess.Click += new EventHandler(picCharacter_Click);
            picPrincess.Cursor = Cursors.Hand;
            picPrincess.Visible = true;
            lblQuestion.Text = "Developed by teacher Satoru Kishi - Nov/21/2012";
            HideQuestions();
            Adjustment();
        }

        private void ShowAnimacaoLose()
        {
            picCasco.Location = new Point(WIDTH_HALF, picMario.Location.Y + 48);
            timer.Interval = 1;
            timer.Enabled = true;
        }

        private void ShowGameStatus(string mensagem_)
        {
            pnlPreto.Visible = true;
            lblMensagem.Visible = true;
            lblMensagem.Cursor = Cursors.Default;
            lblMensagem.Text = String.Format(mensagem_);
            lblMensagem.BackColor = COR_CASTELO;
            timer.Interval = 1000;
            timer.Enabled = true;
        }

        private void ShowGameStatus()
        {
            ShowGameStatus(String.Format("{0} x {1}", mario.Name, mario.Life));
        }

        private void ShowQuestion()
        {
            Random rnd = new Random(DateTime.Now.Millisecond);
            int quant = questions[questionIndex].Answers.Count;
            Label[] l = { lbl2Answer1, lbl2Answer2, lbl2Answer3, lbl2Answer4, lbl2Answer5 };
            string temp;

            lblAnswer1.Visible = true;
            lblAnswer2.Visible = true;
            lblAnswer3.Visible = true;
            lblAnswer4.Visible = true;
            lblAnswer5.Visible = true;
            lbl2Answer1.Visible = true;
            lbl2Answer2.Visible = true;
            lbl2Answer3.Visible = true;
            lbl2Answer4.Visible = true;
            lbl2Answer5.Visible = true;
            lblQuestion.Visible = true;
            lbl2Question.Visible = true;

            lbl2Question.Text = String.Format("Stage {0} - {1}", questionIndex + 1, questions[questionIndex].Text);

            lbl2Answer1.Text = quant > 0 ? questions[questionIndex].Answers[0].Text : "";
            lbl2Answer2.Text = quant > 1 ? questions[questionIndex].Answers[1].Text : "";
            lbl2Answer3.Text = quant > 2 ? questions[questionIndex].Answers[2].Text : "";
            lbl2Answer4.Text = quant > 3 ? questions[questionIndex].Answers[3].Text : "";
            lbl2Answer5.Text = quant > 4 ? questions[questionIndex].Answers[4].Text : "";

            for (int i = 0; i < quant; i++)
            {
                int random = rnd.Next(i, quant);

                temp = l[i].Text;
                l[i].Text = l[random].Text;
                l[random].Text = temp;
            }
        }

        private void Start()
        {
            questions = new List<Question>(12);
            questionIndex = -1;
            pnlPreto.Visible = false;
            lblMensagem.BackColor = COR_CEU;
            lblMensagem.Text = GAME_START;

            ReadConfigFile();
            LoadPlayer();
            LoadQuestion();

            lblAnswer1.Cursor = Cursors.Hand;
            lblAnswer2.Cursor = Cursors.Hand;
            lblAnswer3.Cursor = Cursors.Hand;
            lblAnswer4.Cursor = Cursors.Hand;
            lblAnswer5.Cursor = Cursors.Hand;

            lblAnswer1.Click += new EventHandler(lblAnswer_Click);
            lblAnswer2.Click += new EventHandler(lblAnswer_Click);
            lblAnswer3.Click += new EventHandler(lblAnswer_Click);
            lblAnswer4.Click += new EventHandler(lblAnswer_Click);
            lblAnswer5.Click += new EventHandler(lblAnswer_Click);

            lblMensagem.Cursor = Cursors.Default;
            lblMensagem.Click -= lblMensagem_StartClick;

            ChangeMarioImage(mario.Action);
            picMario.Click += new EventHandler(picCharacter_Click);
            picMario.Cursor = Cursors.Hand;
            //Luigi pegou emprestado o "picPrincess" só na tela de GAME START
            picPrincess.Click += new EventHandler(picCharacter_Click);
            picPrincess.Cursor = Cursors.Hand;
            picPrincess.Visible = true;
            HideQuestions();
            lblQuestion.Text = "Developed by Satoru Kishi - Nov/21/2012";
            lblQuestion.Visible = true;
            Adjustment();
        }

        private void WrongAnswer(Label sender)
        {
            Label lbl = sender;
            lbl.ForeColor = Color.Red;
            HideQuestions();
            ShowAnimacaoLose();
        }

    }
}
