using CertificateService.Dtos;
using System.Text;
using System.Text.Json;
namespace CertificateService.SyncDataServices.Http
{
    public class HttpSkillDataClient : ISkillDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpSkillDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
 

        public async Task SendCertificateToSkill(CertificateReadDto cer)
        {
            var httpContent= new StringContent(
                JsonSerializer.Serialize(cer),
                Encoding.UTF8,
                "application/json"
            );
            var response= await _httpClient.PostAsync($"{_configuration["SkillService"]}", httpContent);
            if(response.IsSuccessStatusCode)
            {
                Console.WriteLine($"=>sync post to Skill service was ok the address is {response.Headers} and {_configuration["SkillService"]}");
            }
            else
            {
                Console.WriteLine($"=>sync post to Skill service failed the address is {response.Headers} and {_configuration["SkillService"]}");
            }

        }
    }
}
