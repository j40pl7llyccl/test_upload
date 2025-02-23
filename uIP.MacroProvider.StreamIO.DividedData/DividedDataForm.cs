using System;
using System.Windows.Forms;

namespace uIP.MacroProvider.StreamIO.DividedData
{
    public partial class DividedDataForm : Form
    {
        public DividedDataForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // 建立另一個視窗實例
            DataSetSelect dsForm = new DataSetSelect();

            // 顯示新的視窗
            // 如果你想讓這個新視窗「獨立」顯示，主視窗可繼續操作，就用 Show()
            dsForm.Show();

            // 如果你想讓這個新視窗屬於「模式」視窗(Modal)，
            // 也就是必須關掉新視窗後才能回到主視窗，則用 ShowDialog()
            // dsForm.ShowDialog();
        }
    }
}
