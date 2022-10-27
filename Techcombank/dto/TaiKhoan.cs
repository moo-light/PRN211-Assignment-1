using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Techcombank.controller;

namespace Techcombank.dto
{
    public class TaiKhoan :IComparable<TaiKhoan>
    {
        public const string STK_PATTERN = "([0-9]{4}([ ])?){4}";
        private readonly string SEPERATOR ="|";
        private string _soTaiKhoan;
        private double _soDu;
        private List<TTGiaoDich> _ttGiaoDichList;

        public TaiKhoan()
        {
            TTGiaoDichList = new List<TTGiaoDich>();
        }

        public TaiKhoan(double soDu, string soTaiKhoan)
        {
            SoDu = soDu;
            SoTaiKhoan = soTaiKhoan;
            TTGiaoDichList = new List<TTGiaoDich>();

        }

        public TaiKhoan(double soDu, string soTaiKhoan, List<TTGiaoDich> tTGiaoDichList)
        {
            SoDu = soDu;
            SoTaiKhoan = soTaiKhoan;
            TTGiaoDichList = tTGiaoDichList;
        }

        public TaiKhoan(string line)
        {
            string[] parts = line.Split(SEPERATOR);
            this.SoTaiKhoan = parts[0].Trim();
            this.SoDu = double.Parse(parts[1].Replace("VND","").Trim());
            this.TTGiaoDichList = new();
        }

        public double SoDu { get => _soDu; set => _soDu = value; }
        public string SoTaiKhoan
        {
            get
            {
                return _soTaiKhoan;
            }

            set => _soTaiKhoan = value;
        }
        public List<TTGiaoDich> TTGiaoDichList { get => _ttGiaoDichList; set => _ttGiaoDichList = value; }
        internal void addGiaoDich(QuanLiChiNhanh quanLi)
        {
            int? choice = null;
            bool cont = false;
            TTGiaoDich tTGiaoDich = null;


            //get Giao Dich Type
            do
            {
                char loaiGD;
                string[] menu = { "Gửi Tiền", "Rút Tiền" };
                choice = MyTool.getMenuChoice(menu);
                switch (choice)
                {
                    case 1: loaiGD = 'D'; break;
                    case 2: loaiGD = 'W'; break;
                    default:loaiGD = 'E'; break;
                }
                if (loaiGD == 'E') break;// 'E' Thoat Chuong trinh
                //so tien giao dich
                double soTienGD = MyTool.readInt($"Nhập Số Tiền Cần " + (loaiGD == 'D' ? "Gửi" : "Rút"), 0, null);
                while (this.SoDu - soTienGD < 0)
                {
                    Console.WriteLine("Dấu Hiệu Nghèo: Bạn ko đủ tiền!");
                    Console.WriteLine("Số Dư = "+SoDu);
                    if (loaiGD == 'D' && MyTool.readBool("Nạp Thêm?"))
                    {
                        napTien();
                    }
                    else break;
                }
                //'G' cần tìm thêm 1 số tài khoản
                bool isGui = false; 
                while(loaiGD == 'D'&& !isGui)
                {
                    string soTaiKhoan = MyTool.readPattern("Nhập Số Tài Khoản Cần Gửi", TaiKhoan.STK_PATTERN);
                    soTaiKhoan = formatSTK(soTaiKhoan);
                    TaiKhoan taiKhoanNhan =  quanLi.SearchTaiKhoanByID(soTaiKhoan);
                    if(soTaiKhoan == this.SoTaiKhoan)
                    {
                        if (!MyTool.readBool("Tài khoản đó là tài khoàn của bạn! Nhập Lại?"))
                            break;
                    }
                    if(taiKhoanNhan == null)
                    {
                        if (!MyTool.readBool("Không tìm thấy tài khoản! Nhập Lại?"))
                            break;
                    }
                        taiKhoanNhan.SoDu += soTienGD;
                        isGui = true;
                }
                if (isGui == false&& loaiGD=='D') break;
                this.SoDu -= soTienGD;
                //new ttGiao dich
                DateTime ngayGD = new(DateTime.Now.Ticks);
                tTGiaoDich = new(ngayGD, soTienGD, loaiGD);
                TTGiaoDichList.Prepend(tTGiaoDich);
                Console.WriteLine($"{menu[(int)(choice - 1)]} Thành Công!");
                Console.WriteLine(this.ToSimplifiedString());
            } while (MyTool.readBool("Tiếp Tục Giao Dịch?"));
            Console.WriteLine("---------------------------------------------");
            Console.WriteLine("Kết thúc Giao Dịch");
        }

        

        internal void napTien()
        {
            int soTien = MyTool.readInt("Số Tiền Nạp Thêm", 0, null);
            this.SoDu += soTien;
            Console.WriteLine("Nạp Tiền Thành Công");
            Console.WriteLine($"{soTien:f}VND");
        }

        public static string formatSTK(string soTaiKhoan)
        {
            soTaiKhoan = soTaiKhoan.Replace(" ", "");
            var stk = Regex.Matches(soTaiKhoan, "\\d{4}");
            soTaiKhoan = $"{stk[0]} {stk[1]} {stk[2]} {stk[3]}";
            return soTaiKhoan;
        }
        private string? ToSimplifiedString()
        {
            return $"{SoTaiKhoan} | {SoDu:f}VND | {TTGiaoDichList.Count}";
        }
        public override string? ToString()
        {
            StringBuilder result = new($"{SoTaiKhoan} | {SoDu:f}VND | {TTGiaoDichList.Count}");
            if (TTGiaoDichList.Count > 0)
                foreach (TTGiaoDich ttdg in TTGiaoDichList)
                    result.Append($"\n{ttdg}");
            return result.ToString() ?? null;
        }

        public int CompareTo(TaiKhoan? other)
        {
            return this.SoDu.CompareTo(other.SoDu);
        }
    }
}
