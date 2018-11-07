using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;

using System.Data.SqlClient;

namespace SportsRanch
{
    public partial class DB : Form
    {
        public DB()
        {
            InitializeComponent();
        }

        BindingManagerBase bm;

        public void DB_Load(object sender, EventArgs e)
        {
            SqlConnection cn = new SqlConnection();
            cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                "Integrated Security=True";
            DataSet ds = new DataSet();

            SqlDataAdapter daProduct = new SqlDataAdapter("SELECT * FROM ExerciseRecord", cn);
            daProduct.Fill(ds, "ExerciseRecord");

            //textBox6.DataBindings.Clear();
            //textBox7.DataBindings.Clear();
            //textBox8.DataBindings.Clear();
            //textBox9.DataBindings.Clear();
            //textBox10.DataBindings.Clear();
            //textBox1.DataBindings.Add("Text", ds, "ExerciseRecord.RecordId");
            //textBox2.DataBindings.Add("Text", ds, "ExerciseRecord.EndureTime");
            //textBox3.DataBindings.Add("Text", ds, "ExerciseRecord.ExerciseName");
            //textBox4.DataBindings.Add("Text", ds, "ExerciseRecord.Calories");
            //textBox5.DataBindings.Add("Text", ds, "ExerciseRecord.ExerciseGenre");
            //textBox6.DataBindings.Add("Text", ds, "ExerciseRecord.Year");
            //textBox7.DataBindings.Add("Text", ds, "ExerciseRecord.Month");
            //textBox8.DataBindings.Add("Text", ds, "ExerciseRecord.Date");
            //textBox9.DataBindings.Add("Text", ds, "ExerciseRecord.Hr");
            //textBox10.DataBindings.Add("Text", ds, "ExerciseRecord.Min");
            //bm = this.BindingContext[ds, "ExerciseRecord"];
            //hScrollBar1.SmallChange = 1;
            //hScrollBar1.LargeChange = 1;
            //hScrollBar1.Minimum = 0;
            //hScrollBar1.Maximum = bm.Count-1 ;
            //lblCount.Text = (bm.Position+1).ToString ()  +" / "+ bm.Count.ToString ();
            cn.Close();
        }

        private void hScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            bm.Position = hScrollBar1.Value;
            lblCount.Text = (bm.Position + 1).ToString() + " / " + bm.Count.ToString();
        }

