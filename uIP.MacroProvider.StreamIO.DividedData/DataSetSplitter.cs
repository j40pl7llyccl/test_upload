using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace uIP.MacroProvider.StreamIO.DividedData
{
    public partial class DataSetSplitter : Form
    {
        private Label lblFolderPath;
        private TextBox txtFolderPath;
        private Button btnSelectFolder;
        private Label lblTrain;
        private Label lblTest;
        private Label lblVal;
        private NumericUpDown numTrain;
        private NumericUpDown numTest;
        private NumericUpDown numVal;
        private Button btnSplit;
        public DataSetSplitter()
        {
            InitializeComponent();
        }

        private void DataSetSplitter_Load(object sender, EventArgs e)
        {

        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFolderPath.Text = folderDialog.SelectedPath;
                }
            }
        }

        private void btnSplit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFolderPath.Text) || !Directory.Exists(txtFolderPath.Text))
            {
                MessageBox.Show("請選擇有效的資料夾！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            double trainRatio = (double)numTrain.Value / 100;
            double testRatio = (double)numTest.Value / 100;
            double valRatio = (double)numVal.Value / 100;

            if (Math.Abs((trainRatio + testRatio + valRatio) - 1.0) > 0.01)
            {
                MessageBox.Show("比例總和必須為100%！", "錯誤", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] files = Directory.GetFiles(txtFolderPath.Text);
            Random random = new Random();
            files = files.OrderBy(f => random.Next()).ToArray();

            int trainCount = (int)(files.Length * trainRatio);
            int testCount = (int)(files.Length * testRatio);
            int valCount = files.Length - trainCount - testCount;

            string trainPath = Path.Combine(txtFolderPath.Text, "train");
            string testPath = Path.Combine(txtFolderPath.Text, "test");
            string valPath = Path.Combine(txtFolderPath.Text, "val");

            Directory.CreateDirectory(trainPath);
            Directory.CreateDirectory(testPath);
            Directory.CreateDirectory(valPath);

            MoveFiles(files.Take(trainCount).ToArray(), trainPath);
            MoveFiles(files.Skip(trainCount).Take(testCount).ToArray(), testPath);
            MoveFiles(files.Skip(trainCount + testCount).ToArray(), valPath);

            MessageBox.Show("數據集劃分完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void MoveFiles(string[] files, string destination)
        {
            foreach (var file in files)
            {
                string destFile = Path.Combine(destination, Path.GetFileName(file));
                File.Move(file, destFile);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void DataSetSplitter_Load_1(object sender, EventArgs e)
        {

        }
    }
}
