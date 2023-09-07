using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DocV2
{
    public partial class StampView : Form
    {
        public StampView()
        {
            InitializeComponent();
            var path = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DocV2"), "stamp.png");
            if (File.Exists(path))
                pictureBox1.Image = Image.FromFile(path);
        }
        void LoadStamp()
        {
            
        }
        void RemoveStamp()
        {

        }
    }
}
