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
    /// WorkOutside.xaml 的交互逻辑
    /// </summary>
    public partial class WorkOutside : Page
    {
        public WorkOutside()
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
            textBlock_ID.Text = "";
            textBox_money.Text = "";
            textBox_name.Text= "";
            textBox_position.Text = "";
            textBox_sno.Text = "";
            combo_dept.SelectedItem = null;
            dataPicker_date.SelectedDate = null;
            datePicker_endDate.SelectedDate = null;
            photo.Source = null;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_sno.Text == "")
            {
                MessageBox.Show("员工编号不能为空！");
            }
            else if (textBox_name.Text == "")
            {
                MessageBox.Show("员工姓名不能为空！");
            }
            else if (textBox_position.Text == "")
            {
                MessageBox.Show("出差地不能为空！");
            }
            else if (textBox_money.Text == "")
            {
                MessageBox.Show("预算不能为空！");
            }
            else if (dataPicker_date.SelectedDate==null)
            {
                MessageBox.Show("出差日期不能为空！");
            }
            else if (datePicker_endDate.SelectedDate==null)
            {
                MessageBox.Show("结束日期不能为空！");
            }
            else if (MessageBox.Show("确认添加？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
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
                        string str_AddWorkOutSide = "insert into 出差表 values(@sno,@name,@position,@money,@startDate,@endDate)";
                        SqlParameter sno = new SqlParameter("@sno", textBox_sno.Text);
                        comm.Parameters.Add(sno);
                        SqlParameter name = new SqlParameter("@name", textBox_name.Text);
                        comm.Parameters.Add(name);
                        SqlParameter position = new SqlParameter("@position", textBox_position.Text);
                        comm.Parameters.Add(position);
                        SqlParameter money = new SqlParameter("@money", textBox_money.Text);
                        comm.Parameters.Add(money);
                        SqlParameter start = new SqlParameter("@startDate", dataPicker_date.SelectedDate);
                        comm.Parameters.Add(start);
                        SqlParameter end = new SqlParameter("@endDate", datePicker_endDate.SelectedDate);
                        comm.Parameters.Add(end);

                        comm.CommandText = str_AddWorkOutSide;
                        if (comm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("添加出差记录成功！", "成功", MessageBoxButton.OK);

                            string str_all = "select * from 出差表";
                            comm.CommandText = str_all;
                            SqlDataAdapter sda = new SqlDataAdapter(comm);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            dataGrid_workOutside.ItemsSource = dt.DefaultView;
                         }
                         else
                         {
                            MessageBox.Show("添加失败！", "失败", MessageBoxButton.OK);
                         }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void btnModity_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_sno.Text == "")
            {
                MessageBox.Show("请指明员工编号进行修改！");
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

                        SqlParameter sno = new SqlParameter("@sno", textBox_sno.Text);
                        comm.Parameters.Add(sno);
                        if (textBox_position.Text != "")
                        {
                            string str_UpdatePosition = "update 出差表 set 出差地=@position where 员工编号=@sno";
                            SqlParameter position = new SqlParameter("@position", textBox_position.Text);
                            comm.Parameters.Add(position);
                            comm.CommandText = str_UpdatePosition;
                            comm.ExecuteNonQuery();
                        }
                        if (textBox_money.Text != "")
                        {
                            string str_UpdateMoney = "update 出差表 set 出差预算=@money where 员工编号=@sno";
                            SqlParameter money = new SqlParameter("@money", textBox_money.Text);
                            comm.Parameters.Add(money);
                            comm.CommandText = str_UpdateMoney;
                            comm.ExecuteNonQuery();
                        }
                        if (dataPicker_date.SelectedDate != null)
                        {
                            string str_UpdateStart = "update 出差表 set 出差日期=@startDate where 员工编号=@sno";
                            SqlParameter start = new SqlParameter("@startDate", dataPicker_date.SelectedDate);
                            comm.Parameters.Add(start);
                            comm.CommandText = str_UpdateStart;
                            comm.ExecuteNonQuery();
                        }
                        if (datePicker_endDate.SelectedDate != null)
                        {
                            string str_UpdateEnd = "update 出差表 set 结束日期=@endDate where 员工编号=@sno";
                            SqlParameter end = new SqlParameter("@endDate", datePicker_endDate.SelectedDate);
                            comm.Parameters.Add(end);
                            comm.CommandText = str_UpdateEnd;
                            comm.ExecuteNonQuery();
                        }

                        if (comm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("修改信息成功！", "成功", MessageBoxButton.OK);

                            string str_allStaff = "select * from 员工表";
                            comm.CommandText = str_allStaff;
                            SqlDataAdapter sda = new SqlDataAdapter(comm);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            dataGrid_workOutside.ItemsSource = dt.DefaultView;
                        }
                        else
                        {
                            MessageBox.Show("修改信息失败！", "失败", MessageBoxButton.OK);
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

                        string strSelect = "select * from 出差表 where 员工编号=@sno";
                        com.CommandText = strSelect;
                        Object obj1 = com.ExecuteScalar();
                        SqlDataAdapter sda = new SqlDataAdapter(com);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        dataGrid_workOutside.ItemsSource = dt.DefaultView;

                        //显示照片
                        string strSelect_photo = "select 照片 from 员工表 where 员工编号=@sno";
                        com.CommandText = strSelect_photo;
                        Object obj13 = com.ExecuteScalar();
                        photo.Source = new BitmapImage(new Uri(obj13.ToString(), UriKind.Absolute));
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
