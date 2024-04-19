using CITPracticum.Data.Migrations;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace CITPracticum.Controllers
{
    public class JobPostingController : Controller
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmployerRepository _employerRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IApplicationRepository _applicationRepository;

        public JobPostingController(IJobPostingRepository jobPostingRepository, UserManager<AppUser> userManager, IEmployerRepository employerRepository, IStudentRepository studentRepository, IApplicationRepository applicationRepository)
        {
            _jobPostingRepository = jobPostingRepository;
            _userManager = userManager;
            _employerRepository = employerRepository;
            _studentRepository = studentRepository;
            _applicationRepository = applicationRepository;
        }
        // displays all job postings on index page
        public async Task<IActionResult> Index()
        {
            ViewData["ActivePage"] = "Jobs";

            IEnumerable<JobPosting> jobPostings = await _jobPostingRepository.GetAll();

            if (User.IsInRole("employer"))
            {
                // Only show the job postings that the employer has posted.
                var user = await _userManager.GetUserAsync(User);
                var emp = await _employerRepository.GetByIdAsync((Int32)user.EmployerId);
                var empJobPostings = new List<JobPosting>();

                foreach (var jp in jobPostings)
                {
                    if (emp.Id == jp.EmployerId)
                    {
                        empJobPostings.Add(jp);
                    }
                }

                return View(empJobPostings);
            }

            return View(jobPostings);
        }

        public async Task<IActionResult> Applicants(int id)
        {
            ViewData["ActivePage"] = "Jobs";

            JobPosting jobPosting = await _jobPostingRepository.GetByIdAsync(id);
            await _applicationRepository.GetAll();
            await _studentRepository.GetAll();
            return View(jobPosting.JobApplications);
        }
        public async Task<IActionResult> ArchivedPosts(int id)
        {
            ViewData["ActivePage"] = "Jobs";

            IEnumerable<JobPosting> allJobPostings = await _jobPostingRepository.GetAll();
            IEnumerable<JobPosting> archivedJobPostings = allJobPostings
                .Where(jp => jp.Archived);

            return View(archivedJobPostings);
        }

        // goes to a specific job posting page
        public async Task<IActionResult> Detail(int id)
        {
            ViewData["ActivePage"] = "Jobs";
            JobPosting jobPosting = await _jobPostingRepository.GetByIdAsync(id);
            return View(jobPosting);
        }
        // creates a new job post
        public IActionResult Create()
        {
            ViewData["ActivePage"] = "Jobs";
            var createJobPostingViewModel = new CreateJobPostingViewModel();
            return View(createJobPostingViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateJobPostingViewModel jobPostingVM)
        {
            ViewData["ActivePage"] = "Jobs";
            if (User.IsInRole("employer"))
            {
                var user = await _userManager.GetUserAsync(User);

                var jobPosting = new JobPosting()
                {
                    JobTitle = jobPostingVM.JobTitle,
                    JobDescription = jobPostingVM.JobDescription,
                    Deadline = jobPostingVM.Deadline,
                    Company = jobPostingVM.Company,
                    Location = jobPostingVM.Location,
                    Link = jobPostingVM.JobLink,
                    EmployerId = user.EmployerId
                };

                TempData["Success"] = "Job posting created successfully.";

                _jobPostingRepository.Add(jobPosting);
                _jobPostingRepository.Save();

                return RedirectToAction("Index");
            }
            
            if (ModelState.IsValid)
            {
                var jobPosting = new JobPosting()
                {
                    JobTitle = jobPostingVM.JobTitle,
                    JobDescription = jobPostingVM.JobDescription,
                    Deadline = jobPostingVM.Deadline,
                    Company = jobPostingVM.Company,
                    Location = jobPostingVM.Location,
                    Link = jobPostingVM.JobLink,
                    EmployerId = jobPostingVM.EmployerId
                };
                _jobPostingRepository.Add(jobPosting);
                _jobPostingRepository.Save();

                TempData["Success"] = "Job posting created successfully.";
                return RedirectToAction("Index");
            }
            return View(jobPostingVM);
        }

        public async Task<IActionResult> ConfirmArchive(int id)
        {
            ViewData["ActivePage"] = "Jobs";

            var post = await _jobPostingRepository.GetByIdAsync(id);

            return View(post);
        }

        public async Task<IActionResult> ConfirmUnarchive(int id)
        {
            ViewData["ActivePage"] = "Jobs";

            var post = await _jobPostingRepository.GetByIdAsync(id);

            return View(post);
        }

        // edit a job post
        public async Task<IActionResult> Edit(int id)
        {
            ViewData["ActivePage"] = "Jobs";
            var jobPosting = await _jobPostingRepository.GetByIdAsync(id);
            if (jobPosting == null) return View("Error");
            var jobPostingVM = new EditJobPostingViewModel()
            {
                JobTitle = jobPosting.JobTitle,
                JobDescription = jobPosting.JobDescription,
                Deadline = jobPosting.Deadline,
                Company = jobPosting.Company,
                Location = jobPosting.Location,
                PaymentCategory = jobPosting.PaymentCategory,
                JobLink = jobPosting.Link
            };
            return View(jobPostingVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditJobPostingViewModel jobPostingVM)
        {
            ViewData["ActivePage"] = "Jobs";
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit JobPosting");
                return View("Edit", jobPostingVM);
            }
            var curJobPosting = await _jobPostingRepository.GetIdAsyncNoTracking(id);

            if (curJobPosting != null)
            {
                if (User.IsInRole("employer"))
                {
                    var user = await _userManager.GetUserAsync(User);
                    var jobPosting = new JobPosting()
                    {
                        Id = id,
                        JobTitle = jobPostingVM.JobTitle,
                        JobDescription = jobPostingVM.JobDescription,
                        Deadline = jobPostingVM.Deadline,
                        Company = jobPostingVM.Company,
                        PaymentCategory = jobPostingVM.PaymentCategory,
                        Location = jobPostingVM.Location,
                        Link = jobPostingVM.JobLink,
                        EmployerId = user.EmployerId
                    };

                    _jobPostingRepository.Update(jobPosting);

                    TempData["Success"] = "Job posting edited successfully.";
                    return RedirectToAction("Index");
                } else
                {
                    var jobPosting = new JobPosting()
                    {
                        Id = id,
                        JobTitle = jobPostingVM.JobTitle,
                        JobDescription = jobPostingVM.JobDescription,
                        Deadline = jobPostingVM.Deadline,
                        Company = jobPostingVM.Company,
                        PaymentCategory = jobPostingVM.PaymentCategory,
                        Location = jobPostingVM.Location,
                        Link = jobPostingVM.JobLink
                    };

                    _jobPostingRepository.Update(jobPosting);

                    TempData["Success"] = "Job posting edited successfully.";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["Error"] = "There was an error editing the job posting.";
                return View(jobPostingVM);
            }
        }
        // deletes a job post
        public async Task<IActionResult> Delete(int id)
        {
            ViewData["ActivePage"] = "Jobs";
            var jobPostingDetails = await _jobPostingRepository.GetByIdAsync(id);
            if (jobPostingDetails == null) return View("Error");
            return View(jobPostingDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteJobPosting(int id)
        {
            ViewData["ActivePage"] = "Jobs";
            var jobPostingDetails = await _jobPostingRepository.GetByIdAsync(id);

            if (jobPostingDetails == null)
            {
                return View("Error");
            }

            _jobPostingRepository.Delete(jobPostingDetails);
            TempData["Success"] = "Job posting deleted successfully.";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Apply(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var jp = await _jobPostingRepository.GetByIdAsync((Int32)id);
            var student = await _studentRepository.GetByIdAsync((Int32)user.StudentId);
            bool alreadyApplied = await _applicationRepository
                .AnyAsync(a => a.StudentId == student.Id && a.JobPostingId == jp.Id);

            if (alreadyApplied)
            {
                TempData["Error"] = "You have already applied to this job posting.";
                return RedirectToAction("Detail", new { id = jp.Id });
            }

            Application app = new Application();

            app.Student = student;
            app.StudentId = student.Id;
            app.JobPosting = jp;
            app.JobPostingId = jp.Id;

            _applicationRepository.Add(app);
            _applicationRepository.Save();

            _jobPostingRepository.Update(jp);
            _jobPostingRepository.Save();

            student.JobApplications.Add(app);
            _studentRepository.Update(student);
            _studentRepository.Save();

            TempData["Success"] = "You have applied to the job posting successfully.";
            return RedirectToAction("Index", "JobPosting");
        }

        public async Task<IActionResult> Archive(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobPosting = await _jobPostingRepository.GetByIdAsync(id.Value);
            jobPosting.Archived = true;
            _jobPostingRepository.Update(jobPosting);

            if (jobPosting == null)
            {
                return NotFound();
            }

            TempData["Success"] = "Job posting archived successfully.";
            return RedirectToAction("Index", "JobPosting"); // Show the archive confirmation view
        }

        public async Task<IActionResult> UnArchive(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobPosting = await _jobPostingRepository.GetByIdAsync(id.Value);
            //Create new post with same information instead of using same post.
            var posting = new JobPosting()
            {
                EmployerId = jobPosting.EmployerId,
                Employer = jobPosting.Employer,
                Archived = false,
                JobApplications = jobPosting.JobApplications,
                Company = jobPosting.Company,
                Deadline = jobPosting.Deadline,
                JobDescription = jobPosting.JobDescription,
                JobTitle = jobPosting.JobTitle,
                Link = jobPosting.Link,
                Location = jobPosting.Location,
                PaymentCategory = jobPosting.PaymentCategory,
            };
            _jobPostingRepository.Add(posting);
            _jobPostingRepository.Save();

            if (jobPosting == null)
            {
                return NotFound();
            }

            TempData["Success"] = "Job posting restored successfully.";
            return RedirectToAction("ArchivedPosts", "JobPosting");
        }

    }
}
