using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using teste_tecnico.Models;

namespace teste_tecnico.Services
{
    public class ProdutoService
    {        
        private static ProdutoService _instance;
        public static ProdutoService Instance => _instance ?? (_instance = new ProdutoService());

        private readonly string _filePath;
                
        public ObservableCollection<Produto> Produtos { get; private set; }
        
        private ProdutoService()
        {
            string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataDir))
            {
                Directory.CreateDirectory(dataDir);
            }
            _filePath = Path.Combine(dataDir, "produtos.json");

            Produtos = LoadProdutos();
        }

        private ObservableCollection<Produto> LoadProdutos()
        {
            if (!File.Exists(_filePath))
            {
                return new ObservableCollection<Produto>();
            }

            string json = File.ReadAllText(_filePath);
            var produtos = JsonConvert.DeserializeObject<List<Produto>>(json) ?? new List<Produto>();
            return new ObservableCollection<Produto>(produtos);
        }

        public void Add(Produto produto)
        {
            produto.Id = Produtos.Any() ? Produtos.Max(p => p.Id) + 1 : 1;
            Produtos.Add(produto);
        }

        public void Delete(int id)
        {
            var produto = Produtos.FirstOrDefault(p => p.Id == id);
            if (produto != null)
            {
                Produtos.Remove(produto);
            }
        }

        public void SaveChanges()
        {
            string json = JsonConvert.SerializeObject(Produtos, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}
