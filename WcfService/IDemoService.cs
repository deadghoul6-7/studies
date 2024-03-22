using CoreWCF;

namespace ProjectAspEShop2024.WcfService
{
    [ServiceContract]
    public interface IDemoService
    {
        [OperationContract]
        string GetInformation(string name);
    }

    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DemoService : IDemoService
    {
        private static Random rnd = new Random();

        public string GetInformation(string name)
        {
            int grad = rnd.Next(0, 30);
            return $"Привет, {name}, сейчас {grad} град.C";
        }
    }
}
