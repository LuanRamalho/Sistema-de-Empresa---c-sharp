using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace SistemaDeEmpresa
{
    public class FuncionarioRepository
    {
        public void Add(Funcionario funcionario)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = @"INSERT INTO funcionarios (nome, funcao, salario, data_admissao, data_demissao)
                            VALUES (@nome, @funcao, @salario, @data_admissao, @data_demissao)";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nome", funcionario.Nome);
                    cmd.Parameters.AddWithValue("@funcao", funcionario.Funcao);
                    cmd.Parameters.AddWithValue("@salario", funcionario.Salario);
                    cmd.Parameters.AddWithValue("@data_admissao", funcionario.DataAdmissao.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@data_demissao", funcionario.DataDemissao.HasValue ? funcionario.DataDemissao.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(Funcionario funcionario)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = @"UPDATE funcionarios SET 
                                nome=@nome,
                                funcao=@funcao,
                                salario=@salario,
                                data_admissao=@data_admissao,
                                data_demissao=@data_demissao
                            WHERE id=@id";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@nome", funcionario.Nome);
                    cmd.Parameters.AddWithValue("@funcao", funcionario.Funcao);
                    cmd.Parameters.AddWithValue("@salario", funcionario.Salario);
                    cmd.Parameters.AddWithValue("@data_admissao", funcionario.DataAdmissao.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@data_demissao", funcionario.DataDemissao.HasValue ? funcionario.DataDemissao.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@id", funcionario.Id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Delete(int id)
        {
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM funcionarios WHERE id=@id";
                using (var cmd = new SqliteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Funcionario> GetAll()
        {
            List<Funcionario> list = new List<Funcionario>();
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqliteCommand("SELECT * FROM funcionarios", conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Funcionario
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Nome = reader["nome"].ToString(),
                            Funcao = reader["funcao"].ToString(),
                            Salario = Convert.ToDecimal(reader["salario"]),
                            DataAdmissao = DateTime.Parse(reader["data_admissao"].ToString()),
                            DataDemissao = string.IsNullOrEmpty(reader["data_demissao"].ToString()) ? (DateTime?)null : DateTime.Parse(reader["data_demissao"].ToString())
                        });
                    }
                }
            }
            return list;
        }

        public Funcionario GetById(int id)
        {
            Funcionario funcionario = null;
            using (var conn = DatabaseHelper.GetConnection())
            {
                conn.Open();
                using (var cmd = new SqliteCommand("SELECT * FROM funcionarios WHERE id=@id", conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            funcionario = new Funcionario
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Nome = reader["nome"].ToString(),
                                Funcao = reader["funcao"].ToString(),
                                Salario = Convert.ToDecimal(reader["salario"]),
                                DataAdmissao = DateTime.Parse(reader["data_admissao"].ToString()),
                                DataDemissao = string.IsNullOrEmpty(reader["data_demissao"].ToString()) ? (DateTime?)null : DateTime.Parse(reader["data_demissao"].ToString())
                            };
                        }
                    }
                }
            }
            return funcionario;
        }
    }
}