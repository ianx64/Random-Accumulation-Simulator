using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Runtime.CompilerServices;
using Zanaptak.PcgRandom;
using System.Linq;

namespace Random_Accumulation_Simulator
{
    /// <summary>
    /// ModeControl01.xaml 的交互逻辑
    /// </summary>
    /// 



    public partial class ModeControl01 : UserControl
    {
        /// <summary>
        /// 内部变量集合
        /// </summary>

        private int generateCount;
        private int activeCount;
        private double totalWeight;

        public int GenerateCount { get => generateCount; set => generateCount = value; }
        public int ActiveCount { get => activeCount; set => activeCount = value; }
        public double TotalWeight { get => totalWeight; set => totalWeight = value; }

        /// <summary>
        /// 输入控件组集合        
        /// </summary>
        private List<CheckBox> isActiveList = new List<CheckBox>();
        private List<CheckBox> isDiscreteList = new List<CheckBox>();
        private List<TextBox> sValueMinList = new List<TextBox>();
        private List<TextBox> sValueMaxList = new List<TextBox>();
        private List<TextBox> sDeltaList = new List<TextBox>();
        private List<TextBox> sWeightList = new List<TextBox>();    
        public List<CheckBox> IsActiveList { get => isActiveList; set => isActiveList = value; }
        public List<CheckBox> IsDiscreteList { get => isDiscreteList; set => isDiscreteList = value; }
        public List<TextBox> SValueMinList { get => sValueMinList; set => sValueMinList = value; }
        public List<TextBox> SValueMaxList { get => sValueMaxList; set => sValueMaxList = value; }
        public List<TextBox> SDeltaList { get => sDeltaList; set => sDeltaList = value; }
        public List<TextBox> SWeightList { get => sWeightList; set => sWeightList = value; }
        

        /// <summary>
        /// 输入参数集合
        /// </summary>

        private List<double> valueMin = new List<double>();
        private List<double> valueMax = new List<double>();
        private List<double> delta = new List<double>();
        private List<double> weight = new List<double>();
        private double rangeMin;
        private double rangeMax;
        private int accumulationCount;
        private int repeatCount;

        public List<double> ValueMin { get => valueMin; set => valueMin = value; }
        public List<double> ValueMax { get => valueMax; set => valueMax = value; }
        public List<double> Delta { get => delta; set => delta = value; }
        public List<double> Weight { get => weight; set => weight = value; }
        public double RangeMin { get => rangeMin; set => rangeMin = value; }
        public double RangeMax { get => rangeMax; set => rangeMax = value; }
        public int AccumulationCount { get => accumulationCount; set => accumulationCount = value; }
        public int RepeatCount { get => repeatCount; set => repeatCount = value; }


        /// <summary>
        /// 计算结果及输出集合
        /// </summary>
        /// 
        
        private List<double> sumList = new List<double>();
        private double probabilityLower;
        private double probabilityFit;
        private double probabilityHigher;
        private double sumMin;
        private double sumAverage;
        private double sumMax;

        public List<double> SumList { get => sumList; set => sumList = value; }
        public double ProbabilityLower { get => probabilityLower; set => probabilityLower = value; }
        public double ProbabilityFit { get => probabilityFit; set => probabilityFit = value; }
        public double ProbabilityHigher { get => probabilityHigher; set => probabilityHigher = value; }
        public double SumAverage { get => sumAverage; set => sumAverage = value; }
        public double SumMax { get => sumMax; set => sumMax = value; }
        public double SumMin { get => sumMin; set => sumMin = value; }
      

        /// <summary>
        /// 报错检查
        /// </summary>
        ///
        private bool error = false;
        public bool Error { get => error; set => error = value; }

        Pcg pcg = new Pcg(); //Permuted congruential generator (PCG)，使用置换同余生成器算法




        /// <summary>
        /// 初始化，添加第一行（初始随机事件）输入控件组
        /// </summary>
        public ModeControl01()
        {
            InitializeComponent();
            
            IsActiveList.Add(isActive_0);
            IsDiscreteList.Add(isDiscrete_0);
            SValueMinList.Add(sValueMin_0);
            SValueMaxList.Add(sValueMax_0);
            SDeltaList.Add(sDelta_0);
            SWeightList.Add(sWeight_0);
            GenerateCount = 1;


        }



