using Application.Interfaces.Repositories;
using Domain.Entities;
using MediatR;
using Application.Wrappers.Abstract;
using Application.Wrappers.Concrete;

namespace Application.Features.Users.Commands
{
    public class QuickAddOrUpdateUserCommand : IRequest<IResponse>
    {
        public int? Id { get; set; } // null for add, set for update
        public string UserName { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class QuickAddOrUpdateUserCommandHandler : IRequestHandler<QuickAddOrUpdateUserCommand, IResponse>
    {
        private readonly IUserRepository _userRepository;

        public QuickAddOrUpdateUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IResponse> Handle(QuickAddOrUpdateUserCommand request, CancellationToken cancellationToken)
        {
            User user;
            if (request.Id.HasValue)
            {
                user = await _userRepository.GetByIdAsync(request.Id.Value);
                if (user == null)
                {
                    return new ErrorResponse(404, "User not found");
                }
                user.UserName = request.UserName;
                user.Email = request.Email;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                _userRepository.Update(user);
            }
            else
            {
                user = new User
                {
                    UserName = request.UserName,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName
                };
                await _userRepository.AddAsync(user);
            }
            return new SuccessResponse(200, "User saved successfully");
        }
    }
}
