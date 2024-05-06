using CommandLine;

namespace SimpleKafkaSaslProducer;

public class Options
{
    [Option(Required = true, HelpText = "Adresse und Port des Kafka Brokers.")]
    public string? BootstrapServers { get; set; }

    [Option(Required = true, HelpText = " Benutzername für die SASL-Authentifizierung.")]
    public string? SaslUsername { get; set; }

    [Option(Required = true, HelpText = "Passwort für die SASL-Authentifizierung.")]
    public string? SaslPassword { get; set; }

    [Option(Required = true, HelpText = "Topic, dem der Consumer beitreten soll.")]
    public string Topic { get; set; } = string.Empty;
}