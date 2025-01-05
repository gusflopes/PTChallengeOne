using System.Collections.Concurrent;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace UnitTests.Fixtures;

public class EndpointAnalysisFixture : IDisposable
{
    private readonly string _srcPath;
    private readonly ConcurrentDictionary<string, SyntaxTree> _parsedFiles;
    private bool _disposed;
    
    public EndpointAnalysisFixture()
    {
        var webApiAssembly = typeof(WebAPI.Routes).Assembly;
        var webApiPath = webApiAssembly.Location;
        var projectPath = Path.GetDirectoryName(webApiPath);

        while (projectPath != null && !Directory.Exists(Path.Combine(projectPath, "src")))
        {
            projectPath = Directory.GetParent(projectPath)?.FullName;
        }
        if (projectPath == null)
            throw new Exception("Não foi possível localizar o diretório 'src' no caminho do projeto");
        
        _srcPath = Path.Combine(projectPath!, "src", "WebAPI");
        if (!Directory.Exists(_srcPath))
            throw new DirectoryNotFoundException($"Diretório do projeto WebAPI não encontrado em: {_srcPath}");
        
        _parsedFiles = new ConcurrentDictionary<string, SyntaxTree>();

        foreach (var file in Directory.GetFiles(_srcPath, "*.cs", SearchOption.AllDirectories))
        {
            var code = File.ReadAllText(file);
            _parsedFiles[file] = CSharpSyntaxTree.ParseText(code);
        }
    }

    public IEnumerable<(string FilePath, InvocationExpressionSyntax MethodCall, string FullExpression)> FindEndpoints(
        string methodName)
    {
        ThrowIfDisposed();
        
        foreach (var (file, tree) in _parsedFiles)
        {
            var root = tree.GetRoot();
            
            var methodCalls = root.DescendantNodes()
                .OfType<InvocationExpressionSyntax>()
                .Where(m => m.ToString().Contains(methodName));

            foreach (var methodCall in methodCalls)
            {
                var fullExpression = methodCall.Parent;
                while (fullExpression != null && fullExpression is InvocationExpressionSyntax)
                {
                    fullExpression = fullExpression.Parent;
                }
                
                yield return (
                    Path.GetFileName(file),
                    methodCall,
                    fullExpression?.ToString() ?? string.Empty
                );
            }
        }    
    }

    private void ThrowIfDisposed()
    {
        if (_disposed)
            throw new ObjectDisposedException(nameof(EndpointAnalysisFixture));
    }
    
    public virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _parsedFiles.Clear();
            }
            
            _disposed = true;
        }
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}

