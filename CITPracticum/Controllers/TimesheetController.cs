using CITPracticum.Data;
using CITPracticum.Data.Migrations;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.Repository;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CITPracticum.Controllers
{
    public class TimesheetController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IPlacementRepository _placementRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly ITimesheetRepository _timesheetRepository;
        private readonly ITimeEntryRepository _timeEntryRepository;
        private readonly IEmployerRepository _employerRepository;

        public TimesheetController(IPlacementRepository placementRepository, UserManager<AppUser> userManager, IStudentRepository studentRepository, ITimesheetRepository timesheetRepository, ITimeEntryRepository timeEntryRepository, IEmployerRepository employerRepository)
        {
            _placementRepository = placementRepository;
            _userManager = userManager;
            _studentRepository = studentRepository;
            _timesheetRepository = timesheetRepository;
            _timeEntryRepository = timeEntryRepository;
            _employerRepository = employerRepository;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Timesheets";

            var placements = await _placementRepository.GetAll();

            if (User.IsInRole("employer"))
            {
                var usr = await _userManager.GetUserAsync(User);
                var emp = await _employerRepository.GetByIdAsync((Int32)usr.EmployerId);
                var empPlacements = new List<Placement>();

                foreach (var placement in placements)
                {
                    if (emp.Id != placement.EmployerId)
                    {
                        //The employer does not have this student attached to their placement
                        continue;
                    }
                    else
                    {
                        empPlacements.Add(placement);
                    }

                    var student = await _studentRepository.GetByIdAsync((Int32)placement.StudentId);
                    placement.Student = student;
                    if (placement.TimesheetId == null || placement.TimesheetId == 0)
                    {
                        continue;
                    }
                    if (placement.StudentId == null || placement.StudentId == 0)
                    {
                        // No student attached to placement
                        // Return error
                    }

                    var timesheet = await _timesheetRepository.GetByIdAsync((Int32)placement.TimesheetId);
                    await _timeEntryRepository.GetAll();

                    placement.Timesheet = timesheet;
                }

                return View(empPlacements);
            }

            foreach (var placement in placements)
            {
                var student = await _studentRepository.GetByIdAsync((Int32)placement.StudentId);
                placement.Student = student;
                if (placement.TimesheetId == null || placement.TimesheetId == 0)
                {
                    continue;
                }
                if (placement.StudentId == null || placement.StudentId == 0)
                {
                    // No student attached to placement
                    // Return error
                }

                var timesheet = await _timesheetRepository.GetByIdAsync((Int32)placement.TimesheetId);
                await _timeEntryRepository.GetAll();

                placement.Timesheet = timesheet;
            }

            return View(placements);
        }
        public async Task<IActionResult> ViewTimesheet(int? id)
        {
            ViewData["ActivePage"] = "Timesheets";

            if (!User.IsInRole("student"))
            {
                var placement = await _placementRepository.GetByIdAsync((Int32)id);
                var student = await _studentRepository.GetByIdAsync((Int32)placement.StudentId);
                placement.Student = student;
                Timesheet timesheet = await _timesheetRepository.GetByIdAsync((Int32)placement.TimesheetId);
                await _timeEntryRepository.GetAll();
                placement.Timesheet = timesheet;

                return View(placement);
            }

            if (User.IsInRole("student"))
            {
                var usr = await _userManager.GetUserAsync(User);
                int stuId = Convert.ToInt32(usr.StudentId);
                var student = await _studentRepository.GetByIdAsync(stuId);
                var timesheet = new Timesheet();

                var placements = await _placementRepository.GetAll();
                var assignedPlacement = new Placement();

                foreach (var placement in placements)
                {
                    if (placement.StudentId == usr.StudentId)
                    {
                        assignedPlacement = placement;
                        break;
                    }
                }

                assignedPlacement.Student = student;
                if (assignedPlacement.TimesheetId != null)
                {
                    timesheet = await _timesheetRepository.GetByIdAsync((Int32)assignedPlacement.TimesheetId);
                }

                var timeEntries = await _timeEntryRepository.GetAll();

                if (timesheet.TimeEntries == null)
                {
                    foreach (var entry in timeEntries)
                    {
                        if (timesheet.Id == entry.TimesheetId)
                        {
                            timesheet.TimeEntries.Add(entry);
                        }
                    }
                }

                assignedPlacement.Timesheet = timesheet;

                return View(assignedPlacement);
            }

            return View();
        }

        public decimal TotalHoursToEntryDate(Timesheet ts, TimeEntry currentEntry)
        {
            if (ts == null || currentEntry == null)
            {
                return 0;
            }

            decimal totalCountToDate = 0;
            foreach (var entry in ts.TimeEntries)
            {
                // Check if the entry date is less than or equal to the current entry's date
                if (entry.ShiftDate <= currentEntry.ShiftDate && entry.ApprovalCategory == Data.Enum.ApprovalCategory.Yes)
                {
                    totalCountToDate += entry.Hours;
                }
            }

            return totalCountToDate;
        }

        public async Task<IActionResult> CreateTimeEntry(int? id)
        {
            if (id != null)
            {
                var usr = await _userManager.GetUserAsync(User);
                CreateTimeEntryViewModel vm = new CreateTimeEntryViewModel();
                var placement = await _placementRepository.GetByIdAsync((Int32)id);
                if (User.IsInRole("student"))
                {
                    if (usr.StudentId != placement.StudentId)
                    {
                        //This prevents students from adding time entries to other students timesheets.
                        return NotFound();
                    }
                }
                placement.Student = await _studentRepository.GetByIdAsync((Int32)placement.StudentId);

                if (placement.TimesheetId == null)
                {
                    //Create new timesheet if there is no timesheet
                    placement.Timesheet = new Timesheet();
                    _timesheetRepository.Add(placement.Timesheet);
                    _timesheetRepository.Save();
                    //Assign placement timesheet to newly created timesheet
                    placement.TimesheetId = placement.Timesheet.Id;
                    _placementRepository.Update(placement);
                    _placementRepository.Save();
                }
                else
                {
                    placement.Timesheet = await _timesheetRepository.GetByIdAsync((Int32)placement.TimesheetId);
                }

                await _timeEntryRepository.GetAll();

                vm.Placement = placement;
                return View(vm);
            }
            else
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<IActionResult> ProcessEntries(List<int>? timeEntryIds, string actionType, int id)
        {
            switch (actionType)
            {
                case "approve":
                    return await ApproveDeny(id, timeEntryIds, actionType);
                case "deny":
                    return await ApproveDeny(id, timeEntryIds, actionType);
                case "delete":
                    return await DeleteEntries(id, timeEntryIds);
                default:
                    return View();
            }
        }

        [HttpGet("Timesheet/ViewTimesheet/{id}")]
        private async Task<IActionResult> ApproveDeny(int id, List<int> timeEntryIds, string actionType)
        {
            foreach (var entryId in timeEntryIds)
            {
                var entry = await _timeEntryRepository.GetByIdAsync(entryId);
                var timesheet = await _timesheetRepository.GetByIdAsync(entry.TimesheetId);
                await _timeEntryRepository.GetAll();

                var placements = await _placementRepository.GetAll();
                var assignedPlacement = new Placement();
                var usr = await _userManager.GetUserAsync(User);

                foreach (var placement in placements)
                {
                    if (placement.StudentId == usr.StudentId)
                    {
                        assignedPlacement = placement;
                        break;
                    }
                }

                if (actionType == "approve")
                {
                    entry.ApprovalCategory = Data.Enum.ApprovalCategory.Yes;
                }
                else if (actionType == "deny")
                {
                    entry.ApprovalCategory = Data.Enum.ApprovalCategory.No;
                }

                foreach (var item in timesheet.TimeEntries)
                {
                    item.HoursToDate = TotalHoursToEntryDate(timesheet, item);
                    _timeEntryRepository.Update(entry);
                    _timeEntryRepository.Save();
                }

                _timeEntryRepository.Update(entry);
                _timeEntryRepository.Save();
            }

            return RedirectToAction("ViewTimesheet", new { id = id });
        }

        [HttpGet("Timesheet/ViewTimesheet/{id}")]
        private async Task<IActionResult> DeleteEntries(int id, List<int> timeEntryIds)
        {
            var usr = await _userManager.GetUserAsync(User);
            var placement = await _placementRepository.GetByIdAsync(id);
            var timesheet = await _timesheetRepository.GetByIdAsync((Int32)placement.TimesheetId);
            await _timeEntryRepository.GetAll();
            if (User.IsInRole("student"))
            {
                if (usr.StudentId != placement.StudentId)
                {
                    //Prevent students from deleting other students entries
                    return NotFound();
                }
            }
            foreach (var entryId in timeEntryIds)
            {
                var entry = await _timeEntryRepository.GetByIdAsync(entryId);
                _timeEntryRepository.Delete(entry);
                _timeEntryRepository.Save();
            }

            if (User.IsInRole("admin"))
            {
                //Update all of the 
                foreach (var entry in timesheet.TimeEntries)
                {
                    entry.HoursToDate = TotalHoursToEntryDate(timesheet, entry);
                    _timeEntryRepository.Update(entry);
                    _timeEntryRepository.Save();
                }
            }

            return RedirectToAction("ViewTimesheet", new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> SubmitTimeEntry(CreateTimeEntryViewModel te, int? id)
        {
            ViewData["ActivePage"] = "Timesheets";

            if (User.IsInRole("admin") || User.IsInRole("employer"))
            {
                var placement = await _placementRepository.GetByIdAsync((Int32)id);
                if (placement != null)
                {
                    var timesheet = await _timesheetRepository.GetByIdAsync((Int32)placement.TimesheetId);
                    if (timesheet != null)
                    {
                        await _timeEntryRepository.GetAll();
                        if (timesheet.TimeEntries == null)
                        {
                            // Create a list of entries if there are currently none associated with the user.
                            timesheet.TimeEntries = new List<TimeEntry>();
                        }
                        te.NewTimeEntry.ApprovalCategory = Data.Enum.ApprovalCategory.Yes;
                        te.NewTimeEntry.TimesheetId = timesheet.Id;
                        te.NewTimeEntry.Timesheet = timesheet;

                        _timeEntryRepository.Add(te.NewTimeEntry);
                        _timeEntryRepository.Save();
                        foreach (var entry in timesheet.TimeEntries)
                        {
                            entry.HoursToDate = TotalHoursToEntryDate(placement.Timesheet, entry);
                            _timeEntryRepository.Update(entry);
                            _timeEntryRepository.Save();
                        }
                    }
                }

                return RedirectToAction("ViewTimesheet", "Timesheet", new { id = id });
            }

            if (User.IsInRole("student"))
            {
                var usr = await _userManager.GetUserAsync(User);

                var placementVM = await _placementRepository.GetAll();
                var assignedPlacement = new Placement();

                foreach (var placement in placementVM)
                {
                    if (placement.StudentId == usr.StudentId)
                    {
                        assignedPlacement = placement;
                        break;
                    }
                }

                if (assignedPlacement.TimesheetId == null)
                {
                    //Create new timesheet if there is no timesheet
                    assignedPlacement.Timesheet = new Timesheet();
                    _timesheetRepository.Add(assignedPlacement.Timesheet);
                    _timesheetRepository.Save();
                    //Assign placement timesheet to newly created timesheet
                    assignedPlacement.TimesheetId = assignedPlacement.Timesheet.Id;
                    _placementRepository.Update(assignedPlacement);
                    _placementRepository.Save();
                }

                if (assignedPlacement.TimesheetId != null)
                {
                    assignedPlacement.Timesheet = await _timesheetRepository.GetByIdAsync((Int32)assignedPlacement.TimesheetId);
                }

                //If not null, add time entry
                if (te.NewTimeEntry != null && assignedPlacement.Timesheet != null && assignedPlacement.TimesheetId != 0)
                {
                    te.NewTimeEntry.TimesheetId = assignedPlacement.Timesheet.Id;
                    te.NewTimeEntry.ApprovalCategory = Data.Enum.ApprovalCategory.InProgress;
                    _timeEntryRepository.Add(te.NewTimeEntry);
                    _timeEntryRepository.Save();
                }

                if (assignedPlacement.Timesheet.TimeEntries == null)
                {
                    assignedPlacement.Timesheet.TimeEntries = new List<TimeEntry>();
                }

                //Add entry to timesheet, and update list.
                _timesheetRepository.Update(assignedPlacement.Timesheet);
                _timesheetRepository.Save();

                return RedirectToAction("ViewTimesheet", "Timesheet");
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> EditTimeEntry(int entryId)
        {
            var timeEntry = await _timeEntryRepository.GetByIdAsync(entryId);

            if (User.IsInRole("student"))
            {
                //Ensures that students can only edit their own entries.
                var usr = await _userManager.GetUserAsync(User);
                var placement = await _placementRepository.GetByIdAsync((Int32)usr.StudentId);
                var timesheet = await _timesheetRepository.GetByIdAsync((Int32)placement.TimesheetId);
                await _timeEntryRepository.GetAll();
                var entry = await _timeEntryRepository.GetByIdAsync(entryId);
                if (!timesheet.TimeEntries.Contains(entry))
                {
                    return NotFound();
                }
            }

            return View(timeEntry);
        }
        public async Task<IActionResult> EditTE(TimeEntry te)
        {
            var placements = await _placementRepository.GetAll();
            var assignedPlacement = new Placement();

            foreach (var placement in placements)
            {
                if (placement.TimesheetId == te.TimesheetId)
                {
                    assignedPlacement = placement;
                    break;
                }
            }

            _timeEntryRepository.Update(te);
            _timeEntryRepository.Save();

            if (User.IsInRole("admin"))
            {
                var timesheet = await _timesheetRepository.GetByIdAsync((Int32)te.TimesheetId);
                await _timeEntryRepository.GetAll();
                //Update all of the 
                foreach (var entry in timesheet.TimeEntries)
                {
                    entry.HoursToDate = TotalHoursToEntryDate(timesheet, entry);
                    _timeEntryRepository.Update(entry);
                    _timeEntryRepository.Save();
                }
            }

            return RedirectToAction("ViewTimesheet", "Timesheet", new { id = assignedPlacement.Id });
        }

        public IActionResult Timestamp()
        {
            return View();
        }
    }
}