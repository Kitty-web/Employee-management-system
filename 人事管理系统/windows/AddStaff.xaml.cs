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
    /// AddStaff.xaml 的交互逻辑
    /// </summary>
    public partial class AddStaff : Page
    {
        public AddStaff()
        {
            InitializeComponent();

            combo_marry.Items.Add("已婚"); combo_marry.Items.Add("未婚"); combo_marry.Items.Add("离异");
            combo_edu.Items.Add("初中"); combo_edu.Items.Add("高中"); combo_edu.Items.Add("大专");
            combo_edu.Items.Add("本科"); combo_edu.Items.Add("硕士"); combo_edu.Items.Add("博士");
            combo_pro.Items.Add("中共党员"); combo_pro.Items.Add("中共预备党员"); combo_pro.Items.Add("群众");
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
            textBox_Acount.Text = "";
            textBox_ID.Text = "";
            textBox_Name.Text = "";
            textBox_Password.Text = "";
            textBox_Sno.Text = "";
            textBox_State.Text = "";
            textBox_Tele.Text = "";
            textBox_User.Text = "";
            combo_dept.SelectedItem = null;
            combo_edu.SelectedItem = null;
            combo_marry.SelectedItem = null;
            combo_pro.SelectedItem = null;
            datePicker_birth.SelectedDate = null;
            datePicker_hiredate.SelectedDate = null;
            photo.Source = null;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_Sno.Text == "")
            {
                MessageBox.Show("员工编号不能为空！");
            }
            else if (textBox_ID.Text == "")
            {
                MessageBox.Show("身份证号不能为空！");
            }
            else if (textBox_Name.Text == "")
            {
                MessageBox.Show("员工姓名不能为空！");
            }
            else if (textBox_Tele.Text == "")
            {
                MessageBox.Show("员工电话不能为空！");
            }
            else if (textBox_User.Text == "")
            {
                MessageBox.Show("员工账号不能为空！");
            }
            else if (textBox_Password.Text == "")
            {
                MessageBox.Show("账号密码不能为空！");
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
                        string str_AddStaff = "insert into 员工表 values(@sno,@name,@ID,@birth,@hiredate,@state,@tele,@edu,@pro,@marry,@acount,@deptno,@dept,@photo,@user,@password)";
                        SqlParameter sno = new SqlParameter("@sno", textBox_Sno.Text);
                        comm.Parameters.Add(sno);
                        SqlParameter name = new SqlParameter("@name", textBox_Name.Text);
                        comm.Parameters.Add(name);
                        SqlParameter tele = new SqlParameter("@tele", textBox_Tele.Text);
                        comm.Parameters.Add(tele);
                        SqlParameter ID = new SqlParameter("@ID", textBox_ID.Text);
                        comm.Parameters.Add(ID);
                        SqlParameter state = new SqlParameter("@state", textBox_State.Text);
                        comm.Parameters.Add(state);
                        SqlParameter acount = new SqlParameter("@acount", textBox_Acount.Text);
                        comm.Parameters.Add(acount);

                        SqlParameter marry = new SqlParameter("@marry", combo_marry.SelectedItem);
                        comm.Parameters.Add(marry);
                        SqlParameter pro = new SqlParameter("@pro", combo_pro.SelectedItem);
                        comm.Parameters.Add(pro);
                        SqlParameter deptno = new SqlParameter("@deptno", "RS");
                        comm.Parameters.Add(deptno);
                        SqlParameter dept = new SqlParameter("@dept", combo_dept.SelectedItem);
                        comm.Parameters.Add(dept);
                        SqlParameter edu = new SqlParameter("@edu", combo_edu.SelectedItem);
                        comm.Parameters.Add(edu);

                        SqlParameter birth = new SqlParameter("@birth", datePicker_birth.SelectedDate);
                        comm.Parameters.Add(birth); 
                        SqlParameter hire = new SqlParameter("@hiredate", datePicker_hiredate.SelectedDate);
                        comm.Parameters.Add(hire);

                        SqlParameter user = new SqlParameter("@user", textBox_User.Text);
                        comm.Parameters.Add(user);
                        SqlParameter pass = new SqlParameter("@password", textBox_Password.Text);
                        comm.Parameters.Add(pass);
                        if (photo.Source.ToString() == "")
                        {
                            SqlParameter photo1 = new SqlParameter("@photo", "");
                            comm.Parameters.Add(photo1);
                        }
                        else
                        {
                            SqlParameter photo1 = new SqlParameter("@photo", photo.Source.ToString());
                            comm.Parameters.Add(photo1);
                        }
                        //防止账号重复
                        string s = "select 员工姓名 from 员工表 where 登录账号=@user";
                        comm.CommandText = s;
                        if (comm.ExecuteScalar() != null)
                        {
                            MessageBox.Show("该账号已存在！请更换！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                        else
                        {
                            comm.CommandText = str_AddStaff;
                            if (comm.ExecuteNonQuery() > 0)
                            {
                                MessageBox.Show("添加员工成功！", "成功", MessageBoxButton.OK);

                                string str_allStaff = "select * from 员工表";
                                comm.CommandText = str_allStaff;
                                SqlDataAdapter sda = new SqlDataAdapter(comm);
                                DataTable dt = new DataTable();
                                sda.Fill(dt);
                                dataGrid_Staff.ItemsSource = dt.DefaultView;
                            }
                            else
                            {
                                MessageBox.Show("添加员工失败！", "失败", MessageBoxButton.OK);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }           //必须全部填入信息？

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_Sno.Text == "")
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
                        
                        SqlParameter sno = new SqlParameter("@sno", textBox_Sno.Text);
                        comm.Parameters.Add(sno);
                        if (textBox_ID.Text != "")
                        {
                            string str_UpdateID = "update 员工表 set 身份证号=@ID where 员工编号=@sno";
                            SqlParameter id = new SqlParameter("@ID", textBox_ID.Text);
                            comm.Parameters.Add(id);
                            comm.CommandText = str_UpdateID;
                            comm.ExecuteNonQuery();
                        }
                        if (textBox_Tele.Text != "")
                        {
                            string str_UpdateTele = "update 员工表 set 电话=@tele where 员工编号=@sno";
                            SqlParameter tele = new SqlParameter("@tele", textBox_Tele.Text);
                            comm.Parameters.Add(tele);
                            comm.CommandText = str_UpdateTele;
                            comm.ExecuteNonQuery();
                        }
                        if (textBox_User.Text != "")
                        {
                            string str_UpdateUser = "update 员工表 set 登录账号=@user where 员工编号=@sno";
                            SqlParameter user = new SqlParameter("@user", textBox_User.Text);
                            comm.Parameters.Add(user);
                            comm.CommandText = str_UpdateUser;
                            comm.ExecuteNonQuery();
                        }
                        if (textBox_State.Text != "")
                        {
                            string str_UpdateState = "update 员工表 set 职位=@state where 员工编号=@sno";
                            SqlParameter state = new SqlParameter("@state", textBox_State.Text);
                            comm.Parameters.Add(state);
                            comm.CommandText = str_UpdateState;
                            comm.ExecuteNonQuery();
                        }
                        if (!combo_marry.SelectedItem.ToString().Equals(""))
                        {
                            string str_UpdateState = "update 员工表 set 婚姻状况=@marry where 员工编号=@sno";
                            SqlParameter state = new SqlParameter("@marry", combo_marry.SelectedItem);
                            comm.Parameters.Add(state);
                            comm.CommandText = str_UpdateState;
                            comm.ExecuteNonQuery();
                        }
                        if (!combo_edu.SelectedItem.ToString().Equals(""))
                        {
                            string str_UpdateEdu = "update 员工表 set 学历=@edu where 员工编号=@sno";
                            SqlParameter edu = new SqlParameter("@edu", combo_edu.SelectedItem);
                            comm.Parameters.Add(edu);
                            comm.CommandText = str_UpdateEdu;
                            comm.ExecuteNonQuery();
                        }
                        
                        if (!combo_pro.SelectedItem.ToString().Equals(""))
                        {
                            string str_UpdatePro = "update 员工表 set 政治面貌=@pro where 员工编号=@sno";
                            SqlParameter pro = new SqlParameter("@pro", combo_pro.SelectedItem);
                            comm.Parameters.Add(pro);
                            comm.CommandText = str_UpdatePro;
                            comm.ExecuteNonQuery();
                        }
                        if (!combo_dept.SelectedItem.ToString().Equals(""))
                        {
                            string str_UpdateDept = "update 员工表 set 所属部门名称=@dept where 员工编号=@sno";
                            SqlParameter dept = new SqlParameter("@dept", combo_dept.SelectedItem);
                            comm.Parameters.Add(dept);
                            comm.CommandText = str_UpdateDept;
                            comm.ExecuteNonQuery();
                        }
                        if (photo.Source != null)
                        {
                            string str_UpdatePhoto = "update 员工表 set 照片=@photo where 员工编号=@sno";
                            SqlParameter photo1 = new SqlParameter("@photo", photo.Source.ToString());
                            comm.Parameters.Add(photo1);
                            comm.CommandText = str_UpdatePhoto;
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
                            dataGrid_Staff.ItemsSource = dt.DefaultView;
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
        }        //必须全部填入信息？

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (textBox_Sno.Text == "")
            {
                MessageBox.Show("请指明员工编号进行删除！");
            }
            else if (MessageBox.Show("确认删除？", "提示", MessageBoxButton.OKCancel, MessageBoxImage.Question) == MessageBoxResult.OK)
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

                        string str = "exec Pro_Staff_Delete @sno";
                        SqlParameter sno = new SqlParameter("@sno", textBox_Sno.Text);
                        comm.Parameters.Add(sno);
                        comm.CommandText = str;
                        if (comm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("删除成功！", "成功", MessageBoxButton.OK);
                        }
                        else
                        {
                            MessageBox.Show("删除失败！", "失败", MessageBoxButton.OK);
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
                        //显示身份证号
                        string strSelect_ID = "select 身份证号 from 员工表 where 员工编号=@sno";
                    
                        SqlParameter sno = new SqlParameter("@sno", textBox_Sno.Text);       
                        com.Parameters.Add(sno);
                        com.CommandText = strSelect_ID;
                        Object obj1 = com.ExecuteScalar();
                        textBox_ID.Text = obj1.ToString();
                        //显示姓名
                        string strSelect_name = "select 员工姓名 from 员工表 where 员工编号=@sno";
                        com.CommandText = strSelect_name;
                        Object obj12 = com.ExecuteScalar();
                        textBox_Name.Text = obj12.ToString();
                        //显示电话
                        string strSelect_tele = "select 电话 from 员工表 where 员工编号=@sno";
                        com.CommandText = strSelect_tele;
                        Object obj2 = com.ExecuteScalar();
                        textBox_Tele.Text = obj2.ToString();
                        //显示职位
                        string strSelect_state = "select 职位 from 员工表 where 员工编号=@sno";
                        com.CommandText = strSelect_state;
                        Object obj3 = com.ExecuteScalar();
                        textBox_State.Text = obj3.ToString();
                        //显示银行账户
                        string strSelect_acount = "select 银行账户 from 员工表 where 员工编号=@sno";
                        com.CommandText = strSelect_acount;
                        Object obj4 = com.ExecuteScalar();
                        textBox_Acount.Text = obj4.ToString();
                        //显示登录账号
                        string strSelect_user = "select 登录账号 from 员工表 where 员工编号=@sno";
                        com.CommandText = strSelect_user;
                        Object obj5 = com.ExecuteScalar();
                        textBox_User.Text = obj5.ToString();
                        //显示学历
                        string strSelect_edu = "select 学历 from 员工表 where 员工编号=@sno";
                        com.CommandText = strSelect_edu;
                        Object obj6 = com.ExecuteScalar();
                        combo_edu.SelectedItem = obj6.ToString();
                        //显示政治面貌
                        string strSelect_pro = "select 政治面貌 from 员工表 where 员工编号=@sno";
                        com.CommandText = strSelect_pro;
                        Object obj7 = com.ExecuteScalar();
                        combo_pro.SelectedItem = obj7.ToString();
                        //显示婚姻状况
                        string strSelect_marry = "select 婚姻状况 from 员工表 where 员工编号=@sno";
                        com.CommandText = strSelect_marry;
                        Object obj8 = com.ExecuteScalar();
                        combo_marry.SelectedItem = obj8.ToString();
                        //显示所属部门名
                        string strSelect_dept = "select 所属部门名称 from 员工表 where 员工编号=@sno";
                        com.CommandText = strSelect_dept;
                        Object obj9 = com.ExecuteScalar();
                        combo_dept.SelectedItem = obj9.ToString();
                        //显示出生日期
                        string strSelect_birth = "select 出生日期 from 员工表 where 员工编号=@sno";
                        com.CommandText = strSelect_birth;
                        Object obj10 = com.ExecuteScalar();
                        datePicker_birth.SelectedDate = Convert.ToDateTime(obj10);
                        //显示入职日期
                        string strSelect_hiredate = "select 入职日期 from 员工表 where 员工编号=@sno";
                        com.CommandText = strSelect_hiredate;
                        Object obj11 = com.ExecuteScalar();
                        datePicker_hiredate.SelectedDate = Convert.ToDateTime(obj11);
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
    }
}
