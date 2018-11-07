using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Collections;
using System.Media;

namespace SportsRanch
{
    public partial class Form1 : Form
    {
        private bool mouse_down_in_form1 = false; // 判斷是否要移動視窗
        private flatButton.FlatButton active_navbar_btn = null; // 使用中的側邊按鈕
        private int calender_show_month = DateTime.Now.Month;
        private int calender_show_year = DateTime.Now.Year;
        private int calender_show_date = 0;

        // 
        private SoundPlayer player;

        private bool is_recordPage_show_all_id = true;
        private int recordPage_oldest_id = 0; // 目前 recordPage 顯示最前面的舊的一筆資料

        // TODO: 要改成從資料庫設定 record_cnts 初始值
        private int record_num = 0;
        private int userId = 0;
        private string user_name = "";
        private int meat = 0;
        private int grass = 0;
        private int money = 0;
        private float height = 0;
        private float weight = 0;
        private int ranch_level = 0;
        private DB dataBase;
        private ArrayList user_animals = new ArrayList();

        // TODO: 視窗顏色改用 member variables 設定
        public Form1()
        {
            InitializeComponent();
            navbar_btn_homepage.linkControl = home_page;
            navbar_btn_record.linkControl = record_page;
            navbar_btn_farm.linkControl = farm_page;
            //navbar_btn_medal.linkControl = medal_page;
            navbar_btn_calender.linkControl = calender_page;
            navbar_btn_setting.linkControl = setting_page;
            panel9.MouseWheel += new MouseEventHandler(record_MouseWheel);
            this.ShowInTaskbar = false;
            player = new SoundPlayer();
            player.SoundLocation = AppDomain.CurrentDomain.BaseDirectory + "\\Ranch.wav";         
             //farm_page.back

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dataBase = new DB();
            dataBase.DB_Load(sender, e);
            navbar_btn_homepage.PerformClick();

            // 抓下user資料
            ArrayList userdat = dataBase.getSystemData(null, null);
            if (userdat.Count == 11)
            {
                try
                {
                    grass = Int32.Parse("" + userdat[2]);
                    meat = Int32.Parse("" + userdat[3]);
                    money = Int32.Parse("" + userdat[4]);
                    user_name = (string)userdat[6];
                    height = float.Parse("" + userdat[7]);
                    weight = float.Parse("" + userdat[8]);
                    ranch_level = Int32.Parse("" + userdat[9]);
                    record_num = Int32.Parse("" + userdat[10]);
                }
                catch {

                }
                //Console.WriteLine(grass + " ");
                //Console.WriteLine(meat + " ");
                //Console.WriteLine(money + " ");
                //Console.WriteLine(user_name + " ");
                //Console.WriteLine(height + " ");

                game_timer1.Start();
            }

            // 更新動物資料
            ArrayList _animals = dataBase.getAllAnimalName(null, null);
            Console.WriteLine(_animals.Count + "隻");
            PictureBox[] pb = { animal_pb1, animal_pb2, animal_pb3 };
            PictureBox[] feedBox = { feedBox_pb1, feedBox_pb2, feedBox_pb3 };

            for (int i = 0; i < _animals.Count; i++)
            {
                int _level = dataBase.getAnimalLevel(_animals[i] + "");
                int _hungervalue = dataBase.getAnimalHungerValue(_animals[i] + "");
                int _mood = dataBase.getAnimalMood(_animals[i] + "");
                string _speices = dataBase.getAnimalSpecies(_animals[i] + "");
                string _feedHabits = dataBase.getSpeciesFeedingHabit(_speices.Trim() + "");
                //MessageBox.Show(dataBase.getSpeciesFeedingHabit(_speices.Trim() + "") + ", " + dataBase.getSpeciesFeedingHabit("Lion"));
                animal item = new animal(_animals[i] + "", _speices, _level, _hungervalue, _mood, pb[i], feedBox[i], _feedHabits);
                user_animals.Add(item);
                feedBox[i].Tag = item;
            }

            create_animals();
        }


