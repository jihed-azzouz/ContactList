using ContactList.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ContactList.Context;
using ContactList.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Linq;
using ContactList.Models;

public class ContactsController : Controller
{
    private readonly ContactContext _context;
    private readonly ILogger<ContactsController> _logger;

    public ContactsController(ContactContext context, ILogger<ContactsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Contacts
    public async Task<IActionResult> Index()
    {
        var Contacts = await _context.Contacts
                                 .Select(c => new ContactVM
                                 {
                                     Id = c.Id,
                                     FirstName = c.FirstName,
                                     LastName = c.LastName,
                                     PhoneNumber = c.PhoneNumber,
                                     EmailAddress = c.EmailAddress,
                                     CatName = _context.Categories.FirstOrDefault(cat => cat.Id == c.CategoryId).Name
                                 })
                                 .ToListAsync();
        return View(Contacts);
    }

    // GET: Contacts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("Details action called with a null ID.");
            return NotFound();
        }

        var ContactVM = await _context.Contacts
            .Where(c => c.Id == id)
            .Select(c => new ContactVM
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                EmailAddress = c.EmailAddress,
                CatName = _context.Categories.FirstOrDefault(cat => cat.Id == c.CategoryId).Name,
                Date = c.Date
            })
            .FirstOrDefaultAsync();

        if (ContactVM == null)
        {
            _logger.LogWarning($"Details action called with a non-existing Contacts ID: {id}");
            return NotFound();
        }

        return View(ContactVM);
    }

    // GET: Contacts/Create
    public IActionResult Create()
    {
        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
        return View("CreateEdit", new Contact());
    }

    // POST: Contacts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,PhoneNumber,EmailAddress,CategoryId")] Contact Contacts)
    {
        // Remove ModelState errors related to Category
        ModelState.Remove("Category");

        if (ModelState.IsValid)
        {
            Contacts.Date = DateTime.Now;

            _context.Add(Contacts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        else
        {
            // If ModelState is not valid, repopulate CategoryId SelectList and return to view
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", Contacts.CategoryId);
            return View("CreateEdit", Contacts);
        }
    }


    // GET: Contacts/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("Edit action called with a null ID.");
            return NotFound();
        }

        var Contacts = await _context.Contacts.FindAsync(id);
        if (Contacts == null)
        {
            _logger.LogWarning($"Edit action called with a non-existing Contacts ID: {id}");
            return NotFound();
        }

        ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", Contacts.CategoryId);
        return View("CreateEdit", Contacts);
    }



    // POST: Contacts/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,PhoneNumber,EmailAddress,CategoryId")] Contact Contacts)
    {
        if (id != Contacts.Id)
        {
            _logger.LogWarning($"Edit action called with mismatched ID: {id} and Contacts ID: {Contacts.Id}");
            return NotFound();
        }

        // Remove ModelState errors related to Category
        ModelState.Remove("Category");

        if (ModelState.IsValid)
        {
            var existingContacts = await _context.Contacts.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (existingContacts == null)
            {
                _logger.LogWarning($"Contacts with ID: {id} not found for editing.");
                return NotFound();
            }

            Contacts.Date = existingContacts.Date;

            _context.Update(Contacts);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        else
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", Contacts.CategoryId);
            return View("CreateEdit", Contacts);
        }
    }



    // GET: Contacts/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            _logger.LogWarning("Delete action called with a null ID.");
            return NotFound();
        }

        var ContactVM = await _context.Contacts
            .Where(c => c.Id == id)
            .Select(c => new ContactVM
            {
                Id = c.Id,
                FirstName = c.FirstName,
                LastName = c.LastName,
                PhoneNumber = c.PhoneNumber,
                EmailAddress = c.EmailAddress,
                CatName = _context.Categories.FirstOrDefault(cat => cat.Id == c.CategoryId).Name
            })
            .FirstOrDefaultAsync();

        if (ContactVM == null)
        {
            _logger.LogWarning($"Delete action called with a non-existing Contacts ID: {id}");
            return NotFound();
        }

        return View(ContactVM);
    }

    // POST: Contacts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var Contacts = await _context.Contacts.FindAsync(id);
        if (Contacts == null)
        {
            _logger.LogWarning($"DeleteConfirmed action called with a non-existing Contacts ID: {id}");
            return NotFound();
        }

        _context.Contacts.Remove(Contacts);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool ContactsExists(int id)
    {
        return _context.Contacts.Any(e => e.Id == id);
    }
}
