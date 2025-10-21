using System;
using System.Net.Http.Json;
using PortalGalaxy.Common.Response;
using PortalGalaxy.WebApp.Proxy.Interfaces;

namespace PortalGalaxy.WebApp.Proxy.Services;

public class JsonProxy : RestBase, IJsonProxy
{
    public JsonProxy(HttpClient httpClient) 
    : base("data", httpClient)
    {
    }

    public async Task<ICollection<DepartamentoModel>> ListDepartamentos()
    {
        List<DepartamentoModel> departamentos = new();
        try
        {
            departamentos = await HttpClient.GetFromJsonAsync<List<DepartamentoModel>>("data/departamentos.json") ??
                                                    new List<DepartamentoModel>();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return departamentos;
    }

    public async Task<ICollection<DistritoModel>> ListDistritos(string codProvincia)
    {
        var distritos = await HttpClient.GetFromJsonAsync<List<DistritoModel>>("data/distritos.json") ??
            new List<DistritoModel>();

        return distritos.Where(d => d.CodProvincia == codProvincia).ToList();
    }

    public async Task<ICollection<SituacionModel>> ListSituaciones()
    {
        List<SituacionModel> situaciones = new();
        try
        {
            situaciones = await HttpClient.GetFromJsonAsync<List<SituacionModel>>("data/situaciones.json") ??
                                               new List<SituacionModel>();
            
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        return situaciones;
    }

    public async Task<ICollection<ProvinciaModel>> ListProvincias(string codigoDpto)
    {
        var provincias = await HttpClient.GetFromJsonAsync<List<ProvinciaModel>>("data/provincias.json") ??
            new List<ProvinciaModel>();

        return provincias.Where(p => p.CodigoDpto == codigoDpto).ToList();
    }
}
