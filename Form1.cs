﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using AwokeKnowing.GnuplotCSharp;
using System.Threading;

namespace dewiacja
{
    public partial class Form1 : Form
    {
        public enum plot_2D : int
        {
            Uniform,
            Geomtric,
            Normal,
            Exponential
        }

        private plot_2D choice_2D_plot = plot_2D.Uniform;

        uint number_of_sample = 100;
        long move = 0;
        uint size = 3;
        uint segments = 5;
        uint tau_max = 100;
        uint N = 3;

        double[] uniform_table_of_Sample;

        double[] geometric_table_of_Sample;

        double[] normal_table_of_Sample;

        double[] exponential_table_of_Sample;

        public Form1()
        {
            // uniform_distribution(-10, 10, 100);     // wywołanie generatora o rozkładzie równomiernym i zapis do [table_of_sample]
            // geometric_distribution(0.1, 100);       // wywołanie generatora o rozkładzie geometrycznym i zapis do [table_of_sample]
            // normal_distribution(100, 10, 100);      // wywołanie generatora o rozkładzie normalnym i zais do [table_of_sample]
            // exponential_distribution(0.1, 100);     // wywołanie generatora o rozkładzie wykładnicznym i zapis do [table_of_sample]

            // print_test(uniform_distribution(-10, 10, 100));

            InitializeComponent();
            generate_table_sample(number_of_sample);
        }
/*
        // drukownie wartosci probek zapisanych w tablicy
        public void print_test(double[] tableOfSample)
        {
            foreach (var t in tableOfSample)
            {
                Console.WriteLine(t.ToString(CultureInfo.InvariantCulture));
            }
        }
*/
        private void zedGraphControl1_Load(object sender, EventArgs e)
        {
            SetSize1();
        }

        private void SetSize1()
        {
            zedGraphControl1.Location = new Point(0, 0);
            zedGraphControl1.IsShowPointValues = true;
            zedGraphControl1.Size = new Size(this.ClientRectangle.Width - 20, this.ClientRectangle.Height - 50);
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            SetSize1();
        }

        private void zedGraphRefresh1()
        {
            zedGraphControl1.AxisChange();
            Refresh();
        }

        private void generate_table_sample(uint number_of_sample)
        {
            uniform_table_of_Sample = uniform_distribution(-1, 1, number_of_sample);
            geometric_table_of_Sample = geometric_distribution(0.1, number_of_sample);
            normal_table_of_Sample = normal_distribution(1, 1, number_of_sample);
            exponential_table_of_Sample = exponential_distribution(0.1, number_of_sample);
        }


        private void plotGraph2D_Uniform()
        {
            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.CurveList.Clear();

            // Set the Titles
            myPane.Title.Text = "Time Error [ns]";
            myPane.XAxis.Title.Text = "Number of Sample";
            myPane.YAxis.Title.Text = "Value [ns]";

            PointPairList uniformPairList = new PointPairList();

            for (int i = 0; i < uniform_table_of_Sample.Length; i++)
                uniformPairList.Add(i, uniform_table_of_Sample[i]);

            LineItem uniformCurve = myPane.AddCurve("Uniform Distribution",
                uniformPairList, Color.Red, SymbolType.None);
        }

        private void plotGraph2D_Geometric()
        {
            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.CurveList.Clear();

            // Set the Titles
            myPane.Title.Text = "Time Error [ns]";
            myPane.XAxis.Title.Text = "Number of Sample";
            myPane.YAxis.Title.Text = "Value [ns]";

            PointPairList geometricPairList = new PointPairList();


            for (int i = 0; i < geometric_table_of_Sample.Length; i++)
                geometricPairList.Add(i, geometric_table_of_Sample[i]);


            LineItem geometricCurve = myPane.AddCurve("Geometric Distribution",
                geometricPairList, Color.Blue, SymbolType.None);
        }

        private void plotGraph2D_Normal()
        {
            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.CurveList.Clear();

            // Set the Titles
            myPane.Title.Text = "Time Error [ns]";
            myPane.XAxis.Title.Text = "Number of Sample";
            myPane.YAxis.Title.Text = "Value [ns]";

            PointPairList normalPairList = new PointPairList();


            for (int i = 0; i < normal_table_of_Sample.Length; i++)
                normalPairList.Add(i, normal_table_of_Sample[i]);


            LineItem normalCurve = myPane.AddCurve("Normal Distribution",
                normalPairList, Color.Black, SymbolType.None);
        }

