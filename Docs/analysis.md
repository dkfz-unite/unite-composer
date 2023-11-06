# Analysis service

## General
Analysis consists of two parts:
- **Preparation** - preparation of the data, which has to be analysed
- **Execution** - analysis of prepared data

### Preparation
Preparation step requires access to the data. This means it requires data from either search engine (Elasticsearch) or the database (Postgresql). It can also require some data from external resources e.g from the internet, or all at once. Currently the search engine and the database are hosted at the same machine, so the machine must be powerful enough to handle requests from users and the analysis simultaneously.

### Execution
Analysis execution reuires access to machine resources and the data prepared during the preparison step. The more powerful is the machine where the analysis is running, the faster it works. Currently all analyses are performed on the same machine where the search engine works and the data is stored despite all services are running in a separate Docker container. So the analysis execution also enfluenses the user experience.

There are two options to perform the analysis:
- **Internally** - the analysis is performed by the application itself.
- **Externally** - the analysis is performed by an external application (e.g running on a separate computing cluster).

Preparation and execution steps are performed and controlled separately and asynchoronously.

### Internal analyses
Such analyses do not require external services to be performed (e.g. small interactive analyses). In this case **preparation** step can be omitted and the data is analysed directly. Internal analyses are:
- **Oncogrid** - analysis of the most frequently mutated genes in the cohort.
- **Protein analysis** - analysis of the protein domains and its mutations in a cohort.
- **DSS analysis** - analysis of the drug sensitivity score in a cohort.

### External analyses
Such analyses require complex setup and data prepared for them. In this case prepared data is stored in shared location. External analyses are:
- [Differential expression](https://github.com/dkfz-unite/unite-analysis-deseq2) - analysis performed by **DESeq2** algorithm.


## Configuration
To configure the application, change environment variables either in docker or in [launchSettings.json](../Unite.Composer.Web/Properties/launchSettings.json) file:

**`UNITE_ANALYSIS_DATA_PATH`*** - path to the data directory with files used for analysis.
- Usually a mounted image or directory.
- Default (**local**): `"~/mnt/analysis"`.
- Default (**docker**): `"/mnt/analysis"`.

**`UNITE_ANALYSIS_DESEQ2_URL`*** - url of the Deseq2 analysis service.
- Default (**local**): `"http://localhost:5300"`.
- Default (**docker**): `"http://deseq2.analysis.unite.net"`.
