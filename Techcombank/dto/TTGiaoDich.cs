using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techcombank.dto
{
    public class TTGiaoDich
    {
        private readonly string SEPERATOR="|";
        private DateTime _ngayGD;
        private double _soTienGD;
        private char _loaiGD;

        public TTGiaoDich()
        {
        }

        public TTGiaoDich(string line)
        {
            string[] parts = line.Split(SEPERATOR);
            this.NgayGD = DateTime.Parse(parts[1].Trim());
            this.SoTienGD = double.Parse(parts[2].Replace("VND", "").Trim());
            this.LoaiGD = parts[3].Trim()[0];
        }

        public TTGiaoDich( DateTime ngayGD, double soTienGD, char loaiGD)
        {
            NgayGD = ngayGD;
            SoTienGD = soTienGD;
            LoaiGD = loaiGD;
        }

        public DateTime NgayGD { get => _ngayGD; set => _ngayGD = value; }
        public double SoTienGD { get => _soTienGD; set => _soTienGD = value; }
        public char LoaiGD { get => _loaiGD; set => _loaiGD = value; }
        
        public override string? ToString()
        {
            return $"GD | {NgayGD:G} | {SoTienGD:f} | {LoaiGD}";
        }
    }
}
