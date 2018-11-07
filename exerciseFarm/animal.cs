using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace exerciseFarm
{
    class animal
    {
        public PictureBox picture_box;
        public PictureBox feed_box;
        private int hunger;
        public int HungerValue
        {
            set
            {
                if (value < 0)
                    hunger = 0;
                else if (value >= 100)
                    hunger = 100;
                else
                    hunger = value;
            }
            get
            {
                return hunger;
            }
        }
        private int mood;
        public int Mood {
            set {
                if (value < 0)
                    mood = 0;
                else if (value >= 100)
                    mood = 100;
                else
                    mood = value;
            }
            get {
                return mood;
            }
        }
        public int Level;
        public int FoodValue;
        public string Species;
        public string Name;
        public string feedingHabit;
        public animal(string _Name, string _Species, int _Level, int _HungerValue, int _Mood , PictureBox _pb, PictureBox _fb, string _feedingHabit)
        {
            HungerValue = _HungerValue;
            Mood = _Mood;
            Level = _Level;
            Species = _Species.Trim();
            Name = _Name.Trim();
            picture_box = _pb;
            feed_box = _fb;
            feedingHabit = _feedingHabit.Trim();
        }
    }
}
