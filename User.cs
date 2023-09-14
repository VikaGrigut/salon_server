using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ser_ver
{
    [Serializable]
    public class User
    {
        [Range(1, 1000)]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }
        public int Age { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Login { get; set; }
        [Required]
        [RegularExpression(@"^\+375\d{2}\d{3}\d{4}$")]
        public string NumberPhone { get; set; }
        public List<Service> History { get; set; }
        public List<Service> Services { get; set; }
        public int Balance { get; set; } = 100;

        public User() { }

        public (Boolean, List<ValidationResult>) Validation()
        {
            bool correct;
            var context = new ValidationContext(this);
            var result = new List<ValidationResult>();
            if (!Validator.TryValidateObject(this, context, result, true))
            {
                correct = false;
            }
            else
            {
                correct = true;
            }
            return (correct, result);
        }
        public void IdCount(List<User> users)
        {
            this.Id = users.Count + 1;

        }
        public void FromServToHistory()
        {
            DateTime dt = DateTime.Today;
            string[] confirmData = dt.ToString().Split(" ");
            string[] dmy = confirmData[0].ToString().Split(".");
            string[] data;
            List<Service> history = new List<Service>();
            if (this.History == null)
            {

            }
            else
                history = this.History;
            for (int i = 0; i < this.Services.Count; i++)
            {
                data = DataShifr(this.Services[i].Date);
                if (Int32.Parse(data[2]) == Int32.Parse(dmy[2]))
                {
                    if (Int32.Parse(data[1]) == Int32.Parse(dmy[1]))
                    {
                        if (Int32.Parse(data[0]) == Int32.Parse(dmy[0]))
                        {

                        }
                        else if (Int32.Parse(data[0]) < Int32.Parse(dmy[0]))
                        {
                            history.Add(this.Services[i]);
                            this.Services.RemoveAt(i);
                            i--;
                        }
                    }
                    else if (Int32.Parse(data[1]) < Int32.Parse(dmy[1]))
                    {
                        history.Add(this.Services[i]);
                        this.Services.RemoveAt(i);
                        i--;
                    }
                }
                else if (Int32.Parse(data[2]) < Int32.Parse(dmy[2]))
                {
                    history.Add(this.Services[i]);
                    this.Services.RemoveAt(i);
                    i--;
                }
            }
            this.History = history;
        }
        public static string[] DataShifr(string data)
        {
            string[] newData = data.Split(" ");
            switch (newData[1])
            {
                case "января":
                    newData[1] = "1";
                    break;
                case "февраля":
                    newData[1] = "2";
                    break;
                case "марта":
                    newData[1] = "3";
                    break;
                case "апреля":
                    newData[1] = "4";
                    break;
                case "мая":
                    newData[1] = "5";
                    break;
                case "июня":
                    newData[1] = "6";
                    break;
                case "июля":
                    newData[1] = "7";
                    break;
                case "августа":
                    newData[1] = "8";
                    break;
                case "сентября":
                    newData[1] = "9";
                    break;
                case "октября":
                    newData[1] = "10";
                    break;
                case "ноября":
                    newData[1] = "11";
                    break;
                case "декабря":
                    newData[1] = "12";
                    break;
            }
            return newData;
        }
    }
}
