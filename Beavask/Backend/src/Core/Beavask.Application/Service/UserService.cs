using AutoMapper;
using Beavask.Application.Common;
using Beavask.Application.DTOs.User;
using Beavask.Application.Interface.Service;
using Beavask.Application.Interface;
using Beavask.Domain.Entities.Base;

namespace Beavask.Application.Service;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Response<UserWithTeamAndCompanyDto>> GetUserBriefAsync(int id)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetUserWithTeamAndCompanyBrief(c => c.Id == id, id);
            if (user == null)
            {
                return Response<UserWithTeamAndCompanyDto>.NotFound();
            }

            return Response<UserWithTeamAndCompanyDto>.Success(user);
        }
        catch (Exception ex)
        {
            return Response<UserWithTeamAndCompanyDto>.Fail(ex.Message);
        }
    }


    public async Task<Response<IEnumerable<UserDto>>> GetAllAsync()
    {
        try
        {
            var users = await _unitOfWork.UserRepository.GetAsync();
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return Response<IEnumerable<UserDto>>.Success(userDtos);
        }
        catch (Exception ex)
        {
            return Response<IEnumerable<UserDto>>.Fail(ex.Message);
        }
    }

    public async Task<Response<UserDto>> CreateAsync(UserCreateDto userCreateDto)
    {
        try
        {
            var user = _mapper.Map<User>(userCreateDto);
            user.PasswordHash = HashPassword(userCreateDto.Password); 
            user.PasswordSalt = GenerateSalt(); 
            
            await _unitOfWork.UserRepository.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();
            var userDto = _mapper.Map<UserDto>(user);
            return Response<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return Response<UserDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<UserDto>> UpdateAsync(int id, UserUpdateDto userUpdateDto)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                return Response<UserDto>.NotFound();
            }

            _mapper.Map(userUpdateDto, user);
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            
            var userDto = _mapper.Map<UserDto>(user);
            return Response<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return Response<UserDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteAsync(int id)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                return Response<bool>.NotFound();
            }

            await _unitOfWork.UserRepository.DeleteAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }
    private string HashPassword(string password)
    {
        return password;
    }

    private string GenerateSalt()
    {
        return "randomSalt";
    }

    public async Task<Response<bool>> UpdateUserCompanyIdAsync(int id, UpdateCompanyRequest updateCompanyRequest)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                return Response<bool>.NotFound();
            }
            user.CompanyId = updateCompanyRequest.CompanyId;
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return Response<bool>.Success(true);
        }
        catch (Exception ex)
        {
            return Response<bool>.Fail(ex.Message);
        }
    }

    public async Task<Response<UserDto>> GetUserByUsernameAsync(string username)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            if (user == null)
            {
                return Response<UserDto>.NotFound();
            }
            var userDto = _mapper.Map<UserDto>(user);
            return Response<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return Response<UserDto>.Fail(ex.Message);
        }
    }

    public async Task<Response<UserDto>> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
            {
                return Response<UserDto>.NotFound();
            }
            var userDto = _mapper.Map<UserDto>(user);
            return Response<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            return Response<UserDto>.Fail(ex.Message);
        }
    }
}


