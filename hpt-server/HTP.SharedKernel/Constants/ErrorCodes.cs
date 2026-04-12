namespace HPT.SharedKernel.Constants;

public sealed record ErrorCode(string Code, string Message);

public static class ErrorCodes
{
    public static class Auth
    {
       
            public static readonly ErrorCode UserNotFound =
                new("Auth.UserNotFound", "Użytkownik nie istnieje");

            public static readonly ErrorCode Forbidden =
                new("Auth.ForbiddenAccess",
                    "Nie posiadasz uprawnień umożwiających dostęp do tego zasobu");

            public static readonly ErrorCode UserBlocked =
                new("Auth.UserBlocked",
                    "Konto użytkownika jest zablokowane");

            public static readonly ErrorCode UserUnconfirmed =
                new("Auth.UserUnconfired",
                    "Konto użytkownika nie zostało potwierdzone");

            public static readonly ErrorCode AccountInactive =
                new("Auth.AccountInactive",
                    "Konto nie zostało jeszcze aktywowane.");

            public static readonly ErrorCode Unauthenticated =
                new("Auth.UnauthorizedUser",
                    "Unauthorized user");

        public static readonly ErrorCode InvalidCredentials =
            new("Auth.InvalidCredentials",
            "Niepoprawny login lub hasło");

        public static readonly ErrorCode UserRegistrationFailed =
                new("Auth.UserRegistrationFailed",
                    "User registration failed.");

        public static ErrorCode InvalidCredentialsWithAttempts(int remainingAttempts) =>
                new("Auth.InvalidCredentialsWithAttempts",
                    $"Nieprawidłowy login lub hasło. Pozostało prób: {remainingAttempts}");

            public static readonly ErrorCode PasswordLockedOut =
                new("Auth.PasswordLockedOut",
                    "Hasło zablokowane. Spróbuj ponownie za {Minutes} minut.");

            public static readonly ErrorCode InvalidRefreshToken =
                new("Auth.InvalidRefreshToken",
                    "Niepoprawny token odświeżający poświadczenia");

            public static readonly ErrorCode InvalidPasswordRecoveryToken =
                new("Auth.InvalidPasswordRecoveryToken",
                    "Niepoprawny token odzyskiwania hasła");

            public static readonly ErrorCode PasswordChangeFailed =
                new("Auth.PasswordChangeFailed",
                    "Zmiana hasła zakończona niepowodzeniem.");

            public static readonly ErrorCode GeneralFailed =
                    new("Auth.GeneralFailed",
                        "Logowanie użytkownika zakończone niepowodzeniem.");
    }

    public static class BlacklistedOrigin
    {
        public static class Domain
        {
            public static readonly ErrorCode AlreadyExists = new(
                "Domain.BlacklistedOrigin.AlreadyExists", 
                "Domain already exists on the blacklist.");

            public static readonly ErrorCode NotFound = new(
                "Domain.BlacklistedOrigin.NotFound", 
                "Domain not found on the blacklist.");
        }

        public static class Validation
        {
            public static readonly ErrorCode ReasonRequired = new(
                "Validation.BlacklistedOrigin.ReasonRequired",
                "Reason is required.");

            public static readonly ErrorCode CustomReasonEmpty = new(
                "Validation.BlacklistedOrigin.CustomReasonEmpty", 
                "Custom reason cannot be empty.");

            public static readonly ErrorCode CustomReasonMaximumLengthExceeded = new(
                "Validation.BlacklistedOrigin.CustomReasonMaximumLengthExceeded",
                "Custom reason is too long. The maximum number of characters is {MaxLength}.");
        }
    }
    
    public static class CalendarEvent
    {
        public static class Domain
        {
            public static readonly ErrorCode NotFound = new("Domain.CalendarEvent.NotFound", $"Calendar event not found.");
        }


        public static class Validation
        {
            public static readonly ErrorCode EmptyTitle = new(
                "Validation.CalendarEvent.EmptyTitle",
                "Event title cannot be empty."
            );

            public static readonly ErrorCode TitleMaximumLengthExceeded = new(
                "Validation.CalendarEvent.TitleMaximumLengthExceeded",
                "Event title is too long. The maximum number of characters allowed is {MaxLength}."
            );

