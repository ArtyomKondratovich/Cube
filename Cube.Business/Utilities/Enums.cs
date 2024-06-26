﻿namespace Cube.Business.Utilities
{
    public enum SendEmailResult
    {
        Success,
        DataBaseError,
        EmailSendingError,
        EmailExists
    }

    public enum EmailConfirmationResut
    {
        Success,
        ValidationError,
        EmailExists,
        TokenExpired

    }

    public enum CreateConfigResult
    {
        Success,
        UserNotFound,
        DatabaseError
    }
    public enum DeleteConfigResult
    {
        Success,
        UserNotFound,
        ConfigNotFound,
        DatabaseError
    }

    public enum GetConfigResult
    {
        Success,
        UserNotFound,
        ConfigNotFound
    }

    public enum UpdateConfigResult
    {
        Success,
        UserNotFound,
        ConfigNotFound,
        ValidationError,
        DatabaseError
    }

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
        ServerError,
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
        DatabaseError

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
        FriendshipAlreadyExist
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
