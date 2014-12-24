using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.VideoSurveillance;
using Emgu.Util;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using System.Diagnostics;
namespace guitest
{
    public partial class Form1 : Form
    {
        Image<Bgr, Byte> src = new Image<Bgr, byte>("C:/Users/shaghayegh/Documents/Visual Studio 2010/Projects/testEmgu2/threshhodlCapture.png");
        Image<Gray, Byte> Graysrc= new Image<Gray, byte>("C:/Users/shaghayegh/Documents/Visual Studio 2010/Projects/testEmgu2/threshhodlCapture.png");
       
        private Capture _capture;
        private MotionHistory _motionHistory;
        private IBGFGDetector<Bgr> _forgroundDetector;
        public Timer My_Timer = new Timer();
        int FPS = 30;
        StreamWriter wr = null;
        List<Image<Bgr, Byte>> image_array = new List<Image<Bgr, Byte>>();
        public Form1()
        {
            InitializeComponent();
            Decimal f=numericUpDownHeight.Value;
            
        }

        private void RotateBottom_Click(object sender, EventArgs e)
        {
            double angle;
            angle = Convert.ToDouble(numericUpDownRotate.Value);
            Bgr color = new Bgr(255, 255, 255);
            Image<Bgr, Byte> dstrotate = src.Rotate(angle, color);
            
            pictureBox1.Image = dstrotate.ToBitmap();
            pictureBox1.SetBounds(93,288 , dstrotate.Width, dstrotate.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(400, 288, src.Width, src.Height);
            
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                Openfile.Filter = "Image Files (*.tif; *.dcm; *.jpg; *.jpeg; *.bmp)|*.tif; *.dcm; *.jpg; *.jpeg; *.bmp *.png";
                src = new Image<Bgr, byte>(Openfile.FileName);
                Graysrc = src.Convert<Gray, Byte>();
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            SaveFileDialog Savefile = new SaveFileDialog();
            if (Savefile.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image.Save(Savefile.FileName + "" + ".jpeg");
            }
           
        }

        private void Scalebuttom_Click(object sender, EventArgs e)//Resize
        {
           int height=Convert.ToInt16(numericUpDownHeight.Value);
           int width = Convert.ToInt16(numericUpDown2.Value);
           Image<Gray, Byte> dst =new Image<Gray,Byte>(src.Size) ;
           dst=Graysrc.Resize(height, width,Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);

           pictureBox1.Image = dst.ToBitmap();
           pictureBox1.SetBounds(93, 288, dst.Width, dst.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(400, 288, src.Width, src.Height);
            
        }

        private void scale_Click(object sender, EventArgs e)
        {
            double scale;
            scale = Convert.ToDouble(ScaletextBox.Text);
            Image<Bgr, Byte> dstResize = src.Resize(scale, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);//mituni scale khali bedi         
            pictureBox1.Image = dstResize.ToBitmap();
            pictureBox1.SetBounds(93,288, dstResize.Width, dstResize.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(400, 288, src.Width, src.Height);
            
        }

        private void EqualizeHistogram_Click(object sender, EventArgs e)
        {
            Image<Gray, Byte> imageH = src.Convert<Gray, Byte>();
            imageH._EqualizeHist();
            Graysrc = imageH;
            pictureBox1.Image = imageH.ToBitmap();
            pictureBox1.SetBounds(93, 288, imageH.Width, imageH.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(400, 288, src.Width, src.Height);
            
        }

        private void Sharp_Click(object sender, EventArgs e)
        {
            int A = Convert.ToInt16(SharpTextBox.Text);
            Image<Gray, Byte> gray = src.Convert<Gray, Byte>();

            float[,] ConvMat1 = new float[3, 3] { { -1, -1, -1 }, { -1, 8 + A, -1 }, { -1, -1, -1 } };
            float[,] ConvMat2 = new float[3, 3] { { 0, -1, 0 }, { -1, 4 + A, -1 }, { 0, -1, 0 } };
            ConvolutionKernelF ConvC = new ConvolutionKernelF(ConvMat2);
            Image<Gray, float> Sharpimage = gray * ConvC;
            pictureBox1.Image = Sharpimage.ToBitmap();
            pictureBox1.SetBounds(93, 288, Sharpimage.Width, Sharpimage.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(400, 288, src.Width, src.Height);
            
        }

        private void strike_Click(object sender, EventArgs e)
        {
            double l = Convert.ToDouble(LT.Text);
            int imin = Convert.ToInt16(iminT.Text);
            int imax = Convert.ToInt16(imaxT.Text);
            Random randx = new Random();
            Random randy = new Random();
            double x;
            double y;

            Image<Gray, Byte> gray;
         
                gray = src.Convert<Gray, Byte>();
            
            for (int i = 0; i < gray.Rows; ++i)
                for (int j = 0; j < gray.Cols; ++j)
                {
                    {
                        x = randx.NextDouble();
                        y = randy.NextDouble();
                        if (x >= l)
                        {
                            gray[i, j] = new Gray(imin + (imax - imin) * y);


                        }

                    }
                }

            pictureBox1.Image = gray.ToBitmap();
            pictureBox1.SetBounds(93, 288, gray.Width, gray.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(400, 288, src.Width, src.Height);
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int a = -20;
            int b = 20;
            Random Uniformrand = new Random();
            int randnumber;

            Image<Gray, Byte> gray;
       
                gray = src.Convert<Gray, Byte>();
            
            for (int i = 0; i < gray.Rows; ++i)
                for (int j = 0; j < gray.Cols; ++j)
                {

                    randnumber = Uniformrand.Next(a, b);
                    gray[i, j] = new Gray(gray[i, j].Intensity + randnumber);

                }


            pictureBox1.Image = gray.ToBitmap();
            pictureBox1.SetBounds(93, 288, gray.Width, gray.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(400, 288, src.Width, src.Height);
            
        }

        private void blur_Click(object sender, EventArgs e)
        {


            int width = Convert.ToInt16(widthT.Text);
            int height = Convert.ToInt16(heightT.Text);
            Image<Gray, Byte> gray=new Image<Gray,Byte>(src.Size);
            Image<Gray, Byte> gray1;// = src.Convert<Gray, Byte>();
            Image<Bgr, Byte> temp = new Image<Bgr, Byte>(src.Size);
            temp=src.Copy();
            CvInvoke.cvCvtColor(temp, gray, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
            gray1 = gray.Convert<Gray, Byte>();
            gray1 = gray1.SmoothBlur(width, height);
            pictureBox2.Image = gray.ToBitmap();
            pictureBox2.SetBounds(400, 288, gray.Width, gray.Height);
            pictureBox1.Image = gray1.ToBitmap();
            pictureBox1.SetBounds(93, 288, gray1.Width, gray1.Height);
        }

        private void poisson_Click(object sender, EventArgs e)
        {
            Image<Gray, Byte> gray;
       
                gray = src.Convert<Gray, Byte>();
            

            double landa = Convert.ToDouble(landaT.Text);
            double[] pdf = new double[100];
            //
            pdf[0] = Math.Exp(-landa);
            for (int i = 1; i < 100; ++i)
            {
                pdf[i] = pdf[i - 1] * landa / i;
            }
            Random randomx = new Random();
            int x;
            double NormalNoise;
            for (int i = 0; i < gray.Rows; ++i)
                for (int j = 0; j < gray.Cols; ++j)
                {

                    x = randomx.Next(0, 99);
                    NormalNoise = 100 * pdf[x];
                    gray[i, j] = new Gray(gray[i, j].Intensity + NormalNoise);

                }
            pictureBox1.Image = gray.ToBitmap();
            pictureBox1.SetBounds(93, 288, gray.Width, gray.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(400, 288, src.Width, src.Height);
            
        }

        private void GaussianFilter_Click(object sender, EventArgs e)
        {
            int sizematrix = Convert.ToInt16(MatSizeT.Text);
            Image<Gray, Byte> gray;
            Image<Gray, Byte> gray1;// = src.Convert<Gray, Byte>();
       
                gray = src.Convert<Gray, Byte>();
            
            gray1 = gray.SmoothGaussian(sizematrix);
            pictureBox1.Image = gray1.ToBitmap();
            pictureBox1.SetBounds(93,288, gray1.Width, gray1.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(400, 288, src.Width, src.Height);
        }

        private void Gaussiannoise_Click(object sender, EventArgs e)
        {
            double variation = Convert.ToDouble(sigmaT.Text);
            double mean = Convert.ToDouble(meanT.Text);
            /* double fraction=(1/((Math.Sqrt(2*Math.PI))*variation));            
             double a=2*Math.Pow(variation,2);
             double NormalNoise=0;
             double x;
             Random randomx= new Random();

             Image<Gray, Byte> gray;
             Image<Gray, Byte> gray2;
             if (checkBox1.Checked)
             {
                 gray = grayglobal.Convert<Gray, Byte>();
             }
             else
             {
                 gray = src.Convert<Gray, Byte>();
             }

            for (int i = 0; i < gray.Rows; ++i)
                 for (int j = 0; j < gray.Cols; ++j)
                 {
                     x = randomx.NextDouble();
                    
                    // x= randomx.Next(-1, 1);
                     NormalNoise =50*fraction*Math.Exp((-Math.Pow(x,2))/(a));
                     gray[i,j] = new Gray(gray[i,j].Intensity +NormalNoise);

                 }
            dstgray = gray;
             grayglobal = gray;
             saveflag = 2;
             pictureShow.Image = gray.ToBitmap();
             pictureShow.SetBounds(242, 63, gray.Width, gray.Height);*/

            Image<Gray, Byte> gray2;
            Image<Gray, Byte> gray;

            gray = src.Convert<Gray, Byte>();

            gray2 = gray.Copy();
            gray2.SetRandNormal(new MCvScalar(mean), new MCvScalar(variation));
            gray = gray.Or(gray2);

            pictureBox1.Image = gray.ToBitmap();
            pictureBox1.SetBounds(93, 288, gray2.Width, gray2.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(400, 288, src.Width, src.Height);

        }

        private void median_Click(object sender, EventArgs e)
        {
            int sizematrix = Convert.ToInt16(MatMedian.Text);
            Image<Gray, Byte> gray;
            Image<Gray, Byte> gray1;// = src.Convert<Gray, Byte>();
     
                gray = src.Convert<Gray, Byte>();
            
            gray1 = gray.SmoothMedian(sizematrix);

            pictureBox1.Image = gray1.ToBitmap();
            pictureBox1.SetBounds(93, 288, gray1.Width, gray1.Height);
        }

        private void canny_Click(object sender, EventArgs e)
        {
            Image<Gray, Byte> cannyEdges = new Image<Gray, byte>(src.Size);
            Image<Gray, Byte> Grayimg = new Image<Gray, Byte>(src.Size);
            CvInvoke.cvCvtColor(src, Grayimg, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
            //  //حداقل آستانه//حداکثر آستانه//ابعادکرنل
            int Th =Convert.ToInt16(THC.Text);
            int THL = Convert.ToInt16(THLC.Text);
            CvInvoke.cvCanny(Grayimg, cannyEdges, Th, THL, 3);//**&&&astane
            pictureBox1.Image = cannyEdges.ToBitmap();
            pictureBox1.SetBounds(93, 288, cannyEdges.Width, cannyEdges.Height);
            
        }

        private void marr_Click(object sender, EventArgs e)
        {
            Image<Gray, float> cannyEdges = new Image<Gray, float>(src.Size);
            Image<Gray, Byte> Grayimg = new Image<Gray, Byte>(src.Size);
            CvInvoke.cvCvtColor(src, Grayimg, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
            int cernelsize = Convert.ToInt16(cernelSizemarr.Text);
            double sigma12 = Convert.ToDouble(sigma1.Text);
            double sigma21 = Convert.ToDouble(sigma2.Text);
            cannyEdges = Grayimg.Laplace(cernelsize);//اندازه ی کرنل&&&
            cannyEdges = cannyEdges.SmoothGaussian(cernelsize, cernelsize, sigma12, sigma21);//&&&اولی طول کرنل،2-عرض کرنل،3سیگما1،4-سیگما2
            pictureBox1.Image = cannyEdges.ToBitmap();
            pictureBox1.SetBounds(93, 288, cannyEdges.Width, cannyEdges.Height);
        }

        private void navatia_Click(object sender, EventArgs e)
        {
            int thr = Convert.ToInt32(navatiath.Text);
            Image<Bgr, Byte> res = NevatiaBabuFilter(src, thr);
            pictureBox1.Image = res.ToBitmap();
            pictureBox1.SetBounds(93, 288, res.Width, res.Height);
        }
        Image<Bgr, byte> NevatiaBabuFilter(Image<Bgr, byte> src, int thr)/***mikhay un ielio bezar**/
        {
            Image<Bgr, byte> grad = new Image<Bgr, byte>(src.Size);
            int[,] mask1 = new int[5, 5] { { 100, 100, 100, 100, 100 }, { 100, 100, 100, 100, 100 }, { 0, 0, 0, 0, 0 }, { -100, -100, -100, -100, -100 }, { -100, -100, -100, -100, -100 } };
            int[,] mask2 = new int[5, 5] { { 100, 100, 100, 100, 100 }, { 100, 100, 100, 78, -32 }, { 100, 92, 0, -92, -100 }, { 32, -78, -100, -100, -100 }, { -100, -100, -100, -100, -100 } };
            int[,] mask3 = new int[5, 5] { { 100, 100, 100, 32, -100 }, { 100, 100, 92, -78, -100 }, { 0, 100, 0, -100, -100 }, { 100, 78, -92, -100, -100 }, { 100, -32, -100, -100, -100 } };
            int[,] mask4 = new int[5, 5] { { -100, -100, 0, 100, 100 }, { -100, -100, 0, 100, 100 }, { -100, -100, 0, 100, 100 }, { -100, -100, 0, 100, 100 }, { -100, -100, 0, 100, 100 } };
            int[,] mask5 = new int[5, 5] { { -100, 32, 100, 100, 100 }, { -100, -78, 92, 100, 100 }, { -100, -100, 0, 100, 100 }, { -100, -100, -92, 78, 100 }, { -100, -100, -100, -32, 100 } };
            int[,] mask6 = new int[5, 5] { { 100, 100, 100, 100, 100 }, { -32, 78, 100, 100, 100 }, { -100, -92, 0, 92, 100 }, { -100, -100, -100, -78, 32 }, { -100, -100, -100, -100, -100 } };

            int[] sum = new int[6] { 0, 0, 0, 0, 0, 0 };
            for (int i = 2; i < src.Rows - 2; i++)
            {
                for (int j = 2; j < src.Cols - 2; j++)
                {
                    sum[0] = 0;
                    for (int x = -2; x < 3; x++)
                    {
                        for (int y = -2; y < 3; y++)
                        {
                            sum[0] += src.Data[i + x, j + y, 0] * mask1[x + 2, y + 2];
                        }
                    }
                    sum[1] = 0;
                    for (int x = -2; x < 3; x++)
                    {
                        for (int y = -2; y < 3; y++)
                        {
                            sum[1] += ((src.Data[i + x, j + y, 0])) * mask2[x + 2, y + 2];
                        }
                    }
                    sum[2] = 0;
                    for (int x = -2; x < 3; x++)
                    {
                        for (int y = -2; y < 3; y++)
                        {
                            sum[2] += ((src.Data[i + x, j + y, 0])) * mask3[x + 2, y + 2];
                        }
                    }
                    sum[3] = 0;
                    for (int x = -2; x < 3; x++)
                    {
                        for (int y = -2; y < 3; y++)
                        {
                            sum[3] += ((src.Data[i + x, j + y, 0])) * mask4[x + 2, y + 2];
                        }
                    }
                    sum[4] = 0;
                    for (int x = -2; x < 3; x++)
                    {
                        for (int y = -2; y < 3; y++)
                        {
                            sum[4] += ((src.Data[i + x, j + y, 0])) * mask5[x + 2, y + 2];
                        }
                    }
                    sum[5] = 0;
                    for (int x = -2; x < 3; x++)
                    {
                        for (int y = -2; y < 3; y++)
                        {
                            sum[5] += ((src.Data[i + x, j + y, 0])) * mask6[x + 2, y + 2];
                        }
                    }
                    int max1 = sum[0];
                    for (int h = 0; h < 5; h++)
                    {
                        max1 = Math.Max(max1, sum[h]);
                    }

                    if (max1 > thr)
                    {
                        grad.Data[i, j, 0] = 0;
                        grad.Data[i, j, 1] = 255;//noghate roye labe be range sabz dar miad shom mitunid avazesh konid
                        grad.Data[i, j, 2] = 0;
                    }
                    else
                    {
                        grad.Data[i, j, 0] = src.Data[i, j, 0];
                        grad.Data[i, j, 1] = src.Data[i, j, 1];
                        grad.Data[i, j, 2] = src.Data[i, j, 2];
                    }
                }
            }
            return grad;
        }

        private void sobel_Click(object sender, EventArgs e)
        {
            Image<Gray, float> dst;
            Image<Gray, Byte> Grayimg = new Image<Gray, Byte>(src.Size);
            CvInvoke.cvCvtColor(src, Grayimg, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
            int xorder1=Convert.ToInt16(xorder.Text);
            int yorder1 = Convert.ToInt16(yorder.Text);
            int apreture1 = Convert.ToInt16(aperture.Text);             
            dst = Grayimg.Sobel(xorder1, yorder1, apreture1);//&&&&avali va dovomi jahate moshtagh ro neshun midan ke ya x bashe ya y osilan 0,1 ya 1,0 dar nazar gerefte mishan sevomi ham ke andazeye kernele
            pictureBox1.Image = dst.ToBitmap();
            pictureBox1.SetBounds(93, 288,dst.Width, dst.Height);
        }

        private void hough_Click(object sender, EventArgs e)
        {
             Image<Gray, Byte> gray = src.Convert<Gray, Byte>().PyrDown().PyrUp();

            Gray cannyThreshold = new Gray(180);
            Gray cannyThresholdLinking = new Gray(120);
            Gray circleAccumulatorThreshold = new Gray(120);

            CircleF[] circles = gray.HoughCircles(
                cannyThreshold,
                circleAccumulatorThreshold,
                5.0, //Resolution of the accumulator used to detect centers of the circles
                10.0, //min distance 
                5, //min radius
                0 //max radius
                )[0]; //Get the circles from the first channel
            //      
            double cannyThresholdD1 = 10;
            double cannytreshholdD2 = 20;
            Image<Gray, Byte> cannyEdges = gray.Canny(cannyThresholdD1, cannytreshholdD2);
            LineSegment2D[] lines = cannyEdges.HoughLinesBinary(
                1, //Distance resolution in pixel-related units
                Math.PI / 45.0, //Angle resolution measured in radians.
                20, //threshold
                30, //min Line width
                10 //gap between lines
                )[0]; //Get the lines from the first channel

            #region Find triangles and rectangles
            List<Triangle2DF> triangleList = new List<Triangle2DF>();
            List<MCvBox2D> boxList = new List<MCvBox2D>();

            using (MemStorage storage = new MemStorage()) //allocate storage for contour approximation
                for (Contour<Point> contours = cannyEdges.FindContours(); contours != null; contours = contours.HNext)
                {
                    Contour<Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.05, storage);

                    if (contours.Area > 250) //only consider contours with area greater than 250
                    {
                        if (currentContour.Total == 3) //The contour has 3 vertices, it is a triangle
                        {
                            Point[] pts = currentContour.ToArray();
                            triangleList.Add(new Triangle2DF(
                               pts[0],
                               pts[1],
                               pts[2]
                               ));
                        }
                        else if (currentContour.Total == 4) //The contour has 4 vertices.
                        {
                            #region determine if all the angles in the contour are within the range of [80, 100] degree
                            bool isRectangle = true;
                            Point[] pts = currentContour.ToArray();
                            LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                            for (int i = 0; i < edges.Length; i++)
                            {
                                double angle = Math.Abs(
                                   edges[(i + 1) % edges.Length].GetExteriorAngleDegree(edges[i]));
                                if (angle < 80 || angle > 100)
                                {
                                    isRectangle = false;
                                    break;
                                }
                            }
                            #endregion

                            if (isRectangle) boxList.Add(currentContour.GetMinAreaRect());
                        }
                    }
                }
            #endregion

            pictureBox1.Image = src.ToBitmap();
           
            #region draw triangles and rectangles
            Image<Bgr, Byte> triangleRectangleImage = src.CopyBlank();
            foreach (Triangle2DF triangle in triangleList)
                triangleRectangleImage.Draw(triangle, new Bgr(Color.DarkBlue), 2);
            foreach (MCvBox2D box in boxList)
                triangleRectangleImage.Draw(box, new Bgr(Color.DarkOrange), 2);
            if (traiangleradio.Checked)
            {
                pictureBox1.Image = triangleRectangleImage.ToBitmap();
                pictureBox1.SetBounds(93, 288, triangleRectangleImage.Width, triangleRectangleImage.Height);
            }
            #endregion

            #region draw circles
            Image<Bgr, Byte> circleImage = src.CopyBlank();
            foreach (CircleF circle in circles)
                circleImage.Draw(circle, new Bgr(Color.Brown), 2);
            if (circleradio.Checked)
            {
                pictureBox1.Image = circleImage.ToBitmap();
                pictureBox1.SetBounds(93, 288, circleImage.Width, circleImage.Height);
            }
            #endregion

            #region draw lines
            Image<Bgr, Byte> lineImage = src.CopyBlank();
            foreach (LineSegment2D line in lines)
                lineImage.Draw(line, new Bgr(Color.Green), 2);
            if (lineradio.Checked)
            {
                pictureBox1.Image = lineImage.ToBitmap();
                pictureBox1.SetBounds(93, 288, lineImage.Width, lineImage.Height);
            }
        
            #endregion

        }

        private void Pyramid_Click(object sender, EventArgs e)
        {
            Image<Gray, Byte> grayImage = new Image<Gray, Byte>(src.Size);
            CvInvoke.cvCvtColor(src, grayImage, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
            grayImage = grayImage.PyrDown();
            grayImage = grayImage.PyrDown();
            grayImage = grayImage.PyrDown();
            Gray cannyThreshold = new Gray(180);
            Gray cannyThresholdLinking = new Gray(120);
            Image<Gray, Byte> cannyImage = new Image<Gray, Byte>(grayImage.Size);
            CvInvoke.cvCanny(grayImage, cannyImage, 10, 60, 3);
            Image<Gray, Byte> cannyEdges = grayImage.Canny(180, 120);
            LineSegment2D[] lines = cannyEdges.HoughLinesBinary(
                1, //Distance resolution in pixel-related units
                Math.PI / 45.0, //Angle resolution measured in radians.
                20, //threshold
                30, //min Line width
                10 //gap between lines
                )[0]; //Get the lines from the first channel
            Image<Bgr, Byte> bgrgrayimage = new Image<Bgr, Byte>(grayImage.Size);
            Image<Bgr, Byte> BoundryImage = bgrgrayimage.CopyBlank();

            StructuringElementEx kernel = new StructuringElementEx(3, 3, 1, 1, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_ELLIPSE);
            CvInvoke.cvDilate(cannyImage, cannyImage, kernel, 1);

            IntPtr cont = IntPtr.Zero;

            Point[] pts;
            Point p = new Point(0, 0);
            using (MemStorage storage = new MemStorage()) //allocate storage for contour approximation
                for (Contour<Point> contours = cannyImage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                  Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL); contours != null; contours = contours.HNext)
                {
                    pts = contours.ToArray();
                    BoundryImage.DrawPolyline(pts, true, new Bgr(255, 0, 255), 1);

                }
            BoundryImage = BoundryImage.PyrUp();
            BoundryImage = BoundryImage.PyrUp();
            BoundryImage = BoundryImage.PyrUp();
            pictureBox1.Image = BoundryImage.ToBitmap();
            pictureBox1.SetBounds(93, 288, BoundryImage.Width, BoundryImage.Height);
            

        }
           struct path
        {
            public int x;
            public int y;
        };
        private void bordarTracing_Click(object sender, EventArgs e)
        {
              Image<Gray, Byte> img1= new Image<Gray, Byte>(src.Size);
            Image<Gray, Byte> img2 = new Image<Gray, Byte>(src.Size);

            Image<Bgr, Byte> img3 = new Image<Bgr, Byte>(src.Size);
            int dimK = 3;
            float minThr = 10;
            float maxthr = 60;
           
            
            CvInvoke.cvCvtColor(src, img1,Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
            img1=img1.SmoothBlur(3,3);
            CvInvoke.cvCanny(img1, img2, minThr, maxthr, dimK);
            path[] Dir = new path[8];
            int[,] contpurMat=new int[src.Height,src.Width];
            CvInvoke.cvCvtColor(img2, img3, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_GRAY2BGR);
            pictureBox1.Image = img3.ToBitmap();
            
            Dir[0].y = 0; //y
            Dir[0].x = 1;//x
            Dir[1].y = -1;//y 
            Dir[1].x = 1;//x
            Dir[2].y = -1;
            Dir[2].x = 0;
            Dir[3].y = -1;
            Dir[3].x = -1;
            Dir[4].y = 0;
            Dir[4].x = -1;
            Dir[5].y = 1;
            Dir[5].x = -1;
            Dir[6].y = 1;
            Dir[6].x = 0;
            Dir[7].y = 1;
            Dir[7].x = 1;
            int dir = 7;
         
            
            for (int j = 0; j < img3.Height; j++)
            {
                for (int i = 0; i < img3.Width; i++)
                {

                    if (img3.Data[j, i, 0] == 255)
                    {                      
                                for (int q = 0; q < img3.Height; q++)
                                {
                                    for (int w = 0; w < img3.Width; w++)
                                    {
                                        contpurMat[q, w] = 0;
                                     }
                                }
                        path p0,p1,ptn0,pt;
                        p0.y = j;
                        p0.x = i;
                        int temp = 0;
                        pt = p0;
                        p1 = p0;
                        ptn0 = p0;
                        bool Flag = true;
                        bool flag2 = false;
                        contpurMat[j, i] = 1;
                        int counter = 0;
                        while (true)
                        {
                            int dirt;
                          
                            if (dir % 2 == 0)
                            {
                                dirt = (dir + 7) % 8;
                            }
                            else
                            {
                                dirt = (dir + 6) % 8;
                            }
                            if (pt.y - 1 >= 0 && pt.x - 1 >= 0 && pt.y + 1 < img3.Height && pt.x + 1 < img3.Width)
                            {
                                 temp = dirt+4;
                                for (int k = 0; k < 8; k++)
                                {
                                    if (img3.Data[pt.y + Dir[dirt].y, pt.x + Dir[dirt].x, 0] == 255)
                                    {
                                        counter++;
                                        if (Flag == true)
                                        {
                                            p1.y = pt.y + Dir[dirt].y;
                                            p1.x = pt.x + Dir[dirt].x;
                                            Flag = false;
                                        }
                                        dir = dirt;
                                        contpurMat[pt.y + Dir[dirt].y, pt.x + Dir[dirt].x] = 1;
                                       
                                        ptn0.y = pt.y;
                                        ptn0.x = pt.x;
                                        pt.y += Dir[dirt].y;
                                        pt.x += +Dir[dirt].x;
                                        break;
                                    }
                                    dirt++;
                                    dirt %= 8;
                                    if (dirt == temp)
                                    {
                                        break;
                                        flag2 = true;
                                    }
                                }
                            }
                            else
                            {
                                break;
                            }
                            if (flag2 == true)
                            {
                                break;
                            }
                            if ((counter>1)&&(ptn0.y == p0.y) && (ptn0.x == p0.x) && (pt.y == p1.y) && (pt.x == p1.x))
                            {
                                for (int q = 0; q < img3.Height; q++)
                                {
                                    for (int w = 0; w < img3.Width; w++)
                                    {
                                        if (contpurMat[q, w] == 1)
                                        {
                                            img3.Data[q, w, 0] = 0;
                                            img3.Data[q, w, 1] = 255;
                                            img3.Data[q, w, 2] = 0;
                                        }
                                        }
                                }
                                break;
                            }
                        }
                    }
                }
            }//end for

            pictureBox1.Image = img3.ToBitmap();
            pictureBox1.SetBounds(93, 288, img3.Width, img3.Height);

        }

        private void contourbotton_Click(object sender, EventArgs e)
        {
            Gray cannyThreshold = new Gray(180);
            Gray cannyThresholdLinking = new Gray(120);
            Image<Gray, Byte> grayImage = src.Convert<Gray, Byte>().PyrDown().PyrUp();
            Image<Gray, Byte> cannyImage = new Image<Gray, Byte>(grayImage.Size);
            CvInvoke.cvCanny(grayImage, cannyImage, 10, 60, 3);
            Image<Gray, Byte> cannyEdges = grayImage.Canny(180, 120);
            LineSegment2D[] lines = cannyEdges.HoughLinesBinary(
                1, //Distance resolution in pixel-related units
                Math.PI / 45.0, //Angle resolution measured in radians.
                20, //threshold
                30, //min Line width
                10 //gap between lines
                )[0]; //Get the lines from the first channel
            Image<Bgr, Byte> BoundryImage = src.CopyBlank();

            StructuringElementEx kernel = new StructuringElementEx(3, 3, 1, 1, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_ELLIPSE);
            CvInvoke.cvDilate(cannyImage, cannyImage, kernel, 1);

            IntPtr cont = IntPtr.Zero;

            Point[] pts;
            Point p = new Point(0, 0);
            using (MemStorage storage = new MemStorage()) //allocate storage for contour approximation
                for (Contour<Point> contours = cannyImage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                  Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL); contours != null; contours = contours.HNext)
                {
                    pts = contours.ToArray();
                    BoundryImage.DrawPolyline(pts, true, new Bgr(255, 0, 255), 1);

                }
            
            pictureBox1.Image = BoundryImage.ToBitmap();
            pictureBox1.SetBounds(93, 288, BoundryImage.Width, BoundryImage.Height);

        }

    
        private void Adaptive_Click(object sender, EventArgs e)
        {
            int threshold_value = 50; //0-255//???????????
            int blocksize = 3;
            blocksize = Convert.ToInt16(textBox1.Text);
            Image<Gray, Byte> img1 = new Image<Gray, Byte>(src.Size);
            CvInvoke.cvCvtColor(src, img1, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
            
            img1 = img1.ThresholdAdaptive(new Gray(255), Emgu.CV.CvEnum.ADAPTIVE_THRESHOLD_TYPE.CV_ADAPTIVE_THRESH_GAUSSIAN_C, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY, blocksize, new Gray(10));
            pictureBox1.Image = img1.ToBitmap();
            pictureBox1.SetBounds(93, 288, img1.Width, img1.Height);
        }

       
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                //Binary
                int threshold_value = 50; //0-255
                threshold_value = trackBar2.Value ;
                Image<Gray, Byte> img1 = new Image<Gray, Byte>(src.Size);
                CvInvoke.cvCvtColor(src, img1, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
                img1 = img1.ThresholdBinary(new Gray(threshold_value), new Gray(255));
                pictureBox1.Image = img1.ToBitmap();
                pictureBox1.SetBounds(93, 288, img1.Width, img1.Height);
            }
            else if (comboBox1.SelectedIndex == 1)
            {
                //BinaryInv
                int threshold_value = 50; //0-255
                threshold_value = trackBar2.Value;
                Image<Gray, Byte> img1 = new Image<Gray, Byte>(src.Size);
                CvInvoke.cvCvtColor(src, img1, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
                img1 = img1.ThresholdBinaryInv(new Gray(threshold_value), new Gray(255));
                pictureBox1.Image = img1.ToBitmap();
                pictureBox1.SetBounds(93, 288, img1.Width, img1.Height);
            }
            else if (comboBox1.SelectedIndex == 2)
            {
                //ToZero
                int threshold_value = 50; //0-255
                threshold_value = trackBar2.Value ;
                Image<Gray, Byte> img1 = new Image<Gray, Byte>(src.Size);
                CvInvoke.cvCvtColor(src, img1, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
                img1 = img1.ThresholdToZero(new Gray(threshold_value));
                pictureBox1.Image = img1.ToBitmap();
                pictureBox1.SetBounds(93, 288, img1.Width, img1.Height);
            }
            else if (comboBox1.SelectedIndex == 3)
            {
                //ToZeroInv
                int threshold_value = 50; //0-255
                threshold_value = trackBar2.Value ;
                Image<Gray, Byte> img1 = new Image<Gray, Byte>(src.Size);
                CvInvoke.cvCvtColor(src, img1, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
                img1 = img1.ThresholdToZeroInv(new Gray(threshold_value));
                pictureBox1.Image = img1.ToBitmap();
                pictureBox1.SetBounds(93, 288, img1.Width, img1.Height);
            }
            else if (comboBox1.SelectedIndex == 4)
            {
                //Trunc
                int threshold_value = 50; //0-255
                threshold_value = trackBar2.Value ;
                Image<Gray, Byte> img1 = new Image<Gray, Byte>(src.Size);
                CvInvoke.cvCvtColor(src, img1, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
                img1 = img1.ThresholdTrunc(new Gray(threshold_value));
                pictureBox1.Image = img1.ToBitmap();
                pictureBox1.SetBounds(93, 288, img1.Width, img1.Height);
            }
            
        }

        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            int Blue_threshold = 50; //0-255
            int Green_threshold = 50; //0-255
            int Red_threshold = 50; //0-255
            Blue_threshold = trackBar6.Value;
            Green_threshold = trackBar7.Value;
            Red_threshold = trackBar8.Value;
            Image<Bgr, Byte> img_colour = new Image<Bgr, Byte>(src.Size);
            img_colour = src.Copy();
            Image<Bgr, Byte> img_colour2 = img_colour.Resize(0.4, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            pictureBox2.Image = img_colour2.ToBitmap(); //Display if you want
            pictureBox2.SetBounds(93, 288, img_colour2.Width, img_colour.Height);
            img_colour2 = img_colour2.ThresholdBinary(new Bgr(Blue_threshold, Green_threshold, Red_threshold), new Bgr(255, 255, 255));
            pictureBox1.Image = img_colour2.ToBitmap();//display results in different picturebox
            pictureBox1.SetBounds(599, 288, img_colour2.Width, img_colour.Height);
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            int Blue_threshold = 50; //0-255
            int Green_threshold = 50; //0-255
            int Red_threshold = 50; //0-255
            Blue_threshold = trackBar6.Value;
            Green_threshold = trackBar7.Value;
            Red_threshold = trackBar8.Value;
            Image<Bgr, Byte> img_colour = new Image<Bgr, Byte>(src.Size);
            img_colour = src.Copy();
            Image<Bgr, Byte> img_colour2 = img_colour.Resize(0.4, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            pictureBox2.Image = img_colour2.ToBitmap(); //Display if you want
            pictureBox2.SetBounds(93, 288, img_colour2.Width, img_colour.Height);
            img_colour2 = img_colour2.ThresholdBinary(new Bgr(Blue_threshold, Green_threshold, Red_threshold), new Bgr(255, 255, 255));
            pictureBox1.Image = img_colour2.ToBitmap();//display results in different picturebox
            pictureBox1.SetBounds(599, 288, img_colour2.Width, img_colour.Height);
        }

        private void trackBar8_Scroll(object sender, EventArgs e)
        {
            int Blue_threshold = 50; //0-255
            int Green_threshold = 50; //0-255
            int Red_threshold = 50; //0-255
            Blue_threshold = trackBar6.Value;
            Green_threshold = trackBar7.Value;
            Red_threshold = trackBar8.Value;
            Image<Bgr, Byte> img_colour = new Image<Bgr, Byte>(src.Size);
            img_colour = src.Copy();
            Image<Bgr, Byte> img_colour2 = img_colour.Resize(0.4, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
            pictureBox2.Image = img_colour2.ToBitmap(); //Display if you want
            pictureBox2.SetBounds(93, 288, img_colour2.Width, img_colour.Height);
            img_colour2 = img_colour2.ThresholdBinary(new Bgr(Blue_threshold, Green_threshold, Red_threshold), new Bgr(255, 255, 255));
            pictureBox1.Image = img_colour2.ToBitmap();//display results in different picturebox
            pictureBox1.SetBounds(599, 288, img_colour2.Width, img_colour.Height);
        }

        private void FloodFill_Click(object sender, EventArgs e)
        {
            Image<Bgr, Byte> My_Image =src;
            int spx1 = Convert.ToInt16(SPX.Text);
            int spy1 = Convert.ToInt16(SPY.Text);
            Point seedPoint = new Point(spx1, spy1);
            MCvScalar newVal = new MCvScalar(255, 0, 0);
            MCvScalar lo_diff = new MCvScalar(8, 90, 60);
            MCvScalar up_diff = new MCvScalar(10, 100, 70);
            Image<Gray, Byte> grImg = new Image<Gray, Byte>(My_Image.Width, My_Image.Height);
            Image<Gray, Byte> fillMask = new Image<Gray, Byte>(My_Image.Width + 2, My_Image.Height + 2);
            MCvConnectedComp comp = new MCvConnectedComp();

            CvInvoke.cvFloodFill(My_Image.Ptr,
                seedPoint,
                newVal,
                lo_diff,
                up_diff,
                out comp,
                Emgu.CV.CvEnum.CONNECTIVITY.EIGHT_CONNECTED,
                Emgu.CV.CvEnum.FLOODFILL_FLAG.DEFAULT,
                fillMask
                );
           
            pictureBox1.Image = My_Image.ToBitmap();
            pictureBox1.SetBounds(93, 288, My_Image.Width, My_Image.Height);
        }


        
        Image<Bgr, Byte> frameDiff2=new Image<Bgr, byte>("C:/Users/shaghayegh/Documents/Visual Studio 2010/Projects/testEmgu2/3.png");
        Image<Bgr, Byte> frameDiff1 = new Image<Bgr, byte>("C:/Users/shaghayegh/Documents/Visual Studio 2010/Projects/testEmgu2/2.png");
           
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                Openfile.Filter = "Image Files (*.tif; *.dcm; *.jpg; *.jpeg; *.bmp)|*.tif; *.dcm; *.jpg; *.jpeg; *.bmp";
                frameDiff1 = new Image<Bgr, byte>(Openfile.FileName);
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog Openfile = new OpenFileDialog();
            if (Openfile.ShowDialog() == DialogResult.OK)
            {
                Openfile.Filter = "Image Files (*.tif; *.dcm; *.jpg; *.jpeg; *.bmp)|*.tif; *.dcm; *.jpg; *.jpeg; *.bmp";
                frameDiff2 = new Image<Bgr, byte>(Openfile.FileName);
            }
        }
        private void MotionButtom_Click(object sender, EventArgs e)
        {
            
            
            pictureBox1.Image = frameDiff1.ToBitmap();
            pictureBox1.SetBounds(12, 267, frameDiff1.Width, frameDiff1.Height);
            pictureBox2.Image = frameDiff2.ToBitmap();
            pictureBox2.SetBounds(355, 267, frameDiff1.Width, frameDiff1.Height);
            Image<Bgr, Byte> flow = new Image<Bgr, byte>(frameDiff1.Size);
            Image<Bgr, Byte> velx11 = new Image<Bgr, byte>(frameDiff1.Size);
            Image<Bgr, Byte> vely11 = new Image<Bgr, byte>(frameDiff1.Size);
            CvInvoke.cvAbsDiff(frameDiff1, frameDiff2, flow);
            CvInvoke.cvThreshold(frameDiff1, frameDiff2, 100, 100, 0);
            CvInvoke.cvErode(frameDiff1, frameDiff2, IntPtr.Zero, 2);
            pictureBox3.Image = flow.ToBitmap();
            pictureBox3.SetBounds(734, 267, frameDiff1.Width, frameDiff1.Height);
        }




        private void ProcessFrame(object sender, EventArgs e)
        {
            using (Image<Bgr, Byte> image = _capture.RetrieveBgrFrame())
            {
            using (MemStorage storage = new MemStorage()) //create storage for motion components
            {
                if (_forgroundDetector == null)
                {
                    //_forgroundDetector = new BGCodeBookModel<Bgr>();
                    _forgroundDetector = new FGDetector<Bgr>(Emgu.CV.CvEnum.FORGROUND_DETECTOR_TYPE.FGD);
                    //_forgroundDetector = new BGStatModel<Bgr>(image, Emgu.CV.CvEnum.BG_STAT_TYPE.FGD_STAT_MODEL);
                }

                _forgroundDetector.Update(image);

                
                pictureBox1.Image = image.ToBitmap();

                //update the motion history
                _motionHistory.Update(_forgroundDetector.ForegroundMask);

                //pictureBox2.SetBounds(355, 267, _forgroundDetector.ForegroundMask.Height, _forgroundDetector.ForegroundMask.Height);
                pictureBox2.Image = _forgroundDetector.ForegroundMask.ToBitmap();

                #region get a copy of the motion mask and enhance its color
                double[] minValues, maxValues;
                Point[] minLoc, maxLoc;
                _motionHistory.Mask.MinMax(out minValues, out maxValues, out minLoc, out maxLoc);
                Image<Gray, Byte> motionMask = _motionHistory.Mask.Mul(255.0 / maxValues[0]);
                #endregion

                //create the motion image 
                Image<Bgr, Byte> motionImage = new Image<Bgr, byte>(motionMask.Size);
                //display the motion pixels in blue (first channel)
                motionImage[0] = motionMask;

                //Threshold to define a motion area, reduce the value to detect smaller motion
                double minArea = 100;

                storage.Clear(); //clear the storage
                Seq<MCvConnectedComp> motionComponents = _motionHistory.GetMotionComponents(storage);

                //iterate through each of the motion component
                foreach (MCvConnectedComp comp in motionComponents)
                {
                    //reject the components that have small area;
                    if (comp.area < minArea) continue;

                    // find the angle and motion pixel count of the specific area
                    double angle, motionPixelCount;
                    _motionHistory.MotionInfo(comp.rect, out angle, out motionPixelCount);

                    //reject the area that contains too few motion
                    if (motionPixelCount < comp.area * 0.05) continue;

                    //Draw each individual motion in red
                    DrawMotion(motionImage, comp.rect, angle, new Bgr(Color.Red));
                }

                // find and draw the overall motion angle
                double overallAngle, overallMotionPixelCount;
                _motionHistory.MotionInfo(motionMask.ROI, out overallAngle, out overallMotionPixelCount);
                DrawMotion(motionImage, motionMask.ROI, overallAngle, new Bgr(Color.Green));

                //Display the amount of motions found on the current image
                UpdateText(String.Format("Total Motions found: {0}; Motion Pixel count: {1}", motionComponents.Total, overallMotionPixelCount));

                //Display the image of the motion
               
                pictureBox3.Image = motionImage.ToBitmap();
            }
        }
        }

        private void UpdateText(String text)
        {
            if (InvokeRequired && !IsDisposed)
            {
                Invoke((Action<String>)UpdateText, text);
            }
            else
            {
                label3.Text = text;
            }
        }

        private static void DrawMotion(Image<Bgr, Byte> image, Rectangle motionRegion, double angle, Bgr color)
        {
            float circleRadius = (motionRegion.Width + motionRegion.Height) >> 2;
            Point center = new Point(motionRegion.X + motionRegion.Width >> 1, motionRegion.Y + motionRegion.Height >> 1);

            CircleF circle = new CircleF(
               center,
               circleRadius);

            int xDirection = (int)(Math.Cos(angle * (Math.PI / 180.0)) * circleRadius);
            int yDirection = (int)(Math.Sin(angle * (Math.PI / 180.0)) * circleRadius);
            Point pointOnCircle = new Point(
                center.X + xDirection,
                center.Y - yDirection);
            LineSegment2D line = new LineSegment2D(center, pointOnCircle);

            image.Draw(circle, color, 1);
            image.Draw(line, color, 2);
        }


        private void motion2_Click(object sender, EventArgs e)
        {
            
            My_Timer.Interval = 1000 / FPS;
            //   My_Timer.Tick += new EventHandler(My_Timer_Tick);
            My_Timer.Start();
      
            pictureBox1.SetBounds(12, 267, src.Width, src.Height);
            pictureBox2.SetBounds(355, 267, src.Width, src.Height);
            pictureBox3.SetBounds(734, 267, src.Width, src.Height);
            
            //try to create the capture
            if (_capture == null)
            {
                try
                {
                    if(radioButton1.Checked)
                        _capture = new Capture();
                    else
                    _capture = new Capture("C:/Users/shaghayegh/Documents/Visual Studio 2010/Projects/motionemgu/EN.avi");
                    
                }
                catch (NullReferenceException excpt)
                {   //show errors if there is any
                    MessageBox.Show(excpt.Message);
                }
            }

            if (_capture != null) //if camera capture has been successfully created
            {
                _motionHistory = new MotionHistory(
                    1.0, //in second, the duration of motion history you wants to keep
                    0.05, //in second, maxDelta for cvCalcMotionGradient
                    0.5); //in second, minDelta for cvCalcMotionGradient

                _capture.ImageGrabbed += ProcessFrame;
                _capture.Start();
            }
          
        }

        private void Close_Click(object sender, EventArgs e)
        {
            _capture.Stop();
        }

        private void tajamoi_Click(object sender, EventArgs e)
        {
            Emgu.CV.CvEnum.INTER interType = Emgu.CV.CvEnum.INTER.CV_INTER_NN;
            interType = Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR;
            pictureBox1.SetBounds(93, 288, 400, 400);
            pictureBox2.SetBounds(660, 288, 400, 400);
            pictureBox3.SetBounds(800, 288, 1, 1);
            Emgu.CV.Capture capture; 
            if (radioButton5.Checked)
            {
                capture= new Capture("C:/Users/shaghayegh/Documents/Visual Studio 2010/Projects/motionemgu/EN.avi");
            }
                else
            {
                 capture = new Capture(0);
             }
            Timer timer = new Timer();
            capture.QueryFrame();
            Image<Gray, Byte> gray1;
            Image<Gray, Byte> gray2;
            Image<Gray, Byte> gray3;
            Image<Gray, Byte> gray4;
            Image<Gray, Byte> Diff1;
            Image<Gray, Byte> Diff2;
            Image<Gray, Byte> Diff3;
            Image<Gray, Byte> Sigma;

            Action a = new Action(() =>
            {
                while (true)
                {
                    // CvInvoke.cvQueryFrame(capture.Ptr);
                    gray1 = capture.QueryGrayFrame().SmoothGaussian(7);
                    gray2 = capture.QueryGrayFrame().SmoothGaussian(7);
                    gray3 = capture.QueryGrayFrame().SmoothGaussian(7);
                    gray4 = capture.QueryGrayFrame().SmoothGaussian(7);


                    Diff1 = gray4.AbsDiff(gray3.Resize(gray4.Width,gray4.Height, interType));
                    Diff2 = gray4.AbsDiff(gray2.Resize(gray4.Width,gray4.Height, interType));
                    Diff3 = gray4.AbsDiff(gray1.Resize(gray4.Width,gray4.Height, interType));
                    Sigma = Diff1.Mul(0.6).Add(Diff2.Mul(0.3));
                    Sigma = Sigma.Add(Diff3.Mul(0.1));
                    Sigma = Sigma.ThresholdBinary(new Gray(20), new Gray(255));

                    pictureBox1.Image = Sigma.ToBitmap();
                    pictureBox2.Image = gray1.ToBitmap();
                    CvInvoke.cvWaitKey(33);


                }
            });
            a.BeginInvoke(a.EndInvoke, null);
        }

        private void tafazoli_Click(object sender, EventArgs e)
        {
            Emgu.CV.CvEnum.INTER interType = Emgu.CV.CvEnum.INTER.CV_INTER_NN;
            interType = Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR;
            pictureBox1.SetBounds(93, 288, 400, 400);
            pictureBox2.SetBounds(660, 288, 400, 400);
            pictureBox3.SetBounds(800, 288, 1, 1);
            Emgu.CV.Capture capture ;
            if (radioButton3.Checked)
            {
                capture = new Capture("C:/Users/shaghayegh/Documents/Visual Studio 2010/Projects/motionemgu/EN.avi");
            }
            else
            {
                capture = new Capture(0);
            }
            Timer timer = new Timer();
            capture.QueryFrame();
            Image<Gray, Byte> gray;
            Image<Gray, Byte> gray2;
            Image<Gray, Byte> gray3;

            Action a = new Action(() =>
            {
                while (true)
                {
                    // CvInvoke.cvQueryFrame(capture.Ptr);
                    gray = capture.QueryGrayFrame().SmoothGaussian(7);
                    gray2 = capture.QueryGrayFrame().SmoothGaussian(7);

                    gray3 = gray2.AbsDiff(gray.Resize(gray2.Width,
                        gray2.Height, interType));
                    gray3 = gray3.AbsDiff(gray3.ThresholdBinary(new Gray(100), new Gray(255)));
                    pictureBox1.Image = gray3.ToBitmap();
                    pictureBox2.Image = gray.ToBitmap();
                    CvInvoke.cvWaitKey(33);


                }
            });
            a.BeginInvoke(a.EndInvoke, null);
        }

   

        void thining(Image<Gray, byte> image1)
             {
             Gray WHITE =new Gray (0);
             Gray BLACK = new Gray(255);
             Gray GRAY = new Gray(128);
             int BLACK2 =255;
             int GRAY2 =128;

       


            int[,] offset = new int[9, 2] {{0,0},{1,0},{1,-1},{0,-1},{-1,-1},
		      {-1,0},{-1,1},{0,1},{1,1} }; /* offsets for neighbors */
            int[] n_odd = new int[4] { 1, 3, 5, 7 };    
            int px, py;                         
            int[] b = new int[9];                  
            int[] condition = new int[6];        
            int counter;                        
            int i, x, y, copy, sum;             
            int x_size1 = image1.Width;
            int y_size1 = image1.Height;
            int x_size2 = x_size1;
            int y_size2 = y_size1;
            Image<Gray, byte> image2 = new Image<Gray, byte>(image1.Size);
            for (y = 0; y < y_size2; y++)
            {
                for (x = 0; x < x_size2; x++)
                {
                    image2[y, x] = image1[y, x];
                }
            }
            /* processing starts */
            do
            {
                counter = 0;
                for (y = 0; y < y_size1; y++)
                {
                    for (x = 0; x < x_size1; x++)
                    {
                        /* substitution of 9-neighbor gray values */
                        for (i = 0; i < 9; i++)
                        {
                            b[i] = 0;
                            px = x + offset[i, 0];
                            py = y + offset[i, 1];
                            if (px >= 0 && px < x_size1 &&
                                py >= 0 && py < y_size1)
                            {
                                if (image2[py, px].Intensity == BLACK2)
                                {
                                    b[i] = 1;
                                }
                                else if (image2[py, px].Intensity == GRAY2)
                                {
                                    b[i] = -1;
                                }
                            }
                        }
                        for (i = 0; i < 6; i++)
                        {
                            condition[i] = 0;
                        }

                        /* condition 1: figure point */
                        if (b[0] == 1) condition[0] = 1;
                        sum = 0;
                        for (i = 0; i < 4; i++)
                        {
                            sum = sum + 1 - Math.Abs(b[n_odd[i]]);
                        }
                        if (sum >= 1) condition[1] = 1;
                        sum = 0;
                        for (i = 1; i <= 8; i++)
                        {
                            sum = sum + Math.Abs(b[i]);
                        }
                        if (sum >= 2) condition[2] = 1;

                        sum = 0;
                        for (i = 1; i <= 8; i++)
                        {
                            if (b[i] == 1) sum++;
                        }
                        if (sum >= 1) condition[3] = 1;
                        if (func_nc8(b) == 1) condition[4] = 1;
                        sum = 0;
                        for (i = 1; i <= 8; i++)
                        {
                            if (b[i] != -1)
                            {
                                sum++;
                            }
                            else
                            {
                                copy = b[i];
                                b[i] = 0;
                                if (func_nc8(b) == 1) sum++;
                                b[i] = copy;
                            }
                        }
                        if (sum == 8) condition[5] = 1;

                        /* final decision */
                        if (condition[0] == 1 & condition[1] == 1 & condition[2] == 1 & condition[3] == 1 & condition[4] == 1 & condition[5] == 1)
                        {
                            image2[y, x] = GRAY; /* equals -1 */
                            counter++;
                        }
                    } 
                } 

                if (counter != 0)
                {
                    for (y = 0; y < y_size1; y++)
                    {
                        for (x = 0; x < x_size1; x++)
                        {
                            if (image2[y, x].Intensity == GRAY2)
                                image2[y, x] = WHITE;
                        }
                    }
                }
            } while (counter != 0);
            pictureBox1.SetBounds(93, 288, image1.Width, image1.Height);
            pictureBox2.SetBounds(550, 288, image2.Width, image2.Height);
            pictureBox3.SetBounds(700, 288, 1, 1);
            pictureBox1.Image = image1.ToBitmap();
            pictureBox2.Image = image2.ToBitmap();
        }//end function
      int func_nc8(int[] b)
            {
            int[] n_odd = new int[4] { 1, 3, 5, 7 }; 
            int i, j, sum;         
            int[] d = new int[10];
            for (i = 0; i <= 9; i++)
            {
                j = i;
                if (i == 9) j = 1;
                // if (abs(*(b + j)) == 1) {
                if (Math.Abs(b[j]) == 1)
                {
                    d[i] = 1;
                }
                else
                {
                    d[i] = 0;
                }
            }
            sum = 0;
            for (i = 0; i < 4; i++)
            {
                j = n_odd[i];
                sum = sum + d[j] - d[j] * d[j + 1] * d[j + 2];
            }
            return (sum);
        }

      private void MedilAxis_Click(object sender, EventArgs e)
      {
          Image<Gray, byte> grayimage = new Image<Gray, byte>(src.Size);
          CvInvoke.cvCvtColor(src, grayimage, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);

          thining(grayimage);        /* Hilditch's thinning operation */
      }

      private void BoundingRectangle_Click(object sender, EventArgs e)
      {
          Image<Gray, Byte> grayImage = src.Convert<Gray, Byte>().PyrDown().PyrUp();
          Image<Gray, Byte> cannyImage = new Image<Gray, Byte>(grayImage.Size);
          CvInvoke.cvCanny(grayImage, cannyImage, 10, 60, 3);


          StructuringElementEx kernel = new StructuringElementEx(3, 3, 1, 1, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_ELLIPSE);
          CvInvoke.cvDilate(cannyImage, cannyImage, kernel, 1);

          IntPtr cont = IntPtr.Zero;

          Image<Bgr, Byte> depthImage2 = src.CopyBlank();
          //Graphics graphicsBitmap = Graphics.FromImage(depthImage2);
          Bitmap GPic = src.ToBitmap();
          Graphics graphicsBitmap = Graphics.FromImage(GPic);
          Point p = new Point(0, 0);
          //pictureBox6.Image = cannyImage.ToBitmap();
          using (MemStorage storage = new MemStorage()) //allocate storage for contour approximation
              for (Contour<Point> contours = cannyImage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL); contours != null; contours = contours.HNext)
              {

                  IntPtr seq = CvInvoke.cvConvexHull2(contours, storage.Ptr, Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE, 0);
                  IntPtr defects = CvInvoke.cvConvexityDefects(contours, seq, storage);
                  Seq<Point> tr = contours.GetConvexHull(Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);

                  Seq<Emgu.CV.Structure.MCvConvexityDefect> te = contours.GetConvexityDefacts(storage, Emgu.CV.CvEnum.ORIENTATION.CV_CLOCKWISE);
                  graphicsBitmap.DrawRectangle(new Pen(new SolidBrush(Color.Red)), tr.BoundingRectangle);




              }
          pictureBox1.SetBounds(200, 288, src.Width, src.Height);
          pictureBox1.Image = GPic;
          pictureBox2.SetBounds(700, 288, src.Width, src.Height);
          pictureBox2.Image = grayImage.ToBitmap();//baraye hame bezar
          pictureBox3.SetBounds(900, 288, 1, 1);//???checkkon hame ja in text boxaro save karde bashi
      }

      private void yaxis_Click(object sender, EventArgs e)
      {
          Image<Gray, byte> Src_Image = new Image<Gray, byte>(src.Size); 
          CvInvoke.cvCvtColor(src, Src_Image, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
          Image<Gray, Byte> grayImage = Src_Image.Convert<Gray, Byte>().PyrDown().PyrUp();
          Image<Gray, Byte> cannyImage = new Image<Gray, Byte>(grayImage.Size);
          CvInvoke.cvCanny(grayImage, cannyImage, 10, 60, 3);
          wr = File.CreateText(textBox2.Text + ".txt");

          StructuringElementEx kernel = new StructuringElementEx(3, 3, 1, 1, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_ELLIPSE);
          CvInvoke.cvDilate(cannyImage, cannyImage, kernel, 1);

          IntPtr cont = IntPtr.Zero;

          Point p = new Point(0, 0);
          pictureBox1.SetBounds(200, 288, cannyImage.Width, cannyImage.Height);
          pictureBox1.Image = cannyImage.ToBitmap();
          pictureBox3.SetBounds(900, 288, 1, 1);//???checkkon hame ja in text boxaro save karde bashi
          using (MemStorage storage = new MemStorage()) //allocate storage for contour approximation
              for (Contour<Point> contours = cannyImage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL); contours != null; contours = contours.HNext)
              {
                  MCvScalar color1 = new MCvScalar(0, 0, 0);
                  MCvScalar color2 = new MCvScalar(255, 255, 255);
                  CvInvoke.cvDrawContours(cannyImage.Ptr,
                      contours,
                      color2, color1, 0, -1, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, p);
                  pictureBox2.SetBounds(700, 288, cannyImage.Width, cannyImage.Height);
                  pictureBox2.Image = cannyImage.ToBitmap();//baraye hame bezar
              }
          bool flag = true;
          double f;
          
          for (int i = 0; i < cannyImage.Rows; ++i)
              for (int j = 0; j < cannyImage.Cols - 1; ++j)
                  if (cannyImage[i, j].Intensity == 0)
                  {
                      String MyString = "";
                      MyString +=" x=" + i.ToString() + ":";
                      while ((cannyImage[i, j].Intensity == 0) && (j < cannyImage.Cols - 1))
                      {
                          MyString += j.ToString() + "-";
                          

                          double PV = cannyImage[i, j].Intensity;
                          int PVI = Convert.ToInt32(PV);
                          ++j;

                      }
                    
                     
                      wr.WriteLine(MyString);
                  }
          
         
          wr.Close();
         
      }

      private void Xaxis_Click(object sender, EventArgs e)
      {
          Image<Gray, byte> Src_Image = new Image<Gray, byte>(src.Size); 
          CvInvoke.cvCvtColor(src, Src_Image, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
          Image<Gray, Byte> grayImage = Src_Image.Convert<Gray, Byte>().PyrDown().PyrUp();
          Image<Gray, Byte> cannyImage = new Image<Gray, Byte>(grayImage.Size);
          CvInvoke.cvCanny(grayImage, cannyImage, 10, 60, 3);
          wr = File.CreateText(textBox2.Text+ ".txt");

          StructuringElementEx kernel = new StructuringElementEx(3, 3, 1, 1, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_ELLIPSE);
          CvInvoke.cvDilate(cannyImage, cannyImage, kernel, 1);

          IntPtr cont = IntPtr.Zero;

          Point p = new Point(0, 0);
          pictureBox1.SetBounds(200, 288, cannyImage.Width, cannyImage.Height);
          pictureBox1.Image = cannyImage.ToBitmap();
          pictureBox3.SetBounds(900, 288, 1, 1);//???checkkon hame ja in text boxaro save karde bashi
          using (MemStorage storage = new MemStorage()) //allocate storage for contour approximation
              for (Contour<Point> contours = cannyImage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL); contours != null; contours = contours.HNext)
              {
                  MCvScalar color1 = new MCvScalar(0, 0, 0);
                  MCvScalar color2 = new MCvScalar(255, 255, 255);
                  CvInvoke.cvDrawContours(cannyImage.Ptr,
                      contours,
                      color2, color1, 0, -1, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, p);
                  pictureBox2.SetBounds(700, 288, cannyImage.Width, cannyImage.Height);
                  pictureBox2.Image = cannyImage.ToBitmap();//baraye hame bezar
              }
          for (int j = 0; j < cannyImage.Cols - 1; ++j)
            for (int i = 0; i < cannyImage.Rows; ++i)  
                  if (cannyImage[i, j].Intensity == 0)
                  {
                      String MyString = "";
                      MyString += " y=" + j.ToString() + ":";
                      
                      while ( (i < cannyImage.Height - 2)&(cannyImage[i, j].Intensity == 0) )
                      {
                          MyString +=  i.ToString() + "-";
                          

                          double PV = cannyImage[i, j].Intensity;
                          int PVI = Convert.ToInt32(PV);
                          ++i;
                      }
                   
                      wr.WriteLine(MyString);
                  }


          wr.Close();
      }

      private void SpatialOccupancy_Click(object sender, EventArgs e)
      {
          Image<Gray, byte> Src_Image = new Image<Gray, byte>(src.Size); 
          CvInvoke.cvCvtColor(src, Src_Image, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
          Image<Gray, Byte> grayImage = Src_Image.Convert<Gray, Byte>().PyrDown().PyrUp();
          Image<Gray, Byte> cannyImage = new Image<Gray, Byte>(grayImage.Size);
          CvInvoke.cvCanny(grayImage, cannyImage, 10, 60, 3);
          wr = File.CreateText(textBox2.Text+ ".txt");

          StructuringElementEx kernel = new StructuringElementEx(3, 3, 1, 1, Emgu.CV.CvEnum.CV_ELEMENT_SHAPE.CV_SHAPE_ELLIPSE);
          CvInvoke.cvDilate(cannyImage, cannyImage, kernel, 1);

          IntPtr cont = IntPtr.Zero;

          Point p = new Point(0, 0);
          pictureBox1.SetBounds(200, 288, cannyImage.Width, cannyImage.Height);
          pictureBox1.Image = cannyImage.ToBitmap();
          pictureBox3.SetBounds(900, 288, 1, 1);//???checkkon hame ja in text boxaro save karde bashi
          using (MemStorage storage = new MemStorage()) //allocate storage for contour approximation
              for (Contour<Point> contours = cannyImage.FindContours(Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_EXTERNAL); contours != null; contours = contours.HNext)
              {
                  MCvScalar color1 = new MCvScalar(0, 0, 0);
                  MCvScalar color2 = new MCvScalar(255, 255, 255);
                  CvInvoke.cvDrawContours(cannyImage.Ptr,
                      contours,
                      color2, color1, 0, -1, Emgu.CV.CvEnum.LINE_TYPE.EIGHT_CONNECTED, p);
                  pictureBox2.SetBounds(700, 288, cannyImage.Width, cannyImage.Height);
                  pictureBox2.Image = cannyImage.ToBitmap();//baraye hame bezar
              }
          for (int j = 0; j < cannyImage.Cols - 1; ++j)
            for (int i = 0; i < cannyImage.Rows; ++i)  
                  if (cannyImage[i, j].Intensity == 0)
                  {
                      String MyString = "";
                      MyString += " (" + j.ToString() + ",";
                      MyString+=i.ToString() +")";
                      wr.WriteLine(MyString);
                  }


          wr.Close();
      }

      private void label11_Click(object sender, EventArgs e)
      {

      }

      private void hoght_Click(object sender, EventArgs e)
      {

          pictureBox1.SetBounds(93,288 , src.Width, src.Height);
          pictureBox2.SetBounds(400,288,src.Width,src.Height);
          Image<Bgr, Byte> img =src.Copy();
          Image<Gray, byte> temp_img;

          temp_img = img.Convert<Gray, Byte>();
          Gray cannyThreshold = new Gray(22);
          Gray cannyThresholdLinking = new Gray(17);
          Gray circleAccumulatorThreshold = new Gray(5);

          CircleF[] circles = temp_img.HoughCircles(
              cannyThreshold,
              circleAccumulatorThreshold,
              ((double)trackBar1.Value) / 100, 300, 30, 60)[0];
          Text = circles.Length.ToString();

          //  Image<Gray, Byte> cannyEdges = temp_img.Canny(cannyThreshold, cannyThresholdLinking);
          Image<Gray, Byte> cannyEdges = temp_img.Canny(10, 20);//***************************************
          LineSegment2D[] lines = cannyEdges.HoughLinesBinary(
              1, //Distance resolution in pixel-related units
              Math.PI / 45.0, //Angle resolution measured in radians.
              20, //threshold
              30, //min Line width
              10 //gap between lines
              )[0]; //Get the lines from the first channel

          #region Find triangles and rectangles
          List<Triangle2DF> triangleList = new List<Triangle2DF>();
          List<MCvBox2D> boxList = new List<MCvBox2D>();

          using (MemStorage storage = new MemStorage())
              for (Contour<Point> contours = cannyEdges.FindContours(); contours != null; contours = contours.HNext)
              {
                  Contour<Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.05, storage);

                  if (contours.Area > 250)
                  {
                      if (currentContour.Total == 3)
                      {
                          Point[] pts = currentContour.ToArray();
                          triangleList.Add(new Triangle2DF(
                             pts[0],
                             pts[1],
                             pts[2]
                             ));
                      }
                      else if (currentContour.Total == 4) //The contour has 4 vertices.
                      {
                          #region determine if all the angles in the contour are within the range of [80, 100] degree
                          bool isRectangle = true;
                          Point[] pts = currentContour.ToArray();
                          LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

                          for (int i = 0; i < edges.Length; i++)
                          {
                              double angle = Math.Abs(
                                 edges[(i + 1) % edges.Length].GetExteriorAngleDegree(edges[i]));
                              if (angle < 80 || angle > 100)
                              {
                                  isRectangle = false;
                                  break;
                              }
                          }
                          #endregion

                          if (isRectangle) boxList.Add(currentContour.GetMinAreaRect());
                      }
                  }
              }
          #endregion

          pictureBox1.Image = img.ToBitmap();

          if (radioButtontriangel.Checked)
          {
                  Image<Bgr, Byte> triangleRectangleImage = img.CopyBlank();
                  if (checkBox1.Checked)
                  {
                      foreach (Triangle2DF triangle in triangleList)
                          img.Draw(triangle, new Bgr(Color.DarkBlue), 2);
                      foreach (MCvBox2D box in boxList)
                          img.Draw(box, new Bgr(Color.DarkOrange), 2);
                      pictureBox1.Image = img.ToBitmap();
                  }
                  else
                  {
                      foreach (Triangle2DF triangle in triangleList)
                          triangleRectangleImage.Draw(triangle, new Bgr(Color.DarkBlue), 2);
                      foreach (MCvBox2D box in boxList)
                          triangleRectangleImage.Draw(box, new Bgr(Color.DarkOrange), 2);
                      pictureBox1.Image = triangleRectangleImage.ToBitmap();
                  }
                  
          }else if (circleradio3.Checked)
          {
                  Image<Bgr, Byte> circleImage = img.CopyBlank();
                  int tedad = 0;//tedade sefid ha
                  if (checkBox1.Checked)
                  {
                      foreach (CircleF circle in circles)
                          if (circle.Center.X < temp_img.Size.Width / 5 || circle.Center.X > temp_img.Size.Width / 1.2 || circle.Center.Y < temp_img.Size.Height / 5 || circle.Center.Y > temp_img.Size.Height / 1.2)
                              ;//nothing
                          else
                          {

                              for (int k1 = (int)(circle.Center.X - circle.Radius); k1 <= circle.Center.X + circle.Radius; k1++)
                              {
                                  for (int k2 = (int)(circle.Center.Y - circle.Radius); k2 <= circle.Center.Y + circle.Radius; k2++)
                                  {
                                      if (temp_img[k1, k2].Intensity > 55)
                                          tedad++;
                                  }
                              }
                              MessageBox.Show(tedad.ToString());
                              if (tedad < (0.6 * (4 * circle.Radius * circle.Radius)))
                                  ;
                              else
                                  img.Draw(circle, new Bgr(Color.Brown), 10);
                          }
                      pictureBox1.Image = img.ToBitmap();
                  }
                  else
                  {
                      foreach (CircleF circle in circles)
                          circleImage.Draw(circle, new Bgr(Color.Brown), 2);
                      pictureBox1.Image = circleImage.ToBitmap();
                  }
                  
          }
          else
          {
                  Image<Bgr, Byte> lineImage = img.CopyBlank();
                  if (checkBox1.Checked)
                  {
                      foreach (LineSegment2D line in lines)
                          img.Draw(line, new Bgr(Color.Green), 2);
                      pictureBox1.Image = img.ToBitmap();
                  }
                  else
                  {
                      foreach (LineSegment2D line in lines)
                          lineImage.Draw(line, new Bgr(Color.Green), 2);
                      pictureBox1.Image = lineImage.ToBitmap();
                  }
                
          }}



      Capture _capture1; //capture device

      Image<Bgr, Byte> Framet; //current Frame from camera
      Image<Bgr, Byte> Previous_Framet; //Previiousframe aquired
      Image<Bgr, Byte> Differencet; //Difference between the two frames

      double ContourThresh = 0.003; //stores alpha for thread access
      int Threshold = 60; //stores threshold for thread access
        private void button3_Click(object sender, EventArgs e)
        {
            pictureBox1.SetBounds(93, 288, 300, 300);
            pictureBox2.SetBounds(400, 288, 300, 300);
            pictureBox3.SetBounds(710, 288, 1, 1);
            
            _capture1 = new Capture();  //set up cature
            _capture1.ImageGrabbed += ProcessFramet; //set up capture event handler
            _capture1.Start(); //start aquasitio
        }
        private void ProcessFramet(object sender, EventArgs arg)
        {
          
            if (Framet == null) //we need at least one fram to work out running average so acquire one before doing anything
            {
                //display the frame aquired
                Framet = _capture1.RetrieveBgrFrame(); //we could use RetrieveGrayFrame if we didn't care about displaying colour image
                pictureBox1.Image = Framet.ToBitmap();
                Previous_Framet = Framet.Copy(); //copy the frame to act as the previous
            }
            else
            {
                //acquire the frame
                Framet = _capture1.RetrieveBgrFrame(); //aquire a frame

                Differencet = Previous_Framet.AbsDiff(Framet); //find the absolute difference 
                /*Play with the value 60 to set a threshold for movement*/
                Differencet = Differencet.ThresholdBinary(new Bgr(Threshold, Threshold, Threshold), new Bgr(255, 255, 255)); //if value > 60 set to 255, 0 otherwise 
                pictureBox3.Image = Differencet.ToBitmap();
                 //display the absolute difference 

                Previous_Framet = Framet.Copy(); //copy the frame to act as the previous frame

                #region Draw the contours of difference
                //this is tasken from the ShapeDetection Example
                using (MemStorage storage = new MemStorage()) //allocate storage for contour approximation
                    //detect the contours and loop through each of them
                    for (Contour<Point> contours = Differencet.Convert<Gray, Byte>().FindContours(
                          Emgu.CV.CvEnum.CHAIN_APPROX_METHOD.CV_CHAIN_APPROX_SIMPLE,
                          Emgu.CV.CvEnum.RETR_TYPE.CV_RETR_LIST,
                          storage);
                       contours != null;
                       contours = contours.HNext)
                    {
                        //Create a contour for the current variable for us to work with
                        Contour<Point> currentContour = contours.ApproxPoly(contours.Perimeter * 0.05, storage);

                        //Draw the detected contour on the image
                        if (currentContour.Area > ContourThresh) //only consider contours with area greater than 100 as default then take from form control
                        {
                            Framet.Draw(currentContour.BoundingRectangle, new Bgr(Color.Red), 2);
                        }
                    }
                #endregion
                pictureBox1.Image = Framet.ToBitmap();
                pictureBox2.Image = Previous_Framet.ToBitmap();
               


            }
        }

        private void canny_Click_1(object sender, EventArgs e)
        {
            Image<Gray, Byte> cannyEdges = new Image<Gray, byte>(src.Size);
            Image<Gray, Byte> Grayimg = new Image<Gray, Byte>(src.Size);
            CvInvoke.cvCvtColor(src, Grayimg, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
            //  //حداقل آستانه//حداکثر آستانه//ابعادکرنل
            int Th = Convert.ToInt16(THC.Text);
            int THL = Convert.ToInt16(THLC.Text);
            CvInvoke.cvCanny(Grayimg, cannyEdges, Th, THL, 3);//**&&&astane
            pictureBox1.Image = cannyEdges.ToBitmap();
            pictureBox1.SetBounds(93, 288, cannyEdges.Width, cannyEdges.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(500, 288, src.Width, src.Height);
            pictureBox3.SetBounds(700, 288, 1, 1);
        }

        private void marr_Click_1(object sender, EventArgs e)
        {
            Image<Gray, float> cannyEdges = new Image<Gray, float>(src.Size);
            Image<Gray, Byte> Grayimg = new Image<Gray, Byte>(src.Size);
            CvInvoke.cvCvtColor(src, Grayimg, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
            int cernelsize = Convert.ToInt16(cernelSizemarr.Text);
            double sigma12 = Convert.ToDouble(sigma1.Text);
            double sigma21 = Convert.ToDouble(sigma2.Text);
            cannyEdges = Grayimg.Laplace(cernelsize);//اندازه ی کرنل&&&
            cannyEdges = cannyEdges.SmoothGaussian(cernelsize, cernelsize, sigma12, sigma21);//&&&اولی طول کرنل،2-عرض کرنل،3سیگما1،4-سیگما2
            pictureBox1.Image = cannyEdges.ToBitmap();
            pictureBox1.SetBounds(93, 288, cannyEdges.Width, cannyEdges.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(500, 288, src.Width, src.Height);
            pictureBox3.SetBounds(700, 288, 1, 1);
        }

        private void navatia_Click_1(object sender, EventArgs e)
        {
            int thr = Convert.ToInt32(navatiath.Text);
            Image<Bgr, Byte> res = NevatiaBabuFilter(src, thr);
            pictureBox1.Image = res.ToBitmap();
            pictureBox1.SetBounds(93, 288, res.Width, res.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(500, 288, src.Width, src.Height);
            pictureBox3.SetBounds(700, 288, 1, 1);
        }

        private void sobel_Click_1(object sender, EventArgs e)
        {
            Image<Gray, float> dst;
            Image<Gray, Byte> Grayimg = new Image<Gray, Byte>(src.Size);
            CvInvoke.cvCvtColor(src, Grayimg, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);
            int xorder1 = Convert.ToInt16(xorder.Text);
            int yorder1 = Convert.ToInt16(yorder.Text);
            int apreture1 = Convert.ToInt16(aperture.Text);
            dst = Grayimg.Sobel(xorder1, yorder1, apreture1);//&&&&avali va dovomi jahate moshtagh ro neshun midan ke ya x bashe ya y osilan 0,1 ya 1,0 dar nazar gerefte mishan sevomi ham ke andazeye kernele
            pictureBox1.Image = dst.ToBitmap();
            pictureBox1.SetBounds(93, 288, dst.Width, dst.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(500, 288, src.Width, src.Height);
            pictureBox3.SetBounds(700, 288, 1, 1);
        }

        private void Adaptive_Click_1(object sender, EventArgs e)
        {
            int blocksize = 3;
            blocksize = Convert.ToInt16(textBox1.Text);
            Image<Gray, Byte> img1 = new Image<Gray, Byte>(src.Size);
            CvInvoke.cvCvtColor(src, img1, Emgu.CV.CvEnum.COLOR_CONVERSION.CV_BGR2GRAY);

            img1 = img1.ThresholdAdaptive(new Gray(255), Emgu.CV.CvEnum.ADAPTIVE_THRESHOLD_TYPE.CV_ADAPTIVE_THRESH_GAUSSIAN_C, Emgu.CV.CvEnum.THRESH.CV_THRESH_BINARY, blocksize, new Gray(10));
            pictureBox1.Image = img1.ToBitmap();
            pictureBox1.SetBounds(93, 288, img1.Width, img1.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(500, 288, src.Width, src.Height);
            pictureBox3.SetBounds(700, 288, 1, 1);
        }

        private void Flip_Click(object sender, EventArgs e)
        {
            double angle;
            angle = Convert.ToDouble(numericUpDownRotate.Value);
            Bgr color = new Bgr(255, 255, 255);
            Image<Bgr, Byte> dstrotate=new Image<Bgr,Byte>(src.Size);
           
            if (Horizental.Checked)
            {
                 dstrotate = src.Flip(Emgu.CV.CvEnum.FLIP.HORIZONTAL);
            }
            else if (Vertical.Checked)
            {
                 dstrotate = src.Flip(Emgu.CV.CvEnum.FLIP.VERTICAL);
            }
            else
            {
                dstrotate = src.Flip(Emgu.CV.CvEnum.FLIP.NONE);
            }
            pictureBox1.Image = dstrotate.ToBitmap();
            pictureBox1.SetBounds(93, 288, dstrotate.Width, dstrotate.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(500, 288, src.Width, src.Height);
            pictureBox3.SetBounds(700, 288, 1, 1);
        }

        private void gray_Click(object sender, EventArgs e)
        {
            Image<Gray, Byte> gray;
            if (checkBox1.Checked)
            {
                gray = Graysrc.Convert<Gray, Byte>();
            }
            else
            {
                gray = src.Convert<Gray, Byte>();
            }

            for (int i = 0; i < gray.Rows; ++i)
                for (int j = 0; j < gray.Cols; ++j)
                {
                    {

                        gray[i, j] = new Gray(255 - gray[i, j].Intensity);

                    }
                }
          
            pictureBox1.Image = gray.ToBitmap();
            pictureBox1.SetBounds(93, 288, gray.Width, gray.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(500, 288, gray.Width, gray.Height);
            pictureBox3.SetBounds(700, 288, 1, 1);
        }

        private void log_Click(object sender, EventArgs e)
        {
            double c = Convert.ToDouble(Clog.Text);
            Image<Gray, Byte> gray;
            if (checkBox1.Checked)
            {
                gray = Graysrc.Convert<Gray, Byte>();
            }
            else
            {
                gray = src.Convert<Gray, Byte>();
            }
            for (int i = 0; i < gray.Rows; ++i)
                for (int j = 0; j < gray.Cols; ++j)
                {
                    {

                        gray[i, j] = new Gray(c * Math.Log(1 + gray[i, j].Intensity, 2));//pas chera in jaro dorost kar mikone?
                        //aha ma in ja miai dar akhar dar c zarbesh mikonim ke khodesh be sefid nazdikesh mikone yani dar har do ravesh ba afzayesh c zasefid afzayesh peida mikonam
                    }
                }
            pictureBox1.Image = gray.ToBitmap();
            pictureBox1.SetBounds(93, 288, gray.Width, gray.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(500, 288, gray.Width, gray.Height);
            pictureBox3.SetBounds(700, 288, 1, 1);
        }

        private void powergray_Click(object sender, EventArgs e)
        {
            double gama = 1.1;
            double c = Convert.ToDouble(Cpower.Text);
            Image<Gray, Byte> gray;
            if (checkBox1.Checked)
            {
                gray = Graysrc.Convert<Gray, Byte>();
            }
            else
            {
                gray = src.Convert<Gray, Byte>();
            }
            for (int i = 0; i < gray.Rows; ++i)
                for (int j = 0; j < gray.Cols; ++j)
                {
                    {

                        gray[i, j] = new Gray(c * Math.Pow(gray[i, j].Intensity, gama));//dar inja ba afzayeshe gama tasvir sefid mishe chon sathe 255 sefid ast

                    }
                }
            pictureBox1.Image = gray.ToBitmap();
            pictureBox1.SetBounds(93, 288, gray.Width, gray.Height);
            pictureBox2.Image = src.ToBitmap();
            pictureBox2.SetBounds(500, 288, gray.Width, gray.Height);
            pictureBox3.SetBounds(700, 288, 1, 1);
        }

    }  
  }