        private void plotGraph2D_Exponential()
        {
            GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.CurveList.Clear();

            // Set the Titles
            myPane.Title.Text = "Time Error [ns]";
            myPane.XAxis.Title.Text = "Number of Sample";
            myPane.YAxis.Title.Text = "Value [ns]";

            PointPairList exponentialPairList = new PointPairList();


            for (int i = 0; i < exponential_table_of_Sample.Length; i++)
                exponentialPairList.Add(i, exponential_table_of_Sample[i]);

            LineItem exponentialCurve = myPane.AddCurve("Exponential Distribution",
                exponentialPairList, Color.Green, SymbolType.None);
        }

        private void plotGraph2D_operator(uint size, long move)
        {
            GraphPane myPane = zedGraphControl1.GraphPane;

            PointPairList operator_ADEV_PairList = new PointPairList();
            PointPairList operator_TDEV_internal_PairList = new PointPairList();
            PointPairList operator_TDEV_external_PairList = new PointPairList();


            double[] operator_ADEV = {-1.5, -2, -2, -1.5, -2, -2, -1.5};
            double[] operator_TDEV_internal = {-2.5, -3, -3, -2.5, -3, -3, -2.5};
            double[] operator_TDEV_external = {-3.5, -4, -4, -3.5, -4, -4, -3.5};

            for (int i = 0; i < operator_ADEV.Length; i++)
            {
                operator_ADEV_PairList.Add(i + move, operator_ADEV[i]);
                operator_TDEV_internal_PairList.Add(i + move, operator_TDEV_internal[i]);
                operator_TDEV_external_PairList.Add(i + move, operator_TDEV_external[i]);
            }

            LineItem operatorCurve_ADEV = myPane.AddCurve("ADEV",
                operator_ADEV_PairList, Color.Gold, SymbolType.None);

            LineItem operatorCurve_TDEV_internal = myPane.AddCurve("TDEV_internal",
                operator_TDEV_internal_PairList, Color.Purple, SymbolType.None);

            LineItem operatorCurve_TDEV_exnternal = myPane.AddCurve("TDEV_exnternal",
                operator_TDEV_external_PairList, Color.Sienna, SymbolType.None);

            myPane.BarSettings.Base = BarBase.X;
        }


        // parametry generatorów
        private ulong _grain = 1179;
        private const ulong M = 2147483647;
        private const ulong A = 16807;

        //generator próbek o rozkładzie równomiernym (0, 1)
        public double make_grain()
        {
            _grain = _grain * A % M;
            return _grain / (double) M;
        }

        //generator próbek o rozkładzie równomiernym (min, max, number_of_sample)
        public double[] uniform_distribution(int min, int max, ulong numberOfSample)
        {
            var tableOfSample = new double[numberOfSample];
            for (var i = 0; i < tableOfSample.Length; i++)
            {
                tableOfSample[i] = min + (max - min) * make_grain();
            }

            return tableOfSample;
        }

        //generator próbek o rozkładzie geometrycznym (p (average [0, 1]), number_of_sample)
        public double[] geometric_distribution(double p, ulong numberOfSample)
        {
            var tableOfSample = new double[numberOfSample];
            for (var i = 0; i < tableOfSample.Length; i++)
            {
                double kernel = 1;

                while (true)
                {
                    if (make_grain() > p)
                    {
                        kernel++;
                        continue;
                    }

                    break;
                }

                tableOfSample[i] = kernel;
            }

            return tableOfSample;
        }

        //generator próbek o rozkładzie normalnym(e (average), ew (variance), number_of_sample)
        public double[] normal_distribution(double e, double ew, ulong numberOfSample)
        {
            var tableOfSample = new double[numberOfSample];
            for (var i = 0; i < tableOfSample.Length; i++)
            {
                double kernel = 1;
                double temp = 0;
                for (var j = 0; j < 12; j++)
                {
                    temp += make_grain();
                }

                temp -= 6.0;
                kernel = temp * ew + e;

                tableOfSample[i] = kernel;
            }

            return tableOfSample;
        }

