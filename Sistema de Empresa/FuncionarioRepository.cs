using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace SistemaDeEmpresa
{
    public class FuncionarioRepository
    {
        private List<Funcionario> _funcionarios;
        
        // Configurações para salvar o JSON de forma bonita e aceitando acentuação em PT-BR
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public FuncionarioRepository()
        {
            CarregarDados();
        }

        private void CarregarDados()
        {
            if (File.Exists(DatabaseHelper.DatabaseFile))
            {
                string json = File.ReadAllText(DatabaseHelper.DatabaseFile, Encoding.UTF8);
                _funcionarios = JsonSerializer.Deserialize<List<Funcionario>>(json, _jsonOptions) ?? new List<Funcionario>();
            }
            else
            {
                _funcionarios = new List<Funcionario>();
            }
        }

        private void SalvarDados()
        {
            string json = JsonSerializer.Serialize(_funcionarios, _jsonOptions);
            File.WriteAllText(DatabaseHelper.DatabaseFile, json, Encoding.UTF8);
        }

        public void Add(Funcionario funcionario)
        {
            _funcionarios.Add(funcionario);
            SalvarDados();
        }

        public void Update(Funcionario funcionario)
        {
            // Como os objetos operam por referência em memória, 
            // a interface já altera as propriedades do objeto original.
            // Precisamos apenas sobrescrever o arquivo JSON.
            SalvarDados();
        }

        public void Delete(Funcionario funcionario)
        {
            // Deleta usando a referência do objeto na memória
            _funcionarios.Remove(funcionario);
            SalvarDados();
        }

        public List<Funcionario> GetAll()
        {
            return _funcionarios;
        }
    }
}