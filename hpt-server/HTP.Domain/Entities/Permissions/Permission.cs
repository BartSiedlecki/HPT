using HPT.SharedKernel;
using HTP.Domain.ValueObjects;
using HTP.SharedKernel;

namespace HTP.Domain.Entities.Permissions;

public sealed class Permission : Entity
{
    public PermissionId Id { get; init; } = null!;
    public PermissionDescription? Description { get; private set; }

    private Permission() { }

    public static Result<Permission> Create(PermissionId id, PermissionDescription? description)
    {
        var newPermission = new Permission() { Id = id };
        var descriptionResult =  newPermission.SetDescription(description);

        if (descriptionResult.IsFailure)
            return Result.Failure<Permission>(descriptionResult.Error);

        return Result.Success(newPermission);
    }

    public Result SetDescription(PermissionDescription? description)
    {
        Description = description;

        return Result.Success();
    }
}