        private void btnCls_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
        }

        void Edit(string sqlstr)
        {
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            cn.Close();
        }

        void DataBindingsClear()
        {
            textBox1.DataBindings.Clear();
            textBox2.DataBindings.Clear();
            textBox3.DataBindings.Clear();
            textBox4.DataBindings.Clear();
            textBox5.DataBindings.Clear();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            MessageBox.Show("INSERT INTO ExerciseRecord VALUES(" +
                textBox1.Text.Replace("'", "") + "," +
                textBox2.Text.Replace("'", "") + "," +
                textBox3.Text.Replace("'", "") + "," +
                textBox4.Text.Replace("'", "") + "," +
                textBox5.Text.Replace("'", "") + "," +
                textBox6.Text.Replace("'", "") + "," +
                textBox7.Text.Replace("'", "") + "," +
                textBox8.Text.Replace("'", "") + "," +
                textBox9.Text.Replace("'", "") + "," +
                textBox10.Text + ")");
            Edit("INSERT INTO ExerciseRecord VALUES(" +
                textBox1.Text.Replace("'", "") + "," +
                textBox2.Text.Replace("'", "") + "," +
                textBox3.Text.Replace("'", "") + "," +
                textBox4.Text.Replace("'", "") + "," +
                textBox5.Text.Replace("'", "") + "," +
                textBox6.Text.Replace("'", "") + "," +
                textBox7.Text.Replace("'", "") + "," +
                textBox8.Text.Replace("'", "") + "," +
                textBox9.Text.Replace("'", "") + "," +
                textBox10.Text + ")");
             DataBindingsClear();
             DB_Load(sender, e);  
        }

        public ArrayList getRecordId(int year, object sender, EventArgs e)
        {
            ArrayList IDs = new ArrayList();
            // Console.WriteLine("SELECT * FROM ExerciseRecord WHERE Year =" + year );
            string sqlstr = "SELECT * FROM ExerciseRecord WHERE Year =" + year;
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    IDs.Add(Convert.ToInt32(dataReader[0].ToString()));
                }
                //connection.Close();
                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // textBox11.Text = str;
            // DataBindingsClear();
            // DB_Load(sender, e);
            cn.Close();
            return IDs;
        }
        public ArrayList getRecordId(int year, int month, object sender, EventArgs e)
        {
            ArrayList IDs = new ArrayList();
            // Console.WriteLine("SELECT * FROM ExerciseRecord WHERE Year =" + year + "AND Month = " + month);
            string sqlstr = "SELECT * FROM ExerciseRecord WHERE Year =" + year + "AND Month = " + month ;
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    IDs.Add(Convert.ToInt32(dataReader[0].ToString()));
                }
                //connection.Close();
                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //textBox11.Text = str;
            DataBindingsClear();
            DB_Load(sender, e);
            cn.Close();
            return IDs;
        }
        public ArrayList getRecordId(int year, int month, int date, object sender, EventArgs e)
        {
            ArrayList IDs = new ArrayList();
            SqlConnection cn = new SqlConnection();
            // Console.WriteLine("SELECT * FROM ExerciseRecord WHERE Year =" + year + "AND Month = " + month + "AND Date = " + date);
            string sqlstr = "SELECT * FROM ExerciseRecord WHERE Year =" + year + "AND Month = " + month + "AND Date = " + date;
            try
            {
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();

                while (dataReader.Read())
                {
                    
                    IDs.Add(Convert.ToInt32(dataReader[0].ToString()));
                }
                dataReader.Close();
                //connection.Close();
                //textBox11.Text = str;
                cmd.ExecuteNonQuery();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DataBindingsClear();
            DB_Load(sender, e);
            cn.Close();
            return IDs;
        }
        public int getRecordTimeHr(int recordId, object sender, EventArgs e)
        {
            int Hr = -1;
            SqlConnection cn = new SqlConnection();
            //Int32.Parse(textBox1.Text);
            // Console.WriteLine("SELECT Hr FROM ExerciseRecord WHERE RecordId =" + recordId);
            string sqlstr = "SELECT Hr FROM ExerciseRecord WHERE RecordId =" + recordId;
            try
            {
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    Hr = (Convert.ToInt32(dataReader[0].ToString()));
                }


                //connection.Close();
                textBox11.Text = Hr +"";
                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //DataBindingsClear();
            //Form1_Load(sender, e);
            cn.Close();
            return Hr;
        }
        public int getRecordTimeMin(int recordId, object sender, EventArgs e)
        {
            int Min = -1;
            // Console.WriteLine("SELECT Min FROM ExerciseRecord WHERE RecordId =" + recordId);
            string sqlstr = "SELECT Min FROM ExerciseRecord WHERE RecordId =" + recordId;
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    Min = (Convert.ToInt32(dataReader[0].ToString()));
                }


                //connection.Close();
                textBox11.Text = Min + "";
                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //DataBindingsClear();
            //Form1_Load(sender, e);
            cn.Close();
            return Min;
        }
        public int getRecordDate(int recordId, object sender, EventArgs e)
        {
            int Date = -1;
            // Console.WriteLine("SELECT Date FROM ExerciseRecord WHERE RecordId =" + recordId);
            string sqlstr = "SELECT Date FROM ExerciseRecord WHERE RecordId =" + recordId;
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    Date = (Convert.ToInt32(dataReader[0].ToString()));
                }


                //connection.Close();
                textBox11.Text = Date + "";
                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //DataBindingsClear();
            //Form1_Load(sender, e);
            cn.Close();
            return Date;
        }
        public int getRecordYear(int recordId, object sender, EventArgs e)
        {
            int Year = -1;
            // Console.WriteLine("SELECT Year FROM ExerciseRecord WHERE RecordId =" + recordId);
            string sqlstr = "SELECT Year FROM ExerciseRecord WHERE RecordId =" + recordId;
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    Year = (Convert.ToInt32(dataReader[0].ToString()));
                }


                //connection.Close();
                textBox11.Text = Year + "";
                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //DataBindingsClear();
            //Form1_Load(sender, e);
            cn.Close();
            return Year;
        }
        public int getRecordMonth(int recordId, object sender, EventArgs e)
        {
            int Month = -1;
            // Console.WriteLine("SELECT Month FROM ExerciseRecord WHERE RecordId =" + recordId);
            string sqlstr = "SELECT Month FROM ExerciseRecord WHERE RecordId =" + recordId;
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    Month = (Convert.ToInt32(dataReader[0].ToString()));
                }


                //connection.Close();
                textBox11.Text = Month + "";
                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //DataBindingsClear();
            //Form1_Load(sender, e);
            cn.Close();
            return Month;
        }
        public float getRecordEndureTime(int recordId, object sender, EventArgs e)
        {
            // float EndureTime = float.Parse(textBox1.Text);
            // 如果回傳值 < 0 表示沒有拿到資料
            float EndureTime = -1;
            // Console.WriteLine("SELECT EndureTime FROM ExerciseRecord WHERE RecordId =" + recordId);
            string sqlstr = "SELECT EndureTime FROM ExerciseRecord WHERE RecordId =" + recordId;
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    // Bug fixed: Type of EndureTime is float.
                    EndureTime = (Convert.ToSingle(dataReader[0].ToString()));
                }

                //connection.Close();
                //textBox11.Text = EndureTime + "";
                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            // DataBindingsClear();
            // Form1_Load(sender, e);
            cn.Close();
            return EndureTime;
        }
        public string getRecordExerciseName(int recordId, object sender, EventArgs e)
        {
            string ExerciseName = null;
            // Console.WriteLine("SELECT ExerciseName FROM ExerciseRecord WHERE RecordId =" + recordId);
            string sqlstr = "SELECT ExerciseName FROM ExerciseRecord WHERE RecordId =" + recordId;
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    ExerciseName = (dataReader[0].ToString());
                }


                //connection.Close();
                //textBox11.Text = ExerciseName + "";
                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //DataBindingsClear();
            //Form1_Load(sender, e);
            cn.Close();
            return ExerciseName;
        }
        public float getRecordCalories(int recordId, object sender, EventArgs e)
        {
            float Calories = -1;
            // Console.WriteLine("SELECT  Calories FROM ExerciseRecord WHERE RecordId =" + recordId);
            string sqlstr = "SELECT  Calories FROM ExerciseRecord WHERE RecordId =" + recordId;
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    Calories = (Convert.ToSingle(dataReader[0].ToString()));
                }


                //connection.Close();
                textBox11.Text = Calories + "";
                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //DataBindingsClear();
            //Form1_Load(sender, e);
            cn.Close();
            return Calories;
        }
        public string getRecordExerciseGenre(int recordId, object sender, EventArgs e)
        {
            string ExerciseGenre = null;
            // Console.WriteLine("SELECT ExerciseGenre FROM ExerciseRecord WHERE RecordId =" + recordId);
            string sqlstr = "SELECT ExerciseGenre FROM ExerciseRecord WHERE RecordId =" + recordId;
            SqlConnection cn = new SqlConnection();
            try
            {
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    ExerciseGenre = (dataReader[0].ToString());
                }


                //connection.Close();
                textBox11.Text = ExerciseGenre;
                cmd.ExecuteNonQuery();
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            //DataBindingsClear();
            //DB_Load(sender, e);
            cn.Close();
            return ExerciseGenre;
        }

        // add
        public void addExerciseData(int RecordId, float EndureTime, string ExerciseName, float Calories, string ExerciseGenre, int Year, int Month, int Date, int Hr, int Min)
        {
            // Console.WriteLine("INSERT INTO ExerciseRecord VALUES(" +
                //RecordId + "," +
                //EndureTime + ", N'" +
                //ExerciseName + "', " +
                //Calories + ", N'" +
                //ExerciseGenre + "'," +
                //Year + "," +
                //Month + "," +
                //Date + "," +
                //Hr + "," +
                //Min + ")");
            Edit("INSERT INTO ExerciseRecord VALUES(" +
                RecordId + "," +
                EndureTime + ", N'" +
                ExerciseName + "', " +
                Calories + ", N'" +
                ExerciseGenre + "'," +
                Year + "," +
                Month + "," +
                Date + "," +
                Hr + "," +
                Min + ")");
        }

        // del
        public void deleteExerciseData(int recordId, object sender, EventArgs e)
        {
            Console.Write("Delete from ExerciseRecord WHERE RecordId = " + recordId
            );
            Edit("Delete from ExerciseRecord WHERE RecordId = " + recordId
            );
        }


        // **6/27 update

        public bool addExerciseName(string ExerciseName, float Calories, string ExerciseType, object sender, EventArgs e)
        {
        
            Edit("INSERT INTO ExerciseType VALUES(N'" +
                ExerciseName + "'," +
                Calories + ", N'" +
                ExerciseType + "')");
            return true;
        }

        public ArrayList getAllExerciseName(object sender, EventArgs e)
        {

            ArrayList myAL = new ArrayList();
            string sqlstr = "SELECT * FROM ExerciseType";
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    myAL.Add(dataReader[0].ToString());
                    myAL.Add(dataReader[1].ToString());
                    myAL.Add(dataReader[2].ToString());
                }

                foreach (Object obj in myAL)
                    textBox11.Text += (string)(obj) + "   ";

                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return myAL;
        }


        public ArrayList getAllAnimalName(object sender, EventArgs e)
        {
            ArrayList myAL = new ArrayList();
            string sqlstr = "SELECT AnimalName FROM ObtainedAnimals";
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    myAL.Add(dataReader[0].ToString());
                }


                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return myAL;
        }

        public int getAnimalHungerValue(string animalName)
        {
            int Hunger = 0;
            string sqlstr = "SELECT HungryValue FROM ObtainedAnimals WHERE animalName =" + "\'" + animalName + "\'";
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    Hunger = Int32.Parse(dataReader[0].ToString());
                    dataReader.Close();
                }
                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DataBindingsClear();
            return Hunger;
        }

        public string getAnimalSpecies(string animalName)
        {
            string FeedingHabit = "";
            string sqlstr = "SELECT Species FROM ObtainedAnimals WHERE animalName =" + "\'" + animalName + "\'";
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    FeedingHabit = (dataReader[0].ToString());
                    dataReader.Close();
                }

                dataReader.Close();
                cmd.ExecuteNonQuery();
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return FeedingHabit;
        }
        // Return: mood
        public int getAnimalMood(string animalName)
        {
            int Hunger = 0;
            string sqlstr = "SELECT Mood FROM ObtainedAnimals WHERE animalName =" + "\'" + animalName + "\'";
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    Hunger = Int32.Parse(dataReader[0].ToString());
                    dataReader.Close();
                }
                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return Hunger;
        }

        public int getAnimalLevel(string animalName)
        {
            int Hunger = 0;
            string sqlstr = "SELECT AnimalLevel FROM ObtainedAnimals WHERE animalName =" + "\'" + animalName + "\'";
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    Hunger = Int32.Parse(dataReader[0].ToString());
                    dataReader.Close();
                }


                dataReader.Close();
                cmd.ExecuteNonQuery();
                dataReader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            DataBindingsClear();
            return Hunger;
        }

        public ArrayList getSystemData(object sender, EventArgs e)
        {
            ArrayList myAL = new ArrayList();
            string sqlstr = "SELECT * FROM System";
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                while (dataReader.Read())
                {
                    myAL.Add(dataReader[0].ToString());
                    myAL.Add(dataReader[1].ToString());
                    myAL.Add(dataReader[2].ToString());
                    myAL.Add(dataReader[3].ToString());
                    myAL.Add(dataReader[4].ToString());
                    myAL.Add(dataReader[5].ToString());
                    myAL.Add(dataReader[6].ToString());
                    myAL.Add(dataReader[7].ToString());
                    myAL.Add(dataReader[8].ToString());
                    myAL.Add(dataReader[9].ToString());
                    myAL.Add(dataReader[10].ToString());

                }
                int l = 1;
                foreach (Object obj in myAL)
                {
                    textBox11.Text += l + ": " + (string)(obj) + " ";
                    l++;
                }
                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return myAL;
        }


        public void updateSystemDatumById(int userId, DateTime logoutTime, int grass, int meat,
             int money, int animalNumber, string username,
             float height, float weight, int ranchLevel,
             int recordNumber)
        {
            Edit("Delete from System WHERE UserId = " + userId
            );
            
            Edit("INSERT INTO System VALUES(" +
                userId + "," + "\'" +
                logoutTime.ToString("d") + "\'" + "," +
                grass + "," +
                meat + "," +
                money + "," +
                animalNumber + "," + "\'" +
                username.Replace("'", "") + "\'" + "," +
                height + "," +
                weight + "," +
                ranchLevel + "," +
                recordNumber + ")");
            DataBindingsClear();
        }

        public bool delExerciseByName(string exerciseName)
        {
            Edit("Delete from ExerciseType WHERE exerciseName = N'" + exerciseName + "'");
            return true;
        }

        public string getSpeciesFeedingHabit(string species)
        {
            string FeedingHabit = "";
            string sqlstr = "SELECT FeedingHabit FROM AnimalSpecies WHERE species =" + "\'" + species + "\'";
            try
            {
                SqlConnection cn = new SqlConnection();
                cn.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;" +
                    "AttachDbFilename=|DataDirectory|Exercise.mdf;" +
                    "Integrated Security=True";
                cn.Open();
                SqlCommand cmd = new SqlCommand(sqlstr, cn);
                SqlDataReader dataReader = cmd.ExecuteReader();
                if (dataReader.Read())
                {
                    FeedingHabit = (dataReader[0].ToString());
                    dataReader.Close();
                }

                dataReader.Close();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return FeedingHabit;
        }

        public bool addAnimal(string animalName, int hungryValue, int mood, string species, int animalLevel)
        {
      
            Edit("INSERT INTO ObtainedAnimals VALUES(" + "\'" +
               animalName + "\'" + "," +
               hungryValue + "," +
               mood + "," + "\'" +
               species + "\'" + "," +
               animalLevel +
               ")");
            return true;
        }

        public bool delAnimal(string animalName)
        {
            Edit("Delete from ObtainedAnimals WHERE AnimalName = " + "\'" + animalName + "\'"
            );
            return true;
        }

        // **












        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int RecordId = Int32.Parse(textBox1.Text);
            MessageBox.Show("Delete from ExerciseRecord WHERE RecordId = " + RecordId


            );
            Edit("Delete from ExerciseRecord WHERE RecordId = " + RecordId
            );
            MessageBox.Show("INSERT INTO ExerciseRecord VALUES(" +
                textBox1.Text.Replace("'", "") + "," +
                textBox2.Text.Replace("'", "") + "," +
                textBox3.Text.Replace("'", "") + "," +
                textBox4.Text.Replace("'", "") + "," +
                textBox5.Text.Replace("'", "") + "," +
                textBox6.Text.Replace("'", "") + "," +
                textBox7.Text.Replace("'", "") + "," +
                textBox8.Text.Replace("'", "") + "," +
                textBox9.Text.Replace("'", "") + "," +
                textBox10.Text + ")");
            Edit("INSERT INTO ExerciseRecord VALUES(" +
                textBox1.Text.Replace("'", "") + "," +
                textBox2.Text.Replace("'", "") + "," +
                textBox3.Text.Replace("'", "") + "," +
                textBox4.Text.Replace("'", "") + "," +
                textBox5.Text.Replace("'", "") + "," +
                textBox6.Text.Replace("'", "") + "," +
                textBox7.Text.Replace("'", "") + "," +
                textBox8.Text.Replace("'", "") + "," +
                textBox9.Text.Replace("'", "") + "," +
                textBox10.Text + ")");
            DataBindingsClear();
            DB_Load(sender, e);
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
                int RecordId = Int32.Parse(textBox1.Text);
                MessageBox.Show("Delete from ExerciseRecord WHERE RecordId = " + RecordId


                );
                Edit("Delete from ExerciseRecord WHERE RecordId = " + RecordId
                );
            DataBindingsClear();
            DB_Load(sender, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            getRecordId(Int32.Parse(textBox6.Text),sender, e);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            getRecordId(Int32.Parse(textBox6.Text), Int32.Parse(textBox7.Text), sender, e);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            getRecordId(Int32.Parse(textBox6.Text), Int32.Parse(textBox7.Text), Int32.Parse(textBox8.Text), sender, e);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            getRecordTimeHr(Int32.Parse(textBox1.Text), sender, e);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            getRecordTimeMin(Int32.Parse(textBox1.Text), sender, e);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            getRecordEndureTime(Int32.Parse(textBox1.Text), sender, e);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            getRecordExerciseName(Int32.Parse(textBox1.Text), sender, e);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            getRecordCalories(Int32.Parse(textBox1.Text), sender, e);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            getRecordExerciseGenre(Int32.Parse(textBox1.Text), sender, e);
        }
    }
}
