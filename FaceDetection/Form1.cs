using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using Emgu.CV;
using Emgu.CV.Structure;

namespace FaceDetection
{
    public partial class Form1 : Form
    {

        static readonly CascadeClassifier YuzBelirle = new CascadeClassifier("haarcascade_frontalface_alt_tree.xml");

        FilterInfoCollection devices;
        VideoCaptureDevice myCam;
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            myCam = new VideoCaptureDevice(devices[cmbDevices.SelectedIndex].MonikerString);
            myCam.NewFrame += Dev_NewFrame;
            myCam.Start();
        }

        private void Dev_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap gorsel = (Bitmap)eventArgs.Frame.Clone();
            Image<Bgr, byte> picture = new Image<Bgr, byte>(gorsel);
            
            Rectangle[] frames = YuzBelirle.DetectMultiScale(picture, 1.2, 1);
            foreach(Rectangle frame in frames)
            {
                using (Graphics grafik = Graphics.FromImage(gorsel))
                {
                    using (Pen kalem = new Pen(Color.Red, 5))
                    {
                        grafik.DrawRectangle(kalem, frame);
                    }
                }
            }         
            pictureBox1.Image = gorsel;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach(FilterInfo dev in devices)
            {
                cmbDevices.Items.Add(dev.Name);
            }
            cmbDevices.SelectedIndex = 0;
            myCam = new VideoCaptureDevice();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(myCam.IsRunning)
            {
                myCam.Stop();
            }
        }
    }
}
