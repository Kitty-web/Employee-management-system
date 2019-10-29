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
    /// PersonnelTransfer.xaml 的交互逻辑
    /// </summary>
    public partial class PersonnelTransfer : Page
    {
        public PersonnelTransfer()
        {
            InitializeComponent();
            combo_oldDept.Items.Add("人事部"); combo_oldDept.Items.Add("财务部"); combo_oldDept.Items.Add("销售部");
            combo_oldDept.Items.Add("生产部"); combo_oldDept.Items.Add("技术部"); combo_oldDept.Items.Add("采购部");
            combo_oldDept.Items.Add("后勤部"); combo_oldDept.Items.Add("基础设施部"); combo_oldDept.Items.Add("办公室");
            combo_nowDept.Items.Add("人事部"); combo_nowDept.Items.Add("财务部"); combo_nowDept.Items.Add("销售部");
            combo_nowDept.Items.Add("生产部"); combo_nowDept.Items.Add("技术部"); combo_nowDept.Items.Add("采购部");
            combo_nowDept.Items.Add("后勤部"); combo_nowDept.Items.Add("基础设施部"); combo_nowDept.Items.Add("办公室");
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
            textBox_name.Text = "";
            textBox_nowState.Text = "";
            textBox_oldState.Text = "";
            textBox_sno.Text = "";
            combo_nowDept.SelectedItem = null;
            combo_oldDept.SelectedItem = null;
            datePicker_date.SelectedDate = null;
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
            else if (combo_oldDept.SelectedItem == null)
            {
                MessageBox.Show("原属部门不能为空！");
            }
            else if (combo_nowDept.SelectedItem == null)
            {
                MessageBox.Show("现属部门不能为空！");
            }
            else if (textBox_oldState.Text == "")
            {
                MessageBox.Show("原职位不能为空！");
            }
            else if (textBox_nowState.Text == "")
            {
                MessageBox.Show("现职位不能为空！");
            }
            else if (datePicker_date.SelectedDate == null)
            {
                MessageBox.Show("调动日期不能为空！");
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
                        string str_AddWorkOutSide = "insert into 人事调动表 values(@sno,@name,@oldDept,@nowDept,@oldState,@nowState,@date)";
                        SqlParameter sno = new SqlParameter("@sno", textBox_sno.Text);
                        comm.Parameters.Add(sno);
                        SqlParameter name = new SqlParameter("@name", textBox_name.Text);
                        comm.Parameters.Add(name);
                        SqlParameter oldDept = new SqlParameter("@oldDept",combo_oldDept.SelectedItem);
                        comm.Parameters.Add(oldDept);
                        SqlParameter nowDept = new SqlParameter("@nowDept", combo_nowDept.SelectedItem);
                        comm.Parameters.Add(nowDept);
                        SqlParameter oldState = new SqlParameter("@oldState", textBox_oldState.Text);
                        comm.Parameters.Add(oldState);
                        SqlParameter nowState = new SqlParameter("@nowState", textBox_nowState.Text);
                        comm.Parameters.Add(nowState);
                        SqlParameter date = new SqlParameter("@date", datePicker_date.SelectedDate);
                        comm.Parameters.Add(date);

                        comm.CommandText = str_AddWorkOutSide;
                        if (comm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("添加调动记录成功！", "成功", MessageBoxButton.OK);

                            string str_all = "select * from 人事调动表";
                            comm.CommandText = str_all;
                            SqlDataAdapter sda = new SqlDataAdapter(comm);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            dataGrid_personnel.ItemsSource = dt.DefaultView;

                            //显示照片
                            string strSelect_photo = "select 照片 from 员工表 where 员工编号=@sno";
                            comm.CommandText = strSelect_photo;
                            Object obj13 = comm.ExecuteScalar();
                            photo.Source = new BitmapImage(new Uri(obj13.ToString(), UriKind.Absolute));
                            
                            String s = "select 记录编号 from 人事调动表 where 员工编号=@sno and 变动日期=@date";
                            comm.CommandText = s;
                            Object obj = comm.ExecuteScalar();
                            textBlock_ID.Text = obj.ToString();

                            string str_UpdateState = "update 员工表 set 职位=@nowState where 员工编号=@sno";
                            comm.CommandText = str_UpdateState;
                            comm.ExecuteNonQuery();
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

                        SqlParameter sno = new SqlParameter("@sno", textBox_sno.Text);
                        com.Parameters.Add(sno);

                        string strSelect = "select * from 人事调动表 where 员工编号=@sno";
                        com.CommandText = strSelect;
                        Object obj1 = com.ExecuteScalar();
                        SqlDataAdapter sda = new SqlDataAdapter(com);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        dataGrid_personnel.ItemsSource = dt.DefaultView;

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
