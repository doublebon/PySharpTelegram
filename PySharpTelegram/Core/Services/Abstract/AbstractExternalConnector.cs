using System.Reflection;
using PySharpTelegram.Core.Attributes;
using PySharpTelegram.Core.Support;

namespace PySharpTelegram.Core.Services.Abstract;

public abstract class AbstractExternalConnector
{
    private readonly string _namespaceFromRoot;
    private readonly Type _rootExternalConnetorType;

    protected AbstractExternalConnector(Type rootExternalConnetorType, string namespaceFromRoot)
    {
        _namespaceFromRoot = namespaceFromRoot;
        _rootExternalConnetorType = rootExternalConnetorType;
    }

    public MethodInfo[] FindTelegramMethods(Type[] methodsWithAttributes)
    {
        var someType = _rootExternalConnetorType.Namespace?.Split('.')[0];
        var namespaceToAnalyze = $"{someType}.{_namespaceFromRoot}";
        var dllFiles = Directory.GetFiles(".", "*.dll", SearchOption.AllDirectories);
        // Load the assemblies
        var assemblies = dllFiles.Select(Assembly.LoadFrom).ToList();
        var types = assemblies
            .SelectMany(a => a.GetTypes())
            .Where(t => t.Namespace != null && t.Namespace.StartsWith(namespaceToAnalyze));
                
        var methods = new List<MethodInfo>();
        foreach (var type in types)
        {
            var typeMethods = type.GetMethods(BindingFlags.Static | BindingFlags.Public)
                .Where(method => methodsWithAttributes.Any(method.IsDefined));
            methods.AddRange(typeMethods);
        }
        
        return methods.OrderBy(m => m.GetCustomAttributes(typeof(MessageFilter.ByCommandAttribute), false).Length == 0).ToArray();
    }
}