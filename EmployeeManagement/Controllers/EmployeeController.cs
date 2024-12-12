using System.Data;
using System.Net.Mime;
using System.Reflection;
using AspNetCore.Reporting;
using ClosedXML.Excel;
using EmployeeManagement.Data.Entities;
using EmployeeManagement.Models;
using EmployeeManagement.Models.Filters;
using EmployeeManagement.Models.Reports;
using EmployeeManagement.Services.Departments;
using EmployeeManagement.Services.Employees;
using EmployeeManagement.Services.Positions;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IPositionService _positionService;
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentService _departmentSerice;
        private readonly IWebHostEnvironment _webHostEnvironment;

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

        public async Task<IActionResult> ExportToExcel(string employeeID, string name, int departmentID, DateTime? registrationDate)
        {
            var filter = new EmployeeFilter
            {
                EmployeeID = employeeID,
                Name = name,
                DepartmentID = departmentID,
                RegistrationDate = registrationDate,
            };
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
        }

        public async Task<IActionResult> EmployeeReport(string employeeID, string name, int departmentID, DateTime? registrationDate)
        {
            var filter = new EmployeeFilter
            {
                EmployeeID = employeeID,
                Name = name,
                DepartmentID = departmentID,
                RegistrationDate = registrationDate,
            };
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

            var reportPath = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "RptEmployee.rdlc");

            var report = new LocalReport(reportPath);

            var datasource = ToDataTable(employeeDtos);

            report.AddDataSource("EmployeeDS", datasource);

            var result = report.Execute(RenderType.Pdf, 1, null);

            return File(result.MainStream, MediaTypeNames.Application.Octet, "EmployeeReport.pdf");
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

            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var employee = new Employee
            {
                EmployeeID = viewModel.EmployeeID,
                EmployeeName = viewModel.EmployeeName,
                BirthDate = viewModel.BirthDate,
                Gender = viewModel.Gender,
                PositionID = viewModel.PositionID,
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
                Departments = departments,
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string employeeId, EditEmployeeViewModel viewModel)
        {
            var existingEmployee = await _employeeService.GetEmployeeByEmployeeIDAsync(employeeId);
            if (existingEmployee == null) return NotFound();

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

        public DataTable ToDataTable<T>(List<T> items)
        {
            var dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
