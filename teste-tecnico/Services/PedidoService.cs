using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using teste_tecnico.Models;

namespace teste_tecnico.Services
{
    public class PedidoService
    {
        private static PedidoService _instance;
        public static PedidoService Instance => _instance ?? (_instance = new PedidoService());

        private readonly string _filePath;
        public ObservableCollection<Pedido> Pedidos { get; private set; }

        private PedidoService()
        {
            string dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
            if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
            _filePath = Path.Combine(dataDir, "pedidos.json");
            Pedidos = LoadPedidos();
        }

        private ObservableCollection<Pedido> LoadPedidos()
        {
            if (!File.Exists(_filePath)) return new ObservableCollection<Pedido>();
            string json = File.ReadAllText(_filePath);
            var pedidos = JsonConvert.DeserializeObject<List<Pedido>>(json) ?? new List<Pedido>();
            return new ObservableCollection<Pedido>(pedidos);
        }

        public void Add(Pedido pedido)
        {
            pedido.Id = Pedidos.Any() ? Pedidos.Max(p => p.Id) + 1 : 1;
            Pedidos.Add(pedido);
        }
        
        public void SaveChanges()
        {
            string json = JsonConvert.SerializeObject(Pedidos, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }
    }
}