        //generator próbek o rozkładzie wykładniczym(lambda [0,1])
        public double[] exponential_distribution(double lambda, ulong numberOfSample)
        {
            var tableOfSample = new double[numberOfSample];
            for (var i = 0; i < tableOfSample.Length; i++)
            {
                double kernel = 1;
                kernel = -1 / lambda * Math.Log(make_grain());

                tableOfSample[i] = kernel;
            }

            return tableOfSample;
        }


        // button generate
        private void button1_Click(object sender, EventArgs e)
        {
            generate_table_sample(number_of_sample);

            zedGraphControl1.GraphPane.CurveList.Clear();
            switch (choice_2D_plot)
            {
                case plot_2D.Uniform:
                    plotGraph2D_Uniform();
                    break;

                case plot_2D.Geomtric:
                    plotGraph2D_Geometric();
                    break;

                case plot_2D.Normal:
                    plotGraph2D_Normal();
                    break;

                case plot_2D.Exponential:
                    plotGraph2D_Exponential();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            zedGraphRefresh1();
        }

        // button Uniform Distribution
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            choice_2D_plot = plot_2D.Uniform;
        }

        // button Geometric Distribution
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            choice_2D_plot = plot_2D.Geomtric;
        }

        // button Normal Distribution
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            choice_2D_plot = plot_2D.Normal;
        }

        // button Exponential Distribution
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            choice_2D_plot = plot_2D.Exponential;
        }

        // number of sample time error 2D
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            number_of_sample = (uint) numericUpDown1.Value;
        }

        // segments
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            segments = (uint) numericUpDown2.Value;
        }

        // tau_max
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            tau_max = (uint) numericUpDown3.Value;
        }

        // N
        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            N = (uint) numericUpDown4.Value;
        }

        // one sample left "<"
        private void button2_Click(object sender, EventArgs e)
        {
            if (move > 0)
                move--;
            zedGraphControl1.GraphPane.CurveList.Clear();
            switch (choice_2D_plot)
            {
                case plot_2D.Uniform:
                    plotGraph2D_Uniform();
                    break;

                case plot_2D.Geomtric:
                    plotGraph2D_Geometric();
                    break;

                case plot_2D.Normal:
                    plotGraph2D_Normal();
                    break;

                case plot_2D.Exponential:
                    plotGraph2D_Exponential();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            plotGraph2D_operator(size, move);
            zedGraphRefresh1();
        }

        // stop/auto "||"
        private void button3_Click(object sender, EventArgs e)
        {
            move = number_of_sample - (size * 2 + 1);
            zedGraphControl1.GraphPane.CurveList.Clear();
            switch (choice_2D_plot)
            {
                case plot_2D.Uniform:
                    plotGraph2D_Uniform();
                    break;

                case plot_2D.Geomtric:
                    plotGraph2D_Geometric();
                    break;

                case plot_2D.Normal:
                    plotGraph2D_Normal();
                    break;

                case plot_2D.Exponential:
                    plotGraph2D_Exponential();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            plotGraph2D_operator(size, move);
            zedGraphRefresh1();
            Refresh();
        }

        // one sample right ">"
        private void button4_Click(object sender, EventArgs e)
        {
            if (move < (number_of_sample - (size * 2 + 1)))
                move++;
            zedGraphControl1.GraphPane.CurveList.Clear();
            switch (choice_2D_plot)
            {
                case plot_2D.Uniform:
                    plotGraph2D_Uniform();
                    break;

                case plot_2D.Geomtric:
                    plotGraph2D_Geometric();
                    break;

                case plot_2D.Normal:
                    plotGraph2D_Normal();
                    break;

                case plot_2D.Exponential:
                    plotGraph2D_Exponential();
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            plotGraph2D_operator(size, move);
            zedGraphRefresh1();
        }

        // calculation
        private double AllanDeviation(double[] data, int tau_maximum,
            double[] samples_tab, int sampling_interval, int sampletab_size)
        {
            int tau_zero = tau_maximum / 10;
         //   int sampling_interval = tau_zero;

            int sampling_momentum = 0;

            double[] temp_tab;

            switch (choice_2D_plot)
            {
                case plot_2D.Uniform:
                {
                    temp_tab = uniform_table_of_Sample;
                    break;
                }

                case plot_2D.Geomtric:
                {
                    temp_tab = geometric_table_of_Sample;
                    break;
                }

                case plot_2D.Normal:
                {
                    temp_tab = normal_table_of_Sample;
                    break;
                }

                case plot_2D.Exponential:
                {
                    temp_tab = exponential_table_of_Sample;
                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }



            //////////////////
           double[] tab = samples_tab;                         //tablica próbek błędu czasu
            int number_of_elements = sampletab_size;            //liczba próbek
            int gap = sampling_interval;                        //odstęp próbkowania tau

            double[] a_dev = new double[sampletab_size];       //tablica do przechowywania wartości dewiacji allana
            double temp = 0;                                    //do przechowywania sumy kwadratów drugich różnic
            double[] square_sum = new double[sampletab_size];
            double fraction = 0;
            int intervals = 2;                              //liczba przedziałów

            bool is_active = false;                         //flaga o aktywności operatora

            int current = 0;                                  //aktualna próbka
            double sum = 0;


          while (current <= number_of_elements)
            {
                for (var j = 2 * gap + 1; j < current; j++)
                {

                    if (gap == 0)
                    {
                        gap++;
                        continue;
                    }

                    if (current <= 2 * gap)
                    {
                        current++;
                        a_dev[current] = 0;
                        square_sum[current] = 0;
                        continue;
                    }

                    else
                    {
                        is_active = true;      //spełniony warunek aktywacji operatora
                        sum = tab[j] - 2 * tab[j - gap] + tab[j - 2 * gap];
                        temp += Math.Pow(sum, 2);
                        square_sum[current] = temp;
                    }
                }

                fraction = 1 / (2 * Math.Pow(intervals, 2) * Math.Pow(gap, 2) * (current - 2 * gap));       //ułamek ze wzoru

                a_dev[current] = Math.Sqrt(fraction * (square_sum[current - 1] + Math.Pow(tab[current] - 2 * tab[current - gap] + tab[current - 2 * gap], 2)));   //dewiacja allana dla danej chwili próbkowania 
            }

            return 0;
        }

        private static void AllanDeviation()
        {
 


     
        }

        // calculation
        private double TimeDeviation(double[] data, int tau_zero)
        {
            int sampling_interval = tau_zero;
            int observation = 2 * tau_zero;

            int j = 0;
            int sampling_momentum = 0;

            double[] temp_tab;

            switch (choice_2D_plot)
            {
                case plot_2D.Uniform:
                {
                    temp_tab = uniform_table_of_Sample;
                    break;
                }

                case plot_2D.Geomtric:
                {
                    temp_tab = geometric_table_of_Sample;
                    break;
                }

                case plot_2D.Normal:
                {
                    temp_tab = normal_table_of_Sample;
                    break;
                }

                case plot_2D.Exponential:
                {
                    temp_tab = exponential_table_of_Sample;
                    break;
                }

                default:
                    throw new ArgumentOutOfRangeException();
            }

            while (j < observation)
            {
            }

            return 0;
        }

       // DTDEV
        private void button5_Click(object sender, EventArgs e)
        {
            plot_3D();
        }

        // DADEV 
        private void button6_Click(object sender, EventArgs e)
        {
            plot_3D();
        }



        private void plot_3D()
        {
            double[] z = new double[31 * 31];
            for (int x = 0; x < 31; x++)
            for (int y = 0; y < 31; y++)
                z[31 * x + y] = (x - 15) * (x - 15) + (y - 15) * (y - 15);
            GnuPlot.Set("pm3d");
            GnuPlot.Set("autoscale");
            GnuPlot.Set("contour base");
            GnuPlot.SPlot(31, z);
            Thread.Sleep(2000);

            GnuPlot.HoldOn();
            GnuPlot.Set("view map");
            GnuPlot.Unset("surface");
            //GnuPlot.Set("cntrparam levels 10");
            //GnuPlot.Set("palette gray");
            //GnuPlot.SPlot(31, z);
            GnuPlot.HoldOff();
            Thread.Sleep(2000);

            //GnuPlot.HoldOff();
           
        }

        // close
        private void button7_Click(object sender, EventArgs e)
        {
             GnuPlot.Close();
        }
    }
}