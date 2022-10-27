using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Schema;
using Techcombank.dto;

namespace Techcombank.controller
{
    public class QuanLiChiNhanh: List<ChiNhanh>
    {
        public QuanLiChiNhanh()
        {
        }
        public QuanLiChiNhanh(string filename)
        {
            List<string> lines = MyTool.readLineFromFile(filename);
            ChiNhanh? chiNhanh = null;
            KhachHang? khachHang = null;
            TaiKhoan? taiKhoan = null;
            TTGiaoDich? tTGiaoDich = null;
            if(lines != null)
            foreach (string line in lines)
            {

                if (Regex.IsMatch(line, ChiNhanh.ID_PATTERN))
                { 
                    chiNhanh = new(line);
                    this.Add(chiNhanh);
                }
                if (MyTool.validStr(line,KhachHang.ID_PATTERN))
                { 
                    khachHang = new(line);
                    chiNhanh?.KhachHangList.Add(khachHang);
                }
                if (Regex.IsMatch(line, TaiKhoan.STK_PATTERN))
                { 
                    taiKhoan = new(line);
                    khachHang.TaiKhoanList.Add(taiKhoan);
                }
                if (Regex.IsMatch(line,"GD"))
                {
                    tTGiaoDich = new(line);
                    taiKhoan.TTGiaoDichList.Add(tTGiaoDich);
                }
            }
        }

        public void AddChiNhanh()
        {
            ChiNhanh? chiNhanh;
            string? id; 
            string? name;
            string? address;
            id = MyTool.readPattern("Thêm ID Chi Nhánh",ChiNhanh.ID_PATTERN).ToUpper();
            chiNhanh = this.SearchChiNhanhByID(id);
            if (chiNhanh != null)
            {
                Console.WriteLine("Chi Nhánh Tồn Tại!");
            }
            else
            {
                name = MyTool.readNonBlank("Tên Chi Nhánh");
                address = MyTool.readNonBlank("Địa Chỉ Chi Nhánh");
                List<KhachHang>? khachHangList = new();
                chiNhanh = new ChiNhanh(id, name, address, khachHangList);
                this.Add(chiNhanh);
            }
            if (MyTool.readBool("Thêm Khách Hàng?"))//3 
            {
                chiNhanh.AddKhachHang();
            }
        }
        public void SearchKhachHang()
        {   if(this.Count ==0 )
            {
                Console.WriteLine("Cần Thêm Chi Nhánh!"); 
                return;
            }
            string input = MyTool.readNonBlank("Nhập ID Khách Hàng").ToUpper();
            KhachHang? kh = SearchKhachHangByID(input);
            if(kh == null)
            {
                Console.WriteLine("ID Không Tồn Tại!");
                if (!MyTool.validStr(input,KhachHang.ID_PATTERN))
                    Console.WriteLine($"Thử Sử Dụng kiểu {KhachHang.ID_PATTERN}");
                return;

            }
            Console.WriteLine(kh);
        }
        public void AddGiaoDich()
        {
            if (this.Count == 0)
            {
                Console.WriteLine("Cần Thêm Chi Nhánh!");
                return;
            }

            int choice;
            KhachHang kh = null;
            TaiKhoan tk = null;
            string idTK, idKH;
            idKH = MyTool.readPattern("Nhập ID Khách Hàng: ", KhachHang.ID_PATTERN);
            kh = this.SearchKhachHangByID(idKH);
            if (kh == null)
            {
                Console.WriteLine("Khách Hàng ko tồn tại!");
                return;
            }
            if (kh.TaiKhoanList.Count == 0)
            {
                Console.WriteLine("Khách Hàng ko có tài khoản nào!");
                return;
            }
            choice = MyTool.getMenuChoiceStrict(kh.TaiKhoanList.ToArray());
            tk = kh.TaiKhoanList[choice - 1];
            tk.addGiaoDich(this);
        }

        internal TaiKhoan SearchTaiKhoanByID(string soTaiKhoan)
        {
            TaiKhoan tk = null;
            foreach (ChiNhanh ch in this)
            {
               tk = ch.SearchsoTaiKhoan(soTaiKhoan);
               if(tk!=null) return tk;
            }
            return null;
        }

