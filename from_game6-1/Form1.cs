using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MinesweeperGame
{
    public partial class Form1 : Form
    {
        private const int SIZE = 17; // 게임 보드 크기
        private const int MINE_COUNT = 20; // 지뢰 개수
        private int score;
        private bool gameOver;
        private bool gameWon;
        private bool firstClick;
        private int[,] mineBoard; // 지뢰 보드
        private bool[,] revealed; // 클릭된 셀 상태
        private bool[,] flagged; // 우클릭된 셀 상태
        private int cellsToReveal; // 열어야 할 칸 수

        public Form1()
        {
            InitializeComponent();
            CenterForm(); // 창 위치 조정
            InitializeGame();
        }

        private void CenterForm()
        {
            Rectangle screen = Screen.PrimaryScreen.WorkingArea;
            int x = (screen.Width - this.Width) / 2;
            int y = 100; // 고정된 y 위치
            this.StartPosition = FormStartPosition.Manual;
            this.Location = new Point(x, y);
        }

        private void InitializeGame()
        {
            mineBoard = new int[SIZE, SIZE];
            revealed = new bool[SIZE, SIZE];
            flagged = new bool[SIZE, SIZE];
            score = 0;
            gameOver = false;
            gameWon = false;
            firstClick = true;
            cellsToReveal = SIZE * SIZE - MINE_COUNT; // 열어야 할 칸 수

            this.Text = "지뢰찾기 게임";
            this.ClientSize = new Size(680, 720);
            this.BackColor = Color.White;
            this.Controls.Clear();

            int buttonSize = 40;
            int margin = 5;

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    Button btn = new Button
                    {
                        Size = new Size(buttonSize - margin, buttonSize - margin),
                        Location = new Point(j * buttonSize, i * buttonSize),
                        Tag = new Tuple<int, int>(i, j),
                        BackColor = Color.Black,
                        FlatStyle = FlatStyle.Flat,
                        ForeColor = Color.White,
                        Font = new Font("Arial", 10, FontStyle.Bold)
                    };
                    btn.FlatAppearance.BorderColor = Color.Black;
                    btn.Click += Button_Click;
                    btn.MouseUp += Button_MouseUp;
                    this.Controls.Add(btn);
                }
            }

            Random rand = new Random();
            for (int i = 0; i < MINE_COUNT; i++)
            {
                int x, y;
                do
                {
                    x = rand.Next(SIZE);
                    y = rand.Next(SIZE);
                } while (mineBoard[x, y] == -1);
                mineBoard[x, y] = -1;
            }

            for (int i = 0; i < SIZE; i++)
            {
                for (int j = 0; j < SIZE; j++)
                {
                    if (mineBoard[i, j] != -1)
                    {
                        mineBoard[i, j] = CountAdjacentMines(i, j);
                    }
                }
            }

            Label scoreLabel = new Label
            {
                Name = "scoreLabel",
                Location = new Point(10, SIZE * buttonSize + 10),
                Text = "Score: " + score,
                ForeColor = Color.Black,
                Font = new Font("Arial", 12, FontStyle.Bold)
            };
            this.Controls.Add(scoreLabel);

            // 오른쪽 하단에 "지뢰찾기 게임" 텍스트를 추가
            Label gameTitleLabel = new Label
            {
                Name = "gameTitleLabel",
                Text = "지뢰찾기 게임",
                Location = new Point(this.ClientSize.Width - 180, this.ClientSize.Height - 40),
                Font = new Font("Arial", 18, FontStyle.Bold),
                ForeColor = Color.Black,
                AutoSize = true
            };
            this.Controls.Add(gameTitleLabel);
        }

        private int CountAdjacentMines(int x, int y)
        {
            int mineCount = 0;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int nx = x + i;
                    int ny = y + j;
                    if (nx >= 0 && ny >= 0 && nx < SIZE && ny < SIZE && mineBoard[nx, ny] == -1)
                    {
                        mineCount++;
                    }
                }
            }
            return mineCount;
        }

        private void Button_Click(object sender, EventArgs e)
        {
            if (gameOver) return;

            Button btn = sender as Button;
            var position = (Tuple<int, int>)btn.Tag;
            int x = position.Item1;
            int y = position.Item2;

            // 첫 클릭에서 지뢰가 터진 경우
            if (firstClick)
            {
                firstClick = false;
                if (mineBoard[x, y] == -1)
                {
                    score = 0;
                    btn.BackColor = Color.Red;
                    btn.Text = "💥";
                    ShowAllMines();
                    gameOver = true;
                    MessageBox.Show($"게임 종료! 최종 점수: {GetGameScore()}");
                    return;
                }
                else
                {
                    RevealCell(x, y);
                }
            }

            // 지뢰 클릭 시
            if (mineBoard[x, y] == -1)
            {
                btn.BackColor = Color.Red;
                btn.Text = "💥";
                ShowAllMines();
                gameOver = true;
                MessageBox.Show($"게임 종료! 최종 점수: {GetGameScore()}");
                return;
            }

            revealed[x, y] = true;
            btn.Text = mineBoard[x, y] == 0 ? "" : mineBoard[x, y].ToString();
            btn.BackColor = Color.LightBlue;

            if (mineBoard[x, y] == 0)
            {
                RevealAdjacentCells(x, y);
            }

            score++;
            UpdateScore();

            // 모든 지뢰를 제외한 칸을 열었을 경우 승리
            if (score == cellsToReveal)
            {
                gameWon = true;
                score = 100;
                MessageBox.Show($"게임 승리! 최종 점수: {GetGameScore()}");
            }
        }

        private void RevealCell(int x, int y)
        {
            Button btn = (Button)this.Controls[x * SIZE + y];
            revealed[x, y] = true;
            btn.Text = mineBoard[x, y] == 0 ? "" : mineBoard[x, y].ToString();
            btn.BackColor = Color.LightBlue;
            score++;
            UpdateScore();
        }

        private void RevealAdjacentCells(int x, int y)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int nx = x + i;
                    int ny = y + j;
                    if (nx >= 0 && ny >= 0 && nx < SIZE && ny < SIZE && !revealed[nx, ny] && mineBoard[nx, ny] != -1)
                    {
                        RevealCell(nx, ny);
                        if (mineBoard[nx, ny] == 0)
                        {
                            RevealAdjacentCells(nx, ny);
                        }
                    }
                }
            }
        }

        private void Button_MouseUp(object sender, MouseEventArgs e)
        {
            if (gameOver) return;

            if (e.Button == MouseButtons.Right)
            {
                Button btn = sender as Button;
                var position = (Tuple<int, int>)btn.Tag;
                int x = position.Item1;
                int y = position.Item2;

                if (revealed[x, y]) return;

                if (flagged[x, y])
                {
                    btn.Text = "";
                    flagged[x, y] = false;
                }
                else
                {
                    btn.Text = "⚑";
                    flagged[x, y] = true;
                }
            }
        }

        private void UpdateScore()
        {
            Label scoreLabel = this.Controls["scoreLabel"] as Label;
            if (scoreLabel != null)
            {
                scoreLabel.Text = "Score: " + score;
            }
        }

        private void ShowAllMines()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Button btn)
                {
                    var position = (Tuple<int, int>)btn.Tag;
                    int x = position.Item1;
                    int y = position.Item2;

                    if (mineBoard[x, y] == -1)
                    {
                        btn.BackColor = Color.Red;
                        btn.Text = "💥";
                    }
                }
            }
        }

        private string GetGameScore()
        {
            return score.ToString();
        }
    }
}
