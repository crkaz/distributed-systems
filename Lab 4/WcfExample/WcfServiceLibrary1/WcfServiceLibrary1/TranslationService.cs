using System.ServiceModel;

namespace WcfServiceLibrary1
{

    [ServiceContract] // Specifies that this interface is the Contract for the Service
    public interface ITranslationService
    {
        [OperationContract] // Specifies that this method is an Operation in the Contract
        string Translate(string input);
    }

    public class TranslationService : ITranslationService
    {
        public string Translate(string input)
        {
            return input + "eth";
        }
    }
}
