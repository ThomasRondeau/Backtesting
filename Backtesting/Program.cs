
namespace Backtesting
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            OllamaService ollama = new("llama3.2");
            var response = await ollama.GenerateResponse("Explique moi le concept de récursivité");

            Console.WriteLine(response);



        }
    }
}