            public static readonly ErrorCode DescriptionMaximumLengthExceeded = new(
                "Validation.CalendarEvent.DescriptionMaximumLengthExceeded",
                "Event description is too long. The maximum number of characters allowed is {MaxLength}."
            );

            public static readonly ErrorCode DateToCannotBeBeforeDateFrom = new(
                "Validation.CalendarEvent.DateToCannotBeBeforeDateFrom",
                "Invalid date range."
            );

            public static readonly ErrorCode ReminderMinutesBeforeOutOfRange = new(
                "Validation.CalendarEvent.ReminderMinutesBeforeOutOfRange",
                "Invalid number of minutes."
            );

            public static readonly ErrorCode EmptyColor = new(
                "Validation.CalendarEvent.EmptyColor",
                "Event color is required."
            );

            public static readonly ErrorCode InvalidColorFormat = new(
                "Validation.CalendarEvent.InvalidColorFormat",
                "Invalid color format. Hexadecimal format is required (#000000)."
            );

            // Association
            public static readonly ErrorCode InvalidAssociationType = new(
                "Validation.CalendarEvent.InvalidAssociationType",
                "Invalid associated resource type."
            );

            public static readonly ErrorCode EmptyAssociatedId = new(
                "Validation.CalendarEvent.EmptyAssociatedId",
                "Associated resource identifier cannot be empty."
            );

            public static readonly ErrorCode EmptyAssociatedName = new(
                "Validation.CalendarEvent.EmptyAssociatedName",
                "Associated resource name cannot be empty."
            );

            public static readonly ErrorCode AssociatedNameMaximumLengthExceeded = new(
                "Validation.CalendarEvent.AssociatedNameMaximumLengthExceeded",
                "Associated resource name is too long. The maximum number of characters allowed is {MaxLength}."
            );
        }
    }

    public static class CalendarEventAssociation
    {
        public static class Validation
        {
            public static readonly ErrorCode EmptyId = new(
                "Validation.CalendarEventAssociation.EmptyAssociatedId",
                "There was a problem assigning the resource to the calendar event.");

            public static readonly ErrorCode EmptyAssociatedName = new(
                "Validation.CalendarEventAssociation.EmptyAssociatedName",
                "There was a problem assigning the resource to the calendar event.");
        }
    }

    public static class CustomBlackListedReason
    {
        public static class Validation
        {
            public static readonly ErrorCode EmptyText = new(
                "Validation.CustomBlackListedReason.EmptyText",
                "Custom reason text cannot be empty."
            );

            public static readonly ErrorCode TextMaximumLengthExceeded = new(
                "Validation.CustomBlackListedReason.TextMaximumLengthExceeded",
                "Custom reason text is too long. The maximum number of characters is {MaxLength}."
            );

            public static readonly ErrorCode UnexpectedCustomValue = new(
                "Validation.CustomBlackListedReason.UnexpectedCustomValue",
                "Custom reason text should only be set when the reason type is 'Custom reason'."
            );
        }
    }

    public static class Email
    {
        public static class Validation
        {
            public static readonly ErrorCode Empty =
                new("Validation.Email.Empty", 
                    "Email address cannot be empty.");

            public static readonly ErrorCode MaximumLengthExceeded =
                new("Validation.Email.MaximumLengthExceeded",
                    "Email address is too long. The maximum number of characters is {MaxLength}.");

            public static readonly ErrorCode InvalidFormat = new(
                "Validation.Email.InvalidFormat", 
                "Invalid email address format.");
        }
    }

    public static class FirstName
    {
        public static class Validation
        {
            public static ErrorCode Empty = new(
                "Validation.FirstName.Empty",
                "First name address cannot be empty.");

            public static ErrorCode InvalidFormat = new(
                "Validation.FirstName.InvalidFormat",
                "Invalid first name format. Use letters only.");

            public static ErrorCode MinLengthNotMet = new(
                "Validation.FirstName.MinLengthNotMet",
                "First name is too short. The minimum number of characters is {MinLength}.");

            public static ErrorCode MaxLengthExceeded = new(
                "Validation.FirstName.MaxLengthExceeded",
                "First name is too long. The maximum number of characters is {MaxLength}.");
        }
    }

