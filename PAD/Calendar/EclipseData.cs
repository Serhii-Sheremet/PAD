using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace PAD
{
    public class EclipseData
    {
        public DateTime Date { get; set; }
        public int EclipseId { get; set; }

        public List<EclipseData> ParseFile(string[] zList)
        {
            //24.03.2018 15:35:10 GMT     9°50'05"Gem Первая четверть
            // 6.07.2018  8:50:42 GMT+1  20°05'58"Psc Последняя четверть 
            //13.07.2018  3:47:52 GMT+1  26°34'30"Gem Новолуние          Частное Солнечное затмение
            //19.07.2018 20:52:14 GMT+1   2°58'44"Lib Первая четверть    
            //27.07.2018 21:20:21 GMT+1  10°38'07"Cap Полнолуние         Полное Лунное затмение
            // 4.08.2018 19:17:59 GMT+1  18°12'15"Ari Последняя четверть 
            //11.08.2018 10:57:44 GMT+1  24°34'53"Cnc Новолуние          Частное Солнечное затмение
            List<EclipseData> parsedList = new List<EclipseData>();
            for (int i = 0; i < zList.Length; i++)
            {
                var arr = zList[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                if (arr.Last().Equals("затмение"))
                {
                    if (arr[0].Length == 9)
                        arr[0] = "0" + arr[0];
                    if (arr[1].Length == 7)
                        arr[1] = "0" + arr[1];
                    var realdate = DateTime.ParseExact((arr[0] + " " + arr[1]), "d.MM.yyyy H:mm:ss", DateTimeFormatInfo.CurrentInfo);

                    if (arr[2].Length > 3)
                        realdate = realdate.AddHours(-1);

                    int zatType = -1;
                    if (arr[Array.IndexOf(arr, arr.Last()) - 1].Equals("Лунное"))
                        zatType = 1;
                    else
                        zatType = 2;
                    parsedList.Add(new EclipseData { Date = realdate, EclipseId = zatType });
                }
            }
            return parsedList;
        }

        public List<EclipseData> ParseUpdateFile(List<string> nList)
        {
            List<EclipseData> parsedList = new List<EclipseData>();
            foreach (string str in nList)
            {
                string temp = Utility.Decrypt(str, true);
                var arg = temp.Split(new char[] { '|' });
                DateTime date = DateTime.ParseExact(arg[0], "yyyy-MM-dd HH:mm:ss", DateTimeFormatInfo.CurrentInfo);
                parsedList.Add(new EclipseData
                {
                    Date = date,
                    EclipseId = Convert.ToInt32(arg[1])
                });
            }
            return parsedList;
        }

    }
}
