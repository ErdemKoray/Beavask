using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.Team;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;
using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Service;

public class TeamService(IUnitOfWork unitOfWork, IMapper mapper) : ITeamService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<Response<TeamDto>> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.TeamRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<TeamDto>.NotFound();

            var dto = _mapper.Map<TeamDto>(entity);
            return Response<TeamDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<TeamDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<TeamDto>>> GetAllAsync()
    {
        try
        {
            var entities = await _unitOfWork.TeamRepository.GetAsync();
            var dtos = _mapper.Map<IEnumerable<TeamDto>>(entities);
            return Response<IEnumerable<TeamDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<TeamDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<TeamDto>> CreateAsync(TeamCreateDto teamCreate)
    {
        try
        {
            var entity = _mapper.Map<Team>(teamCreate);
            await _unitOfWork.TeamRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<TeamDto>(entity);
            return Response<TeamDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<TeamDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<TeamDto>> UpdateAsync(int id, TeamUpdateDto teamUpdate)
    {
        try
        {
            var entity = await _unitOfWork.TeamRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<TeamDto>.NotFound();

            _mapper.Map(teamUpdate, entity);
            await _unitOfWork.TeamRepository.UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            var dto = _mapper.Map<TeamDto>(entity);
            return Response<TeamDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<TeamDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var entity = await _unitOfWork.TeamRepository.GetByIdAsync(id);
            if (entity == null)
                return Response<bool>.NotFound();

            await _unitOfWork.TeamRepository.DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();

            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }
}
