using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Techcombank.dto
{
    public class KhachHang:IComparer<KhachHang>
    {
        public const string ID_PATTERN = "[Dd][0-9]{3,}";// [Dd] D []
        private readonly string SEPERATOR = "|";
        private string id;
        private string name;
        private string address;
        private List<TaiKhoan> taiKhoanList;
        private double tongSoDu;

        

        public KhachHang(string id, string name, string address, List<TaiKhoan> taiKhoanList)
        {
            Id = id;
            Name = name;
            Address = address;
            TaiKhoanList = taiKhoanList;
        }

        public KhachHang(string line)
        {
            string[] parts = line.Split(SEPERATOR);
            this.Id = parts[0].Trim();
            this.Name = parts[1].Trim();
            this.Address = parts[2].Trim();
            this.taiKhoanList = new();
        }

        /*Mã khách hàng
o Tên khách hàng
o Địa chỉ gồm: thôn, xã/phường, quận/huyện và tỉnh/thành phố
o Danh sách các tài khoản mà Khách hàng này có*/
        public string Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public List<TaiKhoan> TaiKhoanList { get => taiKhoanList; set => taiKhoanList = value; }
        public double TongSoDu { get => tongSoDu; set => tongSoDu = value; }
        public void addTaiKhoan(List<TaiKhoan> taiKhoanList, ChiNhanh chiNhanh)
        {
            do
            {
                string soTaiKhoan = MyTool.readPattern("Số Tài Khoản:", TaiKhoan.STK_PATTERN).ToUpper();//read pattern "regex" regular expression
                soTaiKhoan = TaiKhoan.formatSTK(soTaiKhoan);

                TaiKhoan? taiKhoan;

                if (chiNhanh.KhachHangList.Count == 0)
                    taiKhoan = this.SearchSoTaiKhoan(soTaiKhoan);
                else
                    taiKhoan = chiNhanh.SearchsoTaiKhoan(soTaiKhoan);

                if (taiKhoan != null)
                {
                    Console.WriteLine("Tài Khoản tồn tại");
                }
                else
                {
                    taiKhoan = new TaiKhoan(0, soTaiKhoan);
                    TaiKhoanList.Add(taiKhoan);
                    if (MyTool.readBool("Nạp Thêm Tiền?"))
                    {
                        taiKhoan.napTien();
                    }
                    else
                        Console.WriteLine(taiKhoan);
                }
            } while (MyTool.readBool("Tiếp Tục Thêm Số Tài Khoản?"));
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Kết Thúc Thêm Tài Khoản");


        }



        internal TaiKhoan? SearchSoTaiKhoan(string soTaiKhoan)
        {
            if (taiKhoanList.Count == 0) return null;
            try
            {
                return taiKhoanList?.SingleOrDefault(x => x.SoTaiKhoan.Equals(soTaiKhoan));
            }
            catch (Exception e) { }
            return null;
        }
        public override string? ToString()
        {
            StringBuilder? result = new StringBuilder($"{Id} | {Name} | {Address} | {taiKhoanList.Count}");
            foreach (TaiKhoan tk in taiKhoanList)
                result.Append($"\n{tk.ToString()}");
            return result.ToString() ?? null;
        }


        public string ToSimplifiedString()
        {
            return $"{Id} | {Name} | {Address} | {taiKhoanList.Count}";
        }

        internal void UpdateTongSoDu()
        {
            this.tongSoDu = this.TaiKhoanList.Sum(x => x.SoDu);
        }

        public int Compare(KhachHang? x, KhachHang? y)
        {
            return x.tongSoDu.CompareTo(y.tongSoDu);
        }
    }
}
