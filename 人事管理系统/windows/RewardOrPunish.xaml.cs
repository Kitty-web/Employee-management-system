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
    /// RewardOrPunish.xaml 的交互逻辑
    /// </summary>
    public partial class RewardOrPunish : Page
    {
        public RewardOrPunish()
        {
            InitializeComponent();
            combo_rewardType.Items.Add("奖励"); combo_rewardType.Items.Add("惩罚");

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

        private void btnClear_Cliak(object sender, RoutedEventArgs e)
        {
            textBlock_ID.Text = "";
            textBox_name.Text = "";
            textBox_punishMoney.Text = "";
            textBox_rewardMoney.Text = "";
            textBox_sno.Text = "";
            textBox_reason.Text = "";
            combo_dept.SelectedItem = null;
            combo_rewardType.SelectedItem = null;
            datePicker_reward.SelectedDate = null;
            photo.Source = null;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (datePicker_reward.SelectedDate == null)
            {
                MessageBox.Show("必须填写奖惩日期！");
            }
            else if (textBox_sno.Text == "")
            {
                MessageBox.Show("员工编号不能为空！");
            }
            else if (textBox_name.Text == "")
            {
                MessageBox.Show("员工姓名不能为空！");
            }
            else if (combo_rewardType.SelectedItem == null)
            {
                MessageBox.Show("请选择奖惩类型");
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
                        string str_AddCheckOn = "insert into 奖惩表 values(@sno,@name,@type,@reward,@punish,@date,@reason)";
                        SqlParameter sno = new SqlParameter("@sno", textBox_sno.Text);
                        comm.Parameters.Add(sno);

                        //显示照片
                        string strSelect_photo = "select 照片 from 员工表 where 员工编号=@sno";
                        comm.CommandText = strSelect_photo;
                        Object obj13 = comm.ExecuteScalar();
                        photo.Source = new BitmapImage(new Uri(obj13.ToString(), UriKind.Absolute));

                        SqlParameter name = new SqlParameter("@name", textBox_name.Text);
                        comm.Parameters.Add(name);
                        SqlParameter date = new SqlParameter("@date", datePicker_reward.SelectedDate);
                        comm.Parameters.Add(date);
                        SqlParameter type = new SqlParameter("@type", combo_rewardType.SelectedItem);
                        comm.Parameters.Add(type);

                        SqlParameter reason = new SqlParameter("@reason", textBox_reason.Text);
                        comm.Parameters.Add(reason);

                            SqlParameter reward = new SqlParameter("@reward", textBox_rewardMoney.Text);
                            comm.Parameters.Add(reward);
                       
                            SqlParameter punish = new SqlParameter("@punish", textBox_punishMoney.Text);
                            comm.Parameters.Add(punish);
                        

                        comm.CommandText = str_AddCheckOn;
                        if (comm.ExecuteNonQuery() > 0)
                        {
                            MessageBox.Show("添加完成！", "成功", MessageBoxButton.OK);

                            String s = "select 记录编号 from 奖惩表 where 员工编号=@sno and 奖惩日期=@date";
                            comm.CommandText = s;
                            Object obj = comm.ExecuteScalar();
                            textBlock_ID.Text = obj.ToString();

                            string str_all = "select * from 奖惩表";
                            comm.CommandText = str_all;
                            SqlDataAdapter sda = new SqlDataAdapter(comm);
                            DataTable dt = new DataTable();
                            sda.Fill(dt);
                            dataGrid_reward.ItemsSource = dt.DefaultView;
                        }
                        else
                        {
                            MessageBox.Show("奖惩记录添加失败！", "失败", MessageBoxButton.OK);
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

                        String strSelect_CheckOn = "select * from 奖惩表 where 员工编号=@sno";
                        SqlParameter sno = new SqlParameter("@sno", textBox_sno.Text);
                        com.Parameters.Add(sno);
                        com.CommandText = strSelect_CheckOn;
                        SqlDataAdapter sda = new SqlDataAdapter(com);
                        DataTable dt = new DataTable();
                        sda.Fill(dt);
                        dataGrid_reward.ItemsSource = dt.DefaultView;

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