    public static class LastName
    {
        public static class Validation
        {
            public static ErrorCode Empty = new(
                "Validation.LastName.Empty",
                "Last name address cannot be empty.");

            public static ErrorCode InvalidFormat = new(
                "Validation.LastName.InvalidFormat",
                "Invalid last name format. Use letters only.");

            public static ErrorCode MinLengthNotMet = new(
                "Validation.LastName.MinLengthNotMet",
                "Last name is too short. The minimum number of characters is {MinLength}.");

            public static ErrorCode MaxLengthExceeded = new(
                "Validation.LastName.MaxLengthExceeded",
                "Last name is too long. The maximum number of characters is {MaxLength}.");
        }
    }

    public static class Password
    {
        public static class Validation
        {
            public static ErrorCode Empty =>
                new("Validation.Password.Empty", 
                    "Password cannot be empty.");

            public static ErrorCode MinLengthNotMet =>
                new("Validation.Password.MinLengthNotMet",
                    "Password is too short. The minimum number of characters is {MinLength}.");

            public static ErrorCode MaximumLengthExceeded =>
                new("Validation.Password.MaximumLengthExceeded",
                    "Password is too long. The maximum number of characters is {MaxLength}.");
        }
    }

    public static class PhoneNumber
    {
        public static class Validation
        {
            public static ErrorCode Empty =>
                new("Validation.PhoneNumber.Empty", 
                    "Phone number cannot be empty.");

            public static ErrorCode MaximumLengthExceeded =>
                new("Validation.PhoneNumber.MaximumLengthExceeded",
                    "Phone number is too long. The maximum number of characters is {MaxLength}.");

            public static ErrorCode InvalidFormat =>
                new("Validation.PhoneNumber.InvalidFormat",
                    "Invalid phone number format. The number may contain only digits, spaces, and the characters - ( ) +.");
        }
    }

    public static class Phrase
    {
        public static class Domain
        {
            public static ErrorCode NotFound => 
                new("Domain.Phrase.NotFound",
                    "Phrase not found.");
        }

        public static class Validation
        {
            public static ErrorCode AlreadyExists =>
                new("Validation.Phrase.AlreadyExists", 
                    "The phrase already exists in the database.");

            public static ErrorCode EmptyText =>
                new("Validation.Phrase.EmptyText", 
                    "Phrase text cannot be empty.");

            public static ErrorCode TextMaximumLengthExceeded =>
                new("Validation.Phrase.TextMaximumLengthExceeded",
                    "Phrase text is too long. The maximum number of characters is {MaxLength}.");

            public static ErrorCode NoteMaximumLengthExceeded =>
                new("Validation.Phrase.NoteMaximumLengthExceeded",
                    "Note is too long. The maximum number of characters is {MaxLength}.");

            public static ErrorCode CustomSerpStartingPointInvalidValue =>
                new("Validation.Phrase.CustomSerpStartingPointInvalidValue",
                    "The value must be between 1 and 300.");

            public static ErrorCode CustomSerpResultsLimitInvalidValue =>
                new("Validation.Phrase.CustomSerpResultsLimitInvalidValue",
                    "The value must be between 1 and 1000.");

            public static ErrorCode UserNotAssignedToPhrase =>
                new("Validation.Phrase.UserNotAssignedToPhrase",
                    "The user is not assigned to this phrase.");
        }
    }

    public static class Prospect
    {
        public static class Domain
        {
            public static ErrorCode NotFound =>
            new("Domain.Prospect.NotFound",
                "Prospect not found.");
            public static ErrorCode DomainOnBlackList =>
                new("Domain.Prospect.DomainOnBlackList", 
                    "The domain is on the blacklist.");
            public static ErrorCode ProspectDomainDuplicatedInPhraseScope =>
                new("Domain.Prospect.DomainDuplicatedInPhrase",
                    "The domain already exists in one of the phrase prospects.");


        }

        public static class Validation
        {
            public static ErrorCode ContactNameMaximumLengthExceeded =>
                new("Validation.Prospect.ContactNameMaximumLengthExceeded",
                    "Contact name is too long. The maximum number of characters is {MaxLength}.");

