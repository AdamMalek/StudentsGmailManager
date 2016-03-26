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
using Topshelf;

namespace NotificationService
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void Start()
        {            
            Service s = new Service();
            s.Run();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Start();
        }
    }
}
