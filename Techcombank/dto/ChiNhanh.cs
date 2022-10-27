using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techcombank.dto
{
    public class ChiNhanh
    {
        public const string ID_PATTERN = @"[Cc]\d{3,}";
        private const string SEPERATOR = "|";
        private string id;
        private string _name;
        private string _address;
        private List<KhachHang> _khachHangList;

        public ChiNhanh()
        {
            KhachHangList = new List<KhachHang>();
        }

        public ChiNhanh(string id, string? name, string address, List<KhachHang> khachHangList)
        {
            Id = id;
            Name = name;
            Address = address;
            KhachHangList = khachHangList;
        }

        public ChiNhanh(string line)
        {
            string[] parts = line.Split(SEPERATOR);
            this.Id = parts[0].Trim();
            this.Name = parts[1].Trim();
            this.Address = parts[2].Trim();
            this.KhachHangList = new();
        }

        public string Id { get => id; set => id = value; }
        public string? Name { get => _name; set => _name = value; }
        public string Address { get => _address; set => _address = value; }
        public List<KhachHang> KhachHangList { get => _khachHangList; set => _khachHangList = value;}

        public void AddKhachHang()
        {
            bool exit = false;
            List<TaiKhoan> taiKhoanList;
            do
            {
                string id = MyTool.readPattern("ID của khách hàng",KhachHang.ID_PATTERN).ToUpper();
                    KhachHang? khachHang = SearchKhachHangId(id);
                if (khachHang != null)
                {
                    Console.WriteLine("Khách Hàng tồn tại");
                    taiKhoanList = khachHang.TaiKhoanList;
                }
                else
                {
                    string name = MyTool.readNonBlank("Tên của Khách Hàng");
                    string address = MyTool.readNonBlank("Thông tin địa chỉ"); 
                    taiKhoanList = new List<TaiKhoan>();
                    khachHang = new KhachHang(id, name, address, taiKhoanList);
                    Console.WriteLine($"Đã Thêm khách Hàng: {khachHang}");
                    KhachHangList?.Add(khachHang);
                }
                if (MyTool.readBool("Thêm Tài Khoản Ngân Hàng?"))
                    {
                        khachHang.addTaiKhoan(taiKhoanList,this);
                    }
                if (!MyTool.readBool("Tiếp Tục Thêm Khách Hàng?")) exit = true;// Y = continue , N = break loop
            } while (!exit);
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Kết Thúc Thêm Khách Hàng");

        }
        private KhachHang? SearchKhachHangId(string id)
        {   if (KhachHangList.Count == 0) return null;
            try
            {
                return KhachHangList?.FirstOrDefault(x => x.Id == id);
            }catch(Exception e)
            {

            }
            return null;
        }
        internal TaiKhoan? SearchsoTaiKhoan(string soTaiKhoan)
        {
            if (KhachHangList.Count == 0) return null;
            TaiKhoan? taiKhoan = null;
            foreach (KhachHang khachHang in KhachHangList)
            {
                taiKhoan = khachHang?.SearchSoTaiKhoan(soTaiKhoan);
                if (taiKhoan != null) return taiKhoan;
            }
            return null;
        }

        public override string? ToString()
        {
            StringBuilder result = new($"{Id} | {Name} | {Address} | {KhachHangList.Count}");
            foreach(KhachHang kh in KhachHangList) 
                result.Append("\n"+kh.ToString());
            return result.ToString() ?? null;
        }

        internal KhachHang? searchKhachHangByID(string khachHangID)
        {
            return KhachHangList?.SingleOrDefault(x => x.Id == khachHangID);
        }
    }
}
