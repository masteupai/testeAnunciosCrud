using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace teste_webmotors.ViewModel
{
    public class AnuncioVMRetornar
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public string Versao { get; set; }
        public int Ano { get; set; }
        public int Quilometragem { get; set; }
        public string Observacao { get; set; }
    }
}