            public static ErrorCode NoteMaximumLengthExceeded =>
                new("Validation.Prospect.NoteMaximumLengthExceeded",
                    "Note is too long. The maximum number of characters is {MaxLength}.");

            public static ErrorCode PositionInGoogleRequired =>
                new("Validation.Prospect.PositionInGoogleRequired", 
                    "Google position is required.");

            public static ErrorCode PositionInGoogleInvalid =>
                new("Validation.Prospect.PositionInGoogleInvalid", 
                    "Invalid Google position value.");

        }
    }

    public static class Role
    {
        public static class Domain
        {
            public static ErrorCode AddingRoleFailed =>
                new("Domain.Role.AddingFailed", 
                    "Failed to add a new role.");

            public static ErrorCode NotFound =>
                new("Domain.Role.NotFound", 
                    "Role not found.");
        }

        public static class Validation
        {
            public static ErrorCode EmptyId =>
                new("Validation.Role.EmptyId", 
                    "Role ID cannot be empty.");

            public static ErrorCode EmptyName =>
                new("Validation.Role.EmptyName", 
                    "Role name cannot be empty.");

            public static ErrorCode NameMaximumLengthExceeded(int? maxLength) =>
                new("Validation.Role.NameMaximumLengthExceeded",
                    maxLength.HasValue ?
                    $"Role name is too long. The maximum number of characters is {maxLength}." : 
                    "Role name is too long. The maximum number of characters is {MaxLength}.");

            public static ErrorCode NameAlreadyExists =>
                new("Validation.Role.NameAlreadyExists",
                    "A role with the same name already exists.");
        }

    }

    public static class Url
    {
        public static readonly ErrorCode Empty =
            new("Validation.Url.Empty", 
                "URL cannot be empty.");

        public static readonly ErrorCode InvalidFormat =
            new("Validation.Url.InvalidFormat", 
                "Invalid URL format.");

        public static readonly ErrorCode MaximumLengthExceeded =
            new("Validation.Url.MaximumLengthExceeded",
                "URL is too long. The maximum number of characters is {MaxLength}.");
    }

    public static class User
    {
        public static class Domain
        {
            public static ErrorCode NotFound =>
                new("Domain.User.NotFound", 
                    "User not found.");

            public static ErrorCode BlockingFailed =>
                new("Domain.User.BlockingFailed", 
                    "Failed to block the user.");

            public static ErrorCode UserCreationFailed =>
                new("Domain.User.UserCreationFailed", 
                    "An error occurred while creating the user.");

            public static ErrorCode UserRoleGrantFailed =>
                new("Domain.User.RoleGrantFailed", 
                    "An error occurred while granting a role to the user.");

            public static ErrorCode UserRoleRevokeFailed =>
                new("Domain.User.RoleRevokeFailed", 
                    "An error occurred while revoking the user's role.");
        }

        public static class Validation
        {
            public static ErrorCode EmailAlreadyExists =>
                new("Validation.User.EmailAlreadyExists",
                    "A user with the specified email address already exists.");

            public static ErrorCode EmptyFirstName =>
                new("Validation.User.EmptyFirstName", "User first name cannot be empty.");

            public static ErrorCode FirstNameMaximumLengthExceeded =>
                new("Validation.User.FirstNameMaximumLengthExceeded",
                    "User first name is too long. The maximum number of characters is {MaxLength}.");

            public static ErrorCode FullNameAlreadyExists =>
                new("Validation.User.FullNameAlreadyExists",
                    "A user with the same first and last name already exists.");

            public static ErrorCode EmptyLastName =>
                new("Validation.User.EmptyLastName", "User last name cannot be empty.");

            public static ErrorCode LastNameMaximumLengthExceeded =>
                new("Validation.User.LastNameMaximumLengthExceeded",
                    "User last name is too long. The maximum number of characters is {MaxLength}.");
        }
    }

    public static class UserNotification
    {
        public static class Domain
        {
            public static ErrorCode NotFound =
                new("Validation.UserNotification.NotFound",
                    "User notification was not found in the database.");
        }
    }

    public static class UserPresence
    {
        public static class Domain
        {
            public static ErrorCode NotFound =
                new("UserPresence.NotFound",
                    "User presence log was not found in the database.");
        }
    }

}
