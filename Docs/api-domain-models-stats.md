# Statistics Data Model
Specifies which statistics data is available for one or several entries.

>[!NOTE]
> Statistics data fields depend on domain data type and can be different for different types of entries.

Domain data types:
- `Donors` and `Donor` - Data related to donors.
- `Images` and `Image` - Data related to images of different type.
- `Specimens` and `Specimen` - Data related to specimens of different type.
- `Genes` and `Gene` - Data related to genes.
- `Variants` and `Variant` - Data related to variants.

For example:
- For `Donors`:
    - Categories with DNA and RNA data like `Ssms` or `GeneExp` show, whether any specimen of the donor has this data available.
- For `Images`:
    <!-- - Categories other than image type are not available: `Cts` are not available for `Mris`. -->
    - Categories `Lines`, `Organoids`, `Xenografts` and related are not available, as image data is connected onle with donor derived materials.
    - Categories with DNA and RNA data like `Ssms` or `GeneExp` show, whether any image donor material has this data available.
- For `Specimens`:
    - Categories other than specimen type are not available: `Lines`, `Organoids`, `Xenografts` are not available for `Materials`.
- For `Genes`:
    - Category `Donors` shows whether any donor in his/her specimens has gene either expressed or affected by any variant.
    - Categories wth DNA data like `Ssms` or `Cnvs` show, whether any variant is affecting the gene in any specimen.
    - Categories with RNA data like `GeneExp` show gene expression levels in different specimens.
- For `Variants`:
    - Categories other than variant type are not available: `Cnvs`, `Svs` are not available for `Ssms`.
    - Category `Donors` shows whether any donor in his/her specimens has this variant.


## Fields
**`Total`** - Total number of entries of the given type.
- Type: _Integer_
- Example: `100`

**`Donors`** - Whether donor specific data is available.
- Note: Always available, since all types of entries eventually belong to donors.
- Type: _Boolean_
- Example: `true`

**`Clinical`** - Whether donor clinical data is available.
- Type: _Boolean_
- Example: `true`

**`Treatments`** - Whether donor treatments data is available.
- Type: _Boolean_
- Example: `true`

**`Mris`** - Whether MR images data is available.
- Type: _Boolean_
- Example: `true`

**`Materials`** - Whether donor derived materials data is available.
- Type: _Boolean_
- Example: `true`

**`MaterialsMolecular`** - Whether donor derived materials molecular data is available.
- Type: _Boolean_
- Example: `true`

**`Lines`** - Whether cell lines data is available.
- Type: _Boolean_
- Example: `true`

**`LinesMolecular`** - Whether cell lines molecular data is available.
- Type: _Boolean_
- Example: `true`

**`LinesDrugs`** - Whether cell lines drugs screening data is available.
- Type: _Boolean_
- Example: `true`

**`Organoids`** - Whether organoids data is available.
- Type: _Boolean_
- Example: `true`

**`OrganoidsMolecular`** - Whether organoids molecular data is available.
- Type: _Boolean_
- Example: `true`

**`OrganoidsDrugs`** - Whether organoids drugs screening data is available.
- Type: _Boolean_
- Example: `true`

**`OrganoidsInterventions`** - Whether organoids interventions data is available.
- Type: _Boolean_
- Example: `true`

**`Xenografts`** - Whether xenografts data is available.
- Type: _Boolean_
- Example: `true`

**`XenograftsMolecular`** - Whether xenografts molecular data is available.
- Type: _Boolean_
- Example: `true`

**`XenograftsDrugs`** - Whether xenografts drugs screening data is available.
- Type: _Boolean_
- Example: `true`

**`XenograftsInterventions`** - Whether xenografts interventions data is available.
- Type: _Boolean_
- Example: `true`

**`Ssms`** - Whether simple somatic mutations data is available.
- Type: _Boolean_
- Example: `true`

**`Cnvs`** - Whether copy number variants data is available.
- Type: _Boolean_
- Example: `true`

**`Svs`** - Whether structural variants data is available.
- Type: _Boolean_
- Example: `true`

**`GeneExp`** - Whether bulk gene expression data is available.
- Type: _Boolean_
- Example: `true`
