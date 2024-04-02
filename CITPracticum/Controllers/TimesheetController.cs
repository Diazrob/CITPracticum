using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.Repository;
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

        public TimesheetController(IPlacementRepository placementRepository, UserManager<AppUser> userManager, IStudentRepository studentRepository)
        {
            _placementRepository = placementRepository;
            _userManager = userManager;
            _studentRepository = studentRepository;
        }
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Timesheets";

            return View();
        }
        public async Task<IActionResult> ViewTimesheet()
        {
            ViewData["ActivePage"] = "Timesheets";

            if (User.IsInRole("student"))
            {
                var usr = await _userManager.GetUserAsync(User);
                int stuId = Convert.ToInt32(usr.StudentId);
                var student = await _studentRepository.GetByIdAsync(stuId);
                var usrLastName = student.LastName;
                var usrFirstName = student.FirstName;
                var usrStuId = student.StuId;
                var usrEmail = student.StuEmail;

                var placementVM = new Placement()
                {
                    Student = new Student
                    {
                        FirstName = usrFirstName,
                        LastName = usrLastName,
                        StuId = usrStuId,
                        StuEmail = usrEmail,
                    },
                    Timesheet = new Timesheet()
                    {
                        TimeEntries = new List<TimeEntry>
                        {
                            new TimeEntry()
                            {
                                //Change to real data, just using for testing atm
                                ShiftDate = new DateTime(2023, 4, 5),
                                Hours = (decimal)4.75,
                                Description = "Helped take out the trash.",
                            },
                            new TimeEntry()
                            {
                                //Change to real data, just using for testing atm
                                ShiftDate = new DateTime(2024, 04, 1),
                                Hours = (decimal)7.4,
                                Description = "Helped take out the trash.",
                            },
                            new TimeEntry()
                            {
                                //Change to real data, just using for testing atm
                                ShiftDate = new DateTime(2023, 5, 20),
                                Hours = (decimal)3.88,
                                Description = "Helped take out the trash.",
                            },
                        }
                    }
                };
                foreach (var entry in placementVM.Timesheet.TimeEntries)
                {
                    entry.HoursToDate = TotalHoursToEntryDate(placementVM.Timesheet, entry);
                }
                return View(placementVM);
            }
            else
            {
                return View();
            }
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
                if (entry.ShiftDate <= currentEntry.ShiftDate)
                {
                    totalCountToDate += entry.Hours;
                }
            }

            return totalCountToDate;
        }

        public IActionResult CreateTimeEntry()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitTimeEntry(CreateTimeEntryViewModel te)
        {
            ViewData["ActivePage"] = "Timesheets";

            if (User.IsInRole("student"))
            {
                var usr = await _userManager.GetUserAsync(User);
                int stuId = Convert.ToInt32(usr.StudentId);

                var placementVM = await _placementRepository.GetAll();
                var assignedPlacement = new Placement();

                foreach (var placement in placementVM)
                {
                    if (placement.StudentId == usr.StudentId)
                    {
                        assignedPlacement = placement;
                    }
                }

                if (assignedPlacement.Timesheet == null)
                {
                    assignedPlacement.Timesheet = new Timesheet();
                }
                if (assignedPlacement.Timesheet.TimeEntries == null)
                {
                    assignedPlacement.Timesheet.TimeEntries = new List<TimeEntry>();
                }

                foreach (var entry in assignedPlacement.Timesheet.TimeEntries)
                {
                    entry.HoursToDate = TotalHoursToEntryDate(assignedPlacement.Timesheet, entry);
                }

                assignedPlacement.Timesheet.TimeEntries.Add(te.NewTimeEntry);

                _placementRepository.Update(assignedPlacement);
                return RedirectToAction("ViewTimesheet", "Timesheet");
            }
            else
            {
                return View();
            }
        }
        public IActionResult Timestamp()
        {
            return View();
        }
    }
}
