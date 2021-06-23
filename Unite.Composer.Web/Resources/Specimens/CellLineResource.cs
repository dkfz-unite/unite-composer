using System;
using Unite.Indices.Entities.Basic.Specimens;

namespace Unite.Composer.Web.Resources.Specimens
{
    public class CellLineResource
    {
        public string ReferenceId { get; set; }
        public string Species { get; set; }
        public string Type { get; set; }
        public string CultureType { get; set; }

        public string Name { get; set; }
        public string DepositorName { get; set; }
        public string DepositorEstablishment { get; set; }
        public DateTime? EstablishmentDate { get; set; }

        public string PubMedLink { get; set; }
        public string AtccLink { get; set; }
        public string ExPasyLink { get; set; }


        public CellLineResource(CellLineIndex index)
        {
            ReferenceId = index.ReferenceId;
            Species = index.Species;
            Type = index.Type;
            CultureType = index.CultureType;

            Name = index.Name;
            DepositorName = index.DepositorName;
            DepositorEstablishment = index.DepositorEstablishment;
            EstablishmentDate = index.EstablishmentDate;

            PubMedLink = index.PubMedLink;
            AtccLink = index.AtccLink;
            ExPasyLink = index.ExPasyLink;
        }
    }
}
