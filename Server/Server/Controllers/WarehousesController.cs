﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Data;
using Server.Models;
using Server.Response;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly DistributionContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryController> _logger;

        public WarehousesController(DistributionContext context, IMapper mapper, ILogger<InventoryController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Warehouse>>> GetWarehouses()
        {
            try
            {
                _logger.LogInformation("Retrieving all warehouses.");

                var warehouses = await _context.Warehouses.ToListAsync();

                _logger.LogInformation($"Number of warehouses retrieved: {warehouses.Count}");

                _logger.LogInformation($"Warehouses details: {JsonConvert.SerializeObject(warehouses)}");

                return warehouses;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error occurred while retrieving warehouses.{0}", ex);
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<WarehouseResource>> GetWarehouse(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving warehouse with ID {id}");

                var warehouseEf = await _context.Warehouses.FindAsync(id);

                if (warehouseEf == null)
                {
                    _logger.LogWarning($"Warehouse with ID {id} not found.");
                    return NotFound();
                }

                var warehouseResource = _mapper.Map<Warehouse, WarehouseResource>(warehouseEf);
                _logger.LogInformation($"Warehouse details: {JsonConvert.SerializeObject(warehouseResource)}");

                return Ok(warehouseResource);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred while retrieving warehouse with ID {id}.", ex);
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutWarehouse(int id, WarehouseResource warehouse)
        {
            _logger.LogInformation($"Updating warehouse with ID {id}. Request details: {JsonConvert.SerializeObject(warehouse)}");

            if (id != warehouse.Id)
            {
                _logger.LogError("Bad request: ID in the URL does not match the ID in the request body.");
                return BadRequest();
            }

            try
            {
                _context.Entry(_mapper.Map<WarehouseResource, Warehouse>(warehouse)).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Warehouse with ID {id} updated successfully.");
                    return NoContent();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError($"Concurrency exception while updating warehouse with ID : {id} {ex}");

                    if (!WarehouseExists(id))
                    {
                        _logger.LogError($"Warehouse with ID {id} not found.");
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError($"Unexpected error occurred during warehouse update : {ex}");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred during warehouse update :{ex}");
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<WarehouseResource>> PostWarehouse(WarehouseResource warehouse)
        {
            _logger.LogInformation($"Received POST request to add warehouse. Request details: {JsonConvert.SerializeObject(warehouse)}");

            var saveEntity = _mapper.Map<WarehouseResource, Warehouse>(warehouse);
            _context.Warehouses.Add(saveEntity);
            try
            {
                _logger.LogInformation("Creating a new Warehouse.");

                await _context.SaveChangesAsync();
                warehouse.Id = saveEntity.Id;
                _logger.LogInformation($"Warehouse with ID {warehouse.Id} created successfully.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError($"Error while updating the database: {ex.Message}");

                if (WarehouseExists(warehouse.Id))
                {
                    _logger.LogError($"Conflict: Warehouse with ID {warehouse.Id} already exists.");
                    return Conflict();
                }
                else
                {
                    _logger.LogError("Unexpected error occurred during warehouse creation.{0}", ex);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred during order creation : {ex}");
                throw;
            }

            return CreatedAtAction("GetWarehouse", new { id = saveEntity.Id }, warehouse);
        }

        private bool WarehouseExists(int id)
        {
            return _context.Warehouses.Any(e => e.Id == id);
        }
    }
}
