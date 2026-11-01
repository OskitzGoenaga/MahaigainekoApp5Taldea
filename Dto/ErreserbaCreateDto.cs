using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MahaigainekoAppTransakzioak.Dto
{
    public class ErreserbaCreateDto
    {
        public int PistaId { get; set; }
        public int BazkideaId { get; set; }
        public string Data { get; set; } = string.Empty;
        public string Ordua { get; set; } = string.Empty;
    }
}
