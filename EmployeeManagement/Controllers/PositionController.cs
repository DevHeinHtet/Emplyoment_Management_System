using EmployeeManagement.Data.Entities;
using EmployeeManagement.Models;
using EmployeeManagement.Services.Departments;
using EmployeeManagement.Services.Positions;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class PositionController : Controller
    {
        private readonly IPositionService _positionService;
        private readonly IDepartmentService _departmentService;

        public PositionController(IPositionService positionService, IDepartmentService departmentService)
        {
            _positionService = positionService;
            _departmentService = departmentService;
        }

        public async Task<IActionResult> Index()
        {
            var positions = await _positionService.GetAllPositionsWithDepartmentAsync();
            return View(positions);
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.GetDepartmentsAsync();
            var viewModel = new CreatePositionViewModel
            {
                Departments = departments,
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreatePositionViewModel viewModel)
        {
            var departments = await _departmentService.GetDepartmentsAsync();
            var position = new Position()
            {
                DepartmentID = viewModel.DepartmentID,
                Name = viewModel.PositionName,
            };

            await _positionService.AddPositionAsync(position);

            viewModel.Departments = departments;
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var position = await _positionService.GetPositionByIdAsync(id);

            if (position == null) return NotFound();

            var departments = await _departmentService.GetDepartmentsAsync();

            var viewModel = new EditPositionViewModel
            {
                Departments = departments,
                DepartmentID = position.DepartmentID,
                PositionName = position.Name
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditPositionViewModel viewModel)
        {
            var existingPosition = await _positionService.GetPositionByIdAsync(id);
            if (existingPosition == null) return NotFound();

            existingPosition.DepartmentID = viewModel.DepartmentID;
            existingPosition.Name = viewModel.PositionName;

            await _positionService.UpdatePositionAsync(existingPosition);

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var position = await _positionService.GetPositionByIdAsync(id);
            if (position == null)
            {
                return NotFound();
            }
            await _positionService.DeletePositionAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
