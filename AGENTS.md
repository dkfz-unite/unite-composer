# UNITE repository instructions

Before changing this repository, read the shared UNITE instructions and
relevant project documentation maintained in
[dkfz-unite/.github](https://github.com/dkfz-unite/.github/blob/main/README.md).

1. Prefer the local shared checkout: `../unite-meta/AGENTS.md`, relative to this
   repository root. A sibling `.github/AGENTS.md` or a user-provided path is also
   valid. Resolve its document links relative to that shared checkout.
2. Read the shared `AGENTS.md`, project overview, repository map, and topic
   documents relevant to the task, following the shared README's task index.
3. If no shared checkout is accessible, use the GitHub versions beginning with
   [shared AGENTS.md](https://github.com/dkfz-unite/.github/blob/main/AGENTS.md).
   If those are unavailable too, report the missing context and proceed only
   with what local documentation/code establishes; do not invent shared rules.
4. Read this repository's README and relevant local docs; inspect Git status.
   More specific instructions here supplement the shared guidance.

Keep common rules in `.github`; keep component details in this repository.

## Composer

Status: reviewed with the owner (2026-10-07). Defects are tracked in Jira; see
[Known issues](#known-issues).

Composer is the portal's read API for integrated data. It exposes search, single
entities, stats and TSV downloads over the Elasticsearch indexes; persists saved
datasets in MongoDB; and serves administrator submission review, task status and
project-user management. It does not ingest data (feeds) or run analyses
(`unite-analysis`). Shared background: [Search and access](../unite-meta/docs/search-and-access.md),
[Datasets and portal](../unite-meta/docs/datasets-and-portal.md), and the review
part of [Data ingestion](../unite-meta/docs/data-ingestion.md#feed-validation-and-data-review).

### Projects

| Project | Contents |
| --- | --- |
| `Unite.Composer.Web` | Startup, JWT/authorization setup, controllers, response `Resources/`, options from environment variables |
| `Unite.Composer` | Services: `Admin/` (submission review, task stats, data users), `Data/` (datasets, projects, submissions status, autocomplete, omics/specimen helpers), `Clients/` (Ensembl, Identity, donors feed), `Visualization/Lolliplot` |
| `Unite.Composer.Download` | TSV download services and column mapping (`Services/Tsv/Mapping`) for donors, images, specimens |

There are no test projects.

### Build and run

```sh
dotnet build Unite.Composer.sln
dotnet run --project Unite.Composer.Web --launch-profile Unite.Composer.Web
```

- Restore needs the `https://nuget.pkg.github.com/dkfz-unite/index.json` NuGet
  source with GitHub credentials in the developer's NuGet configuration; the
  repository has no `nuget.config`. The Dockerfile adds the source from build args.
- The launch profile serves `http://localhost:5002` and expects local
  PostgreSQL 5432, MongoDB 27017, Elasticsearch 9200, Ensembl Data 5200,
  Identity 5000 and the donors feed 5100.
- Docker: in the `unite-env` checkout, run `sh deploy.sh`, `sh build.remote.sh`
  or `sh build.local.sh` from `applications/unite-composer/`; see
  [Deployment and development](../unite-meta/docs/deployment-and-development.md).

### Configuration and API

Environment variables, Docker setup and access ports are in the
[README](README.md); the domain API is described in [Docs/api-domain.md](Docs/api-domain.md)
and its linked pages. Read those first. Developer-specific notes:

- Options read environment variables directly (`Configuration/Options`).
  `UNITE_API_KEY` must be exactly 32 characters.
- `UNITE_IDENTITY_HOST` and `UNITE_FEED_DONORS_HOST` are needed only by
  project-user management (user emails from Identity) and project reindexing
  (donors feed).

### Request handling pattern

- Controllers default to `[Authorize]`. Administrative endpoints use
  `[Authorize(Roles = Roles.Admin)]`, where the role value is `"Root"`; data
  changes use policy `Data.Writer` (permission claim `Data.Write`).
- Index-backed endpoints wrap the request in
  `new PersonalSearchCriteria(User.GetUserId(), User.GetIsRoot(), criteria)` and
  call `ISearchService<TIndex>`. Build the user scope only from token claims
  (`Extensions/PersonalizationExtensions.cs`), never from request data.
- `GetUserId()` reads `ClaimTypes.NameIdentifier`. Without it, the search library
  returns no accessible projects, so ordinary users see no project-scoped data.
- Bulk downloads first call `Stats` with personal criteria to obtain permitted
  IDs, then load SQL data in `Unite.Composer.Download`.
- `DefaultExceptionFilter` logs and returns a bare 500; invalid model state
  returns 400. JSON omits nulls and serializes enums by `EnumMember` value.

### Where to change things

| Change | Location |
| --- | --- |
| Search semantics, filters, criteria, index documents | `unite-indices` (`Unite.Indices.Search`), then bump the package here |
| Response shape of an entity | `Unite.Composer.Web/Resources/Domain/...` |
| New domain endpoint | `Controllers/Domain/<Type>/`, following the `<Type>Controller` (single) / `<Type>sController` (collection) split |
| Download options (data types) | `Unite.Composer.Download/Models/DataTypesCriteria.cs`, repositories, `Services/Tsv/DownloadService.cs`; portal `_shared/components/download/DownloadForm.vue` |
| TSV download columns | `Unite.Composer.Download/Services/Tsv/Mapping` |
| Genomic profile (data per genome bin) | `Unite.Composer/Data/Omics/Ranges` (`GenomicProfileService`, `GenomicRangesFilterService`), `SpecimenController` `{id}/profile`; portal `visualization/profile` |
| Submission review | `Unite.Composer/Admin/Services/SubmissionsService.cs` |
| Saved datasets | `Unite.Composer/Data/Datasets` and `Controllers/Data/Datasets` |
| Project users / visibility | `Unite.Composer/Data/Projects/ProjectService.cs`, `Controllers/Domain/Projects` |
| Protein mutation plot (lolliplot) | `Unite.Composer/Visualization/Lolliplot`, `Controllers/Visualization/LolliplotController.cs` |
| Specimen drug screening data | `Unite.Composer/Data/Specimens/DrugScreeningService.cs`, `SpecimenController` `{id}/drugs` |

The genomic profile, downloads and availability flags are described in
[Datasets and portal](../unite-meta/docs/datasets-and-portal.md#genomic-profile).
The lolliplot and drug screening visualizations are served by Composer, not by
`unite-analysis`. Dataset analyses (DEG, GAF, Scell, ...) belong to
`unite-analysis`; see [Analysis pipeline](../unite-meta/docs/analysis-pipeline.md).

When a change spans the portal, also update `unite/Unite.Web/Client` and verify
through the Vite UI.

### Shared libraries

`Unite.Composer` and `Unite.Composer.Download` both reference `Unite.Data.Context`;
keep their versions identical (they currently differ: 12.10.33 vs 12.10.32).
`Unite.Cache` is pinned at 1.0.3 (latest 1.3.0). Versions across consumers are
in the [Repository map](../unite-meta/docs/repository-map.md#package-versions-per-consumer).
Commented `ProjectReference` entries for `unite-data` and `unite-indices` support
local library development; do not commit them enabled (see
[Developing shared libraries](../unite-meta/docs/deployment-and-development.md#developing-shared-libraries)).
`Clients/JsonHttpClient.cs` duplicates the copy in `unite-feed-omics`; extraction
to `unite-essentials-http` is planned, not done.

### CI

`docker-images.yml` builds amd64 and arm64 images
(`ghcr.io/dkfz-unite/unite-composer`) under one manifest on pushes to `main`.

### Known issues

Defects are tracked in the private Jira, not here; see the shared
[Track defects](../unite-meta/AGENTS.md#track-defects). Check Jira before
changing dataset or project endpoints.

