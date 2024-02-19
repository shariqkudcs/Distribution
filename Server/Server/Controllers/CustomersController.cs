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
    public class CustomersController : ControllerBase
    {
        private readonly DistributionContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<InventoryController> _logger;

        public CustomersController(DistributionContext context, IMapper mapper, ILogger<InventoryController> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            try
            {
                _logger.LogInformation("Retrieving all customers.");

                var customers = await _context.Customers.ToListAsync();

                _logger.LogInformation($"Number of customers retrieved: {customers.Count}");

                _logger.LogInformation($"Customers details: {JsonConvert.SerializeObject(customers)}");

                return customers;
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error occurred while retrieving customers.", ex);
                throw;
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResource>> GetCustomer(int id)
        {
            try
            {
                _logger.LogInformation($"Retrieving customer with ID {id}");

                var customerEf = await _context.Customers.FindAsync(id);

                if (customerEf == null)
                {
                    _logger.LogWarning($"Customer with ID {id} not found.");
                    return NotFound();
                }

                var customerResource = _mapper.Map<Customer, CustomerResource>(customerEf);
                _logger.LogInformation($"Customer details: {JsonConvert.SerializeObject(customerResource)}");

                return Ok(customerResource);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred while retrieving customer with ID {id}.", ex);
                throw;
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, CustomerResource customer)
        {
            try
            {
                _logger.LogInformation($"Updating customer with ID {id}. Request details: {JsonConvert.SerializeObject(customer)}");

                if (id != customer.Id)
                {
                    _logger.LogError("Bad request: ID in the URL does not match the ID in the request body.");
                    return BadRequest();
                }

                _context.Entry(_mapper.Map<CustomerResource, Customer>(customer)).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();

                    _logger.LogInformation($"Customer with ID {id} updated successfully.");
                    return NoContent();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    _logger.LogError($"Concurrency exception while updating customer with ID {id}.", ex);

                    if (!CustomerExists(id))
                    {
                        _logger.LogError($"Customer with ID {id} not found.");
                        return NotFound();
                    }
                    else
                    {
                        _logger.LogError("Unexpected error occurred during customer update.", ex);
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error occurred during customer update.", ex);
                throw;
            }
        }

        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(CustomerResource customer)
        {
            try
            {
                _logger.LogInformation("Creating a new customer. Request details: {JsonConvert.SerializeObject(customer)}");

                _logger.LogInformation($"Customer details: {JsonConvert.SerializeObject(customer)}");

                _context.Customers.Add(_mapper.Map<CustomerResource, Customer>(customer));
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Customer with ID {customer.Id} created successfully.");

                return CreatedAtAction("GetCustomer", new { id = customer.Id }, customer);
            }
            catch (DbUpdateException)
            {
                if (CustomerExists(customer.Id))
                {
                    _logger.LogError($"Conflict: Customer with ID {customer.Id} already exists.");
                    return Conflict();
                }
                else
                {
                    _logger.LogError("Unexpected error occurred during customer creation.");
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error occurred during customer creation.", ex);
                throw;
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting customer with ID {id}");

                var customer = await _context.Customers.FindAsync(id);

                if (customer == null)
                {
                    _logger.LogWarning($"Customer with ID {id} not found for deletion.");
                    return NotFound();
                }

                _logger.LogInformation($"Customer details before deletion: {JsonConvert.SerializeObject(customer)}");

                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"Customer with ID {id} deleted successfully.");

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error occurred during customer deletion with ID {id}.", ex);
                throw;
            }
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
