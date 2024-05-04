using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
        
        // Main timesheet page, list of students with placements
        public async Task<IActionResult> Index(string sortOrder, string nameFilter)
        {
            // Set the page name for breadcrumbs
            ViewData["ActivePage"] = "Timesheets";

            // Sort view variable
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            // Filter view variable
            ViewData["CurrentNameFilter"] = nameFilter;


            // Grab all placements
            var placements = await _placementRepository.GetAll();

            // Employer logic
            if (User.IsInRole("employer"))
            {
                // Grab current user, employer, and make a list of placements
                var usr = await _userManager.GetUserAsync(User);
                var emp = await _employerRepository.GetByIdAsync((Int32)usr.EmployerId);
                var empPlacements = new List<Placement>();

                // Loop logic for setting the timesheet data to their placements
                foreach (var placement in placements)
                {
                    // Check for all users that belong to this employer
                    if (emp.Id != placement.EmployerId)
                    {
                        // The employer does not have this student attached to their placement
                        continue;
                    }
                    else
                    {
                        // Enter the placement to the return list if they belong to the placement
                        empPlacements.Add(placement);
                    }

                    // Grab student attached to the placement
                    var student = await _studentRepository.GetByIdAsync((Int32)placement.StudentId);
                    placement.Student = student;
                    
                    // Check if there is a timesheet on the placement
                    if (placement.TimesheetId == null || placement.TimesheetId == 0)
                    {
                        // If the timesheet is null, skip the current placement
                        continue;
                    }
                    // Check if timesheet was found, but no student
                    if (placement.StudentId == null || placement.StudentId == 0)
                    {
                        // No student attached to placement with timesheet
                        // Return error
                    }

                    // Grab timesheet from placement
                    var timesheet = await _timesheetRepository.GetByIdAsync((Int32)placement.TimesheetId);
                    await _timeEntryRepository.GetAll();

                    // Fill the timesheet information to the placement
                    placement.Timesheet = timesheet;
                }

                // Apply filters
                if (!string.IsNullOrEmpty(nameFilter))
                {
                    empPlacements = empPlacements.Where(u => u.Student.FirstName.ToUpper().Contains(nameFilter.ToUpper())).ToList();
                }

                // Apply sorting
                switch (sortOrder)
                {
                    case "name_desc":
                        empPlacements = empPlacements.OrderByDescending(u => u.Student.FirstName).ToList();
                        break;
                    default:
                        empPlacements = empPlacements.OrderBy(u => u.Student.FirstName).ToList();
                        break;
                }

                // Return employer assigned placements
                return View(empPlacements);
            }

            // Admin logic
            foreach (var placement in placements)
            {
                // Grab student
                var student = await _studentRepository.GetByIdAsync((Int32)placement.StudentId);
                placement.Student = student;
                
                // Check if timesheet was found
                if (placement.TimesheetId == null || placement.TimesheetId == 0)
                {
                    continue;
                }
                if (placement.StudentId == null || placement.StudentId == 0)
                {
                    // No student attached to placement
                    // Return error
                }

                // Grab timesheet, and fill info into placement
                var timesheet = await _timesheetRepository.GetByIdAsync((Int32)placement.TimesheetId);
                await _timeEntryRepository.GetAll();
                placement.Timesheet = timesheet;
            }

            // Apply filters
            if (!string.IsNullOrEmpty(nameFilter))
            {
                placements = placements.Where(u => u.Student.FirstName.ToUpper().Contains(nameFilter.ToUpper()));
            }

            // Apply sorting
            switch (sortOrder)
            {
                case "name_desc":
                    placements = placements.OrderByDescending(u => u.Student.FirstName);
                    break;
                default:
                    placements = placements.OrderBy(u => u.Student.FirstName);
                    break;
            }
            
            // Return all placements for admin view
            return View(placements.ToList());
        }

        // View time entries for a selected student
        public async Task<IActionResult> ViewTimesheet(int? id, string sortOrder, DateTime? dateFilter)
        {
            // Set the page name for breadcrumbs
            ViewData["ActivePage"] = "Timesheets";
            // Sorting view variable
            ViewData["DateSortParm"] = String.IsNullOrEmpty(sortOrder) ? "date_desc" : "";
            // Filter view variable
            ViewData["DateFilter"] = dateFilter;

            // Admin and employe logic
            if (User.IsInRole("admin") || User.IsInRole("employer"))
            {
                // Grab placement, and its students information
                var placement = await _placementRepository.GetByIdAsync((Int32)id);
                var student = await _studentRepository.GetByIdAsync((Int32)placement.StudentId);
                placement.Student = student;

                // Grab the timesheet and fill the placement with its information
                Timesheet timesheet = await _timesheetRepository.GetByIdAsync((Int32)placement.TimesheetId);
                await _timeEntryRepository.GetAll();
                placement.Timesheet = timesheet;

                // Apply filters
                if (dateFilter != null)
                {
                    timesheet.TimeEntries = timesheet.TimeEntries
                            .Where(entry => entry.ShiftDate.Date == dateFilter.Value.Date)
                            .ToList();
                }

                // Apply sorting
                switch (sortOrder)
                {
                    case "date_desc":
                        timesheet.TimeEntries = timesheet.TimeEntries.OrderByDescending(u => u.ShiftDate).ToList();
                        break;
                    default:
                        timesheet.TimeEntries = timesheet.TimeEntries.OrderBy(u => u.ShiftDate).ToList();
                        break;
                }

                return View(placement);
            }

            // Student logic, they can only see their own entries
            if (User.IsInRole("student"))
            {
                // Grab the logged in user and the student account
                var usr = await _userManager.GetUserAsync(User);
                var student = await _studentRepository.GetByIdAsync((Int32)usr.StudentId);
                // Create variable for timesheet data
                var timesheet = new Timesheet();

                // Grab all placements
                var placements = await _placementRepository.GetAll();
                // Create variable for their assigned placement
                var assignedPlacement = new Placement();

                // Loop to find the students placement
                foreach (var placement in placements)
                {
                    if (placement.StudentId == usr.StudentId)
                    {
                        assignedPlacement = placement;
                        break;
                    }
                }

                // Fill the placement with student information
                assignedPlacement.Student = student;
                // If there was a placement found, assign timesheet variable with the present data
                if (assignedPlacement.TimesheetId != null)
                {
                    timesheet = await _timesheetRepository.GetByIdAsync((Int32)assignedPlacement.TimesheetId);
                }

                // Grab all time entry information
                var timeEntries = await _timeEntryRepository.GetAll();

                // If there are no time entries on the timesheet
                if (timesheet.TimeEntries == null)
                {
                    // Assign the proper time entries to the timesheet
                    foreach (var entry in timeEntries)
                    {
                        if (timesheet.Id == entry.TimesheetId)
                        {
                            timesheet.TimeEntries.Add(entry);
                        }
                    }
                }

                // Apply filters
                if (dateFilter != null)
                {
                    timesheet.TimeEntries = timesheet.TimeEntries
                            .Where(entry => entry.ShiftDate.Date == dateFilter.Value.Date)
                            .ToList();
                }

                // Apply sorting
                switch (sortOrder)
                {
                    case "date_desc":
                        timesheet.TimeEntries = timesheet.TimeEntries.OrderByDescending(u => u.ShiftDate).ToList();
                        break;
                    default:
                        timesheet.TimeEntries = timesheet.TimeEntries.OrderBy(u => u.ShiftDate).ToList();
                        break;
                }

                // Final assigning of timesheet
                assignedPlacement.Timesheet = timesheet;

                // Return the student placement to their view
                return View(assignedPlacement);
            }

            return View();
        }

        // Internal method to ensure that the total hours are correct on each entry.
        public decimal TotalHoursToEntryDate(Timesheet ts, TimeEntry currentEntry)
        {
            // Check if entered information is valid
            if (ts == null || currentEntry == null)
            {
                return 0;
            }
            
            // Initialize to zero
            decimal totalCountToDate = 0;

            // Loop to ensure the hours field is accurate
            foreach (var entry in ts.TimeEntries)
            {
                // Check if the entry date is less than or equal to the current entry's date
                if (entry.ShiftDate <= currentEntry.ShiftDate && entry.ApprovalCategory == Data.Enum.ApprovalCategory.Yes)
                {
                    // Add the current value plus the new value
                    totalCountToDate += entry.Hours;
                }
            }

            // Return the total hours that were calculated
            return totalCountToDate;
        }

        // Create time entry logic
        public async Task<IActionResult> CreateTimeEntry(int? id)
        {
            // Check if input was valid
            if (id != null)
            {
                // Grab current user
                var usr = await _userManager.GetUserAsync(User);
                // Initialize the view model
                CreateTimeEntryViewModel vm = new CreateTimeEntryViewModel();
                // Grab the assigned placement
                var placement = await _placementRepository.GetByIdAsync((Int32)id);
                
                // Student logic
                if (User.IsInRole("student"))
                {
                    if (usr.StudentId != placement.StudentId)
                    {
                        //This prevents students from adding time entries to other students timesheets.
                        ViewData["Error"] = "You do not have access to that page.";
                        return RedirectToAction("ViewTimesheet", new { id = usr.StudentId });
                    }
                }

                // Grab the student on the placement
                placement.Student = await _studentRepository.GetByIdAsync((Int32)placement.StudentId);

                // Check if there is a timesheet on the placement
                if (placement.TimesheetId == null)
                {
                    // Create new timesheet if there is no timesheet
                    placement.Timesheet = new Timesheet();
                    _timesheetRepository.Add(placement.Timesheet);
                    _timesheetRepository.Save();
                    // Assign placement timesheet to newly created timesheet
                    placement.TimesheetId = placement.Timesheet.Id;
                    _placementRepository.Update(placement);
                    _placementRepository.Save();
                }
                else
                {
                    // Grab placement timesheet
                    placement.Timesheet = await _timesheetRepository.GetByIdAsync((Int32)placement.TimesheetId);
                }

                // Grab all time entries
                await _timeEntryRepository.GetAll();

                // Put information into the viewmodel
                vm.Placement = placement;

                // Return the view model to the user
                return View(vm);
            }
            else
            {
                // Display error message
                ViewData["Error"] = "The given id was not found in the system.";
                return RedirectToAction("ViewTimesheet");
            }
        }

        // Modal logic, this sends the user to the correct area based on the modal they are opening.
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

        // Approve and Deny time entry logic
        [HttpGet("Timesheet/ViewTimesheet/{id}")]
        private async Task<IActionResult> ApproveDeny(int id, List<int> timeEntryIds, string actionType)
        {
            // Loop that deals with the selected time entries
            foreach (var entryId in timeEntryIds)
            {
                // Find the time entry
                var entry = await _timeEntryRepository.GetByIdAsync(entryId);
                // Find the timesheet that belongs to the entry
                var timesheet = await _timesheetRepository.GetByIdAsync(entry.TimesheetId);
                await _timeEntryRepository.GetAll();

                var placements = await _placementRepository.GetAll();
                var usr = await _userManager.GetUserAsync(User);

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