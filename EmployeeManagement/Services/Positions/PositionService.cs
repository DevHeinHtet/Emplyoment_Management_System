using EmployeeManagement.Data.Entities;
using EmployeeManagement.Interfaces;
using EmployeeManagement.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EmployeeManagement.Services.Positions;

public class PositionService : IPositionService
{
    private readonly IUnitOfWork _unitOfWork;

    public PositionService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Position>> GetAllPositionsWithDepartmentAsync()
    {
        return await _unitOfWork.Positions.GetAllPositionsWithDepartmentAsync();
    }

    public async Task<IEnumerable<Position>> GetPositionsByDepartment(int departmentID)
    {
        return await _unitOfWork.Positions.GetPositionsByDepartment(departmentID);
    }

    public async Task<Position> GetPositionByIdAsync(int id)
    {
        return await _unitOfWork.Positions.GetByIdAsync(id);
    }

    public async Task AddPositionAsync(Position position)
    {
        await _unitOfWork.Positions.AddAsync(position);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task UpdatePositionAsync(Position position)
    {
        _unitOfWork.Positions.Update(position);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task DeletePositionAsync(int id)
    {
        var position = await GetPositionByIdAsync(id);
        if (position != null)
        {
            _unitOfWork.Positions.Delete(position);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

