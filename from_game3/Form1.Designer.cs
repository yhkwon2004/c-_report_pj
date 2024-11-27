namespace from_game3
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] {
            "Strike ball"}, -1, System.Drawing.Color.Empty, System.Drawing.Color.Empty, new System.Drawing.Font("굴림", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129))));
            this.label1 = new System.Windows.Forms.Label();
            this.label_eing = new System.Windows.Forms.Label();
            this.domainUpDown1 = new System.Windows.Forms.DomainUpDown();
            this.domainUpDown2 = new System.Windows.Forms.DomainUpDown();
            this.domainUpDown3 = new System.Windows.Forms.DomainUpDown();
            this.hit = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("굴림", 26F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label1.Location = new System.Drawing.Point(49, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(368, 52);
            this.label1.TabIndex = 0;
            this.label1.Text = "숫자 야구 게임";
            // 
            // label_eing
            // 
            this.label_eing.AutoSize = true;
            this.label_eing.Font = new System.Drawing.Font("굴림", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.label_eing.Location = new System.Drawing.Point(118, 73);
            this.label_eing.Name = "label_eing";
            this.label_eing.Size = new System.Drawing.Size(212, 40);
            this.label_eing.TabIndex = 1;
            this.label_eing.Text = "Strike ball ";
            this.label_eing.Click += new System.EventHandler(this.hit_test);
            // 
            // domainUpDown1
            // 
            this.domainUpDown1.Font = new System.Drawing.Font("굴림", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.domainUpDown1.Items.Add("0");
            this.domainUpDown1.Items.Add("1");
            this.domainUpDown1.Items.Add("2");
            this.domainUpDown1.Items.Add("3");
            this.domainUpDown1.Items.Add("4");
            this.domainUpDown1.Items.Add("5");
            this.domainUpDown1.Items.Add("6");
            this.domainUpDown1.Items.Add("7");
            this.domainUpDown1.Items.Add("8");
            this.domainUpDown1.Items.Add("9");
            this.domainUpDown1.Location = new System.Drawing.Point(489, 181);
            this.domainUpDown1.Name = "domainUpDown1";
            this.domainUpDown1.Size = new System.Drawing.Size(123, 173);
            this.domainUpDown1.TabIndex = 2;
            this.domainUpDown1.Text = "0";
            // 
            // domainUpDown2
            // 
            this.domainUpDown2.Font = new System.Drawing.Font("굴림", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.domainUpDown2.Items.Add("0");
            this.domainUpDown2.Items.Add("1");
            this.domainUpDown2.Items.Add("2");
            this.domainUpDown2.Items.Add("3");
            this.domainUpDown2.Items.Add("4");
            this.domainUpDown2.Items.Add("5");
            this.domainUpDown2.Items.Add("6");
            this.domainUpDown2.Items.Add("7");
            this.domainUpDown2.Items.Add("8");
            this.domainUpDown2.Items.Add("9");
            this.domainUpDown2.Location = new System.Drawing.Point(646, 181);
            this.domainUpDown2.Name = "domainUpDown2";
            this.domainUpDown2.Size = new System.Drawing.Size(123, 173);
            this.domainUpDown2.TabIndex = 3;
            this.domainUpDown2.Text = "0";
            this.domainUpDown2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // domainUpDown3
            // 
            this.domainUpDown3.Font = new System.Drawing.Font("굴림", 72F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.domainUpDown3.Items.Add("0");
            this.domainUpDown3.Items.Add("1");
            this.domainUpDown3.Items.Add("2");
            this.domainUpDown3.Items.Add("3");
            this.domainUpDown3.Items.Add("4");
            this.domainUpDown3.Items.Add("5");
            this.domainUpDown3.Items.Add("6");
            this.domainUpDown3.Items.Add("7");
            this.domainUpDown3.Items.Add("8");
            this.domainUpDown3.Items.Add("9");
            this.domainUpDown3.Location = new System.Drawing.Point(795, 181);
            this.domainUpDown3.Name = "domainUpDown3";
            this.domainUpDown3.Size = new System.Drawing.Size(123, 173);
            this.domainUpDown3.TabIndex = 4;
            this.domainUpDown3.Text = "0";
            // 
            // hit
            // 
            this.hit.Font = new System.Drawing.Font("굴림", 48F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.hit.Location = new System.Drawing.Point(585, 393);
            this.hit.Name = "hit";
            this.hit.Size = new System.Drawing.Size(237, 113);
            this.hit.TabIndex = 5;
            this.hit.Text = "HIT!";
            this.hit.UseVisualStyleBackColor = true;
            this.hit.Click += new System.EventHandler(this.hit_test);
            // 
            // listView1
            // 
            this.listView1.BackgroundImageTiled = true;
            this.listView1.Font = new System.Drawing.Font("궁서체", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(129)));
            this.listView1.HideSelection = false;
            this.listView1.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
            this.listView1.Location = new System.Drawing.Point(34, 119);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(401, 450);
            this.listView1.Sorting = System.Windows.Forms.SortOrder.Descending;
            this.listView1.TabIndex = 6;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(972, 590);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.hit);
            this.Controls.Add(this.domainUpDown3);
            this.Controls.Add(this.domainUpDown2);
            this.Controls.Add(this.domainUpDown1);
            this.Controls.Add(this.label_eing);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label_eing;
        private System.Windows.Forms.DomainUpDown domainUpDown1;
        private System.Windows.Forms.DomainUpDown domainUpDown2;
        private System.Windows.Forms.DomainUpDown domainUpDown3;
        private System.Windows.Forms.Button hit;
        private System.Windows.Forms.ListView listView1;
    }
}

