using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace from_game9
{
    public partial class Form1 : Form
    {
        public const int BoardWidth = 10;
        public const int BoardHeight = 20;
        private const int BlockSize = 30;
        private const int NextBlockWidth = 4; // 다음 블록의 너비
        private const int NextBlockHeight = 2; // 다음 블록의 높이

        private Timer gameTimer;
        private int[,] board;
        private Tetromino currentTetromino;
        private Tetromino[] nextTetrominos;
        private int score;

        // 중복된 테트로미노를 방지하기 위한 집합
        private HashSet<string> usedTetrominos;

        public Form1()
        {
            InitializeComponent();
            usedTetrominos = new HashSet<string>();

            // 게임 창 크기 고정
            this.ClientSize = new Size(473, 626);  // 창 크기  설정
            this.FormBorderStyle = FormBorderStyle.FixedDialog;  // 창 크기 변경 불가
            this.MaximizeBox = false;  // 최대화 버튼 비활성화
            this.MinimizeBox = false;  // 최소화 버튼 비활성화
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitializeGame();
        }

        private void InitializeGame()
        {
            board = new int[BoardWidth, BoardHeight];
            nextTetrominos = new Tetromino[1]; // 다음 블록 3개
            score = 0; // 점수 초기화
            gameTimer = new Timer();
            gameTimer.Interval = 500; // 0.5초마다 블록 떨어짐
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
            SpawnTetromino();
        }

        private void SpawnTetromino()
        {
            if (nextTetrominos[0] == null)
            {
                for (int i = 0; i < nextTetrominos.Length; i++)
                {
                    Tetromino newTetromino;
                    do
                    {
                        newTetromino = Tetromino.CreateRandomTetromino();
                    } while (usedTetrominos.Contains(newTetromino.GetHashCode().ToString()));

                    nextTetrominos[i] = newTetromino;
                    usedTetrominos.Add(newTetromino.GetHashCode().ToString());
                }
            }

            currentTetromino = nextTetrominos[nextTetrominos.Length - 1];

            for (int i = nextTetrominos.Length - 1; i > 0; i--)
            {
                nextTetrominos[i] = nextTetrominos[i - 1];
            }

            Tetromino nextTetrominoToAdd;
            do
            {
                nextTetrominoToAdd = Tetromino.CreateRandomTetromino();
            } while (usedTetrominos.Contains(nextTetrominoToAdd.GetHashCode().ToString()));
            nextTetrominos[0] = nextTetrominoToAdd;
            usedTetrominos.Add(nextTetrominoToAdd.GetHashCode().ToString());

            if (!IsValidPosition(currentTetromino))
            {
                gameTimer.Stop();
                MessageBox.Show("게임 오버");
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            MoveDown();
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawBoard(e.Graphics);
            DrawTetromino(e.Graphics);
            DrawNextTetrominos(e.Graphics);
            DrawScore(e.Graphics);
            DrawBorder(e.Graphics);
        }

        private void DrawBorder(Graphics g)
        {
            using (Pen pen = new Pen(Color.Black, 2))
            {
                g.DrawRectangle(pen, 0, 0, BoardWidth * BlockSize, BoardHeight * BlockSize);
            }
        }

        private void DrawBoard(Graphics g)
        {
            for (int x = 0; x < BoardWidth; x++)
            {
                for (int y = 0; y < BoardHeight; y++)
                {
                    if (board[x, y] != 0)
                    {
                        g.FillRectangle(Brushes.Blue, x * BlockSize, y * BlockSize, BlockSize, BlockSize);
                    }
                }
            }
        }

        private void DrawTetromino(Graphics g)
        {
            for (int y = 0; y < currentTetromino.Shape.GetLength(0); y++)
            {
                for (int x = 0; x < currentTetromino.Shape.GetLength(1); x++)
                {
                    if (currentTetromino.Shape[y, x] != 0)
                    {
                        g.FillRectangle(Brushes.Red,
                            (currentTetromino.X + x) * BlockSize,
                            (currentTetromino.Y + y) * BlockSize,
                            BlockSize, BlockSize);
                    }
                }
            }
        }

        private void DrawNextTetrominos(Graphics g)
        {
            using (Pen pen = new Pen(Color.Black, 2))
            {
                int nextBlockX = BoardWidth * BlockSize + 10;
                int nextBlockY = 100;

                // 전체 영역 테두리 그리기
                g.DrawRectangle(pen, nextBlockX - 2, nextBlockY - 2, NextBlockWidth * BlockSize + 4, NextBlockHeight * nextTetrominos.Length * BlockSize + 4);

                // 각 다음 블록을 그리기
                for (int i = 0; i < nextTetrominos.Length; i++)
                {
                    for (int y = 0; y < nextTetrominos[i].Shape.GetLength(0); y++)
                    {
                        for (int x = 0; x < nextTetrominos[i].Shape.GetLength(1); x++)
                        {
                            if (nextTetrominos[i].Shape[y, x] != 0)
                            {
                                g.FillRectangle(Brushes.Green,
                                    nextBlockX + x * BlockSize,
                                    nextBlockY + (nextTetrominos.Length - 1 - i) * (NextBlockHeight + 1) * BlockSize + y * BlockSize,
                                    BlockSize, BlockSize);
                            }
                        }
                    }
                }
            }
        }

        private void DrawScore(Graphics g)
        {
            g.DrawString($"점수: {score}", new Font("Arial", 16), Brushes.Black, new PointF(BoardWidth * BlockSize + 10, 10));
        }

        private void MoveDown()
        {
            if (IsValidPosition(currentTetromino, 0, 1))
            {
                currentTetromino.Y++;
            }
            else
            {
                MergeTetromino();
                ClearLines();
                SpawnTetromino();
            }
        }

        private bool IsValidPosition(Tetromino tetromino, int offsetX = 0, int offsetY = 0)
        {
            for (int y = 0; y < tetromino.Shape.GetLength(0); y++)
            {
                for (int x = 0; x < tetromino.Shape.GetLength(1); x++)
                {
                    if (tetromino.Shape[y, x] != 0)
                    {
                        int newX = tetromino.X + x + offsetX;
                        int newY = tetromino.Y + y + offsetY;

                        if (newX < 0 || newX >= BoardWidth || newY >= BoardHeight || (newY >= 0 && board[newX, newY] != 0))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private void MergeTetromino()
        {
            for (int y = 0; y < currentTetromino.Shape.GetLength(0); y++)
            {
                for (int x = 0; x < currentTetromino.Shape.GetLength(1); x++)
                {
                    if (currentTetromino.Shape[y, x] != 0)
                    {
                        board[currentTetromino.X + x, currentTetromino.Y + y] = 1;
                    }
                }
            }
        }

        private void ClearLines()
        {
            for (int y = BoardHeight - 1; y >= 0; y--)
            {
                bool fullLine = true;
                for (int x = 0; x < BoardWidth; x++)
                {
                    if (board[x, y] == 0)
                    {
                        fullLine = false;
                        break;
                    }
                }

                if (fullLine)
                {
                    // 라인 삭제
                    for (int shiftY = y; shiftY > 0; shiftY--)
                    {
                        for (int x = 0; x < BoardWidth; x++)
                        {
                            board[x, shiftY] = board[x, shiftY - 1];
                        }
                    }
                    score += 100; // 점수 추가
                    y++; // 다시 검사
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case Keys.Left:
                    if (IsValidPosition(currentTetromino, -1, 0))
                    {
                                                currentTetromino.X--;
                    }
                    break;

                case Keys.Right:
                    if (IsValidPosition(currentTetromino, 1, 0))
                    {
                        currentTetromino.X++;
                    }
                    break;

                case Keys.Down:
                    if (IsValidPosition(currentTetromino, 0, 1))
                    {
                        currentTetromino.Y++;
                    }
                    break;

                case Keys.Up:
                    currentTetromino.Rotate();
                    if (!IsValidPosition(currentTetromino))
                    {
                        currentTetromino.Rotate(-1); // 회전 취소
                    }
                    break;

                case Keys.Space:
                    while (IsValidPosition(currentTetromino, 0, 1))
                    {
                        currentTetromino.Y++;
                    }
                    MergeTetromino();
                    ClearLines();
                    SpawnTetromino();
                    break;
            }

            Invalidate();
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // 점수 반환 메서드
        public string GetScore()
        {
            return score.ToString();
        }
    }

    // 테트로미노 클래스
    public class Tetromino
    {
        public int[,] Shape { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        private static Random random = new Random();

        public static Tetromino CreateRandomTetromino()
        {
            int shapeType = random.Next(7);
            Tetromino tetromino = new Tetromino();
            tetromino.X = Form1.BoardWidth / 2 - 1;
            tetromino.Y = 0;

            switch (shapeType)
            {
                case 0: // I
                    tetromino.Shape = new int[,] {
                        { 1, 1, 1, 1 }
                    };
                    break;
                case 1: // O
                    tetromino.Shape = new int[,] {
                        { 1, 1 },
                        { 1, 1 }
                    };
                    break;
                case 2: // T
                    tetromino.Shape = new int[,] {
                        { 0, 1, 0 },
                        { 1, 1, 1 }
                    };
                    break;
                case 3: // L
                    tetromino.Shape = new int[,] {
                        { 1, 0, 0 },
                        { 1, 1, 1 }
                    };
                    break;
                case 4: // J
                    tetromino.Shape = new int[,] {
                        { 0, 0, 1 },
                        { 1, 1, 1 }
                    };
                    break;
                case 5: // S
                    tetromino.Shape = new int[,] {
                        { 0, 1, 1 },
                        { 1, 1, 0 }
                    };
                    break;
                case 6: // Z
                    tetromino.Shape = new int[,] {
                        { 1, 1, 0 },
                        { 0, 1, 1 }
                    };
                    break;
            }

            return tetromino;
        }

        // 회전 메서드
        public void Rotate(int times = 1)
        {
            for (int i = 0; i < times; i++)
            {
                int[,] rotatedShape = new int[Shape.GetLength(1), Shape.GetLength(0)];
                for (int y = 0; y < Shape.GetLength(0); y++)
                {
                    for (int x = 0; x < Shape.GetLength(1); x++)
                    {
                        rotatedShape[x, Shape.GetLength(0) - 1 - y] = Shape[y, x];
                    }
                }
                Shape = rotatedShape;
            }
        }
    }
}

