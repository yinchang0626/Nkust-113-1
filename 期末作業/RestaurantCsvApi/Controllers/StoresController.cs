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
        private readonly string csvFilePath = @"C:\\Users\\user\\Desktop\\RestaurantCsvApi\\wwwroot\\112年度「美食在台北-鍋際大賞」店家.csv"; // 更新這裡為你實際的CSV文件路徑

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

            var store = stores.FirstOrDefault(s => s.序號 == id);

            if (store == null)
            {
                return NotFound();
            }

            return Ok(store);
        }
    }
}
