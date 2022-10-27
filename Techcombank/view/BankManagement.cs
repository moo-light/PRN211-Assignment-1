using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Techcombank.controller;
using Techcombank.dto;

namespace Techcombank.view
{
    public class BankManagement
    { 
        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            string database = @"C:\Users\Admin\OneDrive\Máy tính\techcombank.txt";

            string[] menu = {
                    "Thêm chi nhánh"
                    ,"Tìm khách hàng"
                    ,"Gửi tiền & rút tiền"
                    ,"Lịch sử giao dịch của chi nhánh"
                    ,"Khách hàng ưu tú theo từng chi nhánh"
                    ,"Liệt kê tổng số dư Khách Hàng"
                    ,"Khách Hàng có giao dịch nhiều nhất"
            };
            QuanLiChiNhanh teckcombank = new QuanLiChiNhanh(database);
            Console.WriteLine("Chào Mừng các bạn đến với Teckcombank");
            bool cont = false;
            int choice = 0;
            do
            {
                choice = MyTool.getMenuChoice(menu);
                switch (choice)
                {
                    case 1:teckcombank.AddChiNhanh(); break;
                    case 2:teckcombank.SearchKhachHang(); break;
                    case 3:teckcombank.AddGiaoDich(); break;
                    case 4:teckcombank.PrintLSGiaoDich(); break;
                    case 5:teckcombank.PrintKhachHangUuTu(); break;
                    case 6:teckcombank.PrintLietKeTongSoDu(); break;
                    case 7:teckcombank.PrintKhachHangNoiTroi(); break;
                    default:
                        Console.WriteLine("Tạm Biệt");
                        teckcombank.WriteToFile(database);
                        cont = true;
                        break;

                }
            } while (!cont);

            //sr.Close();
            //sw.Close();
        }
    }
}