        // 滑鼠移動Form、滑鼠關掉Form
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            uploadDatabase();
            uploadFarmDatabase();
            Application.Exit();
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouse_down_in_form1 = true;
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            mouse_down_in_form1 = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (mouse_down_in_form1)
            {
                this.SetDesktopLocation(MousePosition.X - 410, MousePosition.Y - 25);
            }
        }

        // 側邊欄位操作
        private void navbarBtn_Click(object sender, EventArgs e)
        {
            flatButton.FlatButton btn = (flatButton.FlatButton)sender;
            Panel panel = (Panel)btn.linkControl;


            // 其他畫面都關掉

            if (active_navbar_btn != null)
            {
                active_navbar_btn.themeColor = btn.themeColor;
                active_navbar_btn.linkControl.Enabled = false;
                active_navbar_btn.linkControl.Visible = false;
                active_navbar_btn.Enabled = true;
            }

            btn.themeColor = Color.FromArgb(166, 211, 179);

            if (!is_recordPage_show_all_id)
            {
                is_recordPage_show_all_id = true;
                record_page.Enabled = false;
                record_page.Visible = false;
            }

            // 改變btn的狀態
            btn.Enabled = false;
            btn.Cursor = Cursors.Arrow;
            active_navbar_btn = btn;
            panel.Enabled = true;
        }

