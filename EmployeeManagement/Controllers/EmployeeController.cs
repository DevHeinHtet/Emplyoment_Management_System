using System.Data;
using System.Net.Mime;
using System.Reflection;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Wordprocessing;
using EmployeeManagement.Data.Entities;
using EmployeeManagement.DataSets;
using EmployeeManagement.Models;
using EmployeeManagement.Models.Filters;
using EmployeeManagement.Models.Reports;
using EmployeeManagement.Services.Departments;
using EmployeeManagement.Services.Employees;
using EmployeeManagement.Services.Positions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.NETCore;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IPositionService _positionService;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentSerice;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string PROFILE = "Profiles";
        private readonly string NRCPHOTO = "NRCPhotos";
        private readonly string PDF = "PDFFiles";

        public EmployeeController(
            IPositionService positionService,
            IEmployeeService employeeService,
            IDepartmentService departmentSerice,
            IWebHostEnvironment webHostEnvironment)
        {
            _positionService = positionService;
            _employeeService = employeeService;
            _departmentSerice = departmentSerice;
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        public async Task<IActionResult> Index(EmployeeViewModel viewModel)
        {
            var filter = new EmployeeFilter
            {
                EmployeeID = viewModel.EmployeeID,
                Name = viewModel.Name,
                DepartmentID = viewModel.DepartmentID,
                RegistrationDate = viewModel.RegistrationDate,
            };
            var employees = await _employeeService.GetEmployeesWithIncludeAsync(filter);
            var departments = await _departmentSerice.GetDepartmentsAsync();

            viewModel.Departments = departments;
            viewModel.Employees = employees;

            return View(viewModel);
        }

        public IActionResult ViewPdf(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return BadRequest("Invalid file name.");
            }

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, $"files/{PDF}", fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("The requested file was not found.");
            }

            try
            {
                var fileBytes = System.IO.File.ReadAllBytes(filePath);
                return File(fileBytes, "application/pdf", fileName, enableRangeProcessing: true);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while processing the file: {ex.Message}");
            }
        }

        public async Task<IActionResult> ExportToExcel(EmployeeFilter filter)
        {
            var employees = await _employeeService.GetEmployeesWithIncludeAsync(filter);

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Employees");

                worksheet.Cell(1, 1).Value = "Employee ID";
                worksheet.Cell(1, 2).Value = "Employee Name";
                worksheet.Cell(1, 3).Value = "Position";
                worksheet.Cell(1, 4).Value = "Birth Date";
                worksheet.Cell(1, 5).Value = "Gender";
                worksheet.Cell(1, 6).Value = "Registration Date";

                int row = 2;
                foreach (var employee in employees)
                {
                    worksheet.Cell(row, 1).Value = employee.EmployeeID;
                    worksheet.Cell(row, 2).Value = employee.EmployeeName;
                    worksheet.Cell(row, 3).Value = employee.Position.Name;
                    worksheet.Cell(row, 4).Value = employee.BirthDate.ToString("yyyy-MM-dd");
                    worksheet.Cell(row, 5).Value = employee.Gender;
                    worksheet.Cell(row, 6).Value = employee.RegistrationDate.ToString("yyyy-MM-dd");
                    row++;
                }

                // Save file to memory stream
                using (var memoryStream = new MemoryStream())
                {
                    workbook.SaveAs(memoryStream);
                    return File(memoryStream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Employees.xlsx");
                }
            }
            //var result = report.Render("EXCELOPENXML");
            //return File(result, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "EmployeeReport.xlsx");
        }

        public async Task<IActionResult> EmployeeReport(EmployeeFilter filter)
        {
            var employees = await _employeeService.GetEmployeesWithIncludeAsync(filter);

            var employeeDtos = employees.Select(x => new EmployeeDto
            {
                EmployeeID = x.EmployeeID,
                Name = x.EmployeeName,
                BirthDate = x.BirthDate,
                Department = x.Position.Department.Name,
                Position = x.Position.Name,
                RegistrationDate = x.RegistrationDate
            }).ToList();

            var basePath = _webHostEnvironment.WebRootPath;
            string reportPath = Path.Combine(basePath, @"Reports\RptEmployee.rdlc");

            using var report = new LocalReport();
            report.ReportPath = reportPath;
            report.DataSources.Add(new ReportDataSource("EmployeeDS", employeeDtos));

            var result = report.Render("PDF");

            return File(result, "application/pdf", "EmployeeReport.pdf");
        }

        public async Task<IActionResult> Create()
        {
            var newEmployeeID = await _employeeService.GenerateEmployeeID();
            var departments = await _departmentSerice.GetDepartmentsAsync();

            var viewModel = new EmployeeRegistrationViewModel
            {
                EmployeeID = newEmployeeID,
                Departments = departments,
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(EmployeeRegistrationViewModel viewModel)
        {
            var newEmployeeID = await _employeeService.GenerateEmployeeID();
            var departments = await _departmentSerice.GetDepartmentsAsync();

            viewModel.EmployeeID = newEmployeeID;
            viewModel.Departments = departments;

            if (viewModel.RecommendationLetter != null &&
                viewModel.RecommendationLetter.ContentType != "application/pdf")
            {
                ModelState.AddModelError("RecommendationLetter", "The uploaded file is not a valid PDF.");
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var profilePath = await UploadFile(PROFILE, viewModel.Profile);
            var nrcFrontPath = await UploadFile(NRCPHOTO, viewModel.NrcFront);
            var nrcBackPath = await UploadFile(NRCPHOTO, viewModel.NrcBack);
            var recommendationPath = await UploadFile(PDF, viewModel.RecommendationLetter);

            var employee = new Employee
            {
                EmployeeID = viewModel.EmployeeID,
                EmployeeName = viewModel.EmployeeName,
                BirthDate = viewModel.BirthDate,
                Gender = viewModel.Gender,
                PositionID = viewModel.PositionID,
                EmployeePhoto = profilePath,
                NrcFrontImage = nrcFrontPath,
                NrcBackImage = nrcBackPath,
                RegistrationDate = viewModel.RegistrationDate,
            };
            await _employeeService.AddEmployeeAsync(employee);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string employeeId)
        {
            var employee = await _employeeService.GetEmployeeByEmployeeIDAsync(employeeId);

            if (employee == null) return NotFound();

            var departments = await _departmentSerice.GetDepartmentsAsync();

            var viewModel = new EditEmployeeViewModel
            {
                EmployeeID = employee.EmployeeID,
                EmployeeName = employee.EmployeeName,
                BirthDate = employee.BirthDate,
                Gender = employee.Gender,
                DepartmentID = employee.Position.DepartmentID,
                PositionID = employee.Position.PositionID,
                RegistrationDate = employee.RegistrationDate,
                EmployeePhoto = FullFilePath(PROFILE, employee.EmployeePhoto),
                NrcFrontImage = FullFilePath(NRCPHOTO, employee.NrcFrontImage),
                NrcBackImage = FullFilePath(NRCPHOTO, employee.NrcBackImage),
                Departments = departments,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string employeeId, EditEmployeeViewModel viewModel)
        {
            var existingEmployee = await _employeeService.GetEmployeeByEmployeeIDAsync(employeeId);
            if (existingEmployee == null) return NotFound();

            if (viewModel.RecommendationLetter != null &&
                viewModel.RecommendationLetter.ContentType != "application/pdf")
            {
                ModelState.AddModelError("RecommendationLetter", "The uploaded file is not a valid PDF.");
            }

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if (viewModel.Profile != null)
            {
                DeleteFileIsExists(PROFILE, existingEmployee.EmployeePhoto);
                existingEmployee.EmployeePhoto = await UploadFile(PROFILE, viewModel.Profile);
            }

            if (viewModel.NrcFront != null)
            {
                DeleteFileIsExists(NRCPHOTO, existingEmployee.NrcFrontImage);
                existingEmployee.NrcFrontImage = await UploadFile(NRCPHOTO, viewModel.NrcFront);
            }

            if (viewModel.NrcBack != null)
            {
                DeleteFileIsExists(NRCPHOTO, existingEmployee.NrcBackImage);
                existingEmployee.NrcBackImage = await UploadFile(NRCPHOTO, viewModel.NrcBack);
            }

            if (viewModel.RecommendationLetter != null)
            {
                DeleteFileIsExists(NRCPHOTO, existingEmployee.NrcBackImage);
                existingEmployee.RecommendationLetter = await UploadFile(PDF, viewModel.RecommendationLetter);
            }

            existingEmployee.EmployeeName = viewModel.EmployeeName;
            existingEmployee.BirthDate = viewModel.BirthDate;
            existingEmployee.Gender = viewModel.Gender;
            existingEmployee.PositionID = viewModel.PositionID;
            existingEmployee.RegistrationDate = viewModel.RegistrationDate;

            await _employeeService.UpdateEmployeeAsync(existingEmployee);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string employeeId)
        {
            var employee = await _employeeService.GetEmployeeByEmployeeIDAsync(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            await _employeeService.DeleteEmployeeAsync(employeeId);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> GetPositionsByDepartment(int departmentId)
        {
            var positions = await _positionService.GetPositionsByDepartment(departmentId);
            return Json(positions);
        }

        private async Task<string> UploadFile(string folderName, IFormFile file)
        {
            if (file != null && !string.IsNullOrWhiteSpace(folderName))
            {
                // Save the photo to the wwwroot folder
                string extension = Path.GetExtension(file.FileName);
                string newFileName = $"{Guid.NewGuid().ToString()}{extension}";
                string filePath = Path.Combine(_webHostEnvironment.WebRootPath, $"files/{folderName}", newFileName);

                // Create the images folder if it doesn't exist
                var directory = Path.GetDirectoryName(filePath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                return $"{newFileName}";
            }
            return string.Empty;
        }

        private string FullFilePath(string folderName, string fileName)
        {
            if (!string.IsNullOrWhiteSpace(fileName))
            {
                return $"/files/{folderName}/{fileName}";
            }
            return string.Empty;
        }

        private void DeleteFileIsExists(string folderName, string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                string existingFileFullPath = Path.Combine(_webHostEnvironment.WebRootPath, $"files/{folderName}", fileName);

                if (System.IO.File.Exists(existingFileFullPath))
                {
                    System.IO.File.Delete(existingFileFullPath);
                }
            }
        }
    }
}
