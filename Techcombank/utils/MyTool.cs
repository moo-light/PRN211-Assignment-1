using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public class MyTool
{
    public static bool validStr(string str, string regex)
    {
        return Regex.Match(str, regex).Success;
    }
    public static bool validPassword(string str, int minLen)
    {
        if (str.Length <= minLen) return false;
        return Regex.Match(str, ".*[a-zA-Z]+.*").Success//At Least 1 Character
               && Regex.Match(str, ".*[\\d]+.*").Success // At Least 1 DIgit
               && Regex.Match(str, "[\\W]").Success; //At least 1 Spec char
    }
    //Date Format: yyyy/MM/dd,MM/dd/yyyy , dd/MM/yyyy, ...
    public static DateTime parseDate(string dateStr, string dateFormat)
    {
        return DateTime.ParseExact(dateStr, dateFormat, new CultureInfo("en-US"));//tim hieu sau
    }

    public static string DateToStr(DateTime date, string dateFormat)
    {
        return date.ToString(dateFormat);
    }
    public static bool parseBool(string boolStr)
    {
        char c = boolStr.Trim().ToUpper()[0];
        return c.Equals('1') || c.Equals('Y') || c.Equals('T');
    }
    public static string readNonBlank(string msg)
    {
        String input = "";
        bool valid;
        do
        {
            Console.Write($"{msg}: ");
            input = Console.ReadLine();
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("try again!");
            }
        } while (string.IsNullOrEmpty(input));
        return input;
    }
    public static string readPattern(string msg, string pattern)
    {
        String? input;
        bool valid;
        do
        {
            Console.Write($"{msg} [{pattern}]:");
            input = Console.ReadLine();
            if (!validStr(input, pattern))
            {
                Console.WriteLine("Nhập Lại!");
            }
        } while (!validStr(input, pattern));
        return input;
    }
    public static int readInt(string msg,int? min,int? max)
    {
        int result = 0;
        if (min == null) min = int.MinValue;
        if (max == null) max = int.MaxValue;
        bool cont = false;
        do
        {
            try
            {
                Console.Write($"{msg}: ");
                result = int.Parse(Console.ReadLine());
                if(result >= min && result <= max)
                cont = true;
                else { 
                Console.WriteLine("Nhập Lại!");
                }
            }
            catch (Exception e){
            Console.WriteLine("Nhập Lại!");
            }

        } while (!cont);
        return result;
    }
    public static bool readBool(string msg)
    {
        String input;
        bool cont = false ;
        do
        {

            Console.Write($"{msg} [1/0,Y/N,T/F]: ");
            input = Console.ReadLine();
            try
            {
                cont = validStr("[10YNTF]", input.ToUpper());
            }
            catch (Exception e) { }
            if(!cont)
            Console.WriteLine("Nhập Lại!");
        } while (!cont);
        return parseBool(input);
    }
    //method read line from file
    public static List<string> readLineFromFile(string filename)
    {
        List<string> list = new List<string>();
        if (!File.Exists(filename))
        {
            File.Create(filename); 
            return null;
        }
        try
        {
            // tao instance cua StreamReader de doc mot file.
            // lenh using cung duoc su dung de dong StreamReader.
            using (StreamReader sr = new StreamReader(filename))
            {   
                string? line;


                // doc va hien thi cac dong trong file cho toi
                // khi tien toi cuoi file. 
                while ((line = sr.ReadLine()) != null)
                {
                    list.Add(line);
                }
                sr.Close();
            }
        }
        catch (Exception e)
        {
            // thong bao loi.
            Console.WriteLine("Khong the doc du lieu tu file da cho: ");
            Console.WriteLine(e.Message);
        }
        return list;
    }
    // ham nay chap nhan 2 parameter la filename va list
    public static void writeFile(string filename, List<string> list)
    {
        using (StreamWriter sw = new StreamWriter(filename))
        {

            foreach (string s in list)
            {
                sw.Write(s);
            }
        }
        //inra textfile.txt ko phai dealers.txt haizzz
    }
    public static int getMenuChoiceStrict(object[] menu)
    {
        Console.WriteLine("--------------------------------------------------");
        for (int i = 0; i < menu.Length; i++)
        {
            Console.WriteLine($"{i + 1}. {menu[i].ToString()}");
        }
        Console.WriteLine("--------------------------------------------------");
        do
        {
            try
            {
                return readInt("Chọn 1 số", 1, menu.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine("Nhập Lại!");
            }
        } while (true);
    }
    public static int getMenuChoice(object[] menu)
    {
        Console.WriteLine("--------------------------------------------------");
        for (int i = 0; i < menu.Length; i++)
        {
            Console.WriteLine($"{i+1}. {menu[i].ToString()}");
        }
        Console.WriteLine("Số Khác- Thoát Chương Trình");
        Console.WriteLine("--------------------------------------------------");
        do
        {
            try
            {
                return readInt("Chọn 1 số", 1, null);
            }
            catch (Exception e)
            {
                Console.WriteLine("Nhập Lại!");
            }
        } while (true);
    }
}
