using ser_ver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace курсач
{
    static class Search
    {
        public static (string, Boolean) LoginIn(List<User> users, string loginUser, string passwordUser)
        {
            string message = "";
            bool exist = false;
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Login == loginUser)
                {
                    if (users[i].Password == passwordUser)
                    {
                        exist = true;
                        break;
                    }
                    else
                    {
                        exist = false;
                        message = "Неверный пароль";
                        break;
                    }

                }
                else
                {
                    exist = false;
                    message = "Неверный логин";
                }

            }
            if (loginUser == "")
                message = "Введите логин";
            else if (passwordUser == "")
                message = "Введите пароль";
            return (message, exist);
        }
        public static Boolean Repetitions(List<User> users, string loginUser)
        {
            bool repetition = false;
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Login == loginUser)
                    repetition = true;
            }
            return repetition;
        }
        public static User Profile(List<User> users, string loginUser)
        {
            int x = -1;
            for (int i = 0; i < users.Count; i++)
            {
                if (users[i].Login == loginUser)
                {
                    x = i;
                    break;
                }

            }
            return users[x];
        }
        public static (Boolean, string) CorrectDate(string[] data1)
        {
            bool corect = true;
            string message = "";
            DateTime dt = DateTime.Today;
            string[] confirmData = dt.ToString().Split(" ");
            string[] dmy = confirmData[0].ToString().Split(".");
            if (Int32.Parse(data1[2]) == Int32.Parse(dmy[2]) | Int32.Parse(data1[2]) == Int32.Parse(dmy[2]) + 1)
            {
                if ((Int32.Parse(data1[1]) == Int32.Parse(dmy[1]) & Int32.Parse(data1[2]) != 2024) | (Int32.Parse(data1[1]) == 1))
                {
                    if (Int32.Parse(data1[0]) >= Int32.Parse(dmy[0]) | (Int32.Parse(data1[0]) < Int32.Parse(dmy[0]) & (Int32.Parse(data1[1]) != Int32.Parse(dmy[1]))))
                    {

                    }
                    else 
                    {
                        corect = false;
                        message = "Введена некорректная дата. Выберете другую дату.";
                    }
                }
                else
                {
                    corect = false;
                    message = "Введена некорректная дата. Выберете другую дату.";
                }
            }
            else
            {
                corect = false;
                message = "Введена некорректная дата. Выберете другую дату.";
            }
            return (corect, message);
        }
    }
}
