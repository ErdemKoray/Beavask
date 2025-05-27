using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.Team;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;
using Beavask.Domain.Entities.Base;
using Beavask.Application.DTOs.User;
using Beavask.Application.DTOs.Event;
using Beavask.Application.Interface.Logging;

namespace Beavask.Application.Service;

public class TeamService(IUnitOfWork unitOfWork, IMapper mapper, ICurrentCompanyService currentCompanyService, ICurrentUserService currentUserService, ILogger logger) : ITeamService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;
    private readonly ICurrentCompanyService _currentCompanyService = currentCompanyService;
    private readonly ICurrentUserService _currentUserService = currentUserService;
    private readonly ILogger _logger = logger;

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

    public async Task<Response<TeamWithMembersDto>> GetTeamWithMembersAsync(int teamId)
    {
        try
        {
            var entity = await _unitOfWork.TeamRepository.GetTeamWithMembersAsync(teamId);
            if (entity == null)
                return Response<TeamWithMembersDto>.NotFound();

            var dto = _mapper.Map<TeamWithMembersDto>(entity);
            return Response<TeamWithMembersDto>.Success(dto);
        }
        catch (Exception ex)
        {
            return Response<TeamWithMembersDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<UserDto>>> GetMembersByTeamIdAsync(int teamId)
    {
        var members = await _unitOfWork.TeamRepository.GetMembersByTeamId(teamId);
        if (members == null || !members.Any())
            return Response<IEnumerable<UserDto>>.NotFound();
        
        var dtos = _mapper.Map<IEnumerable<UserDto>>(members);
        return Response<IEnumerable<UserDto>>.Success(dtos);
    
    }

    public async Task<Response<IEnumerable<EventDto>>> GetEventsByTeamIdAsync(int teamId)
    {
        var events = await _unitOfWork.TeamRepository.GetEventsByTeamId(teamId);
        if (events == null || !events.Any())
            return Response<IEnumerable<EventDto>>.NotFound();
        
        var dtos = _mapper.Map<IEnumerable<EventDto>>(events);
        return Response<IEnumerable<EventDto>>.Success(dtos);
    }

    public async Task<Response<TeamDto>> CreateTeamForCompanyAsync(TeamCreateDto teamCreateDto)
    {
        try
        {
            var _team = _mapper.Map<Team>(teamCreateDto);
            _team.CompanyId = _currentCompanyService.CompanyId;
            await _unitOfWork.TeamRepository.AddAsync(_team);
            await _unitOfWork.SaveChangesAsync();
            await _logger.LogInformation("Team created successfully", context: _currentCompanyService.CompanyId.ToString());
            var dto = _mapper.Map<TeamDto>(_team);
            return Response<TeamDto>.Success(dto);
        }
        catch (Exception ex)
        {
            await _logger.LogError("Error creating team", ex, context: _currentCompanyService.CompanyId.ToString());
            return Response<TeamDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<TeamDto>> GetTeamByCompanyIdAsync(int companyId)
    {
        try
        {
            var team = await _unitOfWork.TeamRepository.GetSingleByConditionAsync(t => t.CompanyId == companyId);
            if (team == null)
                return Response<TeamDto>.NotFound();

            var dto = _mapper.Map<TeamDto>(team);
            return Response<TeamDto>.Success(dto);
        }
        catch (Exception ex)
        {
            await _logger.LogError("Error getting team by company id", ex, context: companyId.ToString());
            return Response<TeamDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> AssignUserToTeamAsync(int teamId, int userId)
    {
        try
        {
            var team = await _unitOfWork.TeamRepository.GetByIdAsync(teamId);
            if (team == null)
            {
                await _logger.LogError("Team not found", context: teamId.ToString());
                return Response<bool>.Fail("Team not found");
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                await _logger.LogError("User not found", context: userId.ToString());
                return Response<bool>.Fail("User not found");
            }
            if(user.CompanyId != _currentCompanyService.CompanyId)
            {
                await _logger.LogError("User not found in company", context: userId.ToString());
                return Response<bool>.Fail("User not found in company");
            }
            
            if (user.TeamId == teamId)
            {
                await _logger.LogError("User already in team", context: teamId.ToString());
                return Response<bool>.Fail("User already in team");
            }

            if (user.TeamId != null)
            {
                await _logger.LogError("User already in another team", context: teamId.ToString());
                return Response<bool>.Fail("User already in another team");
            }

            user.TeamId = teamId;
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            await _logger.LogInformation("User assigned to team successfully", context: teamId.ToString());
            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            await _logger.LogError("Error assigning user to team", ex, context: teamId.ToString());
            return Response<bool>.Fail(ex.Message);
        }
    }

    public async Task<Response<IEnumerable<TeamDto>>> GetAllTeamsByCompanyIdAsync(int companyId)
    {
        try
        {
            var teams = await _unitOfWork.TeamRepository.GetAllTeamsByCompanyId(companyId);
            var dtos = _mapper.Map<IEnumerable<TeamDto>>(teams);
            return Response<IEnumerable<TeamDto>>.Success(dtos);
        }
        catch (Exception ex)
        {
            await _logger.LogError("Error getting all teams by company id", ex, context: companyId.ToString());
            return Response<IEnumerable<TeamDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<TeamDto>> GetTeamByUserIdAsync()
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(_currentUserService.UserId.Value);
            if (user == null)
            {
                await _logger.LogError("User not found", context: _currentUserService.UserId.ToString());
                return Response<TeamDto>.Fail("User not found");
            }

            if (user.TeamId == null)
            {
                await _logger.LogError("User has no team", context: _currentUserService.UserId.ToString());
                return Response<TeamDto>.Fail("User has no team");
            }

            var userTeam = await _unitOfWork.TeamRepository.GetByIdAsync(user.TeamId.Value);
            if (userTeam == null)
            {
                await _logger.LogError("Team not found", context: user.TeamId.ToString());
                return Response<TeamDto>.Fail("Team not found");
            }

            var dto = _mapper.Map<TeamDto>(userTeam);
            return Response<TeamDto>.Success(dto);
        }
        catch (Exception ex)
        {
            await _logger.LogError("Error getting team by user id", ex, context: _currentUserService.UserId.ToString());
            return Response<TeamDto>.Fail(ex.Message);
        }
    }
}
