using System;
using System.Text.Json.Serialization;

namespace SistemaDeEmpresa
{
    public class Funcionario
    {
        // O campo Id foi removido para o padrão NoSQL

        public string Nome { get; set; } = string.Empty;
        public string Funcao { get; set; } = string.Empty;
        public decimal Salario { get; set; }
        public DateTime DataAdmissao { get; set; }
        public DateTime? DataDemissao { get; set; }

        // JsonIgnore evita que as propriedades de leitura sejam salvas no banco JSON
        [JsonIgnore]
        public string SalarioFormatado => Salario.ToString("C", new System.Globalization.CultureInfo("pt-BR"));
        [JsonIgnore]
        public string DataAdmissaoFormatada => DataAdmissao.ToString("dd/MM/yyyy");
        [JsonIgnore]
        public string DataDemissaoFormatada => DataDemissao.HasValue ? DataDemissao.Value.ToString("dd/MM/yyyy") : "";
    }
}