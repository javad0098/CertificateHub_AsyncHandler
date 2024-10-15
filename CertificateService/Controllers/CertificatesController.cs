using System.Collections.Generic; // Ensure to include the correct namespaces for IEnumerable
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CertificateService.Data;
using CertificateService.Models;
using CertificateService.Dtos;
using CertificateService.SyncDataServices.Http;
using CertificateService.AsyncDataServices;

namespace CertificatesService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CertificatesController : ControllerBase
    {
        private readonly ISkillDataClient _skillDataClient;
        private readonly IMapper _maper;
        private readonly ICertificateRepo _repository;
        private readonly IMessageBusClient _messageBusClient;

        public CertificatesController(ICertificateRepo repository, IMapper maper, ISkillDataClient skillDataClient, IMessageBusClient messageBusClient)
        {
            _skillDataClient = skillDataClient;
            _maper = maper;
            _repository = repository;
            _messageBusClient = messageBusClient;
        }
        // GET api/certificates
        [HttpGet]
        public ActionResult<IEnumerable<Certificate>> GetCertificates()
        {

            Console.WriteLine(">> Getting All Certificates");

            var certificates = _repository.GetAllCertificates();

            return Ok(_maper.Map<IEnumerable<CertificateReadDto>>(certificates));
        }

        [HttpGet("{id}", Name = "GetCertificateById")]
        public ActionResult<Certificate> GetCertificateById(int id)
        {


            var certificate = _repository.GetCertificateById(id);

            if (certificate != null)
            {
                return Ok(_maper.Map<CertificateReadDto>(certificate));
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult<Certificate>> CreateCertificate(CertificateCreateDtos certificateCreate)
        {
            var certificate = _maper.Map<Certificate>(certificateCreate);
            _repository.CreateCertificate(certificate);
            _repository.SaveChenges();
            var certificateReadDto = _maper.Map<CertificateReadDto>(certificate);
            // Send ASync message
            try
            {
                await _skillDataClient.SendCertificateToSkill(certificateReadDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"==> could not send synchronously: {ex.Message}");
            }

            // Send ASync message 
            try
            {
                var certificatePublishedDto = _maper.Map<CertificatePublishedDto>(certificateReadDto);
                certificatePublishedDto.Event = "Certificate_Published";
                _messageBusClient.PublishNewCertificate(certificatePublishedDto);

            }
            catch (Exception ex)
            {
                Console.WriteLine($"==> could not send Asynchronously: {ex.Message}");
            }

            return CreatedAtRoute(nameof(GetCertificateById), new { id = certificateReadDto.Id }, certificateReadDto);
        }
    }
}
