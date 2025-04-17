# Download Data Model
Specifies which connected data do we want to download for one or several entries.

>[!NOTE]
> Data download service will find all connected data for the given entries and return it in a single zip archive.

**`Donors`** - Download related donors data.
- Note: All types of entries eventially belong to donors. So donors data can always be downloaded.
- Type: _Boolean_
- Example: `true`

**`Clinical`** - Download related donors clinical data.
- Type: _Boolean_
- Example: `true`

**`Treatments`** - Download related donors treatments data.
- Type: _Boolean_
- Example: `true`

**`Specimens`** - Download related specimens data.
- Type: _Boolean_
- Example: `true`

**`Molecular`** - Download related specimens molecular data.
- Type: _Boolean_
- Example: `false`

**`Interventions`** - Download related specimens interventions data.
- Type: _Boolean_
- Example: `true`

**`Drugs`** - Download related specimens drugs screening data.
- Type: _Boolean_
- Example: `false`

**`Mrs`** - Download related MR Images data.
- Note: MR images are connected with donor derived materials only.
- Type: _Boolean_
- Example: `false`

**`Sms`** - Download related simple mutations data.
- Type: _Boolean_
- Example: `true`

**`SmsTranscriptsSlim`** - Include simple mutation affected gene names.
- Note: SMs data will include additional column with affected gene names.
- Type: _Boolean_
- Limitations: Available only if `Sms` is set to `true` and `SmsTranscriptsFull` is set to `false`.
- Example: `false`

**`SmsTranscriptsFull`** - Include simple mutation affected gene transcripts.
- Note: SMs data will include additional columns specifying variant affected transcripts. There can be multiple transcripts per variant, so final data size grows fast.
- Type: _Boolean_
- Limitations: Available only if `Sms` is set to `true` and `SmsTranscriptsSlim` is set to `false`.
- Example: `true`

**`Cnvs`** - Download related copy number variants data.
- Type: _Boolean_
- Example: `false`

**`CnvsTranscriptsSlim`** - Include copy number variant affected gene names.
- Note: CNVs data will include additional column with affected gene names.
- Type: _Boolean_
- Limitations: Available only if `Cnvs` is set to `true` and `CnvsTranscriptsFull` is set to `false`.
- Example: `false`

**`CnvsTranscriptsFull`** - Include copy number variant affected gene transcripts.
- Note: CNVs data will include additional columns specifying variant affected transcripts. There can be multiple transcripts per variant, so final data size grows fast.
- Type: _Boolean_
- Limitations: Available only if `Cnvs` is set to `true` and `CnvsTranscriptsSlim` is set to `false`.
- Example: `false`

**`Svs`** - Download related structural variants data.
- Type: _Boolean_
- Example: `false`

**`SvsTranscriptsSlim`** - Include structural variant affected gene names.
- Note: SVs data will include additional column with affected gene names.
- Type: _Boolean_
- Limitations: Available only if `Svs` is set to `true` and `SvsTranscriptsFull` is set to `false`.
- Example: `false`

**`SvsTranscriptsFull`** - Include structural variant affected gene transcripts.
- Note: SVs data will include additional columns specifying variant affected transcripts. There can be multiple transcripts per variant, so final data size grows fast.
- Type: _Boolean_
- Limitations: Available only if `Svs` is set to `true` and `SvsTranscriptsSlim` is set to `false`.
- Example: `false`

**`GeneExp`** - Download related bulk gene expression data.
- Type: _Boolean_
- Example: `true`
