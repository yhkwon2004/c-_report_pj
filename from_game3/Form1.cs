using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace from_game3
{
    public partial class Form1 : Form
    {
        private List<int> secretNumber; // 비밀 번호를 저장할 리스트
        private int strikes; // 스트라이크 수
        private int balls; // 볼 수
        private int attemptCount; // 시도 횟수
        private const string scoreFilePath = "score.txt"; // 스코어 파일 경로
        private int score; // 점수 변수 추가

        public Form1()
        {
            InitializeComponent();
            InitializeGame(); // 게임 초기화
            SetupListView();  // ListView 설정
            this.FormBorderStyle = FormBorderStyle.FixedDialog;  // 창 크기 변경 불가
            this.MaximizeBox = false;  // 최대화 버튼 비활성화
        }

        // 랜덤 비밀 번호 생성 메서드
        private List<int> GenerateRandomNumber()
        {
            Random random = new Random();
            return Enumerable.Range(0, 10)
                             .OrderBy(x => random.Next())
                             .Take(3)
                             .ToList();
        }

        // 게임 초기화 메서드
        private void InitializeGame()
        {
            secretNumber = GenerateRandomNumber(); // 랜덤 비밀 번호 생성
            strikes = 0; // 스트라이크 초기화
            balls = 0;   // 볼 초기화
            attemptCount = 1; // 시도 횟수 초기화
            score = 100; // 점수 초기화
            listView1.Items.Clear(); // ListView 초기화

            // 도메인 업다운 값 초기화
            domainUpDown1.SelectedItem = 0;
            domainUpDown2.SelectedItem = 0;
            domainUpDown3.SelectedItem = 0;
        }

        // ListView 설정 메서드
        private void SetupListView()
        {
            listView1.View = View.Details; // 리스트 뷰 형식 설정 (세부 항목 보기)
            listView1.HeaderStyle = ColumnHeaderStyle.None; // 헤더 스타일 설정
            listView1.Columns.Clear(); // 컬럼 초기화
            listView1.Columns.Add("결과", 300); // 결과를 표시할 컬럼 추가
            listView1.Scrollable = true; // 스크롤 가능하게 설정
        }

        // 사용자의 추측을 테스트하는 메서드
        private void hit_test(object sender, EventArgs e)
        {
            int guess1 = Convert.ToInt32(domainUpDown1.SelectedItem);
            int guess2 = Convert.ToInt32(domainUpDown2.SelectedItem);
            int guess3 = Convert.ToInt32(domainUpDown3.SelectedItem);
            List<int> guesses = new List<int> { guess1, guess2, guess3 };

            CalculateStrikeAndBall(guesses);

            // ListView에 결과 추가
            listView1.Items.Insert(0, new ListViewItem($"[{attemptCount}] {strikes} Strike {balls} Ball"));
            attemptCount++; // 시도 횟수 증가

            // 시도 횟수에 따른 점수 계산
            if (attemptCount <= 30)
            {
                score = Math.Max(0, 100 - (attemptCount - 1) * 3); // 시도 횟수에 따라 점수 계산 (30번 이상이면 0점)
            }

            // 30회 시도 시 게임 실패 메시지
            if (attemptCount > 30)
            {
                MessageBox.Show("게임 실패! 30회 시도했습니다.");
                SaveScore(); // 스코어 저장
                RestartGame(); // 게임 재시작
            }

            // 3개의 스트라이크가 모두 맞으면 게임 승리
            if (strikes == 3)
            {
                MessageBox.Show($"축하합니다! 게임을 이겼습니다! \n현재 점수: {score}");
                SaveScore(); // 스코어 저장
                RestartGame(); // 게임 재시작
            }
        }

        // 스트라이크와 볼을 계산하는 메서드
        private void CalculateStrikeAndBall(List<int> guesses)
        {
            strikes = 0; // 스트라이크 초기화
            balls = 0;   // 볼 초기화

            for (int i = 0; i < guesses.Count; i++)
            {
                if (guesses[i] == secretNumber[i]) // 위치와 숫자가 모두 맞으면 스트라이크
                {
                    strikes++;
                }
                else if (secretNumber.Contains(guesses[i])) // 숫자는 맞지만 위치가 다르면 볼
                {
                    balls++;
                }
            }
        }

        // 스코어를 파일에 저장하는 메서드
        private void SaveScore()
        {
            using (StreamWriter writer = new StreamWriter(scoreFilePath, true))
            {
                writer.WriteLine($"시도 횟수: {attemptCount - 1}, 점수: {score}");
            }
        }

        // 게임을 재시작하는 메서드
        private void RestartGame()
        {
            Form1 newGame = new Form1(); // 새 인스턴스 생성
            newGame.Show(); // 새 게임 폼 표시
            this.Close(); // 현재 폼 닫기
        }

        // 도메인 업다운의 선택 항목 변경 이벤트 핸들러
        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e) { }
        private void domainUpDown2_SelectedItemChanged(object sender, EventArgs e) { }
        private void domainUpDown3_SelectedItemChanged(object sender, EventArgs e) { }
        private void listView1_SelectedIndexChanged(object sender, EventArgs e) { }

        // 폼 로드 이벤트 핸들러
        private void Form1_Load(object sender, EventArgs e) { }

        // 게임 종료 후 최종 점수를 반환
        private string GetGameResult()
        {
            return score.ToString(); // 점수만 문자열로 반환
        }
    }
}
