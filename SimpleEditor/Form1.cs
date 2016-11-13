using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimpleEditor
{
    public partial class MainWindow : Form
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private ImageProcessing.ImageOperation operation = new ImageProcessing.ImageOperation();

        private void menuOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                operation.SetImage(openFileDialog1.FileName);
            }
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            operation.OnImageLoaded += Operation_OnImageLoaded;
            operation.OnImageChanged += Operation_OnImageChanged;
            operation.OnError += Operation_OnError;
            operation.OnExecuted += Operation_OnExecuted;
        }

        private void Operation_OnExecuted(string message)
        {
            status.Text = message;
        }

        private void Operation_OnError(string message)
        {
            status.Text = message;
        }

        private void Operation_OnImageChanged(Bitmap image)
        {
            pictureBox1.Image = image;
        }

        private void Operation_OnImageLoaded(Bitmap image, List<ImageProcessing.Action> availableActions)
        {
            pictureBox1.Image = image;
            var categories = availableActions.Select(s => s.Category).Distinct().ToList();
            this.menu.Items.Clear();
            this.menu.Items.Add(menuOpen);
            foreach (var category in categories)
            {
                var menu = new ToolStripMenuItem();
                menu.Text = category;
                foreach (var item in availableActions.Where(q => q.Category == category).ToArray())
                {
                    var pos = new ToolStripMenuItem();
                    pos.Text = item.ActionName;
                    pos.Name = item.ActionMethod;
                    pos.Click += ActionClick;
                    menu.DropDownItems.Add(pos);
                }
                this.menu.Items.Add(menu);
            }            
        }

        private void ActionClick(object obj, EventArgs args)
        {
            ToolStripMenuItem pos = (ToolStripMenuItem)obj;
            var name = pos.Name;
            var ot = operation.GetType();
            var mi = ot.GetMethod(name);
            mi.Invoke(operation, new object[] { });
        }

        private void menuEdition_Click(object sender, EventArgs e)
        {

        }
    }
}
