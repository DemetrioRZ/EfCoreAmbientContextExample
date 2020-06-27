using Microsoft.AspNetCore.Mvc;

namespace EfCoreAmbientContextExample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private readonly IEntityAService _entityAService;

        private readonly IEntityBService _entityBService;

        public TestController(
            IEntityAService entityAService, 
            IEntityBService entityBService)
        {
            _entityAService = entityAService;
            _entityBService = entityBService;
        }

        [HttpGet("mssql")]
        public ActionResult TestMsSql()
        {
            _entityAService.TestA();
            
            _entityBService.TestB();
            
            return new OkResult();
        }
    }
}