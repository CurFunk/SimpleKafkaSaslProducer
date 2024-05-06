using System.Text;
using CommandLine;
using Confluent.Kafka;

namespace SimpleKafkaSaslProducer;

internal class Program
{
    private static async Task<int> Main(string[] args)
    {
        await Parser.Default.ParseArguments<Options>(args).WithParsedAsync(async opt =>
        {
            var cancellationTokenSource = new CancellationTokenSource();
            Console.CancelKeyPress += (sender, eventArgs) =>
            {
                Console.WriteLine("Kafka: Verbindung wird beendet.");
                // Bei einer Tastenunterbrechung (z.B. CTRL+C) die Cancellation durchführen
                eventArgs.Cancel = true; // Verhindert, dass das Programm beendet wird
                cancellationTokenSource.Cancel();
            };

            var configDictionary = new Dictionary<string, string>
            {
                { "bootstrap.servers", opt.BootstrapServers },
                { "security.protocol", "SASL_SSL" },
                { "sasl.mechanism", "PLAIN" },
                { "sasl.username", opt.SaslUsername },
                { "sasl.password", opt.SaslPassword },
                { "enable.idempotence", "True" }
            };

            var config = new ProducerConfig(configDictionary);

            var producer = new ProducerBuilder<string, byte[]>(config).Build();

            try
            {
                Console.WriteLine("Bitte Key eingeben");
                var key = Console.ReadLine();

                Console.WriteLine("Bitte Value eingeben");
                var value = Console.ReadLine();

                var message = new Message<string, byte[]>
                {
                    Key = key,
                    Value = Encoding.UTF8.GetBytes(value)
                };

                var report = await producer.ProduceAsync(opt.Topic, message, cancellationTokenSource.Token);
                //{
                Console.WriteLine($"Status: {report.Status}.");
                //});
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Environment.Exit(-1);
            }
        });
        Console.WriteLine("Anwendung wird beendet. Bitte beliebigen Knopf drücken.");

        Console.ReadKey();
        return 0;
    }
}