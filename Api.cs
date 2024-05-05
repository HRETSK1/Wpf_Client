using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1
{
    class Api
    {
        private HttpClient _httpClient;
        public Api(string apiUri)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(apiUri)
            };
        }

        public async Task<IEnumerable<User>> GetAsync()
        {
            var response = await _httpClient.GetAsync("api/User");
            return await response.Content.ReadAsAsync<ObservableCollection<User>>();
        }

        public async Task<HttpResponseMessage> CreateAsync(User user) 
        {
            var response = await _httpClient.PostAsJsonAsync("api/User", user);
            return response;
        }

        public async Task<HttpResponseMessage> EditAsync(User user)
        {
            var response = await _httpClient.PutAsJsonAsync("api/User", user);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/User/{id}");
            return response;
        }


    }
}
