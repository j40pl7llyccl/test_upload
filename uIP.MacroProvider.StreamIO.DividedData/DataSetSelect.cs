using System;
using System.IO;
using System.Windows.Forms;
using uIP.MacroProvider.StreamIO.DividedData;

namespace uIP.MacroProvider.StreamIO.DividedData
{
    public partial class DataSetSelect : Form
    {
        public DataSetSelect()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void bt_directory1_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = fbd.SelectedPath;
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void bt_directory2_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = fbd.SelectedPath;
                }
            }
        }

        private void bt_directory3_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    textBox3.Text = fbd.SelectedPath;
                }
            }
        }

        /// <summary>
        /// 用來在 CheckedChanged 事件中，即時阻擋同一行或同一列已經有勾選的情況。
        /// </summary>
        /// <param name="current">這次被勾選/取消的 CheckBox</param>
        /// <param name="rowMate1">同行的其他兩個 CheckBox(之一)</param>
        /// <param name="rowMate2">同行的其他兩個 CheckBox(之二)</param>
        /// <param name="colMate1">同列(同一 Dataset)的其他兩個 CheckBox(之一)</param>
        /// <param name="colMate2">同列(同一 Dataset)的其他兩個 CheckBox(之二)</param>
        /// <param name="rowLabel">給訊息用，如「第1行」</param>
        /// <param name="colLabel">給訊息用，如「Train」或「Test」等</param>
        private void HandleCheckBoxChanged(
            CheckBox current,
            CheckBox rowMate1, CheckBox rowMate2,
            CheckBox colMate1, CheckBox colMate2,
            string rowLabel,   // e.g. "第1行"
            string colLabel    // e.g. "Train"
        )
        {
            // 只有在使用者「勾選」(Checked = true)時才要做阻擋；若是取消勾選就不干涉。
            if (current.Checked)
            {
                // (A) 檢查：同行是否已有勾選
                if (rowMate1.Checked || rowMate2.Checked)
                {
                    MessageBox.Show(
                        $"{rowLabel} 已經勾選了其他類型，不能再勾選 {colLabel}！",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning
                    );
                    // 立刻把這次的勾選取消
                    current.Checked = false;
                    return;
                }

                // (B) 檢查：同列(同一 Dataset)是否在其他行被勾選
                if (colMate1.Checked || colMate2.Checked)
                {
                    MessageBox.Show(
                        $"{colLabel} 已經在其他行被使用，無法重複勾選！",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning
                    );
                    current.Checked = false;
                    return;
                }
            }
        }


        // ======================================================
        // 以下為 9 個 CheckBox，各自的 CheckedChanged 事件
        // ======================================================

        // 第 1 行: Training=checkBox1, Test=checkBox2, Val=checkBox3
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            HandleCheckBoxChanged(
                current: checkBox1,
                rowMate1: checkBox2,   // 同行其他兩個：Test
                rowMate2: checkBox3,   //                Val
                colMate1: checkBox4,   // 同列其他兩個：第2行Train
                colMate2: checkBox7,   //                第3行Train
                rowLabel: "第1行",
                colLabel: "Train"
            );
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            HandleCheckBoxChanged(
                current: checkBox2,
                rowMate1: checkBox1,
                rowMate2: checkBox3,
                colMate1: checkBox5,   // 第2行Test
                colMate2: checkBox8,   // 第3行Test
                rowLabel: "第1行",
                colLabel: "Test"
            );
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            HandleCheckBoxChanged(
                current: checkBox3,
                rowMate1: checkBox1,
                rowMate2: checkBox2,
                colMate1: checkBox6,   // 第2行Val
                colMate2: checkBox9,   // 第3行Val
                rowLabel: "第1行",
                colLabel: "Validation"
            );
        }

        // 第 2 行: Training=checkBox4, Test=checkBox5, Val=checkBox6
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            HandleCheckBoxChanged(
                current: checkBox4,
                rowMate1: checkBox5,
                rowMate2: checkBox6,
                colMate1: checkBox1,   // 第1行Train
                colMate2: checkBox7,   // 第3行Train
                rowLabel: "第2行",
                colLabel: "Train"
            );
        }

        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            HandleCheckBoxChanged(
                current: checkBox5,
                rowMate1: checkBox4,
                rowMate2: checkBox6,
                colMate1: checkBox2,   // 第1行Test
                colMate2: checkBox8,   // 第3行Test
                rowLabel: "第2行",
                colLabel: "Test"
            );
        }

        private void checkBox6_CheckedChanged(object sender, EventArgs e)
        {
            HandleCheckBoxChanged(
                current: checkBox6,
                rowMate1: checkBox4,
                rowMate2: checkBox5,
                colMate1: checkBox3,   // 第1行Val
                colMate2: checkBox9,   // 第3行Val
                rowLabel: "第2行",
                colLabel: "Validation"
            );
        }

        // 第 3 行: Training=checkBox7, Test=checkBox8, Val=checkBox9
        private void checkBox7_CheckedChanged(object sender, EventArgs e)
        {
            HandleCheckBoxChanged(
                current: checkBox7,
                rowMate1: checkBox8,
                rowMate2: checkBox9,
                colMate1: checkBox1,
                colMate2: checkBox4,
                rowLabel: "第3行",
                colLabel: "Train"
            );
        }

        private void checkBox8_CheckedChanged(object sender, EventArgs e)
        {
            HandleCheckBoxChanged(
                current: checkBox8,
                rowMate1: checkBox7,
                rowMate2: checkBox9,
                colMate1: checkBox2,
                colMate2: checkBox5,
                rowLabel: "第3行",
                colLabel: "Test"
            );
        }

        private void checkBox9_CheckedChanged(object sender, EventArgs e)
        {
            HandleCheckBoxChanged(
                current: checkBox9,
                rowMate1: checkBox7,
                rowMate2: checkBox8,
                colMate1: checkBox3,
                colMate2: checkBox6,
                rowLabel: "第3行",
                colLabel: "Validation"
            );
        }

        private int GetFileCount(string path)
        {
            // 若 path 為空或資料夾不存在，就當作 0
            if (string.IsNullOrEmpty(path) || !Directory.Exists(path))
                return 0;

            // 如果要包含所有子資料夾，請改成 SearchOption.AllDirectories
            string[] files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
            return files.Length;
        }

        private void bt_ok_Click(object sender, EventArgs e)
        {
            var plugin = new ProcessPath();

            // 讀取 CheckBox 狀態
            bool r1Train = checkBox1.Checked;
            bool r1Test = checkBox2.Checked;
            bool r1Val = checkBox3.Checked;

            bool r2Train = checkBox4.Checked;
            bool r2Test = checkBox5.Checked;
            bool r2Val = checkBox6.Checked;

            bool r3Train = checkBox7.Checked;
            bool r3Test = checkBox8.Checked;
            bool r3Val = checkBox9.Checked;

            // (A) 檢查(同一行不可多選、同一Dataset不可重複)
            Func<bool, bool, bool, int> countChecks = (a, b, c) => (a ? 1 : 0) + (b ? 1 : 0) + (c ? 1 : 0);

            if (countChecks(r1Train, r1Test, r1Val) > 1)
            {
                MessageBox.Show("第1行同時選了多個 Dataset，請修正。", "警告");
                return;
            }
            if (countChecks(r2Train, r2Test, r2Val) > 1)
            {
                MessageBox.Show("第2行同時選了多個 Dataset，請修正。", "警告");
                return;
            }
            if (countChecks(r3Train, r3Test, r3Val) > 1)
            {
                MessageBox.Show("第3行同時選了多個 Dataset，請修正。", "警告");
                return;
            }

            int trainRowCount = (r1Train ? 1 : 0) + (r2Train ? 1 : 0) + (r3Train ? 1 : 0);
            if (trainRowCount > 1)
            {
                MessageBox.Show("Train Dataset 重複使用於多個路徑，請修正。", "警告");
                return;
            }
            int testRowCount = (r1Test ? 1 : 0) + (r2Test ? 1 : 0) + (r3Test ? 1 : 0);
            if (testRowCount > 1)
            {
                MessageBox.Show("Test Dataset 重複使用於多個路徑，請修正。", "警告");
                return;
            }
            int valRowCount = (r1Val ? 1 : 0) + (r2Val ? 1 : 0) + (r3Val ? 1 : 0);
            if (valRowCount > 1)
            {
                MessageBox.Show("Validation Dataset 重複使用於多個路徑，請修正。", "警告");
                return;
            }

            // ============== 重點：改以「檔案數」來計算比例 ==============
            // (B) 先取得三個路徑的檔案數
            int c1 = GetFileCount(textBox1.Text);
            int c2 = GetFileCount(textBox2.Text);
            int c3 = GetFileCount(textBox3.Text);

            // (C) 將每個路徑對應到的檔案數，累加到 trainCount / testCount / valCount
            int trainCount = 0;
            int testCount = 0;
            int valCount = 0;

            // 第1行(若勾Train，trainCount += c1；若勾Test，testCount += c1；若勾Val，valCount += c1)
            if (r1Train) trainCount += c1;
            if (r1Test) testCount += c1;
            if (r1Val) valCount += c1;

            // 第2行
            if (r2Train) trainCount += c2;
            if (r2Test) testCount += c2;
            if (r2Val) valCount += c2;

            // 第3行
            if (r3Train) trainCount += c3;
            if (r3Test) testCount += c3;
            if (r3Val) valCount += c3;

            // (D) 計算總檔案數
            int total = trainCount + testCount + valCount;
            if (total == 0)
            {
                // 代表三個路徑的檔案數加起來=0 (或都沒勾選)
                // 根據需求可提示
                MessageBox.Show("3個路徑的檔案數為 0，或尚未選擇路徑！", "提醒");
                return;
            }

            // (E) 算百分比
            double trainPercent = (trainCount * 100.0) / total;
            double testPercent = (testCount * 100.0) / total;
            double valPercent = (valCount * 100.0) / total;

            // 您可用三個 label 顯示結果 (或其他方式)
            labelTrainPercent.Text = $"Train: {trainCount} 檔 (約 {trainPercent:0.0}%)";
            labelTestPercent.Text = $"Test : {testCount} 檔 (約 {testPercent:0.0}%)";
            labelValPercent.Text = $"Val  : {valCount} 檔 (約 {valPercent:0.0}%)";

            // (F) 進行搬檔
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                plugin.DoProcessPath(textBox1.Text, r1Train, r1Test, r1Val);
            }
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                plugin.DoProcessPath(textBox2.Text, r2Train, r2Test, r2Val);
            }
            if (!string.IsNullOrEmpty(textBox3.Text))
            {
                plugin.DoProcessPath(textBox3.Text, r3Train, r3Test, r3Val);
            }

            // 提示完成
            MessageBox.Show("資料集分割完成！", "完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }


    }
}

