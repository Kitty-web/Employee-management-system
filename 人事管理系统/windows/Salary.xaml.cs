using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using System.Data;
using System.Data.SqlClient;


namespace 人事管理系统.windows
{
    /// <summary>
    /// Salary.xaml 的交互逻辑
    /// </summary>
    public partial class Salary : Page
    {
        public Salary()
        {
            InitializeComponent();
            combo_dept.Items.Add("人事部"); combo_dept.Items.Add("财务部"); combo_dept.Items.Add("销售部");
            combo_dept.Items.Add("生产部"); combo_dept.Items.Add("技术部"); combo_dept.Items.Add("采购部");
            combo_dept.Items.Add("后勤部"); combo_dept.Items.Add("基础设施部"); combo_dept.Items.Add("办公室");
        }

        private void btnSelectImg_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.FileName = "图片";
            dlg.DefaultExt = "*.*";
            dlg.Filter = "全部文件(.*)|*.*|bmp文件(.bmp)|*.bmp|jpg文件(.jpg)|*.jpg|gif文件(.gif)|*.gif|png文件(.png)|*.png|ico文件(.ico)|*.ico";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                photo.Source = new BitmapImage(new Uri(dlg.FileName, UriKind.Absolute));
            }
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            textBox_all.Text = "";
            textBox_base.Text = "";
            textBox_name.Text = "";
            textBox_punish.Text = "";
            textBox_reward.Text = "";
            textBox_sno.Text = "";
            datePicker_date.SelectedDate = null;
            combo_dept.SelectedItem = null;
            photo.Source = null;
        }
        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_sno.Text == "")
            {
                MessageBox.Show("员工编号不能为空！");
            }
            else if (textBox_name.Text == "")
            {
                MessageBox.Show("员工姓名不能为空！");
            }
            else if (textBox_base.Text == "")
            {
                MessageBox.Show("员工底薪不能为空！");
            }
            else if (MessageBox.Show("确认修改？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
            {
                string strConnection = "Server=DELL;";
                strConnection += "initial catalog=员工信息管理系统;";
                strConnection += "user id=sa;";
                strConnection += "password=l753159t;";
                strConnection += "Connect Timeout=5";

                using (SqlConnection conn = new SqlConnection(strConnection))
                {
                    try
                    {
                        conn.Open();
                        SqlCommand comm = new SqlCommand();
                        comm.Connection = conn;
                        string str_AddSalary = "update 工资表 set 底薪=@base where 员工编号=@sno";
                        SqlParameter sno = new SqlParameter("@sno", textBox_sno.Text);
                        comm.Parameters.Add(sno);
                        SqlParameter mbase = new SqlParameter("@base", textBox_base.Text);
                        comm.Parameters.Add(mbase);

                        comm.CommandText = str_AddSalary;
                        if (comm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("修改工资成功！", "成功", MessageBoxButton.OK);

                            string str_all = "select * from 工资表";
                            comm.CommandText = str_all;
                            SqlDataAdapter sda = new SqlDataAdapter(comm);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            dataGrid_salary.ItemsSource = dt.DefaultView;
                        }
                        else
                        {
                            MessageBox.Show("修改工资失败！", "失败", MessageBoxButton.OK);
                        }

                        if (textBox_reward.Text != "")
                        {
                            SqlParameter reward = new SqlParameter("@reward", textBox_reward.Text);
                            comm.Parameters.Add(reward);
                            string s1 = "update 工资表 set 奖金=@reward where 员工编号=@sno";
                            comm.CommandText = s1;
                            comm.ExecuteNonQuery();
                        }

                        if (textBox_punish.Text != "")
                        {
                            SqlParameter punish = new SqlParameter("@punish", textBox_punish.Text);
                            comm.Parameters.Add(punish);
                            string s2 = "update 工资表 set 扣除=@punish where 员工编号=@sno";
                            comm.CommandText = s2;
                            comm.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_sno.Text == "")
            {
                MessageBox.Show("请指明员工编号进行查询！");
            }
            else
            {
                string strConnection = "Server=DELL;";
                strConnection += "initial catalog=员工信息管理系统;";
                strConnection += "user id=sa;";
                strConnection += "password=l753159t;";
                strConnection += "Connect Timeout=5";
                using (SqlConnection objConnection = new SqlConnection(strConnection))
                {
                    try
                    {
                        objConnection.Open();
                        SqlCommand com = new SqlCommand();
                        com.Connection = objConnection;

                        SqlParameter sno = new SqlParameter("@sno", textBox_sno.Text);
                        com.Parameters.Add(sno);
                        //显示姓名
                        string strSelect_name = "select 员工姓名 from 工资表 where 员工编号=@sno";
                        com.CommandText = strSelect_name;
                        Object obj12 = com.ExecuteScalar();
                        textBox_name.Text = obj12.ToString();
                       
                        //显示底薪
                        string strSelect_base = "select 底薪 from 工资表 where 员工编号=@sno";
                        com.CommandText = strSelect_base;
                        Object obj4 = com.ExecuteScalar();
                        textBox_base.Text = obj4.ToString();
                        //显示奖金
                        string strSelect_reward = "select 奖金 from 工资表 where 员工编号=@sno";
                        com.CommandText = strSelect_reward;
                        Object obj5 = com.ExecuteScalar();
                        textBox_reward.Text = obj5.ToString();
                        //显示扣除
                        string strSelect_punish = "select 扣除 from 工资表 where 员工编号=@sno";
                        com.CommandText = strSelect_punish;
                        Object obj6 = com.ExecuteScalar();
                        textBox_punish.Text = obj6.ToString();
                        //显示总工资
                        string strSelect_all = "select 总工资 from 工资表 where 员工编号=@sno";
                        com.CommandText = strSelect_all;
                        Object obj7 = com.ExecuteScalar();
                        textBox_all.Text = obj7.ToString();
                        //显示所属部门名
                        string strSelect_dept = "select 所属部门名称 from 员工表 where 员工编号=@sno";
                        com.CommandText = strSelect_dept;
                        Object obj9 = com.ExecuteScalar();
                        combo_dept.SelectedItem = obj9.ToString();
                        //显示照片
                        string strSelect_photo = "select 照片 from 员工表 where 员工编号=@sno";
                        com.CommandText = strSelect_photo;
                        Object obj13 = com.ExecuteScalar();
                        photo.Source = new BitmapImage(new Uri(obj13.ToString(), UriKind.Absolute));

                        objConnection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void btnFinal_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_sno.Text == null)
            {
                MessageBox.Show("请选择需要结算的员工！");
            }
            else
            {
                string strConnection = "Server=DELL;";
                strConnection += "initial catalog=员工信息管理系统;";
                strConnection += "user id=sa;";
                strConnection += "password=l753159t;";
                strConnection += "Connect Timeout=5";
                using (SqlConnection objConnection = new SqlConnection(strConnection))
                {
                    try
                    {
                        objConnection.Open();
                        SqlCommand com = new SqlCommand();
                        com.Connection = objConnection;

                        SqlParameter sno = new SqlParameter("@sno", textBox_sno.Text);
                        com.Parameters.Add(sno);
                        String s = "select 是否结算 from 工资表 where 员工编号=@sno";
                        com.CommandText = s;
                        Object obj = com.ExecuteScalar();
                        if (obj.ToString() == "否")
                        {
                            String strSelect_CheckOn = "exec Pro_Salary_Final @sno,@date";
                            SqlParameter date = new SqlParameter("@date", datePicker_date.SelectedDate);
                            com.Parameters.Add(date);
                            com.CommandText = strSelect_CheckOn;
                            if (com.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("该员工工资结算完成", "成功", MessageBoxButton.OK);

                                //显示照片
                                string strSelect_photo = "select 照片 from 员工表 where 员工编号=@sno";
                                com.CommandText = strSelect_photo;
                                Object obj13 = com.ExecuteScalar();
                                photo.Source = new BitmapImage(new Uri(obj13.ToString(), UriKind.Absolute));

                                string str = "select * from 工资表 where 员工编号=@sno";
                                com.CommandText = str;
                                SqlDataAdapter sda = new SqlDataAdapter(com);
                                DataTable dt = new DataTable();
                                sda.Fill(dt);
                                dataGrid_salary.ItemsSource = dt.DefaultView;
                            }
                            else
                            {
                                MessageBox.Show("结算失败", "失败", MessageBoxButton.OK);
                            }
                        }
                        else
                        {
                            MessageBox.Show("该员工已经结算过！");
                        }

                        objConnection.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void btnInit_Click(object sender, RoutedEventArgs e)
        {
            string strConnection = "Server=DELL;";
            strConnection += "initial catalog=员工信息管理系统;";
            strConnection += "user id=sa;";
            strConnection += "password=l753159t;";
            strConnection += "Connect Timeout=5";
            using (SqlConnection objConnection = new SqlConnection(strConnection))
            {
                try
                {
                    objConnection.Open();
                    SqlCommand com = new SqlCommand();
                    com.Connection = objConnection;


                    String s = "update 工资表 set 奖金=0,扣除=0,是否结算='否',总工资=4000";
                    com.CommandText = s;
                    if (com.ExecuteNonQuery() > 0)
                    {
                        MessageBox.Show("初始化完成", "成功", MessageBoxButton.OK);

                        string str = "select * from 工资表";
                        com.CommandText = str;
                        SqlDataAdapter sda = new SqlDataAdapter(com);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        dataGrid_salary.ItemsSource = dt.DefaultView;
                    }
                    else
                    {
                        MessageBox.Show("初始化失败", "失败", MessageBoxButton.OK);
                    }

                    objConnection.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