        private void navbarPanel_enabled(object sender, EventArgs e) {
            Panel panel = (Panel)sender;
            if (panel.Enabled)
            {
                Console.WriteLine("enabled");
                panel.Visible = true;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            addRecord_panel.Enabled = false;
            addRecord_panel.Visible = false;
        }

        // 月曆
        private void calender_page_EnabledChanged(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            if (panel.Enabled)
            {
                calender_label_month.Text = calender_show_month + "月";
                calender_label_year.Text = calender_show_year + "年";

                // 找到本月1日是星期幾
                DateTime first_day = new DateTime(calender_show_year, calender_show_month, 1);
                Console.Write((int)first_day.DayOfWeek);
                int days = DateTime.DaysInMonth(calender_show_year, calender_show_month);
                int dayOfWeek = (int)first_day.DayOfWeek;


                for (int i = 0; i < 42; i++)
                {
                    flatButton.FlatButton btn = (flatButton.FlatButton)tableLayoutPanel1.GetControlFromPosition(i % 7, i / 7 + 1);
                    if (i < dayOfWeek || i > days + dayOfWeek - 1)
                    {
                        btn.themeColor = Color.FromArgb(160, Color.WhiteSmoke);
                        btn.Text = "";
                        btn.Enabled = false;
                    }
                    else
                    {
                        int day = (i - dayOfWeek + 1);
                        btn.userData = day + "";
                        float cals = 0;
                        ArrayList Ids = dataBase.getRecordId(calender_show_year, calender_show_month, day, sender, e); //抓資料數量進來

                        for (int j = 0; j < Ids.Count; j++) {
                            cals += dataBase.getRecordCalories((int)Ids[j], sender, e);
                        }

                        if (cals > 1000) {
                            btn.themeColor = Color.FromArgb(255, Color.LightCoral);
                        }
                        else if (cals > 500) {
                            btn.themeColor = Color.FromArgb(180, Color.LightCoral);
                        }
                        else if (cals > 0) {
                            btn.themeColor = Color.FromArgb(100, Color.LightCoral);
                        }
                        else {
                            btn.themeColor = Color.FromArgb(117, 205, 176);
                        }

                        btn.Text = day + "";
                        btn.Enabled = true;
                    }
                }
                panel.Visible = true;
            }
        }

        private void calender_edit_Click(object sender, EventArgs e)
        {
            calender_show_date = Int32.Parse(((flatButton.FlatButton)sender).userData);
            is_recordPage_show_all_id = false;
            if (active_navbar_btn != null)
            {
                // 將月曆頁面關閉
                active_navbar_btn.linkControl.Visible = false;
                active_navbar_btn.linkControl.Enabled = false;
                active_navbar_btn.Enabled = true;
            }

            record_page.Enabled = true;
        }

        private void addMonth_Click(object sender, EventArgs e)
        {
            if (calender_show_month < 12 && calender_show_year > 2015)
                calender_show_month++;
            else
            {
                calender_show_month = 1;
                calender_show_year++;
            }
            calender_page_EnabledChanged(calender_page, e);
        }

        private void reduceMonth_Click(object sender, EventArgs e)
        {
            if (calender_show_month > 1 && calender_show_year > 2015)
                calender_show_month--;
            else
            {
                calender_show_month = 12;
                calender_show_year--;
            }
            calender_page_EnabledChanged(calender_page, e);
        }



        // 紀錄
        private void record_page_EnabledChanged(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            if (panel.Enabled) {
                ArrayList Ids = new ArrayList();
                if (is_recordPage_show_all_id)
                {
                    recordPage_oldest_id = 0;
                    int cnts = 0;
                    for (int i = record_num; i > 0; i--)
                    {
                        Ids.Add(i);
                        recordPage_oldest_id = i;
                        cnts++;
                        if (cnts == 8)
                        {
                            break;
                        }
                    }
                } else
                {
                    Ids = dataBase.getRecordId(calender_show_year, calender_show_month, calender_show_date, sender, e);
                }

                tableLayoutPanel2.Controls.Clear();
                recordPage_Update(Ids, sender, e);
                panel.Visible = true;

            }
        }

        private void recordPage_Update(ArrayList Ids, object sender, EventArgs e)
        {
            string[,] data = new string[Ids.Count, 5];
            for (int i = 0; i < Ids.Count; i++)
            {
                int recordId = (int)Ids[i];
                data[i, 0] = recordId + "";
                data[i, 1] = (dataBase.getRecordEndureTime(recordId, sender, e) * 60) + "";
                data[i, 2] = dataBase.getRecordExerciseName(recordId, sender, e);
                data[i, 3] = dataBase.getRecordCalories(recordId, sender, e) + "";
                data[i, 4] = dataBase.getRecordYear(recordId, sender, e) + "/" + dataBase.getRecordMonth(recordId, sender, e) + "/" + dataBase.getRecordDate(recordId, sender, e);
            }
            recordTable_Load(data, tableLayoutPanel2);
        }

        private void recordTable_Load(string[,] data, TableLayoutPanel table)
        {
            //讀取資料
            for (int i = 0; i < data.GetLongLength(0); i++)
            {
                if (data[i, 2] == null) // 如果運動內容為null表示此資料不存在
                    continue;

                Panel record = new Panel();
                record.Margin = new Padding(10);
                record.Dock = DockStyle.Fill;
                record.AutoSize = true;
                record.BackColor = Color.FromArgb(117, 205, 176);

                Label date = new Label();
                Label cost = new Label();
                Label content = new Label();
                Label cal = new Label();

                date.BackColor = cost.BackColor = content.BackColor = cal.BackColor = Color.FromArgb(117, 205, 176);

                date.AutoSize = true;
                cost.AutoSize = true;
                content.AutoSize = true;
                cal.AutoSize = true;

                flatButton.FlatButton edit = new flatButton.FlatButton();
                edit.Text = "編輯";
                edit.userData = data[i, 0] + "";
                edit.themeColor = Color.LightCoral;
                edit.Click += new EventHandler(edit_page_Show);

                Font txt_font = new Font(new FontFamily("微軟正黑體"), 17f, FontStyle.Regular, GraphicsUnit.Pixel);
                date.Font = txt_font;
                cost.Font = txt_font;
                content.Font = txt_font;
                cal.Font = txt_font;

                Color txt_color = Color.FromArgb(51, 40, 52);
                date.ForeColor = txt_color;
                cost.ForeColor = txt_color;
                content.ForeColor = txt_color;
                cal.ForeColor = txt_color;

                date.Dock = DockStyle.Bottom;
                cost.Dock = DockStyle.Bottom;
                content.Dock = DockStyle.Bottom;
                cal.Dock = DockStyle.Bottom;
                edit.Dock = DockStyle.Right;


                date.Text = data[i, 4] + "\t id:" + data[i, 0];
                cost.Text = data[i, 1] + "分鐘";
                content.Text = data[i, 2];
                cal.Text = data[i, 3] + "大卡";

                record.Controls.Add(date);
                record.Controls.Add(cost);
                record.Controls.Add(content);
                record.Controls.Add(cal);
                record.Controls.Add(edit);
                table.Controls.Add(record);

            }
        }

        private void record_Scroll(object sender, ScrollEventArgs e)
        {
            record_ScrollToBottom();
        }

        private void record_MouseWheel(object sender, MouseEventArgs e)
        {
            record_ScrollToBottom();
        }
        private void record_ScrollToBottom() {
            if (is_recordPage_show_all_id && (panel9.PreferredSize.Height - panel9.Height + panel9.AutoScrollPosition.Y) < 0)
            {
                // TODO: Load new record.
                if (recordPage_oldest_id > 0)
                {
                    int cnts = 0;
                    ArrayList Ids = new ArrayList();
                    for (int i = recordPage_oldest_id - 1; i > 0; i--)
                    {
                        Ids.Add(i);
                        recordPage_oldest_id = i;
                        cnts++;
                        if (cnts == 8)
                            break;

                    }
                    recordPage_Update(Ids, null, null);
                }

            }
        }
        //儲存,修改,刪除資料
        private void record_save_Click(object sender, EventArgs e)
        {
            Console.WriteLine(addRecord_type.SelectedText);
            addRecord_panel.Enabled = false;
            addRecord_panel.Visible = false;

            float cost = (float)addRecord_cost.Value / 60f;

            dataBase.addExerciseData(
                record_num++,
                (float)addRecord_cost.Value / 60f,
                addRecord_type.GetItemText(addRecord_type.SelectedItem),
                (float)addRecord_kcal.Value,
                "testing",
                addRecord_date.Value.Year,
                addRecord_date.Value.Month,
                addRecord_date.Value.Day,
                addRecord_time.Value.Hour,
                addRecord_time.Value.Minute);
            MessageBox.Show("已新增一筆資料");
            record_page.Enabled = false;
            record_page.Enabled = true;

            // 更新database
            uploadDatabase();
            // 
            meat += 3;
            grass += 3;
        }

        private void record_edit_Click(object sender, EventArgs e)
        {
            addRecord_panel.Enabled = false;
            addRecord_panel.Visible = false;
            dataBase.deleteExerciseData(Int32.Parse(record_btn_edit.userData), sender, e);

            dataBase.addExerciseData(
                Int32.Parse(record_btn_edit.userData),
                (float)addRecord_cost.Value / 60f,
                addRecord_type.GetItemText(addRecord_type.SelectedItem),
                (float)addRecord_kcal.Value,
                "testing",
                addRecord_date.Value.Year,
                addRecord_date.Value.Month,
                addRecord_date.Value.Day,
                addRecord_time.Value.Hour,
                addRecord_time.Value.Minute);
            record_page.Enabled = false;
            record_page.Enabled = true;
        }

        private void record_del_Click(object sender, EventArgs e)
        {
            addRecord_panel.Enabled = false;
            addRecord_panel.Visible = false;
            dataBase.deleteExerciseData(Int32.Parse(record_btn_edit.userData), sender, e);
            record_page.Enabled = false;
            record_page.Enabled = true;
        }

        // 填寫資料欄位
        private void edit_page_Show(object sender, EventArgs e)
        {
            flatButton.FlatButton panel = (flatButton.FlatButton)sender;
            Console.WriteLine(">>" + panel.userData);
            int recordId = Int32.Parse(panel.userData);

            record_btn_edit.Visible = true;
            record_btn_del.Visible = true;
            record_btn_save.Visible = false;
            // 將recordId放到欲操作的userData中
            record_btn_edit.userData = panel.userData;
            record_btn_del.userData = panel.userData;

            // TODO: 將資料庫的資料拉進來
            addRecord_date.Value = new DateTime(dataBase.getRecordYear(recordId, sender, e), dataBase.getRecordMonth(recordId, sender, e), dataBase.getRecordDate(recordId, sender, e));
            addRecord_time.Value = new DateTime(2018, 1, 1, dataBase.getRecordTimeHr(recordId, sender, e), dataBase.getRecordTimeMin(recordId, sender, e), 0);
            addRecord_cost.Value = (decimal)dataBase.getRecordEndureTime(recordId, sender, e) * 60;
            addRecord_type.SelectedItem = dataBase.getRecordExerciseName(recordId, sender, e);
            update_exersiceType();

            addRecord_kcal.Value = (decimal)dataBase.getRecordCalories(recordId, sender, e);
            addRecord_panel.Enabled = true;

            addRecord_panel.BringToFront();
        }

        private void add_page_show(object sender, EventArgs e)
        {

            record_btn_edit.Visible = false;
            record_btn_del.Visible = false;
            record_btn_save.Visible = true;

            addRecord_panel.Enabled = true;
            addRecord_date.Value = DateTime.Now;
            addRecord_time.Value = DateTime.Now;
            addRecord_cost.Value = 0;
            addRecord_type.SelectedIndex = 0;
            update_exersiceType();
            addRecord_kcal.Value = 0;

            addRecord_panel.BringToFront();
        }

        private void addRecord_type_TextChanged(object sender, EventArgs e)
        {
            
            if( addRecord_type.GetItemText(addRecord_type.SelectedItem) == "新增(+)")
            {
                MessageBox.Show("add type");
                addExersiceType_panel.Visible = true;
                addExersiceType_panel.Enabled = true;
                addExersiceType_panel.BringToFront();

            }
        }

        // 首頁
        private void home_page_EnabledChanged(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            if (panel.Enabled)
            {
                label3.Text = "年運動狀況";
                homePage_year_label.Text = calender_show_year + "年";
                chart1.Series["Series1"].Points.Clear();
                chart1.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "MMMM";
                for (int i = 1; i < 13; i++) {
                    ArrayList Ids = dataBase.getRecordId(calender_show_year, i, sender, e);
                    float cals = 0;
                    for (int j = 0; j < Ids.Count; j++)
                    {
                        cals += dataBase.getRecordCalories((int)Ids[j], sender, e);
                    }
                    cals += 100;
                    chart1.Series["Series1"].Points.AddXY(new DateTime(calender_show_year, i, 1), cals);
                }

                chart1.Visible = true;
                chart2.Visible = false;
                panel.Visible = true;
            }
        }

        private void homepage_next_btn_Click(object sender, EventArgs e)
        {
            label3.Text = "週運動狀況";
            homePage_year_label.Text = "這週";
            chart2.Series["Series1"].Points.Clear();
            chart2.ChartAreas["ChartArea1"].AxisX.LabelStyle.Format = "dddd";
            int date = DateTime.Now.Day;


            for (int i = 0; i < 7; i++)
            {
                ArrayList Ids = dataBase.getRecordId(DateTime.Now.Year, DateTime.Now.Month, date - i, sender, e);
                float cals = 0;
                for (int j = 0; j < Ids.Count; j++)
                {
                    cals += dataBase.getRecordCalories((int)Ids[j], sender, e);
                }
                cals += 50;
                chart2.Series["Series1"].Points.AddXY(new DateTime(DateTime.Now.Year, DateTime.Now.Month, date - i), cals);
            }
            chart1.Visible = false;
            chart2.Visible = true;
        }

        private void homepage_back_btn_Click(object sender, EventArgs e)
        {
            home_page.Enabled = false;
            home_page.Enabled = true;
        }

        // 農場
        private void create_animals()
        {
            PictureBox[] pb = { animal_pb1, animal_pb2, animal_pb3 };
            PictureBox[] feedBox = { feedBox_pb1, feedBox_pb2, feedBox_pb3 };
            Label[] name = { animal_name1, animal_name2, animal_name3 };
            Label[] level = { animal_lv1, animal_lv2, animal_lv3 };
            flat_progressBar.flat_ProgressBar[] mood = { animal_mood1, animal_mood2, animal_mood3 };
            flat_progressBar.flat_ProgressBar[] hunger_value = { animal_hungerValue1, animal_hungerValue2, animal_hungerValue3 };
            Panel[] pl = { animal_status1, animal_status2, animal_status3 };
            ImageList[] animal_pics = { imageList1, imageList1 };



            Console.WriteLine(user_animals.Count + "隻");

            for (int i = 0; i < user_animals.Count; i++)
            {
                animal _animal = (animal)user_animals[i];
                pb[i].Image = animal_pics[0].Images[0]; // 看物種給list
                pb[i].SizeMode = PictureBoxSizeMode.Zoom;
                pb[i].Size = new Size(150, 142);
                pb[i].Tag = pl[i];
                pb[i].MouseEnter += new EventHandler(this.change);
                pb[i].MouseLeave += new EventHandler(this.dischange);

                name[i].Text = _animal.Name;
                mood[i].Value = _animal.Mood;
                hunger_value[i].Value = _animal.HungerValue;
                level[i].Text = "LV" + _animal.Level;

                feedBox[i].Image = imageList2.Images[0];
                feedBox[i].Size = new Size(150, 77);
                feedBox[i].SizeMode = PictureBoxSizeMode.Zoom;
            }
            //animal_timer1.Start();
        }

        private void update_farm()
        {
            PictureBox[] pb = { animal_pb1, animal_pb2, animal_pb3 };
            Label[] name = { animal_name1, animal_name2, animal_name3 };
            Label[] level = { animal_lv1, animal_lv2, animal_lv3 };
            flat_progressBar.flat_ProgressBar[] mood = { animal_mood1, animal_mood2, animal_mood3 };
            flat_progressBar.flat_ProgressBar[] hunger_value = { animal_hungerValue1, animal_hungerValue2, animal_hungerValue3 };

            grass_num.Text = grass + " 份";
            meat_num.Text = meat + " 份";

            for (int i = 0; i < user_animals.Count; i++)
            {
                animal _animal = (animal)user_animals[i];
                pb[i].Size = new Size(150, 142);

                name[i].Text = _animal.Name;
                mood[i].Value = _animal.Mood;
                hunger_value[i].Value = _animal.HungerValue;
                level[i].Text = "LV" + _animal.Level;
            }
            //animal_timer1.Start();
        }

        private void farm_page_enabled(object sender, EventArgs e)
        {
            Panel panel = (Panel)sender;
            if (panel.Enabled)
            {
                player.PlayLooping();
                animal_timer1.Start();
                // 更新 使用者資訊
                grass_num.Text =  grass + " 份";
                meat_num.Text =  meat + " 份";
                
                // 畫animal
                
                panel.Visible = true;
            }
            else {
                animal_timer1.Stop();
                player.Stop();
            }
        }


        int feedMode = 0;

        private void change(object sender, EventArgs e)
        {
            ((PictureBox)sender).Image = imageList1.Images[20];
            ((Panel)((PictureBox)sender).Tag).Visible = true;
            animal_timer1.Stop();
        }

        private void dischange(object sender, EventArgs e)
        {
            ((PictureBox)sender).Image = imageList1.Images[movestep];
            animal_timer1.Start();
            ((Panel)((PictureBox)sender).Tag).Visible = false;
        }

        int movestep = 0;
        int movestep1 = 5;
        int movestep2 = 8;
        private void animal_timer1_Tick(object sender, EventArgs e)
        {
            
            animal_pb1.Image = (movestep < 20) ? imageList1.Images[movestep++] : imageList1.Images[movestep = 0];
            animal_pb2.Image = (movestep1 < 20) ? imageList1.Images[movestep1++] : imageList1.Images[movestep1 = 0];
            animal_pb3.Image = (movestep2 < 20) ? imageList1.Images[movestep2++] : imageList1.Images[movestep2 = 0];
        }

        private void feed_meat_btn_Click(object sender, EventArgs e)
        {
            feedMode = 1;
        }

        private void feed_grass_btn_Click(object sender, EventArgs e)
        {
            feedMode = 2;
        }

        private void animal_pb_Click(object sender, EventArgs e)
        {

        }

        private void feedBox_pb1_Click(object sender, EventArgs e)
        {
            PictureBox panel = (PictureBox)sender;
            animal ani = (animal)panel.Tag;
            if(ani.HungerValue == 0) {
                MessageBox.Show(ani.Name + "：飽了!");


            }else if(meat <= 0)
            {

            }
            else if (feedMode == 1 && ani.feedingHabit == "Carnivorous")
            {
                // 餵食
                MessageBox.Show(ani.Name + "：好ㄘ~" );
                ani.HungerValue = 0;
                meat--;
                //update
                update_farm();
            }
            else if (feedMode == 1 && ani.feedingHabit != "Carnivorous")
            {
                MessageBox.Show(ani.Name + "：覺得好難吃QQ" );
                ani.Mood -= 20;
                meat--;

                //update
                update_farm();
            }
            else if (feedMode == 2 && ani.feedingHabit == "Herbivorous")
            {
                // 餵食
                MessageBox.Show(ani.Name + "：好ㄘ~" );
                ani.HungerValue = 0;
                grass--;

                //update
                update_farm();
            }
            else if (feedMode == 2 && ani.feedingHabit != "Herbivorous")
            {
                // 餵食
                MessageBox.Show(ani.Name + ":覺得好難吃QQ" );
                ani.Mood -= 20;
                grass--;

                //update
                update_farm();
            }
        }

        int food_value = 0;

        private void feedBox_mouseEnter(object sender, EventArgs e)
        {
            switch (feedMode)
            {
                case 0:
                    //((PictureBox)sender).Image = imageList1.Images[16];
                    break;
                case 1:
                    if(meat>0)
                        ((PictureBox)sender).Image = imageList2.Images[8];
                    break;
                case 2:
                    if (grass > 0)
                        ((PictureBox)sender).Image = imageList2.Images[4];
                    break;
            }

        }
        private void feedBox_mouseLeave(object sender, EventArgs e)
        {
            Console.WriteLine("feedmode:"+ feedMode);
            switch (feedMode)
            {
                case 0:
                    //((PictureBox)sender).Image = imageList1.Images[16];
                    break;
                case 1:
                    ((PictureBox)sender).Image = imageList2.Images[food_value];
                    break;
                case 2:
                    ((PictureBox)sender).Image =  imageList2.Images[food_value];
                    break;
            }
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            addExersiceType_panel.Visible = false;
            addExersiceType_panel.Enabled = false;

        }



        //新增資料
        private void flatButton7_Click(object sender, EventArgs e)
        {
            if(dataBase.addExerciseName(addExersizeName_textBox.Text, (float)addExersizeName_UpDown.Value, "", null, null))
            {
                MessageBox.Show("新的運動類型新增好了!");
                addExersiceType_panel.Enabled = false;
                addExersiceType_panel.Visible = false;
                update_exersiceType();
            }
                
            //dataBase.addExerciseName("test", 69f, "", null, null);
        }

        // 更新運動資料: 1. 下拉選單的項目
        private void update_exersiceType()
        {
            // 更新下拉選單的項目
            addRecord_type.Items.Clear();
            ArrayList names = dataBase.getAllExerciseName(null, null);
            for (int i = 0; i < names.Count/3; i++)
            {
                addRecord_type.Items.Add(((string)names[i * 3]).Trim());
            }
            addRecord_type.SelectedIndex = 0;

            addRecord_type.Items.Add("新增(+)");

        }

        // Setting
        private void setting_page_EnabledChange(object sender, EventArgs e)
        {
            // 將所有type拉進來
            Panel panel = (Panel)sender;
            ArrayList names = dataBase.getAllExerciseName(null, null);
            exersiceType_table.Controls.Clear();

            for (int i = 0; i < names.Count / 3; i++)
            {
                Console.WriteLine((names[i * 3] + "").Trim() + " " + names[i * 3 + 1] + " " + names[i * 3 + 2]);
                Label name = new Label();
                Label txt = new Label();
                NumericUpDown cal = new NumericUpDown();
                flatButton.FlatButton edit_btn = new flatButton.FlatButton();
                Font txt_font = new Font(new FontFamily("微軟正黑體"), 17f, FontStyle.Regular, GraphicsUnit.Pixel);

                name.AutoSize = false;
                name.Size = new Size(50, 20);
                txt.AutoSize = false;
                txt.Size = new Size(100, 20);
                cal.Size = new Size(80, 20);

                name.Font = txt_font;
                txt.Font = txt_font;
                name.Text = (names[i * 3] + "").Trim();
                txt.Text = "kcal/min";
                cal.Maximum = 50000;
                cal.Value = int.Parse((string)names[i * 3 + 1]);
                edit_btn.Text = "刪除";
                edit_btn.Tag = name.Text;

                edit_btn.Click += new System.EventHandler(del_exersiceType);

                exersiceType_table.Controls.Add(name);
                exersiceType_table.Controls.Add(cal);
                exersiceType_table.Controls.Add(txt);
                exersiceType_table.Controls.Add(edit_btn);
            }
            panel.Visible = true;
            Console.WriteLine(panel.Name);
        }

        private void del_exersiceType(object sender, EventArgs e)
        {
            flatButton.FlatButton panel = (flatButton.FlatButton)sender;
            dataBase.delExerciseByName((string)panel.Tag);
            MessageBox.Show("已經把刪除" + (string)panel1.Tag + "了~");
            setting_page.Enabled = false;
            setting_page.Enabled = true;

        }

        // 更新系統資料
        private void uploadDatabase()
        {
            dataBase.updateSystemDatumById(userId, DateTime.Now, grass, meat, money, user_animals.Count, user_name, height, weight, ranch_level, record_num);
        }

        private void uploadFarmDatabase()
        {
            for (int i = 0; i < user_animals.Count; i++)
            {
                animal ani = (animal)user_animals[i];
                dataBase.delAnimal(ani.Name);
                dataBase.addAnimal(ani.Name, ani.HungerValue, ani.Mood, ani.Species, ani.Level);
            }
        }

        private void game_timer1_Tick(object sender, EventArgs e)
        {
            add_hunger();
            foreach(animal ani in user_animals)
            {
                ani.HungerValue += 3;
            }
        }

        private void add_hunger()
        {
            update_farm();
        }

        // SQL
        //void Edit(string sqlstr)
        //{
        //    try
        //    {
        //        SqlConnection cn = new SqlConnection();
        //        cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
        //            "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
        //            "Integrated Security=True";
        //        cn.Open();
        //        SqlCommand cmd = new SqlCommand(sqlstr, cn);
        //        cmd.ExecuteNonQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

        //private void sendRecordToSQLDB(int RecordId, int EndureTime, string ExerciseName, int Calories, string ExerciseGenre, int Year, int Month, int Date, int Hr, int Min)
        //{
        //    Console.WriteLine("INSERT INTO ExerciseRecord VALUES(" +
        //        RecordId + "," +
        //        EndureTime + ", N'" +
        //        ExerciseName + "', " +
        //        Calories + ", N'" +
        //        ExerciseGenre + "'," +
        //        Year + "," +
        //        Month + "," +
        //        Date + "," +
        //        Hr + "," +
        //        Min + ")");
        //    Edit("INSERT INTO ExerciseRecord VALUES(" +
        //        RecordId + "," +
        //        EndureTime + ", N'" +
        //        ExerciseName + "', " +
        //        Calories + ", N'" +
        //        ExerciseGenre + "'," +
        //        Year + "," +
        //        Month + "," +
        //        Date + "," +
        //        Hr + "," +
        //        Min + ")");
        //}

    }
}