        /// <summary>
        /// 添加随机事件。按键后添加一个随机事件的相关控件（一行输入控件组）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e)
        {
            

            // 添加新的行
            while (flexibleGrid.RowDefinitions.Count < GenerateCount+1)
            {
                Grid grid = new Grid();
                RowDefinition newRow = new RowDefinition();
                flexibleGrid.RowDefinitions.Add(newRow);

            }


            //添加isActive_n
            CheckBox isActiveBox = new CheckBox {
                Tag = GenerateCount,//开关CheckBox时用于追踪事件来源
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 10, 10, 10),
                IsChecked = true,                
            };

            flexibleGrid.Children.Add(isActiveBox);
            flexibleGrid.RegisterName("isActive_" + GenerateCount, isActiveBox);
            isActiveBox.SetValue(Grid.ColumnProperty, 0);
            isActiveBox.SetValue(Grid.RowProperty, GenerateCount);     

            isActiveBox.Checked += new RoutedEventHandler(isActive_n_Checked);
            isActiveBox.Unchecked += new RoutedEventHandler(isActive_n_Unchecked);
            IsActiveList.Add(isActiveBox);


            //添加isDiscrete_n
            CheckBox isDiscreteBox = new CheckBox
            {
                Tag = GenerateCount,//开关CheckBox时用于追踪事件来源
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10, 10, 10, 10),
                IsChecked = false,
            };

            flexibleGrid.Children.Add(isDiscreteBox);
            flexibleGrid.RegisterName("isDiscrete_" + GenerateCount, isDiscreteBox);
            isDiscreteBox.SetValue(Grid.ColumnProperty, 1);
            isDiscreteBox.SetValue(Grid.RowProperty, GenerateCount);

            isDiscreteBox.Checked += new RoutedEventHandler(isDiscrete_n_Checked);
            isDiscreteBox.Unchecked += new RoutedEventHandler(isDiscrete_n_Unchecked);
            IsDiscreteList.Add(isDiscreteBox);



            //添加sValueMin_n
            TextBox valueMinBox = new TextBox
            {
                Tag = GenerateCount,//以下TextBox的Tag没什么用，顺手加的，万一有新功能
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                MinWidth = 80,
                Height = 20

            };

            flexibleGrid.Children.Add(valueMinBox);
            flexibleGrid.RegisterName("sValueMin_" + GenerateCount, valueMinBox);
            valueMinBox.SetValue(Grid.ColumnProperty, 2);
            valueMinBox.SetValue(Grid.RowProperty, GenerateCount);
            SValueMinList.Add(valueMinBox);


            //添加sValueMax_n
            TextBox valueMaxBox = new TextBox
            {
                Tag = GenerateCount,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                MinWidth = 80,
                Height = 20

            };

            flexibleGrid.Children.Add(valueMaxBox);
            flexibleGrid.RegisterName("sValueMax_" + GenerateCount, valueMaxBox);
            valueMaxBox.SetValue(Grid.ColumnProperty, 3);
            valueMaxBox.SetValue(Grid.RowProperty, GenerateCount);
            SValueMaxList.Add(valueMaxBox);




            //添加sDelta_n
            TextBox deltaBox = new TextBox
            {
                Tag = GenerateCount,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Visibility = Visibility.Hidden,
                MinWidth = 80,
                Height = 20
                
            }; 

            flexibleGrid.Children.Add(deltaBox);
            flexibleGrid.RegisterName("sDelta_" + GenerateCount, deltaBox);
            deltaBox.SetValue(Grid.ColumnProperty, 4);
            deltaBox.SetValue(Grid.RowProperty, GenerateCount);
            SDeltaList.Add(deltaBox);


            //添加sWeight_n
            TextBox weightBox = new TextBox
            {
                Tag = GenerateCount,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Visibility = Visibility.Visible,
                MinWidth=80,
                Height=20,
                Text="1"
            }; 

            flexibleGrid.Children.Add(weightBox);
            flexibleGrid.RegisterName("sWeight_" + GenerateCount, weightBox);
            weightBox.SetValue(Grid.ColumnProperty, 5);
            weightBox.SetValue(Grid.RowProperty, GenerateCount);
            SWeightList.Add(weightBox);
           
