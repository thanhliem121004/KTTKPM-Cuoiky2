using E_commerceTechnologyWebsite.KhoLuuTru;
using E_commerceTechnologyWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace E_commerceTechnologyWebsite.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class ContactController : Controller
    {
        private readonly DataContext _dataContext;
        public ContactController(DataContext context)
        {
            _dataContext = context;
        }

        public IActionResult Index()
        {
            var contacts = _dataContext.Contact.ToList();
            ViewBag.TotalContacts = contacts.Count;
            return View(contacts);
        }
        public async Task<IActionResult> Edit(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return NotFound();
            }

            var contact = await _dataContext.Contact.FindAsync(name);
            if (contact == null)
            {
                return NotFound();
            }
            return View(contact);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string name, ContactModel contactModel)
        {
            if (name != contactModel.Name)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var contact = await _dataContext.Contact.FindAsync(name);
                    if (contact == null)
                    {
                        return NotFound();
                    }

                    contact.Email = contactModel.Email;
                    contact.PhoneNumber = contactModel.PhoneNumber;
                    contact.Description = contactModel.Description;
                    contact.Map = contactModel.Map;

                    if (contactModel.ImageUpload != null)
                    {
                        if (!string.IsNullOrEmpty(contact.Logoimg))
                        {
                            string oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "media", "contacts", contact.Logoimg);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        string uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "media", "contacts");
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + contactModel.ImageUpload.FileName;
                        string filePath = Path.Combine(uploadsDir, uniqueFileName);
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await contactModel.ImageUpload.CopyToAsync(fileStream);
                        }
                        contact.Logoimg = uniqueFileName;
                    }

                    _dataContext.Update(contact);
                    await _dataContext.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactExists(contactModel.Name))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contactModel);
        }

        private bool ContactExists(string name)
        {
            return _dataContext.Contact.Any(e => e.Name == name);
        }
    }
}