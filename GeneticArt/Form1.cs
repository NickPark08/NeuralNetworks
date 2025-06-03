using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace GeneticArt
{
    public partial class Form1 : Form
    {

        GeneticArtTrainer trainer;
        bool artStarted = false;
        Random random;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            trainer = new GeneticArtTrainer(new Bitmap(pictureBox1.Image), 100, 100);
            random = new Random();
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            artStarted = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(artStarted)
            {
                trainer.Train();
                //trainer.ParallelTrain(random);
                pictureBox2.Image = trainer.GetBestImage(pictureBox2.Width, pictureBox2.Height);
            }
        }
    }

}