            GenerateCount++; //随机事件计数器

        }

        /// <summary>
        /// 删掉最新一个随机事件的相关控件（最下一行输入控件组）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Remove_Click(object sender, RoutedEventArgs e)
        {

            if (GenerateCount > 1)
            {
                GenerateCount--; //随机事件数计数器向后一位，指向最后生成的事件

                CheckBox isActiveBox = flexibleGrid.FindName("isActive_" + GenerateCount) as CheckBox;
                if (isActiveBox != null)
                {
                    flexibleGrid.Children.Remove(isActiveBox);
                    flexibleGrid.UnregisterName("isActive_" + GenerateCount);
                    IsActiveList.Remove(isActiveBox);
                }

                CheckBox isDiscreteBox = flexibleGrid.FindName("isDiscrete_" + GenerateCount) as CheckBox;
                if (isDiscreteBox != null)
                {
                    flexibleGrid.Children.Remove(isDiscreteBox);
                    flexibleGrid.UnregisterName("isDiscrete_" + GenerateCount);
                    IsDiscreteList.Remove(isDiscreteBox);
                }

                TextBox valueMinBox = flexibleGrid.FindName("sValueMin_" + GenerateCount) as TextBox;
                if (valueMinBox != null)
                {
                    flexibleGrid.Children.Remove(valueMinBox);
                    flexibleGrid.UnregisterName("sValueMin_" + GenerateCount);
                    SValueMinList.Remove(valueMinBox);
                }

                TextBox valueMaxBox = flexibleGrid.FindName("sValueMax_" + GenerateCount) as TextBox;
                if (valueMaxBox != null)
                {
                    flexibleGrid.Children.Remove(valueMaxBox);
                    flexibleGrid.UnregisterName("sValueMax_" + GenerateCount);
                    SValueMaxList.Remove(valueMaxBox);
                }

                TextBox deltaBox = flexibleGrid.FindName("sDelta_" + GenerateCount) as TextBox;
                if (deltaBox != null)
                {
                    flexibleGrid.Children.Remove(deltaBox);
                    flexibleGrid.UnregisterName("sDelta_" + GenerateCount);
                    SDeltaList.Remove(deltaBox);
                }

                TextBox weightBox = flexibleGrid.FindName("sWeight_" + GenerateCount) as TextBox;
                if (weightBox != null)
                {
                    flexibleGrid.Children.Remove(weightBox);
                    flexibleGrid.UnregisterName("sWeight_" + GenerateCount);
                    SDeltaList.Remove(weightBox);
                }


            }
        }

             

        /// <summary>
        /// 选中“激活”时，显示对应 概率权重，无需实时监控数据，这个比binding方便多了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void isActive_n_Checked(object sender, RoutedEventArgs e)
        {
            if (SWeightList.Count > 0)
            {  //isActive_0.isChecked初始值是True，不检验会在 InitializeComponent()报错


                CheckBox checkBox = e.Source as CheckBox;
                string i = checkBox.Tag.ToString();
                int j = Convert.ToInt32(i);
                SWeightList[j].SetValue(TextBox.VisibilityProperty, Visibility.Visible);

            }

        }

        /// <summary>
        /// 取消选中“激活”时，隐藏对应 概率权重，无效但不修改值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void isActive_n_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SWeightList.Count > 0) //以下检验影响不大，顺手加了
            {
                CheckBox checkBox = e.Source as CheckBox;
                string i = checkBox.Tag.ToString();
                int j = Convert.ToInt32(i);
                SWeightList[j].SetValue(TextBox.VisibilityProperty, Visibility.Hidden);
              
            }
        }
        /// <summary>
        /// 选中“离散”时，显示对应 离散值邻间距
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         private void isDiscrete_n_Checked(object sender, RoutedEventArgs e)
        {
            if (SDeltaList.Count > 0)
            {
                CheckBox checkBox = e.Source as CheckBox;
                string i = checkBox.Tag.ToString();
                int j = Convert.ToInt32(i);
                SDeltaList[j].SetValue(TextBox.VisibilityProperty, Visibility.Visible);
            }

        }

        /// <summary>
        /// 取消选中“离散”时，隐藏对应 离散值邻间距，无效但不修改值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void isDiscrete_n_Unchecked(object sender, RoutedEventArgs e)
        {
            if (SDeltaList.Count > 0)
            {
                CheckBox checkBox = e.Source as CheckBox;
                string i = checkBox.Tag.ToString();
                int j = Convert.ToInt32(i);
                SDeltaList[j].SetValue(TextBox.VisibilityProperty, Visibility.Hidden);
            }

        }


        /// <summary>
        /// 开始模拟！！！
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            Clean ();
            Assign();
            if (Error == true)
            {
                return;
            }

            SortSumList();
            if (Error == true)
            {
                return;
            }
            MinAveMax();
            GetProbability();
            Output();

        }

        /// <summary>
        /// 清空输出结果，不影响输入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            Clean();
        }



        public void Assign()
        {
            ///录入累加值相关参数

            if (sRangeMax.Text == "")
            {
                sRangeMax.Text = "0";
            }
            if (sRangeMin.Text == "")
            {
                sRangeMin.Text = "0";
            }
            if (sAccumulation.Text == "")
            {
                sAccumulation.Text = "0";
            }
            if (sRepeat.Text == "")
            {
                sRepeat.Text = "0";
            }

            RangeMax = Convert.ToDouble(sRangeMax.Text);
            RangeMin = Convert.ToDouble(sRangeMin.Text);
            AccumulationCount = Convert.ToInt32(sAccumulation.Text);
            RepeatCount = Convert.ToInt32(sRepeat.Text);



            if (RangeMin > RangeMax)
            {
                sDebug.Text = "落点上下界有误";
                Error = true;
                return;
            }

            if (AccumulationCount <= 0)
            {
                sDebug.Text = "累加次数有误";
                Error = true;
                return;
            }

            if (RepeatCount <= 0)
            {
                sDebug.Text = "模拟次数有误";
                Error = true;
                return;
            }



            ///录入所有随机事件参数

            for (int i = 0; i < GenerateCount; i++)//依序检查每个随机事件
            {

               
                if (IsActiveList[i].IsChecked == true)//检查事件是否激活，若是则录入
                {





                    ///检验并收录入事件概率权重，权重为0不会报错，效果和未激活一致


                    if (SWeightList[i].Text == "") {
                        sDebug.Text = "事件"+(i+1)+"已激活，概率权重不可为空";
                        Error = true;
                        return;
                    }

                    Weight.Add(Convert.ToDouble(SWeightList[i].Text));

                    if (Weight[ActiveCount] < 0)
                    {
                        sDebug.Text = "事件" + (i + 1) + "已激活，概率权重不可为负";
                        Error = true;
                        return;
                    }

                    TotalWeight += Weight[ActiveCount];


                    ///检验并录入随机数上下界

                    if (SValueMinList[i].Text == "")
                    {
                        SValueMinList[i].Text = "0"; //Convert.ToDouble("0"+ negative value)会报错，用if先换成0更方便，且有利于用户理解
                    }

                    ValueMin.Add(Convert.ToDouble(SValueMinList[i].Text));


                    if (SValueMaxList[i].Text == "")
                    {
                        SValueMaxList[i].Text = "0";
                    }
                                        
                    ValueMax.Add(Convert.ToDouble(SValueMaxList[i].Text));


                    if (ValueMin[ActiveCount] > ValueMax[ActiveCount])
                    {
                        sDebug.Text = "事件"+(i+1)+"随机数上下界有误";
                        Error = true;
                        return;
                    }


                    ///检验并录入离散值邻间距，连续变量则用0标识

                    if (IsDiscreteList[i].IsChecked == true){
                        if (SDeltaList[i].Text == "")
                        {
                            sDebug.Text = "事件" + (i + 1) + "为离散随机数，离散值邻间距不可为空";
                            Error = true;
                            return;
                        }
                        Delta.Add(Convert.ToDouble(SDeltaList[i].Text));

                        if (Delta[ActiveCount]<=0)
                        {
                            sDebug.Text = "事件" + (i + 1) + "为离散随机数，离散值邻间距不可为0或负值";
                            Error = true;
                            return;
                        }
                    }
                    else
                    {
                        Delta.Add(0); //0用于标识连续变量
                    }

                    ActiveCount++; //激活事件计数
                }

            
            }

            
            if (ActiveCount == 0)
            {
                sDebug.Text = "尚无激活的随机事件";
                Error = true;
                return;
            }

            if (TotalWeight <= 0)
            {
                sDebug.Text = "事件总概率权重不能为0";
                Error = true;
                return;
            }

        }




        public void SortSumList() //生成所有落点并排序
        {
            

            for (int i = 0; i < RepeatCount; i++) //重复模拟i次
            {
                double sum = 0;//每次模拟的累加和

                for (int j =0; j< AccumulationCount; j++)//每次模拟累加j个随机值
                {
                    double random = GetRandom();
                    if (Error == true)
                    {
                        return;
                    }
                    sum = sum + random;
                    
                }

                SumList.Add(sum);//将生成的累加和录入集合
            }

            SumList.Sort();//从小到大排列SumList
        }

        public double GetRandom() //根据权重随机抽取（PCG）一个已激活的随机事件,返回随机值
        {
            
            double placement = pcg.NextDouble() * TotalWeight;
            double checkPoint = 0;

            for (int i = 0; i < ActiveCount; i++)
            {
                checkPoint += Weight[i];
                if (placement < checkPoint) //触发随机事件
                {
                    if (Delta[i]==0)
                    {
                        return ContinuousRandom(ValueMin[i], ValueMax[i]);
                    }
                    else
                    {
                        return DiscreteRandom(ValueMin[i], ValueMax[i], Delta[i]);
                    }

                }
            }
            
            if (placement == checkPoint)
            {
                if (Delta[ActiveCount] == 0)
                {
                    return ContinuousRandom(ValueMin[ActiveCount], ValueMax[ActiveCount]);
                }
                else
                {
                    return DiscreteRandom(ValueMin[ActiveCount], ValueMax[ActiveCount], Delta[ActiveCount]);
                }
            }
            else
            {
                Error = true;
                sDebug.Text = "随机值生成器出错，请重试";
                return 0;
                
            }
        }

        public double ContinuousRandom(double lowerBound, double upperBound) //生成连续变量，使用PCG产生随机数
        {
            
            double scale = upperBound - lowerBound;
            double x = pcg.NextDouble() * scale + lowerBound;
            return x;
        }

        public double DiscreteRandom(double lowerBound, double upperBound, double delta) //生成离散变量, 使用PCG产生随机数
        {
            
            int deltaNumber = Convert.ToInt32(Math.Floor((upperBound - lowerBound) / delta));
            int normalised = pcg.Next(0,deltaNumber+1);
            double x = lowerBound + (delta * normalised);
            return x;
        }

            
                        

        public void MinAveMax()
        {
            SumMin = SumList[0];
            SumAverage = SumList.Average();
            SumMax = SumList.Last();
        }


        public void GetProbability()
        {


            double fitCount = 0;
            double lowerCount = 0;
            double higherCount = 0;

            for (int i = 0; i < SumList.Count; i++)
            {

                if (SumList[i] < RangeMin)
                {
                    lowerCount++;
                }
                else if (SumList[i] <= RangeMax)
                {
                    fitCount++;
                }
                else
                {
                    higherCount++;
                }
            }

            ProbabilityLower = lowerCount * 100 / SumList.Count;
            ProbabilityFit = fitCount * 100 / SumList.Count;
            ProbabilityHigher = higherCount * 100 / SumList.Count;

            //求出落点区域概率

        }


        public void Output()
        {
            sMax.Text = SumMax.ToString("0.####");
            sMin.Text = SumMin.ToString("0.####");
            sAverage.Text = SumAverage.ToString("0.####");
            sProbabilityLower.Text = ProbabilityLower.ToString("0.####") + "%";
            sProbabilityFit.Text = ProbabilityFit.ToString("0.####") + "%";
            sProbabilityHigher.Text = ProbabilityHigher.ToString("0.####") + "%";
            
        }

        public void Clean()
        {
            sMin.Text = "";
            sAverage.Text = "";
            sMax.Text = "";
            sProbabilityLower.Text = "";
            sProbabilityFit.Text = "";
            sProbabilityHigher.Text = "";
            sDebug.Text = "";

            ValueMin.Clear();
            ValueMax.Clear();
            Delta.Clear();
            Weight.Clear();
            SumList.Clear();
                       
            ActiveCount = 0;
            TotalWeight = 0;

            Error = false;
        }


    }
}
