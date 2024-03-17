using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CITPracticum.Controllers
{
    public class JobPostingController : Controller
    {
        private readonly IJobPostingRepository _jobPostingRepository;

        public JobPostingController(IJobPostingRepository jobPostingRepository)
        {
            _jobPostingRepository = jobPostingRepository;
        }
        // displays all job postings on index page
        public async Task<IActionResult> Index()
        {
            IEnumerable<JobPosting> jobPostings = await _jobPostingRepository.GetAll();
            return View(jobPostings);
        }

        // goes to a specific job posting page
        public async Task<IActionResult> Detail(int id)
        {
            JobPosting jobPosting = await _jobPostingRepository.GetByIdAsync(id);
            return View(jobPosting);
        }
        // creates a new job post
        public IActionResult Create()
        {
            var createJobPostingViewModel = new CreateJobPostingViewModel();
            return View(createJobPostingViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Create (CreateJobPostingViewModel jobPostingVM)
        {
            if (ModelState.IsValid)
            {
                var jobPosting = new JobPosting()
                {
                    JobTitle = jobPostingVM.JobTitle,
                    JobDescription = jobPostingVM.JobDescription,
                    DueDate = jobPostingVM.DueDate,
                    Company = jobPostingVM.Company,
                    Location = jobPostingVM.Location,
                    Link = jobPostingVM.JobLink
                };
                _jobPostingRepository.Add(jobPosting);
                return RedirectToAction("Index");
            }
            return View(jobPostingVM);
        }

        // edit a job post
        public async Task<IActionResult> Edit(int id)
        {
            var jobPosting = await _jobPostingRepository.GetByIdAsync(id);
            if (jobPosting == null) return View("Error");
            var jobPostingVM = new EditJobPostingViewModel()
            {
                JobTitle = jobPosting.JobTitle,
                JobDescription = jobPosting.JobDescription,
                DueDate = jobPosting.DueDate,
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
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit JobPosting");
                return View("Edit", jobPostingVM);
            }
            var curJobPosting = await _jobPostingRepository.GetIdAsyncNoTracking(id);

            if(curJobPosting != null)
            {
                var jobPosting = new JobPosting()
                {
                    Id = id,
                    JobTitle = jobPostingVM.JobTitle,
                    JobDescription = jobPostingVM.JobDescription,
                    DueDate = jobPostingVM.DueDate,
                    Company = jobPostingVM.Company,
                    PaymentCategory = jobPostingVM.PaymentCategory,
                    Location = jobPostingVM.Location,
                    Link = jobPostingVM.JobLink
                };
                _jobPostingRepository.Update(jobPosting);

                return RedirectToAction("Index");
            } else
            {
                return View(jobPostingVM);
            }
        }
        // deletes a job post
        public async Task<IActionResult> Delete(int id)
        {
            var jobPostingDetails = await _jobPostingRepository.GetByIdAsync(id);
            if (jobPostingDetails == null) return View("Error");
            return View(jobPostingDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteJobPosting(int id)
        {
            var jobPostingDetails = await _jobPostingRepository.GetByIdAsync(id);

            if(jobPostingDetails == null)
            {
                return View("Error");
            }

            _jobPostingRepository.Delete(jobPostingDetails);
            return RedirectToAction("Index");
        }
    }
}
