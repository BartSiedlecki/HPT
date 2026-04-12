
namespace HPT.SharedKernel.Constants;

public static class FieldLengths
{
    public static class BlacklistedOrigin
    {
        public const int CustomReasonMaxLength = 100;
        public const int ReasonNameMaxLength = 100;
    }

    public static class CalendarEvent
    {
        public const int AssociatedNameMaxLength = 300;
        public const int TitleMaxLength = 200;
        public const int DescriptionMaxLength = 1000;
    }

    public static class CustomBlackListedReason
    {
        public const int TextMaxLength = 100;
    }

    public static class Email
    {
        public const int MaxLength = 256;
    }

    public static class FirstName
    {
        public const int MinLength = 2;
        public const int MaxLength = 50;
    }

    public static class LastName
    {
        public const int MinLength = 2;
        public const int MaxLength = 50;
    }

    public static class Login
    {
        public const int MaxLength = Email.MaxLength;
    }

    public class OutboxMessage
    {
        public const int EventTypeMaxLength = 256;
    }

    public static class Password
    {
        public const int MinLength = 8;
        public const int MaxLength = 100;
    }

    public static class Permission
    {
        public const int DescriptionMaxLength = 150;
    }

    public static class PermissionId
    {
        public const int MaxLength = 100;
    }

    public static class PhoneNumber
    {
        public const int MaxLength = 100;
    }

    public static class Phrase
    {
        public const int TextMaxLength = 200;        
        public const int NoteMaxLength = 300;        
    }

    public static class Prospect
    {
        public const int ContactNameMaxLength = 50;
        public const int NoteMaxLength = 4000;
    }

    public static class Role
    {
        public const int NameMaxLength = 256;
    }

    public static class Setting
    {
        public const int AreaMaxLength = 50;
        public const int KeyMaxLength = 100;
        public const int ValueMaxLength = 100;
        public const int DescriptionMaxLength = 300;
    }

    public static class Url
    {
        public const int MaxLength = 2000;
    }

    public static class User
    {
        public const int FirstNameMaxLength = 50;
        public const int LastNameMaxLength = 50;
    }
}
