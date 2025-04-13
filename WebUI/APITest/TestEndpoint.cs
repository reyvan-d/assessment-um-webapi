using System.Text.Json;

namespace APITest
{
    [TestClass]
    public class TestEndpoint
    {
        protected HttpClient _client = new HttpClient();

        public TestEndpoint()
        {
            _client.BaseAddress = new Uri("https://localhost:7267");
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
        }

        [TestMethod]
        public async Task TestGET()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/User/GetUserCount");
            var response = await _client.SendAsync(request);

            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task TestPOST()
        {
            var getrequest = new HttpRequestMessage(HttpMethod.Get, "/api/Group/GetAllGroups");
            var response = await _client.SendAsync(getrequest);
            Assert.IsTrue(response.IsSuccessStatusCode);
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var result = await JsonSerializer.DeserializeAsync<GetAllResp>(responseStream);
            var postrequest = new HttpRequestMessage(HttpMethod.Post, "/api/Group/GetTotalUsersForGroup");
            var reqstruct = new GetTotalPostReq(result.groups.First().Key);
            postrequest.Content = new StringContent(JsonSerializer.Serialize<GetTotalPostReq>(reqstruct), System.Text.Encoding.UTF8, "application/json");
            var postresult = await _client.SendAsync(postrequest);
            Assert.IsTrue(postresult.IsSuccessStatusCode);
        }

        private class GetAllResp
        {
            public Dictionary<long, string> groups { get; set; }
        }

        private class GetTotalPostReq
        {
            public GetTotalPostReq(long id)
            {
                GroupId = id;
            }

            public long GroupId { get; set; }
        }
    }
}
