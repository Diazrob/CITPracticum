using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class JobPostingController : Controller
    {
        private readonly IJobPostingRepository _jobPostingRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmployerRepository _employerRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IApplicationRepository _applicationRepository;

        // Job Posting Controller
        public JobPostingController(IJobPostingRepository jobPostingRepository, UserManager<AppUser> userManager, IEmployerRepository employerRepository, IStudentRepository studentRepository, IApplicationRepository applicationRepository)
        {
            _jobPostingRepository = jobPostingRepository;
            _userManager = userManager;
            _employerRepository = employerRepository;
            _studentRepository = studentRepository;
            _applicationRepository = applicationRepository;
        }
        // Displays all job postings on index page
        public async Task<IActionResult> Index(int page = 1, int pageSize = 6)
        {
            // Sets the page name for breadcrumbs
            ViewData["ActivePage"] = "Jobs";

            // Fetch all job postings, and non archived postings.
            IEnumerable<JobPosting> jobPostings = await _jobPostingRepository.GetAll();
            IEnumerable<JobPosting> nonArchived = jobPostings.Where(jp => jp.Archived == false).ToList();

            // Employer logic
            if (User.IsInRole("employer"))
            {
                // Grab current logged in user
                var user = await _userManager.GetUserAsync(User);
                // Grab active postings that are associated with the employer
                nonArchived = jobPostings.Where(jp => jp.Archived == false && jp.EmployerId == user.EmployerId).ToList();
            }

            // Pagination logic
            var count = nonArchived.Count();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            var currentPage = page;
            var skip = (page - 1) * pageSize;

            // Valid postings according to the page, and page size.
            var paginatedPostings = nonArchived.Skip(skip).Take(pageSize).ToList();

            // Set view variables for current, and total pages.
            ViewBag.PageNum = currentPage;
            ViewBag.TotalPages = totalPages;

            // Employer logic
            if (User.IsInRole("employer"))
            {
                // Grab user and employer
                var user = await _userManager.GetUserAsync(User);
                var emp = await _employerRepository.GetByIdAsync((int)user.EmployerId);

                // Grab postings that are associated with the employer
                var empJobPostings = jobPostings.Where(jp => jp.EmployerId == emp.Id)
                                                .Skip(skip)
                                                .Take(pageSize)
                                                .ToList();

                // Return the given postings
                return View(empJobPostings);
            }

            // Return the active postings
            return View(paginatedPostings);
        }

        // Applicants page, admin can view the applicants on the posts
        public async Task<IActionResult> Applicants(int id)
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Jobs";

            // Find the job posting with the given id
            JobPosting jobPosting = await _jobPostingRepository.GetByIdAsync(id);
            // Get all job posting applications
            await _applicationRepository.GetAll();
            // Get all application students
            await _studentRepository.GetAll();
            
            // Return the applicant information to the page
            return View(jobPosting.JobApplications);
        }

        // Archived posts page
        public async Task<IActionResult> ArchivedPosts(int page = 1, int pageSize = 6)
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Jobs";

            // Grab all job postings, and then the archived postings
            IEnumerable<JobPosting> allJobPostings = await _jobPostingRepository.GetAll();
            IEnumerable<JobPosting> archivedJobPostings = allJobPostings
                .Where(jp => jp.Archived).ToList();

            // Pagination logic
            var count = archivedJobPostings.Count();
            var totalPages = (int)Math.Ceiling(count / (double)pageSize);
            var curPage = page;
            var skip = (page - 1) * pageSize;

            // Grab valid postings per the page size and page number
            var paginatedPostings = archivedJobPostings.Skip(skip).Take(pageSize).ToList();

            // Sets the view variables with the current and total page information
            ViewBag.PageNum = curPage;
            ViewBag.TotalPages = totalPages;

            // Return the valid posts back
            return View(paginatedPostings);
        }

        // Goes to a specific job posting
        public async Task<IActionResult> Detail(int id)
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Jobs";

            // Grab the job posting with the given id
            JobPosting jobPosting = await _jobPostingRepository.GetByIdAsync(id);

            // Return the job posting
            return View(jobPosting);
        }

        // The view for the create job posting form
        public IActionResult Create()
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Jobs";

            // Create the view model that the user will see when the page loads
            var createJobPostingViewModel = new CreateJobPostingViewModel();

            // Return the view model
            return View(createJobPostingViewModel);
        }

        // Create job posting logic
        [HttpPost]
        public async Task<IActionResult> Create(CreateJobPostingViewModel jobPostingVM)
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Jobs";

            // Employer logic
            if (User.IsInRole("employer"))
            {
                // Grab the logged in user
                var user = await _userManager.GetUserAsync(User);

                // Create a job posting with the given information, and assign it to their account
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

                // Display a success message
                TempData["Success"] = "Job posting created successfully.";

                // Add the posting to the database
                _jobPostingRepository.Add(jobPosting);
                _jobPostingRepository.Save();

                // Return back to the job post page
                return RedirectToAction("Index");
            }

            if (User.IsInRole("admin"))
            {
                // If there are no errors
                if (ModelState.IsValid)
                {
                    // Create a job posting
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
            }

            // Display an error message
            TempData["Error"] = "There was an error creating the job posting.";

            // Return back to the create job posting form with errors
            return View(jobPostingVM);
        }

        // Confirm Archive posting page
        public async Task<IActionResult> ConfirmArchive(int id)
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Jobs";

            // Grab the post from the database with the given id
            var post = await _jobPostingRepository.GetByIdAsync(id);

            // Return user to the confirmation page with the post information
            return View(post);
        }

        // Confirm restore posting page
        public async Task<IActionResult> ConfirmUnarchive(int id)
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Jobs";

            // Grab the post from the database with the given id
            var post = await _jobPostingRepository.GetByIdAsync(id);

            // Return user to the confirmation page with the post information
            return View(post);
        }

        // Edit a job post page
        public async Task<IActionResult> Edit(int id, int pageNum)
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Jobs";
            
            // Set view variable for archived page number
            ViewBag.ArchPageNum = pageNum;

            // Grab job posting with given id
            var jobPosting = await _jobPostingRepository.GetByIdAsync(id);

            // Display error if the post was not found
            if (jobPosting == null) return View("Error");

            // Create the job posting view model
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

            // Display the job posting information to form
            return View(jobPostingVM);
        }

        // Edit job posting logic
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditJobPostingViewModel jobPostingVM, int pageNum = 1)
        {
            // Set the page name for the breadcrumbs
            ViewData["ActivePage"] = "Jobs";

            // If there are errors, send back to edit page
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit JobPosting");
                return View("Edit", jobPostingVM);
            }

            // Grab the job posting with the given id
            var curJobPosting = await _jobPostingRepository.GetIdAsyncNoTracking(id);

            // If the page was found
            if (curJobPosting != null)
            {
                // Employer logic
                if (User.IsInRole("employer"))
                {
                    // Don't query the database if no information was changed
                    if (curJobPosting.JobTitle == jobPostingVM.JobTitle &&
                        curJobPosting.JobDescription == jobPostingVM.JobDescription &&
                        curJobPosting.Deadline == jobPostingVM.Deadline &&
                        curJobPosting.Company == jobPostingVM.Company &&
                        curJobPosting.Location == jobPostingVM.Location &&
                        curJobPosting.PaymentCategory == jobPostingVM.PaymentCategory &&
                        curJobPosting.Link == jobPostingVM.JobLink)
                    {
                        // Display an error message
                        TempData["Error"] = "No data was changed, no changes saved.";

                        // Send user back to job posting page with error message.
                        return RedirectToAction("Index", new { page = pageNum });
                    }

                    // Grab the current logged in user
                    var user = await _userManager.GetUserAsync(User);

                    // Set the new information to the existing post
                    curJobPosting.JobTitle = jobPostingVM.JobTitle;
                    curJobPosting.JobDescription = jobPostingVM.JobDescription;
                    curJobPosting.Deadline = jobPostingVM.Deadline;
                    curJobPosting.Company = jobPostingVM.Company;
                    curJobPosting.Location = jobPostingVM.Location;
                    curJobPosting.PaymentCategory = jobPostingVM.PaymentCategory;
                    curJobPosting.Link = jobPostingVM.JobLink;
                    curJobPosting.EmployerId = user.EmployerId;

                    // Update the posting in the database
                    _jobPostingRepository.Update(curJobPosting);

                    // Display a success message
                    TempData["Success"] = "Job posting edited successfully.";

                    // Return the user back to the postings page with a success message
                    return RedirectToAction("Index", new { page = pageNum });
                } else
                {
                    // Don't query the database if no information was changed
                    if (curJobPosting.JobTitle == jobPostingVM.JobTitle &&
                        curJobPosting.JobDescription == jobPostingVM.JobDescription &&
                        curJobPosting.Deadline == jobPostingVM.Deadline &&
                        curJobPosting.Company == jobPostingVM.Company &&
                        curJobPosting.Location == jobPostingVM.Location &&
                        curJobPosting.PaymentCategory == jobPostingVM.PaymentCategory &&
                        curJobPosting.Link == jobPostingVM.JobLink)
                    {
                        // Display an error message
                        TempData["Error"] = "No data was changed. Please try again.";

                        // Return to archived area if the post was archived
                        if (curJobPosting.Archived)
                        {
                            return RedirectToAction("ArchivedPosts", new { page = pageNum });
                        }

                        // Return to the active postings area
                        return RedirectToAction("Index", new { page = pageNum });
                    }

                    // Set the new information to the existing post
                    curJobPosting.JobTitle = jobPostingVM.JobTitle;
                    curJobPosting.JobDescription = jobPostingVM.JobDescription;
                    curJobPosting.Deadline = jobPostingVM.Deadline;
                    curJobPosting.Company = jobPostingVM.Company;
                    curJobPosting.Location = jobPostingVM.Location;
                    curJobPosting.PaymentCategory = jobPostingVM.PaymentCategory;
                    curJobPosting.Link = jobPostingVM.JobLink;

                    // Update the job posting
                    _jobPostingRepository.Update(curJobPosting);

                    // Display a success message
                    TempData["Success"] = "Job posting edited successfully.";

                    // Display the archived post page if the post was archived
                    if (curJobPosting.Archived)
                    {
                        return RedirectToAction("ArchivedPosts", new { page = pageNum });
                    }

                    // Display the job posting page
                    return RedirectToAction("Index", new { page = pageNum });
                }
            }
            else
            {
                // Display an error message
                TempData["Error"] = "There was an error editing the job posting.";

                // Go back to the edit page with errors
                return View(jobPostingVM);
            }
        }

        // Delete confirmation page
        public async Task<IActionResult> Delete(int id)
        {
            // Set the page name for breadcrumbs
            ViewData["ActivePage"] = "Jobs";

            // Grab job posting from database with the given id
            var jobPostingDetails = await _jobPostingRepository.GetByIdAsync(id);

            // If no post was found, display errors
            if (jobPostingDetails == null) return View("Error");

            // Return the job posting to confirmation page
            return View(jobPostingDetails);
        }

        // Delete logic after page post
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteJobPosting(int id)
        {
            // Set the page name for breadcrumbs
            ViewData["ActivePage"] = "Jobs";

            // Grab the job posting from the database with the given id
            var jobPostingDetails = await _jobPostingRepository.GetByIdAsync(id);

            // If the job post was not found, display an error
            if (jobPostingDetails == null)
            {
                return View("Error");
            }

            // Attempt to delete the post from the database
            _jobPostingRepository.Delete(jobPostingDetails);

            // Display a success message
            TempData["Success"] = "Job posting deleted successfully.";

            // Return to the job posting index page
            return RedirectToAction("Index");
        }

        // Application logic for job postings
        public async Task<IActionResult> Apply(int? id)
        {
            if (id == null || id == 0)
            {
                TempData["Error"] = "Job posting was not found in the database.";
                return RedirectToAction("Index");
            }

            // Grab user, job posting, student, and check if they are already applied to the post or not
            var user = await _userManager.GetUserAsync(User);
            var jp = await _jobPostingRepository.GetByIdAsync((Int32)id);
            var student = await _studentRepository.GetByIdAsync((Int32)user.StudentId);
            bool alreadyApplied = await _applicationRepository
                .AnyAsync(a => a.StudentId == student.Id && a.JobPostingId == jp.Id);

            // Display error and redirect to the job posting if they already applied to that posting
            if (alreadyApplied)
            {
                TempData["Error"] = "You have already applied to this job posting.";
                return RedirectToAction("Detail", new { id = jp.Id });
            }

            // Create a new Application
            Application app = new Application();

            // Input the student and job post information into the application
            app.Student = student;
            app.StudentId = student.Id;
            app.JobPosting = jp;
            app.JobPostingId = jp.Id;

            // Add the application to the database
            _applicationRepository.Add(app);
            _applicationRepository.Save();

            // Update the job posting
            _jobPostingRepository.Update(jp);
            _jobPostingRepository.Save();

            // Add the job application to the student
            student.JobApplications.Add(app);

            // Update the student
            _studentRepository.Update(student);
            _studentRepository.Save();

            // Display a success message
            TempData["Success"] = "You have applied to the job posting successfully.";

            // Return the user back to the job posting index page
            return RedirectToAction("Index", "JobPosting");
        }

        // Job post archiving logic
        public async Task<IActionResult> Archive(int? id)
        {
            // Check if the given input is valid
            if (id == null)
            {
                TempData["Error"] = "The given id was not valid.";
                return RedirectToAction("Index", "JobPosting");
            }

            // Grab job posting from the database
            var jobPosting = await _jobPostingRepository.GetByIdAsync(id.Value);
            // Set archived to true
            jobPosting.Archived = true;
            // Update the job posting in the database
            _jobPostingRepository.Update(jobPosting);

            // Check if job post is null, display error
            if (jobPosting == null)
            {
                TempData["Error"] = "The job post was not found.";
                return RedirectToAction("Index", "JobPosting");
            }

            // Display a success message
            TempData["Success"] = "Job posting archived successfully.";

            // Return to index page with success message
            return RedirectToAction("Index", "JobPosting");
        }

        // Job post cloning logic
        public async Task<IActionResult> UnArchive(int? id)
        {
            // Check if input id is valid
            if (id == null)
            {
                TempData["Error"] = "The given id was not valid.";
                return RedirectToAction("ArchivedPosts", "JobPosting");
            }

            // Find the job post
            var jobPosting = await _jobPostingRepository.GetByIdAsync(id.Value);

            // Create new post with same information instead of using same post.
            // This is what we call cloning, it is to keep records of the previous posts. 
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
            
            // Add new post to the database
            _jobPostingRepository.Add(posting);
            _jobPostingRepository.Save();

            // Display a success message
            TempData["Success"] = "Job posting restored successfully.";

            // Return users to archived posts page
            return RedirectToAction("ArchivedPosts", "JobPosting");
        }

    }
}
