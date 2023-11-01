using PySharpTelegram.Core.Services.Abstract;

namespace TestApp.Connector;

public class ExternalConnector : AbstractExternalConnector
{
    public ExternalConnector(string namespaceFromRoot) : base(typeof(ExternalConnector), namespaceFromRoot){}
}