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
    /// Assess.xaml 的交互逻辑
    /// </summary>
    public partial class Assess : Page
    {
        public Assess()
        {
            InitializeComponent();

            combo_dept.Items.Add("人事部"); combo_dept.Items.Add("财务部"); combo_dept.Items.Add("销售部");
            combo_dept.Items.Add("生产部"); combo_dept.Items.Add("技术部"); combo_dept.Items.Add("采购部");
            combo_dept.Items.Add("后勤部"); combo_dept.Items.Add("基础设施部"); combo_dept.Items.Add("办公室");

            combo_result.Items.Add("优秀"); combo_result.Items.Add("合格"); combo_result.Items.Add("不合格");
            combo_yewu.Items.Add("优"); combo_yewu.Items.Add("良"); combo_yewu.Items.Add("合格");
            combo_yewu.Items.Add("不合格");
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
            textBlock_ID.Text = "";
            textBox_askLeave.Text = "";
            textBox_lateCount.Text="";
            textBox_name.Text = "";
            textBox_queqin.Text = "";
            textBox_sno.Text = "";
            combo_dept.SelectedItem = null;
            combo_result.SelectedItem = null;
            combo_yewu.SelectedItem = null;
            datePicker_date.SelectedDate = null;
            photo.Source = null;
        }

        //将请假、缺勤、迟到情况显示
        private void btnAssess_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_sno.Text == null)
            {
                MessageBox.Show("请选择需要考核的员工！");
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

                        String str_Assess = "exec Pro_Assess @sno,@date";
                        SqlParameter sno = new SqlParameter("@sno", textBox_sno.Text);
                        com.Parameters.Add(sno);
                        SqlParameter date = new SqlParameter("@date", datePicker_date.SelectedDate);
                        com.Parameters.Add(date);
                        com.CommandText = str_Assess;
                        if (com.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("该员工基本信息考核完成", "成功", MessageBoxButton.OK);

                            //显示迟到次数
                            string str1 = "select 迟到次数 from 考核表 where 员工编号=@sno and year(考核日期)=year(@date) and month(考核日期)=month(@date)";
                            com.CommandText = str1;
                            Object obj3 = com.ExecuteScalar();
                            textBox_lateCount.Text = obj3.ToString();
                            //显示缺勤次数
                            string str2 = "select 缺勤次数 from 考核表 where 员工编号=@sno and year(考核日期)=year(@date) and month(考核日期)=month(@date)";
                            com.CommandText = str2;
                            Object obj4 = com.ExecuteScalar();
                            textBox_queqin.Text = obj4.ToString();
                            //显示请假天数
                            string str3 = "select 请假天数 from 考核表 where 员工编号=@sno and year(考核日期)=year(@date) and month(考核日期)=month(@date)";
                            com.CommandText = str3;
                            Object obj5 = com.ExecuteScalar();
                            textBox_askLeave.Text = obj5.ToString();
                            //显示姓名
                            string strSelect_name = "select 员工姓名 from 员工表 where 员工编号=@sno";
                            com.CommandText = strSelect_name;
                            Object obj12 = com.ExecuteScalar();
                            textBox_name.Text = obj12.ToString();
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
                            //显示考核编号
                            string str4 = "select 记录编号 from 考核表 where 员工编号=@sno and year(考核日期)=year(@date) and month(考核日期)=month(@date)";
                            com.CommandText = str4;
                            Object obj = com.ExecuteScalar();
                            textBlock_ID.Text = obj.ToString();
                        }
                        else
                        {
                            MessageBox.Show("考核失败", "失败", MessageBoxButton.OK);
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_sno.Text == "")
            {
                MessageBox.Show("员工编号不能为空！");
            }
            else if (combo_yewu.SelectedItem==null)
            {
                MessageBox.Show("业务评价不能为空！");
            }
            else if (combo_result.SelectedItem==null)
            {
                MessageBox.Show("综合评价不能为空！");
            }
            
            else if (MessageBox.Show("确认考核？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
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
                        string str_AddAssess = "update 考核表 set 业务考核=@yewu,综合评价=@final where 员工编号=@sno and year(考核日期)=year(@date) and month(考核日期)=month(@date)";
                        SqlParameter sno = new SqlParameter("@sno", textBox_sno.Text);
                        comm.Parameters.Add(sno);
                        SqlParameter date = new SqlParameter("@date", datePicker_date.SelectedDate);
                        comm.Parameters.Add(date);
                        SqlParameter yewu = new SqlParameter("@yewu", combo_yewu.SelectedItem);
                        comm.Parameters.Add(yewu);
                        SqlParameter final = new SqlParameter("@final", combo_result.SelectedItem);
                        comm.Parameters.Add(final);
                        

                        comm.CommandText = str_AddAssess;
                        if (comm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("考核完成！", "成功", MessageBoxButton.OK);
                            string str_all = "select * from 考核表 where 员工编号=@sno";
                            comm.CommandText = str_all;

                            SqlDataAdapter sda = new SqlDataAdapter(comm);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            dataGrid_assess.ItemsSource = dt.DefaultView;
                        }
                        else
                        {
                            MessageBox.Show("考核失败！", "失败", MessageBoxButton.OK);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {

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

                        String strSelect_AskLeave = "select * from 考核表 where 员工编号=@sno";
                        SqlParameter sno = new SqlParameter("@sno", textBox_sno.Text);
                        com.Parameters.Add(sno);
                        com.CommandText = strSelect_AskLeave;
                        SqlDataAdapter sda = new SqlDataAdapter(com);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        dataGrid_assess.ItemsSource = dt.DefaultView;

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
}
