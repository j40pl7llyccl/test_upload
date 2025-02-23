using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

using uIP.Lib;
using uIP.Lib.DataCarrier;
using uIP.Lib.Utility;

namespace MainDevelopment
{
    public partial class Form1 : Form
    {
        internal class AnyC
        {
            internal int A { get; set; } = 0;
            internal string S { get; set; } = "";
        }

        //private LogStringToFile m_FileLog = null;
        private string _strInitPath;
        public Form1()
        {
            InitializeComponent();
            _strInitPath = Directory.GetCurrentDirectory();
            ULibAgent.Singleton.InitResources( _strInitPath, this );
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            ULibAgent.Singleton.Dispose();

            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        private void btn_openFolder_Click( object sender, EventArgs e )
        {
            string fullPath = null;
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.SelectedPath = _strInitPath;
            if( dlg.ShowDialog() == DialogResult.OK )
            {
                fullPath = Directory.Exists( dlg.SelectedPath ) ? String.Copy( dlg.SelectedPath ) : null;
            }
            dlg.Dispose(); dlg = null;
            if ( String.IsNullOrEmpty( fullPath ) )
                return;

            //m_FileLog = new LogStringToFile( 50, fullPath, 6, Path.Combine(Directory.GetCurrentDirectory(), "bak") );
        }

        private void btn_addText_Click( object sender, EventArgs e )
        {
            //if ( m_FileLog == null ) return;
            //m_FileLog.MessageLog( tbox_Input.Text );
        }

        private void button_openImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK )
            {
                if (!string.IsNullOrEmpty(dlg.FileName))
                {
                    using(var fs = File.Open(dlg.FileName, FileMode.Open))
                    {
                        var bmp = new Bitmap(fs);
                        UImageBuffer buf = new UImageBuffer();
                        if (buf.FromBitmap(bmp))
                        {
                            var convB = buf.ToBitmap();
                            pictureBox_loadImage.Image?.Dispose();
                            pictureBox_loadImage.Image = convB;
                            pictureBox_loadImage.Width = convB.Width;
                            pictureBox_loadImage.Height = convB.Height;
                        }
                        buf.Dispose();
                        bmp?.Dispose();
                    }
                }
            }
            dlg.Dispose();
        }

        private void button_btnTrim_Click(object sender, EventArgs e)
        {
            if (pictureBox_loadImage.Image == null)
                return;
            if (pictureBox_loadImage.Image is Bitmap b)
            {
                UImageBuffer buf = new UImageBuffer();
                if (buf.FromBitmap(b))
                {
                    UImageBuffer roiBuff = new UImageBuffer();
                    Rectangle r = new Rectangle();
                    if (UImageBuffer.Trim(roiBuff, buf, 
                          new Rectangle(Convert.ToInt32(numericUpDown_roiL.Value), Convert.ToInt32(numericUpDown_roiT.Value), 
                                        Convert.ToInt32(numericUpDown_roiW.Value), Convert.ToInt32(numericUpDown_roiH.Value)),
                          ref r,
                          checkBox_bufPack.Checked))
                    {
                        SaveFileDialog dlg = new SaveFileDialog() { Filter = "bmp|*.bmp|png|*.png" };
                        if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dlg.FileName))
                        {
                            var ext = Path.GetExtension(dlg.FileName).ToLower();
                            ImageFormat format = ImageFormat.Bmp;
                            if (ext == ".png") format = ImageFormat.Png;
                            roiBuff.SaveBmp(dlg.FileName, format);
                        }
                    }
                    roiBuff.Dispose();
                }
                buf.Dispose();
            }
        }

        private void button_convert24_Click(object sender, EventArgs e)
        {
            if (pictureBox_loadImage.Image == null) return;
            if (pictureBox_loadImage.Image is Bitmap b)
            {
                var sb = UImageBuffer.ConvertBitmap(b, PixelFormat.Format24bppRgb, false);
                if(sb != null)
                {
                    SaveFileDialog dlg = new SaveFileDialog() { Filter = "bmp|*.bmp|png|*.png" };
                    if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dlg.FileName))
                    {
                        var ext = Path.GetExtension(dlg.FileName).ToLower();
                        ImageFormat format = ImageFormat.Bmp;
                        if (ext == ".png") format = ImageFormat.Png;
                        sb.Save(dlg.FileName, format);
                    }
                    sb.Dispose();
                }
            }
        }

        private void button_showScriptEditor_Click( object sender, EventArgs e )
        {
            if (ResourceManager.Get(ResourceManager.ScriptEditor) is Form f)
            {
                f?.Show();
            }
        }

        private void button_encrypt_Click( object sender, EventArgs e )
        {
            OpenFileDialog dlg = new OpenFileDialog();
            string filepath = null;
            if (dlg.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(dlg.FileName))
            {
                filepath = string.Copy( dlg.FileName );
            }
            dlg.Dispose();
            if ( string.IsNullOrEmpty( filepath ) )
                return;

            string dstPath = Path.Combine( Path.GetDirectoryName( filepath ), $"{Path.GetFileNameWithoutExtension( filepath )}.bin" );
            FileEncryptUtility.ENC( filepath, dstPath );
        }

        private void button_decrypt_Click( object sender, EventArgs e )
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                FileEncryptUtility.DEC( dlg.FileName, Path.Combine( Path.GetDirectoryName(dlg.FileName), $"{Path.GetFileNameWithoutExtension(dlg.FileName)}_{CommonUtilities.GetCurrentTimeStr("")}" ));
            }
            dlg.Dispose();
        }
    }
}
