using Entity.Models;
using System.Text.Json;
using System.Net.Http.Headers;
using System.ComponentModel;

namespace UniversidadWeb
{
	public class Client
	{
		public string Baseurl;

		public Client(string baseurl)
		{
			Baseurl = baseurl;
		}

		public async Task<List<T>> GetList<T>(string url)
		{
			var list = new List<T>();
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Baseurl);
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage Res = await client.GetAsync(url);
				if (Res.IsSuccessStatusCode)
				{
					var EmpResponse = Res.Content.ReadAsStringAsync().Result;
					var rpta = JsonSerializer.Deserialize<RespuestaGeneral<List<T>>>(EmpResponse);
					list = rpta.DatosRespuesta;
				}
				return list;
			}
		}

		public async Task<T> GetEntity<T>(string url)
		{
			var entity = default(T);
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Baseurl);
				client.DefaultRequestHeaders.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage Res = await client.GetAsync(url);
				if (Res.IsSuccessStatusCode)
				{
					var EmpResponse = Res.Content.ReadAsStringAsync().Result;
					var rpta = JsonSerializer.Deserialize<RespuestaGeneral<T>>(EmpResponse);
					entity = rpta.DatosRespuesta;
				}
				return entity;
			}
		}

		public async Task<RespuestaGeneral> Post<T>(string url, T dto)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Baseurl);
				var result = await client.PostAsJsonAsync(url, dto);
				if (result.IsSuccessStatusCode)
				{
					var rpta = JsonSerializer.Deserialize<RespuestaGeneral<T>>(result.Content.ReadAsStringAsync().Result);
					return rpta;
				}
				else
				{
					throw new Exception(result.RequestMessage.ToString());
				}
			}
		}

		public async Task<RespuestaGeneral> Put<T>(string url, T dto)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Baseurl);
				var result = await client.PutAsJsonAsync(url, dto);
				if (result.IsSuccessStatusCode)
				{
					var rpta = JsonSerializer.Deserialize<RespuestaGeneral<T>>(result.Content.ReadAsStringAsync().Result);
					return rpta;
				}
				else
				{
					throw new Exception(result.RequestMessage.ToString());
				}
			}
		}

		public async Task<bool> Delete(string url)
		{
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(Baseurl);
				var result = await client.DeleteAsync(url);
				if (result.IsSuccessStatusCode)
				{
					return true;
				}
				return false;
			}
		}
	}
}
