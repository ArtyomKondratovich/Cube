namespace Cube.Application.Services
{
    public enum GetChatResult
    {
        Success,
        ChatNotFound
    }

    public enum GetAllChatsResult
    {
        Success,
        UserNotFound
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

    public enum SignInResult
    {
        Success,
        WrongLoginOrPassword
    }

    public enum SignUpResult
    {
        Success,
        EmailAlreadyExists,
        ValidationError
    }

    public class Enums
    {
    }
}
