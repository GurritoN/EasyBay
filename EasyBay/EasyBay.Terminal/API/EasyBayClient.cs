using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.IO;
using System.Threading.Tasks;
using EasyBay.Terminal.Models;
using EasyBay.Messaging;
using System.Text;

namespace EasyBay.Terminal.API
{
    public class EasyBayClient
    {
        private HttpClient client;
        private const string URI = @"http://localhost:51868/api/";

        public EasyBayClient(string token)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(URI);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        #region User

        public static async Task<bool> CreateUser(string username, string password, string email)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(URI);
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("email", email)
            });
            var response = await client.PostAsync("user", content);
            return response.IsSuccessStatusCode;
        }

        public static async Task<string> GetToken(string username, string password)
        {
            var client = new HttpClient();
            client.BaseAddress = new Uri(URI);
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password)
            });
            var response = await client.PostAsync("user/token", content);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : null;
        }

        public async Task<User> GetUser(string username)
        {
            var response = await client.GetAsync($"user/{username}");
            return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync()) : null;
        }

        public async Task<bool> EditUser(string username, string password, string email)
        {
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("username", username),
                new KeyValuePair<string, string>("password", password),
                new KeyValuePair<string, string>("email", email)
            });
            var response = await client.PutAsync("user", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteUser(string username)
        {
            var response = await client.DeleteAsync($"user/{username}");
            return response.IsSuccessStatusCode;
        }
        #endregion

        #region Lots
        public async Task<bool> CreateLot(CreateLotRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("lots", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Lot>> GetLotPage(int page, int pagesize)
        {
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("page", page.ToString()),
                new KeyValuePair<string, string>("pagesize", pagesize.ToString())
            });
            var response = await client.PostAsync("lots/all", content);
            string rs = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<List<Lot>>(rs) : null;
        }

        public async Task<Lot> GetLot(int lotId)
        {
            var response = await client.GetAsync($"lots/{lotId}");
            return response.IsSuccessStatusCode ? JsonConvert.DeserializeObject<Lot>(await response.Content.ReadAsStringAsync()) : null;
        }

        public async Task<bool> EditLot(EditLotRequest request)
        {
            var content = new StringContent(JsonConvert.SerializeObject(request));
            var response = await client.PutAsync("lots", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteLot(int lotId)
        {
            var response = await client.DeleteAsync($"lots/{lotId}");
            return response.IsSuccessStatusCode;
        }

        #endregion

        #region Actions

        public async Task<bool> RaisePrice(int lotId, decimal newPrice)
        {
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("lotId", lotId.ToString()),
                new KeyValuePair<string, string>("newPrice", newPrice.ToString())
            });
            var response = await client.PostAsync("action/raise", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> BuyOut(int lotId)
        {
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("lotId", lotId.ToString())
            });
            var response = await client.PostAsync("action/buyout", content);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> Deposit(decimal amount)
        {
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("amount", amount.ToString())
            });
            var response = await client.PostAsync("action/deposit", content);
            return response.IsSuccessStatusCode;
        }

        #endregion
    }
}