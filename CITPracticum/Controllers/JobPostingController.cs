﻿using CITPracticum.Data.Migrations;
using CITPracticum.Interfaces;
using CITPracticum.Models;
using CITPracticum.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

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
            ViewData["ActivePage"] = "Jobs";

            IEnumerable<JobPosting> jobPostings = await _jobPostingRepository.GetAll();
            return View(jobPostings);
        }

        public async Task<IActionResult> Applicants(int id)
        {
            ViewData["ActivePage"] = "Jobs";

            JobPosting jobPosting = await _jobPostingRepository.GetByIdAsync(id);
            return View(jobPosting);
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
        public async Task<IActionResult> Create (CreateJobPostingViewModel jobPostingVM)
        {
            ViewData["ActivePage"] = "Jobs";
            if (ModelState.IsValid)
            {
                var jobPosting = new JobPosting()
                {
                    JobTitle = jobPostingVM.JobTitle,
                    JobDescription = jobPostingVM.JobDescription,
                    Deadline = jobPostingVM.Deadline,
                    Company = jobPostingVM.Company,
                    Location = jobPostingVM.Location,
                    Link = jobPostingVM.JobLink
                };
                _jobPostingRepository.Add(jobPosting);
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

            if(curJobPosting != null)
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

                return RedirectToAction("Index");
            } else
            {
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

            if(jobPostingDetails == null)
            {
                return View("Error");
            }

            _jobPostingRepository.Delete(jobPostingDetails);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Apply(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }



            var jobPosting = await _jobPostingRepository.GetByIdAsync(id.Value);
            //jobPosting.Applicants.Add();
            _jobPostingRepository.Update(jobPosting);

            if (jobPosting == null)
            {
                return NotFound();
            }

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

            return RedirectToAction("Index", "JobPosting"); // Show the archive confirmation view
        }

        public async Task<IActionResult> UnArchive(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobPosting = await _jobPostingRepository.GetByIdAsync(id.Value);
            jobPosting.Archived = false;
            _jobPostingRepository.Update(jobPosting);
            
            if (jobPosting == null)
            {
                return NotFound();
            }

            return RedirectToAction("ArchivedPosts", "JobPosting");
        }

    }
}
