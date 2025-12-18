namespace Build1.PostMVC.Unity.App.Modules.InternetReachability
{
    public interface IInternetReachabilityController
    {
        bool  IsChecking     { get; }
        bool? LastResult     { get; }
        long  LastResultCode { get; }

        void Configure(string url, int timeout);
        void Check();
    }
}