﻿using AutoMapper;
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
    public class ItemsController : ControllerBase
    {
        private readonly DistributionContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryController> _logger;

        public ItemsController(DistributionContext context, IMapper mapper, ILogger<InventoryController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemResource>> GetItem(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving item with ID {id}");

                var itemEf = await _context.Items.FindAsync(id);

                if (itemEf == null)
                {
                    _logger.LogWarning($"Item with ID {id} not found.");
                    return NotFound();
                }

                var itemResource = _mapper.Map<Item, ItemResource>(itemEf);
                _logger.LogInformation($"Item details: {JsonConvert.SerializeObject(itemResource)}");

                return itemResource;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred while retrieving item with ID {id}.", ex);
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutItem(int id, ItemResource item)
        {
            _logger.LogInformation($"Updating item with ID {id}. Request details: {JsonConvert.SerializeObject(item)}");

            if (id != item.Id)
            {
                _logger.LogError("Bad request: ID in the URL does not match the ID in the request body.");
                return BadRequest();
            }

            try
            {
                _context.Entry(_mapper.Map<ItemResource, Item>(item)).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Item with ID {id} updated successfully.");
                    return NoContent();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError($"Concurrency exception while updating item with ID {id}.", ex);

                    if (!ItemExists(id))
                    {
                        _logger.LogError($"Item with ID {id} not found.");
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Unexpected error occurred during item update.{0}", ex);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error occurred during item update.{0}", ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<ItemResource>> PostItem(ItemResource item)
        {
            _logger.LogInformation($"Received POST request to add item. Request details: {JsonConvert.SerializeObject(item)}");

            var saveEntity = _mapper.Map<ItemResource, Item>(item);
            _context.Items.Add(saveEntity);
            try
            {
                _logger.LogInformation("Creating a new item.");

                await _context.SaveChangesAsync();
                item.Id = saveEntity.Id;

                _logger.LogInformation($"Item with ID {item.Id} created successfully.");

            }
            catch (DbUpdateException ex)
            {
                if (ItemExists(item.Id))
                {
                    _logger.LogError($"Conflict: Item with ID {item.Id} already exists.");
                    return Conflict();
                }
                else
                {
                    _logger.LogError($"Unexpected error occurred during item creation : {ex}");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred during item creation : {ex}");
                throw;
            }

            return CreatedAtAction("GetItem", new { id = saveEntity.Id }, item);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting item with ID {id}");

                var item = await _context.Items.FindAsync(id);

                if (item == null)
                {
                    _logger.LogWarning($"Item with ID {id} not found for deletion.");
                    return NotFound();
                }

                _logger.LogInformation($"Item details before deletion: {JsonConvert.SerializeObject(item)}");

                _context.Items.Remove(item);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Item with ID {id} deleted successfully.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred during item deletion with ID {id}.", ex);
                throw;
            }
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
    }
}