        public void PrintLSGiaoDich()
        {
            if (this.Count == 0)
            {
                Console.WriteLine("Cần Thêm Chi Nhánh!");
                return;
            }
            ChiNhanh chiNhanh = null;
            string id = MyTool.readPattern("ID Chi Nhánh", ChiNhanh.ID_PATTERN).ToUpper();

            chiNhanh = this.SearchChiNhanhByID(id);
            if (chiNhanh == null)
            {
                Console.WriteLine("Chi Nhánh Không Tồn Tại!");
                return;
            }
            Console.WriteLine(chiNhanh.ToString());
        }

        public void PrintKhachHangUuTu()
        {
            if (this.Count == 0)
            {
                Console.WriteLine("Cần Thêm Chi Nhánh!");
                return;
            }
            ChiNhanh chiNhanh = null;
            string id = MyTool.readPattern("ID Chi Nhánh", ChiNhanh.ID_PATTERN).ToUpper();

            chiNhanh = this.SearchChiNhanhByID(id);

            if (chiNhanh == null)
            {
                Console.WriteLine("Chi Nhánh Không Tồn Tại!");
                return;
            }
            foreach(KhachHang kh in chiNhanh.KhachHangList)
            {
                Console.WriteLine(kh.ToSimplifiedString());
                Console.WriteLine(kh.TaiKhoanList.Max()) ;
            }
        }

        public void PrintLietKeTongSoDu()
        {
            if (this.Count == 0)
            {
                Console.WriteLine("Cần Thêm Chi Nhánh!");
                return;
            }
            ChiNhanh chiNhanh = null;
            string id = MyTool.readPattern("ID Chi Nhánh", ChiNhanh.ID_PATTERN).ToUpper();

            chiNhanh = this.SearchChiNhanhByID(id);

            if (chiNhanh == null)
            {
                Console.WriteLine("Chi Nhánh Không Tồn Tại!");
                return;
            }
            foreach (KhachHang kh in chiNhanh.KhachHangList)
                kh.UpdateTongSoDu();
            List<KhachHang> khachHangList = new(chiNhanh.KhachHangList);
            khachHangList.Sort((x, y) => x.TongSoDu.CompareTo(y.TongSoDu));
            khachHangList.ForEach(x => Console.WriteLine(x.ToSimplifiedString() +" | "+ x.TongSoDu));

        }

        public void PrintKhachHangNoiTroi()
        {
            //this chinhanh list khachhang list taikhoan list giaodichlist.Count   
            if (this.Count == 0)
            {
                Console.WriteLine("Cần Thêm Chi Nhánh!");
                return;
            }

            for (int i = 0; i < this.Count; i++)
            {
                ChiNhanh ch = this[i];
                KhachHang maxKH = ch.KhachHangList[0]; 
                int maxSum = 0;
                ch.KhachHangList.ForEach(kh =>
                {
                    int sum = 0;
                    sum = kh.TaiKhoanList.Sum(tk => tk.TTGiaoDichList.Count);
                    if (maxSum < sum)
                    {
                        maxSum = sum;
                        maxKH = kh;
                    }
                });
                //foreach (KhachHang kh in ch.KhachHangList)
                //{
                //    int sum = 0; 
                //    foreach (TaiKhoan tk in kh.TaiKhoanList)
                //        sum += tk.TTGiaoDichList.Count;
                //    if (maxSum < sum) { 
                //     maxSum = sum ;
                //        maxKH = kh;
                //    }
                //}
                Console.WriteLine(maxKH);
            }

        }
        public ChiNhanh? SearchChiNhanhByID(string id)
        {
            //return this?.SingleOrDefault(x => x.Id.Equals(id.ToUpper()));
            ChiNhanh ch =  (from x in this
                   where x.Id.Equals(id.ToUpper())
                   select x).First();
        foreach(ChiNhanh c in this)
            {
                c.Id.Equals(id.ToUpper());
            }
            return ch;
        }
        public KhachHang? SearchKhachHangByID(string khachHangID)
        {
            khachHangID = khachHangID.ToUpper();
            if (this.Count == 0) return null;
            foreach (ChiNhanh chiNhanh in this)
                {
                KhachHang? result = chiNhanh?.searchKhachHangByID(khachHangID);
                if (result != null) return result;
            }
            return null;
        }

        public void WriteToFile(string filename)
        {
            StreamWriter sr = new(filename);
            this.ForEach(ch => sr.WriteLine(ch.ToString());
            //foreach (ChiNhanh ch in this)
            //{
            //    sr.WriteLine(ch.ToString());
            //}
            sr.Close();
        }
    }
}
