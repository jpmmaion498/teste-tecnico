using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using teste_tecnico.Models;

namespace teste_tecnico.Services
{
    public class PessoaService
    {
        private static PessoaService _instance;
        public static PessoaService Instance => _instance ?? (_instance = new PessoaService());

        private readonly string _filePath;
        public ObservableCollection<Pessoa> Pessoas { get; private set; }

        private PessoaService()
        {
            string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
            _filePath = Path.Combine(dataDir, "pessoas.json");
            Pessoas = LoadPessoas();
        }

        private ObservableCollection<Pessoa> LoadPessoas()
        {
            if (!File.Exists(_filePath)) return new ObservableCollection<Pessoa>();
            string json = File.ReadAllText(_filePath);
            var pessoas = JsonConvert.DeserializeObject<List<Pessoa>>(json) ?? new List<Pessoa>();
            return new ObservableCollection<Pessoa>(pessoas);
        }

        public void Add(Pessoa pessoa)
        {
            pessoa.Id = Pessoas.Any() ? Pessoas.Max(p => p.Id) + 1 : 1;
            Pessoas.Add(pessoa);
        }

        public void Delete(int id)
        {
            var pessoa = Pessoas.FirstOrDefault(p => p.Id == id);
            if (pessoa != null) Pessoas.Remove(pessoa);
        }

        public void SaveChanges()
        {
            string json = JsonConvert.SerializeObject(Pessoas, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}
