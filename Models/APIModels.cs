using System.Runtime.CompilerServices;

namespace Models
{

    public class TokenModel
    {
        public TokenModel()
        {
                
        }

        public TokenModel(string access, string refresh)
        {
            AccessToken = access;
            RefreshToken = refresh;
        }

        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }

    public class SuccessModel<T, I>
    {
        public SuccessModel()
        {

        }

        public SuccessModel(T main, I other)
        {
            MainData = main;
            OtherData = other;
        }


        public T MainData { get; set; }

        public I OtherData { get; set; }
    }

    public class SuccessModel<T>
    {
        public SuccessModel()
        {
            OtherData = null;
        }

        public SuccessModel(T main)
        {
            MainData = main;
            OtherData = null;
        }


        public T MainData { get; set; }

        public string OtherData { get; }
    }

    public class ErrorModel
    {
        public string Caption { get; set; }

        public string Message { get; set; }
    }
}
