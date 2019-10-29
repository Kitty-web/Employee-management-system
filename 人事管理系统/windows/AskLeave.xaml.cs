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
using System.Data.SqlClient;
using System.Data;

namespace 人事管理系统.windows
{
    /// <summary>
    /// AskLeave.xaml 的交互逻辑
    /// </summary>
    public partial class AskLeave : Page
    {
        public AskLeave()
        {
            InitializeComponent();
            combo_IsExceed.Items.Add("是"); combo_IsExceed.Items.Add("否");
            combo_IsPass.Items.Add("通过"); combo_IsPass.Items.Add("不通过");

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
                    
                    string str = "select 假期名称,起始日期,终止日期 from 假期表";
                    comm.CommandText = str;
                  
                    SqlDataAdapter sda = new SqlDataAdapter(comm);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    dataGrid_freeday.ItemsSource = dt.DefaultView;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            textBox_dayCount.Text = "";
            combo_IsPass.SelectedItem=null;
            textBox_Name.Text = "";
            textBox_reason.Text = "";
            textBox_Sno.Text = "";
            textBlock_ID.Text = "";
            dataPicker_applyDate.SelectedDate = null;
            dataPicker_startDate.SelectedDate = null;
            datePicker_endDate.SelectedDate = null;
            datePicker_endLeave.SelectedDate = null;
            combo_IsExceed.SelectedItem = null;
        }
        private void btnAskLeave_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_Sno.Text == "")
            {
                MessageBox.Show("员工编号不能为空！");
            }
            else if (textBox_Name.Text == "")
            {
                MessageBox.Show("员工姓名不能为空！");
            }
            else if (dataPicker_applyDate.SelectedDate==null)
            {
                MessageBox.Show("必须填写申请日期！");
            }
            else if (dataPicker_startDate.SelectedDate == null)
            {
                MessageBox.Show("必须填写假期开始日期！");
            }
            else if (datePicker_endDate.SelectedDate == null)
            {
                MessageBox.Show("必须填写假期结束日期！");
            }
            else if (combo_IsPass.SelectedItem == null)
            {
                MessageBox.Show("请审核请假申请是否通过");
            }
            else if (textBox_reason.Text == "")
            {
                MessageBox.Show("必须填写请假理由！");
            }
            else if (MessageBox.Show("确认请假？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
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
                        string str_AddLeave = "insert into 请假表(员工编号,员工姓名,申请日期,开始日期,结束日期,请假原因,准假情况,请假天数) values(@sno,@name,@applyDate,@startDate,@endDate,@reason,@isPass,@dayCount)";
                        SqlParameter sno = new SqlParameter("@sno", textBox_Sno.Text);
                        comm.Parameters.Add(sno);
                        SqlParameter name = new SqlParameter("@name", textBox_Name.Text);
                        comm.Parameters.Add(name);
                        SqlParameter apply = new SqlParameter("@applyDate", dataPicker_applyDate.SelectedDate);
                        comm.Parameters.Add(apply);
                        SqlParameter start = new SqlParameter("@startDate", dataPicker_startDate.SelectedDate);
                        comm.Parameters.Add(start);
                        SqlParameter end = new SqlParameter("@endDate", datePicker_endDate.SelectedDate);
                        comm.Parameters.Add(end);
                        SqlParameter reason = new SqlParameter("@reason", textBox_reason.Text);
                        comm.Parameters.Add(reason);
                        SqlParameter isPass = new SqlParameter("@isPass", combo_IsPass.SelectedItem);
                        comm.Parameters.Add(isPass);
                        SqlParameter dayCount = new SqlParameter("@dayCount", textBox_dayCount.Text);
                        comm.Parameters.Add(dayCount);

                        comm.CommandText = str_AddLeave;
                        if (comm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("请假完成！", "成功", MessageBoxButton.OK);

                            String s = "select 记录编号 from 请假表 where 员工编号=@sno and 申请日期=@applyDate";
                            comm.CommandText = s;
                            Object obj = comm.ExecuteScalar();
                            textBlock_ID.Text = obj.ToString();
                            
                            string str_allStaff = "select * from 请假表";
                            comm.CommandText = str_allStaff;
                            SqlDataAdapter sda = new SqlDataAdapter(comm);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            dataGrid_AskLeave.ItemsSource = dt.DefaultView;
                        }
                        else
                        {
                            MessageBox.Show("请假记录添加失败！", "失败", MessageBoxButton.OK);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void btnEndLeave_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_Sno.Text == "")
            {
                MessageBox.Show("员工编号不能为空！");
            }
            else if (datePicker_endLeave.SelectedDate == null)
            {
                MessageBox.Show("必须填写销假日期！");
            }
            else if (combo_IsExceed.SelectedItem == null)
            {
                MessageBox.Show("必须填写是否超期！");
            }
            else if (MessageBox.Show("确认销假？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
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
                        string str_AddEndLeave = "update 请假表 set 销假日期=@endLeave,是否超期=@isExceed where 员工编号=@sno and 销假日期 is NULL and 准假情况='通过'";
                        SqlParameter sno = new SqlParameter("@sno", textBox_Sno.Text);
                        comm.Parameters.Add(sno);
                        
                        SqlParameter endLeave = new SqlParameter("@endLeave", datePicker_endLeave.SelectedDate);
                        comm.Parameters.Add(endLeave);
                        SqlParameter isExceed = new SqlParameter("@isExceed", combo_IsExceed.SelectedItem);
                        comm.Parameters.Add(isExceed); 

                        comm.CommandText = str_AddEndLeave;
                        if (comm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("销假完成！", "成功", MessageBoxButton.OK);
                           
                            string str_allStaff = "select * from 请假表";
                            comm.CommandText = str_allStaff;
                            SqlDataAdapter sda = new SqlDataAdapter(comm);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            dataGrid_AskLeave.ItemsSource = dt.DefaultView;
                        }
                        else
                        {
                            MessageBox.Show("不存在该请假记录！", "失败", MessageBoxButton.OK);
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
            if (textBox_Sno.Text == "")
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

                        String strSelect_AskLeave = "select * from 请假表 where 员工编号=@sno";
                        SqlParameter sno = new SqlParameter("@sno", textBox_Sno.Text);
                        com.Parameters.Add(sno);
                        com.CommandText = strSelect_AskLeave;
                        SqlDataAdapter sda = new SqlDataAdapter(com);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        dataGrid_AskLeave.ItemsSource = dt.DefaultView;

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
