using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace laba1_v45_6
{
    public partial class Form1 : Form
    {
        Analysis analysis = new Analysis();
        AnalysisToken analysisToken = new AnalysisToken();


        public Form1()
        {
            InitializeComponent();
            _Form1 = this;
        }

        private void button1_Click(object sender, EventArgs e) //get path to file
        {
            Analysis.PathToFile(textBox1, richTextBox2);
        }

        private void button2_Click(object sender, EventArgs e) //get analys
        {
            analysis.Gate(richTextBox1, richTextBox2);
        }

        private void button3_Click(object sender, EventArgs e) //get token
        {
            bool check = false;
            analysisToken.ReWork(richTextBox2, richTextBox3, check);
        }
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
        }
        private void richTextBox1_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                int firstcharindex = richTextBox1.GetFirstCharIndexOfCurrentLine();
                int currentline = richTextBox1.GetLineFromCharIndex(firstcharindex);
                string currentlinetext = richTextBox1.Lines[currentline];
                richTextBox1.Select(firstcharindex, currentlinetext.Length + 1);
            }
            catch { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool ll = false;

            analysisToken.ReWork(richTextBox2, richTextBox3, ll);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            bool lr = true;

            analysisToken.ReWork(richTextBox2, richTextBox3, lr);
        }
        public static Form1 _Form1;
        public void update(string message)
        {
            richTextBox4.Text += message + Environment.NewLine;
        }

        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
