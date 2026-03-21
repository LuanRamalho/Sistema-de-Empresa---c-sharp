using System;

namespace SistemaDeEmpresa
{
    public class Funcionario
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Funcao { get; set; } = string.Empty;
        public decimal Salario { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime? DataDemissao { get; set; }

        public string SalarioFormatado => Salario.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
        public string DataAdmissaoFormatada => DataAdmissao.ToString("dd/MM/yyyy");
        public string DataDemissaoFormatada => DataDemissao.HasValue ? DataDemissao.Value.ToString("dd/MM/yyyy") : "";
    }
}