namespace Shop3.Utilities.Enum
{
    public enum ErrorCode
    {
        BAD_REQUEST = 9999,
        DUPLICATE_USER_NAME = 1001,
        REQUIRED_EMAIL = 1002,
        REQUIRED_LAST_NAME = 1003,
        REQUIRED_FIRST_NAME = 1004,
        REQUIRED_FULL_NAME = 1005,
        REQUIRED_PASS_WORD = 1006,
        LOGIN_FAILURE = 1007,
        REGISTER_FAILURE = 1008,
        GET_FAILURE = 1009
    }

    public enum SuccessCode
    {
        LOGIN_SUCCESS = 5000,
        REGISTER_SUCCESS = 5001
    }
}
