using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam.Configuration.Options;
using Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam.Resources;

namespace Unite.Composer.Visualization.Lolliplot.Annotations.Clients.Pfam;

public class PfamApiClient
{
    private const string _sequenceSearchUrl = @"/search/sequence?output=xml";
    private const string _sequenceSearchResultsUrl = @"/search/sequence/resultset/{0}?output=xml";

    private readonly IPfamOptions _options;
    private readonly ILogger _logger;


    public PfamApiClient(IPfamOptions options)
    {
        _options = options;
        _logger = NullLogger<PfamApiClient>.Instance;
    }

    public PfamApiClient(IPfamOptions options, ILogger<PfamApiClient> logger)
    {
        _options = options;
        _logger = logger;
    }


    /// <summary>
    /// Performs search of protein domains for given sequence of amino acids.
    /// </summary>
    /// <param name="sequence">Protein sequence</param>
    /// <exception cref="ArgumentNullException">If sequence is null or empty.</exception>
    /// <exception cref="ArgumentException">If sequence length is logner that 10000 aa.</exception>
    /// <returns>Protein with protein domains.</returns>
    public async Task<Protein> Sequence(string sequence)
    {
        if (string.IsNullOrWhiteSpace(sequence))
        {
            throw new ArgumentNullException(nameof(sequence));
        }

        if (sequence.Length > 10000)
        {
            throw new ArgumentException("Maximum supported length of the sequence is 10000 aa.", nameof(sequence));
        }

        var job = await SequenceSearch(sequence);

        if (job == null)
        {
            return null;
        }

        var jobResult = await SequenceSearchResults(job.Id);

        if (jobResult == null)
        {
            return null;
        }

        return jobResult.Match.Protein;
    }

    /// <summary>
    /// Submits sequence search job to pfam.
    /// </summary>
    /// <param name="sequence">Protein sequence</param>
    /// <returns>Job.</returns>
    private async Task<Job> SequenceSearch(string sequence)
    {
        using var httpClient = new XmlHttpClient(_options.Host, true);

        var url = string.Format(_sequenceSearchUrl);

        var data = new KeyValuePair<string, string>[] {
            new KeyValuePair<string, string>("seq", sequence)
        };

        try
        {
            var xml = await httpClient.PostAsync(url, data);

            if (xml.StartsWith("<?xml"))
            {
                return Deserialize<SequenceSearchRequestResource>(xml)?.Job;
            }
            else
            {
                return null;
            }
        }
        catch (Exception exception)
        {
            _logger.LogWarning(exception.Message);

            return null;
        }
    }

    /// <summary>
    /// Requests sequence search results for given job.
    /// </summary>
    /// <param name="jobId">Job identifier</param>
    /// <returns>Job result.</returns>
    private async Task<JobResult> SequenceSearchResults(string jobId)
    {
        using var httpClient = new XmlHttpClient(_options.Host, true);

        var url = string.Format(_sequenceSearchResultsUrl, jobId);

        for (int i = 0; i < 5; i++)
        {
            try
            {
                var xml = await httpClient.GetAsync(url);

                if (xml.StartsWith("<?xml"))
                {
                    return Deserialize<SequenceSearchResultResource>(xml)?.Result;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            catch (Exception exception)
            {
                _logger.LogWarning(exception.Message);

                return null;
            }
        }

        return null;
    }


    private T Deserialize<T>(string xml)
    {
        try
        {
            using var stringReader = new StringReader(xml);
            var serializer = new XmlSerializer(typeof(T));
            var data = (T)serializer.Deserialize(stringReader);

            return data;
        }
        catch
        {
            return default(T);
        }
    }
}
