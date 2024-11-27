using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace from_game5
{
    public partial class Form1 : Form
    {
        private string game_score = string.Empty;
        public string Score { get; private set; }
        public string GameName { get; private set; } = "뱀게임";

        // 게임 그리드 크기 (21x21 크기)
        private const int gridSize = 21;

        // 한 타일의 픽셀 크기 (20x20 픽셀)
        private const int tileSize = 20;

        // 뱀의 몸을 구성하는 좌표 리스트 (뱀의 몸통)
        private List<Point> snake;

        // 현재 먹이 위치를 저장
        private Point food;

        // 뱀의 이동 방향 (상, 하, 좌, 우)
        private Direction direction;

        // 게임 진행을 제어하는 타이머 (뱀 이동 간격 설정)
        private Timer timer;

        // 랜덤 먹이 생성기를 위한 랜덤 객체
        private Random random;

        // 현재 게임의 점수를 저장
        private int score;

        // 게임 종료 여부를 나타내는 상태 플래그
        private bool gameOver;

        // 뱀의 이동 방향을 나타내는 열거형
        private enum Direction { Up, Down, Left, Right }

        // 폼 생성자
        public Form1()
        {
            InitializeComponent(); // 폼 컴포넌트 초기화
            InitializeGame();      // 게임 초기화

            // 게임 창 크기 고정
            this.ClientSize = new Size(420, 480);  // 창 크기를 420x480으로 설정
            this.FormBorderStyle = FormBorderStyle.FixedDialog;  // 창 크기 변경 불가
            this.MaximizeBox = false;  // 최대화 버튼 비활성화
        }

        // 게임 초기 설정을 담당
        private void InitializeGame()
        {
            // 더블 버퍼링을 활성화하여 깜빡임 방지
            this.DoubleBuffered = true;

            // 랜덤 객체 초기화
            random = new Random();

            // 뱀의 초기 위치 (5, 5에 시작)
            snake = new List<Point> { new Point(5, 5) };

            // 뱀의 초기 이동 방향 (오른쪽)
            direction = Direction.Right;

            // 점수 초기화
            score = 0;

            // 게임 상태 초기화
            gameOver = false;

            // 타이머 설정 (100ms 간격으로 Tick 이벤트 발생)
            timer = new Timer();
            timer.Interval = 100;        // 뱀이 0.1초마다 이동
            timer.Tick += Timer_Tick;    // Tick 이벤트 발생 시 호출될 메서드
            timer.Start();               // 타이머 시작

            // 폼의 그리기 이벤트 핸들러 등록
            this.Paint += Form1_Paint;

            // 키보드 입력 이벤트 핸들러 등록
            this.KeyDown += Form1_KeyDown;

            // 첫 번째 먹이 생성
            GenerateFood();
        }

        // 타이머 Tick 이벤트 (뱀이 이동하도록 함)
        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveSnake();       // 뱀을 한 칸 이동
            this.Invalidate(); // 화면 갱신 요청 (Paint 이벤트 호출)
        }

        // 뱀 이동 처리
        private void MoveSnake()
        {
            // 뱀의 머리 좌표
            Point head = snake[0];

            // 현재 이동 방향에 따라 머리의 새 좌표 계산
            switch (direction)
            {
                case Direction.Up:
                    head.Y--;
                    break;
                case Direction.Down:
                    head.Y++;
                    break;
                case Direction.Left:
                    head.X--;
                    break;
                case Direction.Right:
                    head.X++;
                    break;
            }

            // 벽 충돌 확인 (그리드 경계 바깥으로 나가면 게임 종료)
            if (head.X < 1 || head.X >= gridSize - 1 || head.Y < 1 || head.Y >= gridSize - 1)
            {
                EndGame(); // 게임 종료
                return;
            }

            // 먹이를 먹었는지 확인
            if (head == food)
            {
                // 먹이를 먹으면 뱀 길이 증가 (새 머리를 리스트에 추가)
                snake.Insert(0, head);

                // 점수 증가
                score += 10;

                // 새로운 먹이 생성
                GenerateFood();
            }
            else
            {
                // 뱀이 한 칸 이동 (머리 추가 + 꼬리 제거)
                snake.Insert(0, head);
                snake.RemoveAt(snake.Count - 1);
            }

            // 뱀이 자기 자신과 충돌했는지 확인
            if (snake.Skip(1).Contains(head))
            {
                EndGame(); // 게임 종료
                return;
            }
        }

        // 새로운 먹이 좌표 생성
        private void GenerateFood()
        {
            do
            {
                // 먹이는 경계 안쪽에서 랜덤 생성 (1~19 범위)
                food = new Point(random.Next(1, gridSize - 1), random.Next(1, gridSize - 1));
            } while (snake.Contains(food)); // 먹이가 뱀과 겹치지 않도록
        }

        // 폼의 화면을 다시 그리는 이벤트
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // 경계 그리기 (검은색 박스로 표현)
            for (int i = 0; i < gridSize; i++)
            {
                g.FillRectangle(Brushes.Black, i * tileSize, 0, tileSize, tileSize);                  // 상단 경계
                g.FillRectangle(Brushes.Black, i * tileSize, (gridSize - 1) * tileSize, tileSize, tileSize); // 하단 경계
                g.FillRectangle(Brushes.Black, 0, i * tileSize, tileSize, tileSize);                 // 왼쪽 경계
                g.FillRectangle(Brushes.Black, (gridSize - 1) * tileSize, i * tileSize, tileSize, tileSize); // 오른쪽 경계
            }

            // 뱀의 몸 그리기 (녹색 타일)
            foreach (var point in snake)
            {
                g.FillRectangle(Brushes.Green, point.X * tileSize, point.Y * tileSize, tileSize, tileSize);
            }
            // 먹이 그리기 (빨간색 타일)
            g.FillRectangle(Brushes.Red, food.X * tileSize, food.Y * tileSize, tileSize, tileSize);

            // 점수 표시
            g.DrawString($"점수: {score}", new Font("Arial", 12), Brushes.Black, new PointF(5, 430));

            // 게임 종료 시 '게임 오버' 메시지 표시
            if (gameOver)
            {
                g.DrawString("게임 오버!", new Font("Arial", 24), Brushes.Red, new PointF(120, 50));
            }
        }

        // 키보드 입력 이벤트 처리 (뱀의 방향 전환)
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            // 방향 전환은 반대 방향으로 바로 변경할 수 없음
            if (e.KeyCode == Keys.Up && direction != Direction.Down) direction = Direction.Up;
            if (e.KeyCode == Keys.Down && direction != Direction.Up) direction = Direction.Down;
            if (e.KeyCode == Keys.Left && direction != Direction.Right) direction = Direction.Left;
            if (e.KeyCode == Keys.Right && direction != Direction.Left) direction = Direction.Right;
        }

        // 게임 종료 및 점수 반환
        public void EndGame()
        {
            gameOver = true; // 게임 종료 상태 설정
            timer.Stop();    // 타이머 정지
            MessageBox.Show($"게임 종료! 최종 점수: {score}");
            Score = score.ToString(); // Score 속성에 점수 할당
            this.Close();    // 폼 닫기
        }

        private void Form1_Load(object sender, EventArgs e) { }
    }
}
