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
using System.Data;
using System.Data.SqlClient;
using System.Threading;
using Microsoft.Win32;

namespace 人事管理系统.windows
{
    /// <summary>
    /// AddDept.xaml 的交互逻辑
    /// </summary>
    public partial class AddDept : Page
    {
        public AddDept()
        {
            InitializeComponent();

            /*combo_dept.Items.Add("人事部"); combo_dept.Items.Add("财务部"); combo_dept.Items.Add("销售部");
            combo_dept.Items.Add("生产部"); combo_dept.Items.Add("技术部"); combo_dept.Items.Add("采购部");
            combo_dept.Items.Add("后勤部"); combo_dept.Items.Add("基础设施部"); combo_dept.Items.Add("办公室");*/
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            textBox_DeptID.Text = "";
            textBox_Dept.Text = "";
            textBox_Name.Text = "";
            textBox_Sno.Text = "";
            photo.Source = null;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_Name.Text == "")
            {
                MessageBox.Show("部门名称不能为空！");
            }
            else  if (textBox_DeptID.Text == "")
            {
                MessageBox.Show("部门编号不能为空！");
            }

            else if (textBox_Sno.Text == "")
            {
                MessageBox.Show("负责人编号不能为空！");
            }
            else if (textBox_Name.Text == "")
            {
                MessageBox.Show("负责人姓名不能为空！");
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
                        string str_AddDept = "insert into 部门表 values(@deptno,@dept,@sname,@sno)";
                        SqlParameter sno = new SqlParameter("@sno", textBox_Sno.Text);
                        comm.Parameters.Add(sno);
                        SqlParameter name = new SqlParameter("@sname", textBox_Name.Text);
                        comm.Parameters.Add(name);
                        SqlParameter deptno = new SqlParameter("@deptno", textBox_DeptID.Text);
                        comm.Parameters.Add(deptno);
                        SqlParameter dept = new SqlParameter("@dept", textBox_Dept.Text);
                        comm.Parameters.Add(dept);
                        //防止编号重复
                        string s = "select 部门名称 from 部门表 where 部门编号=@deptno";
                        comm.CommandText = s;
                        if (comm.ExecuteScalar() != null)
                        {
                            MessageBox.Show("该部门已存在！请勿重复添加！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            comm.CommandText = str_AddDept;
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("添加部门成功！", "成功", MessageBoxButton.OK);

                                String strSelect_photo = "select 照片 from 员工表 where 员工编号=@sno";
                                comm.CommandText = strSelect_photo;
                                Object obj = comm.ExecuteScalar();
                                photo.Source = new BitmapImage(new Uri(obj.ToString(), UriKind.Absolute));

                                string str_allStaff = "select * from 部门表";
                                comm.CommandText = str_allStaff;
                                SqlDataAdapter sda = new SqlDataAdapter(comm);
                                DataTable dt = new DataTable();
                                sda.Fill(dt);
                                dataGrid_dept.ItemsSource = dt.DefaultView;
                            }
                            else
                            {
                                MessageBox.Show("添加部门失败！", "失败", MessageBoxButton.OK);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }       //添加的部门负责人要在员工表中存在

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_DeptID.Text == "")
            {
                MessageBox.Show("请指明部门编号进行修改！");
            }
            else if(textBox_Dept.Text==""&&textBox_Sno.Text=="")
            {
                MessageBox.Show("请输入修改信息！");
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

                        SqlParameter deptno = new SqlParameter("@deptno", textBox_DeptID.Text);
                        comm.Parameters.Add(deptno);
                        if (textBox_Dept.Text != "")
                        {
                            string str_UpdateDept = "update 部门表 set 部门名称=@dept where 部门编号=@deptno";
                            SqlParameter dept = new SqlParameter("@dept", textBox_Dept.Text);
                            comm.Parameters.Add(dept);
                            comm.CommandText = str_UpdateDept;
                            comm.ExecuteNonQuery();
                        }
                        if (textBox_Sno.Text != "")
                        {
                            string str_UpdateSno = "exec Pro_Dept_Update @deptno,@sno";
                            SqlParameter sno = new SqlParameter("@sno", textBox_Dept.Text);
                            comm.Parameters.Add(sno);
                            comm.CommandText = str_UpdateSno;
                            comm.ExecuteNonQuery();
                        }
                  
                        if (comm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("修改信息成功！", "成功", MessageBoxButton.OK);

                            string str_allStaff = "select * from 部门表";
                            comm.CommandText = str_allStaff;
                            SqlDataAdapter sda = new SqlDataAdapter(comm);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            dataGrid_dept.ItemsSource = dt.DefaultView;
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
            if (textBox_DeptID.Text == "")
            {
                MessageBox.Show("请指明部门编号进行查询！");
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

                        SqlParameter deptno = new SqlParameter("@deptno", textBox_DeptID.Text);
                        com.Parameters.Add(deptno);
                        
                        //显示姓名
                        string strSelect_name = "select 部门经理姓名 from 部门表 where 部门编号=@deptno";
                        com.CommandText = strSelect_name;
                        Object obj12 = com.ExecuteScalar();
                        textBox_Name.Text = obj12.ToString();
                        //显示部门经理编号
                        string strSelect_sno = "select 部门经理编号 from 部门表 where 部门编号=@deptno";
                        com.CommandText = strSelect_sno;
                        Object obj1 = com.ExecuteScalar();
                        textBox_Sno.Text = obj1.ToString();
                        
                        //显示所属部门名
                        string strSelect_dept = "select 部门名称 from 部门表 where 部门编号=@deptno";
                        com.CommandText = strSelect_dept;
                        Object obj9 = com.ExecuteScalar();
                        textBox_Dept.Text = obj9.ToString();
                        
                        //显示照片
                        string strSelect_photo = "select 照片 from 员工表 where 所属部门编号=@deptno and 职位='部门经理'";
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

        private void btnImage_Clcik(object sender, RoutedEventArgs e)
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
    }
}
