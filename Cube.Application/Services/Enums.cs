namespace Cube.Application.Services
{
    public enum GetAllUsers
    {
        Success
    }

    public enum GetUserFriends 
    { 
        Success,
        UserNotFound,
        ServerError
    }

    public enum GetChatMessagesResult 
    {
        Success,
        ChatNotFound,
        ServerError
    }

    public enum GetChatResult
    {
        Success,
        ChatNotFound,
        UserNotAMember
    }

    public enum GetAllChatsResult
    {
        Success,
        UserNotFound,
        DataBaseError
    }

    public enum CreateChatResult
    {
        Success,
        ParticipantsNotFound,
        ValidationError,
        WrongChatSettings
    }

    public enum DeleteChatResult
    {
        Success,
        ChatNotFound,
        UserNotFound,
    }

    public enum UpdateChatResult
    {
        Success,
        ChatNotFound,
        NothingToUpdate,
        ValidationError,
        DeletingParticipantsError,
        ParticipantsNotFound
    }

    public enum GetMessageResult
    {
        Success,
        MessageNotFound
    }

    public enum SendMessageResult
    {
        Success,
        ChatNotFound,
        UserNotFound,
        ValidationError,
        DataBaseError
    }

    public enum DeleteMessageResult
    {
        Success,
        MessageNotFound,
        


    }

    public enum UpdateMessageResult
    {
        Success,
        MessageNotFound,
        UserNotFound,
        NullOrEmptyNewMessage,
    }

    public enum LoginResult
    {
        Success,
        WrongLoginOrPassword
    }

    public enum RegisterResult
    {
        Success,
        RoleDoesntExist,
        EmailAlreadyExists,
        ValidationError,
        DataBaseError
    }

    public enum CreateUserResult 
    {
        Success,
        ValidationError,
        AccountNotFound,
        AccountLinkError,
        DateError,
        DataBaseError

    }

    public enum UpdateUserResult
    {
        Success,
        ValidationError,
        AccountNotFound,

    }
    public enum DeleteUserResult
    {
        Success,
        ValidationError,
        UserNotFound,

    }
    public enum GetUserResult
    {
        Success,
        ValidationError,
        UserNotFound
    }

    public enum CreateFriendshipResult
    {
        Success,
        Unsuccess,
        UserNotFound,
        FriendNotFound,

    }

    public enum CreateImageResult 
    {
        Success,
        OwnerNotFound,
        ImageAlreadyExist,
        CurruptedFile,
        ServerError
    }

    public enum GetImageResult
    {
        Success,
        OwnerNotFound,
        ImageFileNotFound,
        ServerError
    }

    public enum TokenValidationResult 
    { 
        Success,
        TimeExpired,
        IncorrectToken
    }

    public enum GetUserNotificationResult
    {
        Success,
        UserNotFound
    }

    public enum CreateNotificationResult
    {
        Success,
        NotificationSenderNotFound,
        UsersNotFound,
    }

    public enum DeleteNotificationResult
    {
        Success,
        NotificationNotFound
    }
}
