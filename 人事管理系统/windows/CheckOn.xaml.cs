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
    /// CheckOn.xaml 的交互逻辑
    /// </summary>
    public partial class CheckOn : Page
    {
        public CheckOn()
        {
            InitializeComponent();
            combo_onOroFF.Items.Add("上午"); combo_onOroFF.Items.Add("下午");
            combo_checkOnType.Items.Add("准时"); combo_checkOnType.Items.Add("迟到");
            combo_checkOnType.Items.Add("缺勤"); combo_checkOnType.Items.Add("出差");
            combo_checkOnType.Items.Add("请假");

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
            textBlock_CheckID.Text = "";
            textBox_sno.Text = "";
            textBox_time.Text = "";
            textBox_name.Text = "";
            combo_checkOnType.SelectedItem = null;
            combo_dept.SelectedItem = null;
            combo_onOroFF.SelectedItem = null;
            datePicker_checkOn.SelectedDate = null;
            photo.Source = null;
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (datePicker_checkOn.SelectedDate == null)
            {
                MessageBox.Show("必须填写考勤日期！");
            }
            else if (textBox_sno.Text == "")
            {
                MessageBox.Show("员工编号不能为空！");
            }      
            else if (combo_onOroFF.SelectedItem == null)
            {
                MessageBox.Show("请选择上午或下午！");
            }
            else if (textBox_time.Text == null)
            {
                MessageBox.Show("必须填写签到时间！");
            }
            else if (combo_checkOnType.SelectedItem == null)
            {
                MessageBox.Show("请选择考勤类型");
            }
            else if (MessageBox.Show("确认考勤？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
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
                        string str_AddCheckOn = "insert into 考勤表 values(@date,@sno,@name,@noon,@time,@type)";
                        SqlParameter sno = new SqlParameter("@sno", textBox_sno.Text);
                        comm.Parameters.Add(sno);
                        
                        //显示姓名
                        string strSelect_name = "select 员工姓名 from 员工表 where 员工编号=@sno";
                        comm.CommandText = strSelect_name;
                        Object obj12 = comm.ExecuteScalar();
                        SqlParameter name = new SqlParameter("@name", obj12.ToString());
                        comm.Parameters.Add(name);
                        textBox_name.Text = obj12.ToString();
                        

                        SqlParameter date = new SqlParameter("@date", datePicker_checkOn.SelectedDate);
                        comm.Parameters.Add(date);
                        SqlParameter noon = new SqlParameter("@noon", combo_onOroFF.SelectedItem);
                        comm.Parameters.Add(noon);
                        SqlParameter time = new SqlParameter("@time", Convert.ToDateTime(textBox_time.Text));
                        comm.Parameters.Add(time);
                        SqlParameter type = new SqlParameter("@type", combo_checkOnType.SelectedItem);
                        comm.Parameters.Add(type);

                        //显示照片
                        string strSelect_photo = "select 照片 from 员工表 where 员工编号=@sno";
                        comm.CommandText = strSelect_photo;
                        Object obj13 = comm.ExecuteScalar();
                        photo.Source = new BitmapImage(new Uri(obj13.ToString(), UriKind.Absolute));

                        comm.CommandText = str_AddCheckOn;
                        if (comm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("考勤完成！", "成功", MessageBoxButton.OK);

                            String s = "select 考勤编号 from 考勤表 where 员工编号=@sno and 考勤日期=@date and 上下午=@noon";
                            comm.CommandText = s;
                            Object obj = comm.ExecuteScalar();
                            textBlock_CheckID.Text = obj.ToString();

                            string str_all = "select * from 考勤表";
                            comm.CommandText = str_all;
                            SqlDataAdapter sda = new SqlDataAdapter(comm);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            dataGrid_checkOn.ItemsSource = dt.DefaultView;
                        }
                        else
                        {
                            MessageBox.Show("考勤记录添加失败！", "失败", MessageBoxButton.OK);
                        }
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
            //将缺勤、请假、出差的加入考勤表，然后显示考勤表
            if (datePicker_checkOn.SelectedDate == null)
            {
                MessageBox.Show("请选择需要结算的日期！");
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

                        String strSelect_CheckOn = "exec Pro_CheckOn_Final @date";
                        SqlParameter date = new SqlParameter("@date", datePicker_checkOn.SelectedDate);
                        com.Parameters.Add(date);
                        if (datePicker_checkOn.SelectedDate > DateTime.Now)
                        {
                            MessageBox.Show("该日期还未考勤！");
                        }
                        else
                        {
                            com.CommandText = strSelect_CheckOn;
                        }
                        if (com.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("本日考勤记录结算完成", "成功", MessageBoxButton.OK);

                            string str_all = "select * from 考勤表";
                            com.CommandText = str_all;
                            SqlDataAdapter sda = new SqlDataAdapter(com);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            dataGrid_checkOn.ItemsSource = dt.DefaultView;
                        }
                        else {
                            MessageBox.Show("结算失败", "失败", MessageBoxButton.OK);
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

                        String strSelect_CheckOn = "select * from 考勤表 where 员工编号=@sno";
                        SqlParameter sno = new SqlParameter("@sno", textBox_sno.Text);
                        com.Parameters.Add(sno);
                        com.CommandText = strSelect_CheckOn;
                        SqlDataAdapter sda = new SqlDataAdapter(com);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        dataGrid_checkOn.ItemsSource = dt.DefaultView;

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
