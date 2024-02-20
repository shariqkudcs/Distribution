using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Data;
using Server.Models;
using Server.Resource;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DistrictsController : ControllerBase
    {
        private readonly DistributionContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryController> _logger;

        public DistrictsController(DistributionContext context, IMapper mapper, ILogger<InventoryController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<District>>> GetDistricts()
        {
            try
            {
                _logger.LogInformation("Retrieving all districts.");

                var districts = await _context.Districts.ToListAsync();

                _logger.LogInformation($"Number of districts retrieved: {districts.Count}");

                _logger.LogInformation($"Districts details: {JsonConvert.SerializeObject(districts)}");

                return districts;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error occurred while retrieving districts.{0}", ex);
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<DistrictResource>> GetDistrict(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving district with ID {id}");

                var districtEf = await _context.Districts.FindAsync(id);

                if (districtEf == null)
                {
                    _logger.LogWarning($"District with ID {id} not found.");
                    return NotFound();
                }

                var districtResource = _mapper.Map<District, DistrictResource>(districtEf);
                _logger.LogInformation($"District details: {JsonConvert.SerializeObject(districtResource)}");

                return districtResource;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred while retrieving district with ID {id}.", ex);
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDistrict(int id, DistrictResource district)
        {
            try
            {
                _logger.LogInformation($"Updating district with ID {id}. Request details: {JsonConvert.SerializeObject(district)}");

                if (id != district.Id)
                {
                    _logger.LogError("Bad request: ID in the URL does not match the ID in the request body.");
                    return BadRequest();
                }

                _context.Entry(_mapper.Map<DistrictResource, District>(district)).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"District with ID {id} updated successfully.");
                    return NoContent();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError($"Concurrency exception while updating district with ID {id}.", ex);

                    if (!DistrictExists(id))
                    {
                        _logger.LogError($"District with ID {id} not found.");
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Unexpected error occurred during district update.{0}", ex);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error occurred during district update.{0}", ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<DistrictResource>> PostDistrict(DistrictResource district)
        {
            try
            {
                _logger.LogInformation($"Creating a new district. Request details: {JsonConvert.SerializeObject(district)}");

                _logger.LogInformation($"District details: {JsonConvert.SerializeObject(district)}");

                _context.Districts.Add(_mapper.Map<DistrictResource, District>(district));
                await _context.SaveChangesAsync();

                _logger.LogInformation($"District with ID {district.Id} created successfully.");

                return CreatedAtAction("GetDistrict", new { id = district.Id }, district);
            }
            catch (DbUpdateException)
            {
                if (DistrictExists(district.Id))
                {
                    _logger.LogError($"Conflict: District with ID {district.Id} already exists.");
                    return Conflict();
                }
                else
                {
                    _logger.LogError("Unexpected error occurred during district creation.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error occurred during district creation.{0}", ex);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDistrict(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting district with ID {id}");

                var district = await _context.Districts.FindAsync(id);

                if (district == null)
                {
                    _logger.LogWarning($"District with ID {id} not found for deletion.");
                    return NotFound();
                }

                _logger.LogInformation($"District details before deletion: {JsonConvert.SerializeObject(district)}");

                _context.Districts.Remove(district);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"District with ID {id} deleted successfully.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred during district deletion with ID {id}.", ex);
                throw;
            }
        }

        private bool DistrictExists(int id)
        {
            return _context.Districts.Any(e => e.Id == id);
        }
    }
}
