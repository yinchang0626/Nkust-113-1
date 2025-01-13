using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using CsvHelper.Configuration;

namespace CsvQueryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoresController : ControllerBase
    {
        private readonly string csvFilePath = @"C:\\Users\\user\\Desktop\\RestaurantCsvApi\\wwwroot\\112�~�סu�����b�x�_-��ڤj��v���a.csv"; // ��s�o�̬��A��ڪ�CSV�����|

        [HttpGet]
        public ActionResult<IEnumerable<Store>> GetStores()
        {
            var stores = new List<Store>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, config))
            {
                stores = csv.GetRecords<Store>().ToList();
            }

            return Ok(stores);
        }

        [HttpGet("{id}")]
        public ActionResult<Store> GetStore(int id)
        {
            var stores = new List<Store>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
            };

            using (var reader = new StreamReader(csvFilePath))
            using (var csv = new CsvReader(reader, config))
            {
                stores = csv.GetRecords<Store>().ToList();
            }

            var store = stores.FirstOrDefault(s => s.�Ǹ� == id);

            if (store == null)
            {
                return NotFound();
            }

            return Ok(store);
        }
    }
}
