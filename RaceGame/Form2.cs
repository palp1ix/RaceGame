using System.Threading;
using System.Windows.Forms;

namespace RaceGame
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            rjProgressBar1.Maximum = 40000;
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {
            if (rjProgressBar1.Value >= 40000)
                this.Close();
            else
                rjProgressBar1.Value += 100;
        }
    }
}
